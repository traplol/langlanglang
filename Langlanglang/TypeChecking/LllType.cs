using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.SymbolStore;
using Langlanglang.Compiler;
using Langlanglang.Parsing.AstNodes;

namespace Langlanglang.TypeChecking
{
    public class LllType : LllSymbol
    {
        public bool IsPrimitive { get; }
        public int PointerDepth { get; protected set; }
        public bool IsAReference { get; protected set; }
        public Dictionary<string, AstExtend> Extensions { get; protected set; }

        public bool IsPointer => PointerDepth > 0;

        public LllType(LllType copyFrom, int pointerDepth)
            : base(copyFrom)
        {
            IsPrimitive = copyFrom.IsPrimitive;
            Extensions = copyFrom.Extensions;
            IsAReference = copyFrom.IsAReference;
            PointerDepth = pointerDepth;
        }

        public LllType(string name, string cName, bool isPrimitive, bool isAReference, int pointerDepth = 0)
            : base(name, name, cName)
        {
            IsPrimitive = isPrimitive;
            PointerDepth = pointerDepth;
            Extensions = new Dictionary<string, AstExtend>();
            IsAReference = isAReference;
            //CtorAndDtor();
        }

        public LllType(string name, string cName)
            : base(name, name, cName)
        {
            IsPrimitive = true;
            PointerDepth = 0;
            Extensions = new Dictionary<string, AstExtend>();
            //CtorAndDtor();
        }

        public virtual LllType Clone(int withPtrDepth, bool isAReference)
        {
            var ty = new LllType(this, withPtrDepth)
            {
                IsAReference = isAReference
            };
            return ty;
        }

        private void CtorAndDtor()
        {
            if (LllCompiler.SymTable == null)
            {
                return;
            }
            var ctor = CreateDefaultCtorOrDtor(Name, "Ctor");
            var ctorCName = NameGenerator.UniqName("ext", ctor.Name);
            if (LllCompiler.SymTable.LookupSymbol(ctor.MangledName) == null)
            {
                LllCompiler.SymTable.AddSymbolAtGlobalScope(new LllFunction(ctorCName, ctor));
                ctor.CanOverride = true;
                Extensions.Add(ctor.MangledName, ctor);
            }

            var dtor = CreateDefaultCtorOrDtor(Name, "Dtor");
            var dtorCName = NameGenerator.UniqName("ext", dtor.Name);
            if (LllCompiler.SymTable.LookupSymbol(dtor.MangledName) == null)
            {
                LllCompiler.SymTable.AddSymbolAtGlobalScope(new LllFunction(dtorCName, dtor));
                dtor.CanOverride = true;
                Extensions.Add(dtor.MangledName, dtor);
            }
        }

        private static AstExtend CreateDefaultCtorOrDtor(string type, string ctorOrDtor)
        {
            var si = new SourceInfo(string.Format("<default_{0}_{1}>", type, ctorOrDtor), -1, -1);
            return new AstExtend(
                si,
                type,
                ctorOrDtor,
                true,
                new List<AstDeclaration>()
                {
                    new AstDeclaration(si, "this", new AstType(si, type, 1, 0, false, false), null)
                },
                new AstType(si, "void", 0, 0, false, false), 
                new List<AstNode>());
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", Name, new string('*', PointerDepth));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            var other = obj as LllType;
            if (other == null)
            {
                return false;
            }
            var tmpPtrDepth = IsAReference ? PointerDepth - 1 : PointerDepth;
            return Name == other.Name && tmpPtrDepth == other.PointerDepth;
        }

        public virtual List<LllType> Select()
        {
            return new List<LllType> {this};
        }

        public override int GetHashCode()
        {
            throw new NotSupportedException("LllType does not support GetHashCode");
        }

        public virtual bool TryCast(LllType toType)
        {
            return false;
        } 
    }
}
