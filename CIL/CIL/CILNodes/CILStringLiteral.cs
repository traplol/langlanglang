using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL.SymbolStore;

namespace CIL.CILNodes
{
    public class CILStringLiteral : CILExpression
    {
        public string String { get; }

        public CILStringLiteral(SourceInfo si, string s) 
            : base(si)
        {
            String = s;
        }


        public override string TryInferType(CIntermediateLang cil)
        {
            return "char*";
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            var tmp = NameGenerator.NewTemp();
            sb.LineDecl(SourceInfo);
            sb.AppendLine(string.Format(
                "{0} {1} = \"{2}\";", 
                TryInferType(cil),
                tmp,
                String));
            cil.LastUsedVar = tmp;
            //cil.LastUsedVar = string.Format("\"{0}\"", String);
        }
    }
}
