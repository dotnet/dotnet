namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    

    [TestClass]
    public class InternalContextTests
    {
        [TestMethod]
        public void SdkVersionIsNullByDefaultToAvoidSendingItToEndpointUnnecessarily()
        {
            var context = new InternalContext();
            Assert.IsNull(context.SdkVersion);
        }

        [TestMethod]
        public void IpCanBeChangedByUserToSpecifyACustomValue()
        {
            var context = new InternalContext();
            context.SdkVersion = "0.0.11.00.1";
            Assert.AreEqual("0.0.11.00.1", context.SdkVersion);
        }
    }
}
