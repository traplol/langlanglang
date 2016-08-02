using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL.SymbolStore;

namespace CIL.CILNodes
{
    public class CILMemberAccess : CILExpression
    {
        public CILExpression Thing { get; }
        public string Member { get; }

        public CILMemberAccess(SourceInfo si, CILExpression thing, string member) 
            : base(si)
        {
            // TODO: ensure thing resolves to a struct
            Thing = thing;
            Member = member;
        }

        public override string TryInferType(CIntermediateLang cil)
        {
            var thingTy = Thing.TryInferType(cil);
            var @struct = cil.SymTable.LookupStruct(thingTy);
            // TODO: Handle key not found exception
            return @struct.Members[Member].TryInferType();
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            var ty = Thing.TryInferType(cil);
            var separator = CTypes.IsPointerType(ty) ? "->" : ".";
            var @struct = cil.SymTable.LookupStruct(ty);
            // TODO: Handle key not found exception
            var member = @struct.Members[Member];

            Thing.Codegen(cil, sb);
            var thingVar = cil.LastUsedVar;
            cil.LastUsedVar = string.Format("{0}{1}{2}", thingVar, separator, member.Name);
        }
    }
}
