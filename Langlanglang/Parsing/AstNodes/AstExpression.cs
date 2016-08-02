using CIL;
using CIL.CILNodes;
using Langlanglang.TypeChecking;

namespace Langlanglang.Parsing.AstNodes
{
    public abstract class AstExpression : AstNode
    {
        protected AstExpression(SourceInfo si)
            : base(si)
        {
            
        }

        public abstract LllType TryInferType(CIntermediateLang cil);
        public abstract CILExpression ToCILExpression(CIntermediateLang cil);

        public override CILNode ToCILNode(CIntermediateLang cil)
        {
            return ToCILExpression(cil);
        }
    }
}
