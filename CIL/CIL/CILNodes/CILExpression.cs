using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIL.CILNodes
{
    public abstract class CILExpression : CILNode
    {
        protected CILExpression(SourceInfo si)
            : base(si)
        {
            
        }
    }
}
