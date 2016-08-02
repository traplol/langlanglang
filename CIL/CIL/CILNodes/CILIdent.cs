using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIL.CILNodes
{
    public class CILIdent : CILExpression
    {
        public string Name { get; }

        public CILIdent(SourceInfo si, string name) 
            : base(si)
        {
            Name = name;
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            cil.LastUsedVar = Name;
        }

        public override string TryInferType(CIntermediateLang cil)
        {
            var resolved = cil.SymTable.LookupSym(Name);
            if (resolved is CILFunction)
            {
                return (resolved as CILFunction).TryInferType(cil);
            }
            if (resolved is CILVariableDecl)
            {
                return (resolved as CILVariableDecl).TryInferType();
            }
            throw new NotImplementedException("TODO: Exception for inability to infer type of identifier");
            //return variable.Type + new string('*', variable.PointerDepth);
        }
    }
}
