using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;
using Langlanglang.Compiler;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstIf : AstStatement
    {
        public AstExpression Condition { get; set; }
        public List<AstNode> TrueBody { get; set; }
        public List<AstNode> FalseBody { get; set; }

        public AstIf(SourceInfo si, AstExpression condition, List<AstNode> trueBody, List<AstNode> falseBody)
            : base(si)
        {
            Condition = condition;
            TrueBody = trueBody ?? new List<AstNode>();
            FalseBody = falseBody ?? new List<AstNode>();
        }

        public override CILStatement ToCILStatement(CIntermediateLang cil)
        {
            var branch = new CILBranch(SourceInfo, Condition.ToCILExpression(cil));
            foreach (var n in TrueBody)
            {
                branch.AddTrueBranchStmt(n.ToCILNode(cil));
            }
            foreach (var n in FalseBody)
            {
                branch.AddFalseBranchStmt(n.ToCILNode(cil));
            }
            return branch;
        }
    }
}
