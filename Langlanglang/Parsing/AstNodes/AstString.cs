using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;
using Langlanglang.Compiler;
using Langlanglang.TypeChecking;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstString : AstExpression
    {
        public string String { get; set; }

        public AstString(SourceInfo si, string s)
            : base(si)
        {
            String = s.Length > 2 ? s.Substring(1, s.Length - 2) : "";
        }

        public override CILExpression ToCILExpression(CIntermediateLang cil)
        {
            return new CILStringLiteral(SourceInfo, String);
        }

        public override LllType TryInferType(CIntermediateLang cil)
        {
            return new LllType(LllCompiler.SymTable.LookupType("char"), 1);
        }
    }
}
