using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIL;
using CIL.CILNodes;
using Langlanglang.Compiler;
using Langlanglang.Compiler.Exceptions;
using Langlanglang.TypeChecking;
using Microsoft.CSharp.RuntimeBinder;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstCall : AstExpression
    {
        public AstExpression Callee { get; set; }
        public List<AstExpression> Args { get; set; }

        public AstCall(SourceInfo si, AstExpression callee, List<AstExpression> args)
            : base(si)
        {
            Callee = callee;
            Args = args;
        }

        public override CILExpression ToCILExpression(CIntermediateLang cil)
        {
            var args = Args.Select(a => a.ToCILExpression(cil)).ToList();
            var ident = Callee as AstIdent;
            if (ident != null)
            {
                ident.Name = FixIdent(cil, ident.Name);
                var sym = LllCompiler.SymTable.LookupSymbol(ident.Name) as LllFunction;
                var func = sym?.Extra as AstFunc;
                if (func != null && func.IsGeneric)
                {
                    var actual = CompileGeneric(cil, func);
//                    var actual = func.ConvertFromGenericToActual(cil, Args, SourceInfo);
//                    LllCompiler.Ast.CompileGeneric(actual);
                    return new CILCall(SourceInfo, new CILIdent(SourceInfo, actual.CILFunction.Name), args);
                }
            }

            var extend = Callee as AstMemberAccess;
            if (extend != null)
            {
                var fromTy = extend.From.TryInferType(cil);
                var member = extend.MemberIdent;
                var extName = FixIdent(cil, string.Format("{0}_{1}", fromTy.Name, member));
                if (fromTy.Extensions.ContainsKey(extName))
                {
                    var ext = fromTy.Extensions[extName];
                    if (ext.UsesThisPtr)
                    {
                        args.Insert(0, extend.From.ToCILExpression(cil));
                    }
                    if (ext.IsGeneric)
                    {
                        //ext = ext.ConvertFromGenericToActual(cil, Args, SourceInfo) as AstExtend;
                        //LllCompiler.Ast.CompileGeneric(ext);
                        ext = CompileGeneric(cil, ext) as AstExtend;
                    }
                    var extId = new AstIdent(SourceInfo, ext.MangledName);
                    return new CILCall(SourceInfo, extId.ToCILExpression(cil), args);
                }
                throw new UndefinedSymbolException(
                    string.Format("Error: {0} : type '{1}' does not have extension '{2}'", 
                        SourceInfo, fromTy, member));
            }


            var callee = Callee.ToCILExpression(cil);
            var callee_ = callee as CILCall;
            if (callee_ != null)
            {
                callee_.Args.AddRange(args);
                return callee_;
            }
            return new CILCall(SourceInfo, callee, args);
        }

        public override LllType TryInferType(CIntermediateLang cil)
        {
            var extend = Callee as AstMemberAccess;
            if (extend != null)
            {
                var fromTy = extend.From.TryInferType(cil);
                var member = extend.MemberIdent;
                var extName = FixIdent(cil, string.Format("{0}_{1}", fromTy.Name, member));
                if (fromTy.Extensions.ContainsKey(extName))
                {
                    var ext = fromTy.Extensions[extName] as AstFunc;
                    if (ext.IsGeneric)
                    {
                        ext = CompileGeneric(cil, ext);
                    }
                    return LllCompiler.SymTable.LookupType(ext.ReturnType).Clone(ext.ReturnPtrDepth);
                }
            }
            var ident = Callee as AstIdent;
            if (ident != null)
            {
                var fixedIdent = FixIdent(cil, ident.Name);
                var func = LllCompiler.SymTable.LookupSymbol(fixedIdent)?.Extra as AstFunc;
                if (func != null)
                {
                    if (func.IsGeneric)
                    {
                        var compiled = CompileGeneric(cil, func);
                        return compiled.GetRealReturnType();
                    }
                    else
                    {
                        return func.GetRealReturnType();
                    }
                }
            }
            return Callee.TryInferType(cil);
        }

        private AstFunc CompileGeneric(CIntermediateLang cil, AstFunc func)
        {
            var compiled = func.ConvertFromGenericToActual(cil, Args, SourceInfo);
            LllCompiler.Ast.CompileGeneric(cil, compiled);
            return compiled;
        }

        public string FixIdent(CIntermediateLang cil, string ident)
        {
            // check againt concrete symbols
            var concretWithThisName = LllCompiler.SymTable.WithName(ident);
            var match = MatchAgainst(cil, concretWithThisName);
            if (match != null)
            {
                return match;
            }
            // maybe a generic exists?
            var genericsWithThisName = LllCompiler.SymTable.Generics(ident);
            match = MatchAgainst(cil, genericsWithThisName);
            if (match != null)
            {
                return match;
            }

            var sb = new StringBuilder();
            sb.Append(ident);
            foreach (var a in Args)
            {
                var t = a.TryInferType(cil);
                sb.Append(string.Format("_{0}{1}", t.Name, new string('p', t.PointerDepth)));
            }
            if (LllCompiler.SymTable.LookupSymbol(sb.ToString()) != null)
            {
                return sb.ToString();
            }
            return ident;
        }

        private string MatchAgainst(CIntermediateLang cil, List<LllSymbol> symbols)
        {
            var viableFunctions = new List<AstFunc>();
            foreach (var sym in symbols)
            {
                var lllfunc = sym as LllFunction;
                var func = lllfunc?.Extra as AstFunc;
                if (func == null) { continue; }
                // Count +- 1 accounts for the thisptr
                //if (func.Params.Count < Args.Count - 1 || func.Params.Count > Args.Count + 1) { continue; } 
                //if (CheckAsExtendThenFunc(cil, func))
                if (func.MatchesArgsExact(cil, Args))
                {
                    return func.MangledName;
                }
                if (func.MatchesArgsWeak(cil, Args))
                {
                    viableFunctions.Add(func);
                }
            }
            if (viableFunctions.Count == 1)
            {
                return viableFunctions[0].MangledName;
            }
            if (viableFunctions.Count > 1)
            {
                // search for an exact match
                var viableErrors = new StringBuilder();
                foreach (var viable in viableFunctions)
                {
                    viableErrors.AppendLine(string.Format("{0} : `{1}'", viable.SourceInfo, viable.MangledName));
                }
                throw new AbiguousCallToAnOverloadedFunctionException(
                    string.Format("Error: {0} : Ambiguous call to an overloaded function:\n{1}",
                    SourceInfo, viableErrors));
            }
            return null;
        }

    }
}
