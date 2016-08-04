using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;
using CIL.SymbolStore;
using Langlanglang.Compiler;
using Langlanglang.TypeChecking;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstFunc : AstNode
    {
        public List<AstDeclaration> Params { get; set; } 
        public string Name  { get; set; } 
        public string MangledName { get; set; }
        public List<AstNode> Body { get; set; }
        public bool IsGeneric { get; set; }

        public AstType ReturnType { get; set; }

        public CILFunction CILFunction { get; protected set; }

        public AstFunc(SourceInfo si, string name, List<AstDeclaration> @params, AstType returnType, List<AstNode> body)
            : base(si)
        {
            Name = name;
            MangledName = GetMangledName(name, @params);
            Params = @params;
            Body = body;
            ReturnType = returnType;

            IsGeneric = @params.Any(p => p.IsGeneric);
            if (!IsGeneric && returnType.IsGeneric)
            {
                throw new NotImplementedException("Function cannot return generic type not defined in param list.");
            }
        }

        public virtual void CDecl(CIntermediateLang cil)
        {
            if (LllCompiler.SymTable.LookupSymbol(MangledName) != null)
            {
                var func = LllCompiler.SymTable.LookupFunction(MangledName);
                var funcAst = func.Extra as AstFunc;
                if (funcAst != null)
                {
                    CILFunction = funcAst.CILFunction;
                }
                return;
            }
            var cName = NameGenerator.UniqName("func", Name);
            LllCompiler.SymTable.AddSymbolAtGlobalScope(new LllFunction(cName, this));
            if (!IsGeneric)
            {
                var retType = ReturnType.ToLllType();
                CILFunction = cil.CreateFunction(
                    SourceInfo,
                    retType.CName,
                    retType.PointerDepth,
                    cName,
                    Params.Select(p => p.ToCILVariableDecl(cil)).ToList());
            }
        }

        public LllType GetRealReturnType()
        {
            return ReturnType.ToLllType();
        }

        public override CILNode ToCILNode(CIntermediateLang cil)
        {
            return CILFunction;
        }

        public virtual void CDefine(CIntermediateLang cil)
        {
            if (!IsGeneric && !CILFunction.HasBeenDefined)
            {
                var tmp = LllCompiler.CurrentFunction;
                LllCompiler.SymTable.Push();
                LllCompiler.CurrentFunction = this;
                for (int i = 0; i < Params.Count; ++i)
                {
                    var cPar = CILFunction.Params[i];
                    var par = Params[i];
                    LllCompiler.SymTable.AddSymbol(new LllSymbol(par.Name, par.Name, cPar.Name, par));
                }
                foreach (var n in Body)
                {
                    CILFunction.AddBodyNode(n.ToCILNode(cil));
                }
                CILFunction.HasBeenDefined = true;
                LllCompiler.SymTable.Pop();
                LllCompiler.CurrentFunction = tmp;
            }
        }

        public bool HasIntegerParams()
        {
            foreach (var par in Params)
            {
                if (par.Type.PointerDepth != 0) { continue; }
                var parType = par.Type.ToLllType();
                if (parType is LllIntegerType) return true;
            }
            return false;
        }

        protected static string GetMangledName(string name, List<AstDeclaration> @params)
        {
            var sb = new StringBuilder();
            sb.Append(name);
            foreach (var p in @params)
            {
                if (p.IsGeneric)
                {
                    sb.Append("_##");
                }
                else
                {
                    sb.Append(string.Format("_{0}{1}", p.Type, new string('p', p.Type.PointerDepth)));
                }
            }
            return sb.ToString();
        }

        public virtual AstFunc ConvertFromGenericToActual(CIntermediateLang cil, List<AstExpression> args, SourceInfo si)
        {
            if (args.Count != Params.Count)
            {
                throw new NotImplementedException("TODO: Exception for ConvertFronGenericToActual where args and pars mismatch count");
            }
            var clonedPars = Params.Select(p => p.ShallowClone()).ToList();
            var retType = ReturnType.TypeName;
            var retPtrDepth = ReturnType.PointerDepth;
            var genericTypes = new Dictionary<string, LllType>();
            for (int i = 0; i < clonedPars.Count; ++i)
            {
                var p = clonedPars[i];
                if (p.IsGeneric)
                {
                    var arg = args[i];
                    var argTy = arg.TryInferType(cil);
                    var alreadyAdded = genericTypes.ContainsKey(p.Type.TypeName);
                    if (alreadyAdded)
                    {
                        var pType = genericTypes[p.Type.TypeName];
                        if (argTy.TryCast(pType))
                        {
                            argTy = pType;
                        }
                        else
                        {
                            throw new NotImplementedException("TODO: Error for using the same generic identifier for differing argument types.");
                        }
                    }
                    if (argTy is LllIntegerType && argTy.PointerDepth == 0)
                    {
                        if (argTy.TryCast(LllIntegerType.I32))
                        {
                            argTy = LllIntegerType.I32;
                        }
                    }
                    if (!alreadyAdded)
                    {
                        //genericTypes.Add(p.Type, arg.TryInferType(cil).Clone(argTy.PointerDepth));
                        genericTypes.Add(p.Type.TypeName, arg.TryInferType(cil));
                    }
                    var newType = new AstType(p.SourceInfo, argTy.Name, argTy.PointerDepth, 0, argTy.IsAReference, false);
                    p.Type = newType;
                }
            }
            if (genericTypes.ContainsKey(retType))
            {
                var type = genericTypes[retType];
                retType = type.Name;
                retPtrDepth = type.PointerDepth;
            }
            var fixedRetType = new AstType(ReturnType.SourceInfo, retType, retPtrDepth, 0, ReturnType.IsAReference,
                false);

            // TODO: Deep copy the body.
            return new AstFunc(si, Name, clonedPars, fixedRetType, Body);
        }

        public virtual bool MatchesArgsExact(CIntermediateLang cil, List<AstExpression> args)
        {
            if (args.Count != Params.Count)
            {
                return false;
            }
            if (IsGeneric)
            {
                return false;
            }

            for (int i = 0; i < args.Count; ++i)
            {
                var par = Params[i];
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

        public virtual bool MatchesArgsWeak(CIntermediateLang cil, List<AstExpression> args)
        {
            if (args.Count != Params.Count)
            {
                return false;
            }
            var matches = true;
            for (int i = 0; matches && i < args.Count; ++i)
            {
                // for something to weakly match, the parameter needs to either be generic or
                // both the param and arg need to be integers
                var par = Params[i];
                if (par.IsGeneric) { continue; }
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
