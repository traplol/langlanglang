using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;
using CIL.SymbolStore;
using Langlanglang.Compiler;
using Langlanglang.Tokenization;
using Langlanglang.TypeChecking;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstUnaryOp : AstExpression
    {
        public TokenType Op { get; set; }
        public AstExpression Expression { get; set; }

        public AstUnaryOp(SourceInfo si, TokenType op, AstExpression expression)
            : base(si)
        {
            Op = op;
            Expression = expression;
        }

        public override CILExpression ToCILExpression(CIntermediateLang cil)
        {
            switch (Op)
            {
                case TokenType.New:
                    return CreateCtorCall(cil);
                case TokenType.Delete:
                    return CreateDtorCall(cil);
                case TokenType.Minus:
                    return new CILUnaryOp(SourceInfo, CILUnaryOp.OpType.Negative, Expression.ToCILExpression(cil));
                case TokenType.Plus:
                    return new CILUnaryOp(SourceInfo, CILUnaryOp.OpType.Positive, Expression.ToCILExpression(cil));
                case TokenType.ArithNot:
                    return new CILUnaryOp(SourceInfo, CILUnaryOp.OpType.OnesCompl, Expression.ToCILExpression(cil));
                case TokenType.LogicNot:
                    return new CILUnaryOp(SourceInfo, CILUnaryOp.OpType.BooleanCast, Expression.ToCILExpression(cil));
            }
            throw new NotImplementedException();
        }

        public override LllType TryInferType(CIntermediateLang cil)
        {
            if (Op == TokenType.New)
            {
                var ty = Expression.TryInferType(cil);
                return ty.Clone(ty.PointerDepth + 1, ty.IsAReference);
            }
            return Expression.TryInferType(cil);
        }

        private CILExpression CreateCtorCall(CIntermediateLang cil)
        {
            var call = Expression as AstCall;
            var ctorId = call?.Callee as AstIdent;
            if (ctorId == null)
            {
                throw new NotImplementedException();
            }
            var @sizeof = new AstCall(SourceInfo, new AstIdent(SourceInfo, "sizeof"), new List<AstExpression>
            {
                new AstIdent(SourceInfo, ctorId.Name)
            });
            var malloc = new AstCall(SourceInfo, new AstIdent(SourceInfo, "alloc"), new List<AstExpression>
            {
                @sizeof
            });
            var tmp = NameGenerator.NewTemp();
            var tmpType = new AstType(SourceInfo, ctorId.Name, 1, 0, false, false);
            var decl = new AstDeclaration(SourceInfo, tmp, tmpType, malloc);
            var rewrite = new List<CILNode>();
            var cdecl = decl.ToCILVariableDeclAndDecl(cil);
            cil.DeclareLocalVariable(cdecl);
            rewrite.Add(cdecl);
            var ctor = call.FixIdent(cil, string.Format("{0}_{1}", ctorId.Name, "Ctor"));
            call.Args.Insert(0, new AstIdent(SourceInfo, decl.Name));
            ctorId.Name = ctor;
            var ctorCall = call.ToCILExpression(cil);
            rewrite.Add(ctorCall);
            var objId = new AstIdent(SourceInfo, decl.Name);
            rewrite.Add(objId.ToCILExpression(cil));
            return new CILRewriteExpression(SourceInfo, rewrite);
        }

        private CILExpression CreateDtorCall(CIntermediateLang cil)
        {
            var rewrite = new List<CILNode>();
            var dtorTy = Expression.TryInferType(cil);
            var dtor = string.Format("{0}_{1}_{0}p", dtorTy.Name, "Dtor");
            var dtorSym = LllCompiler.SymTable.LookupSymbol(dtor);
            var dtorExt = dtorSym?.Extra as AstExtend;
            if (dtorExt != null && !dtorExt.CanOverride)
            {
                var callDtor = new AstCall(SourceInfo, new AstIdent(SourceInfo, dtor), new List<AstExpression> {Expression});
                rewrite.Add(callDtor.ToCILExpression(cil));
            }
            var callFree = new AstCall(SourceInfo, new AstIdent(SourceInfo, "free"), new List<AstExpression> {Expression});
            rewrite.Add(callFree.ToCILExpression(cil));
            return new CILRewriteExpression(SourceInfo, rewrite);
        }
    }
}
