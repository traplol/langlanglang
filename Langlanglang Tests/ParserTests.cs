using System;
using System.Security.Cryptography.X509Certificates;
using Langlanglang.Parsing;
using Langlanglang.Parsing.AstNodes;
using Langlanglang.Tokenization;
using Langlanglang.Tokenization.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Langlanglang_Tests
{
    [TestClass]
    public class ParserTests
    {
        private readonly Tokenizer _tokenizer;

        public ParserTests()
        {
            _tokenizer = Tokenizer.CreateBasic();
        }

        private Ast ParseString(string source)
        {
            var tks = _tokenizer.Parse(source);
            var ts = new TokenStream(tks);
            return AstParser.Parse(ts);
        }

        [TestMethod]
        public void AstTestVariableDecl()
        {
            var ast = ParseString("x : String;");
            Assert.AreEqual(1, ast.Roots.Count);
            var decl = ast.Roots[0] as AstDeclaration;
            Assert.AreEqual(decl.Name, "x");
            Assert.AreEqual(decl.Type, "String");
            Assert.AreEqual(decl.AssigningValue, null);
        }

        [TestMethod]
        public void AstTestVariableDeclAssignWithExplicitType()
        {
            var ast = ParseString("x : Number = 5;");
            Assert.AreEqual(1, ast.Roots.Count);
            var decl = ast.Roots[0] as AstDeclaration;
            Assert.AreEqual(decl.Name, "x");
            Assert.AreEqual(decl.Type, "Number");

            var num = decl.AssigningValue as AstNumber;
            Assert.IsNotNull(num);
            Assert.AreEqual(num.Number, 5);
        }

        [TestMethod]
        public void AstTestVariableDeclAssignWithoutExplicitType()
        {
            var ast = ParseString("x := 5;");
            Assert.AreEqual(1, ast.Roots.Count);
            var decl = ast.Roots[0] as AstDeclaration;
            Assert.AreEqual(decl.Name, "x");
            Assert.IsNull(decl.Type);

            var num = decl.AssigningValue as AstNumber;
            Assert.IsNotNull(num);
            Assert.AreEqual(num.Number, 5);
        }

        [TestMethod]
        public void AstTestBinaryOp()
        {
            var ast = ParseString("1 + 2;");
            Assert.AreEqual(1, ast.Roots.Count);
            var binop = ast.Roots[0] as AstBinaryOp;
            Assert.IsNotNull(binop);
        }

        [TestMethod]
        public void AstTestFuncDefWithNoParamsAndNoReturnType()
        {
            var ast = ParseString("func test() {}");
            Assert.AreEqual(1, ast.Roots.Count);
            var func = ast.Roots[0] as AstFunc;
            Assert.AreEqual("test", func.Name);
            Assert.AreEqual(0, func.Params.Count);
            Assert.AreEqual("void", func.ReturnType);
        }

        [TestMethod]
        public void AstTestFuncDefWithNoParamsAndExplicitReturnType()
        {
            var ast = ParseString("func test() -> Object {}");
            Assert.AreEqual(1, ast.Roots.Count);
            var func = ast.Roots[0] as AstFunc;
            Assert.AreEqual("test", func.Name);
            Assert.AreEqual(0, func.Params.Count);
            Assert.AreEqual("Object", func.ReturnType);
        }

        [TestMethod]
        public void AstTestFuncDefWithParamsNoReturnType()
        {
            var ast = ParseString("func test(a : Ty1, b : Ty2) {}");
            Assert.AreEqual(1, ast.Roots.Count);
            var func = ast.Roots[0] as AstFunc;
            Assert.AreEqual("test_Ty1_Ty2", func.MangledName);
            Assert.AreEqual(2, func.Params.Count);
            Assert.AreEqual("void", func.ReturnType);
        }

        [TestMethod]
        public void AstTestFuncDefWithParamsAndExplicitReturnType()
        {
            var ast = ParseString("func test(a : Ty1, b : Ty2) -> Ty3 {}");
            Assert.AreEqual(1, ast.Roots.Count);
            var func = ast.Roots[0] as AstFunc;
            Assert.AreEqual("test_Ty1_Ty2", func.MangledName);
            Assert.AreEqual(2, func.Params.Count);
            Assert.AreEqual("Ty3", func.ReturnType);
        }

        [TestMethod]
        public void AstTestFuncDefWithGenericParams()
        {
            var ast = ParseString("func test(a : #Ty1, b : #Ty2) -> Ty3 {}");
            Assert.AreEqual(1, ast.Roots.Count);
            var func = ast.Roots[0] as AstFunc;
            Assert.AreEqual("test_##_##", func.MangledName);
            Assert.AreEqual(2, func.Params.Count);
            Assert.IsTrue(func.IsGeneric);
            Assert.AreEqual("Ty3", func.ReturnType);
        }

        [TestMethod]
        public void AstTestEmptyStructDecl()
        {
            var ast = ParseString("struct MyTestStruct{}");
            Assert.AreEqual(1, ast.Roots.Count);
            var struc = ast.Roots[0] as AstStruct;
            Assert.AreEqual("MyTestStruct", struc.Name);
            Assert.AreEqual(0, struc.Members.Count);
        }

        [TestMethod]
        public void AstTestStructDecl()
        {
            var ast = ParseString("struct MyTestStruct{ member1 : Number; }");
            Assert.AreEqual(1, ast.Roots.Count);
            var struc = ast.Roots[0] as AstStruct;
            Assert.AreEqual("MyTestStruct", struc.Name);
            Assert.AreEqual(1, struc.Members.Count);
        }

        [TestMethod]
        public void AstTestEmptyExtend()
        {
            var ast = ParseString("extend String MyMethod(){}");
            Assert.AreEqual(1, ast.Roots.Count);
            var extend = ast.Roots[0] as AstExtend;
            Assert.AreEqual("String", extend.Extends);
            Assert.AreEqual("String_MyMethod", extend.MangledName);
            Assert.AreEqual(0, extend.Params.Count);
            Assert.IsFalse(extend.UsesThisPtr);
            Assert.AreEqual(extend.ReturnType, "void");
        }

        [TestMethod]
        public void AstTestExtendWithThis()
        {
            var ast = ParseString("extend String MyMethod(this){}");
            Assert.AreEqual(1, ast.Roots.Count);
            var extend = ast.Roots[0] as AstExtend;
            Assert.AreEqual("String", extend.Extends);
            Assert.AreEqual("String_MyMethod_Stringp", extend.MangledName);
            Assert.AreEqual(1, extend.Params.Count);
            Assert.IsTrue(extend.UsesThisPtr);
            Assert.AreEqual(extend.ReturnType, "void");
        }

        [TestMethod]
        public void AstTestExtendWithNoThisPtrAndWithParams()
        {
            var ast = ParseString("extend String MyMethod(x : Number, y : Number) -> String {}");
            Assert.AreEqual(1, ast.Roots.Count);
            var extend = ast.Roots[0] as AstExtend;
            Assert.AreEqual("String", extend.Extends);
            Assert.AreEqual("String_MyMethod_Number_Number", extend.MangledName);
            Assert.AreEqual(2, extend.Params.Count);
            Assert.IsFalse(extend.UsesThisPtr);
            Assert.AreEqual("String", extend.ReturnType);
        }

        [TestMethod]
        public void AstTestExtendConstructor()
        {
            var ast = ParseString("extend String(this) {}");
            Assert.AreEqual(1, ast.Roots.Count);
            var extend = ast.Roots[0] as AstExtend;
            Assert.AreEqual("String", extend.Extends);
            Assert.AreEqual("String_Ctor_Stringp", extend.MangledName);
            Assert.AreEqual(1, extend.Params.Count);
            Assert.IsTrue(extend.UsesThisPtr);
            Assert.AreEqual("void", extend.ReturnType);
            Assert.AreEqual(0, extend.ReturnPtrDepth);
        }

        [TestMethod]
        public void AstTestExtendDestructor()
        {
            var ast = ParseString("extend ~String(this) {}");
            Assert.AreEqual(1, ast.Roots.Count);
            var extend = ast.Roots[0] as AstExtend;
            Assert.AreEqual("String", extend.Extends);
            Assert.AreEqual("String_Dtor_Stringp", extend.MangledName);
            Assert.AreEqual(1, extend.Params.Count);
            Assert.IsTrue(extend.UsesThisPtr);
            Assert.AreEqual("void", extend.ReturnType);
            Assert.AreEqual(0, extend.ReturnPtrDepth);
        }

        [TestMethod]
        public void AstTestForeignSimple()
        {
            var ast = ParseString("foreign func someFunc();");
            Assert.AreEqual(1, ast.Roots.Count);
            var foreign = ast.Roots[0] as AstForeign;
            Assert.AreEqual("someFunc", foreign.Name);
            Assert.AreEqual("someFunc", foreign.CName);
            Assert.AreEqual(0, foreign.Params.Count);
            Assert.AreEqual("void", foreign.ReturnType);
            Assert.IsFalse(foreign.IsVarArgs);
        }

        [TestMethod]
        public void AstTestForeignSimpleWithReturn()
        {
            var ast = ParseString("foreign func getchar() -> i32;");
            Assert.AreEqual(1, ast.Roots.Count);
            var foreign = ast.Roots[0] as AstForeign;
            Assert.AreEqual("getchar", foreign.Name);
            Assert.AreEqual("getchar", foreign.CName);
            Assert.AreEqual(0, foreign.Params.Count);
            Assert.AreEqual("i32", foreign.ReturnType);
            Assert.IsFalse(foreign.IsVarArgs);
        }

        [TestMethod]
        public void AstTestForeignWithVarArgs()
        {
            var ast = ParseString("foreign func printf (fmt : char*, ...) -> i32;");
            Assert.AreEqual(1, ast.Roots.Count);
            var foreign = ast.Roots[0] as AstForeign;
            Assert.AreEqual("printf", foreign.Name);
            Assert.AreEqual("printf", foreign.CName);
            Assert.AreEqual(1, foreign.Params.Count);
            Assert.AreEqual("i32", foreign.ReturnType);
            Assert.IsTrue(foreign.IsVarArgs);
        }

        [TestMethod]
        public void AstTestForeignAlias()
        {
            var ast = ParseString("foreign func p as 'printf' (fmt : char*, ...) -> i32;");
            Assert.AreEqual(1, ast.Roots.Count);
            var foreign = ast.Roots[0] as AstForeign;
            Assert.AreEqual("p", foreign.Name);
            Assert.AreEqual("printf", foreign.CName);
            Assert.AreEqual(1, foreign.Params.Count);
            Assert.AreEqual("i32", foreign.ReturnType);
            Assert.IsTrue(foreign.IsVarArgs);
        }

        [TestMethod]
        public void TestCallSimple()
        {
            var ast = ParseString("test();");
            Assert.AreEqual(1, ast.Roots.Count);
            var call = ast.Roots[0] as AstCall;
            var callee = call.Callee as AstIdent;
            Assert.AreEqual("test", callee.Name);
        }

        [TestMethod]
        public void AstTestCallACallResult()
        {
            var ast = ParseString("test()();");
            Assert.AreEqual(1, ast.Roots.Count);
            var call = ast.Roots[0] as AstCall;
            var callee1 = call.Callee as AstCall;
            var callee2 = callee1.Callee as AstIdent;
            Assert.AreEqual("test", callee2.Name);
        }

        [TestMethod]
        public void AstTestCallACallResultMulti()
        {
            var ast = ParseString("test()()()();");
            Assert.AreEqual(1, ast.Roots.Count);
            var call = ast.Roots[0] as AstCall;
            var callee1 = call.Callee as AstCall;
            var callee2 = callee1.Callee as AstCall;
            var callee3 = callee2.Callee as AstCall;
            var callee4 = callee3.Callee as AstIdent;
            Assert.AreEqual("test", callee4.Name);
        }

        [TestMethod]
        public void AstTestMemberAccessSimple()
        {
            var ast = ParseString("a.b;");
            Assert.AreEqual(1, ast.Roots.Count);
            var memberAccess = ast.Roots[0] as AstMemberAccess;
            var @from = memberAccess.From as AstIdent;
            Assert.AreEqual("a", @from.Name);
            Assert.AreEqual("b", memberAccess.MemberIdent);
        }

        [TestMethod]
        public void AstTestMemberAccessMulti()
        {
            var ast = ParseString("a.b.c;");
            Assert.AreEqual(1, ast.Roots.Count);
            var memberAccess = ast.Roots[0] as AstMemberAccess;
            Assert.AreEqual("c", memberAccess.MemberIdent);
            var @from1 = memberAccess.From as AstMemberAccess;
            Assert.AreEqual("b", @from1.MemberIdent);
            var @from2 = @from1.From as AstIdent;
            Assert.AreEqual("a", @from2.Name);
        }

        [TestMethod]
        public void AstTestIndexSimple()
        {
            var ast = ParseString("a[123456];");
            Assert.AreEqual(1, ast.Roots.Count);
            var idx = ast.Roots[0] as AstIndex;
            var @from = idx.From as AstIdent;
            Assert.AreEqual("a", @from.Name);
            Assert.AreEqual(1, idx.Subscript.Count);
            var sub1 = idx.Subscript[0] as AstNumber;
            Assert.AreEqual(123456, sub1.Number);
        }

        [TestMethod]
        public void AstTestIndexMulti()
        {
            var ast = ParseString("a[123]['test'];");
            Assert.AreEqual(1, ast.Roots.Count);
            var idx1 = ast.Roots[0] as AstIndex;
            Assert.AreEqual(1, idx1.Subscript.Count);
            var idx1Sub = idx1.Subscript[0] as AstString;
            Assert.AreEqual("test", idx1Sub.String);
            var idx2 = idx1.From as AstIndex;
            Assert.AreEqual(1, idx2.Subscript.Count);
            var idx2Sub = idx2.Subscript[0] as AstNumber;
            Assert.AreEqual(123, idx2Sub.Number);
        }

        [TestMethod]
        public void AstTestMixCallMemberIndex()
        {
            var ast = ParseString("a[5].b.c();");
            Assert.AreEqual(1, ast.Roots.Count);
            var fromRight0 = ast.Roots[0] as AstCall; // ()
            var fromRight1 = fromRight0.Callee as AstMemberAccess; // .c
            var fromRight2 = fromRight1.From as AstMemberAccess; // .b
            var fromRight3 = fromRight2.From as AstIndex; // [5]
            var fromRight4 = fromRight3.From as AstIdent; // a
            Assert.AreEqual(fromRight4.Name, "a");
        }

        [TestMethod]
        public void AstTestForLoop()
        {
            var ast = ParseString("for i := 0; i < 10; i = i + 1 {}");
            Assert.AreEqual(1, ast.Roots.Count);
            var @for = ast.Roots[0] as AstFor;
            Assert.AreEqual(1, @for.Pre.Count);
            Assert.IsTrue(@for.Pre[0] is AstDeclaration);

            Assert.IsTrue(@for.Condition is AstBinaryOp);

            Assert.AreEqual(1, @for.Update.Count);
            Assert.IsTrue(@for.Update[0] is AstAssign);

            Assert.AreEqual(0, @for.Body.Count);
        }

        [TestMethod]
        public void AstTestForInLoopWithVarEndAsc()
        {
            var ast = ParseString("for i in 0..N {}");
            Assert.AreEqual(1, ast.Roots.Count);
            Assert.IsTrue(ast.Roots[0] is AstFor);
        }

        [TestMethod]
        public void AstTestForInLoopWithVarEndDesc()
        {
            var ast = ParseString("for i in N..0 {}");
            Assert.AreEqual(1, ast.Roots.Count);
            Assert.IsTrue(ast.Roots[0] is AstFor);
        }

        [TestMethod]
        public void AstTestForInLoopWithVarAndSub()
        {
            var ast = ParseString("for i in 0..N - {}");
            Assert.AreEqual(1, ast.Roots.Count);
            Assert.IsTrue(ast.Roots[0] is AstFor);
        }

        [TestMethod]
        public void AstTestForInLoopWithVarAndPlus()
        {
            var ast = ParseString("for i in 0..N + {}");
            Assert.AreEqual(1, ast.Roots.Count);
            Assert.IsTrue(ast.Roots[0] is AstFor);
        }

        [TestMethod]
        public void AstTestForInLoopAsc()
        {
            var ast = ParseString("for i in 0..10 {}");
            Assert.AreEqual(1, ast.Roots.Count);
            Assert.IsTrue(ast.Roots[0] is AstFor);
        }

        [TestMethod]
        public void AstTestForInLoopDesc()
        {
            var ast = ParseString("for i in 10..0 {}");
            Assert.AreEqual(1, ast.Roots.Count);
            Assert.IsTrue(ast.Roots[0] is AstFor);
        }

        [TestMethod]
        public void AstTestForInLoopNegatives()
        {
            var ast = ParseString("for i in -5..-1 {}");
            Assert.AreEqual(1, ast.Roots.Count);
            Assert.IsTrue(ast.Roots[0] is AstFor);
        }

        [TestMethod]
        public void AstTestForeachLoop()
        {
            var ast = ParseString("foreach i in [0..10] {}");
            Assert.AreEqual(1, ast.Roots.Count);
            Assert.IsTrue(ast.Roots[0] is AstForeach);
        }

        [TestMethod]
        public void AstTestWhileLoop()
        {
            var ast = ParseString("while i < 10 {}");
            Assert.AreEqual(1, ast.Roots.Count);
            Assert.IsTrue(ast.Roots[0] is AstWhile);
        }

        [TestMethod]
        public void AstTestIfOnly()
        {
            var ast = ParseString("if true {}");
            Assert.AreEqual(1, ast.Roots.Count);
            Assert.IsTrue(ast.Roots[0] is AstIf);
        }

        [TestMethod]
        public void AstTestIfWithElse()
        {
            var ast = ParseString("if true {} else {}");
            Assert.AreEqual(1, ast.Roots.Count);
            Assert.IsTrue(ast.Roots[0] is AstIf);
        }

        [TestMethod]
        public void AstTestIfWithElseIf()
        {
            var ast = ParseString("if 1 == 2 {} else if 1 == 1 {}");
            Assert.AreEqual(1, ast.Roots.Count);
            Assert.IsTrue(ast.Roots[0] is AstIf);
        }

        [TestMethod]
        public void AstTestIfWithElseIfElse()
        {
            var ast = ParseString("if x > y {} else if x < y {} else {}");
            Assert.AreEqual(1, ast.Roots.Count);
            Assert.IsTrue(ast.Roots[0] is AstIf);
        }

        [TestMethod]
        public void AstTestPackageSimple()
        {
            var ast = ParseString("package stdlib;");
            Assert.AreEqual(1, ast.Roots.Count);
            Assert.IsTrue(ast.Roots[0] is AstPackage);
        }

        [TestMethod]
        public void AstTestPackageMulti()
        {
            var ast = ParseString("package stdlib.math.rng;");
            Assert.AreEqual(1, ast.Roots.Count);
            Assert.IsTrue(ast.Roots[0] is AstPackage);
        }

        [TestMethod]
        public void AstTestRequireSimple()
        {
            var ast = ParseString("require stdlib;");
            Assert.AreEqual(1, ast.Roots.Count);
            Assert.IsTrue(ast.Roots[0] is AstRequire);
        }

        [TestMethod]
        public void AstTestRequireMulti()
        {
            var ast = ParseString("require stdlib.math.rng;");
            Assert.AreEqual(1, ast.Roots.Count);
            Assert.IsTrue(ast.Roots[0] is AstRequire);
        }

        [TestMethod]
        public void AstTestRequireWithStar()
        {
            var ast = ParseString("require stdlib.math.rng.*;");
            Assert.AreEqual(1, ast.Roots.Count);
            Assert.IsTrue(ast.Roots[0] is AstRequire);
        }

        [TestMethod]
        public void AstOrderOfOperations()
        {
            var ast = ParseString("1 + 2 * 3;");
            Assert.AreEqual(1, ast.Roots.Count);
            var root = ast.Roots[0] as AstBinaryOp;
            Assert.AreEqual(TokenType.Plus, root.Op);
            var lhs = root.Lhs as AstNumber;
            Assert.AreEqual(1, lhs.Number);
            var rhs = root.Rhs as AstBinaryOp;

            Assert.AreEqual(TokenType.Star, rhs.Op);
            var rhs_lhs = rhs.Lhs as AstNumber;
            Assert.AreEqual(2, rhs_lhs.Number);
            var rhs_rhs = rhs.Rhs as AstNumber;
            Assert.AreEqual(3, rhs_rhs.Number);
        }

        [TestMethod]
        public void AstTestVoidReturn()
        {
            var ast = ParseString("func void_return() {return;}");
            Assert.AreEqual(1, ast.Roots.Count);
            var func = ast.Roots[0] as AstFunc;
            Assert.AreEqual(1, func.Body.Count);
            var ret = func.Body[0] as AstReturn;
            Assert.IsNull(ret.ExpressionToReturn);
        }

        [TestMethod]
        public void AstTestReturn()
        {
            var ast = ParseString("func num_return() -> Number {return 1;}");
            Assert.AreEqual(1, ast.Roots.Count);
            var func = ast.Roots[0] as AstFunc;
            Assert.AreEqual(1, func.Body.Count);
            var ret = func.Body[0] as AstReturn;
            Assert.IsNotNull(ret.ExpressionToReturn);
        }

        [TestMethod]
        public void AstTestNewStruct()
        {
            var ast = ParseString("new SomeStruct(42);");
            Assert.AreEqual(1, ast.Roots.Count);
            var @new = ast.Roots[0] as AstUnaryOp;
            Assert.AreEqual(TokenType.New, @new.Op);
            Assert.IsTrue(@new.Expression is AstCall);
        }

        [TestMethod]
        public void AstTestDeleteThing()
        {
            var ast = ParseString("delete thing;");
            Assert.AreEqual(1, ast.Roots.Count);
            var @new = ast.Roots[0] as AstUnaryOp;
            Assert.AreEqual(TokenType.Delete, @new.Op);
            Assert.IsTrue(@new.Expression is AstIdent);
        }

        [TestMethod]
        public void AstTestNewArray_42()
        {
            var ast = ParseString("new int[42];");
            Assert.AreEqual(1, ast.Roots.Count);
            var @new = ast.Roots[0] as AstNewArrayOp;
            Assert.AreEqual(TokenType.New, @new.Op);
            Assert.AreEqual("int", @new.Type);
            Assert.AreEqual(1, @new.PointerDepth);
            Assert.IsTrue(@new.Expression is AstNumber);
        }

        [TestMethod]
        public void AstTestNewArray_n()
        {
            var ast = ParseString("new int[n];");
            Assert.AreEqual(1, ast.Roots.Count);
            var @new = ast.Roots[0] as AstNewArrayOp;
            Assert.AreEqual(TokenType.New, @new.Op);
            Assert.AreEqual("int", @new.Type);
            Assert.AreEqual(1, @new.PointerDepth);
            Assert.IsTrue(@new.Expression is AstIdent);
        }

        [TestMethod]
        public void AstTestNewArrayOfPointers()
        {
            var ast = ParseString("new int*[n];");
            Assert.AreEqual(1, ast.Roots.Count);
            var @new = ast.Roots[0] as AstNewArrayOp;
            Assert.AreEqual(TokenType.New, @new.Op);
            Assert.AreEqual("int", @new.Type);
            Assert.AreEqual(2, @new.PointerDepth);
            Assert.IsTrue(@new.Expression is AstIdent);
        }

        [TestMethod]
        public void AstTestFixedSizeArray()
        {
            var ast = ParseString("x : int[45];");
            Assert.AreEqual(1, ast.Roots.Count);
            var decl = ast.Roots[0] as AstDeclaration;
            Assert.AreEqual("x", decl.Name);
            Assert.AreEqual("int", decl.Type);
            Assert.AreEqual(1, decl.PointerDepth);
            Assert.IsTrue(decl.IsFixedArray);
            Assert.AreEqual(decl.FixedArraySize, 45);
        }

        [TestMethod]
        public void AstTestFixedSizedArrayOfPointers()
        {
            var ast = ParseString("x : int*[45];");
            Assert.AreEqual(1, ast.Roots.Count);
            var decl = ast.Roots[0] as AstDeclaration;
            Assert.AreEqual("x", decl.Name);
            Assert.AreEqual("int", decl.Type);
            Assert.AreEqual(2, decl.PointerDepth);
            Assert.IsTrue(decl.IsFixedArray);
            Assert.AreEqual(decl.FixedArraySize, 45);
        }
    }
}
