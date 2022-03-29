using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lexicon_LMS_G1.Tests
{
    [TestClass]
    public class UnitTest1
    {
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassSetUp(TestContext context)
        {
            context.WriteLine("ClassSetUp run");
        }

        [TestMethod]
        public void TestMethod1()
        {
            TestContext.WriteLine($"TEST {TestContext.TestName}");
        }

        [TestMethod]
        public void TestMethod2()
        {
            TestContext.WriteLine($"TEST {TestContext.TestName}");
        }


    }
}