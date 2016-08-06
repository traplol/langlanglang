using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langlanglang.TypeChecking.Exceptions
{
    public class SymbolAlreadyDeclaredException : LllException
    {
        public SymbolAlreadyDeclaredException(string message) : base(message) { }
    }
}
