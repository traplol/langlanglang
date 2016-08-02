using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using CIL;
using Langlanglang.Tokenization.Exceptions;

namespace Langlanglang.Tokenization
{
    public class TokenStream
    {
        private int _idx;
        private readonly IList<Token> _tokens;
        public int Count => _tokens.Count;
        public int Index => _idx;
        public Token Current => Get(_idx);
        public SourceInfo SourceInfo => GetSourceInfo();

        public TokenStream(IList<Token> tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException("tokens");
            }
            _tokens = tokens;
            _idx = 0;
        }

        public Token Get(int i)
        {
            if (i >= _tokens.Count || i < 0)
            {
                return null;
            }
            return _tokens[i];
        }

        public Token Peek(int n = 0)
        {
            return Get(_idx + n);
        }

        public Token Accept(params TokenType[] types)
        {
            var tk = Peek();
            if (tk == null)
            {
                return null;
            }
            if (types.Contains(tk.Type))
            {
                ++_idx;
                return tk;
            }
            return null;
        }

        public Token Expect(TokenType type)
        {
            var tk = Accept(type);
            if (tk == null)
            {
                var thistk = Peek();
                var prevtk = Peek(-1);
                if (thistk == null)
                {
                    throw new UnexpectedTokenException(type, 
                        string.Format("Error: {0} - Expected:{1}, but got nothing!", prevtk.SourceInfo, type));
                }
                throw new UnexpectedTokenException(type, 
                    string.Format("Error: {0} - Expected:{1}, but got:{2}", prevtk.SourceInfo, type, thistk));
            }
            return tk;
        }

        public void ThrowIfTokenIsNot(TokenType type)
        {
            if (Current.Type != type)
            {
                throw new UnexpectedTokenException(type, string.Format("{0} : Expected {1}", SourceInfo, type));
            }
        }
        public void ThrowIfTokenIs(TokenType type)
        {
            if (Current.Type == type)
            {
                throw new UnexpectedTokenException(type, string.Format("{0} : Did not expect {1}", SourceInfo, type));
            }
        }
        public void ThrowIfTokenIsNotIn(params TokenType[] types)
        {
            if (!types.Contains(Current.Type))
            {
                throw new UnexpectedTokenException(Current.Type, string.Format("{0} : Did not expect {1}", SourceInfo, Current.Type));
            }
        }
        public void ThrowIfTokenIsIn(params TokenType[] types)
        {
            if (types.Contains(Current.Type))
            {
                throw new UnexpectedTokenException(Current.Type, string.Format("{0} : Did not expect {1}", SourceInfo, Current.Type));
            }
        }

        public Token Advance(int n=1)
        {
            var tk = Get(_idx);
            _idx += n;
            return tk;
        }

        public Token Rewind(int n=1)
        {
            return Advance(-n);
        }

        public void Restore(int i)
        {
            _idx = i;
        }

        private SourceInfo GetSourceInfo()
        {
            if (Current != null)
            {
                return Current.SourceInfo;
            }
            return new SourceInfo("<error>", -1, -1);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            int i = 0;
            while (true)
            {
                var tk = Peek(i);
                if (tk == null)
                {
                    break;
                }
                sb.Append(tk.StringValue + " ");
                i++;
            }
            return sb.ToString();
        }
    }
}
