using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIL.CILNodes
{
    public class CILFunction : CILType
    {
        public CILType ReturnType { get; }
        public int ReturnPointerDepth { get; }
        public List<CILVariableDecl> Params { get; } 
        public List<CILNode> Body { get; }
        public bool IsVarArgs { get; }
        public bool HasBeenDefined { get; set; }

        internal CILFunction(SourceInfo sourceInfo, string name, CILType returnType, int returnPointerDepth, List<CILVariableDecl> @params, bool isVarArgs, List<CILNode> body) 
            : base(sourceInfo, name, false)
        {
            ReturnType = returnType;
            ReturnPointerDepth = returnPointerDepth;
            Params = @params;
            Body = body;
            IsVarArgs = isVarArgs;
            HasBeenDefined = false;
        }

        internal CILFunction(SourceInfo sourceInfo, string name, CILType returnType, int returnPointerDepth, List<CILVariableDecl> @params, bool isVarArgs) 
            : base(sourceInfo, name, false)
        {
            ReturnType = returnType;
            ReturnPointerDepth = returnPointerDepth;
            Params = @params;
            IsVarArgs = isVarArgs;
            Body = new List<CILNode>();
        }

        public void AddBodyNode(CILNode node)
        {
            Body.Add(node);
        }

        public override void Codegen(CIntermediateLang cil, IndentingStringBuilder sb)
        {
            var tmp = cil.CurrentFunction;
            cil.CurrentFunction = this;
            sb.LineDecl(SourceInfo);
            sb.AppendLine(GetDeclaration());
            sb.AppendLine("{");
            sb.Indent();
            cil.PushScope();
            foreach (var par in Params)
            {
                cil.DeclareLocalVariable(par);
            }
            foreach (var stmt in Body)
            {
                stmt.Codegen(cil, sb);
            }
            cil.PopScope();
            sb.Dedent();
            sb.AppendLine("}");
            cil.CurrentFunction = tmp;
        }

        public override string GetDeclaration()
        {
            var sb = new StringBuilder(string.Format(
                "{0}{1} {2}(",
                ReturnType,
                new string('*', ReturnPointerDepth),
                Name));
            if (Params.Count == 0 && !IsVarArgs)
            {
                sb.Append("void");
            }
            for (int i = 0; i < Params.Count; ++i)
            {
                sb.Append(Params[i].GetDeclaration());
                if (i + 1 < Params.Count)
                {
                    sb.Append(", ");
                }
            }
            if (IsVarArgs)
            {
                if (Params.Count > 0)
                {
                    sb.Append(", ");
                }
                sb.Append("...");
            }
            sb.Append(")");
            return sb.ToString();
        }

        public override string TryInferType(CIntermediateLang cil)
        {
            return ReturnType + new string('*', ReturnPointerDepth);
        }
    }
}
