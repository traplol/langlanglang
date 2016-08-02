using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL.SymbolStore;

namespace CIL.CILNodes
{
    public class CILUnaryOp : CILExpression
    {
        public enum OpType
        {
            Reference, Dereference,
            Negative,
            Positive,
            OnesCompl,
            LogicNot,
            BooleanCast,
        }

        private enum Side
        {
            Pre, Post
        }

        public OpType Op { get; }
        public CILExpression Expr { get; }

        public CILUnaryOp(SourceInfo si, OpType op, CILExpression expr) 
            : base(si)
        {
            Op = op;
            Expr = expr;
        }

        public override string TryInferType(CIntermediateLang cil)
        {
            return Expr.TryInferType(cil);
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            var op = GetOpString();
            Expr.Codegen(cil, sb);
            var tmp = cil.LastUsedVar;
            var result = NameGenerator.NewTemp();
            var fmt = GetOpSide() == Side.Pre
                ? "{0} {1} = ({2}({3}));"
                : "{0} {1} = (({3}){2});";
            sb.LineDecl(SourceInfo);
            sb.AppendLine(string.Format(
                fmt,
                TryInferType(cil),
                result,
                op,
                tmp));
            cil.LastUsedVar = result;
        }

        private string GetOpString()
        {
            switch (Op)
            {
                case OpType.Reference:
                    return "&";
                case OpType.Dereference:
                    return "*";
                case OpType.Negative:
                    return "-";
                case OpType.Positive:
                    return "+";
                case OpType.OnesCompl:
                    return "~";
                case OpType.LogicNot:
                    return "!";
                case OpType.BooleanCast:
                    return "!!";
            }
            throw new NotImplementedException();
        }

        private Side GetOpSide()
        {
            switch (Op)
            {
                case OpType.Reference:
                case OpType.Dereference:
                case OpType.Negative:
                case OpType.Positive:
                case OpType.OnesCompl:
                case OpType.LogicNot:
                case OpType.BooleanCast:
                    return Side.Pre;
            }
            throw new NotImplementedException();
        }
    }
}
