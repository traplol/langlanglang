using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Langlanglang.Tokenization
{
    public class TokenParser
    {
        public bool IsRegexParser { get; }
        public int Priority { get; }
        private readonly Tokenizer.TokenParserDelegate _parser;
        private readonly Regex _regex;
        private readonly TokenType _tokenType;

        public TokenParser(int priority, Tokenizer.TokenParserDelegate parser)
        {
            if (parser == null)
            {
                throw new ArgumentNullException("parser cannot be null");
            }
            Priority = priority;
            IsRegexParser = false;
            _parser = parser;
        }

        public TokenParser(int priority, TokenType tokenType, Regex regex)
        {
            if (regex == null)
            {
                throw new ArgumentNullException("regex cannot be null");
            }
            Priority = priority;
            IsRegexParser = true;
            _regex = regex;
            _tokenType = tokenType;
        }

        public Token ParseToken(string source, ref int index)
        {
            if (IsRegexParser)
            {
                return ParseTokenRegex(source, ref index);
            }
            return _parser(source, ref index);
        }

        private Token ParseTokenRegex(string source, ref int index)
        {
            var matches = _regex.Match(source, index);
            if (!matches.Success)
            {
                return null;
            }
            var match = matches.Groups[1];
            index += match.Length;
            return new Token(_tokenType, match.Value, "test", -1, -1);
        }

    }
}
