using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;
using Langlanglang.TypeChecking;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstAssign : AstExpression
    {
        public AstExpression Destination { get; set; }
        public AstExpression Expression { get; set; }

        public AstAssign(SourceInfo si, AstExpression destination, AstExpression expression) 
            : base(si)
        {
            Destination = destination;
            Expression = expression;
        }

        public override CILExpression ToCILExpression(CIntermediateLang cil)
        {
            return new CILAssignment(
                SourceInfo, 
                Destination.ToCILExpression(cil), 
                Expression.ToCILExpression(cil));
        }

        public override LllType TryInferType(CIntermediateLang cil)
        {
            throw new NotImplementedException();
        }
    }
}
