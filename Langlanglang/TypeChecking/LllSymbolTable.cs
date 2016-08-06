using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Langlanglang.Parsing.AstNodes;
using Langlanglang.TypeChecking.Exceptions;

namespace Langlanglang.TypeChecking
{
    public class LllSymbolTable
    {
        private readonly Stack<Dictionary<string, LllSymbol>> _lllUniqueSymbols;
        private readonly Stack<Dictionary<string, LllSymbolList>> _lllSymbols;
        private readonly Dictionary<string, string> _aliases;

        private LllSymbolTable()
        {
            _aliases = new Dictionary<string, string>();
            _lllSymbols = new Stack<Dictionary<string, LllSymbolList>>();
            _lllUniqueSymbols = new Stack<Dictionary<string, LllSymbol>>();
            Push();
        }

        public void Push()
        {
            _lllSymbols.Push(new Dictionary<string, LllSymbolList>());
            _lllUniqueSymbols.Push(new Dictionary<string, LllSymbol>());
        }

        public void Pop()
        {
            _lllSymbols.Pop();
            _lllUniqueSymbols.Pop();
        }

        public void AddSymbol(LllSymbol symbol)
        {
            var uniqBot = _lllUniqueSymbols.Peek();
            if (uniqBot.ContainsKey(symbol.MangledName))
            {
                var sym = uniqBot[symbol.MangledName];
                var msg = string.Format(
                    "Error: {0} : Symbol `{1}' already declared at {2}.", 
                    symbol.Extra.SourceInfo, symbol.Name, sym.Extra.SourceInfo);
                throw new SymbolAlreadyDeclaredException(msg);
            }
            uniqBot.Add(symbol.MangledName, symbol);
            var top = _lllSymbols.Peek();
            if (top.ContainsKey(symbol.Name))
            {
                top[symbol.Name].Add(symbol);
            }
            else
            {
                var symList = new LllSymbolList();
                symList.Add(symbol);
                top.Add(symbol.Name, symList);
            }
        }

        public void AddSymbolAtGlobalScope(LllSymbol symbol)
        {
            _lllUniqueSymbols.Last().Add(symbol.MangledName, symbol);
            var bottom = _lllSymbols.Peek();
            if (bottom.ContainsKey(symbol.Name))
            {
                bottom[symbol.Name].Add(symbol);
            }
            else
            {
                var symList = new LllSymbolList();
                symList.Add(symbol);
                bottom.Add(symbol.Name, symList);
            }
        }

        public void SetSymbolAtGlobalScope(LllSymbol symbol)
        {
            _lllUniqueSymbols.Last()[symbol.MangledName] = symbol;
            var bottom = _lllSymbols.Peek();
            if (bottom.ContainsKey(symbol.Name))
            {
                var withSharedName = bottom[symbol.Name];
                withSharedName.RemoveAll(sym => sym.MangledName == symbol.MangledName);
                withSharedName.Add(symbol);
            }
            else
            {
                var symList = new LllSymbolList();
                symList.Add(symbol);
                bottom.Add(symbol.Name, symList);
            }
        }

        public void AddAlias(string alias, string aliasTo)
        {
            _aliases.Add(alias, aliasTo);
        }

        public string ResolveAlias(string alias)
        {
            if (_aliases.ContainsKey(alias))
            {
                return _aliases[alias];
            }
            return alias;
        }

        public List<LllSymbol> WithName(string name)
        {
            foreach (var dict in _lllSymbols)
            {
                if (dict.ContainsKey(name))
                {
                    return dict[name].ConcreteSymbols;
                }
            }
            return new List<LllSymbol>();
        }

        public List<LllSymbol> Generics(string name)
        {
            foreach (var dict in _lllSymbols)
            {
                if (dict.ContainsKey(name))
                {
                    return dict[name].GenericSymbols;
                }
            }
            return new List<LllSymbol>();
        } 

        public LllSymbol LookupSymbol(string name)
        {
            name = ResolveAlias(name);
            foreach (var dict in _lllUniqueSymbols)
            {
                if (dict.ContainsKey(name))
                {
                    return dict[name];
                }
            }
            return null;
        }

        public LllStruct LookupStruct(string name)
        {
            var s = LookupSymbol(name) as LllStruct;
            if (s == null)
            {
                throw new NotImplementedException("TODO: Symbol not a struct...");
            }
            return s;
        }

        public LllFunction LookupFunction(string name)
        {
            var s = LookupSymbol(name) as LllFunction;
            if (s == null)
            {
                throw new NotImplementedException("TODO: Symbol not a function...");
            }
            return s;
        }

        public LllType LookupType(string name)
        {
            var s = LookupSymbol(name) as LllType;
            if (s == null)
            {
                throw new NotImplementedException("TODO: Symbol not a type...");
            }
            return s;
        }

        public static LllSymbolTable CreateBasic()
        {
            var st = new LllSymbolTable();
            st.AddSymbol(LllIntegerType.Char);

            st.AddSymbol(LllIntegerType.I8);
            st.AddSymbol(LllIntegerType.I16);
            st.AddSymbol(LllIntegerType.I32);
            st.AddSymbol(LllIntegerType.I64);

            st.AddSymbol(LllIntegerType.U8);
            st.AddSymbol(LllIntegerType.U16);
            st.AddSymbol(LllIntegerType.U32);
            st.AddSymbol(LllIntegerType.U64);

            st.AddSymbol(new LllType("float", "float"));
            st.AddSymbol(new LllType("double", "double"));

            st.AddSymbol(new LllType("void", "void"));

            st.AddAlias("short", "i16");
            st.AddAlias("int", "i32");
            st.AddAlias("long", "i64");
            st.AddAlias("size", "u64");
            st.AddAlias("bool", "i8");

            return st;
        }
    }
}
