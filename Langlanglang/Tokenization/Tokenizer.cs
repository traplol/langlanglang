using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using CIL;
using Langlanglang.Tokenization.Exceptions;

namespace Langlanglang.Tokenization
{
    public class Tokenizer
    {
        public delegate Token TokenParserDelegate(string source, ref int index);

        public TokenParserContainer ParserContainer { get; }
        public List<Regex> IgnoreList { get; } 

        public string FilePath { get; set; }


        private Tokenizer()
        {
            ParserContainer = new TokenParserContainer();
            IgnoreList = new List<Regex>();
            FilePath = "<test>";
        }

        public IList<Token> Parse(string source)
        {
            var tokens = new List<Token>();
            var line = 1;
            var column = 1;
            for (int i = 0; i < source.Length;)
            {
                bool shouldContinue = false;
                foreach (var re in IgnoreList)
                {
                    var m = re.Match(source, i);
                    if (m.Success)
                    {
                        var n = m.Groups[0].Value.Count(c => c == '\n');
                        if (n > 0)
                        {
                            line += n;
                            column = 1;
                        }
                        i += m.Length;
                        shouldContinue = true;
                        break;
                    }
                }
                if (shouldContinue)
                {
                    continue;
                }
                var token = ParserContainer.ParseToken(source, ref i);
                if (token == null)
                {
                    throw new UnexpectedTokenException(TokenType.NoMatch, string.Format("Unexpected token: L{0}:C{1}", line, column));
                }
                tokens.Add(token);
                token.SourceInfo = new SourceInfo(FilePath, line, column);
            }
            return tokens;
        }

        public static Tokenizer CreateBasic()
        {
            var tokenizer = new Tokenizer();
            
            tokenizer.IgnoreList.Add(new Regex(@"\G[ \t\r\n]+"));
            tokenizer.IgnoreList.Add(new Regex(@"\G//.*"));
            tokenizer.IgnoreList.Add(new Regex(@"\G/\*[.\s\S\n\r]*?\*/"));

            tokenizer.ParserContainer.AddParser(0, TokenType.Ident, IdentRegex);

            tokenizer.ParserContainer.AddParser(1, TokenType.String, StringRegex);

            tokenizer.ParserContainer.AddParser(2, TokenType.Assign, AssignRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.Plus, PlusRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.Minus, MinusRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.Star, StarRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.FSlash, FSlashRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.LParen, LParenRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.RParen, RParenRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.LSquBracket, LSquBracketRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.RSquBracket, RSquBracketRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.LCurBracket, LCurBracketRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.RCurBracket, RCurBracketRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.LessThan, LessThanRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.GreaterThan, GreaterThanRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.LogicNot, LogicNotRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.ArithNot, ArithNotRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.ArithOr, ArithOrRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.ArithAnd, ArithAndRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.ArithXor, ArithXor);
            tokenizer.ParserContainer.AddParser(2, TokenType.Dot, DotRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.Colon, ColonRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.Semicolon, SemicolonRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.Comma, CommaRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.Percent, PercentRegex);
            tokenizer.ParserContainer.AddParser(2, TokenType.Hash, HashRegex);

            tokenizer.ParserContainer.AddParser(3, TokenType.LeftShift, LeftShiftRegex);
            tokenizer.ParserContainer.AddParser(3, TokenType.RightShift, RightShiftRegex);
            tokenizer.ParserContainer.AddParser(3, TokenType.LessThanEquals, LessThanEqualsRegex);
            tokenizer.ParserContainer.AddParser(3, TokenType.LogicEquals, LogicEqualsRegex);
            tokenizer.ParserContainer.AddParser(3, TokenType.LogicNotEquals, LogicNotEqualsRegex);
            tokenizer.ParserContainer.AddParser(3, TokenType.LogicOr, LogicOrRegex);
            tokenizer.ParserContainer.AddParser(3, TokenType.LogicAnd, LogicAndRegex);
            tokenizer.ParserContainer.AddParser(3, TokenType.GreaterThanEquals, GreaterThanEqualsRegex);
            tokenizer.ParserContainer.AddParser(3, TokenType.RightArrow, RightArrowRegex);
            tokenizer.ParserContainer.AddParser(3, TokenType.DotDot, DotDotRegex);

            tokenizer.ParserContainer.AddParser(4, TokenType.DotDotDot, DotDotDotRegex);

            tokenizer.ParserContainer.AddParser(9, TokenType.Func, FuncRegex);
            tokenizer.ParserContainer.AddParser(9, TokenType.Return, ReturnRegex);
            tokenizer.ParserContainer.AddParser(9, TokenType.Break, BreakRegex);
            tokenizer.ParserContainer.AddParser(9, TokenType.Continue, ContinueRegex);
            tokenizer.ParserContainer.AddParser(9, TokenType.If, IfRegex);
            tokenizer.ParserContainer.AddParser(9, TokenType.Else, ElseRegex);
            tokenizer.ParserContainer.AddParser(9, TokenType.Struct, StructRegex);
            tokenizer.ParserContainer.AddParser(9, TokenType.Extend, ExtendRegex);
            tokenizer.ParserContainer.AddParser(9, TokenType.Require, RequireRegex);
            tokenizer.ParserContainer.AddParser(9, TokenType.Package, PackageRegex);
            tokenizer.ParserContainer.AddParser(9, TokenType.While, WhileRegex);
            tokenizer.ParserContainer.AddParser(9, TokenType.For, ForRegex);
            tokenizer.ParserContainer.AddParser(9, TokenType.New, NewRegex);
            tokenizer.ParserContainer.AddParser(9, TokenType.Delete, DeleteRegex);
            tokenizer.ParserContainer.AddParser(9, TokenType.In, InRegex);
            tokenizer.ParserContainer.AddParser(9, TokenType.As, AsRegex);

            tokenizer.ParserContainer.AddParser(10, TokenType.Foreach, ForeachRegex);
            tokenizer.ParserContainer.AddParser(10, TokenType.Foreign, ForeignRegex);

            tokenizer.ParserContainer.AddParser(11, TokenType.Number, NumberRegex);
            return tokenizer;
        }

