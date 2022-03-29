using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Tests.SetUp
{
    [TestClass]
    public class GlobalSetUp
    {
        [AssemblyInitialize]
        public static void AssemblySetUp(TestContext context)
        {
            context.WriteLine("Assembley init start");
        }

        [AssemblyCleanup]
        public static void AssemblyCleanUp()
        {
            
        }
    }
}
