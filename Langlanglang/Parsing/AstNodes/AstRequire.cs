using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstRequire : AstNode
    {
        public AstPackageIdent Required { get; set; }
        public bool HasWildcardMatch { get; set; }

        public AstRequire(SourceInfo si, AstPackageIdent required, bool hasWildcardMatch)
            : base(si)
        {
            Required = required;
            HasWildcardMatch = hasWildcardMatch;
        }

        public override CILNode ToCILNode(CIntermediateLang cil)
        {
            throw new NotImplementedException();
        }
    }
}
