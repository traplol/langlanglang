using System;
using Langlanglang.Parsing;
using Langlanglang.Tokenization;
using Langlanglang.Tokenization.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Langlanglang_Tests
{
    [TestClass]
    public class ParserMalformedTests
    {

        private readonly Tokenizer _tokenizer;

        public ParserMalformedTests()
        {
            _tokenizer = Tokenizer.CreateBasic();
        }

        private Ast ParseString(string source)
        {
            var tks = _tokenizer.Parse(source);
            var ts = new TokenStream(tks);
            return AstParser.Parse(ts);
        }

        [TestMethod]
        public void MalformedTestRequireSemicolon()
        {
            try
            {
                ParseString("x");
                Assert.Fail();
            }
            catch (UnexpectedTokenException ex)
            {
                Assert.AreEqual(ex.Token, TokenType.Semicolon);
            }
        }

        [TestMethod]
        public void MalformedTestIndexEmpty()
        {
            try
            {
                ParseString("a[];");
                Assert.Fail();
            }
            catch (UnexpectedTokenException)
            {
            }
        }

        [TestMethod]
        public void MalformedTestPackage1()
        {
            try
            {
                ParseString("package ;");
                Assert.Fail();
            }
            catch (UnexpectedTokenException ex)
            {
                Assert.AreEqual(TokenType.Ident, ex.Token);
            }
        }
        [TestMethod]
        public void MalformedTestPackage2()
        {
            try
            {
                ParseString("package .;");
                Assert.Fail();
            }
            catch (UnexpectedTokenException ex)
            {
                Assert.AreEqual(TokenType.Ident, ex.Token);
            }
        }
        [TestMethod]
        public void MalformedTestPackage3()
        {
            try
            {
                ParseString("package stdlib.;");
                Assert.Fail();
            }
            catch (UnexpectedTokenException ex)
            {
                Assert.AreEqual(TokenType.Semicolon, ex.Token);
            }
        }
        [TestMethod]
        public void MalformedTestPackage4()
        {
            try
            {
                ParseString("package .stdlib;");
                Assert.Fail();
            }
            catch (UnexpectedTokenException ex)
            {
                Assert.AreEqual(TokenType.Ident, ex.Token);
            }
        }
    }
}
