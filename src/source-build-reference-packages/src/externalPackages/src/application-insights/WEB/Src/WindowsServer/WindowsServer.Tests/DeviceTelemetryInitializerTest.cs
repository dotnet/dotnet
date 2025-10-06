#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Net.NetworkInformation;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Assert = Xunit.Assert;

    [TestClass]
    public class DeviceTelemetryInitializerTest
    {
        [TestMethod]
        public void ReadingDeviceUniqueIdYieldsCorrectValue()
        {
            DeviceTelemetryInitializer source = new DeviceTelemetryInitializer();
            var requestTelemetry = new RequestTelemetry();

            Assert.Null(requestTelemetry.Context.Device.Id);

            source.Initialize(requestTelemetry);

            string id = requestTelemetry.Context.Device.Id;

            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string hostName = Dns.GetHostName();

            if (hostName.EndsWith(domainName, StringComparison.OrdinalIgnoreCase) == false)
            {
                hostName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", hostName, domainName);
            }

            Assert.Equal(hostName, id);
        }
    }
}
#endif