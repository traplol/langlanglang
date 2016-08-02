using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Langlanglang.Tokenization
{
    public class TokenParserContainer
    {
        private readonly List<TokenParser> _parsers;

        public TokenParserContainer()
        {
            _parsers = new List<TokenParser>();
        }

        public void AddParser(int priority, Tokenizer.TokenParserDelegate parser)
        {
            Add(new TokenParser(priority, parser));
        }

        public void AddParser(int priority, TokenType tokenType, Regex regex)
        {
            Add(new TokenParser(priority, tokenType, regex));
        }

        public void AddParser(TokenParser parser)
        {
            if (parser == null)
            {
                throw new ArgumentNullException("parser cannot be null");
            }
            _parsers.Add(parser);
        }

        private void Add(TokenParser parser)
        {
            _parsers.Add(parser);
            _parsers.Sort((p1, p2) => -p1.Priority.CompareTo(p2.Priority));
        }

        public Token ParseToken(string source, ref int index)
        {
            foreach (var parser in _parsers)
            {
                var token = parser.ParseToken(source, ref index);
                if (token != null)
                {
                    return token;
                }
            }
            return null;
        }
    }
}
