using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL.SymbolStore;

namespace CIL.CILNodes
{
    public class CILLoop : CILStatement
    {
        public List<CILNode> Before { get; }
        public CILExpression Condition { get; }
        public List<CILExpression> After { get; }
        public List<CILNode> Body { get; }

        public CILLoop(SourceInfo si, List<CILNode> before, CILExpression condition, List<CILExpression> after, List<CILNode> body) 
            : base(si)
        {
            Before = before;
            Condition = condition;
            After = after;
            Body = body;
        }

        public CILLoop(SourceInfo si, CILExpression condition) 
            : base(si)
        {
            Before = new List<CILNode>();
            Condition = condition;
            After = new List<CILExpression>();
            Body = new List<CILNode>();
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            cil.PushScope();
            sb.AppendLine("{");
            sb.Indent();
            foreach (var b in Before)
            {
                b.Codegen(cil, sb);
            }
            var lbl = NameGenerator.NewLabel();
            sb.AppendLineNoIndent(string.Format("{0}:", lbl));
            sb.AppendLine("{");
            sb.Indent();
            Condition.Codegen(cil, sb);
            var cond = cil.LastUsedVar;
            sb.LineDecl(SourceInfo);
            sb.AppendLine(string.Format("if ({0})", cond));
            sb.AppendLine("{");
            sb.Indent();
            foreach (var stmt in Body)
            {
                stmt.Codegen(cil, sb);
            }
            foreach (var stmt in After)
            {
                stmt.Codegen(cil, sb);
            }
            sb.AppendLine(string.Format("goto {0};", lbl));
            sb.Dedent();
            sb.AppendLine("}");
            sb.Dedent();
            sb.AppendLine("}");
            sb.Dedent();
            sb.AppendLine("}");
            cil.PopScope();
        }
    }
}
