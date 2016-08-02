using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL.SymbolStore;

namespace CIL.CILNodes
{
    public class CILCall : CILExpression
    {
        public CILExpression Callee { get; }
        public List<CILExpression> Args { get; }

        public CILCall(SourceInfo si, CILExpression callee) : base(si)
        {
            Callee = callee;
            Args = new List<CILExpression>();
        }

        public CILCall(SourceInfo si, CILExpression callee, List<CILExpression> args)
            : base(si)
        {
            Callee = callee;
            Args = args;
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            var argTmps = new List<string>();
            foreach (var arg in Args)
            {
                arg.Codegen(cil, sb);
                argTmps.Add(cil.LastUsedVar);
            }
            Callee.Codegen(cil, sb);

            // This doesn't account for:
            //      calling a returned function pointer
            //      calling from an array subscript
            //      calling a member
            var callee = cil.SymTable.LookupFunction(cil.LastUsedVar);
            var callRetType = callee.TryInferType(cil);
            sb.LineDecl(SourceInfo);
            if (callRetType == "void")
            {
                sb.Append(string.Format("{0}(", callee.Name));
                cil.LastUsedVar = null;
            }
            else
            {
                var tmp = NameGenerator.NewTemp();
                sb.Append(string.Format(
                    "{0} {1} = {2}(",
                    callee.TryInferType(cil),
                    tmp,
                    callee.Name));
                cil.LastUsedVar = tmp;
            }
            for (int i = 0; i < argTmps.Count; ++i)
            {
                sb.AppendNoIndent(argTmps[i]);
                if (i + 1 < argTmps.Count)
                {
                    sb.AppendNoIndent(", ");
                }
            }
            sb.AppendLineNoIndent(");");
        }

        public override string TryInferType(CIntermediateLang cil)
        {
            return Callee.TryInferType(cil);
        }
    }
}
