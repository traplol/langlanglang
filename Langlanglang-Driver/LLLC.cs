using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Langlanglang.Compiler;

namespace Langlanglang_Driver
{
    public class LLLC
    {
        private readonly ICompiler _compiler;

        public bool IsDebug { get { return _compiler.IsDebug; } set { _compiler.IsDebug = value; } }
        public bool IsRelease { get { return !IsDebug; } set { IsDebug = !value; } }

        private Queue<string> _filesToCompile; 

        public LLLC(ICompiler compiler)
        {
            _compiler = compiler;
        }

        public void Usage()
        {
            Console.WriteLine(
@"usage: lllc [opts...] -c files...
    --help for more options
"
);
        }

        public void Help()
        {
            Console.WriteLine(
@"usage: lllc [opts...] -c files...
    --help               Displays this message.
    --release            Sets the build to release mode, enabling optimizations.
    --debug              (On by default) Sets the build to debug mode, enabling debug symbols.
    -obj <directory>     Sets the object directory if the compiler uses one.
    -exe <directory>     Sets the directory of the compiled executable(s)
    -c <files>           Compiles one or more files as separate executables
"
);
        }

        public void Compile(string[] args)
        {
            if (!ParseArgs(args))
            {
                return;
            }
            _compiler.IncludeDirs.Clear();
            var lllcDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
            var libDir = lllcDir + "\\lib";
            _compiler.IncludeDirs.Add(libDir);

            var sw = new Stopwatch();
            while (_filesToCompile.Count > 0)
            {
                var file = _filesToCompile.Dequeue();
                var lllc = new LllCompiler();
                sw.Restart();
                var compiledCode = lllc.CompileFile(file);
                sw.Stop();
                if (string.IsNullOrEmpty(compiledCode))
                {
                    // TODO: Message saying it failed to compile.
                    continue;
                }
                Console.WriteLine("Compiled to C in: {0}ms", sw.ElapsedMilliseconds);

                var fullPath = new FileInfo(file);
                var fileNoExt = fullPath.Name;
                var extPos = fileNoExt.LastIndexOf(".lll");
                if (extPos > 0)
                {
                    fileNoExt = fileNoExt.Remove(extPos);
                }
                var outDir = fullPath.DirectoryName + "\\output";
                if (_compiler.ExecutableDir == null)
                {
                    _compiler.ExecutableDir = outDir;
                }
                if (_compiler.ObjectDir == null)
                {
                    _compiler.ObjectDir = outDir + "\\obj";
                }
                var cfilePath = outDir + "\\" + fileNoExt + ".c";
                Directory.CreateDirectory(outDir);
                Console.WriteLine("Writing C code to: '{0}'", cfilePath);
                File.WriteAllText(cfilePath, compiledCode);
                _compiler.CompileFiles(fileNoExt, new List<string> {cfilePath});
            }
        }

        private bool ParseArgs(string[] args)
        {
            if (args.Length < 1)
            {
                Usage();
                return false;
            }
            bool debug = false, release = false;
            string objDir = null, exeDir = null;
            var files = new Queue<string>();

            bool isAfterDashC = false;
            for (int i = 0; i < args.Length; ++i)
            {
                var arg = args[i];
                if (arg == "--help")
                {
                    Help();
                    return false;
                }
                if (isAfterDashC)
                {
                    files.Enqueue(arg);
                }
                if (arg == "-obj")
                {
                    objDir = args[++i];
                }
                else if (arg == "-exe")
                {
                    exeDir = args[++i];
                }
                else if (arg == "-c")
                {
                    isAfterDashC = true;
                }
                else if (arg == "--release")
                {
                    release = true;
                }
                else if (arg == "--debug")
                {
                    debug = true;
                }
            }

            if (release && debug)
            {
                Console.WriteLine("Cannot specify both --debug and --release!");
                return false;
            }
            if (release)
            {
                IsRelease = true;
            }
            else
            {
                IsDebug = true;
            }
            if (objDir != null)
            {
                _compiler.ObjectDir = objDir;
            }
            if (exeDir != null)
            {
                _compiler.ExecutableDir = exeDir;
            }
            _filesToCompile = files;
            return true;
        }

    }
}
