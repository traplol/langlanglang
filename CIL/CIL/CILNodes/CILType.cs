using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIL.CILNodes
{
    public class CILType : CILStatement
    {
        public string Name { get; }
        public bool IsPrimitive { get; }

        internal CILType(SourceInfo sourceInfo, string name, bool isPrimitive) 
            : base(sourceInfo)
        {
            Name = name;
            IsPrimitive = isPrimitive;
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            throw new NotSupportedException();
        }

        public virtual string GetDeclaration()
        {
            return Name;
        }

        public override string ToString()
        {
            return GetDeclaration();
        }
    }
}
