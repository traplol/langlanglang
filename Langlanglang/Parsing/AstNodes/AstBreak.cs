using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstBreak : AstStatement
    {
        public AstBreak(SourceInfo si)
            : base(si)
        {
            
        }

        public override CILStatement ToCILStatement(CIntermediateLang cil)
        {
            throw new NotImplementedException();
        }
    }
}
