using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIL.CILNodes
{
    public class CILFixedArray : CILVariableDecl
    {
        public long Size { get; set; }

        public CILFixedArray(SourceInfo sourceInfo, CILType type, int pointerDepth, string name, int size, bool isStatic = false)
            : base(sourceInfo, type, pointerDepth, name, isStatic)
        {
            Size = size;
        }

        public override string GetDeclaration()
        {
            return string.Format(
                "{0} {1}[{2}]",
                TryInferType(), Name, Size);
        }

        public override string TryInferType()
        {
            return string.Format("{0}{1}", Type, new string('*', PointerDepth-1));
        }
    }
}
