namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PropertyFetcherTests
    {
        [TestMethod]
        public void FetchWithMultipleObjectTypes()
        {
            PropertyFetcher fetcher = new PropertyFetcher("Name");

            string value1 = (string)fetcher.Fetch(new TestClass1 { Name = "Value1" });
            string value2 = (string)fetcher.Fetch(new TestClass2 { Name = "Value2" });

            Assert.AreEqual("Value1", value1);
            Assert.AreEqual("Value2", value2);
        }

        private class TestClass1
        {
            public string Name { get; set; }
        }

        private class TestClass2
        {
            public string Name { get; set; }
        }
    }
}
