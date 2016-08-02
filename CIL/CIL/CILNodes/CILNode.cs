using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CIL.CILNodes
{
    public abstract class CILNode
    {
        public SourceInfo SourceInfo { get; }

        protected CILNode(SourceInfo sourceInfo)
        {
            SourceInfo = sourceInfo;
        }

        public abstract void Codegen(CIntermediateLang cil, IndentingStringBuilder sb);

        public virtual string TryInferType(CIntermediateLang cil)
        {
            throw new NotSupportedException();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            var other = obj as CILNode;
            return other != null && EqualsImpl(other);
        }

        protected virtual bool EqualsImpl(CILNode other)
        {
            return false;
        }
    }
}
