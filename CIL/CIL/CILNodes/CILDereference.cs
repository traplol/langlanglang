using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL.SymbolStore;

namespace CIL.CILNodes
{
    public class CILDereference : CILExpression
    {
        public CILExpression What { get; }

        public CILDereference(SourceInfo si, CILExpression what) 
            : base(si)
        {
            What = what;
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            What.Codegen(cil, sb);
            var id = cil.LastUsedVar;
            var deref = string.Format("(*({0}))", id);
            cil.LastUsedVar = deref;
        }

        public override string TryInferType(CIntermediateLang cil)
        {
            var ty = What.TryInferType(cil);
            return CTypes.DereferenceType(ty);
        }
    }
}
