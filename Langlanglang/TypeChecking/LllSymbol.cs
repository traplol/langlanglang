using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Langlanglang.Parsing.AstNodes;

namespace Langlanglang.TypeChecking
{
    public class LllSymbol
    {
        public string Name { get; set; }
        public string MangledName { get; set; }
        public string CName { get; }
        public AstNode Extra { get; }

        public LllSymbol(string name, string mangledName, string cName, AstNode extra = null)
        {
            Name = name;
            MangledName = mangledName;
            CName = cName;
            Extra = extra;
        }

        public LllSymbol(LllSymbol copyFrom)
        {
            Name = copyFrom.Name;
            CName = copyFrom.CName;
            Extra = copyFrom.Extra;
        }
    }
}
