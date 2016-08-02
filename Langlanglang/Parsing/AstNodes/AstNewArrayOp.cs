using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;
using CIL.SymbolStore;
using Langlanglang.Compiler;
using Langlanglang.Tokenization;
using Langlanglang.TypeChecking;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstNewArrayOp : AstUnaryOp
    {
        public string Type { get; set; }
        public int PointerDepth { get; set; } 

        public AstNewArrayOp(SourceInfo si, AstExpression expression, string type, int pointerDepth) 
            : base(si, TokenType.New, expression)
        {
            Type = type;
            PointerDepth = pointerDepth;
        }

        public override CILExpression ToCILExpression(CIntermediateLang cil)
        {
            var type = TryInferType(cil).Clone(PointerDepth-1);
            var @sizeof = new CILSizeof(SourceInfo, string.Format("{0}{1}", type.CName, new string('*', type.PointerDepth)));
            var size = new CILBinaryOp(SourceInfo, Expression.ToCILExpression(cil), CILBinaryOp.OpType.Mul, @sizeof);
            var alloc = new CILCall(SourceInfo, new CILIdent(SourceInfo, "malloc"), new List<CILExpression> {size});
            var tmpName = NameGenerator.NewTemp();
            var tmp = new CILVariableDecl(SourceInfo, cil.SymTable.LookupType(type.CName), PointerDepth, tmpName, alloc);
            var rewrite = new CILRewriteExpression(SourceInfo, new List<CILNode>
            {
                tmp,
                new CILIdent(SourceInfo, tmp.Name)
            });
            return rewrite;
            throw new NotImplementedException("TODO: Implement AstNewArrayOp");
        }

        public override LllType TryInferType(CIntermediateLang cil)
        {
            return LllCompiler.SymTable.LookupType(Type).Clone(PointerDepth);
        }
    }
}
