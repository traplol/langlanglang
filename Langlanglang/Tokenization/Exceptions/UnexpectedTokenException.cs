using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langlanglang.Tokenization.Exceptions
{
    public class UnexpectedTokenException : LllException
    {
        public TokenType Token { get; }
        public UnexpectedTokenException(TokenType token)
        {
            Token = token;
        }

        public UnexpectedTokenException(TokenType token, string message) 
            : base(message)
        {
            Token = token;
        }

        public UnexpectedTokenException(TokenType token, string message, Exception inner)
            : base(message, inner)
        {
            Token = token;
        }
    }
}
