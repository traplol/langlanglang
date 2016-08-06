using System;
using Langlanglang.Packages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Langlanglang_Tests
{
    [TestClass]
    public class PackageTests
    {
        private readonly BasePackage _basePackage;

        public PackageTests()
        {
            _basePackage = new BasePackage(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'));
            _basePackage.AddPackage("std", null);
            _basePackage.AddPackage("base2", null);
            _basePackage.AddPackage("std.vector", "lib\\vector.lll");
            _basePackage.AddPackage("std.fixedarray", "lib\\fixedarray.lll");
            _basePackage.AddPackage("std.test", "lib\\test.lll");
            _basePackage.AddPackage("base2.pkg", "lib\\base2\\bas2.lll");
        }

        [TestMethod]
        public void PackageTestGetExistingPackage()
        {
            var package = _basePackage.GetPackage("std.fixedarray");
            Assert.AreEqual("fixedarray", package.Name);
            Assert.AreEqual("lib\\fixedarray.lll", package.FilePath);
        }

        [TestMethod]
        public void PackageTestGetNonexistingPackage()
        {
            try
            {
                var package = _basePackage.GetPackage("bad1.bad2.bad3");
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                /* PASS */
                return;
            }
        }

        [TestMethod]
        public void PackageTestAddNewPackage()
        {
            var package = _basePackage.AddPackage("bad1.bad2.bad3", "some\\lib\\bad3.lll");
            Assert.AreEqual("bad3", package.Name);
        }

        [TestMethod]
        public void PackageTestAddNewSubPackage()
        {
            var package = _basePackage.AddPackage("bad1.bad2.bad4", "bad\\lib\\bad4.lll");
            Assert.AreEqual("bad4", package.Name);
        }

        [TestMethod]
        public void PackageTestAddExistingPackage()
        {
            try
            {
                _basePackage.AddPackage("std.vector", "some\\other\\path.lll");
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                /* PASS */
                return;
            }
        }

        [TestMethod]
        public void PackageTestAddAndGetVeryLongPackage()
        {
            var p20_1 = _basePackage.AddPackage("p1.p2.p3.p4.p5.p6.p7.p8.p9.p10.p11.p12.p13.p14.p15.p16.p17.p18.p19.p20", "some\\other\\path.lll");
            var p20_2 = _basePackage.GetPackage("p1.p2.p3.p4.p5.p6.p7.p8.p9.p10.p11.p12.p13.p14.p15.p16.p17.p18.p19.p20");
            var p15 = _basePackage.GetPackage("p1.p2.p3.p4.p5.p6.p7.p8.p9.p10.p11.p12.p13.p14.p15");
            var p20_3 = p15.GetPackage("p16.p17.p18.p19.p20".Split('.'));
            Assert.IsNotNull(p20_1);
            Assert.IsNotNull(p20_2);
            Assert.IsNotNull(p20_3);
            Assert.IsTrue(ReferenceEquals(p20_1, p20_2));
            Assert.IsTrue(ReferenceEquals(p20_1, p20_3));
        }
    }
}
