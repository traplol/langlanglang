using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;
using Langlanglang.Compiler;
using Langlanglang.TypeChecking;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstMemberAccess : AstExpression
    {
        public AstExpression From { get; set; }
        public string MemberIdent { get; set; }

        public AstMemberAccess(SourceInfo si, AstExpression @from, string memberIdent)
            : base(si)
        {
            From = @from;
            MemberIdent = memberIdent;
        }

        public override CILExpression ToCILExpression(CIntermediateLang cil)
        {
            return new CILMemberAccess(SourceInfo, From.ToCILExpression(cil), MemberIdent);
        }

        public override LllType TryInferType(CIntermediateLang cil)
        {
            var from = From.TryInferType(cil);
            if (from.Extensions.ContainsKey(MemberIdent))
            {
                var ext = from.Extensions[MemberIdent];
                return ext.ReturnType.ToLllType();
            }
            var from_ = from as LllStruct;
            if (from_ == null)
            {
                throw new NotImplementedException("TODO");
            }
            var member = from_.GetMember(MemberIdent);
            var membType = LllCompiler.SymTable.LookupType(member.Type.TypeName);
            return membType.Clone(member.Type.PointerDepth, membType.IsAReference);
        }
    }
}
