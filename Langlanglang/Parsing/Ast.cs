using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;
using Langlanglang.Parsing.AstNodes;

namespace Langlanglang.Parsing
{
    public class Ast
    {
        public List<AstNode> Roots { get; }
        public List<AstNode> TopLevelStatements { get; } 
        public List<AstForeign> Foreigns { get; } 
        public List<AstFunc> Functions { get; }  
        //public List<AstFunc> GenericFunctions { get; }
        //public List<AstExtend> Extends { get; } 
        public List<AstStruct> Structs { get; }
        private CIntermediateLang _cil;

        public Ast()
        {
            _cil = new CIntermediateLang();
            Roots = new List<AstNode>();
            TopLevelStatements = new List<AstNode>();
            Foreigns = new List<AstForeign>();
            Functions = new List<AstFunc>();
            //GenericFunctions = new List<AstFunc>();
            //Extends = new List<AstExtend>();
            Structs = new List<AstStruct>();
        }

        public void CompileGeneric(CIntermediateLang cil, AstFunc func)
        {
            AddRoot(func);
            func.CDecl(cil);
            func.CDefine(cil);
        }

        public string CompileToC()
        {
            int i;
            //var bsi = new SourceInfo("<builtin>", -1, -1);
            var lll_Main = _cil.CreateFunction(new SourceInfo("<lll_Main>", -1, -1), "void", 0, "lll_Main", new List<CILVariableDecl>());

            Console.WriteLine("Translating AST into CIL representation...");
            i = 0;
            Console.Write("Generating Struct declarations... (0/{0})", Structs.Count);
            foreach (var s in Structs)
            {
                s.CDecl(_cil);
                Console.Write("\rGenerating Struct declarations... ({0}/{1})", ++i, Structs.Count);
            }
            Console.WriteLine();
            i = 0;
            Console.Write("Generating Foreign declarations... (0/{0})", Foreigns.Count);
            foreach (var f in Foreigns)
            {
                f.Decl(_cil);
                Console.Write("\rGenerating Foreign declarations... ({0}/{1})", ++i, Foreigns.Count);
            }
            Console.WriteLine();
            i = 0;
            Console.Write("Generating Function declarations... (0/{0})", Functions.Count);
            foreach (var f in Functions)
            {
                f.CDecl(_cil);
                Console.Write("\rGenerating Function declarations... ({0}/{1})", ++i, Functions.Count);
            }
            //foreach (var e in Extends)
            //{
            //    e.CDecl(cil);
            //}
            Console.WriteLine();
            i = 0;
            Console.Write("Generating Struct definitions...(0/{0})", Structs.Count);
            foreach (var s in Structs)
            {
                s.CDefine(_cil);
                Console.Write("\rGenerating Struct definitions...({0}/{1})", ++i, Structs.Count);
            }
            Console.WriteLine();
            i = 0;
            Console.Write("Generating Function definitions...({0})", Functions.Count);
            foreach (var f in Functions)
            {
                f.CDefine(_cil);
                Console.Write("\rGenerating Function definitions...({0}/{1})", ++i, Functions.Count);
            }
            Console.WriteLine();
            //foreach (var e in Extends)
            //{
            //    e.CDefine(cil);
            //}

            _cil.CurrentFunction = lll_Main;
            i = 0;
            Console.Write("Translating top level statements...(0/{0})", TopLevelStatements.Count);
            foreach (var t in TopLevelStatements)
            {
                lll_Main.AddBodyNode(t.ToCILNode(_cil));
                Console.Write("\rTranslating top level statements...({0}/{1})", ++i, TopLevelStatements.Count);
            }
            Console.WriteLine();

            var mainSi = new SourceInfo("<main>", -1, -1);
            var main = _cil.CreateFunction(mainSi, "int", 0, "main", new List<CILVariableDecl>
            {
                new CILVariableDecl(mainSi, _cil.SymTable.LookupType("int"), 0, "argc"),
                new CILVariableDecl(mainSi, _cil.SymTable.LookupType("char"), 2, "argv"),
            });
            main.AddBodyNode(new CILCall(mainSi, new CILIdent(mainSi, "lll_Main")));
            main.AddBodyNode(new CILReturn(mainSi, new CILInteger(mainSi, 0)));

            Console.WriteLine("Generating text to be written to file...");
            var code = new StringBuilder();
            code.AppendLine("#include <stddef.h>");
            code.AppendLine("#include <stdio.h>");
            code.AppendLine("#include <stdint.h>");

            code.Append(_cil);
            return code.ToString();
        }

        public void AppendAst(Ast ast)
        {
            foreach (var root in ast.Roots)
            {
                AddRoot(root);
            }
            //cil.CopyFrom(ast.cil);
        }

        public void AddRoot(AstNode node)
        {
            Roots.Add(node);
            if (node is AstForeign)
            {
                Foreigns.Add((AstForeign)node);
            }
            //else if (node is AstExtend)
            //{
            //    Extends.Add((AstExtend)node);
            //}
            else if (node is AstFunc)
            {
                AddFunc((AstFunc)node);
            }
            else if (node is AstStruct)
            {
                Structs.Add((AstStruct) node);
            }
            else
            {
                TopLevelStatements.Add(node);
            }
        }

        private void AddFunc(AstFunc func)
        {
            //if (func.IsGeneric)
            //{
            //    GenericFunctions.Add(func);
            //}
            //else
            {
                Functions.Add(func);
            }
        }
    }
}
