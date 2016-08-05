using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL.Exceptions;
using CIL.SymbolStore;

namespace CIL.CILNodes
{
    public class CILReturn : CILStatement
    {
        public CILExpression ReturnExpression { get; }

        public CILReturn(SourceInfo si, CILExpression returnExpression) 
            : base(si)
        {
            ReturnExpression = returnExpression;
        }

        public CILReturn(SourceInfo si)
            : base(si)
        {
            ReturnExpression = null;
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            if (cil.CurrentFunction == null)
            {
                throw new NotImplementedException("TODO: Exception for return not in function");
            }
            var funcType = cil.CurrentFunction.TryInferType(cil);
            if (ReturnExpression == null)
            {
                if (funcType != "void")
                {
                    throw new CILTypeMismatchException(SourceInfo, funcType, "void");
                }
                sb.LineDecl(SourceInfo);
                sb.AppendLine("return;");
            }
            else
            {
                var retType = ReturnExpression.TryInferType(cil);
                if ((!CTypes.IsIntegerType(retType) && !CTypes.IsIntegerType(funcType)) 
                    && retType != funcType)
                {
                    throw new CILTypeMismatchException(SourceInfo, funcType, retType);
                }
                ReturnExpression.Codegen(cil, sb);
                sb.LineDecl(SourceInfo);
                sb.AppendLine(string.Format("return {0};", cil.LastUsedVar));
            }
        }
    }
}
