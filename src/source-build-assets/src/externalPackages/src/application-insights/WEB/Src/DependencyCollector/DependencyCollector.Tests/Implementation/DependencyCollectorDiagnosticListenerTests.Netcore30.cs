namespace Microsoft.ApplicationInsights.Tests
{
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;

    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// .NET Core 3.0 specific tests.
    /// </summary>
    public partial class DependencyCollectorDiagnosticListenerTests
    {
        [TestMethod]
        public void NetCore30_OnActivityStartInjectsHeaders()
        {
            var activity = new Activity("System.Net.Http.HttpRequestOut");
            activity.AddBaggage("k", "v");
            activity.TraceStateString = "trace=state";
            activity.Start();

            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = this.CreateHttpListener(HttpInstrumentationVersion.V3))
            {
                listener.OnActivityStart(requestMsg);

                // Tracesparent, tracestate and Correlation-Context are injected by HttpClient when W3C is on
                var requestIds = requestMsg.Headers.GetValues(RequestResponseHeaders.RequestIdHeader).ToArray();
                Assert.AreEqual(1, requestIds.Length);
                Assert.AreEqual($"|{activity.TraceId.ToHexString()}.{activity.SpanId.ToHexString()}.", requestIds[0]);

                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceParentHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceStateHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.CorrelationContextHeader));

                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.StandardRootIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.StandardParentIdHeader));
                Assert.AreEqual(this.testApplicationId1,
                    GetRequestContextKeyValue(requestMsg, RequestResponseHeaders.RequestContextCorrelationSourceKey));
            }
        }

        [TestMethod]
        public void NetCore30_OnActivityStartInjectsHeadersRequestIdOff()
        {
            using (var listenerWithoutRequestId = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: false,
                HttpInstrumentationVersion.V3))
            {
                var activity = new Activity("System.Net.Http.HttpRequestOut");
                activity.AddBaggage("k", "v");
                activity.TraceStateString = "trace=state";
                activity.Start();

                HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);
                listenerWithoutRequestId.OnActivityStart(requestMsg);

                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.RequestIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceParentHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceStateHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.CorrelationContextHeader));
            }
        }

        [TestMethod]
        public void NetCore30_OnActivityStartInjectsLegacyHeaders()
        {
            var listenerWithLegacyHeaders = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new[] { "excluded.host.com" },
                injectLegacyHeaders: true,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V3);

            using (listenerWithLegacyHeaders)
            {
                var activity = new Activity("System.Net.Http.HttpRequestOut");
                activity.AddBaggage("k", "v");
                activity.TraceStateString = "trace=state";
                activity.Start();

                HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);
                listenerWithLegacyHeaders.OnActivityStart(requestMsg);

                // Traceparent and tracestate and Correlation-Context are injected by HttpClient
                // check only legacy headers here
                Assert.AreEqual(activity.RootId,
                    requestMsg.Headers.GetValues(RequestResponseHeaders.StandardRootIdHeader).Single());
                Assert.AreEqual(activity.SpanId.ToHexString(), requestMsg.Headers.GetValues(RequestResponseHeaders.StandardParentIdHeader).Single());
                Assert.AreEqual(this.testApplicationId1, GetRequestContextKeyValue(requestMsg, RequestResponseHeaders.RequestContextCorrelationSourceKey));
            }
        }

        [TestMethod]
        public void NetCore30_OnActivityStartInjectsW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;

            using (var listenerWithoutW3CHeaders = this.CreateHttpListener(HttpInstrumentationVersion.V3))
            {
                var activity = new Activity("System.Net.Http.HttpRequestOut");
                activity.AddBaggage("k", "v");
                activity.TraceStateString = "trace=state";
                activity.Start();

                HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);
                listenerWithoutW3CHeaders.OnActivityStart(requestMsg);

                // Request-Id and Correlation-Context are injected by HttpClient
                // check only W3C headers here
                Assert.AreEqual(this.testApplicationId1, GetRequestContextKeyValue(requestMsg, RequestResponseHeaders.RequestContextCorrelationSourceKey));
                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceParentHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceParentHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.RequestIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.CorrelationContextHeader));
            }
        }

        [TestMethod]
        public void NetCore30_OnStartActivityWithUriInExcludedDomainList()
        {
            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, "http://excluded.host.com/path/to/file.html");
            using (var listener = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V3))
            {
                listener.OnActivityStart(requestMsg);
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.RequestContextHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.RequestIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.StandardParentIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.StandardRootIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceParentHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceStateHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.CorrelationContextHeader));
            }
        }

        [TestMethod]
        public void NetCore30_OnStartActivityWithUriInExcludedDomainListW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;

            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, "http://excluded.host.com/path/to/file.html");
            using (var listener = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V2))
            {
                listener.OnActivityStart(requestMsg);
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.RequestContextHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.RequestIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.StandardParentIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.StandardRootIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceParentHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(W3C.W3CConstants.TraceStateHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.CorrelationContextHeader));
            }
        }
    }
}