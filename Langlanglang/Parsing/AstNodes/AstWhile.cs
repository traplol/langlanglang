using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstWhile : AstStatement
    {
        public AstExpression Condition { get; set; }
        public List<AstNode> Body { get; set; }

        public AstWhile(SourceInfo si, AstExpression condition, List<AstNode> body)
            : base(si)
        {
            Condition = condition;
            Body = body;
        }

        public override CILStatement ToCILStatement(CIntermediateLang cil)
        {
            var loop = new CILLoop(SourceInfo, Condition.ToCILExpression(cil));
            loop.Body.AddRange(Body.Select(n => n.ToCILNode(cil)));
            return loop;
        }
    }
}
