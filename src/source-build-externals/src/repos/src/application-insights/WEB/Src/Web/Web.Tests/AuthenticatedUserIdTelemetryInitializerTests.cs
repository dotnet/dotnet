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
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AuthenticatedUserIdTelemetryInitializerTests
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
            // Arrange
            HttpContext.Current = null;
            var source = new AuthenticatedUserIdTelemetryInitializer();

            // Act
            var eventTelemetry = new EventTelemetry("name");
            source.Initialize(eventTelemetry);

            // Assert
            Assert.AreEqual("name", eventTelemetry.Name);
        }

        [TestMethod]
        public void InitializeSetsIdForTelemetryUsingIdFromRequestTelemetry()
        {
            // Arrange
            var eventTelemetry = new EventTelemetry("name");
            var source = new TestableAuthenticatedUserIdTelemetryInitializer();
            RequestTelemetry requestTelemetry = source.FakeContext.CreateRequestTelemetryPrivate();
            requestTelemetry.Context.User.AuthenticatedUserId = "1";

            // Act
            source.Initialize(eventTelemetry);

            // Assert
            Assert.AreEqual("1", eventTelemetry.Context.User.AuthenticatedUserId);
        }

        [TestMethod]
        public void InitializeDoesNotSetIdIfTelemetryHasIt()
        {
            // Arrange
            var eventTelemetry = new EventTelemetry("name");
            var source = new TestableAuthenticatedUserIdTelemetryInitializer();
            RequestTelemetry requestTelemetry = source.FakeContext.CreateRequestTelemetryPrivate();
            requestTelemetry.Context.User.AuthenticatedUserId = "1";
            eventTelemetry.Context.User.AuthenticatedUserId = "2";

            // Act
            source.Initialize(eventTelemetry);

            // Assert
            Assert.AreEqual("2", eventTelemetry.Context.User.AuthenticatedUserId);
        }

        [TestMethod]
        public void InitializeDoesNotSetAuthIdIfCookieIsEmpty()
        {
            // Arrange
            var initializer = new TestableAuthenticatedUserIdTelemetryInitializer();
            var cookieString = string.Empty;
            RequestTelemetry requestTelemetry = initializer.FakeContext.WithAuthCookie(cookieString);

            // Act
            initializer.Initialize(new StubTelemetry());

            // Assert
            Assert.AreEqual(null, requestTelemetry.Context.User.AuthenticatedUserId);
        }

        [TestMethod]
        public void InitializeDoesNotSetAuthIdIfCookieINull()
        {
            // Arrange
            var initializer = new TestableAuthenticatedUserIdTelemetryInitializer();
            string cookieString = null;
            RequestTelemetry requestTelemetry = initializer.FakeContext.WithAuthCookie(cookieString);

            // Act
            initializer.Initialize(new StubTelemetry());

            // Assert
            Assert.AreEqual(null, requestTelemetry.Context.User.AuthenticatedUserId);
        }

        [TestMethod]
        public void InitializeReadsAuthIdFromSimpleCookie()
        {
            // Arrange
            var initializer = new TestableAuthenticatedUserIdTelemetryInitializer();
            var cookieString = "123|account123";
            RequestTelemetry requestTelemetry = initializer.FakeContext.WithAuthCookie(cookieString);

            // Act
            initializer.Initialize(new StubTelemetry());

            // Assert
            Assert.AreEqual("123", requestTelemetry.Context.User.AuthenticatedUserId);
        }

        [TestMethod]
        public void InitializeReadsAuthIdFromNonAsciiCharactersInCookie()
        {
            // Arrange
            var initializer = new TestableAuthenticatedUserIdTelemetryInitializer();
            var cookieString = "123实|account123";
            RequestTelemetry requestTelemetry = initializer.FakeContext.WithAuthCookie(cookieString);

            // Act
            initializer.Initialize(new StubTelemetry());

            // Assert
            Assert.AreEqual("123实", requestTelemetry.Context.User.AuthenticatedUserId);
        }

        [TestMethod]
        public void InitializeReadsAuthIdFromSpecialCharactersInCookie()
        {
            // Arrange
            var initializer = new TestableAuthenticatedUserIdTelemetryInitializer();
            var cookieString = "$#@!!!!|account123";
            RequestTelemetry requestTelemetry = initializer.FakeContext.WithAuthCookie(cookieString);

            // Act
            initializer.Initialize(new StubTelemetry());

            // Assert
            Assert.AreEqual("$#@!!!!", requestTelemetry.Context.User.AuthenticatedUserId);
        }

        [TestMethod]
        public void InitializeHandleAuthIdFromMalformedCookie()
        {
            // Arrange
            var initializer = new TestableAuthenticatedUserIdTelemetryInitializer();
            var cookieString = "|";
            RequestTelemetry requestTelemetry = initializer.FakeContext.WithAuthCookie(cookieString);

            // Act
            initializer.Initialize(new StubTelemetry());

            // Assert
            Assert.AreEqual(null, requestTelemetry.Context.User.AuthenticatedUserId);
        }

        private class TestableAuthenticatedUserIdTelemetryInitializer : AuthenticatedUserIdTelemetryInitializer
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
