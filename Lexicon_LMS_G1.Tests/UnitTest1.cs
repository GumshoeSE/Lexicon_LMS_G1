using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lexicon_LMS_G1.Tests
{
    [TestClass]
    public class UnitTest1
    {
        public TestContext tc { get; set; }

        [ClassInitialize]
        public static void ClassSetUp(TestContext context)
        {

        }

        [TestMethod]
        public void TestMethod1()
        {
            tc.WriteLine($"TEST {tc.TestName}");
        }

        [TestMethod]
        public void TestMethod2()
        {
            tc.WriteLine($"TEST {tc.TestName}");

            var t = testc();

            //Assert.IsTrue

        }

        private object testc()
        {
            throw new System.NotImplementedException();
        }
    }
}