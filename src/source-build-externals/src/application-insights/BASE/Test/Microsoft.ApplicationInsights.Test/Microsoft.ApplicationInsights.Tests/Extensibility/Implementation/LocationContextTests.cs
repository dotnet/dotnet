namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    
    
    [TestClass]
    public class LocationContextTests
    {
        [TestMethod]
        public void ClassIsPublicToAllowSpecifyingCustomLocationContextPropertiesInUserCode()
        {
            Assert.IsTrue(typeof(LocationContext).GetTypeInfo().IsPublic);
        }

        [TestMethod]
        public void IpIsNullByDefaultToAvoidSendingItToEndpointUnnecessarily()
        {
            var context = new LocationContext();
            Assert.IsNull(context.Ip);
        }

        [TestMethod]
        public void IpCanBeChangedByUserToSpecifyACustomValue()
        {
            var context = new LocationContext();
            context.Ip = "192.168.1.1";
            Assert.AreEqual("192.168.1.1", context.Ip);
        }
    }
}
