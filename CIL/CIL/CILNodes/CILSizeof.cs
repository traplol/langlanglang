using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL.SymbolStore;

namespace CIL.CILNodes
{
    public class CILSizeof : CILExpression
    {
        public string What { get; }

        public CILSizeof(SourceInfo si, string what) : base(si)
        {
            What = what;
        }

        public override string TryInferType(CIntermediateLang cil)
        {
            return "size_t";
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            var tmp = NameGenerator.NewTemp();
            sb.LineDecl(SourceInfo);
            sb.AppendLine(string.Format(
                "{0} {1} = sizeof({2});", 
                TryInferType(cil),
                tmp,
                What));
            cil.LastUsedVar = tmp;
        }
    }
}
