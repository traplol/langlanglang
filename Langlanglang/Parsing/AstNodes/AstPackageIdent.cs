using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstPackageIdent : AstNode
    {
        public List<string> Names { get; set; }

        public AstPackageIdent(SourceInfo si, List<string> names)
            : base(si)
        {
            Names = names;
        }

        public override CILNode ToCILNode(CIntermediateLang cil)
        {
            throw new NotImplementedException();
        }
    }
}
