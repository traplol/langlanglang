using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIL.CILNodes
{
    public class CILRewriteExpression : CILExpression
    {
        public List<CILNode> Nodes { get; }

        public CILRewriteExpression(SourceInfo si, List<CILNode> nodes) 
            : base(si)
        {
            Nodes = nodes;
        }

        public override string TryInferType(CIntermediateLang cil)
        {
            return Nodes.Last().TryInferType(cil);
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            foreach (var n in Nodes)
            {
                n.Codegen(cil, sb);
            }
        }
    }
}
