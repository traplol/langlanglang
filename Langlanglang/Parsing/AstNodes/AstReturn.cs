using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;
using Langlanglang.Compiler;
using Langlanglang.TypeChecking;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstReturn : AstStatement
    {
        public AstExpression ExpressionToReturn { get; set; }

        public AstReturn(SourceInfo si, AstExpression expressionToReturn)
            : base(si)
        {
            ExpressionToReturn = expressionToReturn;
        }

        public override CILStatement ToCILStatement(CIntermediateLang cil)
        {
            if (LllCompiler.CurrentFunction.ReturnType.IsAReference)
            {
                return new CILReturn(SourceInfo,
                    new CILReference(SourceInfo, ExpressionToReturn?.ToCILExpression(cil)));
            }
            return new CILReturn(SourceInfo, ExpressionToReturn?.ToCILExpression(cil));
        }
    }
}
