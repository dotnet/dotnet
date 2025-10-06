namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Web.Helpers;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class RequestTrackingUtilitiesTest
    {
        private delegate bool GetAppIdReturns(string ikey, out string appId);

        [TestMethod]
        public void UpdateRequestTelemetryFromRequestUpdatesTelemetryItems()
        {
            var requestTelemetry = new RequestTelemetry();

            var headers = new Dictionary<string, string>
            {
                { RequestResponseHeaders.RequestContextHeader, $"{RequestResponseHeaders.RequestContextCorrelationSourceKey}=sourceAppId" }
            };
            var context = HttpModuleHelper.GetFakeHttpContext(headers);

            var appIdProvider = GetMockAppIdProvider("currentAppId");

            RequestTrackingUtilities.UpdateRequestTelemetryFromRequest(requestTelemetry, context.Request, appIdProvider.Object);

            Assert.AreEqual(context.Request.Unvalidated.Url, requestTelemetry.Url);
            Assert.AreEqual("sourceAppId", requestTelemetry.Source);
        }

        [TestMethod]
        public void UpdateRequestTelemetryFromRequestDoesntSetSourceIfSame()
        {
            var requestTelemetry = new RequestTelemetry();

            var headers = new Dictionary<string, string>
            {
                { RequestResponseHeaders.RequestContextHeader, $"{RequestResponseHeaders.RequestContextCorrelationSourceKey}=currentAppId" }
            };
            var context = HttpModuleHelper.GetFakeHttpContext(headers);

            var appIdProvider = GetMockAppIdProvider("currentAppId");

            RequestTrackingUtilities.UpdateRequestTelemetryFromRequest(requestTelemetry, context.Request, appIdProvider.Object);

            Assert.AreEqual(context.Request.Unvalidated.Url, requestTelemetry.Url);
            Assert.AreEqual(string.Empty, requestTelemetry.Source, "If Source and CurrentAppId match, the requestTelemetry.Source must be empty");
        }

        [TestMethod]
        public void UpdateRequestTelemetryFromRequestDoesntOverwriteIfAlreadyExists()
        {
            var requestTelemetry = new RequestTelemetry
            {
                Url = new Uri("http://shouldPreserveUrl"),
                Source = "shouldPreserveSource"
            };

            var headers = new Dictionary<string, string>
            {
                { RequestResponseHeaders.RequestContextHeader, $"{RequestResponseHeaders.RequestContextCorrelationSourceKey}=sourceAppId" }
            };
            var context = HttpModuleHelper.GetFakeHttpContext(headers);

            var appIdProvider = GetMockAppIdProvider("currentAppId");

            RequestTrackingUtilities.UpdateRequestTelemetryFromRequest(requestTelemetry, context.Request, appIdProvider.Object);

            Assert.AreEqual(new Uri("http://shouldPreserveUrl"), requestTelemetry.Url);
            Assert.AreEqual("shouldPreserveSource", requestTelemetry.Source);
        }

        [TestMethod]
        public void UpdateRequestTelemetryFromRequestHandlesNulls()
        {
            var requestTelemetry = new RequestTelemetry();
            var request = HttpModuleHelper.GetFakeHttpContext().Request;
            var appIdProvider = GetMockAppIdProvider("currentAppId").Object;
            RequestTrackingUtilities.UpdateRequestTelemetryFromRequest(null, request, appIdProvider);
            RequestTrackingUtilities.UpdateRequestTelemetryFromRequest(requestTelemetry, null, appIdProvider);
        }

        [TestMethod]
        public void UpdateRequestTelemetryFromRequestDoesntUpdateSourceIfAppIdProviderIsNull()
        {
            var requestTelemetry = new RequestTelemetry();

            var headers = new Dictionary<string, string>
            {
                { RequestResponseHeaders.RequestContextHeader, $"{RequestResponseHeaders.RequestContextCorrelationSourceKey}=sourceAppId" }
            };
            var context = HttpModuleHelper.GetFakeHttpContext(headers);

            RequestTrackingUtilities.UpdateRequestTelemetryFromRequest(requestTelemetry, context.Request, applicationIdProvider: null);

            Assert.AreEqual(context.Request.Unvalidated.Url, requestTelemetry.Url);
            Assert.AreEqual(string.Empty, requestTelemetry.Source, "If appIdProvider is null, requestTelemetry.Source should not be set");
        }

        private static Mock<IApplicationIdProvider> GetMockAppIdProvider(string appIdToReturn)
        {
            var appIdProvider = new Mock<IApplicationIdProvider>();
            appIdProvider
                .Setup(ap => ap.TryGetApplicationId(It.IsAny<string>(), out It.Ref<string>.IsAny))
                .Returns(new GetAppIdReturns((string ikey, out string id) =>
                {
                    id = appIdToReturn;
                    return true;
                }));
            return appIdProvider;
        }
    }
}
