using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.Exceptions;
using Langlanglang.Parsing;
using Langlanglang.Parsing.AstNodes;
using Langlanglang.Semantics;
using Langlanglang.Tokenization;
using Langlanglang.TypeChecking;

namespace Langlanglang.Compiler
{
    public class LllCompiler
    {
        private readonly Tokenizer _tokenizer;

        public static Ast Ast { get; private set; }
        public static readonly LllSymbolTable SymTable;

        private static readonly Ast _program;

        static LllCompiler()
        {
            SymTable = LllSymbolTable.CreateBasic();

            var tokenizer = Tokenizer.CreateBasic();
            tokenizer.FilePath = "<builtin>";
            var foreigns = new StringBuilder();
            foreigns.AppendLine("foreign func alloc as 'malloc' (size: size) -> void*;");
            foreigns.AppendLine("foreign func free as 'free' (ptr: void*);");
            foreigns.AppendLine("foreign func sizeof as 'sizeof' () -> size;");
            foreigns.AppendLine("foreign func printf as 'printf' (fmt: char*, ...) -> i32;");
            var foreignsTks = tokenizer.Parse(foreigns.ToString());
            var foreignsTs = new TokenStream(foreignsTks);
            _program = AstParser.Parse(foreignsTs);
        }

        public LllCompiler()
        {
            _tokenizer = Tokenizer.CreateBasic();

        }

        public string CompileString(string source)
        {
            var sw = new Stopwatch();
            Console.WriteLine("Parsing sources into tokens...");
            sw.Start();
            var tks = _tokenizer.Parse(source);
            sw.Stop();
            Console.WriteLine("Parsed to tokens in {0}ms", sw.ElapsedMilliseconds);
            var ts = new TokenStream(tks);
            Console.WriteLine("Parsing tokens into AST...");
            sw.Restart();
            Ast = AstParser.Parse(ts);
            sw.Stop();
            Console.WriteLine("Parsed to AST in {0}ms", sw.ElapsedMilliseconds);
            SemanticAnalyzer.Analyze(Ast);
            TypeChecker.Check(Ast);
            _program.AppendAst(Ast);
            return _program.CompileToC();
        }

        public string CompileFile(string filename)
        {
            _tokenizer.FilePath = filename;
            try
            {
                var sw = new Stopwatch();
                Console.WriteLine("Reading file {0}...", filename);
                sw.Start();
                var source = File.ReadAllText(filename);
                sw.Stop();
                Console.WriteLine("Read whole source in {0}ms", sw.ElapsedMilliseconds);
                return CompileString(source);
            }
            catch (LllException ex)
            {
                Console.WriteLine("LLLEX: " + ex.Message);
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                Console.WriteLine("{0} : {1}:{2}", frame.GetFileName(), frame.GetFileLineNumber(), frame.GetFileColumnNumber());
            }
            catch (CILException ex)
            {
                Console.WriteLine("CILEX: " + ex.Message);
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                Console.WriteLine("{0} : {1}:{2}", frame.GetFileName(), frame.GetFileLineNumber(), frame.GetFileColumnNumber());
            }
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
            return "";
        }
    }
}
