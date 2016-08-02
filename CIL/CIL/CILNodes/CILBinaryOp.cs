using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL.SymbolStore;

namespace CIL.CILNodes
{
    public class CILBinaryOp : CILExpression
    {
        public enum OpType
        {
            Add, Sub,
            Mul, Div,
            Mod,
            And, Or, Xor,
            LShift, RShift,
            Equals, NotEquals,
            LessThan, LessThanEquals,
            GreaterThan, GreaterThanEquals
        }
        public CILExpression Lhs { get; }
        public OpType Op { get; }
        public CILExpression Rhs { get; }

        public CILBinaryOp(SourceInfo si, CILExpression lhs, OpType op, CILExpression rhs) 
            : base(si)
        {
            Lhs = lhs;
            Op = op;
            Rhs = rhs;
        }

        public override string TryInferType(CIntermediateLang cil)
        {
            switch (Op)
            {
                case OpType.Add:
                case OpType.Sub:
                case OpType.Mul:
                case OpType.Div:
                    return CTypes.BestNumberType(Lhs.TryInferType(cil), Rhs.TryInferType(cil));
                case OpType.Mod:
                case OpType.And:
                case OpType.Or:
                case OpType.Xor:
                case OpType.LShift:
                case OpType.RShift:
                    return CTypes.LargerIntegerType(Lhs.TryInferType(cil), Rhs.TryInferType(cil));
                case OpType.Equals:
                case OpType.NotEquals:
                case OpType.LessThan:
                case OpType.LessThanEquals:
                case OpType.GreaterThan:
                case OpType.GreaterThanEquals:
                    return "int";
            }
            throw new NotImplementedException("TODO: Exception for type inference failing.");
        }

        private string OpString()
        {
            switch (Op)
            {
                case OpType.Add:
                    return "+";
                case OpType.Sub:
                    return "-";
                case OpType.Mul:
                    return "*";
                case OpType.Div:
                    return "/";
                case OpType.Mod:
                    return "%";
                case OpType.And:
                    return "&";
                case OpType.Or:
                    return "|";
                case OpType.Xor:
                    return "^";
                case OpType.LShift:
                    return "<<";
                case OpType.RShift:
                    return ">>";
                case OpType.Equals:
                    return "==";
                case OpType.NotEquals:
                    return "!=";
                case OpType.LessThan:
                    return "<";
                case OpType.LessThanEquals:
                    return "<=";
                case OpType.GreaterThan:
                    return ">";
                case OpType.GreaterThanEquals:
                    return ">=";
            }
            throw new NotImplementedException("TODO: Exception for unknown type.");
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            Lhs.Codegen(cil, sb);
            var lhsTmp = cil.LastUsedVar;
            Rhs.Codegen(cil, sb);
            var rhsTmp = cil.LastUsedVar;

            var tmp = NameGenerator.NewTemp();
            sb.LineDecl(SourceInfo);
            sb.AppendLine(string.Format("{0} {1} = {2} {3} {4};", TryInferType(cil), tmp, lhsTmp, OpString(), rhsTmp));
            cil.LastUsedVar = tmp;
        }
    }
}
