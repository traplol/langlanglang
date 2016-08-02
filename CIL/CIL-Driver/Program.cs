using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIL;
using CIL.CILNodes;

namespace CIL_Driver
{
    class Program
    {
        private static readonly SourceInfo tsi = new SourceInfo("<test>", -1, -1);
        static void Main(string[] args)
        {
//            var cil = new CIntermediateLang();
//            var main = cil.CreateFunction(tsi, "int", 0, "main", new List<CILVariableDecl>
//            {
//                new CILVariableDecl(tsi, cil.SymTable.LookupType("int"), 0, "argc"),
//                new CILVariableDecl(tsi, cil.SymTable.LookupType("char"), 2, "argv"),
//            });

//            var binop = new CILBinaryOp(tsi, new CILIdent(tsi, "argc"), CILBinaryOp.OpType.Add, new CILInteger(tsi, 42));
//            var index = new CILIndexAccess(tsi, new CILIdent(tsi, "argv"), binop);
//            main.AddBodyNode(new CILAssignment(tsi, index, new CILIdent(tsi, "argc")));
//            main.AddBodyNode(new CILReturn(tsi, new CILIdent(tsi, "argc")));

//            Console.WriteLine(cil);
            Test1();
            Console.ReadLine();
        }

        private static void Test1()
        {
            var cil = new CIntermediateLang();
            var myStruct = cil.CreateStruct(tsi, "String");
            var testFunc = cil.CreateFunction(tsi, "String", 1, "testFunc", new List<CILVariableDecl>());
            var main = cil.CreateFunction(tsi, "int", 0, "main", new List<CILVariableDecl>
            {
                new CILVariableDecl(tsi, cil.SymTable.LookupType("int"), 0, "argc"),
                new CILVariableDecl(tsi, cil.SymTable.LookupType("char"), 2, "argv"),
            });

            main.AddBodyNode(new CILCall(tsi, new CILIdent(tsi, "testFunc"), new List<CILExpression>()
            {
                new CILIdent(tsi, "argc"),
                new CILIdent(tsi, "argv"),
            }));

            testFunc.AddBodyNode(new CILVariableDecl(tsi, cil.SymTable.LookupType("String"), 1, "testStr"));
            var ret = new CILReturn(tsi, new CILIdent(tsi, "testStr"));
            var branch = new CILBranch(tsi, new CILIdent(tsi, "testStr"));
            branch.AddTrueBranchStmt(ret);
            var loop = new CILLoop(tsi, new CILInteger(tsi, 42));
            loop.Body.Add(new CILReturn(tsi, new CILIdent(tsi, "testStr")));
            loop.Before.Add(new CILCall(tsi, new CILIdent(tsi, "testFunc"), new List<CILExpression>()));
            loop.After.Add(new CILCall(tsi, new CILIdent(tsi, "testFunc"), new List<CILExpression>()));
            branch.AddFalseBranchStmt(loop);
            branch.AddFalseBranchStmt(ret);
            testFunc.AddBodyNode(branch);

            myStruct.AddMember(
                new CILVariableDecl(
                    tsi,
                    cil.SymTable.LookupType("int"),
                    0,
                    "length"));
            myStruct.AddMember(
                new CILVariableDecl(
                    tsi, 
                    cil.SymTable.LookupType("char"),
                    1,
                    "string"));
            myStruct.AddMember(
                new CILVariableDecl(
                    tsi,
                    myStruct,
                    1,
                    "test")); 

            testFunc.AddBodyNode(
                new CILAssignment(tsi, 
                    new CILMemberAccess(tsi, new CILMemberAccess(tsi, new CILCall(tsi, new CILIdent(tsi, "testFunc")), "test"), "length"), 
                    new CILInteger(tsi, 42)));
            testFunc.AddBodyNode(
                new CILAssignment(tsi, 
                    new CILMemberAccess(tsi, new CILCall(tsi, new CILIdent(tsi, "testFunc")), "string"),
                    new CILStringLiteral(tsi, "hello world")));
            Console.WriteLine(cil);
        }
    }
}
