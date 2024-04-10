namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DependencyTelemetryExtensionsTests
    {
        [TestMethod]
        public void DependencyTelemetryGetSetHttpRequestOperationDetail()
        {
            var detail = new HttpRequestMessage();

            var telemetry = this.CreateRemoteDependencyTelemetry("mycommand");
            telemetry.SetOperationDetail(OperationDetailConstants.HttpRequestOperationDetailName, detail);
            Assert.IsTrue(telemetry.TryGetHttpRequestOperationDetail(out var retrievedValue));
            Assert.IsNotNull(retrievedValue);
            Assert.AreEqual(detail, retrievedValue);

            // Clear and verify the detail is no longer present            
            new TelemetryClient(TelemetryConfiguration.CreateDefault()).TrackDependency(telemetry);
            Assert.IsFalse(telemetry.TryGetHttpRequestOperationDetail(out retrievedValue));
        }

        [TestMethod]
        public void DependencyTelemetryGetWrongTypeHttpRequestOperationDetail()
        {
            const string detail = "bar";

            var telemetry = this.CreateRemoteDependencyTelemetry("mycommand");
            telemetry.SetOperationDetail(OperationDetailConstants.HttpRequestOperationDetailName, detail);
            Assert.IsFalse(telemetry.TryGetHttpRequestOperationDetail(out var retrievedValue));
            Assert.IsNull(retrievedValue);

            // should not throw                        
            new TelemetryClient(TelemetryConfiguration.CreateDefault()).TrackDependency(telemetry);
        }

        [TestMethod]
        public void DependencyTelemetryGetUnsetHttpRequestOperationDetail()
        {
            var telemetry = this.CreateRemoteDependencyTelemetry("mycommand");
            Assert.IsFalse(telemetry.TryGetHttpRequestOperationDetail(out var retrievedValue));
            Assert.IsNull(retrievedValue);

            // should not throw                        
            new TelemetryClient(TelemetryConfiguration.CreateDefault()).TrackDependency(telemetry);
        }

        [TestMethod]
        public void DependencyTelemetryGetSetHttpResponseOperationDetail()
        {
            var detail = new HttpResponseMessage();

            var telemetry = this.CreateRemoteDependencyTelemetry("mycommand");
            telemetry.SetOperationDetail(OperationDetailConstants.HttpResponseOperationDetailName, detail);
            Assert.IsTrue(telemetry.TryGetHttpResponseOperationDetail(out var retrievedValue));
            Assert.IsNotNull(retrievedValue);
            Assert.AreEqual(detail, retrievedValue);

            // Clear and verify the detail is no longer present            
            new TelemetryClient(TelemetryConfiguration.CreateDefault()).TrackDependency(telemetry);
            Assert.IsFalse(telemetry.TryGetHttpResponseOperationDetail(out retrievedValue));
        }

        [TestMethod]
        public void DependencyTelemetryGetWrongTypeHttpResponseOperationDetail()
        {
            const string detail = "bar";

            var telemetry = this.CreateRemoteDependencyTelemetry("mycommand");
            telemetry.SetOperationDetail(OperationDetailConstants.HttpResponseOperationDetailName, detail);
            Assert.IsFalse(telemetry.TryGetHttpResponseOperationDetail(out var retrievedValue));
            Assert.IsNull(retrievedValue);

            // should not throw                        
            new TelemetryClient(TelemetryConfiguration.CreateDefault()).TrackDependency(telemetry);
        }

        [TestMethod]
        public void DependencyTelemetryGetUnsetHttpResponseOperationDetail()
        {
            var telemetry = this.CreateRemoteDependencyTelemetry("mycommand");
            Assert.IsFalse(telemetry.TryGetHttpResponseOperationDetail(out var retrievedValue));
            Assert.IsNull(retrievedValue);

            // should not throw                        
            new TelemetryClient(TelemetryConfiguration.CreateDefault()).TrackDependency(telemetry);
        }

        [TestMethod]
        public void DependencyTelemetryGetSetHttpResponseHeadersOperationDetail()
        {
            var detail = new WebHeaderCollection();

            var telemetry = this.CreateRemoteDependencyTelemetry("mycommand");
            telemetry.SetOperationDetail(OperationDetailConstants.HttpResponseHeadersOperationDetailName, detail);
            Assert.IsTrue(telemetry.TryGetHttpResponseHeadersOperationDetail(out var retrievedValue));
            Assert.IsNotNull(retrievedValue);
            Assert.AreEqual(detail, retrievedValue);

            // Clear and verify the detail is no longer present            
            new TelemetryClient(TelemetryConfiguration.CreateDefault()).TrackDependency(telemetry);
            Assert.IsFalse(telemetry.TryGetHttpResponseHeadersOperationDetail(out retrievedValue));
        }

        [TestMethod]
        public void DependencyTelemetryGetWrongTypeHttpResponseHeadersOperationDetail()
        {
            const string detail = "bar";

            var telemetry = this.CreateRemoteDependencyTelemetry("mycommand");
            telemetry.SetOperationDetail(OperationDetailConstants.HttpResponseHeadersOperationDetailName, detail);
            Assert.IsFalse(telemetry.TryGetHttpResponseHeadersOperationDetail(out var retrievedValue));
            Assert.IsNull(retrievedValue);

            // should not throw                        
            new TelemetryClient(TelemetryConfiguration.CreateDefault()).TrackDependency(telemetry);
        }

        [TestMethod]
        public void DependencyTelemetryGetUnsetHttpResponseHeadersOperationDetail()
        {
            var telemetry = this.CreateRemoteDependencyTelemetry("mycommand");
            Assert.IsFalse(telemetry.TryGetHttpResponseHeadersOperationDetail(out var retrievedValue));
            Assert.IsNull(retrievedValue);

            // should not throw                        
            new TelemetryClient(TelemetryConfiguration.CreateDefault()).TrackDependency(telemetry);
        }
        private DependencyTelemetry CreateRemoteDependencyTelemetry()
        {
            DependencyTelemetry item = new DependencyTelemetry
            {
                Timestamp = DateTimeOffset.Now,
                Sequence = "4:2",
                Name = "MyWebServer.cloudapp.net",
                Duration = TimeSpan.FromMilliseconds(42),
                Success = true,
                Id = "DepID",
                ResultCode = "200",
                Type = "external call"
            };
            item.Context.InstrumentationKey = Guid.NewGuid().ToString();
            item.Properties.Add("TestProperty", "TestValue");
            item.Context.GlobalProperties.Add("TestPropertyGlobal", "TestValue");
            return item;
        }
        private DependencyTelemetry CreateRemoteDependencyTelemetry(string commandName)
        {
            DependencyTelemetry item = this.CreateRemoteDependencyTelemetry();
            item.Data = commandName;
            return item;
        }
    }
}
