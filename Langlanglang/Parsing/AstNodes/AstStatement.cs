using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;

namespace Langlanglang.Parsing.AstNodes
{
    public abstract class AstStatement : AstNode
    {
        protected AstStatement(SourceInfo si)
            : base(si)
        {
            
        }

        public abstract CILStatement ToCILStatement(CIntermediateLang cil);

        public override CILNode ToCILNode(CIntermediateLang cil)
        {
            return ToCILStatement(cil);
        }
    }
}
