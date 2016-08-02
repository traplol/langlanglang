using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;

namespace Langlanglang.Parsing.AstNodes
{
    public class AstForeach : AstStatement
    {
        public string IterName { get; set; }
        public AstExpression Collection { get; set; }
        public List<AstNode> Body { get; set; }

        public AstForeach(SourceInfo si, string iterName, AstExpression collection, List<AstNode> body)
            : base(si)
        {
            IterName = iterName;
            Collection = collection;
            Body = body;
        }

        public override CILStatement ToCILStatement(CIntermediateLang cil)
        {
            throw new NotImplementedException();
        }
    }
}
