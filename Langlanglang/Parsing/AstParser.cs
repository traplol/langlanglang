using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Langlanglang.Parsing.AstNodes;
using Langlanglang.Tokenization;
using Langlanglang.Tokenization.Exceptions;

namespace Langlanglang.Parsing
{
    public static class AstParser
    {
        private static readonly TokenType[] UnaryOps =
        {
            TokenType.Plus, TokenType.Minus,
            TokenType.ArithNot,
            TokenType.LogicNot,
            TokenType.New, TokenType.Delete, 
        };

        public class OpInfo
        {
            public enum Associativity{ LtoR, RtoL }
            public Associativity Assoc { get; }
            public int Prec { get; }
            private OpInfo(Associativity assoc, int prec)
            {
                Assoc = assoc;
                Prec = prec;
            }

            public bool IsLeftAssoc()
            {
                return Assoc == Associativity.LtoR;
            }
            public bool IsRightAssoc()
            {
                return !IsLeftAssoc();
            }

            public static OpInfo LtoR(int prec) { return new OpInfo(Associativity.LtoR, prec);}
            public static OpInfo RtoL(int prec) { return new OpInfo(Associativity.RtoL, prec);}
        }

        private static readonly Dictionary<TokenType, OpInfo> BinaryOpPrec = new Dictionary<TokenType, OpInfo>
        {
//            { TokenType.Assign, OpInfo.RtoL(30) },

            { TokenType.LogicOr, OpInfo.LtoR(50) },
            { TokenType.LogicAnd, OpInfo.LtoR(60) },

            { TokenType.ArithOr, OpInfo.LtoR(70) },
            { TokenType.ArithXor, OpInfo.LtoR(80) },
            { TokenType.ArithAnd, OpInfo.LtoR(90) },

            { TokenType.LogicEquals, OpInfo.LtoR(100) },
            { TokenType.LogicNotEquals, OpInfo.LtoR(100) },

            { TokenType.LessThan, OpInfo.LtoR(110) },
            { TokenType.LessThanEquals, OpInfo.LtoR(110) },
            { TokenType.GreaterThan, OpInfo.LtoR(110) },
            { TokenType.GreaterThanEquals, OpInfo.LtoR(110) },

            { TokenType.LeftShift, OpInfo.LtoR(115) },
            { TokenType.RightShift, OpInfo.LtoR(115) },

            { TokenType.Plus, OpInfo.LtoR(120) },
            { TokenType.Minus, OpInfo.LtoR(120) },

            { TokenType.Percent, OpInfo.LtoR(130) },
            { TokenType.Star, OpInfo.LtoR(130) },
            { TokenType.FSlash, OpInfo.LtoR(130) },

            { TokenType.Dot, OpInfo.LtoR(200) }
        };

        private static bool IsBinOp(Token tk)
        {
            return tk != null && BinaryOpPrec.ContainsKey(tk.Type);
        }
        private static OpInfo GetBinOpInfo(Token tk)
        {
            if (IsBinOp(tk))
            {
                return BinaryOpPrec[tk.Type];
            }
            return null;
        }
        private static int GetBinOpPrec(Token tk)
        {
            return GetBinOpInfo(tk)?.Prec ?? int.MinValue;
        }
        private static bool IsBinOpLeftAssoc(Token tk)
        {
            var info = GetBinOpInfo(tk);
            return info != null && info.IsLeftAssoc();
        }
        private static bool IsBinOpRightAssoc(Token tk)
        {
            return !IsBinOpLeftAssoc(tk);
        }

        public static Ast Parse(TokenStream tokens)
        {
            var ast = new Ast();
            while (tokens.Peek() != null)
            {
                var tk = tokens.Peek();
                var root = ParseRoot(tokens);
                if (root == null)
                {
                    throw new UnexpectedTokenException(
                        tk.Type, 
                        string.Format("Ast generation failed: L{0}:C{1}", tk.SourceInfo.Line, tk.SourceInfo.Column));
                }
                ast.AddRoot(root);
            }
            return ast;
        }

