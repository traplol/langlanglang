using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langlanglang.Compiler.Exceptions
{
    public class AbiguousCallToAnOverloadedFunctionException : LllException
    {
        public AbiguousCallToAnOverloadedFunctionException(string message) :base(message) { }
    }
}
