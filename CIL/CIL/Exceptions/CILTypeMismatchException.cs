using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIL.Exceptions
{
    public class CILTypeMismatchException : CILException
    {
        public CILTypeMismatchException(SourceInfo sourceInfo, string expected, string actual) 
            : base(sourceInfo, string.Format(
                      "Error {0} : Type mismatch: expected:{1}, actual:{2}", 
                      sourceInfo, expected, actual))
        {
        }

        public CILTypeMismatchException(SourceInfo sourceInfo, string message) 
            : base(sourceInfo, string.Format(
                "Error {0} : Type mismatch: {1}",
                sourceInfo, message))
        {
        }

        public CILTypeMismatchException(SourceInfo sourceInfo, string message, Exception inner) 
            : base(sourceInfo, message, inner)
        {
        }
    }
}
