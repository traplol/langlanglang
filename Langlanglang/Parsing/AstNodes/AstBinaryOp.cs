using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;
using Langlanglang.Compiler;
using Langlanglang.Tokenization;
using Langlanglang.TypeChecking;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstBinaryOp : AstExpression
    {
        public AstExpression Lhs { get; set; }
        public TokenType Op { get; set; }
        public AstExpression Rhs { get; set; }

        public AstBinaryOp(SourceInfo si, AstExpression lhs, TokenType op, AstExpression rhs)
            : base(si)
        {
            Lhs = lhs;
            Op = op;
            Rhs = rhs;
        }

        private class BinOpInfo
        {
            public string CallAlias { get; }
            public CILBinaryOp.OpType CILOpType { get; }

            public BinOpInfo(string callAlias, CILBinaryOp.OpType cilOpType)
            {
                CallAlias = callAlias;
                CILOpType = cilOpType;
            }
        }

        private static Dictionary<TokenType, BinOpInfo> _binOpInfo = new Dictionary<TokenType, BinOpInfo>
        {
            { TokenType.Plus,           new BinOpInfo("__add__",    CILBinaryOp.OpType.Add) },
            { TokenType.Minus,          new BinOpInfo("__sub__",    CILBinaryOp.OpType.Sub) },
            { TokenType.Star,           new BinOpInfo("__mul__",    CILBinaryOp.OpType.Mul) },
            { TokenType.FSlash,         new BinOpInfo("__div__",    CILBinaryOp.OpType.Div) },
            { TokenType.Percent,        new BinOpInfo("__mod__",    CILBinaryOp.OpType.Mod) },
            { TokenType.ArithXor,       new BinOpInfo("__xor__",    CILBinaryOp.OpType.Xor) },
            { TokenType.ArithOr,        new BinOpInfo("__or__",     CILBinaryOp.OpType.Or) },
            { TokenType.ArithAnd,       new BinOpInfo("__and__",    CILBinaryOp.OpType.And) },
            { TokenType.LessThan,       new BinOpInfo("__lt__",     CILBinaryOp.OpType.LessThan) },
            { TokenType.LessThanEquals, new BinOpInfo("__le__",     CILBinaryOp.OpType.LessThanEquals) },
            { TokenType.GreaterThan,    new BinOpInfo("__gt__",     CILBinaryOp.OpType.GreaterThan) },
            { TokenType.GreaterThanEquals, new BinOpInfo("__ge__",  CILBinaryOp.OpType.GreaterThanEquals) },
            { TokenType.LogicEquals,    new BinOpInfo("__eq__",     CILBinaryOp.OpType.Equals) },
            { TokenType.LogicNotEquals, new BinOpInfo("__ne__",     CILBinaryOp.OpType.NotEquals) },
            { TokenType.LeftShift,      new BinOpInfo("__lshift__", CILBinaryOp.OpType.LShift) },
            { TokenType.RightShift,     new BinOpInfo("__rshift__", CILBinaryOp.OpType.RShift) },
        };

        public bool IsConditionalOp()
        {
            switch (Op)
            {
                case TokenType.LogicAnd:
                case TokenType.LogicOr:
                    return true;
            }
            return false;
        }


        private CILExpression ToCILCondition(CIntermediateLang cil)
        {
            var condOp = (Op == TokenType.LogicAnd)
                ? CILCondition.CondType.And 
                : CILCondition.CondType.Or;
            return new CILCondition(
                SourceInfo, 
                Lhs.ToCILExpression(cil), 
                condOp, 
                Rhs.ToCILExpression(cil));
        }

        public override CILExpression ToCILExpression(CIntermediateLang cil)
        {
            if (IsConditionalOp())
            {
                return ToCILCondition(cil);
            }
            var lhsTy = Lhs.TryInferType(cil);
            var lhs = lhsTy.IsAReference 
                ? new CILDereference(Lhs.SourceInfo, Lhs.ToCILExpression(cil)) 
                : Lhs.ToCILExpression(cil);


            var rhsTy = Rhs.TryInferType(cil);
            var rhs = rhsTy.IsAReference 
                ? new CILDereference(Rhs.SourceInfo, Rhs.ToCILExpression(cil)) 
                : Rhs.ToCILExpression(cil);

            // primitives, just use the builtin operators
            if (lhsTy.IsPrimitive && rhsTy.IsPrimitive)
            {
                return new CILBinaryOp(
                    SourceInfo,
                    lhs,
                    _binOpInfo[Op].CILOpType,
                    rhs);
            }

            // not primitive, make a call to lhs.__op__(rhs);
            var opAlias = _binOpInfo[Op].CallAlias;
            var member = new AstMemberAccess(SourceInfo, Lhs, opAlias);
            var opCall = new AstCall(SourceInfo, member, new List<AstExpression> {Rhs});
            return opCall.ToCILExpression(cil);
        }

        public override LllType TryInferType(CIntermediateLang cil)
        {
            var lhsTy = Lhs.TryInferType(cil);
            var rhsTy = Rhs.TryInferType(cil);
            if (lhsTy is LllIntegerType && rhsTy is LllIntegerType)
            {
                if (Rhs is AstNumber)
                {
                    return lhsTy;
                }
                if (Lhs is AstNumber)
                {
                    return rhsTy;
                }
            }
            if (lhsTy.Equals(rhsTy))
            {
                return lhsTy;
            }
            if (rhsTy.TryCast(lhsTy))
            {
                return lhsTy;
            }
            if (lhsTy.TryCast(rhsTy))
            {
                return rhsTy;
            }
            // not primitive, make a call to lhs.__op__(rhs);
            var opAlias = _binOpInfo[Op].CallAlias;
            var member = new AstMemberAccess(SourceInfo, Lhs, opAlias);
            var opCall = new AstCall(SourceInfo, member, new List<AstExpression> {Rhs});
            return opCall.TryInferType(cil);
        }
    }
}
