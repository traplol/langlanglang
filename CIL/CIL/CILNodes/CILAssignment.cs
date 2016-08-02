using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIL.CILNodes
{
    public class CILAssignment : CILExpression
    {
        public CILExpression Destination { get; }
        public CILExpression Value { get; }

        public CILAssignment(SourceInfo si, CILExpression destination, CILExpression value) 
            : base(si)
        {
            Destination = destination;
            Value = value;
        }

        public override string TryInferType(CIntermediateLang cil)
        {
            return Value.TryInferType(cil);
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            Value.Codegen(cil, sb);
            var valTmp = cil.LastUsedVar;
            Destination.Codegen(cil, sb);
            sb.LineDecl(SourceInfo);
            sb.AppendLine(string.Format("{0} = {1};", cil.LastUsedVar, valTmp));
        }
    }
}
