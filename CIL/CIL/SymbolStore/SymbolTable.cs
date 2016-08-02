using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CIL.CILNodes;

namespace CIL.SymbolStore
{
    public class SymbolTable
    {
        private readonly Stack<Dictionary<string, CILNode>> _symTable;
        private readonly Stack<Dictionary<string, string>> _aliases; 

        private SymbolTable()
        {
            _symTable = new Stack<Dictionary<string, CILNode>>();
            _aliases = new Stack<Dictionary<string, string>>();
            Push();
        }

        public void Push()
        {
            _symTable.Push(new Dictionary<string, CILNode>());
            _aliases.Push(new Dictionary<string, string>());
        }

        public void Pop()
        {
            _symTable.Pop();
            _aliases.Pop();
        }

        public void DeclareSym(string name, CILType type)
        {
            _symTable.Peek().Add(name, type);
        }

        public void DeclareGlobalSym(string name, CILType type)
        {
            _symTable.Last().Add(name, type);
        }

        public void DeclareStruct(CILStruct @struct)
        {
            DeclareSym(@struct.Name, @struct);
            AliasSym("struct " + @struct.Name, @struct.Name);
        }

        public void DeclareFunction(CILFunction function)
        {
            DeclareSym(function.Name, function);
        }

        public void DeclareVar(CILVariableDecl variable)
        {
            _symTable.Peek().Add(variable.Name, variable);
        }

        public void DeclareGlobalVar(CILVariableDecl variable)
        {
            _symTable.Last().Add(variable.Name, variable);
        }

        public CILVariableDecl LookupVariable(string name)
        {
            var variable = LookupSym(name) as CILVariableDecl;
            if (variable == null)
            {
                throw new NotImplementedException("TODO: Exception for variable lookup not returning a variable");
            }
            return variable;
        }

        public CILType LookupType(string name)
        {
            var type = LookupSym(name) as CILType;
            if (type == null)
            {
                throw new NotImplementedException("TODO: Exception for type lookup not returning a type");
            }
            return type;
        }

        public CILStruct LookupStruct(string name)
        {
            name = name.Replace("*", "");
            var @struct = LookupSym(name) as CILStruct;
            if (@struct == null)
            {
                throw new NotImplementedException("TODO: Exception for struct lookup not returning a struct");
            }
            return @struct;
        }

        public CILFunction LookupFunction(string name)
        {
            var func = LookupSym(name) as CILFunction;
            if (func == null)
            {
                throw new NotImplementedException("TODO: Exception for function lookup not returning a function");
            }
            return func;
        }

        public CILNode LookupSym(string name)
        {
            name = ResolveAlias(name);
            foreach (var dict in _symTable)
            {
                if (dict.ContainsKey(name))
                {
                    return dict[name];
                }
            }
            return null;
        }

        public string ResolveAlias(string alias)
        {
            foreach (var dict in _aliases)
            {
                if (dict.ContainsKey(alias))
                {
                    return dict[alias];
                }
            }
            return alias;
        }

        public void AliasSym(string name, string aliasTo)
        {
            _aliases.Peek().Add(name, aliasTo);
        }

        public static SymbolTable CreateBasic()
        {
            var st = new SymbolTable();
            var bsi = new SourceInfo("<builtin>", -1, -1);
            st.DeclareSym("int8_t", new CILType(bsi, "int8_t", true));
            st.DeclareSym("int16_t", new CILType(bsi, "int16_t", true));
            st.DeclareSym("int32_t", new CILType(bsi, "int32_t", true));
            st.DeclareSym("int64_t", new CILType(bsi, "int64_t", true));

            st.DeclareSym("uint8_t", new CILType(bsi, "uint8_t", true));
            st.DeclareSym("uint16_t", new CILType(bsi, "uint16_t", true));
            st.DeclareSym("uint32_t", new CILType(bsi, "uint32_t", true));
            st.DeclareSym("uint64_t", new CILType(bsi, "uint64_t", true));

            st.DeclareSym("size_t", new CILType(bsi, "size_t", true));

            st.DeclareSym("char", new CILType(bsi, "char", true));
            st.DeclareSym("short", new CILType(bsi, "short", true));
            st.DeclareSym("int", new CILType(bsi, "int", true));
            st.DeclareSym("float", new CILType(bsi, "float", true));
            st.DeclareSym("double", new CILType(bsi, "double", true));

            st.DeclareSym("void", new CILType(bsi, "void", true));
            return st;
        }
    }
}
