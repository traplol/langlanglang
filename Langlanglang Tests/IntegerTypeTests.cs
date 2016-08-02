using System;
using System.Net;
using Langlanglang.Compiler;
using Langlanglang.TypeChecking;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Langlanglang_Tests
{
    [TestClass]
    public class IntegerTypeTests
    {
        private static readonly LllIntegerType U8 = LllIntegerType.U8; 
        private static readonly LllIntegerType U16 = LllIntegerType.U16; 
        private static readonly LllIntegerType U32 = LllIntegerType.U32; 
        private static readonly LllIntegerType U64 = LllIntegerType.U64; 

        private static readonly LllIntegerType I8 = LllIntegerType.I8; 
        private static readonly LllIntegerType I16 = LllIntegerType.I16; 
        private static readonly LllIntegerType I32 = LllIntegerType.I32; 
        private static readonly LllIntegerType I64 = LllIntegerType.I64;

        // unsigned to x

        [TestMethod]
        public void IntegerTypeTestCast_u8_to_uX()
        {
            Assert.IsTrue(U8.TryCast(U8));
            Assert.IsTrue(U8.TryCast(U16));
            Assert.IsTrue(U8.TryCast(U32));
            Assert.IsTrue(U8.TryCast(U64));
        }

        [TestMethod]
        public void IntegerTypeTestCast_u8_to_sX()
        {
            Assert.IsFalse(U8.TryCast(I8));
            Assert.IsTrue(U8.TryCast(I16));
            Assert.IsTrue(U8.TryCast(I32));
            Assert.IsTrue(U8.TryCast(I64));
        }



        [TestMethod]
        public void IntegerTypeTestCast_u16_to_uX()
        {
            Assert.IsFalse(U16.TryCast(U8));
            Assert.IsTrue(U16.TryCast(U16));
            Assert.IsTrue(U16.TryCast(U32));
            Assert.IsTrue(U16.TryCast(U64));
        }

        [TestMethod]
        public void IntegerTypeTestCast_u16_to_sX()
        {
            Assert.IsFalse(U16.TryCast(I8));
            Assert.IsFalse(U16.TryCast(I16));
            Assert.IsTrue(U16.TryCast(I32));
            Assert.IsTrue(U16.TryCast(I64));
        }




        [TestMethod]
        public void IntegerTypeTestCast_u32_to_uX()
        {
            Assert.IsFalse(U32.TryCast(U8));
            Assert.IsFalse(U32.TryCast(U16));
            Assert.IsTrue(U32.TryCast(U32));
            Assert.IsTrue(U32.TryCast(U64));
        }

        [TestMethod]
        public void IntegerTypeTestCast_u32_to_sX()
        {
            Assert.IsFalse(U32.TryCast(I8));
            Assert.IsFalse(U32.TryCast(I16));
            Assert.IsFalse(U32.TryCast(I32));
            Assert.IsTrue(U32.TryCast(I64));
        }




        [TestMethod]
        public void IntegerTypeTestCast_u64_to_uX()
        {
            Assert.IsFalse(U64.TryCast(U8));
            Assert.IsFalse(U64.TryCast(U16));
            Assert.IsFalse(U64.TryCast(U32));
            Assert.IsTrue(U64.TryCast(U64));
        }

        [TestMethod]
        public void IntegerTypeTestCast_u64_to_sX()
        {
            Assert.IsFalse(U64.TryCast(I8));
            Assert.IsFalse(U64.TryCast(I16));
            Assert.IsFalse(U64.TryCast(I32));
            Assert.IsFalse(U64.TryCast(I64));
        }

        // signed to x

        [TestMethod]
        public void IntegerTypeTestCast_s8_to_uX()
        {
            Assert.IsFalse(I8.TryCast(U8));
            Assert.IsFalse(I8.TryCast(U16));
            Assert.IsFalse(I8.TryCast(U32));
            Assert.IsFalse(I8.TryCast(U64));
        }

        [TestMethod]
        public void IntegerTypeTestCast_s8_to_sX()
        {
            Assert.IsTrue(I8.TryCast(I8));
            Assert.IsTrue(I8.TryCast(I16));
            Assert.IsTrue(I8.TryCast(I32));
            Assert.IsTrue(I8.TryCast(I64));
        }

        [TestMethod]
        public void IntegerTypeTestCast_s16_to_uX()
        {
            Assert.IsFalse(I16.TryCast(U8));
            Assert.IsFalse(I16.TryCast(U16));
            Assert.IsFalse(I16.TryCast(U32));
            Assert.IsFalse(I16.TryCast(U64));
        }

        [TestMethod]
        public void IntegerTypeTestCast_s16_to_sX()
        {
            Assert.IsFalse(I16.TryCast(I8));
            Assert.IsTrue(I16.TryCast(I16));
            Assert.IsTrue(I16.TryCast(I32));
            Assert.IsTrue(I16.TryCast(I64));
        }

        [TestMethod]
        public void IntegerTypeTestCast_s32_to_uX()
        {
            Assert.IsFalse(I32.TryCast(U8));
            Assert.IsFalse(I32.TryCast(U16));
            Assert.IsFalse(I32.TryCast(U32));
            Assert.IsFalse(I32.TryCast(U64));
        }

        [TestMethod]
        public void IntegerTypeTestCast_s32_to_sX()
        {
            Assert.IsFalse(I32.TryCast(I8));
            Assert.IsFalse(I32.TryCast(I16));
            Assert.IsTrue(I32.TryCast(I32));
            Assert.IsTrue(I32.TryCast(I64));
        }

        [TestMethod]
        public void IntegerTypeTestCast_s64_to_uX()
        {
            Assert.IsFalse(I64.TryCast(U8));
            Assert.IsFalse(I64.TryCast(U16));
            Assert.IsFalse(I64.TryCast(U32));
            Assert.IsFalse(I64.TryCast(U64));
        }

        [TestMethod]
        public void IntegerTypeTestCast_s64_to_sX()
        {
            Assert.IsFalse(I64.TryCast(I8));
            Assert.IsFalse(I64.TryCast(I16));
            Assert.IsFalse(I64.TryCast(I32));
            Assert.IsTrue(I64.TryCast(I64));
        }

    }
}
