using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL.SymbolStore;

namespace CIL.CILNodes
{
    public class CILInteger : CILExpression
    {
        public long Integer { get; }

        public CILInteger(SourceInfo si, long integer) 
            : base(si)
        {
            Integer = integer;
        }

        private bool BetweenInclusive(long min, long max)
        {
            return Integer >= min && Integer <= max;
        }

        public override string TryInferType(CIntermediateLang cil)
        {
//            if (BetweenInclusive(sbyte.MinValue, sbyte.MaxValue))
//            {
//                return "int8_t";
//            }
//            if (BetweenInclusive(short.MinValue, short.MaxValue))
//            {
//                return "int16_t";
//            }
            if (BetweenInclusive(int.MinValue, int.MaxValue))
            {
                return "int32_t";
            }
            return "int64_t";
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            var tmp = NameGenerator.NewTemp();
            sb.LineDecl(SourceInfo);
            sb.AppendLine(string.Format(
                "{0} {1} = {2};", 
                TryInferType(cil),
                tmp,
                Integer));
            cil.LastUsedVar = tmp;
            //cil.LastUsedVar = string.Format("{0}", Integer);
        }
    }
}
