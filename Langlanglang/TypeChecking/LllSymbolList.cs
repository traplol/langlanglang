using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Langlanglang.Parsing.AstNodes;

namespace Langlanglang.TypeChecking
{
    public class LllSymbolList
    {
        public List<LllSymbol> ConcreteSymbols { get; }
        public List<LllSymbol> GenericSymbols { get; }

        public LllSymbolList()
        {
            ConcreteSymbols = new List<LllSymbol>();
            GenericSymbols = new List<LllSymbol>();
        }

        public void Add(LllSymbol symbol)
        {
            var funcSym = symbol as LllFunction;
            if (funcSym != null)
            {
                var func = funcSym.Extra as AstFunc;
                if (func.IsGeneric)
                {
                    GenericSymbols.Add(symbol);
                    return;
                }
            }
            var structSym = symbol as LllStruct;
            if (structSym != null)
            {
                var @struct = structSym.Struct;
                if (@struct.IsGeneric)
                {
                    GenericSymbols.Add(symbol);
                    return;
                }
            }
            ConcreteSymbols.Add(symbol);
        }

        public void RemoveAll(Predicate<LllSymbol> match)
        {
            ConcreteSymbols.RemoveAll(match);
            GenericSymbols.RemoveAll(match);
        }
    }
}
