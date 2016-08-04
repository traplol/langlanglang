using System;
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
    public class AstStruct : AstNode
    {
        public string Name { get; set; }
        public List<AstDeclaration> Members { get; set; }

        private CILStruct _cilStruct;

        public AstStruct(SourceInfo si, string name, List<AstDeclaration> members)
            : base(si)
        {
            Name = name;
            Members = members;
        }

        public void CDecl(CIntermediateLang cil)
        {
            var cName = NameGenerator.UniqName("struc", Name);
            LllCompiler.SymTable.AddSymbol(new LllStruct(cName, this, false));
            _cilStruct = cil.CreateStruct(SourceInfo, cName);
        }

        public void CDefine(CIntermediateLang cil)
        {
            var lllStruct = LllCompiler.SymTable.LookupStruct(Name);
            foreach (var m in Members)
            {
                lllStruct.AddMember(m);
                var mType = LllCompiler.SymTable.LookupType(m.Type.TypeName);
                var cType = cil.SymTable.LookupType(mType.CName);
                CILVariableDecl member;
                if (m.IsFixedArray)
                {
                    member = new CILFixedArray(m.SourceInfo, cType, m.Type.PointerDepth, m.Name, m.Type.FixedArraySize);
                }
                else {
                    member = new CILVariableDecl(m.SourceInfo, cType, m.Type.PointerDepth, m.Name);
                }
                _cilStruct.AddMember(member);
            }
        }

        public override CILNode ToCILNode(CIntermediateLang cil)
        {
            return _cilStruct;
        }
    }
}
