using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Langlanglang.Parsing.AstNodes;

namespace Langlanglang.TypeChecking
{
    public class LllStruct : LllType
    {
        private Dictionary<string, AstDeclaration> _members;
        public AstStruct Struct { get; }

        public LllStruct(string cName, AstStruct @struct, int pointerDepth = 0)
            : base(@struct.Name, cName, false, pointerDepth)
        {
            Struct = @struct;
            _members = new Dictionary<string, AstDeclaration>();
        }

        public AstDeclaration GetMember(string name)
        {
            if (_members.ContainsKey(name))
            {
                return _members[name];
            }
            throw new NotImplementedException("TODO: Exception for failed member lookup");
        }

        public void AddMember(AstDeclaration member)
        {
            _members.Add(member.Name, member);
        }

        public override LllType Clone(int withPtrDepth)
        {
            return new LllStruct(CName, Struct, withPtrDepth)
            {
                _members = _members,
                Extensions = Extensions
            };
        }
    }
}
