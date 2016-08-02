using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL.CILNodes;
using CIL.SymbolStore;

namespace CIL
{
    public class CIntermediateLang
    {

        public SymbolTable SymTable { get; }
        private readonly Dictionary<string, CILStruct> _structs;
        private readonly Dictionary<string, CILFunction> _functions;
        private readonly Dictionary<string, CILVariableDecl> _globals;

        internal string LastUsedVar { get; set; }
        public CILFunction CurrentFunction { get; set; }

        public CIntermediateLang()
        {
            SymTable = SymbolTable.CreateBasic();
            _structs = new Dictionary<string, CILStruct>();
            _functions = new Dictionary<string, CILFunction>();
            _globals = new Dictionary<string, CILVariableDecl>();
//            _structs = new List<CILStruct>();
//            _functions = new List<CILFunction>();
//            _globals = new List<CILVariableDecl>();
        }

        public void PushScope()
        {
            SymTable.Push();
        }

        public void PopScope()
        {
            SymTable.Pop();
        }

        public override string ToString()
        {
            var structDecls = new StringBuilder();
            var structDefs = new IndentingStringBuilder();
            foreach (var s in _structs)
            {
                structDecls.AppendLine(string.Format("{0};", s.Value.GetDeclaration()));
                s.Value.Codegen(this, structDefs);
            }

            var funcDecls = new StringBuilder();
            var funcDefs = new IndentingStringBuilder();
            foreach (var f in _functions)
            {
                funcDecls.AppendLine(string.Format("{0};", f.Value.GetDeclaration()));
                f.Value.Codegen(this, funcDefs);
            }

            foreach (var g in _globals)
            {
                throw new NotImplementedException();
            }

            var final = new StringBuilder(structDecls.Length + structDefs.Length + funcDecls.Length + funcDefs.Length);
            final.AppendLine(structDecls.ToString());
            final.AppendLine(funcDecls.ToString());

            final.AppendLine(structDefs.ToString());
            final.AppendLine(funcDefs.ToString());
            return final.ToString();
        }

        public void DeclareLocalVariable(CILVariableDecl variable)
        {
            SymTable.DeclareVar(variable);
        }

        public CILVariableDecl CreateGlobalVar(SourceInfo si, CILType type, int pointerDepth, string name)
        {
            var global = new CILVariableDecl(si, type, pointerDepth, name);
            AddGlobal(global);
            return global;
        }

        public CILVariableDecl CreateStaticGlobal(SourceInfo si, CILType type, int pointerDepth, string name)
        {
            var global = new CILVariableDecl(si, type, pointerDepth, name, true);
            AddGlobal(global);
            return global;
        }

        public CILStruct CreateStruct(SourceInfo si, string name)
        {
            var @struct = new CILStruct(si, name, new List<CILVariableDecl>());
            AddStruct(@struct);
            return @struct;
        }

        public CILFunction CreateFunction(SourceInfo si, string returnType, int returnPointerDepth, string name,
            List<CILVariableDecl> @params)
        {
            var retType = SymTable.LookupType(returnType);
            var func = new CILFunction(si, name, retType, returnPointerDepth, @params, false);
            AddFunction(func);
            return func;
        }

        public CILFunction DeclareFunction(SourceInfo si, string returnType, int returnPointerDepth, string name,
            List<CILVariableDecl> @params, bool isVarArgs)
        {
            var retType = SymTable.LookupType(returnType);
            var func = new CILFunction(si, name, retType, returnPointerDepth, @params, isVarArgs);
            SymTable.DeclareFunction(func);
            return func;
        }

        public void AddFunction(CILFunction function)
        {
            SymTable.DeclareFunction(function);
            _functions.Add(function.Name, function);
        }

        public void AddStruct(CILStruct @struct)
        {
            SymTable.DeclareStruct(@struct);
            _structs.Add(@struct.Name, @struct);
        }

        public void AddGlobal(CILVariableDecl global)
        {
            SymTable.DeclareGlobalVar(global);
            _globals.Add(global.Name, global);
        }
    }
}
