using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL.SymbolStore;

namespace CIL.CILNodes
{
    public class CILIndexAccess : CILExpression
    {
        public CILExpression Pointer { get; }
        public CILExpression Subscription { get; }

        public CILIndexAccess(SourceInfo si, CILExpression pointer, CILExpression subscription) : base(si)
        {
            // TODO: Ensure the thing being indexed is a pointer
            Pointer = pointer;
            Subscription = subscription;
        }

        public override string TryInferType(CIntermediateLang cil)
        {
            // TODO: Remove a level of pointers
            var ty = Pointer.TryInferType(cil);
            return CTypes.DereferenceType(ty);
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            Subscription.Codegen(cil, sb);
            var subTmp = cil.LastUsedVar;
            Pointer.Codegen(cil, sb);
            var sub = string.Format("{0}[{1}]", cil.LastUsedVar, subTmp);
            cil.LastUsedVar = sub;
        }
    }
}
