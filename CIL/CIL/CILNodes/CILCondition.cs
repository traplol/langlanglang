using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CIL.SymbolStore;

namespace CIL.CILNodes
{
    public class CILCondition : CILExpression
    {
        public enum CondType
        {
            Or,
            And,
        }

        public CILExpression Lhs { get; }
        public CondType Op { get; }
        public CILExpression Rhs { get; }

        public CILCondition(SourceInfo si, CILExpression lhs, CondType op, CILExpression rhs) 
            : base(si)
        {
            Lhs = lhs;
            Op = op;
            Rhs = rhs;
        }

        public override string TryInferType(CIntermediateLang cil)
        {
            return "int";
        }

        private void CodegenAnd(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            var tmpResult = NameGenerator.NewTemp();
            sb.LineDecl(SourceInfo);
            sb.AppendLine(string.Format("int {0} = 0;", tmpResult));
            var true_ = new CILInteger(SourceInfo, 1);
            var false_ = new CILInteger(SourceInfo, 0);
            var lhsBranch = new CILBranch(SourceInfo, Lhs);
            lhsBranch.AddTrueBranchStmt(new CILAssignment(SourceInfo, new CILIdent(SourceInfo, tmpResult), true_));
            lhsBranch.AddFalseBranchStmt(new CILAssignment(SourceInfo, new CILIdent(SourceInfo, tmpResult), false_));
            var rhsBranch = new CILBranch(SourceInfo, Rhs);
            rhsBranch.AddTrueBranchStmt(new CILAssignment(SourceInfo, new CILIdent(SourceInfo, tmpResult), true_));
            rhsBranch.AddFalseBranchStmt(new CILAssignment(SourceInfo, new CILIdent(SourceInfo, tmpResult), false_));
            lhsBranch.AddTrueBranchStmt(rhsBranch);
            lhsBranch.Codegen(cil, sb);

            cil.LastUsedVar = tmpResult;
        }

        private void CodegenOr(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            var tmpResult = NameGenerator.NewTemp();
            sb.AppendLine(string.Format("int {0} = 0;", tmpResult));
            var true_ = new CILInteger(SourceInfo, 1);
            var false_ = new CILInteger(SourceInfo, 0);
            var lhsBranch = new CILBranch(SourceInfo, Lhs);
            lhsBranch.AddTrueBranchStmt(new CILAssignment(SourceInfo, new CILIdent(SourceInfo, tmpResult), true_));
            var rhsBranch = new CILBranch(SourceInfo, Rhs);
            rhsBranch.AddTrueBranchStmt(new CILAssignment(SourceInfo, new CILIdent(SourceInfo, tmpResult), true_));
            lhsBranch.AddFalseBranchStmt(rhsBranch);
            lhsBranch.Codegen(cil, sb);

            cil.LastUsedVar = tmpResult;
            //throw new NotImplementedException();
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            if (Op == CondType.And)
            {
                CodegenAnd(cil, sb);
                return;
            }
            if (Op == CondType.Or)
            {
                CodegenOr(cil, sb);
                return;
            }
            throw new NotImplementedException();
        }
    }
}
