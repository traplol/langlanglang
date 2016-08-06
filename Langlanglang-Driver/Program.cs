using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Langlanglang.Compiler;

namespace Langlanglang_Driver
{
    class Program
    {

        static void Main(string[] args)
        {
            var lllc = new LLLC(new Cl());
            lllc.Compile(args);
        }
    }
}
