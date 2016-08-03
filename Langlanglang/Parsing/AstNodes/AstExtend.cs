using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.SymbolStore;
using Langlanglang.Compiler;
using Langlanglang.TypeChecking;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstExtend : AstFunc
    {
        public string Extends { get; set; }
        public bool UsesThisPtr { get; set; }
        public bool CanOverride { get; set; }

        public AstExtend(SourceInfo si, string extends, string name, bool usesThisPtr, List<AstDeclaration> @params, string returnType, int retPtrDepth, List<AstNode> body)
            : base(si, string.Format("{0}_{1}", extends, name), @params, returnType, retPtrDepth, body)
        {
            Extends = extends;
            UsesThisPtr = usesThisPtr;
            CanOverride = false;
        }

        public override void CDecl(CIntermediateLang cil)
        {
            if (LllCompiler.SymTable.LookupSymbol(MangledName) != null)
            {
                var ext = LllCompiler.SymTable.LookupFunction(MangledName);
                var extAst = ext.Extra as AstExtend;
                if (extAst != null && !extAst.CanOverride)
                {
                    CILFunction = extAst.CILFunction;
                    return;
                }
            }
            var cName = NameGenerator.UniqName("ext", Name);
            var extending = LllCompiler.SymTable.LookupType(Extends);
            AstExtend overriding = null;
            if (extending.Extensions.ContainsKey(MangledName))
            {
                overriding = extending.Extensions[MangledName];
            }
            if (overriding != null && overriding.CanOverride)
            {
                LllCompiler.SymTable.SetSymbolAtGlobalScope(new LllFunction(cName, this));
                extending.Extensions[MangledName] = this;
            }
            else
            {
                LllCompiler.SymTable.AddSymbolAtGlobalScope(new LllFunction(cName, this));
                extending.Extensions.Add(MangledName, this);
            }
            if (!IsGeneric)
            {
                var retType = LllCompiler.SymTable.LookupType(ReturnType);
                if (extending.IsPrimitive && UsesThisPtr)
                {
                    Params[0].PointerDepth = 0;
                }
                CILFunction = cil.CreateFunction(
                    SourceInfo,
                    retType.CName,
                    ReturnPtrDepth,
                    cName,
                    Params.Select(p => p.ToCILVariableDecl(cil)).ToList());
            }
        }

        public override AstFunc ConvertFromGenericToActual(CIntermediateLang cil, List<AstExpression> args, SourceInfo si)
        {
            AstFunc tmpFunc;
            if (!UsesThisPtr)
            {
                tmpFunc = base.ConvertFromGenericToActual(cil, args, si);
            }
            else
            {
                // hacky way to deal with thisptr...
                var thisptr = Params[0];
                Params.RemoveAt(0);
                tmpFunc = base.ConvertFromGenericToActual(cil, args, si);
                tmpFunc.Params.Insert(0, thisptr);
                Params.Insert(0, thisptr);
            }
            var actualFunc = new AstExtend(
                SourceInfo,
                Extends,
                Name,
                UsesThisPtr,
                tmpFunc.Params,
                tmpFunc.ReturnType,
                tmpFunc.ReturnPtrDepth,
                tmpFunc.Body)
            {
                Name = Name,
                MangledName = GetMangledName(Name, tmpFunc.Params),
                CILFunction = tmpFunc.CILFunction,
            };
            return actualFunc;
        }

        public override bool MatchesArgsExact(CIntermediateLang cil, List<AstExpression> args)
        {
            if (!UsesThisPtr)
            {
                return base.MatchesArgsExact(cil, args);
            }
            if (Params.Count != args.Count+1)
            {
                return false;
            }
            if (IsGeneric)
            {
                return false;
            }

            for (int i = 0; i < args.Count; ++i)
            {
                // +1 to ignore thisptr
                var par = Params[i+1];
                var arg = args[i];
                var pTy = par.GetRealType();
                var aTy = arg.TryInferType(cil);
                if (!pTy.Equals(aTy))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool MatchesArgsWeak(CIntermediateLang cil, List<AstExpression> args)
        {
            if (!UsesThisPtr)
            {
                return base.MatchesArgsWeak(cil, args);
            }
            if (Params.Count != args.Count+1)
            {
                return false;
            }
            var matches = true;
            for (int i = 0; matches && i < args.Count; ++i)
            {
                // for something to weakly match, the parameter needs to either be generic or
                // both the param and arg need to be integers
                var par = Params[i+1];
                if (par.IsGenericlyTyped) { continue; }
                var arg = args[i];
                var argType = arg.TryInferType(cil);
                var parType = par.GetRealType();
                var argType_ = argType as LllIntegerType;
                var parType_ = parType as LllIntegerType;
                if (argType_ != null && parType_ != null)
                {
                    matches = argType_.PointerDepth == 0 && parType_.PointerDepth == 0;
                    continue;
                }
                matches = argType.Equals(parType);
            }
            return matches;
        }
    }
}
