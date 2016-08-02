using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL.SymbolStore;

namespace CIL.CILNodes
{
    public class CILReference : CILExpression
    {
        public CILExpression Referencing { get; }

        public CILReference(SourceInfo si, CILExpression referencing) 
            : base(si)
        {
            Referencing = referencing;
        }

        public override string TryInferType(CIntermediateLang cil)
        {
            return Referencing.TryInferType(cil) + "*";
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            Referencing.Codegen(cil, sb);
            var tmp = cil.LastUsedVar;
            var newTmp = NameGenerator.NewTemp();
            sb.LineDecl(SourceInfo);
            sb.AppendLine(string.Format(
                "{0} {1} = &({2});", 
                TryInferType(cil),
                newTmp,
                tmp));
            cil.LastUsedVar = newTmp;
        }
    }
}
