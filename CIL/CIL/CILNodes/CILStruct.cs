using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIL.CILNodes
{
    public class CILStruct : CILType
    {
        public Dictionary<string, CILVariableDecl> Members { get; }

        internal CILStruct(SourceInfo si, string name, IReadOnlyCollection<CILVariableDecl> members)
            : base(si, name, false)
        {
            Members = new Dictionary<string, CILVariableDecl>(members.Count);
            foreach (var m in members)
            {
                AddMember(m);
            }
        }

        public void AddMember(CILVariableDecl member)
        {
            Members.Add(member.Name, member);
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            sb.LineDecl(SourceInfo);
            sb.AppendLine(GetDeclaration());
            sb.AppendLine("{");
            cil.PushScope();
            sb.Indent();
            foreach (var kvp in Members)
            {
                sb.LineDecl(SourceInfo);
                kvp.Value.Codegen(cil, sb);
            }
            sb.Dedent();
            cil.PopScope();
            sb.AppendLine("};");
        }

        public override string GetDeclaration()
        {
            return string.Format("struct {0}", Name);
        }

    }
}