        private static AstNode ParseRoot(TokenStream tks)
        {
            switch (tks.Peek().Type)
            {
                case TokenType.If:
                    return ParseIf(tks);
                case TokenType.Require:
                    return ParseRequire(tks);
                case TokenType.Package:
                    return ParsePackage(tks);
                case TokenType.While:
                    return ParseWhile(tks);
                case TokenType.Foreach:
                    return ParseForeach(tks);
                case TokenType.For:
                    return ParseFor(tks);
                case TokenType.Func:
                    return ParseFunc(tks);
                case TokenType.Struct:
                    return ParseStruct(tks);
                case TokenType.Extend:
                    return ParseExtend(tks);
                case TokenType.Foreign:
                    return ParseForeign(tks);
                default:
                    var expr = ParseDeclOrExpr(tks);
                    if (expr != null)
                    {
                        tks.Expect(TokenType.Semicolon);
                    }
                    return expr;
            }
        }

        private static AstNode ParseDeclOrExpr(TokenStream tks)
        {
            var save = tks.Index;
            AstNode node = ParseVariableDeclaration(tks);
            if (node != null)
            {
                return node;
            }
            tks.Restore(save);
            node = ParseExpression(tks);
            return node;
        }

        // <if-stmt>        ::= 'if' <assign> '{' <body> '}'
        //                  ::= 'if' <assign> '{' <body> '}' 'else' '{' <body> '}'
        //                  ::= 'if' <assign> '{' <body> '}' 'else' <if-stmt>
        private static AstIf ParseIf(TokenStream tks)
        {
            var si = tks.SourceInfo;
            tks.Expect(TokenType.If);
            var cond = ParseAssign(tks);
            tks.Expect(TokenType.LCurBracket);
            var trueBody = ParseBody(tks);
            tks.Expect(TokenType.RCurBracket);
            var falseBody = new List<AstNode>();
            if (tks.Accept(TokenType.Else) != null)
            {
                if (tks.Accept(TokenType.LCurBracket) != null)
                {
                    falseBody = ParseBody(tks);
                    tks.Expect(TokenType.RCurBracket);
                }
                else
                {
                    tks.Expect(TokenType.If);
                    tks.Rewind();
                    falseBody.Add(ParseIf(tks));
                }
            }
            return new AstIf(si, cond, trueBody, falseBody);
        }

        // <package-ident>  ::= <ident> ( '.' <ident> )*
        private static AstPackageIdent ParsePackageIdent(TokenStream tks)
        {
            var si = tks.SourceInfo;
            var idents = new List<string>();
            while (true)
            {
                var ident = tks.Expect(TokenType.Ident).StringValue;
                idents.Add(ident);
                if (tks.Peek().Type == TokenType.Dot && tks.Peek(1).Type == TokenType.Ident)
                {
                    tks.Advance();
                    continue;
                }
                break;
            }
            return new AstPackageIdent(si, idents);
        }

        // <require>        ::= 'require' <package-ident> ';'
        //                  |   'require' <package-ident> '.' '*' ';'
        private static AstRequire ParseRequire(TokenStream tks)
        {
            var si = tks.SourceInfo;
            tks.Expect(TokenType.Require);
            var idents = ParsePackageIdent(tks);
            bool usesStar = false;
            if (tks.Accept(TokenType.Dot) != null)
            {
                tks.Expect(TokenType.Star);
                usesStar = true;
            }
            tks.Expect(TokenType.Semicolon);
            return new AstRequire(si, idents, usesStar);
        }

        // <package>        ::= 'package' <package-ident> ';'
        private static AstPackage ParsePackage(TokenStream tks)
        {
            var si = tks.SourceInfo;
            tks.Expect(TokenType.Package);
            var package = ParsePackageIdent(tks);
            tks.Expect(TokenType.Semicolon);
            return new AstPackage(si, package);
        }

        // <while-stmt>     ::= 'while' <assign> '{' <body> '}'
        private static AstWhile ParseWhile(TokenStream tks)
        {
            var si = tks.SourceInfo;
            tks.Expect(TokenType.While);
            var condition = ParseAssign(tks);
            tks.Expect(TokenType.LCurBracket);
            var body = ParseBody(tks);
            tks.Expect(TokenType.RCurBracket);
            return new AstWhile(si, condition, body);
        }

        // <foreach-stmt>   ::= 'foreach' <ident> 'in' <expr> '{' <body> '}'
        private static AstForeach ParseForeach(TokenStream tks)
        {
            var si = tks.SourceInfo;
            tks.Expect(TokenType.Foreach);
            var ident = tks.Expect(TokenType.Ident).StringValue;
            tks.Expect(TokenType.In);
            var collection = ParseAssign(tks);
            tks.Expect(TokenType.LCurBracket);
            var body = ParseBody(tks);
            tks.Expect(TokenType.RCurBracket);
            return new AstForeach(si, ident, collection, body);
        }

