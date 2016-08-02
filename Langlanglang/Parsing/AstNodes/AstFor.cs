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
    public class AstFor : AstStatement
    {
        public List<AstNode> Pre { get; set; }
        public AstExpression Condition { get; set; } 
        public List<AstExpression> Update { get; set; }
        public List<AstNode> Body { get; set; }

        public AstFor(SourceInfo si, List<AstNode> pre, AstExpression condition, List<AstExpression> update, List<AstNode> body)
            : base(si)
        {
            Pre = pre;
            Condition = condition;
            Update = update;
            Body = body;
        }

        public override CILStatement ToCILStatement(CIntermediateLang cil)
        {
            LllCompiler.SymTable.Push();
            var pre = Pre.Select(p => p.ToCILNode(cil)).ToList();
            var cond = Condition.ToCILExpression(cil);
            var update = Update.Select(u => u.ToCILExpression(cil)).ToList();
            var body = Body.Select(b => b.ToCILNode(cil)).ToList();
            var loop = new CILLoop(SourceInfo, pre, cond, update, body);
            LllCompiler.SymTable.Push();
            return loop;
        }

        public override CILNode ToCILNode(CIntermediateLang cil)
        {
            return ToCILStatement(cil);
        }
    }
}
