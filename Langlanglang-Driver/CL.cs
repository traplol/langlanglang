using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langlanglang_Driver
{
    public class Cl : ICompiler
    {
        public string Name { get; }
        public string CompilerExe { get; }

        public string ObjectDir { get; set; }
        public string ExecutableDir { get; set; }
        public List<string> IncludeDirs { get; } 

        public bool IsDebug { get; set; }
        public bool IsRelease { get { return !IsDebug; } set { IsDebug = !value; } }

        public Cl()
        {
            Name = "CL";
            CompilerExe = "cl.exe";
            IncludeDirs = new List<string>();
            IsDebug = true;
        }

        public void SetToDebug()
        {
            IsDebug = true;
        }

        public void SetToRelease()
        {
            IsDebug = false;
        }

        public void CompileFiles(string outName, IEnumerable<string> filepaths)
        {
            var args = GenerateCompilerArgs(outName, filepaths);
            if (ExecutableDir != null)
            {
                Directory.CreateDirectory(ExecutableDir);
            }
            if (ObjectDir != null)
            {
                Directory.CreateDirectory(ObjectDir);
            }
            Console.WriteLine("{0} {1}", CompilerExe, args);
            var p = new Process
            {
                StartInfo = new ProcessStartInfo(CompilerExe, args)
                {
                    UseShellExecute = false
                }
            };
            p.Start();
            p.WaitForExit();
        }

        public string GenerateCompilerArgs(string outName, IEnumerable<string> filepaths)
        {
            var sb = new StringBuilder();
            if (IsDebug)
            {
                sb.Append("/Zi ");
            }
            else
            {
                sb.Append("/O2 /GL ");
            }
            if (ObjectDir != null)
            {
                var objDir = ObjectDir.TrimEnd('\\');
                sb.AppendFormat("/Fo\"{0}\\{1}.obj\" ", objDir, outName);
            }
            if (ExecutableDir != null)
            {
                var exeDir = ExecutableDir.TrimEnd('\\');
                sb.AppendFormat("/Fe\"{0}\\{1}.exe\" ", exeDir, outName);
            }
            foreach (var include in IncludeDirs)
            {
                var incDir = include.TrimEnd('\\');
                sb.AppendFormat("/I\"{0}\" ", incDir);
            }
            foreach (var filepath in filepaths)
            {
                sb.AppendFormat("\"{0}\" ", filepath);
            }
            return sb.ToString();
        }
    }
}
