using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Langlanglang.Tokenization;
using Langlanglang.Tokenization.Exceptions;

namespace Langlanglang_Tests
{
    [TestClass]
    public class TokenizerTests
    {
        private readonly Tokenizer _tokenizer;
        public TokenizerTests()
        {
            _tokenizer = Tokenizer.CreateBasic();
        }

        [TestMethod]
        public void TokensTestNumber()
        {
            var tokens = _tokenizer.Parse(" 123 ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Number, tokens[0].Type);
            Assert.AreEqual("123", tokens[0].StringValue);

            tokens = _tokenizer.Parse(" 1233.45 ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Number, tokens[0].Type);
            Assert.AreEqual("1233.45", tokens[0].StringValue);

            tokens = _tokenizer.Parse(" .45 ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(2, tokens.Count); // ., 45

            tokens = _tokenizer.Parse(" 1.45.43 ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(3, tokens.Count); // 1.45, ., 43
        }

        [TestMethod]
        public void TokensTestIdentifier()
        {
            var tokens = _tokenizer.Parse(" testIdent ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Ident, tokens[0].Type);
            Assert.AreEqual("testIdent", tokens[0].StringValue);

            tokens = _tokenizer.Parse(" _ ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Ident, tokens[0].Type);
            Assert.AreEqual("_", tokens[0].StringValue);

            tokens = _tokenizer.Parse(" a ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Ident, tokens[0].Type);
            Assert.AreEqual("a", tokens[0].StringValue);

            tokens = _tokenizer.Parse(" _abc123 ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Ident, tokens[0].Type);
            Assert.AreEqual("_abc123", tokens[0].StringValue);

            tokens = _tokenizer.Parse(" abc123 ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Ident, tokens[0].Type);
            Assert.AreEqual("abc123", tokens[0].StringValue);

            tokens = _tokenizer.Parse(" _123 ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Ident, tokens[0].Type);
            Assert.AreEqual("_123", tokens[0].StringValue);
        }

        [TestMethod]
        public void TokensTestString()
        {
            var tokens = _tokenizer.Parse(" '_123' ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.String, tokens[0].Type);
            Assert.AreEqual("'_123'", tokens[0].StringValue);

            tokens = _tokenizer.Parse(" \"_123\" ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.String, tokens[0].Type);
            Assert.AreEqual("\"_123\"", tokens[0].StringValue);
        }

        [TestMethod]
        public void TokensTestFunc()
        {
            var tokens = _tokenizer.Parse(" func ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Func, tokens[0].Type);
            Assert.AreEqual("func", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestReturn()
        {
            var tokens = _tokenizer.Parse(" return ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Return, tokens[0].Type);
            Assert.AreEqual("return", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestContinue()
        {
            var tokens = _tokenizer.Parse(" continue ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Continue, tokens[0].Type);
            Assert.AreEqual("continue", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestBreak()
        {
            var tokens = _tokenizer.Parse(" break ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Break, tokens[0].Type);
            Assert.AreEqual("break", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestIf()
        {
            var tokens = _tokenizer.Parse(" if ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.If, tokens[0].Type);
            Assert.AreEqual("if", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestElse()
        {
            var tokens = _tokenizer.Parse(" else ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Else, tokens[0].Type);
            Assert.AreEqual("else", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestStruct()
        {
            var tokens = _tokenizer.Parse(" struct ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Struct, tokens[0].Type);
            Assert.AreEqual("struct", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestExtend()
        {
            var tokens = _tokenizer.Parse(" extend ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Extend, tokens[0].Type);
            Assert.AreEqual("extend", tokens[0].StringValue);
        }
        public void TokensTestPackage()
        {
            var tokens = _tokenizer.Parse(" package ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Package, tokens[0].Type);
            Assert.AreEqual("package", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestRequire()
        {
            var tokens = _tokenizer.Parse(" require ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Require, tokens[0].Type);
            Assert.AreEqual("require", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestWhile()
        {
            var tokens = _tokenizer.Parse(" while ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.While, tokens[0].Type);
            Assert.AreEqual("while", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestForeach()
        {
            var tokens = _tokenizer.Parse(" foreach ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Foreach, tokens[0].Type);
            Assert.AreEqual("foreach", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestFor()
        {
            var tokens = _tokenizer.Parse(" for ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.For, tokens[0].Type);
            Assert.AreEqual("for", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestNew()
        {
            var tokens = _tokenizer.Parse(" new ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.New, tokens[0].Type);
            Assert.AreEqual("new", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestDelete()
        {
            var tokens = _tokenizer.Parse(" delete ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Delete, tokens[0].Type);
            Assert.AreEqual("delete", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestIn()
        {
            var tokens = _tokenizer.Parse(" in ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.In, tokens[0].Type);
            Assert.AreEqual("in", tokens[0].StringValue);
        }

        [TestMethod]
        public void TokensTestAssign()
        {
            var tokens = _tokenizer.Parse(" = ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Assign, tokens[0].Type);
            Assert.AreEqual("=", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestPlus()
        {
            var tokens = _tokenizer.Parse(" + ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Plus, tokens[0].Type);
            Assert.AreEqual("+", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestMinus()
        {
            var tokens = _tokenizer.Parse(" - ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Minus, tokens[0].Type);
            Assert.AreEqual("-", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestStar()
        {
            var tokens = _tokenizer.Parse(" * ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Star, tokens[0].Type);
            Assert.AreEqual("*", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestFSlash()
        {
            var tokens = _tokenizer.Parse(" / ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.FSlash, tokens[0].Type);
            Assert.AreEqual("/", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestLParen()
        {
            var tokens = _tokenizer.Parse(" ( ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.LParen, tokens[0].Type);
            Assert.AreEqual("(", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestRParen()
        {
            var tokens = _tokenizer.Parse(" ) ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.RParen, tokens[0].Type);
            Assert.AreEqual(")", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestLSquBracket()
        {
            var tokens = _tokenizer.Parse(" [ ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.LSquBracket, tokens[0].Type);
            Assert.AreEqual("[", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestRSquBracket()
        {
            var tokens = _tokenizer.Parse(" ] ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.RSquBracket, tokens[0].Type);
            Assert.AreEqual("]", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestLCurBracket()
        {
            var tokens = _tokenizer.Parse(" { ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.LCurBracket, tokens[0].Type);
            Assert.AreEqual("{", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestRCurBracket()
        {
            var tokens = _tokenizer.Parse(" } ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.RCurBracket, tokens[0].Type);
            Assert.AreEqual("}", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestLeftShift()
        {
            var tokens = _tokenizer.Parse(" << ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.LeftShift, tokens[0].Type);
            Assert.AreEqual("<<", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestRightShift()
        {
            var tokens = _tokenizer.Parse(" >> ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.RightShift, tokens[0].Type);
            Assert.AreEqual(">>", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestLessThan()
        {
            var tokens = _tokenizer.Parse(" < ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.LessThan, tokens[0].Type);
            Assert.AreEqual("<", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestLessThanEquals()
        {
            var tokens = _tokenizer.Parse(" <= ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.LessThanEquals, tokens[0].Type);
            Assert.AreEqual("<=", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestGreaterThan()
        {
            var tokens = _tokenizer.Parse(" > ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.GreaterThan, tokens[0].Type);
            Assert.AreEqual(">", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestGreaterThanEquals()
        {
            var tokens = _tokenizer.Parse(" >= ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.GreaterThanEquals, tokens[0].Type);
            Assert.AreEqual(">=", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestLogicNot()
        {
            var tokens = _tokenizer.Parse(" ! ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.LogicNot, tokens[0].Type);
            Assert.AreEqual("!", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestLogicEquals()
        {
            var tokens = _tokenizer.Parse(" == ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.LogicEquals, tokens[0].Type);
            Assert.AreEqual("==", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestLogicNotEquals()
        {
            var tokens = _tokenizer.Parse(" != ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.LogicNotEquals, tokens[0].Type);
            Assert.AreEqual("!=", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestLogicOr()
        {
            var tokens = _tokenizer.Parse(" || ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.LogicOr, tokens[0].Type);
            Assert.AreEqual("||", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestLogicAnd()
        {
            var tokens = _tokenizer.Parse(" && ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.LogicAnd, tokens[0].Type);
            Assert.AreEqual("&&", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestArithNot()
        {
            var tokens = _tokenizer.Parse(" ~ ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.ArithNot, tokens[0].Type);
            Assert.AreEqual("~", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestArithOr()
        {
            var tokens = _tokenizer.Parse(" | ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.ArithOr, tokens[0].Type);
            Assert.AreEqual("|", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestArithAnd()
        {
            var tokens = _tokenizer.Parse(" & ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.ArithAnd, tokens[0].Type);
            Assert.AreEqual("&", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestArithXor()
        {
            var tokens = _tokenizer.Parse(" ^ ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.ArithXor, tokens[0].Type);
            Assert.AreEqual("^", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestDot()
        {
            var tokens = _tokenizer.Parse(" . ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Dot, tokens[0].Type);
            Assert.AreEqual(".", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestColon()
        {
            var tokens = _tokenizer.Parse(" : ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Colon, tokens[0].Type);
            Assert.AreEqual(":", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestSemicolon()
        {
            var tokens = _tokenizer.Parse(" ; ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Semicolon, tokens[0].Type);
            Assert.AreEqual(";", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestComma()
        {
            var tokens = _tokenizer.Parse(" , ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Comma, tokens[0].Type);
            Assert.AreEqual(",", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestRightArrow()
        {
            var tokens = _tokenizer.Parse(" -> ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.RightArrow, tokens[0].Type);
            Assert.AreEqual("->", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestDotDot()
        {
            var tokens = _tokenizer.Parse(" .. ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.DotDot, tokens[0].Type);
            Assert.AreEqual("..", tokens[0].StringValue);
        }

        [TestMethod]
        public void TokensTestOperationWithNoSpaces()
        {
            var tokens = _tokenizer.Parse("x=42");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(3, tokens.Count);

            tokens = _tokenizer.Parse("x=42*3+33/a");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(9, tokens.Count);
        }

        [TestMethod]
        public void TokensTestOperationWithSpaces()
        {
            var tokens = _tokenizer.Parse("x = 42");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(3, tokens.Count);

            tokens = _tokenizer.Parse("x = 42 * 3 + 33 / a");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(9, tokens.Count);
        }

        [TestMethod]
        public void TokensTestMultilineSource()
        {
            const string code = @"x
=
42

+
   5
";
            var tokens = _tokenizer.Parse(code);
            Assert.IsNotNull(tokens);
            Assert.AreEqual(5, tokens.Count);

            Assert.AreEqual(1, tokens[0].SourceInfo.Line);
            Assert.AreEqual(1, tokens[0].SourceInfo.Column);

            Assert.AreEqual(2, tokens[1].SourceInfo.Line);
            Assert.AreEqual(1, tokens[1].SourceInfo.Column);

            Assert.AreEqual(3, tokens[2].SourceInfo.Line);
            Assert.AreEqual(1, tokens[2].SourceInfo.Column);

            Assert.AreEqual(5, tokens[3].SourceInfo.Line);
            Assert.AreEqual(1, tokens[3].SourceInfo.Column);

            Assert.AreEqual(6, tokens[4].SourceInfo.Line);
            Assert.AreEqual(4, tokens[4].SourceInfo.Column);
        }

        [TestMethod]
        public void TokensTestIdent_int()
        {
            var tokens = _tokenizer.Parse(" int ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Ident, tokens[0].Type);
            Assert.AreEqual("int", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestIdent_double()
        {
            var tokens = _tokenizer.Parse(" double ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Ident, tokens[0].Type);
            Assert.AreEqual("double", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestIdent_void()
        {
            var tokens = _tokenizer.Parse(" void ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Ident, tokens[0].Type);
            Assert.AreEqual("void", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestIdent_Number()
        {
            var tokens = _tokenizer.Parse(" Number ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Ident, tokens[0].Type);
            Assert.AreEqual("Number", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestIdent_String()
        {
            var tokens = _tokenizer.Parse(" String ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Ident, tokens[0].Type);
            Assert.AreEqual("String", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestIdent_Object()
        {
            var tokens = _tokenizer.Parse(" Object ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Ident, tokens[0].Type);
            Assert.AreEqual("Object", tokens[0].StringValue);
        }
        [TestMethod]
        public void TokensTestIdent_Type()
        {
            var tokens = _tokenizer.Parse(" Type ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Ident, tokens[0].Type);
            Assert.AreEqual("Type", tokens[0].StringValue);
        }

        [TestMethod]
        public void TokensTestIdent_forth()
        {
            var tokens = _tokenizer.Parse(" forth ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Ident, tokens[0].Type);
            Assert.AreEqual("forth", tokens[0].StringValue);
        }

        [TestMethod]
        public void TokensTestIdent_required()
        {
            var tokens = _tokenizer.Parse(" required ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Ident, tokens[0].Type);
            Assert.AreEqual("required", tokens[0].StringValue);
        }

        [TestMethod]
        public void TokensTestForeign()
        {
            var tokens = _tokenizer.Parse(" foreign ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Foreign, tokens[0].Type);
            Assert.AreEqual("foreign", tokens[0].StringValue);
        }

        [TestMethod]
        public void TokensTestRef()
        {
            var tokens = _tokenizer.Parse(" ref ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Ref, tokens[0].Type);
            Assert.AreEqual("ref", tokens[0].StringValue);
        }

        [TestMethod]
        public void TokensTestDotDotDot()
        {
            var tokens = _tokenizer.Parse(" ... ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.DotDotDot, tokens[0].Type);
            Assert.AreEqual("...", tokens[0].StringValue);
        }

        [TestMethod]
        public void TokensTestPercent()
        {
            var tokens = _tokenizer.Parse(" % ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.Percent, tokens[0].Type);
            Assert.AreEqual("%", tokens[0].StringValue);
        }

        [TestMethod]
        public void TokensTestSingleLineComments()
        {
            var tokens = _tokenizer.Parse(" ... // 123");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(1, tokens.Count);
            Assert.AreEqual(TokenType.DotDotDot, tokens[0].Type);
            Assert.AreEqual("...", tokens[0].StringValue);
        }

        [TestMethod]
        public void TokensTestSectionComments()
        {
            var tokens = _tokenizer.Parse(" ... /* 123 */ 123");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(TokenType.DotDotDot, tokens[0].Type);
            Assert.AreEqual("...", tokens[0].StringValue);
            Assert.AreEqual(TokenType.Number, tokens[1].Type);
            Assert.AreEqual("123", tokens[1].StringValue);
        }


        [TestMethod]
        public void TokensTestMultilineComment()
        {
            var tokens = _tokenizer.Parse(
                @" ... /* 
123 
*/ 123");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(TokenType.DotDotDot, tokens[0].Type);
            Assert.AreEqual("...", tokens[0].StringValue);
            Assert.AreEqual(TokenType.Number, tokens[1].Type);
            Assert.AreEqual("123", tokens[1].StringValue);
        }

        [TestMethod]
        public void TokensTestGenericIdent()
        {
            var tokens = _tokenizer.Parse(" #T ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(TokenType.Hash, tokens[0].Type);
            Assert.AreEqual("#", tokens[0].StringValue);
            Assert.AreEqual(TokenType.Ident, tokens[1].Type);
            Assert.AreEqual("T", tokens[1].StringValue);
        }

        [TestMethod]
        public void TokensTestMinusOne()
        {
            var tokens = _tokenizer.Parse(" -1 ");
            Assert.IsNotNull(tokens);
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(TokenType.Minus, tokens[0].Type);
            Assert.AreEqual("-", tokens[0].StringValue);
            Assert.AreEqual(TokenType.Number, tokens[1].Type);
            Assert.AreEqual("1", tokens[1].StringValue);
        }
    }
}
