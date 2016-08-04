using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;
using Langlanglang.Compiler;
using Langlanglang.TypeChecking;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstType : AstNode
    {
        public string TypeName { get; set; }
        public int PointerDepth { get; set; }
        public int FixedArraySize { get; set; }
        public bool IsAReference { get; set; }
        public bool IsGeneric { get; set; }

        public AstType(SourceInfo sourceInfo, string typeName, int pointerDepth, int fixedArraySize, bool isAReference, bool isGeneric) 
            : base(sourceInfo)
        {
            TypeName = typeName;
            PointerDepth = pointerDepth;
            IsAReference = isAReference;
            FixedArraySize = fixedArraySize;
            IsGeneric = isGeneric;
        }

        public override CILNode ToCILNode(CIntermediateLang cil)
        {
            throw new NotImplementedException();
        }

        public LllType ToLllType()
        {
            var ty = LllCompiler.SymTable.LookupType(TypeName);
            var ptrDepth = IsAReference ? PointerDepth + 1 : PointerDepth;
            return ty.Clone(ptrDepth, IsAReference);
        }

        public override string ToString()
        {
            return TypeName;
        }
    }
}
