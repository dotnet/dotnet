namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TelemetryChannelTests
    {
        [TestMethod]
        public void InstallAddsServerTelemetryChannel()
        {
            string emptyConfig = ConfigurationHelpers.GetEmptyConfig();
            XDocument configAfterTransform = ConfigurationHelpers.InstallTransform(emptyConfig);
            Trace.WriteLine(configAfterTransform.ToString());
            var typeToFind = typeof(ServerTelemetryChannel);

            var node = ConfigurationHelpers.GetTelemetryChannelFromDefaultSink(configAfterTransform)
                .FirstOrDefault(element => element.Attribute("Type").Value == ConfigurationHelpers.GetPartialTypeName(typeToFind));

            Assert.IsNotNull(node);
        }

        [TestMethod]
        public void UninstallRemovesTelemetryChannel()
        {
            string emptyConfig = ConfigurationHelpers.GetEmptyConfig();
            XDocument configAfterInstall = ConfigurationHelpers.InstallTransform(emptyConfig);            
            XDocument configAfterUninstall = ConfigurationHelpers.UninstallTransform(configAfterInstall.ToString());
            Trace.WriteLine(configAfterUninstall.ToString());
            Assert.AreEqual(0, ConfigurationHelpers.GetTelemetryChannelFromDefaultSink(configAfterUninstall).ToList().Count);
        }
    }
}