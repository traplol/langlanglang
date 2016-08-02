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
    public class AstIndex : AstExpression
    {
        public AstExpression From { get; set; }
        public List<AstExpression> Subscript { get; set; }

        public AstIndex(SourceInfo si, AstExpression @from, List<AstExpression> subscript)
            : base(si)
        {
            From = @from;
            Subscript = subscript;
        }

        public override CILExpression ToCILExpression(CIntermediateLang cil)
        {
            if (Subscript.Count != 1)
            {
                throw new NotImplementedException();
            }
            return new CILIndexAccess(
                SourceInfo, 
                From.ToCILExpression(cil), 
                Subscript[0].ToCILExpression(cil));
        }

        public override LllType TryInferType(CIntermediateLang cil)
        {
            var ty = From.TryInferType(cil);
            return ty.Clone(ty.PointerDepth - 1);
        }
    }
}
