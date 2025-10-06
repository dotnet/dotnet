namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System.IO;
    using System.Xml.Linq;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BuildInfoConfigComponentVersionTelemetryInitializerTest
    {
        [TestMethod]
        public void InitializeDoesNotThrowIfFileDoesNotExist()
        {
            var source = new BuildInfoConfigComponentVersionTelemetryInitializer();
            source.Initialize(new RequestTelemetry());
        }

        [TestMethod]
        public void InitializeSetsNullVersionIfBuildRootInfoNull()
        {
            var source = new BuildInfoConfigComponentVersionTelemetryInitializer();
            var requestTelemetry = new RequestTelemetry();
            source.Initialize(requestTelemetry);
            
            Assert.IsNull(requestTelemetry.Context.Component.Version);
        }

        [TestMethod]
        public void GetVersionReturnsNullVersionIfXmlDoesNotHaveLabel()
        {
            var doc = XDocument.Load(new StringReader("<DeploymentEvent></DeploymentEvent>")).Root;

            var source = new BuildInfoConfigComponentVersionTelemetryInitializerMock(doc);
            var requestTelemetry = new RequestTelemetry();
            source.Initialize(requestTelemetry);

            Assert.IsNull(requestTelemetry.Context.Component.Version);
        }

        [TestMethod]
        public void GetVersionReturnsNullVersionIfLabelIsEmpty()
        {
            var doc = XDocument.Load(new StringReader("<DeploymentEvent><Build><MSBuild><BuildLabel></BuildLabel></MSBuild></Build></DeploymentEvent>")).Root;

            var source = new BuildInfoConfigComponentVersionTelemetryInitializerMock(doc);
            var requestTelemetry = new RequestTelemetry();
            source.Initialize(requestTelemetry);

            Assert.IsNull(requestTelemetry.Context.Component.Version);
        }

        [TestMethod]
        public void GetVersionReturnsCorrectVersion()
        {
            var doc = XDocument.Load(new StringReader("<DeploymentEvent><Build><MSBuild><BuildLabel>123</BuildLabel></MSBuild></Build></DeploymentEvent>")).Root;

            var source = new BuildInfoConfigComponentVersionTelemetryInitializerMock(doc);
            var requestTelemetry = new RequestTelemetry();
            source.Initialize(requestTelemetry);
            Assert.AreEqual("123", requestTelemetry.Context.Component.Version);
        }

        private class BuildInfoConfigComponentVersionTelemetryInitializerMock : BuildInfoConfigComponentVersionTelemetryInitializer
        {
            private readonly XElement element;

            public BuildInfoConfigComponentVersionTelemetryInitializerMock(XElement element)
            {
                this.element = element;
            }

            protected override XElement LoadBuildInfoConfig()
            {
                return this.element;
            }
        }
    }
}
