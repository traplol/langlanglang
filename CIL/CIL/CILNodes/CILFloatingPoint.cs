using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL.SymbolStore;

namespace CIL.CILNodes
{
    public class CILFloatingPoint : CILExpression
    {
        public double Number { get; }

        public CILFloatingPoint(SourceInfo si, double number) 
            : base(si)
        {
            Number = number;
        }

        public override string TryInferType(CIntermediateLang cil)
        {
            return "double";
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            var tmp = NameGenerator.NewTemp();
            sb.LineDecl(SourceInfo);
            sb.AppendLine(string.Format("{0} {1} = {2:R}", TryInferType(cil), tmp, Number));
            cil.LastUsedVar = tmp;
        }
    }
}
