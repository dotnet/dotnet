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
    public class UserTelemetryInitializerTest
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
            var source = new UserTelemetryInitializer();
            source.Initialize(new EventTelemetry("name"));
        }

        [TestMethod]
        public void InitializeSetsIdForTelemetryUsingIdFromRequestTelemetry()
        {
            var eventTelemetry = new EventTelemetry("name");
            var source = new TestableUserTelemetryInitializer();
            var requestTelemetry = source.FakeContext.CreateRequestTelemetryPrivate();

            requestTelemetry.Context.User.Id = "1";
            source.Initialize(eventTelemetry);

            Assert.AreEqual("1", eventTelemetry.Context.User.Id);
        }

        [TestMethod]
        public void InitializeDoesNotSetIdIfTelemetryHasIt()
        {
            var eventTelemetry = new EventTelemetry("name");
            var source = new TestableUserTelemetryInitializer();
            var requestTelemetry = source.FakeContext.CreateRequestTelemetryPrivate();

            requestTelemetry.Context.User.Id = "1";
            eventTelemetry.Context.User.Id = "2";
            source.Initialize(eventTelemetry);

            Assert.AreEqual("2", eventTelemetry.Context.User.Id);
        }

        [TestMethod]
        public void InitializeReadSessionIdFromSimpleCookie()
        {
            var requestTelemetry = new RequestTelemetry();

            var initializer = new TestableUserTelemetryInitializer();
            initializer.FakeContext.AddRequestCookie(new HttpCookie("ai_user", "123|" + DateTimeOffset.Now.ToString("O", CultureInfo.InvariantCulture)))
                .AddRequestTelemetry(requestTelemetry);

            var telemetry = new EventTelemetry();

            initializer.Initialize(telemetry);

            Assert.AreEqual("123", telemetry.Context.User.Id);
            Assert.AreEqual("123", requestTelemetry.Context.User.Id);
        }

        [TestMethod]
        public void InitializeDoNotReadCookieWithDateOnly()
        {
            var requestTelemetry = new RequestTelemetry();

            var initializer = new TestableUserTelemetryInitializer();
            initializer.FakeContext.AddRequestCookie(new HttpCookie("ai_user", DateTimeOffset.Now.ToString("O", CultureInfo.InvariantCulture)))
                .AddRequestTelemetry(requestTelemetry);

            var telemetry = new EventTelemetry();

            initializer.Initialize(telemetry);

            Assert.AreNotEqual(string.Empty, requestTelemetry.Context.User.Id);
            Assert.AreNotEqual(string.Empty, telemetry.Context.User.Id);
        }

        [TestMethod]
        public void InitializeReadCookieWithMoreThanTwoParts()
        {
            var requestTelemetry = new RequestTelemetry();
            var time = DateTimeOffset.Now.ToString("O", CultureInfo.InvariantCulture);
            var cookie = new HttpCookie("ai_user", "1|" + time + "|3");

            var initializer = new TestableUserTelemetryInitializer();
            initializer.FakeContext.AddRequestCookie(cookie)
                .AddRequestTelemetry(requestTelemetry);

            var telemetry = new EventTelemetry();

            initializer.Initialize(telemetry);

            Assert.AreEqual("1", requestTelemetry.Context.User.Id);
            Assert.AreEqual("1", telemetry.Context.User.Id);
        }

        [TestMethod]
        public void InitializeDoNotReadCookieWhenTimeIsMalformed()
        {
            var requestTelemetry = new RequestTelemetry();
            var time = DateTimeOffset.Now.ToString("O", CultureInfo.InvariantCulture);
            var cookie = new HttpCookie("ai_user", "1|NotATime");

            var initializer = new TestableUserTelemetryInitializer();
            initializer.FakeContext.AddRequestCookie(cookie)
                .AddRequestTelemetry(requestTelemetry);

            var telemetry = new EventTelemetry();

            initializer.Initialize(telemetry);

            Assert.AreNotEqual(string.Empty, requestTelemetry.Context.User.Id);
            Assert.AreNotEqual(string.Empty, telemetry.Context.User.Id);
        }

        [TestMethod]
        public void InitializeDoNotReadCookieFromEmptyValue()
        {
            var requestTelemetry = new RequestTelemetry();

            var initializer = new TestableUserTelemetryInitializer();
            initializer.FakeContext.AddRequestCookie(new HttpCookie("ai_user", string.Empty))
                .AddRequestTelemetry(requestTelemetry);

            var telemetry = new EventTelemetry();

            initializer.Initialize(telemetry);

            Assert.AreNotEqual(string.Empty, requestTelemetry.Context.User.Id);
            Assert.AreNotEqual(string.Empty, telemetry.Context.User.Id);
        }

        private class TestableUserTelemetryInitializer : UserTelemetryInitializer
        {
            private readonly HttpContext fakeContext = HttpModuleHelper.GetFakeHttpContext();

            public TestableUserTelemetryInitializer()
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
