using CIL;

namespace Langlanglang.Tokenization
{
    public enum TokenType
    {
        NoMatch,

        // OPERATORS
        Hash,               // #
        Assign,             // =
        Plus,               // +
        Minus,              // -
        Star,               // *
        Percent,            // %
        FSlash,             // /
        LParen,             // (
        RParen,             // )
        LSquBracket,        // [
        RSquBracket,        // ]
        LCurBracket,        // {
        RCurBracket,        // }
        LessThan,           // <
        GreaterThan,        // >
        LogicNot,           // !
        ArithNot,           // ~
        ArithOr,            // |
        ArithAnd,           // &
        ArithXor,           // ^
        Dot,                // .
        Colon,              // :
        Semicolon,          // ;
        Comma,              // ,
        RightArrow,         // ->
        LeftShift,          // <<
        RightShift,         // >>
        LogicEquals,        // ==
        LogicNotEquals,     // !=
        LogicOr,            // ||
        LogicAnd,           // &&
        LessThanEquals,     // <=
        GreaterThanEquals,  // >=
        DotDot,             // ..

        DotDotDot,          // ...

        // KEYWORDS
        Func,               // func
        Return,             // return
        Break,              // break
        Continue,           // continue
        If,                 // if
        Else,               // else
        Struct,             // struct
        Extend,             // extend
        Require,            // require
        Package,            // package
        While,              // while
        Foreach,            // foreach
        For,                // for
        In,                 // in
        New,                // new
        Delete,             // delete
        Foreign,            // foreign
        As,                 // as

        // OTHER
        Ident,              // [a-zA-Z_]+[a-zA-Z0-9_]*
        Number,             // [0-9]+(.[0-9]+)?
        String,             // '(.|\\')*'
    }
    public class Token
    {
        public TokenType Type { get; }
        public string StringValue { get; }
        public SourceInfo SourceInfo { get; set; }

        public Token(TokenType type, string stringValue, string fileName, int line, int column)
        {
            Type = type;
            StringValue = stringValue;
            SourceInfo = new SourceInfo(fileName, line, column);
        }

        public decimal ToDecimal()
        {
            return decimal.Parse(StringValue);
        }

        public bool IsIntegral()
        {
            return Type == TokenType.Number && ToDecimal()%1 == 0;
        }

        public string WithoutQuotes()
        {
            if (Type != TokenType.String)
            {
                return StringValue;
            }
            if (StringValue.Length < 2)
            {
                return StringValue;
            }
            if ((StringValue.StartsWith("'") && StringValue.EndsWith("'"))
                || (StringValue.StartsWith("\"") && StringValue.EndsWith("\"")))
            {
                return StringValue.Substring(1, StringValue.Length - 2);
            }
            return StringValue;
        }

        public override string ToString()
        {
            return string.Format("{0}:`{1}' {2}",
                Type,
                StringValue,
                SourceInfo);
        }
    }
}
