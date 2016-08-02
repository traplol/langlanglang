using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Langlanglang.Parsing.AstNodes;

namespace Langlanglang.TypeChecking
{
    public class LllFunction : LllSymbol
    {
        public LllFunction(string cName, AstFunc functionAst)
            : base(functionAst.Name, functionAst.MangledName, cName, functionAst)
        {
        }
    }
}
