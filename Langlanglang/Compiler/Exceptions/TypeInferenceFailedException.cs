using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langlanglang.Compiler.Exceptions
{
    public class TypeInferenceFailedException : LllException
    {
        public TypeInferenceFailedException() { }
        public TypeInferenceFailedException(string message) : base(message) { }
        public TypeInferenceFailedException(string message, Exception inner) : base(message, inner) { }
    }
}
