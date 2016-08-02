using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langlanglang
{
    public class LllException : Exception
    {
        public LllException() { }
        public LllException(string message) : base(message) { }
        public LllException(string message, Exception inner) : base(message, inner) { }
    }
}
