using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langlanglang.TypeChecking.Exceptions
{
    public class StructMemberNotFoundException : LllException
    {
        public StructMemberNotFoundException(string message) : base(message) { }
    }
}
