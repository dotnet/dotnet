namespace Microsoft.ApplicationInsights.Web
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Web;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.ApplicationInsights.Web.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ClientIpHeaderTelemetryInitializerTest
    {
        [TestCleanup]
        public void Cleanup()
        {
            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Trace.WriteLine(Assembly.GetExecutingAssembly().FullName);
        }

        [TestMethod]
        public void ConstructorSetsDefaultClientIpHeader()
        {
            var module = new ClientIpHeaderTelemetryInitializer();
            foreach (string headerName in module.HeaderNames)
            {
                Assert.AreEqual("X-Forwarded-For", headerName);
            }
        }

        [TestMethod]
        public void ConstructorSetsLastUserId()
        {
            var module = new ClientIpHeaderTelemetryInitializer();
            Assert.IsTrue(module.UseFirstIp);
        }

        [TestMethod]
        public void ConstructorSetsDefaultHeadersSeparator()
        {
            var module = new ClientIpHeaderTelemetryInitializer();
            Assert.AreEqual(",", module.HeaderValueSeparators);
        }

        [TestMethod]
        public void InitializeDoesNotSetLocationIpIfProvidedInline()
        {
            var module = new TestableClientIpHeaderTelemetryInitializer();
            var telemetry = new ExceptionTelemetry();
            telemetry.Context.Location.Ip = "10.10.10.10";

            module.Initialize(telemetry);

            Assert.AreEqual("10.10.10.10", telemetry.Context.Location.Ip);
        }

        [TestMethod]
        public void InitializeSetsLocationIpToUserHostAddressIfNoHeadersInRequest()
        {
            var module = new TestableClientIpHeaderTelemetryInitializer();
            var telemetry = new EventTelemetry();

            module.Initialize(telemetry);

            Assert.AreEqual("127.0.0.1", telemetry.Context.Location.Ip);
        }

        [TestMethod]
        public void InitializeSetsLocationIpOfRequestToUserHostAddressIfNoHeadersInRequest()
        {
            var module = new TestableClientIpHeaderTelemetryInitializer();
            var requestTelemetry = module.PlatformContext.CreateRequestTelemetryPrivate();

            module.Initialize(new EventTelemetry());

            Assert.AreEqual("127.0.0.1", requestTelemetry.Context.Location.Ip);
        }

        [TestMethod]
        [DataRow("1.2.3.4", "1.2.3.4")]
        [DataRow("[::1]:80", "::1")]
        [DataRow("0:0:0:0:0:0:0:1", "::1")]
        public void InitializeSetsLocationIpToUserHostAddressFromXForwarderHeader(string value, string expected)
        {
            var dictionary = new Dictionary<string, string> { { "X-Forwarded-For", value } };
            var module = new TestableClientIpHeaderTelemetryInitializer(dictionary);
            var telemetry = new TraceTelemetry();

            module.Initialize(telemetry);

            Assert.AreEqual(expected, telemetry.Context.Location.Ip);
        }

        [TestMethod]
        public void InitializeSetsLocationIpOfRequestToUserHostAddressFromXForwarderHeader()
        {
            var dictionary = new Dictionary<string, string> { { "X-Forwarded-For", "1.2.3.4" } };
            var module = new TestableClientIpHeaderTelemetryInitializer(dictionary);
            var requestTelemetry = module.PlatformContext.CreateRequestTelemetryPrivate();

            module.Initialize(new TraceTelemetry("trace"));

            Assert.AreEqual("1.2.3.4", requestTelemetry.Context.Location.Ip);
        }

        [TestMethod]
        public void InitializeCutsPortFromIp()
        {
            var dictionary = new Dictionary<string, string> { { "X-Forwarded-For", "1.2.3.4:54321" } };
            var module = new TestableClientIpHeaderTelemetryInitializer(dictionary);
            var telemetry = new PageViewTelemetry();

            module.Initialize(telemetry);

            Assert.AreEqual("1.2.3.4", telemetry.Context.Location.Ip);
        }

        [TestMethod]
        public void InitializeSetsLocationIpToUserHostAddressIfHeadersIsEmpty()
        {
            var module = new TestableClientIpHeaderTelemetryInitializer(new Dictionary<string, string>());
            var telemetry = new PageViewTelemetry();

            module.Initialize(telemetry);

            Assert.AreEqual("127.0.0.1", telemetry.Context.Location.Ip);
        }

        [TestMethod]
        public void InitializeSetsLocationIpToUserHostAddressIfHeadersHasMalformedIp()
        {
            var dictionary = new Dictionary<string, string> { { "X-Forwarded-For", "bad" } };
            var module = new TestableClientIpHeaderTelemetryInitializer(dictionary);
            var telemetry = new RequestTelemetry();

            module.Initialize(telemetry);

            Assert.AreEqual("127.0.0.1", telemetry.Context.Location.Ip);
        }

        [TestMethod]
        public void InitializeGetsFirstIpIfHeaderHasSeveralIps()
        {
            var dictionary = new Dictionary<string, string> { { "X-Forwarded-For", "1.2.3.4, 2.3.4.5,3.4.5.6" } };
            var module = new TestableClientIpHeaderTelemetryInitializer(dictionary);
            var telemetry = new EventTelemetry();

            module.Initialize(telemetry);

            Assert.AreEqual("1.2.3.4", telemetry.Context.Location.Ip);
        }

        [TestMethod]
        public void InitializeGetsLastIpIfHeaderHasSeveralIpsAndUseFirstIpFalse()
        {
            var dictionary = new Dictionary<string, string> { { "X-Forwarded-For", "1.2.3.4, 2.3.4.5" } };
            var module = new TestableClientIpHeaderTelemetryInitializer(dictionary) { UseFirstIp = false };
            var telemetry = new ExceptionTelemetry();

            module.Initialize(telemetry);

            Assert.AreEqual("2.3.4.5", telemetry.Context.Location.Ip);
        }

        [TestMethod]
        public void InitializeGetsIpFromHeaderProvidedInline()
        {
            var dictionary = new Dictionary<string, string>
            {
                { "X-Forwarded-For", "1.2.3.4, 2.3.4.5" },
                { "CustomHeader", "3.4.5.6" },
            };
            var module = new TestableClientIpHeaderTelemetryInitializer(dictionary);
            module.HeaderNames.Clear();
            module.HeaderNames.Add("CustomHeader");
            var telemetry = new EventTelemetry();

            module.Initialize(telemetry);

            Assert.AreEqual("3.4.5.6", telemetry.Context.Location.Ip);
        }

        [TestMethod]
        public void InitializeGetsIpFromFirstAvailableHeader()
        {
            var dictionary = new Dictionary<string, string>
            {
                { "CustomHeader1", "1.2.3.4, 2.3.4.5" },
                { "CustomHeader2", "3.4.5.6" },
            };
            var module = new TestableClientIpHeaderTelemetryInitializer(dictionary);
            module.HeaderNames.Add("CustomHeader1");
            module.HeaderNames.Add("CustomHeader2");
            var telemetry = new PageViewTelemetry();

            module.Initialize(telemetry);

            Assert.AreEqual("1.2.3.4", telemetry.Context.Location.Ip);
        }

        [TestMethod]
        public void InitializeUsesCustomHeaderValueSeparatorIfProvidedInline()
        {
            var dictionary = new Dictionary<string, string>
            {
                { "X-Forwarded-For", "1.2.3.4;2.3.4.5,3.4.5.6" },
            };
            var module = new TestableClientIpHeaderTelemetryInitializer(dictionary) { HeaderValueSeparators = ",;" };
            var telemetry = new RequestTelemetry();

            module.Initialize(telemetry);

            Assert.AreEqual("1.2.3.4", telemetry.Context.Location.Ip);
        }

        [TestMethod]
        public void InitializeSkipsHeadersThatHasIncorrectIps()
        {
            var dictionary = new Dictionary<string, string>
            {
                { "CustomHeader1", "BAD" },
                { "CustomHeader2", "3.4.5.6" },
            };
            var module = new TestableClientIpHeaderTelemetryInitializer(dictionary);
            module.HeaderNames.Add("CustomHeader1");
            module.HeaderNames.Add("CustomHeader2");
            var telemetry = new TraceTelemetry("trace");

            module.Initialize(telemetry);

            Assert.AreEqual("3.4.5.6", telemetry.Context.Location.Ip);
        }

        private class TestableClientIpHeaderTelemetryInitializer : ClientIpHeaderTelemetryInitializer
        {
            private readonly HttpContext platformContext;

            public TestableClientIpHeaderTelemetryInitializer(IDictionary<string, string> headers = null)
            {
                this.platformContext = HttpModuleHelper.GetFakeHttpContext(headers);
            }

            public HttpContext PlatformContext
            {
                get { return this.platformContext; }
            }

            protected override HttpContext ResolvePlatformContext()
            {
                return this.platformContext;
            }
        }
    }
}
