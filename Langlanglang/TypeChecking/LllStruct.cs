using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using Langlanglang.Parsing.AstNodes;
using Langlanglang.TypeChecking.Exceptions;

namespace Langlanglang.TypeChecking
{
    public class LllStruct : LllType
    {
        private Dictionary<string, AstDeclaration> _members;
        public AstStruct Struct { get; }

        public LllStruct(string cName, AstStruct @struct, bool isAReference, int pointerDepth = 0)
            : base(@struct.Name, cName, false, isAReference, pointerDepth)
        {
            Struct = @struct;
            _members = new Dictionary<string, AstDeclaration>();
        }

        public AstDeclaration GetMember(SourceInfo loc, string name)
        {
            if (_members.ContainsKey(name))
            {
                return _members[name];
            }
            throw new StructMemberNotFoundException(
                string.Format(
                    "Error: {0} : Struct `{1}'(at {2}) does not contain a member called `{3}'",
                    loc, Struct.Name, Struct.SourceInfo, name));
        }

        public void AddMember(AstDeclaration member)
        {
            _members.Add(member.Name, member);
        }

        public override LllType Clone(int withPtrDepth, bool isAReference)
        {
            return new LllStruct(CName, Struct, isAReference, withPtrDepth)
            {
                _members = _members,
                Extensions = Extensions,
            };
        }
    }
}
