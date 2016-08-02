using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;
using Langlanglang.Compiler;
using Langlanglang.Compiler.Exceptions;
using Langlanglang.TypeChecking;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstIdent : AstExpression
    {
        public string Name { get; set; }

        public AstIdent(SourceInfo si, string name)
            : base(si)
        {
            Name = name;
        }

        public override CILExpression ToCILExpression(CIntermediateLang cil)
        {
            var sym = LllCompiler.SymTable.LookupSymbol(Name);
            if (sym == null)
            {
                throw new UndefinedSymbolException(string.Format("Error: {0} : '{1}' is undefined.", SourceInfo, Name));
            }
            if (sym is LllStruct)
            {
                return new CILIdent(SourceInfo, "struct " + sym.CName);
            }
            return new CILIdent(SourceInfo, sym.CName);
        }

        public override LllType TryInferType(CIntermediateLang cil)
        {
            var sym = LllCompiler.SymTable.LookupSymbol(Name);
            if (sym is LllType)
            {
                return (LllType) sym;
            }
            var decl = sym.Extra as AstDeclaration;
            if (decl != null)
            {
                if (decl.Type == null)
                {
                    return null;
                }
                var cTyName = LllCompiler.SymTable.LookupType(decl.Type);
                return cTyName.Clone(decl.PointerDepth);
                //return new LllType(cTyName, decl.PointerDepth);
            }
            var func = sym.Extra as AstFunc;
            if (func != null)
            {
                var cTyName = LllCompiler.SymTable.LookupType(func.ReturnType);
                return cTyName.Clone(func.ReturnPtrDepth);
                //return new LllType(cTyName, func.ReturnPtrDepth);
            }
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