        private static AstFor ParseForIn(TokenStream tks)
        {
            var si = tks.SourceInfo;
            tks.Expect(TokenType.For);
            var ident = tks.Expect(TokenType.Ident).StringValue;
            tks.Expect(TokenType.In);
            var @from = ParseExpression(tks);
            tks.Expect(TokenType.DotDot);
            var to = ParseExpression(tks);
            var opTk = tks.Accept(TokenType.Plus, TokenType.Minus);
            var updateOp = opTk?.Type ?? TokenType.Plus;
            tks.Expect(TokenType.LCurBracket);
            var body = ParseBody(tks);
            tks.Expect(TokenType.RCurBracket);

            var pre = new AstDeclaration(si, ident, null, 0, @from);
            var condOp = (updateOp == TokenType.Plus)
                    ? TokenType.LessThanEquals 
                    : TokenType.GreaterThanEquals;
            var itrId = new AstIdent(si, ident);
            var cond = new AstBinaryOp(si, itrId, condOp, to);
            var update_ = new AstBinaryOp(si, itrId, updateOp, new AstNumber(si, 1));
            var update = new AstAssign(si, itrId, update_);

            return new AstFor(si, new List<AstNode> {pre}, cond, new List<AstExpression> {update}, body);
        }

        // <for-stmt>   ::= 'for' <for-pre-list> ';' <expr-list> ';' <expr-list> '{' <body> '}'
        //              |   'for' <ident> 'in' <number> '..' <number> '{' <body> '}'
        private static AstFor ParseFor(TokenStream tks)
        {
            // Look ahead and see if this is a for..in.. loop instead
            if (tks.Peek(2).Type == TokenType.In)
            {
                return ParseForIn(tks);
            }
            var si = tks.SourceInfo;
            tks.Expect(TokenType.For);
            var preList = ParseForPreList(tks);
            tks.Expect(TokenType.Semicolon);
            var condition = ParseExpression(tks);
            tks.Expect(TokenType.Semicolon);
            var update = ParseExpressionList(tks);
            tks.Expect(TokenType.LCurBracket);
            var body = ParseBody(tks);
            tks.Expect(TokenType.RCurBracket);
            return new AstFor(si, preList, condition, update, body);
        }

        private static List<AstNode> ParseForPreList(TokenStream tks)
        {
            var list = new List<AstNode>();
            while (true)
            {
                var expr = ParseDeclOrExpr(tks);
                if (expr != null)
                {
                    list.Add(expr);
                    if (tks.Accept(TokenType.Comma) != null)
                    {
                        continue;
                    }
                }
                break;
            }
            return list;
        } 

