using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langlanglang.Compiler.Exceptions
{
    public class UndefinedSymbolException : LllException
    {
        public UndefinedSymbolException(string message) : base(message) { }
    }
}
