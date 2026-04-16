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
    public class AccountIdTelemetryInitializerTests
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
            var source = new AccountIdTelemetryInitializer();

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
            var source = new TestableAccountIdTelemetryInitializer();
            RequestTelemetry requestTelemetry = source.FakeContext.CreateRequestTelemetryPrivate();
            requestTelemetry.Context.User.AccountId = "1";
            
            // Act
            source.Initialize(eventTelemetry);

            // Assert
            Assert.AreEqual("1", eventTelemetry.Context.User.AccountId);
        }

        [TestMethod]
        public void InitializeDoesNotSetIdIfTelemetryHasIt()
        {
            // Arrange
            var eventTelemetry = new EventTelemetry("name");
            var source = new TestableAccountIdTelemetryInitializer();
            RequestTelemetry requestTelemetry = source.FakeContext.CreateRequestTelemetryPrivate();
            requestTelemetry.Context.User.AccountId = "1";
            eventTelemetry.Context.User.AccountId = "2";

            // Act
            source.Initialize(eventTelemetry);

            // Assert
            Assert.AreEqual("2", eventTelemetry.Context.User.AccountId);
        }

        [TestMethod]
        public void InitializeDoesNotSetAccountIdIfCookieDoesNotHaveIt()
        {
            // Arrange
            var initializer = new TestableAccountIdTelemetryInitializer();
            var cookieString = "123";
            RequestTelemetry requestTelemetry = initializer.FakeContext.WithAuthCookie(cookieString);

            // Act
            initializer.Initialize(new StubTelemetry());

            // Assert
            Assert.AreEqual(null, requestTelemetry.Context.User.AccountId);
        }

        [TestMethod]
        public void InitializeDoesNotSetAccountIdIfCookieIsMalformed()
        {
            // Arrange
            var initializer = new TestableAccountIdTelemetryInitializer();
            var cookieString = "123|";
            RequestTelemetry requestTelemetry = initializer.FakeContext.WithAuthCookie(cookieString);

            // Act
            initializer.Initialize(new StubTelemetry());

            // Assert
            Assert.AreEqual(null, requestTelemetry.Context.User.AccountId);
        }

        [TestMethod]
        public void InitializeDoesNotSetAccountIdIfCookieIsEmpty()
        {
            // Arrange
            var initializer = new TestableAccountIdTelemetryInitializer();
            var cookieString = string.Empty;
            RequestTelemetry requestTelemetry = initializer.FakeContext.WithAuthCookie(cookieString);

            // Act
            initializer.Initialize(new StubTelemetry());

            // Assert
            Assert.AreEqual(null, requestTelemetry.Context.User.AccountId);
        }

        [TestMethod]
        public void InitializeReadsAccountIdFromSimpleCookie()
        {
            // Arrange
            var initializer = new TestableAccountIdTelemetryInitializer();
            var cookieString = "123|account123";
            RequestTelemetry requestTelemetry = initializer.FakeContext.WithAuthCookie(cookieString);

            // Act
            initializer.Initialize(new EventTelemetry());

            // Assert
            Assert.AreEqual("account123", requestTelemetry.Context.User.AccountId);
        }

        [TestMethod]
        public void InitializeReadsAccountIdFromNonAsciiCharactersInCookie()
        {
            // Arrange
            var initializer = new TestableAccountIdTelemetryInitializer();
            var cookieString = "123|account123א";
            RequestTelemetry requestTelemetry = initializer.FakeContext.WithAuthCookie(cookieString);

            // Act
            initializer.Initialize(new StubTelemetry());

            // Assert
            Assert.AreEqual("account123א", requestTelemetry.Context.User.AccountId);
        }

        [TestMethod]
        public void InitializeReadsAccountIdFromSpecialCharactersInCookie()
        {
            // Arrange
            var initializer = new TestableAccountIdTelemetryInitializer();
            var cookieString = "123|$#@!!!!";
            RequestTelemetry requestTelemetry = initializer.FakeContext.WithAuthCookie(cookieString);

            // Act
            initializer.Initialize(new StubTelemetry());

            // Assert
            Assert.AreEqual("$#@!!!!", requestTelemetry.Context.User.AccountId);
        }

        private class TestableAccountIdTelemetryInitializer : AccountIdTelemetryInitializer
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
