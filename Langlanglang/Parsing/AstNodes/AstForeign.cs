using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;
using Langlanglang.Compiler;
using Langlanglang.TypeChecking;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstForeign : AstFunc
    {
        public string CName  { get; set; } 
        public bool IsVarArgs { get; set; }

        private static readonly Regex CNameRegex = new Regex(@"^[a-zA-Z_][a-zA-Z0-9_]*$");

        public AstForeign(SourceInfo sourceInfo, List<AstDeclaration> @params, string lllname, string cname, string returnType, int retPtrDepth, bool isVarArgs) 
            : base(sourceInfo, lllname, @params, returnType, retPtrDepth, null)
        {
            Name = lllname;
            CName = cname;
            if (!CNameRegex.IsMatch(cname))
            {
                throw new ArgumentException(string.Format("Error: {0} : foreign cname: '{1}' is not a valid C identifer.", SourceInfo, cname));
            }
            IsVarArgs = isVarArgs;
        }

        public void Decl(CIntermediateLang cil)
        {
            if (cil.SymTable.LookupSym(CName) == null)
            {
                var retType = LllCompiler.SymTable.LookupType(ReturnType);
                cil.DeclareFunction(
                    SourceInfo,
                    retType.CName,
                    ReturnPtrDepth,
                    CName,
                    Params.Select(p => p.ToCILVariableDecl(cil)).ToList(),
                    IsVarArgs);
            }
            LllCompiler.SymTable.AddSymbolAtGlobalScope(new LllSymbol(Name, Name, CName, this));
        }

        public override CILNode ToCILNode(CIntermediateLang cil)
        {
            throw new NotSupportedException();
        }
    }
}
