namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.HttpParsers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AzureServiceBusHttpParserTests
    {
        [TestMethod]
        public void AzureServiceBusHttpParserSupportsNationalClouds()
        {
            var testCases = new List<Tuple<string, string, string>>()
            {
                Tuple.Create("Send message", "POST", "https://myaccount.servicebus.windows.net/myQueue/messages"),
                Tuple.Create("Send message", "POST", "https://myaccount.servicebus.chinacloudapi.cn/myQueue/messages"),
                Tuple.Create("Send message", "POST", "https://myaccount.servicebus.cloudapi.de/myQueue/messages"),
                Tuple.Create("Send message", "POST", "https://myaccount.servicebus.usgovcloudapi.net/myQueue/messages")
            };

            foreach (var testCase in testCases)
            {
                this.AzureServiceBusHttpParserConvertsValidDependencies(
                    testCase.Item1,
                    testCase.Item2,
                    testCase.Item3);
            }
        }

        private void AzureServiceBusHttpParserConvertsValidDependencies(
            string operation, 
            string verb, 
            string url)
        {
            Uri parsedUrl = new Uri(url);

            // Parse with verb
            var d = new DependencyTelemetry(
                dependencyTypeName: RemoteDependencyConstants.HTTP,
                target: parsedUrl.Host,
                dependencyName: verb + " " + parsedUrl.AbsolutePath,
                data: parsedUrl.OriginalString);

            bool success = AzureServiceBusHttpParser.TryParse(ref d);

            Assert.IsTrue(success, operation);
            Assert.AreEqual(RemoteDependencyConstants.AzureServiceBus, d.Type, operation);
            Assert.AreEqual(parsedUrl.Host, d.Target, operation);

            // Parse without verb
            d = new DependencyTelemetry(
                dependencyTypeName: RemoteDependencyConstants.HTTP,
                target: parsedUrl.Host,
                dependencyName: parsedUrl.AbsolutePath,
                data: parsedUrl.OriginalString);

            success = AzureServiceBusHttpParser.TryParse(ref d);

            Assert.IsTrue(success, operation);
            Assert.AreEqual(RemoteDependencyConstants.AzureServiceBus, d.Type, operation);
            Assert.AreEqual(parsedUrl.Host, d.Target, operation);
        }
    }
}
