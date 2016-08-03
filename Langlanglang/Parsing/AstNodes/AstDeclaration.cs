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
        public string Type { get; set; }
        public int FixedArraySize { get; set; }
        public AstExpression AssigningValue { get; set; }
        public int PointerDepth { get; set; }
        public bool IsGenericlyTyped { get; set; }

        public bool IsFixedArray => FixedArraySize != 0;

        public AstDeclaration(SourceInfo si, string name, string type, int pointerDepth, AstExpression assigningValue, bool isGenericlyTyped = false)
            : base(si)
        {
            Name = name;
            Type = type;
            PointerDepth = pointerDepth;
            AssigningValue = assigningValue;
            IsGenericlyTyped = isGenericlyTyped;
            FixedArraySize = 0;
        }

        public AstDeclaration(SourceInfo si, string name, string type, int pointerDepth, AstExpression assigningValue, int fixedArraySize, bool isGenericlyTyped = false)
            : base(si)
        {
            Name = name;
            Type = type;
            PointerDepth = pointerDepth;
            AssigningValue = assigningValue;
            IsGenericlyTyped = isGenericlyTyped;
            FixedArraySize = fixedArraySize;
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
                type = LllCompiler.SymTable.LookupType(Type);
            }
            else
            {
                type = AssigningValue.TryInferType(cil);
                PointerDepth = type.PointerDepth;
                Type = type.Name;
            }
            var cName = NameGenerator.UniqName("var", Name);
            var cType = cil.SymTable.LookupType(type.CName);

            if (IsFixedArray)
            {
                if (AssigningValue != null)
                {
                    throw new NotImplementedException("Assigning to a fixed size array is not implemented.");
                }
                return new CILFixedArray(SourceInfo, cType, PointerDepth, cName, FixedArraySize);
            }

            if (AssigningValue == null)
            {
                var decl = new CILVariableDecl(SourceInfo, cType, PointerDepth, cName);
                return decl;
            }
            var val = AssigningValue.ToCILExpression(cil);
            return new CILVariableDecl(SourceInfo, cType, PointerDepth, cName, val);
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
            return LllCompiler.SymTable.LookupType(Type).Clone(PointerDepth);
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
                PointerDepth, 
                AssigningValue,
                FixedArraySize,
                IsGenericlyTyped);
        }
    }
}
