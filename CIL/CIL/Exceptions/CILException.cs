using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIL.Exceptions
{
    public class CILException : Exception
    {
        public SourceInfo SourceInfo { get; }

        public CILException(SourceInfo sourceInfo)
        {
            SourceInfo = sourceInfo;
        }

        public CILException(SourceInfo sourceInfo, string message) 
            : base(message)
        {
            SourceInfo = sourceInfo;
        }

        public CILException(SourceInfo sourceInfo, string message, Exception inner) 
            : base(message, inner)
        {
            SourceInfo = sourceInfo;
        }
    }
}
