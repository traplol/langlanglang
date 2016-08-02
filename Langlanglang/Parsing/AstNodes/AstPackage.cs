using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstPackage : AstNode
    {
        public AstPackageIdent PackageName { get; set; }

        public AstPackage(SourceInfo si, AstPackageIdent packageName)
            : base(si)
        {
            PackageName = packageName;
        }

        public override CILNode ToCILNode(CIntermediateLang cil)
        {
            throw new NotImplementedException();
        }
    }
}
