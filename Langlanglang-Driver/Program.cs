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
            if (args.Length < 1)
            {
                Console.WriteLine("usage: lllc file opt...");
                return;
            }
            var compiler = new LllCompiler();
            var file = args[0];
            var fullPath = new FileInfo(file);

            var sw = new Stopwatch();
            sw.Start();
            var compiledCode = compiler.CompileFile(file);
            sw.Stop();
            if (compiledCode.Length == 0)
            {
                return;
            }
            Console.WriteLine("Compiled to C in: {0}ms", sw.ElapsedMilliseconds);

            var fileNoExt = fullPath.Name;
            var extPos = fileNoExt.LastIndexOf(".lll");
            if (extPos > 0)
            {
                fileNoExt = fileNoExt.Remove(extPos);
            }
            var outDir = fullPath.DirectoryName + "\\output";
            var exePath = outDir + "\\" + fileNoExt + ".exe";
            var objDir = outDir + "\\obj";
            var objPath = objDir + "\\" + fileNoExt + ".obj";
            var cfilePath = outDir + "\\" + fileNoExt + ".c";
            Directory.CreateDirectory(outDir);
            Directory.CreateDirectory(objDir);
            Console.WriteLine("Writing C code to: '{0}'", cfilePath);
            File.WriteAllText(cfilePath, compiledCode);

            var otherArgs = new StringBuilder();
            otherArgs.Append("/O2 /GL ");
            otherArgs.AppendFormat(@"/Fo""{0}"" ", objPath);
            otherArgs.AppendFormat(@"/Fe""{0}"" ", exePath);
            //otherArgs.AppendFormat(@"/I""C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\include"" ");
            otherArgs.AppendFormat(@"""{0}""", cfilePath);

            var p = new Process
            {
                StartInfo = new ProcessStartInfo("cl.exe", otherArgs.ToString())
                {
                    UseShellExecute = false
                }
            };
            Console.WriteLine("{0} {1}", p.StartInfo.FileName, p.StartInfo.Arguments);
            p.Start();
            p.WaitForExit();
        }
    }
}
