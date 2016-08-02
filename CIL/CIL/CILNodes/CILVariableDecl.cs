using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIL.CILNodes
{
    public class CILVariableDecl : CILNode
    {
        public bool IsStatic { get; }
        public CILType Type { get; }
        public int PointerDepth { get; }
        public string Name { get; }
        public CILExpression AssigningValue { get; }

        public CILVariableDecl(SourceInfo sourceInfo, CILType type, int pointerDepth, string name, bool isStatic = false) 
            : base(sourceInfo)
        {
            IsStatic = isStatic;
            Type = type;
            PointerDepth = pointerDepth;
            Name = name;
            AssigningValue = null;
        }

        public CILVariableDecl(SourceInfo sourceInfo, CILType type, int pointerDepth, string name, CILExpression assigningValue, bool isStatic = false)
            : base(sourceInfo)
        {
            IsStatic = isStatic;
            Type = type;
            PointerDepth = pointerDepth;
            Name = name;
            AssigningValue = assigningValue;
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            cil.DeclareLocalVariable(this);
            if (AssigningValue == null)
            {
                sb.AppendLine(GetDeclaration() + ";");
            }
            else
            {
                sb.LineDecl(SourceInfo);
                AssigningValue.Codegen(cil, sb);
                var tmp = cil.LastUsedVar;
                sb.AppendLine(string.Format("{0} = {1};", GetDeclaration(), tmp));
            }
        }

        public virtual string GetDeclaration()
        {
            return string.Format(
                "{0} {1}",
                TryInferType(),
                Name);
        }

        public virtual string TryInferType()
        {
            return string.Format("{0}{1}", Type, new string('*', PointerDepth));
        }
    }
}