        //private const string Delim = @"[\s=\+\-\*/\(\)\[\]\{\}<>!\|&~\.:;%^]*?";
        private const string Delim = @"[\W]+";

        private static readonly Regex NumberRegex = new Regex(@"\G([0-9]+(\.[0-9]+)?)");
        private static readonly Regex IdentRegex = new Regex(@"\G([a-zA-Z_]+[a-zA-Z0-9_]*)");
        private static readonly Regex StringRegex = new Regex(@"\G(('[^']*')|(""[^""]*""))");

        // OPERATORS AND SYMBOLS
        private static readonly Regex AssignRegex = new Regex(@"\G(=)" );
        private static readonly Regex PlusRegex = new Regex(@"\G(\+)" );
        private static readonly Regex MinusRegex = new Regex(@"\G(\-)");
        private static readonly Regex StarRegex = new Regex(@"\G(\*)");
        private static readonly Regex FSlashRegex = new Regex(@"\G(/)");
        private static readonly Regex LParenRegex = new Regex(@"\G(\()");
        private static readonly Regex RParenRegex = new Regex(@"\G(\))");
        private static readonly Regex LSquBracketRegex = new Regex(@"\G(\[)");
        private static readonly Regex RSquBracketRegex = new Regex(@"\G(\])");
        private static readonly Regex LCurBracketRegex = new Regex(@"\G(\{)");
        private static readonly Regex RCurBracketRegex = new Regex(@"\G(\})");
        private static readonly Regex LessThanRegex = new Regex(@"\G(<)");
        private static readonly Regex GreaterThanRegex = new Regex(@"\G(>)");
        private static readonly Regex LogicNotRegex = new Regex(@"\G(!)");
        private static readonly Regex ArithNotRegex = new Regex(@"\G(~)");
        private static readonly Regex ArithOrRegex = new Regex(@"\G(\|)");
        private static readonly Regex ArithAndRegex = new Regex(@"\G(&)");
        private static readonly Regex ArithXor = new Regex(@"\G(\^)");
        private static readonly Regex DotRegex = new Regex(@"\G(\.)");
        private static readonly Regex ColonRegex = new Regex(@"\G(:)");
        private static readonly Regex SemicolonRegex = new Regex(@"\G(;)");
        private static readonly Regex CommaRegex = new Regex(@"\G(,)");
        private static readonly Regex PercentRegex = new Regex(@"\G(%)");
        private static readonly Regex HashRegex = new Regex(@"\G(#)");

        private static readonly Regex LeftShiftRegex = new Regex(@"\G(<<)");
        private static readonly Regex RightShiftRegex = new Regex(@"\G(>>)");
        private static readonly Regex RightArrowRegex = new Regex(@"\G(->)");
        private static readonly Regex LessThanEqualsRegex = new Regex(@"\G(<=)");
        private static readonly Regex GreaterThanEqualsRegex = new Regex(@"\G(>=)");
        private static readonly Regex LogicNotEqualsRegex = new Regex(@"\G(!=)");
        private static readonly Regex LogicEqualsRegex = new Regex(@"\G(==)");
        private static readonly Regex LogicOrRegex = new Regex(@"\G(\|\|)");
        private static readonly Regex LogicAndRegex = new Regex(@"\G(&&)");
        private static readonly Regex DotDotRegex = new Regex(@"\G(\.\.)");

        private static readonly Regex DotDotDotRegex = new Regex(@"\G(\.\.\.)");

        // KEYWORDS
        private static readonly Regex FuncRegex = new Regex(@"\G(func)" + Delim);
        private static readonly Regex BreakRegex = new Regex(@"\G(break)" + Delim);
        private static readonly Regex ContinueRegex = new Regex(@"\G(continue)" + Delim);
        private static readonly Regex ReturnRegex = new Regex(@"\G(return)" + Delim);
        private static readonly Regex IfRegex = new Regex(@"\G(if)" + Delim);
        private static readonly Regex ElseRegex = new Regex(@"\G(else)" + Delim);
        private static readonly Regex StructRegex = new Regex(@"\G(struct)" + Delim);
        private static readonly Regex ExtendRegex = new Regex(@"\G(extend)" + Delim);
        private static readonly Regex RequireRegex = new Regex(@"\G(require)" + Delim);
        private static readonly Regex PackageRegex = new Regex(@"\G(package)" + Delim);
        private static readonly Regex WhileRegex = new Regex(@"\G(while)" + Delim);
        private static readonly Regex ForeachRegex = new Regex(@"\G(foreach)" + Delim);
        private static readonly Regex ForRegex = new Regex(@"\G(for)" + Delim);
        private static readonly Regex NewRegex = new Regex(@"\G(new)" + Delim);
        private static readonly Regex DeleteRegex = new Regex(@"\G(delete)" + Delim);
        private static readonly Regex InRegex = new Regex(@"\G(in)" + Delim);
        private static readonly Regex ForeignRegex = new Regex(@"\G(foreign)" + Delim);
        private static readonly Regex AsRegex = new Regex(@"\G(as)" + Delim);

    }
}
