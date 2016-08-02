using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;
using CIL;
using CIL.CILNodes;
using Langlanglang.Compiler;

namespace Langlanglang.Parsing.AstNodes
{
    public abstract class AstNode
    {
        public SourceInfo SourceInfo { get; set; }

        protected AstNode(SourceInfo sourceInfo)
        {
            SourceInfo = sourceInfo;
        }

        public abstract CILNode ToCILNode(CIntermediateLang cil);
    }
}
