namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DependencyTargetNameHelperTest
    {
        [TestMethod]
        public void DependencyTargetNameHelperTests()
        {
            Uri httpPortDefaultUri = new Uri("http://www.microsoft.com");
            Uri httpsPortDefaultUri = new Uri("https://www.microsoft.com");
            Uri httpPortExplicitUri = new Uri("http://www.microsoft.com:80");
            Uri httpsPortExplicitUri = new Uri("http://www.microsoft.com:443");
            Uri randomPortExplicitUri = new Uri("http://www.microsoft.com:1010");
            Assert.AreEqual(httpPortDefaultUri.Host, DependencyTargetNameHelper.GetDependencyTargetName(httpPortDefaultUri));
            Assert.AreEqual(httpsPortDefaultUri.Host, DependencyTargetNameHelper.GetDependencyTargetName(httpsPortDefaultUri));
            Assert.AreEqual(httpPortExplicitUri.Host, DependencyTargetNameHelper.GetDependencyTargetName(httpPortExplicitUri));
            Assert.AreEqual(httpsPortExplicitUri.Host, DependencyTargetNameHelper.GetDependencyTargetName(httpsPortExplicitUri));
            Assert.AreEqual(randomPortExplicitUri.Host + ":" + randomPortExplicitUri.Port, DependencyTargetNameHelper.GetDependencyTargetName(randomPortExplicitUri));
        }
    }
}
