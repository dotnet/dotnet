namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Web;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.ApplicationInsights.Web.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SessionTelemetryInitializerTest
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
            var source = new SessionTelemetryInitializer();
            source.Initialize(new EventTelemetry("name"));
        }

        [TestMethod]
        public void InitializeSetsIdForTelemetryUsingIdFromRequestTelemetry()
        {
            var eventTelemetry = new EventTelemetry("name");

            var source = new TestableSessionTelemetryInitializer();
            var context = source.FakeContext.CreateRequestTelemetryPrivate();
            
            context.Context.Session.Id = "1";
            source.Initialize(eventTelemetry);

            Assert.AreEqual("1", eventTelemetry.Context.Session.Id);
        }

        [TestMethod]
        public void InitializeSetsIsFirstForTelemetryUsingIdFromRequestTelemetry()
        {
            var telemetry = new EventTelemetry("name");

            var source = new TestableSessionTelemetryInitializer();
            var context = source.FakeContext.CreateRequestTelemetryPrivate();

            context.Context.Session.IsFirst = true;
            source.Initialize(telemetry);

            Assert.IsTrue(telemetry.Context.Session.IsFirst.HasValue);
            Assert.IsTrue(telemetry.Context.Session.IsFirst.Value);
        }

        [TestMethod]
        public void InitializeDoesNotSetIdIfTelemetryHasIt()
        {
            var eventTelemetry = new EventTelemetry("name");

            var source = new TestableSessionTelemetryInitializer();
            var context = source.FakeContext.CreateRequestTelemetryPrivate();

            context.Context.Session.Id = "1";
            eventTelemetry.Context.Session.Id = "2";
            source.Initialize(eventTelemetry);

            Assert.AreEqual("2", eventTelemetry.Context.Session.Id);
        }

        [TestMethod]
        public void InitializeDoesNotSetIsFirstIfTelemetryHasId()
        {
            var telemetry = new EventTelemetry("name");

            var source = new TestableSessionTelemetryInitializer();
            var context = source.FakeContext.CreateRequestTelemetryPrivate();

            context.Context.Session.IsFirst = true;
            telemetry.Context.Session.IsFirst = false;
            telemetry.Context.Session.Id = "5";
            source.Initialize(telemetry);

            Assert.IsTrue(telemetry.Context.Session.IsFirst.HasValue);
            Assert.IsFalse(telemetry.Context.Session.IsFirst.Value);
        }

        [TestMethod]
        public void NullCookieDoNotInitializeSessionId()
        {
            var requestTelemetry = new RequestTelemetry();

            var initializer = new TestableSessionTelemetryInitializer();
            initializer.FakeContext
                .AddRequestTelemetry(requestTelemetry);

            var telemetry = new EventTelemetry();
            
            initializer.Initialize(telemetry);

            Assert.IsNull(telemetry.Context.Session.Id);
            Assert.IsNull(requestTelemetry.Context.Session.Id);
        }

        [TestMethod]
        public void EmptyCookieDoNotInitializeSessionId()
        {
            var requestTelemetry = new RequestTelemetry();

            var initializer = new TestableSessionTelemetryInitializer();
            initializer.FakeContext
                .AddRequestCookie(new HttpCookie("ai_session", string.Empty))
                .AddRequestTelemetry(requestTelemetry);

            var telemetry = new EventTelemetry();

            initializer.Initialize(telemetry);

            Assert.IsNull(telemetry.Context.Session.Id);
            Assert.IsNull(requestTelemetry.Context.Session.Id);
        }

        [TestMethod]
        public void SimpleCookieWillInitializeSessionId()
        {
            var requestTelemetry = new RequestTelemetry();

            var initializer = new TestableSessionTelemetryInitializer();
            string now = DateTimeOffset.Now.ToString("O", CultureInfo.InvariantCulture);
            initializer.FakeContext
                .AddRequestCookie(new HttpCookie("ai_session", "123|" + now + "|" + now))
                .AddRequestTelemetry(requestTelemetry);

            var telemetry = new EventTelemetry();

            initializer.Initialize(telemetry);

            Assert.AreEqual("123", telemetry.Context.Session.Id);
            Assert.AreEqual("123", requestTelemetry.Context.Session.Id);
        }

        private class TestableSessionTelemetryInitializer : SessionTelemetryInitializer
        {
            private readonly HttpContext fakeContext = HttpModuleHelper.GetFakeHttpContext();

            public TestableSessionTelemetryInitializer()
            {
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