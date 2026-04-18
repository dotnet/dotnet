namespace Microsoft.ApplicationInsights.Tests
{
#if NET452
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// DependencyTrackingTelemetryModule .Net 4.6 specific tests. 
    /// </summary>
    [TestClass]
    public class DependencyTrackingTelemetryModuleHttpTest
    {
        private const string IKey = "F8474271-D231-45B6-8DD4-D344C309AE69";
        private const string FakeProfileApiEndpoint = "https://dc.services.visualstudio.com/v2/track";
        private const string LocalhostUrlDiagSource = "http://localhost:8088/";
        private const string LocalhostUrlEventSource = "http://localhost:8090/";
        private const string expectedAppId = "someAppId";

        private readonly OperationDetailsInitializer operationDetailsInitializer = new OperationDetailsInitializer();
        private readonly DictionaryApplicationIdProvider appIdProvider = new DictionaryApplicationIdProvider();
        private StubTelemetryChannel channel;
        private TelemetryConfiguration config;
        private List<ITelemetry> sentTelemetry;

        [TestInitialize]
        public void Initialize()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;

            ServicePointManager.DefaultConnectionLimit = 1000;
            this.sentTelemetry = new List<ITelemetry>();
            this.channel = new StubTelemetryChannel
            {
                OnSend = telemetry => this.sentTelemetry.Add(telemetry),
                EndpointAddress = FakeProfileApiEndpoint
            };

            this.appIdProvider.Defined = new Dictionary<string, string>
            {
                [IKey] = expectedAppId
            };

            this.config = new TelemetryConfiguration
            {
                InstrumentationKey = IKey,
                TelemetryChannel = this.channel,
                ApplicationIdProvider = this.appIdProvider
            };

            this.config.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            this.config.TelemetryInitializers.Add(this.operationDetailsInitializer);

            DependencyTableStore.IsDesktopHttpDiagnosticSourceActivated = false;
        }

        [TestCleanup]
        public void Cleanup()
        {
            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }

            DependencyTableStore.IsDesktopHttpDiagnosticSourceActivated = false;
            GC.Collect();
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestBasicDependencyCollectionDiagnosticSource()
        {
            this.TestCollectionSuccessfulResponse(
                enableDiagnosticSource: true,
                url: LocalhostUrlDiagSource,
                statusCode: 200,
                enableW3C: true,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: false);
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestDependencyCollectionDiagnosticSourceBacksOffWhenTraceParentPresent()
        {
            using (this.CreateDependencyTrackingModule(true, true, true, false))
            {
                HttpWebRequest request = WebRequest.CreateHttp(LocalhostUrlDiagSource);
                request.Headers.Add("traceparent", "00-0123456789abcdef0123456789abcdef-0123456789abcdef-00");

                using (new LocalServer(
                    new Uri(LocalhostUrlDiagSource).GetLeftPart(UriPartial.Authority) + "/",
                    ctx =>
                    {
                        ctx.Response.StatusCode = 200;
                    }))
                {
                    try
                    {
                        using (request.GetResponse())
                        {
                        }
                    }
                    catch (WebException)
                    {
                        // ignore and let ValidateTelemetry method check status code
                    }
                }

                Assert.IsFalse(this.sentTelemetry.Any());
            }
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestDependencyCollectionDiagnosticSourceTraceParentPresentW3COff()
        {
            using (this.CreateDependencyTrackingModule(true, false, true, false))
            {
                HttpWebRequest request = WebRequest.CreateHttp(LocalhostUrlDiagSource);
                request.Headers.Add("traceparent", "00-0123456789abcdef0123456789abcdef-0123456789abcdef-00");

                using (new LocalServer(
                    new Uri(LocalhostUrlDiagSource).GetLeftPart(UriPartial.Authority) + "/",
                    ctx =>
                    {
                        ctx.Response.StatusCode = 200;
                    }))
                {
                    try
                    {
                        using (request.GetResponse())
                        {
                        }
                    }
                    catch (WebException)
                    {
                        // ignore and let ValidateTelemetry method check status code
                    }
                }

                Assert.AreEqual(1, this.sentTelemetry.Count());
            }
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestBasicDependencyCollectionDiagnosticSourceLegacyHeaders()
        {
            this.TestCollectionSuccessfulResponse(
                enableDiagnosticSource: true,
                url: LocalhostUrlDiagSource,
                statusCode: 200,
                enableW3C: true,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: true);
        }

        [TestMethod]
        [Ignore]
        // Active bug in .NET Fx diagnostics hook: https://github.com/dotnet/corefx/pull/40777
        // Application Insights has to inject Request-Id to work it around
        [Timeout(5000)]
        public void TestBasicDependencyCollectionW3COnRequestIdOffDiagnosticSource()
        {
            this.TestCollectionSuccessfulResponse(
                enableDiagnosticSource: true,
                url: LocalhostUrlDiagSource,
                statusCode: 200,
                enableW3C: true,
                enableRequestIdInW3CMode: false,
                injectLegacyHeaders: true);
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestBasicDependencyCollectionW3COffLegacyOnDiagnosticSource()
        {
            this.TestCollectionSuccessfulResponse(
                enableDiagnosticSource: true,
                url: LocalhostUrlDiagSource,
                statusCode: 200,
                enableW3C: false,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: true);
        }

        [TestMethod]
        [Timeout(5000)]
        public async Task TestZeroContentResponseDiagnosticSource()
        {
            await this.TestCollectionHttpClientSuccessfulResponse(
                url: LocalhostUrlDiagSource, 
                statusCode: 200, 
                contentLength: 0,
                expectResponse: false,
                expectHeadersDetail: true);
        }

        [TestMethod]
        [Timeout(5000)]
        public async Task TestZeroAndNonZeroContentResponseDiagnosticSource()
        {
            await this.TestZeroContentResponseAfterNonZeroResponse(LocalhostUrlDiagSource, 200);
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestDependencyCollectionDiagnosticSourceWithParentActivityAndTracestateAndCc()
        {
            var parent = new Activity("parent").AddBaggage("k", "v").Start();
            parent.TraceStateString = "state=some";

            this.TestCollectionSuccessfulResponse(
                enableDiagnosticSource: true,
                url: LocalhostUrlDiagSource,
                statusCode: 200,
                enableW3C: true,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: false);
            parent.Stop();
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestDependencyCollectionW3COffDiagnosticSourceWithParentActivityAndTracestateAndCc()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;

            var parent = new Activity("parent").AddBaggage("k", "v").Start();
            parent.TraceStateString = "state=some";
            this.TestCollectionSuccessfulResponse(
                enableDiagnosticSource: true,
                url: LocalhostUrlDiagSource,
                statusCode: 200,
                enableW3C: false,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: false);

            parent.Stop();
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestBasicDependencyCollectionEventSource()
        {
            this.TestCollectionSuccessfulResponse(false, LocalhostUrlEventSource, 200);
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestBasicDependencyCollectionW3COffEventSource()
        {
            this.TestCollectionSuccessfulResponse(
                enableDiagnosticSource: false, 
                url: LocalhostUrlEventSource,
                statusCode: 200,
                enableW3C: false);
        }

        [TestMethod]
        [Timeout(500000)]
        public void TestBasicDependencyCollectionEventSourceWithParentActivityTracestateAndCc()
        {
            var parent = new Activity("parent").AddBaggage("k", "v").Start();
            parent.TraceStateString = "state=some";

            this.TestCollectionSuccessfulResponse(
                enableDiagnosticSource: true,
                url: LocalhostUrlDiagSource,
                statusCode: 200,
                enableW3C: true,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: false);

            parent.Stop();
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestDependencyCollectionEventSourceNonSuccessStatusCode()
        {
            this.TestCollectionSuccessfulResponse(false, LocalhostUrlEventSource, 404);
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestDependencyCollectionDiagnosticSourceNonSuccessStatusCode()
        {
            this.TestCollectionSuccessfulResponse(true, LocalhostUrlDiagSource, 404);
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestNoDependencyCollectionDiagnosticSourceNoResponseClose()
        {
            using (this.CreateDependencyTrackingModule(
                enableDiagnosticSource: true,
                enableW3C: true,
                enableRequestIdInW3CMode: true, 
                injectLegacyHeaders: false))
            {
                HttpWebRequest request = WebRequest.CreateHttp(LocalhostUrlDiagSource);

                using (new LocalServer(LocalhostUrlDiagSource))
                {
                    request.GetResponse();
                }

                // HttpDesktopDiagnosticListener cannot collect dependencies if HttpWebResponse was not closed/disposed
                Assert.IsFalse(this.sentTelemetry.Any());
                var requestId = request.Headers[RequestResponseHeaders.RequestIdHeader];
                Assert.IsNotNull(requestId);
                Assert.IsNull(request.Headers[RequestResponseHeaders.StandardRootIdHeader]);
                Assert.IsNull(request.Headers[RequestResponseHeaders.StandardParentIdHeader]);
            }
        }

        // https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/769
        [TestMethod]
        [Timeout(5000)]
        public void TestNoDependencyCollectionDiagnosticSourceInitializedAfterEndpointCached()
        {
            // first time call endpoint before Dependency Collector initialization
            // Use event source Url to make sure endpoint was not hooked by prev tests
            HttpWebRequest request1 = WebRequest.CreateHttp(LocalhostUrlEventSource);
            using (new LocalServer(LocalhostUrlEventSource))
            {
                request1.GetResponse().Dispose();
            }

            // initialize dependency collector
            using (this.CreateDependencyTrackingModule(
                enableDiagnosticSource: true,
                enableW3C: true,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: false))
            {
                HttpWebRequest request2 = WebRequest.CreateHttp(LocalhostUrlEventSource);

                using (new LocalServer(LocalhostUrlEventSource))
                {
                    request2.GetResponse().Dispose();
                }

                // HttpDesktopDiagnosticListener may not collect dependencies if endpoint was recently called before 
                // dependency collection was initialized
                Assert.Inconclusive("At this point dependency may or may not be collected");
            }
        }

        [TestMethod]
        [Timeout(10000)]
        public async Task TestDependencyCollectionDnsIssueRequestDiagnosticSource()
        {
            await this.TestCollectionDnsIssue(true);
        }

        [TestMethod]
        [Timeout(10000)]
        public async Task TestDependencyCollectionDnsIssueRequestEventSource()
        {
            await this.TestCollectionDnsIssue(false);
        }

        [TestMethod]
        [Timeout(5000)]
        public async Task TestDependencyCollectionCanceledRequestDiagnosticSource()
        {
            await this.TestCollectionCanceledRequest(true, LocalhostUrlDiagSource);
        }

        [TestMethod]
        [Timeout(5000)]
        public async Task TestDependencyCollectionCanceledRequestEventSource()
        {
            await this.TestCollectionCanceledRequest(false, LocalhostUrlEventSource);
        }

        [TestMethod]
        [Timeout(5000)]
        public void OnBeginOnEndAreNotCalledForAppInsightsUrl()
        {
            using (var module = new DependencyTrackingTelemetryModule())
            {
                module.Initialize(this.config);

                using (var listener = new TestEventListener())
                {
                    listener.EnableEvents(DependencyCollectorEventSource.Log, EventLevel.Verbose, DependencyCollectorEventSource.Keywords.RddEventKeywords);

                    new HttpClient().GetAsync(FakeProfileApiEndpoint).ContinueWith(t => { }).Wait();

                    foreach (var message in listener.Messages)
                    {
                        Assert.IsFalse(message.EventId == 27 || message.EventId == 28);
                        Assert.IsFalse(message.Message.Contains("HttpDesktopDiagnosticSourceListener: Begin callback called for id"));
                        Assert.IsFalse(message.Message.Contains("HttpDesktopDiagnosticSourceListener: End callback called for id"));
                    }
                }
            }
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestDependencyCollectorPostRequestsAreCollectedDiagnosticSource()
        {
            this.TestCollectionPostRequests(true, LocalhostUrlDiagSource);
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestDependencyCollectorPostRequestsAreCollectedEventSource()
        {
            this.TestCollectionPostRequests(false, LocalhostUrlEventSource);
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestHttpRequestsWithQueryStringAreCollectedDiagnosticSource()
        {
            this.TestCollectionSuccessfulResponse(true, LocalhostUrlDiagSource + "123?q=123", 200);
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestHttpRequestsWithQueryStringAreCollectedEventSource()
        {
            this.TestCollectionSuccessfulResponse(false, LocalhostUrlEventSource + "123?q=123", 200);
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestDependencyCollectionDiagnosticSourceRedirect()
        {
            this.TestCollectionResponseWithRedirects(true, LocalhostUrlDiagSource);
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestDependencyCollectionEventSourceRedirect()
        {
            this.TestCollectionResponseWithRedirects(false, LocalhostUrlEventSource);
        }

        private void TestCollectionPostRequests(bool enableDiagnosticSource, string url)
        {
            using (this.CreateDependencyTrackingModule(
                enableDiagnosticSource: enableDiagnosticSource,
                enableW3C: true,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: false))
            {
                HttpWebRequest request = WebRequest.CreateHttp(url);
                request.Method = "POST";
                request.ContentLength = 1;

                using (new LocalServer(url))
                {
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(new byte[1], 0, 1);
                        stream.Close();
                    }

                    using (request.GetResponse())
                    {
                    }
                }

                this.ValidateTelemetry(
                    diagnosticSource: enableDiagnosticSource,
                    item: (DependencyTelemetry)this.sentTelemetry.Single(),
                    url: new Uri(url), 
                    requestMsg: request, 
                    success: true,
                    resultCode: "200",
                    w3CHeadersExpected: true,
                    responseExpected: true,
                    headersDetailExpected: false,
                    legacyHeadersExpected: false,
                    requestIdHeaderExpected: true);
            }
        }

        private void TestCollectionResponseWithRedirects(bool enableDiagnosticSource, string url)
        {
            using (this.CreateDependencyTrackingModule(
                enableDiagnosticSource: enableDiagnosticSource,
                enableW3C: true,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: false))
            {
                HttpWebRequest request = WebRequest.CreateHttp(url);

                int count = 0;
                Action<HttpListenerContext> onRequest = (context) =>
                {
                    if (count == 0)
                    {
                        context.Response.StatusCode = 302;
                        context.Response.RedirectLocation = url;
                    }
                    else
                    {
                        context.Response.StatusCode = 200;
                    }

                    count++;
                };

                using (new LocalServer(url, onRequest))
                {
                    using (request.GetResponse())
                    {
                    }
                }

                this.ValidateTelemetry(
                    diagnosticSource: enableDiagnosticSource,
                    item: (DependencyTelemetry)this.sentTelemetry.Single(),
                    url: new Uri(url), 
                    requestMsg: request, 
                    success: true,
                    resultCode: "200",
                    w3CHeadersExpected: true,
                    responseExpected: true,
                    headersDetailExpected: false,
                    legacyHeadersExpected: false,
                    requestIdHeaderExpected: true);
            }
        }

        private void TestCollectionSuccessfulResponse(
            bool enableDiagnosticSource, 
            string url, 
            int statusCode, 
            bool enableW3C = true,
            bool enableRequestIdInW3CMode = true,
            bool injectLegacyHeaders = false)
        {
            using (this.CreateDependencyTrackingModule(enableDiagnosticSource, enableW3C, enableRequestIdInW3CMode, injectLegacyHeaders))
            {
                HttpWebRequest request = WebRequest.CreateHttp(url);

                using (new LocalServer(
                    new Uri(url).GetLeftPart(UriPartial.Authority) + "/",
                    ctx =>
                    {
                        if (!enableDiagnosticSource && statusCode != 200)
                        {
                            // https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/548
                            // for quick unsuccessful response OnEnd is fired too fast after begin (before it's completed)
                            // first begin may take a while because of lazy initializations and jit compiling
                            // let's wait a bit here.
                            Thread.Sleep(20);
                        }

                        ctx.Response.StatusCode = statusCode;
                    }))
                {
                    try
                    {
                        using (request.GetResponse())
                        {
                        }
                    }
                    catch (WebException)
                    {
                        // ignore and let ValidateTelemetry method check status code
                    }
                }

                this.ValidateTelemetry(
                    diagnosticSource: enableDiagnosticSource,
                    item: (DependencyTelemetry)this.sentTelemetry.Single(),
                    url: new Uri(url),
                    requestMsg: request,
                    success: statusCode >= 200 && statusCode < 300,
                    resultCode: statusCode.ToString(CultureInfo.InvariantCulture),
                    w3CHeadersExpected: enableW3C,
                    responseExpected: true,
                    headersDetailExpected: false,
                    legacyHeadersExpected: injectLegacyHeaders,
                    requestIdHeaderExpected: enableRequestIdInW3CMode);
            }
        }

        private async Task TestCollectionHttpClientSuccessfulResponse(string url, 
            int statusCode,
            int contentLength,
            bool injectLegacyHeaders = false,
            bool expectResponse = true,
            bool expectHeadersDetail = false)
        {
            using (this.CreateDependencyTrackingModule(
                enableDiagnosticSource: true,
                enableW3C: true,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: false))
            {
                using (HttpClient client = new HttpClient())
                using (new LocalServer(
                    url, 
                    context =>
                    {
                        context.Response.ContentLength64 = contentLength;
                        context.Response.StatusCode = statusCode;
                    }))
                {
                    try
                    {
                        using (HttpResponseMessage response = await client.GetAsync(url))
                        {
                            Assert.AreEqual(0, response.Content.Headers.ContentLength);
                        }
                    }
                    catch (WebException)
                    {
                        // ignore and let ValidateTelemetry method check status code
                    }
                }

                this.ValidateTelemetry(
                    diagnosticSource: true,
                    item: (DependencyTelemetry)this.sentTelemetry.Single(),
                    url: new Uri(url),
                    requestMsg: null,
                    success: statusCode >= 200 && statusCode < 300,
                    resultCode: statusCode.ToString(CultureInfo.InvariantCulture),
                    w3CHeadersExpected: true,
                    responseExpected: expectResponse,
                    headersDetailExpected: expectHeadersDetail,
                    legacyHeadersExpected: injectLegacyHeaders,
                    requestIdHeaderExpected: true);
            }
        }

        private async Task TestZeroContentResponseAfterNonZeroResponse(string url, int statusCode)
        {
            using (this.CreateDependencyTrackingModule(
                enableDiagnosticSource: true,
                enableW3C: true,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: false))
            {
                using (HttpClient client = new HttpClient())
                {
                    using (new LocalServer(
                        url,
                        context =>
                        {
                            context.Response.ContentLength64 = 1;
                            context.Response.StatusCode = statusCode;
                            context.Response.OutputStream.WriteByte(0x1);
                            context.Response.OutputStream.Close();
                        }))
                    {
                        try
                        {
                            using (HttpResponseMessage response = await client.GetAsync(url))
                            {
                                Assert.AreEqual(1, response.Content.Headers.ContentLength);
                            }
                        }
                        catch (WebException)
                        {
                            // ignore and let ValidateTelemetry method check status code
                        }
                    }

                    using (new LocalServer(
                        url,
                        context =>
                        {
                            context.Response.ContentLength64 = 0;
                            context.Response.StatusCode = statusCode;
                        }))
                    {
                        try
                        {
                            using (HttpResponseMessage response = await client.GetAsync(url))
                            {
                                Assert.AreEqual(0, response.Content.Headers.ContentLength);
                            }
                        }
                        catch (WebException)
                        {
                            // ignore and let ValidateTelemetry method check status code
                        }
                    }
                }

                Assert.AreEqual(2, this.sentTelemetry.Count);

                this.ValidateTelemetry(
                    diagnosticSource: true,
                    item: (DependencyTelemetry)this.sentTelemetry.Last(),
                    url: new Uri(url),
                    requestMsg: null,
                    success: statusCode >= 200 && statusCode < 300,
                    resultCode: statusCode.ToString(CultureInfo.InvariantCulture),
                    w3CHeadersExpected: true,
                    responseExpected: false,
                    headersDetailExpected: true,
                    legacyHeadersExpected: false,
                    requestIdHeaderExpected: true);
            }
        }

        private async Task TestCollectionCanceledRequest(bool enableDiagnosticSource, string url)
        {
            using (this.CreateDependencyTrackingModule(
                enableDiagnosticSource: enableDiagnosticSource,
                enableW3C: true,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: false))
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                HttpClient httpClient = new HttpClient();

                using (new LocalServer(
                    url,
                    ctx =>
                    {
                        if (!enableDiagnosticSource)
                        {
                            // https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/548
                            // for quick unsuccesfull response OnEnd is fired too fast after begin (before it's completed)
                            // first begin may take a while because of lazy initializations and jit compiling
                            // let's wait a bit here.
                            Thread.Sleep(20);
                        }

                        cts.Cancel();
                    }))
                {
                    await httpClient.GetAsync(url, cts.Token).ContinueWith(t => { });
                }

                this.ValidateTelemetry(
                    diagnosticSource: enableDiagnosticSource,
                    item: (DependencyTelemetry)this.sentTelemetry.Single(),
                    url: new Uri(url),
                    requestMsg: null,
                    success: false,
                    resultCode: string.Empty,
                    w3CHeadersExpected: true,
                    responseExpected: false,
                    headersDetailExpected: false,
                    legacyHeadersExpected: false,
                    requestIdHeaderExpected: true);
            }
        }

        private async Task TestCollectionDnsIssue(bool enableDiagnosticSource)
        {
            using (this.CreateDependencyTrackingModule(
                enableDiagnosticSource: enableDiagnosticSource,
                enableW3C: true,
                enableRequestIdInW3CMode: true,
                injectLegacyHeaders: false))
            {
                var url = new Uri($"http://{Guid.NewGuid()}/");
                HttpClient client = new HttpClient();
                await client.GetAsync(url).ContinueWith(t => { });

                if (enableDiagnosticSource)
                {
                    // here the start of dependency is tracked with HttpDesktopDiagnosticSourceListener, 
                    // so the expected SDK version should have DiagnosticSource 'rdddsd' prefix. 
                    // however the end is tracked by FrameworkHttpEventListener
                    this.ValidateTelemetry(
                        diagnosticSource: true,
                        item: (DependencyTelemetry)this.sentTelemetry.Single(),
                        url: url, 
                        requestMsg: null, 
                        success: false,
                        resultCode: string.Empty,
                        w3CHeadersExpected: true,
                        responseExpected: false,
                        headersDetailExpected: false,
                        legacyHeadersExpected: false,
                        requestIdHeaderExpected: true);
                }
                else
                {
                    // https://github.com/Microsoft/ApplicationInsights-dotnet-server/issues/548
                    // End is fired before Begin, so EventSource doesn't track telemetry
                    Assert.IsFalse(this.sentTelemetry.Any());
                }
            }
        }

        private void ValidateTelemetry(
            bool diagnosticSource, 
            DependencyTelemetry item, 
            Uri url, 
            WebRequest requestMsg, 
            bool success, 
            string resultCode, 
            bool w3CHeadersExpected,
            bool responseExpected = true, 
            bool headersDetailExpected = false, 
            bool legacyHeadersExpected = false,
            bool requestIdHeaderExpected = true)
        {
            Assert.AreEqual(url, item.Data);

            if (url.Port == 80 || url.Port == 443)
            {
                Assert.AreEqual($"{url.Host}", item.Target);
            }
            else
            {
                Assert.AreEqual($"{url.Host}:{url.Port}", item.Target);
            }

            Assert.IsTrue(item.Duration > TimeSpan.FromMilliseconds(0), "Duration has to be positive");
            Assert.AreEqual(RemoteDependencyConstants.HTTP, item.Type, "HttpAny has to be dependency kind as it includes http and azure calls");
            Assert.IsTrue(
                DateTime.UtcNow.Subtract(item.Timestamp.UtcDateTime).TotalMilliseconds <
                TimeSpan.FromMinutes(1).TotalMilliseconds,
                "timestamp < now");
            Assert.IsTrue(
                item.Timestamp.Subtract(DateTime.UtcNow).TotalMilliseconds >
                -TimeSpan.FromMinutes(1).TotalMilliseconds,
                "now - 1 min < timestamp");
            Assert.AreEqual(resultCode, item.ResultCode);
            Assert.AreEqual(success, item.Success);

            var parentActivity = Activity.Current;
            if (parentActivity != null)
            {
                if (parentActivity.IdFormat == ActivityIdFormat.W3C)
                {
                    Assert.AreEqual(parentActivity.TraceId.ToHexString(), item.Context.Operation.Id);
                    Assert.AreEqual(parentActivity.SpanId.ToHexString(), item.Context.Operation.ParentId);

                    if (parentActivity.TraceStateString != null)
                    {
                        Assert.IsTrue(item.Properties.ContainsKey("tracestate"));
                        Assert.AreEqual(parentActivity.TraceStateString, item.Properties["tracestate"]);
                    }
                    else
                    {
                        Assert.IsFalse(item.Properties.ContainsKey("tracestate"));
                    }
                }
                else
                {
                    Assert.AreEqual(parentActivity.RootId, item.Context.Operation.Id);
                    Assert.AreEqual(parentActivity.Id, item.Context.Operation.ParentId);
                    Assert.IsTrue(item.Id.StartsWith('|' + item.Context.Operation.Id + '.'));
                }
            }
            else
            {
                Assert.IsNotNull(item.Context.Operation.Id);
                Assert.IsNull(item.Context.Operation.ParentId);
            }

            if (diagnosticSource)
            {
                this.operationDetailsInitializer.ValidateOperationDetailsDesktop(item, responseExpected, headersDetailExpected);
                this.ValidateTelemetryForDiagnosticSource(item, url, requestMsg, legacyHeadersExpected, w3CHeadersExpected, requestIdHeaderExpected);
            }
            else
            {
                this.ValidateTelemetryForEventSource(item, url, requestMsg);
            }
        }

        private void ValidateTelemetryForDiagnosticSource(
            DependencyTelemetry item, 
            Uri url, 
            WebRequest requestMsg, 
            bool expectLegacyHeaders, 
            bool expectW3CHeaders,
            bool expectRequestId)
        {
            var expectedMethod = requestMsg != null ? requestMsg.Method : "GET";
            Assert.AreEqual(expectedMethod + " " + url.AbsolutePath, item.Name);

            Assert.AreEqual(
                SdkVersionHelper.GetExpectedSdkVersion(typeof(DependencyTrackingTelemetryModule), "rdddsd:"),
                item.Context.GetInternalContext().SdkVersion);

            if (requestMsg != null)
            {
                var requestIdHeader = requestMsg.Headers[RequestResponseHeaders.RequestIdHeader];
                string expectedRequestId;

                if (expectW3CHeaders)
                {
                    var traceId = item.Context.Operation.Id;
                    var spanId = item.Id;
                    var expectedTraceparent = $"00-{traceId}-{spanId}-00";
                    expectedRequestId = $"|{traceId}.{spanId}.";

                    Assert.AreEqual(expectedTraceparent, requestMsg.Headers[W3C.W3CConstants.TraceParentHeader]);
                    Assert.AreEqual(Activity.Current?.TraceStateString, requestMsg.Headers[W3C.W3CConstants.TraceStateHeader]);
                }
                else
                {
                    expectedRequestId = item.Id;
                    Assert.IsNull(requestMsg.Headers[W3C.W3CConstants.TraceParentHeader]);
                    Assert.IsNull(requestMsg.Headers[W3C.W3CConstants.TraceStateHeader]);
                }

                if (expectRequestId)
                {
                    Assert.AreEqual(expectedRequestId, requestMsg.Headers[RequestResponseHeaders.RequestIdHeader]);
                }
                else
                {
                    Assert.IsNull(requestMsg.Headers[RequestResponseHeaders.RequestIdHeader]);
                }

                if (expectLegacyHeaders)
                {
                    Assert.AreEqual(item.Id, requestMsg.Headers[RequestResponseHeaders.StandardParentIdHeader]);
                    Assert.AreEqual(item.Context.Operation.Id, requestMsg.Headers[RequestResponseHeaders.StandardRootIdHeader]);
                }
                else
                {
                    Assert.IsNull(requestMsg.Headers[RequestResponseHeaders.StandardParentIdHeader]);
                    Assert.IsNull(requestMsg.Headers[RequestResponseHeaders.StandardRootIdHeader]);
                }

                if (Activity.Current != null)
                {
                    var correlationContextHeader = requestMsg.Headers[RequestResponseHeaders.CorrelationContextHeader]
                        .Split(',');

                    var baggage = Activity.Current.Baggage.Select(kvp => $"{kvp.Key}={kvp.Value}").ToArray();
                    Assert.AreEqual(baggage.Length, correlationContextHeader.Length);

                    foreach (var baggageItem in baggage)
                    {
                        Assert.IsTrue(correlationContextHeader.Contains(baggageItem));
                    }
                }
                else
                {
                    Assert.IsNull(requestMsg.Headers[RequestResponseHeaders.CorrelationContextHeader]);
                }
            }
        }

        private void ValidateTelemetryForEventSource(DependencyTelemetry item, Uri url, WebRequest requestMsg)
        {
            Assert.AreEqual(url.AbsolutePath, item.Name);

            Assert.AreEqual(
                SdkVersionHelper.GetExpectedSdkVersion(typeof(DependencyTrackingTelemetryModule), "rddf:"),
                item.Context.GetInternalContext().SdkVersion);

            if (requestMsg != null)
            {
                Assert.IsNull(requestMsg.Headers[RequestResponseHeaders.RequestIdHeader]);
                Assert.IsNull(requestMsg.Headers[RequestResponseHeaders.StandardParentIdHeader]);
                Assert.IsNull(requestMsg.Headers[RequestResponseHeaders.StandardRootIdHeader]);
                Assert.IsNull(requestMsg.Headers[RequestResponseHeaders.CorrelationContextHeader]);
            }
        }

        private DependencyTrackingTelemetryModule CreateDependencyTrackingModule(
            bool enableDiagnosticSource,
            bool enableW3C,
            bool enableRequestIdInW3CMode,
            bool injectLegacyHeaders)
        {
            Activity.DefaultIdFormat = enableW3C ? ActivityIdFormat.W3C : ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;

            var module = new DependencyTrackingTelemetryModule();

            if (!enableDiagnosticSource)
            {
                module.DisableDiagnosticSourceInstrumentation = true;
            }

            module.EnableLegacyCorrelationHeadersInjection = injectLegacyHeaders;
            module.EnableRequestIdHeaderInjectionInW3CMode = enableRequestIdInW3CMode;

            module.Initialize(this.config);
            Assert.AreEqual(enableDiagnosticSource, DependencyTableStore.IsDesktopHttpDiagnosticSourceActivated);

            return module;
        }

        private class LocalServer : IDisposable
        {
            private readonly HttpListener listener;
            private readonly CancellationTokenSource cts;

            public LocalServer(string url, Action<HttpListenerContext> onRequest = null)
            {
                this.listener = new HttpListener();
                this.listener.Prefixes.Add(url);
                this.listener.Start();
                this.cts = new CancellationTokenSource();

                Task.Run(
                    () =>
                    {
                        while (!this.cts.IsCancellationRequested)
                        {
                            HttpListenerContext context = this.listener.GetContext();
                            if (onRequest != null)
                            {
                                onRequest(context);
                            }
                            else
                            {
                                context.Response.StatusCode = 200;
                            }

                            context.Response.OutputStream.Close();
                            context.Response.Close();
                        }
                    },
                    this.cts.Token);
            }

            public void Dispose()
            {
                this.cts.Cancel(false);
                this.listener.Abort();
                ((IDisposable)this.listener).Dispose();
                this.cts.Dispose();
            }
        }
    }
#endif
}