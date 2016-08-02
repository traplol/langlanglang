using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIL.CILNodes
{
    public class CILBranch : CILStatement
    {
        public CILExpression Condition { get; }
        public List<CILNode> TrueBranch { get; }
        public List<CILNode> FalseBranch { get; }

        public CILBranch(SourceInfo si, CILExpression condition) : base(si)
        {
            Condition = condition;
            TrueBranch = new List<CILNode>();
            FalseBranch = new List<CILNode>();
        }

        public CILBranch(SourceInfo si, CILExpression condition, List<CILNode> trueBranch, List<CILNode> falseBranch) 
            : base(si)
        {
            Condition = condition;
            TrueBranch = trueBranch;
            FalseBranch = falseBranch;
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            cil.PushScope();
            sb.AppendLine("{");
            sb.Indent();

            Condition.Codegen(cil, sb);
            sb.LineDecl(SourceInfo);
            sb.AppendLine(string.Format("if ({0})", cil.LastUsedVar));
            sb.AppendLine("{");
            sb.Indent();
            foreach (var stmt in TrueBranch)
            {
                stmt.Codegen(cil, sb);
            }
            sb.Dedent();
            sb.AppendLine("}");
            if (FalseBranch.Count > 0)
            {
                sb.AppendLine("else");
                sb.AppendLine("{");
                sb.Indent();
                foreach (var stmt in FalseBranch)
                {
                    stmt.Codegen(cil, sb);
                }
                sb.Dedent();
                sb.AppendLine("}");
            }

            sb.Dedent();
            sb.AppendLine("}");
            cil.PopScope();
        }

        public void AddTrueBranchStmt(CILNode node)
        {
            TrueBranch.Add(node);
        }

        public void AddFalseBranchStmt(CILNode node)
        {
            FalseBranch.Add(node);
        }
    }
}
