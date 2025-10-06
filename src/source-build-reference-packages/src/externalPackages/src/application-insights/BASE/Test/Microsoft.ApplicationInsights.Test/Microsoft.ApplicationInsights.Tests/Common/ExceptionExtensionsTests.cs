namespace Microsoft.ApplicationInsights.Common
{
    using System;
    using Microsoft.ApplicationInsights.Common.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExceptionExtensionsTests
    {
        [TestMethod]
        public void VerifyCanFlattenMultipleExceptions()
        {
            var ex1 = new Exception("a");
            var ex2 = new Exception("b", ex1);
            var ex3 = new Exception("c", ex2);

            var test = ex3.FlattenMessages();
            Assert.AreEqual("c | b | a", test);
        }

        [TestMethod]
        public void VerifyCanFlattenSingleException()
        {
            var ex1 = new Exception("a");

            var test = ex1.FlattenMessages();
            Assert.AreEqual("a", test);
        }
    }
}
