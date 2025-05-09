namespace Microsoft.ApplicationInsights.Web
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Web;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.ApplicationInsights.Web.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class OperationCorrelationTelemetryInitializerTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }
        }

        [TestMethod]
        public void InitializeDoesNotThrowWhenHttpContextIsNull()
        {
            var source = new OperationCorrelationTelemetryInitializer();
            source.Initialize(new RequestTelemetry());
        }

        [TestMethod]
        public void DefaultHeadersOperationCorrelationTelemetryInitializerAreNotSetByDefault()
        {
            var initializer = new OperationCorrelationTelemetryInitializer();
            Assert.IsNull(initializer.ParentOperationIdHeaderName);
            Assert.IsNull(initializer.RootOperationIdHeaderName);
        }

        [TestMethod]
        public void CustomHeadersOperationCorrelationTelemetryInitializerAreSetProperly()
        {
            var initializer = new OperationCorrelationTelemetryInitializer();
            initializer.ParentOperationIdHeaderName = "myParentHeader";
            initializer.RootOperationIdHeaderName = "myRootHeader";

            Assert.AreEqual("myParentHeader", ActivityHelpers.ParentOperationIdHeaderName);
            Assert.AreEqual("myRootHeader", ActivityHelpers.RootOperationIdHeaderName);

            Assert.AreEqual("myParentHeader", initializer.ParentOperationIdHeaderName);
            Assert.AreEqual("myRootHeader", initializer.RootOperationIdHeaderName);
        }

        [TestMethod]
        public void OperationContextIsSetForNonRequestTelemetry()
        {
            var activity = new Activity("request")
                .SetParentId("|guid.1")
                .AddBaggage("k1", "v1")
                .AddBaggage("k2", "v2")
                .AddBaggage("k1", "v3")
                .Start();

            var source = new TestableOperationCorrelationTelemetryInitializer();

            // simulate OnBegin behavior:
            // create telemetry and start activity for children
            var requestTelemetry = source.FakeContext.CreateRequestTelemetryPrivate();
            
            // lost Acitivity / call context
            activity.Stop();

            var exceptionTelemetry = new ExceptionTelemetry();
            source.Initialize(exceptionTelemetry);

            Assert.AreEqual(requestTelemetry.Context.Operation.Id, exceptionTelemetry.Context.Operation.Id);
            Assert.AreEqual(requestTelemetry.Id, exceptionTelemetry.Context.Operation.ParentId);

            Assert.AreEqual(2, exceptionTelemetry.Properties.Count);

            // undefined behavior for duplicates
            Assert.IsTrue(exceptionTelemetry.Properties["k1"] == "v3" || exceptionTelemetry.Properties["k1"] == "v1");
            Assert.AreEqual("v2", exceptionTelemetry.Properties["k2"]);
        }

        [TestMethod]
        public void OperationContextIsNotUpdatedIfOperationIdIsSet()
        {
            var activity = new Activity("request")
                .SetParentId("|guid.1")
                .AddBaggage("k1", "v1")
                .Start();

            var source = new TestableOperationCorrelationTelemetryInitializer();

            // create telemetry and immediately clean call context/activity
            source.FakeContext.CreateRequestTelemetryPrivate();

            activity.Stop();

            var exceptionTelemetry = new ExceptionTelemetry();
            exceptionTelemetry.Context.Operation.Id = "guid";
            source.Initialize(exceptionTelemetry);

            Assert.IsNull(exceptionTelemetry.Context.Operation.ParentId);

            Assert.AreEqual(0, exceptionTelemetry.Properties.Count);
        }

        private class TestableOperationCorrelationTelemetryInitializer : OperationCorrelationTelemetryInitializer
        {
            private readonly HttpContext fakeContext;

            public TestableOperationCorrelationTelemetryInitializer(IDictionary<string, string> headers = null)
            {
                this.fakeContext = HttpModuleHelper.GetFakeHttpContext(headers);
            }

            public HttpContext FakeContext
            {
                get { return this.fakeContext; }
            }

            protected override HttpContext ResolvePlatformContext()
            {
                return this.fakeContext;
            }
        }
    }
}