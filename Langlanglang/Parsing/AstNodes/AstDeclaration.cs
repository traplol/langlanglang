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
            LllType type;
            if (Type != null)
            {
                type = Type.ToLllType();
            }
            else
            {
                type = AssigningValue.TryInferType(cil);
                Type = new AstType(SourceInfo, type.Name, type.PointerDepth, 0, type.IsAReference, false);
            }
            var cName = NameGenerator.UniqName("var", Name);
            var cType = cil.SymTable.LookupType(type.CName);

            if (IsFixedArray)
            {
                if (AssigningValue != null)
                {
                    throw new NotImplementedException("Assigning to a fixed size array is not implemented.");
                }
                return new CILFixedArray(SourceInfo, cType, type.PointerDepth, cName, Type.FixedArraySize);
            }

            if (AssigningValue == null)
            {
                var decl = new CILVariableDecl(SourceInfo, cType, type.PointerDepth, cName);
                return decl;
            }
            var val = AssigningValue.ToCILExpression(cil);
            return new CILVariableDecl(SourceInfo, cType, type.PointerDepth, cName, val);
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
