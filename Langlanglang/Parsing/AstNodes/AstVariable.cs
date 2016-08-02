using System;
using CIL;
using CIL.CILNodes;
using Langlanglang.TypeChecking;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstVariable : AstExpression
    {
        public string Name { get; set; }

        public AstVariable(SourceInfo si, string name)
            : base(si)
        {
            Name = name;
        }

        public override CILExpression ToCILExpression(CIntermediateLang cil)
        {
            throw new NotImplementedException();
        }

        public override LllType TryInferType(CIntermediateLang cil)
        {
            throw new NotImplementedException();
        }
    }
}