        private static List<AstExpression> ParseAssignList(TokenStream tks)
        {
            var list = new List<AstExpression>();
            while (true)
            {
                var assign = ParseAssign(tks);
                if (assign != null)
                {
                    list.Add(assign);
                    if (tks.Accept(TokenType.Comma) == null)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            return list;
        } 

        private static AstReturn ParseReturn(TokenStream tks)
        {
            var si = tks.SourceInfo;
            tks.Expect(TokenType.Return);
            var expr = ParseAssign(tks);
            tks.Expect(TokenType.Semicolon);
            return new AstReturn(si, expr);
        }
        // <continue>   ::= 'continue'
        private static AstContinue ParseContinue(TokenStream tks)
        {
            throw new NotImplementedException();
        }
        // <break>      ::= 'break'
        private static AstBreak ParseBreak(TokenStream tks)
        {
            throw new NotImplementedException();
        }

        // <foreign>            ::= 'foreign' 'func' <string> '(' <func-param-list> ')' ';'
        //                      |   'foreign' 'func' <string> '(' <func-param-list> ')' -> <ident> ';'
        private static AstForeign ParseForeign(TokenStream tks)
        {
            var si = tks.SourceInfo;
            tks.Expect(TokenType.Foreign);
            tks.Expect(TokenType.Func);
            var name = tks.Expect(TokenType.Ident).StringValue;
            var cname = name;
            if (tks.Accept(TokenType.As) != null)
            {
                cname = tks.Accept(TokenType.String).WithoutQuotes();
            }
            tks.Expect(TokenType.LParen);
            var paramList = ParseFuncParamList(tks);
            bool isVarArgs = tks.Accept(TokenType.DotDotDot) != null;
            tks.Expect(TokenType.RParen);

            string returnType = "void";
            var ptrDepth = 0;
            if (tks.Accept(TokenType.RightArrow) != null)
            {
                returnType = tks.Expect(TokenType.Ident).StringValue;
                while (tks.Accept(TokenType.Star) != null)
                {
                    ptrDepth++;
                }
            }

            tks.Expect(TokenType.Semicolon);

            return new AstForeign(si, paramList, name, cname, returnType, ptrDepth, isVarArgs);
        }

        // <func>               ::= 'func' <ident> '(' <func-param-list> ')' '{' <body> '}'
        //                      |   'func' <ident> '(' <func-param-list> ')' -> <ident> '{' <body> '}'
        private static AstFunc ParseFunc(TokenStream tks)
        {
            var si = tks.SourceInfo;
            tks.Expect(TokenType.Func);
            var ident = tks.Expect(TokenType.Ident);
            tks.Expect(TokenType.LParen);
            var paramList = ParseFuncParamList(tks);
            tks.Expect(TokenType.RParen);

            string returnType = "void";
            var ptrDepth = 0;
            if (tks.Accept(TokenType.RightArrow) != null)
            {
                returnType = tks.Expect(TokenType.Ident).StringValue;
                while (tks.Accept(TokenType.Star) != null)
                {
                    ptrDepth++;
                }
            }

            tks.Expect(TokenType.LCurBracket);
            var body = ParseBody(tks);
            tks.Expect(TokenType.RCurBracket);
            return new AstFunc(si, ident.StringValue, paramList, returnType, ptrDepth, body);
        }

        private static List<AstDeclaration> ParseFuncParamList(TokenStream tks)
        {
            var si = tks.SourceInfo;
            var parameters = new List<AstDeclaration>();
            while (true)
            {
                var ident = tks.Accept(TokenType.Ident);
                if (ident != null)
                {
                    tks.Expect(TokenType.Colon);
                    var isGeneric = tks.Accept(TokenType.Hash) != null;
                    var type = tks.Expect(TokenType.Ident).StringValue;
                    int ptrDepth = 0;
                    while (tks.Accept(TokenType.Star) != null)
                    {
                        ptrDepth++;
                    }
                    parameters.Add(new AstDeclaration(si, ident.StringValue, type, ptrDepth, null, isGeneric));
                    if (tks.Accept(TokenType.Comma) != null)
                    {
                        continue;
                    }
                }
                break;
            }
            return parameters;
        } 
        private static List<AstNode> ParseBody(TokenStream tks)
        {
            var body = new List<AstNode>();
            while (true)
            {
                switch (tks.Peek().Type)
                {
                    case TokenType.If:
                        body.Add(ParseIf(tks));
                        continue;
                    case TokenType.While:
                        body.Add(ParseWhile(tks));
                        continue;
                    case TokenType.Foreach:
                        body.Add(ParseForeach(tks));
                        continue;
                    case TokenType.For:
                        body.Add(ParseFor(tks));
                        continue;
                    case TokenType.Return:
                        body.Add(ParseReturn(tks));
                        continue;
                }
                var expr = ParseDeclOrExpr(tks);
                if (expr == null)
                {
                    break;
                }
                tks.Expect(TokenType.Semicolon);
                body.Add(expr);
            }
            return body;
        } 

        private static AstStruct ParseStruct(TokenStream tks)
        {
            var si = tks.SourceInfo;
            tks.Expect(TokenType.Struct);
            var ident = tks.Expect(TokenType.Ident).StringValue;
            tks.Expect(TokenType.LCurBracket);
            var members = new List<AstDeclaration>();
            while (tks.Accept(TokenType.RCurBracket) == null)
            {
                var memberId = tks.Expect(TokenType.Ident).StringValue;
                tks.Expect(TokenType.Colon);
                var memberType = tks.Expect(TokenType.Ident).StringValue;
                int ptrDepth = 0;
                while (tks.Accept(TokenType.Star) != null)
                {
                    ptrDepth++;
                }
                tks.Expect(TokenType.Semicolon);
                members.Add(new AstDeclaration(si, memberId, memberType, ptrDepth, null));
            }
            return new AstStruct(si, ident, members);
        }

        private static AstExtend ParseDestructor(TokenStream tks)
        {
            var si = tks.SourceInfo;
            tks.Expect(TokenType.Extend);
            tks.Expect(TokenType.ArithNot);
            var typeToExtend = tks.Expect(TokenType.Ident).StringValue;
            tks.Expect(TokenType.LParen);
            var @params = new List<AstDeclaration>();
            var usesThisPtr = ParseExtendParamList(tks, typeToExtend, @params);
            if (!usesThisPtr)
            {
                throw new NotImplementedException("TODO: Exception for destructor with no thisptr");
            }
            if (@params.Count > 1)
            {
                throw new NotImplementedException("TODO: Exception for destructor with params other than thisptr");
            }
            tks.Expect(TokenType.RParen);
            tks.Expect(TokenType.LCurBracket);
            var body = ParseBody(tks);
            tks.Expect(TokenType.RCurBracket);
            return new AstExtend(si, typeToExtend, "Dtor", usesThisPtr, @params, "void", 0, body);
        }

        private static AstExtend ParseConstructor(TokenStream tks)
        {
            var si = tks.SourceInfo;
            tks.Expect(TokenType.Extend);
            var typeToExtend = tks.Expect(TokenType.Ident).StringValue;
            tks.Expect(TokenType.LParen);
            var @params = new List<AstDeclaration>();
            var usesThisPtr = ParseExtendParamList(tks, typeToExtend, @params);
            tks.Expect(TokenType.RParen);
            tks.Expect(TokenType.LCurBracket);
            var body = ParseBody(tks);
            tks.Expect(TokenType.RCurBracket);
            return new AstExtend(si, typeToExtend, "Ctor", usesThisPtr, @params, null, 0, body);
        }

        private static bool ParseExtendParamList(TokenStream tks, string typeToExtend, List<AstDeclaration> @params)
        {
            var si = tks.SourceInfo;
            var firstParam = tks.Accept(TokenType.Ident);
            bool usesThisPtr = false;
            if (firstParam != null)
            {
                // no colon -> no type decl -> using thisptr
                var firstParamType = typeToExtend;
                int ptrDepth = 0;
                if (tks.Accept(TokenType.Colon) == null)
                {
                    ptrDepth = 1;
                    usesThisPtr = true;
                    //@params.Add(new AstDeclaration(firstParam.StringValue, typeToExtend, null));
                    if (tks.Peek().Type != TokenType.RParen)
                    {
                        tks.Expect(TokenType.Comma);
                    }
                }
                else
                {
                    firstParamType = tks.Expect(TokenType.Ident).StringValue;
                    while (tks.Accept(TokenType.Star) != null)
                    {
                        ptrDepth++;
                    }
                    if (tks.Peek().Type != TokenType.RParen)
                    {
                        tks.Expect(TokenType.Comma);
                    }
                }
                @params.Add(new AstDeclaration(si, firstParam.StringValue, firstParamType, ptrDepth, null));
                var otherParams = ParseFuncParamList(tks);
                @params.AddRange(otherParams);
            }
            return usesThisPtr;
        }

        private static AstExtend ParseExtend(TokenStream tks)
        {
            var save = tks.Index;
            var si = tks.SourceInfo;
            tks.Expect(TokenType.Extend);
            if (tks.Accept(TokenType.ArithNot) != null)
            {
                // destructor
                tks.Restore(save);
                return ParseDestructor(tks);
            }
            var typeToExtend = tks.Expect(TokenType.Ident).StringValue;
            if (tks.Accept(TokenType.LParen) != null)
            {
                // constructor
                tks.Restore(save);
                return ParseConstructor(tks);
            }
            var name = tks.Expect(TokenType.Ident).StringValue;
            tks.Expect(TokenType.LParen);
            var @params = new List<AstDeclaration>();
            var usesThisPtr = ParseExtendParamList(tks, typeToExtend, @params);
            tks.Expect(TokenType.RParen);

            var returnType = "void";
            int retPtrDepth = 0;
            if (tks.Accept(TokenType.RightArrow) != null)
            {
                returnType = tks.Expect(TokenType.Ident).StringValue;
                while (tks.Accept(TokenType.Star) != null)
                {
                    retPtrDepth++;
                }
            }

            tks.Expect(TokenType.LCurBracket);
            var body = ParseBody(tks);
            tks.Expect(TokenType.RCurBracket);
            return new AstExtend(si, typeToExtend, name, usesThisPtr, @params, returnType, retPtrDepth, body);
        }

        // <expr>           ::= <decl-assign>
        //                  |   <assign>
        private static AstExpression ParseExpression(TokenStream tks)
        {
            var save = tks.Index;
            var expr = ParseAssign(tks);
            if (expr != null)
            {
                return expr;
            }
            tks.Restore(save);
            return null;
        }

        // <assign>         ::= <binary-expr>
        //                  |   <unary-expr> <assignment-op> <assign>
        private static AstExpression ParseAssign(TokenStream tks)
        {
            var si = tks.SourceInfo;
            var save = tks.Index;
            var expr = ParseBinaryOp(tks);
            if (expr != null && tks.Accept(TokenType.Assign) == null)
            {
                return expr;
            }
            tks.Restore(save);

            expr = ParseUnaryOp(tks);
            if (expr == null)
            {
                tks.Restore(save);
                return null;
            }
            tks.Expect(TokenType.Assign);
            var assign = ParseAssign(tks);
            return new AstAssign(si, expr, assign);
        }

        // <decl-assign>    ::= <ident> ':' <ident> '=' <assign>
        //                  |   <ident> ':' '=' <assign>
        //                  |   <ident> ':' <ident>
        //                  |   <ident> ':' <ident> [ <integer> ]
        private static AstDeclaration ParseVariableDeclaration(TokenStream tks)
        {
            var si = tks.SourceInfo;
            var save = tks.Index;
            var ident = tks.Accept(TokenType.Ident);
            if (ident == null)
            {
                tks.Restore(save);
                return null;
            }
            if (tks.Accept(TokenType.Colon) == null)
            {
                tks.Restore(save);
                return null;
            }
            string varType = null;
            int ptrDepth = 0;
            var type = tks.Accept(TokenType.Ident);
            if (type != null)
            {
                varType = type.StringValue;
                while (tks.Accept(TokenType.Star) != null)
                {
                    ptrDepth++;
                }
            }
            int fixedArraySize = 0;
            if (tks.Accept(TokenType.LSquBracket) != null)
            {
                var size = tks.Expect(TokenType.Number);
                if (!size.IsIntegral())
                {
                    throw new UnexpectedTokenException(size.Type, 
                        string.Format(
                            "Error: {0} : Expected an integral value for an array size.", 
                            size.SourceInfo));
                }
                tks.Expect(TokenType.RSquBracket);
                fixedArraySize = (int) size.ToDecimal();
                ptrDepth++;
            }
            AstExpression rhs = null;
            if (tks.Accept(TokenType.Assign) != null)
            {
                rhs = ParseAssign(tks);
                if (rhs == null)
                {
                    throw new Exception("Expected expression on rhs of variable declaration");
                }
            }
            return new AstDeclaration(si, ident.StringValue, varType, ptrDepth, rhs, fixedArraySize);
        }

        private static List<AstExpression> ParseExpressionList(TokenStream tks)
        {
            var exprList = new List<AstExpression>();
            while (true)
            {
                var exp = ParseAssign(tks);
                if (exp == null)
                {
                    break;
                }
                exprList.Add(exp);
                if (tks.Accept(TokenType.Comma) == null)
                {
                    break;
                }
            }
            return exprList;
        }

        // <primary>            ::= '(' <expr> ')'
        //                      |   <number>
        //                      |   <ident>
        //                      |   <string>
        private static AstExpression ParsePrimary(TokenStream tks)
        {
            var si = tks.SourceInfo;
            var save = tks.Index;
            var tk = tks.Accept(TokenType.LParen);
            if (tk != null)
            {
                var expr = ParseAssign(tks);
                tks.Expect(TokenType.RParen);
                return expr;
            }

            tk = tks.Accept(TokenType.Ident, TokenType.Number, TokenType.String);
            if (tk != null)
            {
                switch (tk.Type)
                {
                    case TokenType.Ident:
                        return new AstIdent(si, tk.StringValue);
                    case TokenType.Number:
                        return new AstNumber(si, tk.ToDecimal());
                    case TokenType.String:
                        return new AstString(si, tk.StringValue);
                }
            }
            tks.Restore(save);
            return null;
        }

        // <postfix-exp>        ::= <primary>
        //                      |   <postfix-exp> '(' <arg-list> ')'
        //                      |   <postfix-exp> '[' <expr> ']'
        private static AstExpression ParsePostfix(TokenStream tks)
        {
            var si = tks.SourceInfo;
            var save = tks.Index;
            var expr = ParsePrimary(tks);
            if (expr == null)
            {
                tks.Restore(save);
                return null;
            }
            Token tk;
            while ((tk = tks.Accept(TokenType.Dot, TokenType.LParen, TokenType.LSquBracket)) != null)
            {
                switch (tk.Type)
                {
                    case TokenType.Dot:
                        var memberIdent = tks.Expect(TokenType.Ident).StringValue;
                        expr = new AstMemberAccess(si, expr, memberIdent);
                        break;
                    case TokenType.LParen:
                        expr = new AstCall(si, expr, ParseExpressionList(tks));
                        tks.Expect(TokenType.RParen);
                        break;
                    case TokenType.LSquBracket:
                        tks.ThrowIfTokenIs(TokenType.RSquBracket);
                        expr = new AstIndex(si, expr, ParseExpressionList(tks));
                        tks.Expect(TokenType.RSquBracket);
                        break;
                }
            }
            return expr;
        }

        private static AstExpression ParseNewExpr(TokenStream tks)
        {
            var si = tks.SourceInfo;
            tks.Expect(TokenType.New);
            var what = tks.Expect(TokenType.Ident).StringValue;
            if (tks.Accept(TokenType.LParen) != null)
            {
                var ctorCall = new AstCall(si, new AstIdent(si, what), ParseExpressionList(tks));
                tks.Expect(TokenType.RParen);
                return new AstUnaryOp(si, TokenType.New, ctorCall);
            }
            var ptrDepth = 1;
            while (tks.Accept(TokenType.Star) != null)
            {
                ptrDepth++;
            }
            tks.Expect(TokenType.LSquBracket);
            var size = ParseExpression(tks);
            tks.Expect(TokenType.RSquBracket);
            return new AstNewArrayOp(si, size, what, ptrDepth);
        }

        // <unary-exp>          ::= <postfix-exp>
        //                      |   <unary-op> <unary-exp>
        private static AstExpression ParseUnaryOp(TokenStream tks)
        {
            var si = tks.SourceInfo;
            var save = tks.Index;
            var postfix = ParsePostfix(tks);
            if (postfix != null)
            {
                return postfix;
            }
            tks.Restore(save);
            var op = tks.Accept(UnaryOps);
            if (op == null)
            {
                tks.Restore(save);
                return null;
            }
            if (op.Type == TokenType.New)
            {
                tks.Restore(save);
                return ParseNewExpr(tks);
            }
            var unary = ParseUnaryOp(tks);
            return new AstUnaryOp(si, op.Type, unary);
        }

        // <bin-expr>           ::= <unary-expr> <binop> <binary-rhs>
        private static AstExpression ParseBinaryOp(TokenStream tks)
        {
            var lhs = ParseUnaryOp(tks);
            var save = tks.Index;
            var expr = ParseBinaryOpHelper(tks, lhs, -1);
            var binop = expr as AstBinaryOp;
            if (binop != null && binop.Rhs == null)
            {
                tks.Restore(save);
                return lhs;
            }
            return expr;
        }

        private static AstExpression ParseBinaryOpHelper(TokenStream tks, AstExpression lhs, int minPrec)
        {
            var si = tks.SourceInfo;
            var binOps = BinaryOpPrec.Select(kvp => kvp.Key).ToArray();
            while (true)
            {
                var op = tks.Current;
                var thisPrec = GetBinOpPrec(op);
                if (thisPrec < minPrec)
                {
                    return lhs;
                }
                tks.ThrowIfTokenIsNotIn(binOps);
                tks.Advance();

                var rhs = ParseUnaryOp(tks);
                var next = tks.Current;
                var nextPrec = GetBinOpPrec(next);
                if (thisPrec < nextPrec ||
                    (thisPrec == nextPrec && IsBinOpRightAssoc(op)))
                {
                    rhs = ParseBinaryOpHelper(tks, rhs, thisPrec + 1);
                }
                lhs = new AstBinaryOp(si, lhs, op.Type, rhs);
            }
        }
    }
}
