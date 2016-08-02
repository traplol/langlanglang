using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIL.Exceptions
{
    class CILSymbolNotFoundException : CILException
    {
        public CILSymbolNotFoundException(SourceInfo sourceInfo) : base(sourceInfo)
        {
        }

        public CILSymbolNotFoundException(SourceInfo sourceInfo, string message) : base(sourceInfo, message)
        {
        }

        public CILSymbolNotFoundException(SourceInfo sourceInfo, string message, Exception inner) : base(sourceInfo, message, inner)
        {
        }
    }
}
