using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;
using CIL.SymbolStore;
using Langlanglang.Compiler;
using Langlanglang.Compiler.Exceptions;
using Langlanglang.TypeChecking;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstDeclaration : AstStatement
    {
        public string Name { get; set; }
        public AstExpression AssigningValue { get; set; }
        public bool IsFixedArray => Type.FixedArraySize != 0;
        public bool IsGeneric => Type.IsGeneric;

        public AstType Type { get; set; }

        public AstDeclaration(SourceInfo si, string name, AstType type, AstExpression assigningValue)
            : base(si)
        {
            Name = name;
            Type = type;
            AssigningValue = assigningValue;
        }

        public override CILStatement ToCILStatement(CIntermediateLang cil)
        {
            throw new NotImplementedException();
            //var decl = ToCILVariableDecl(cil);
            //return new CILAssignment(SourceInfo, decl, AssigningValue.ToCILExpression(cil));
        }

        public CILVariableDecl ToCILVariableDecl(CIntermediateLang cil)
        {
            // TODO: Do propery type checking between declared type and inferred/rhs type
            LllType realType;
            if (Type != null)
            {
                realType = Type.ToLllType();
            }
            else
            {
                realType = AssigningValue.TryInferType(cil);
                if (realType.IsAReference)
                {
                    // Auto-dereference the type...
                    Type = new AstType(SourceInfo, realType.Name, realType.PointerDepth-1, 0, false, false);
                }
                else
                {
                    Type = new AstType(SourceInfo, realType.Name, realType.PointerDepth, 0, realType.IsAReference, false);
                }
            }
            var cName = NameGenerator.UniqName("var", Name);
            var cType = cil.SymTable.LookupType(realType.CName);

            if (IsFixedArray)
            {
                if (AssigningValue != null)
                {
                    throw new NotImplementedException("Assigning to a fixed size array is not implemented.");
                }
                return new CILFixedArray(SourceInfo, cType, realType.PointerDepth, cName, Type.FixedArraySize);
            }

            // covers the case of declaration and function paramters
            if (AssigningValue == null)
            {
                var decl = new CILVariableDecl(SourceInfo, cType, realType.PointerDepth, cName);
                return decl;
            }
            var val = AssigningValue.ToCILExpression(cil);
            var srcTy = AssigningValue.TryInferType(cil);
            var fixedPtrDepth = srcTy.PointerDepth;
            if (srcTy.IsAReference && !Type.IsAReference)
            {
                --fixedPtrDepth;
                val = new CILDereference(SourceInfo, val);
            }
            return new CILVariableDecl(SourceInfo, cType, fixedPtrDepth, cName, val);
        }

        public CILVariableDecl ToCILVariableDeclAndDecl(CIntermediateLang cil)
        {
            var cilVar = ToCILVariableDecl(cil);
            LllCompiler.SymTable.AddSymbol(new LllSymbol(Name, Name, cilVar.Name, this));
            //cil.DeclareLocalVariable(cilVar);
            return cilVar;
        }

        public LllType GetRealType()
        {
            return Type.ToLllType();
            //return LllCompiler.SymTable.LookupType(Type).Clone(PointerDepth);
        }

        public override CILNode ToCILNode(CIntermediateLang cil)
        {
            return ToCILVariableDeclAndDecl(cil);
        }

        public AstDeclaration ShallowClone()
        {
            return new AstDeclaration(
                SourceInfo, 
                Name, 
                Type, 
                AssigningValue);
        }
    }
}
