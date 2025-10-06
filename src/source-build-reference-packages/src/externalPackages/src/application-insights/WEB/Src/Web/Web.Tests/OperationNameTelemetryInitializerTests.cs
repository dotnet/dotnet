namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Diagnostics;
    using System.Web;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.ApplicationInsights.Web.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class OperationNameTelemetryInitializerTests
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
            var requestTelemetry = new RequestTelemetry();

            var source = new OperationNameTelemetryInitializer();
            source.Initialize(requestTelemetry);

            Assert.AreEqual(string.Empty, requestTelemetry.Name);
        }

        public void TestRequestNameWithControllerAndWithAction()
        {
            var platformContext = HttpModuleHelper.GetFakeHttpContext();

            platformContext.Request.RequestContext.RouteData.Values.Add("controller", "Controller");
            platformContext.Request.RequestContext.RouteData.Values.Add("action", "Action");
            platformContext.Request.RequestContext.RouteData.Values.Add("id2", 10);
            platformContext.Request.RequestContext.RouteData.Values.Add("id1", 10);

            string requestName = platformContext.CreateRequestNamePrivate();
            Assert.AreEqual("GET Controller/Action", requestName);
        }

        public void TestRequestNameWithNoControllerAndWithAction()
        {
            var platformContext = HttpModuleHelper.GetFakeHttpContext();

            platformContext.Request.RequestContext.RouteData.Values.Add("action", "Action");
            platformContext.Request.RequestContext.RouteData.Values.Add("id2", 10);
            platformContext.Request.RequestContext.RouteData.Values.Add("id1", 10);

            string requestName = platformContext.CreateRequestNamePrivate();
            Assert.AreEqual("GET " + HttpModuleHelper.UrlPath, requestName);
        }

        public void TestRequestNameWithControllerAndNoActionNoParameters()
        {
            var platformContext = HttpModuleHelper.GetFakeHttpContext();

            platformContext.Request.RequestContext.RouteData.Values.Add("controller", "Controller");
            
            string requestName = platformContext.CreateRequestNamePrivate();
            Assert.AreEqual("GET Controller", requestName);
        }

        public void TestRequestNameWithControllerAndWithNoActionWithParameters()
        {
            var platformContext = HttpModuleHelper.GetFakeHttpContext();

            platformContext.Request.RequestContext.RouteData.Values.Add("controller", "Controller");
            
            // Note that parameters are not sorted here:
            platformContext.Request.RequestContext.RouteData.Values.Add("id2", 10);
            platformContext.Request.RequestContext.RouteData.Values.Add("id1", 10);

            string requestName = platformContext.CreateRequestNamePrivate();
            Assert.AreEqual("GET Controller [id1/id2]", requestName);
        }

        public void TestRequestNameWithNoControllerAndWithNoAction()
        {
            var platformContext = HttpModuleHelper.GetFakeHttpContext();

            platformContext.Request.RequestContext.RouteData.Values.Add("id2", 10);
            platformContext.Request.RequestContext.RouteData.Values.Add("id1", 10);

            string requestName = platformContext.CreateRequestNamePrivate();
            Assert.AreEqual("GET " + HttpModuleHelper.UrlPath, requestName);
        }

        public void TestRequestNameRouteDataEmpty()
        {
            var platformContext = HttpModuleHelper.GetFakeHttpContext();

            string requestName = platformContext.CreateRequestNamePrivate();
            Assert.AreEqual("GET " + HttpModuleHelper.UrlPath, requestName);
        }

        [TestMethod]
        public void InitializeSetsRequestName()
        {
            var requestTelemetry = CreateRequestTelemetry();
            var source = new TestableOperationNameTelemetryInitializer();
            source.FakeContext.CreateRequestTelemetryPrivate();

            source.Initialize(requestTelemetry);

            Assert.AreEqual("GET " + HttpModuleHelper.UrlPath, requestTelemetry.Name);
        }

        [TestMethod]
        public void InitializeSetsRequestOperationName()
        {
            var requestTelemetry = CreateRequestTelemetry();
            var source = new TestableOperationNameTelemetryInitializer();
            source.FakeContext.CreateRequestTelemetryPrivate();
            
            source.Initialize(requestTelemetry);

            Assert.AreEqual("GET " + HttpModuleHelper.UrlPath, requestTelemetry.Context.Operation.Name);
        }

        [TestMethod]
        public void InitializeSetsCustomerRequestOperationNameFromContextIfRootRequestNameIsEmpty()
        {
            var source = new TestableOperationNameTelemetryInitializer();
            var rootRequest = source.FakeContext.CreateRequestTelemetryPrivate();
            Assert.AreEqual(string.Empty, rootRequest.Name);
            RequestTelemetry customerRequestTelemetry = new RequestTelemetry();

            source.Initialize(customerRequestTelemetry);

            Assert.AreEqual("GET " + HttpModuleHelper.UrlPath, customerRequestTelemetry.Context.Operation.Name);
        }

        [TestMethod]
        public void InitializeSetsCustomerRequestOperationNameFromRequestIfRequestNameIsNotEmpty()
        {
            var source = new TestableOperationNameTelemetryInitializer();
            var rootRequest = source.FakeContext.CreateRequestTelemetryPrivate();
            rootRequest.Name = "Test";
            RequestTelemetry customerRequestTelemetry = new RequestTelemetry();

            source.Initialize(customerRequestTelemetry);

            Assert.AreEqual("Test", customerRequestTelemetry.Context.Operation.Name);
        }

        [TestMethod]
        public void InitializeDoesNotOverrideCustomerRequestName()
        {
            var source = new TestableOperationNameTelemetryInitializer();
            source.FakeContext.CreateRequestTelemetryPrivate();
            RequestTelemetry customerRequestTelemetry = new RequestTelemetry("name", DateTimeOffset.UtcNow, TimeSpan.FromSeconds(42), "404", true);

            source.Initialize(customerRequestTelemetry);

            Assert.AreEqual("name", customerRequestTelemetry.Name);
        }

        [TestMethod]
        public void InitializeDoesNotOverrideCustomerOperationName()
        {
            var source = new TestableOperationNameTelemetryInitializer();
            source.FakeContext.CreateRequestTelemetryPrivate();
            var customerTelemetry = new TraceTelemetry("Text");
            customerTelemetry.Context.Operation.Name = "Name";

            source.Initialize(customerTelemetry);

            Assert.AreEqual("Name", customerTelemetry.Context.Operation.Name);
        }

        [TestMethod]
        public void InitializeSetsExceptionOperationName()
        {
            var exceptionTelemetry = CreateExceptionTelemetry();
            var source = new TestableOperationNameTelemetryInitializer();
            source.FakeContext.CreateRequestTelemetryPrivate();

            source.Initialize(exceptionTelemetry);

            Assert.AreEqual("GET " + HttpModuleHelper.UrlPath, exceptionTelemetry.Context.Operation.Name);
        }

        [TestMethod]
        public void InitializeSetsOperationNameWhenRequestTelemetryIsMissingInHttpContext()
        {
            var telemetry = CreateRequestTelemetry();

            var source = new TestableOperationNameTelemetryInitializer();
            source.Initialize(telemetry);

            Assert.AreEqual("GET " + HttpModuleHelper.UrlPath, telemetry.Context.Operation.Name);
        }

        private static RequestTelemetry CreateRequestTelemetry()
        {
            var item = new RequestTelemetry
            {
                Timestamp = DateTimeOffset.Now,
                Name = string.Empty,
                Duration = TimeSpan.FromSeconds(4),
                ResponseCode = "200",
                Success = true,
                Url = new Uri("http://localhost/myapp/MyPage.aspx"),
            };

            item.Context.InstrumentationKey = Guid.NewGuid().ToString();

            item.Metrics.Add("Metric1", 30);
            item.Properties.Add("httpMethod", "GET");
            item.Properties.Add("userHostAddress", "::1");

            return item;
        }

        private static ExceptionTelemetry CreateExceptionTelemetry(Exception exception = null)
        {
            if (exception == null)
            {
                exception = new Exception();
            }

            ExceptionTelemetry output = new ExceptionTelemetry(exception)
            {
                Timestamp = DateTimeOffset.UtcNow,
            };

            output.Context.InstrumentationKey = "required";

            return output;
        }

        private class TestableOperationNameTelemetryInitializer : OperationNameTelemetryInitializer
        {
            private readonly HttpContext fakeContext = HttpModuleHelper.GetFakeHttpContext();

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