using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Langlanglang_Driver
{
    public interface ICompiler
    {
        string Name { get; }
        string CompilerExe { get; }

        string ObjectDir { get; set; }
        string ExecutableDir { get; set; }
        List<string> IncludeDirs { get; } 

        bool IsDebug { get; set; }

        void CompileFiles(string outName, IEnumerable<string> filepaths);
    }
}
