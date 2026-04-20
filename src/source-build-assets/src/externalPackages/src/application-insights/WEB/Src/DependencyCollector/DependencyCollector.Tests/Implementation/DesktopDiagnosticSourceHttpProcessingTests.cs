#if NET452
namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DesktopDiagnosticSourceHttpProcessingTests
    {
        #region Fields
        private const int TimeAccuracyMilliseconds = 50;
        private const string TestInstrumentationKey = nameof(TestInstrumentationKey);
        private const string TestApplicationId = nameof(TestApplicationId);
        private readonly OperationDetailsInitializer operationDetailsInitializer = new OperationDetailsInitializer();
        private Uri testUrl = new Uri("http://www.microsoft.com/");
        private int sleepTimeMsecBetweenBeginAndEnd = 100;
        private TelemetryConfiguration configuration;
        private List<ITelemetry> sendItems;
        private DesktopDiagnosticSourceHttpProcessing httpDesktopProcessingFramework;
        #endregion //Fields

        #region TestInitialize

        [TestInitialize]
        public void TestInitialize()
        {
            this.sendItems = new List<ITelemetry>();

            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;

            this.configuration = new TelemetryConfiguration()
            {
                TelemetryChannel = new StubTelemetryChannel
                {
                    OnSend = telemetry => this.sendItems.Add(telemetry)
                },
                InstrumentationKey = TestInstrumentationKey,
                ApplicationIdProvider = new MockApplicationIdProvider(TestInstrumentationKey, TestApplicationId)
            };

            this.configuration.TelemetryInitializers.Add(this.operationDetailsInitializer);

            this.httpDesktopProcessingFramework = new DesktopDiagnosticSourceHttpProcessing(
                this.configuration, 
                new CacheBasedOperationHolder("testCache", 100 * 1000), 
                setCorrelationHeaders: true,
                correlationDomainExclusionList: new List<string>(),
                injectLegacyHeaders: false,
                injectRequestIdInW3cMode: true);
            DependencyTableStore.IsDesktopHttpDiagnosticSourceActivated = false;
        }

        [TestCleanup]
        public void Cleanup()
        {
            DependencyTableStore.IsDesktopHttpDiagnosticSourceActivated = false;
        }
        #endregion //TestInitiliaze

        /// <summary>
        /// Validates that OnRequestSend and OnResponseReceive sends valid telemetry.
        /// </summary>
        [TestMethod]
        public void RddTestHttpDesktopProcessingFrameworkUpdateTelemetryName()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.testUrl);

            var stopwatchMax = Stopwatch.StartNew();
            this.httpDesktopProcessingFramework.OnBegin(request);
            var stopwatchMin = Stopwatch.StartNew();
            Thread.Sleep(2 * this.sleepTimeMsecBetweenBeginAndEnd);
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
            var response = TestUtils.GenerateHttpWebResponse(HttpStatusCode.OK);
            stopwatchMin.Stop();
            this.httpDesktopProcessingFramework.OnEndResponse(request, response);
            stopwatchMax.Stop();

            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            this.ValidateTelemetryPacketForOnRequestSend(
                this.sendItems[0] as DependencyTelemetry,
                this.testUrl, 
                RemoteDependencyConstants.HTTP, 
                true,
                stopwatchMin.Elapsed.TotalMilliseconds,
                stopwatchMax.Elapsed.TotalMilliseconds, 
                "200");
        }

         /// <summary>
         /// Validates that OnBegin does not inject headers when called with injectCorrelationHeadersFlag = false.
         /// </summary>
         [TestMethod]
         public void RddTestHttpProcessingFrameworkDoNotInjectHeadersWhenFlagIsSet()
         {
             var activity = new Activity("parent").AddBaggage("k", "v").Start();
             HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.testUrl);
             this.httpDesktopProcessingFramework.OnBegin(request, false);
             Assert.IsNull(request.Headers[W3C.W3CConstants.TraceParentHeader]);
             Assert.IsNull(request.Headers[W3C.W3CConstants.TraceStateHeader]);
             Assert.IsNull(request.Headers[RequestResponseHeaders.CorrelationContextHeader]);
             Assert.IsNull(request.Headers[RequestResponseHeaders.StandardRootIdHeader]);
             Assert.IsNull(request.Headers[RequestResponseHeaders.StandardParentIdHeader]);

            // Active bug in .NET Fx diagnostics hook: https://github.com/dotnet/corefx/pull/40777
            // Application Insights has to inject Request-Id to work it around
            Assert.IsNotNull(request.Headers[RequestResponseHeaders.RequestIdHeader]);
             activity.Stop();
         }

        /// <summary>
        /// Validates that even if multiple events have fired, as long as there is only
        /// one HttpWebRequest, only one event should be written, during the first call
        /// to OnResponseReceive.
        /// </summary>
        [TestMethod]
        public void RddTestHttpDesktopProcessingFrameworkNoDuplication()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.testUrl);
            var redirectResponse = TestUtils.GenerateHttpWebResponse(HttpStatusCode.Redirect);
            var successResponse = TestUtils.GenerateHttpWebResponse(HttpStatusCode.OK);

            Stopwatch stopwatchMax = Stopwatch.StartNew();
            this.httpDesktopProcessingFramework.OnBegin(request);
            Stopwatch stopwatchMin = Stopwatch.StartNew();
            Thread.Sleep(2 * this.sleepTimeMsecBetweenBeginAndEnd);
            this.httpDesktopProcessingFramework.OnBegin(request);
            this.httpDesktopProcessingFramework.OnBegin(request);
            this.httpDesktopProcessingFramework.OnBegin(request);
            this.httpDesktopProcessingFramework.OnBegin(request);
            Thread.Sleep(this.sleepTimeMsecBetweenBeginAndEnd);
            Assert.AreEqual(0, this.sendItems.Count, "No telemetry item should be processed without calling End");
            stopwatchMin.Stop();
            this.httpDesktopProcessingFramework.OnEndResponse(request, redirectResponse);
            stopwatchMax.Stop();
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");

            this.httpDesktopProcessingFramework.OnEndResponse(request, redirectResponse);
            this.httpDesktopProcessingFramework.OnEndResponse(request, successResponse);
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            this.ValidateTelemetryPacketForOnRequestSend(this.sendItems[0] as DependencyTelemetry, 
                this.testUrl, 
                RemoteDependencyConstants.HTTP,
                true,
                stopwatchMin.Elapsed.TotalMilliseconds,
                stopwatchMax.Elapsed.TotalMilliseconds, 
                "302");
        }

        /// <summary>
        /// Validates if DependencyTelemetry sent contains the cross component correlation ID.
        /// </summary>
        [TestMethod]
        [Description("Validates if DependencyTelemetry sent contains the cross component correlation ID.")]
        public void RddTestHttpDesktopProcessingFrameworkOnEndAddsAppIdToTargetField()
        {
            // Here is a sample App ID, since the test initialize method adds a random ikey and our mock getAppId method pretends that the appId for a given ikey is the same as the ikey.
            // This will not match the current component's App ID. Hence represents an external component.
            string appId = "0935FC42-FE1A-4C67-975C-0C9D5CBDEE8E";

            var request = WebRequest.Create(this.testUrl);

            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { RequestResponseHeaders.RequestContextHeader, this.GetCorrelationIdHeaderValue(appId) }
            };

            var response = TestUtils.GenerateHttpWebResponse(HttpStatusCode.OK, headers);

            this.httpDesktopProcessingFramework.OnBegin(request);
            this.httpDesktopProcessingFramework.OnEndResponse(request, response);
            Assert.AreEqual(1, this.sendItems.Count, "Only one telemetry item should be sent");
            Assert.AreEqual(this.testUrl.Host + " | " + this.GetCorrelationIdValue(appId), ((DependencyTelemetry)this.sendItems[0]).Target);
        }

        /// <summary>
        /// Ensures that the source request header is added when request is sent.
        /// </summary>
        [TestMethod]
        [Description("Ensures that the source request header is added when request is sent.")]
        public void RddTestHttpDesktopProcessingFrameworkOnBeginAddsSourceHeader()
        {
            var request = WebRequest.Create(this.testUrl);

            Assert.IsNull(request.Headers[RequestResponseHeaders.RequestContextHeader]);

            this.httpDesktopProcessingFramework.OnBegin(request);
            Assert.IsNotNull(request.Headers.GetNameValueHeaderValue(
                RequestResponseHeaders.RequestContextHeader, 
                RequestResponseHeaders.RequestContextCorrelationSourceKey));
        }

#if !NET452
        /// <summary>
        /// Ensures that the legacy correlation headers are NOT added when request is sent if HttpProcessing is configured to.
        /// </summary>
        [TestMethod]
        public void RddTestHttpDesktopProcessingFrameworkOnBeginAddsLegacyHeaders()
        {
            var httpProcessingLegacyHeaders = new DesktopDiagnosticSourceHttpProcessing(
                this.configuration, 
                new CacheBasedOperationHolder("testCache", 100 * 1000),
                setCorrelationHeaders: true,
                correlationDomainExclusionList: new List<string>(),
                injectLegacyHeaders: true,
                injectRequestIdInW3cMode: true);
            var request = WebRequest.Create(this.testUrl);

            Assert.IsNull(request.Headers[RequestResponseHeaders.StandardParentIdHeader]);

            var client = new TelemetryClient(this.configuration);
            using (var op = client.StartOperation<RequestTelemetry>("request"))
            {
                httpProcessingLegacyHeaders.OnBegin(request);

                var actualRootIdHeader = request.Headers[RequestResponseHeaders.StandardRootIdHeader];
                var actualParentIdHeader = request.Headers[RequestResponseHeaders.StandardParentIdHeader];
                var actualRequestIdHeader = request.Headers[RequestResponseHeaders.RequestIdHeader];
                Assert.IsNotNull(actualRootIdHeader);
                Assert.IsNotNull(actualParentIdHeader);
                Assert.IsNotNull(actualRequestIdHeader);

                Assert.AreNotEqual(actualParentIdHeader, op.Telemetry.Context.Operation.Id);

                Assert.AreEqual(actualParentIdHeader, actualRequestIdHeader);
                Assert.AreEqual(Activity.Current.RootId, actualRootIdHeader);
            }
        }
#endif

        /// <summary>
        /// Ensures that the legacy correlation headers are added when request is sent if HttpProcessing is configured to.
        /// </summary>
        [TestMethod]
        public void RddTestHttpDesktopProcessingFrameworkOnBegin()
        {
            var request = WebRequest.Create(this.testUrl);

            Assert.IsNull(request.Headers[RequestResponseHeaders.StandardParentIdHeader]);

            var client = new TelemetryClient(this.configuration);
            using (var op = client.StartOperation<RequestTelemetry>("request"))
            {
                this.httpDesktopProcessingFramework.OnBegin(request);

                var actualRequestIdHeader = request.Headers[RequestResponseHeaders.RequestIdHeader];
                var actualTraceparentHeader = request.Headers[W3C.W3CConstants.TraceParentHeader];

                Assert.IsNull(request.Headers[RequestResponseHeaders.StandardParentIdHeader]);
                Assert.IsNull(request.Headers[RequestResponseHeaders.StandardRootIdHeader]);
                Assert.IsNull(request.Headers[W3C.W3CConstants.TraceStateHeader]);
                Assert.IsNull(request.Headers[RequestResponseHeaders.CorrelationContextHeader]);

                var parentActivity = Activity.Current;
                Assert.IsTrue(actualTraceparentHeader.StartsWith($"00-{parentActivity.TraceId.ToHexString()}-", StringComparison.Ordinal));
                var spanId = actualTraceparentHeader.Split('-')[2];
                Assert.AreEqual($"|{parentActivity.TraceId.ToHexString()}.{spanId}.", actualRequestIdHeader);

                Assert.AreNotEqual(parentActivity.Id, actualTraceparentHeader);
            }
        }

        [TestMethod]
        public void RddTestHttpDesktopProcessingFrameworkOnBeginW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;
            var request = WebRequest.Create(this.testUrl);

            var client = new TelemetryClient(this.configuration);
            using (var op = client.StartOperation<RequestTelemetry>("request"))
            {
                this.httpDesktopProcessingFramework.OnBegin(request);

                var actualRequestIdHeader = request.Headers[RequestResponseHeaders.RequestIdHeader];

                Assert.IsNull(request.Headers[RequestResponseHeaders.StandardParentIdHeader]);
                Assert.IsNull(request.Headers[RequestResponseHeaders.StandardRootIdHeader]);
                Assert.IsNull(request.Headers[W3C.W3CConstants.TraceParentHeader]);
                Assert.IsNull(request.Headers[W3C.W3CConstants.TraceStateHeader]);
                Assert.IsNull(request.Headers[RequestResponseHeaders.CorrelationContextHeader]);

                var parentActivity = Activity.Current;
                Assert.IsTrue(actualRequestIdHeader.StartsWith(parentActivity.Id, StringComparison.Ordinal));
                Assert.AreNotEqual(parentActivity.Id, actualRequestIdHeader);
            }
        }

        [TestMethod]
        public void RddTestHttpDesktopProcessingFrameworkOnBeginW3COnRequestIdOff()
        {
            var request = WebRequest.Create(this.testUrl);

            Assert.IsNull(request.Headers[RequestResponseHeaders.StandardParentIdHeader]);

            var client = new TelemetryClient(this.configuration);
            using (var op = client.StartOperation<RequestTelemetry>("request"))
            {
                var httpDesktopProcessingFrameworkRequestIdOff = new DesktopDiagnosticSourceHttpProcessing(
                    this.configuration,
                    new CacheBasedOperationHolder("testCache", 100 * 1000),
                    setCorrelationHeaders: true,
                    correlationDomainExclusionList: new List<string>(),
                    injectLegacyHeaders: false,
                    injectRequestIdInW3cMode: false);

                httpDesktopProcessingFrameworkRequestIdOff.OnBegin(request);

                var actualRequestIdHeader = request.Headers[RequestResponseHeaders.RequestIdHeader];
                var actualTraceparentHeader = request.Headers[W3C.W3CConstants.TraceParentHeader];

                Assert.IsNull(request.Headers[RequestResponseHeaders.StandardParentIdHeader]);
                Assert.IsNull(request.Headers[RequestResponseHeaders.StandardRootIdHeader]);
                Assert.IsNull(request.Headers[W3C.W3CConstants.TraceStateHeader]);
                Assert.IsNull(request.Headers[RequestResponseHeaders.CorrelationContextHeader]);

                // Active bug in .NET Fx diagnostics hook: https://github.com/dotnet/corefx/pull/40777
                // Application Insights has to inject Request-Id to work it around
                var parentActivity = Activity.Current;
                Assert.IsTrue(actualTraceparentHeader.StartsWith($"00-{parentActivity.TraceId.ToHexString()}-", StringComparison.Ordinal));
                var spanId = actualTraceparentHeader.Split('-')[2];
                Assert.AreEqual($"|{parentActivity.TraceId.ToHexString()}.{spanId}.", actualRequestIdHeader);

                Assert.AreNotEqual(parentActivity.Id, actualTraceparentHeader);
            }
        }

        /// <summary>
        /// Ensures that the source request header is not added, as per the config, when request is sent.
        /// </summary>
        [TestMethod]
        [Description("Ensures that the source request header is not added when the config commands as such")]
        public void RddTestHttpDesktopProcessingFrameworkOnBeginSkipsAddingSourceHeaderPerConfig()
        {
            string hostnamepart = "partofhostname";
            string url = string.Format(CultureInfo.InvariantCulture, "http://hostnamestart{0}hostnameend.com/path/to/something?param=1", hostnamepart);
            var request = WebRequest.Create(new Uri(url));

            Assert.IsNull(request.Headers[RequestResponseHeaders.RequestContextHeader]);
            Assert.AreEqual(0, request.Headers.Keys.Cast<string>().Where((x) => { return x.StartsWith("x-ms-", StringComparison.OrdinalIgnoreCase); }).Count());

            var localHttpProcessingFramework = new DesktopDiagnosticSourceHttpProcessing(
                this.configuration, 
                new CacheBasedOperationHolder("testCache", 100 * 1000),
                setCorrelationHeaders: false,
                correlationDomainExclusionList: new List<string>(),
                injectLegacyHeaders: false,
                injectRequestIdInW3cMode: true);

            localHttpProcessingFramework.OnBegin(request);
            Assert.IsNull(request.Headers[RequestResponseHeaders.RequestContextHeader]);
            Assert.AreEqual(0, request.Headers.Keys.Cast<string>().Count(x => x.StartsWith("x-ms-", StringComparison.OrdinalIgnoreCase)));

            ICollection<string> exclusionList = new SanitizedHostList() { "randomstringtoexclude", hostnamepart };
            localHttpProcessingFramework = new DesktopDiagnosticSourceHttpProcessing(
                this.configuration,
                new CacheBasedOperationHolder("testCache", 100 * 1000),
                setCorrelationHeaders: true,
                correlationDomainExclusionList: exclusionList,
                injectLegacyHeaders: false,
                injectRequestIdInW3cMode: true);

            localHttpProcessingFramework.OnBegin(request);
            Assert.IsNull(request.Headers[RequestResponseHeaders.RequestContextHeader]);
            Assert.AreEqual(0, request.Headers.Keys.Cast<string>().Count(x => x.StartsWith("x-ms-", StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Ensures that the source request header is not overwritten if already provided by the user.
        /// </summary>
        [TestMethod]
        [Description("Ensures that the source request header is not overwritten if already provided by the user.")]
        public void RddTestHttpDesktopProcessingFrameworkOnBeginDoesNotOverwriteExistingSource()
        {
            string sampleHeaderValueWithAppId = RequestResponseHeaders.RequestContextCorrelationSourceKey + "=HelloWorld";
            var request = WebRequest.Create(this.testUrl);

            request.Headers.Add(RequestResponseHeaders.RequestContextHeader, sampleHeaderValueWithAppId);

            this.httpDesktopProcessingFramework.OnBegin(request);
            var actualHeaderValue = request.Headers[RequestResponseHeaders.RequestContextHeader];

            Assert.IsNotNull(actualHeaderValue);
            Assert.AreEqual(sampleHeaderValueWithAppId, actualHeaderValue);

            string sampleHeaderValueWithoutAppId = "helloWorld";
            request = WebRequest.Create(this.testUrl);

            request.Headers.Add(RequestResponseHeaders.RequestContextHeader, sampleHeaderValueWithoutAppId);

            this.httpDesktopProcessingFramework.OnBegin(request);
            actualHeaderValue = request.Headers[RequestResponseHeaders.RequestContextHeader];

            Assert.IsNotNull(actualHeaderValue);
            Assert.AreNotEqual(sampleHeaderValueWithAppId, actualHeaderValue);
        }

        private void ValidateTelemetryPacketForOnRequestSend(
            DependencyTelemetry remoteDependencyTelemetryActual,
            Uri url,
            string kind, 
            bool? success,
            double minDependencyDurationMs,
            double maxDependencyDurationMs,
            string statusCode)
        {
            Assert.AreEqual("GET " + url.AbsolutePath, remoteDependencyTelemetryActual.Name, true, "Resource name in the sent telemetry is wrong");
            string expectedVersion =
                SdkVersionHelper.GetExpectedSdkVersion(typeof(DependencyTrackingTelemetryModule), prefix: "rdddsd:");
            this.ValidateTelemetryPacket(remoteDependencyTelemetryActual, 
                url, 
                kind, 
                success,
                minDependencyDurationMs,
                maxDependencyDurationMs,
                statusCode, 
                expectedVersion);
        }

        private void ValidateTelemetryPacket(
            DependencyTelemetry remoteDependencyTelemetryActual,
            Uri url,
            string kind,
            bool? success,
            double minDependencyDurationMs,
            double maxDependencyDurationMs,
            string statusCode, 
            string expectedVersion,
            bool responseExpected = true)
        {
            Assert.AreEqual(url.Host, remoteDependencyTelemetryActual.Target, true, "Resource target in the sent telemetry is wrong");
            Assert.AreEqual(url.OriginalString, remoteDependencyTelemetryActual.Data, true, "Resource data in the sent telemetry is wrong");
            Assert.AreEqual(kind.ToString(), remoteDependencyTelemetryActual.Type, "DependencyKind in the sent telemetry is wrong");
            Assert.AreEqual(success, remoteDependencyTelemetryActual.Success, "Success in the sent telemetry is wrong");
            Assert.AreEqual(statusCode, remoteDependencyTelemetryActual.ResultCode, "ResultCode in the sent telemetry is wrong");

            this.operationDetailsInitializer.ValidateOperationDetailsDesktop(remoteDependencyTelemetryActual, responseExpected, headersExpected: false);

            Assert.IsTrue(
                remoteDependencyTelemetryActual.Duration.TotalMilliseconds <= maxDependencyDurationMs,
                $"Dependency duration {remoteDependencyTelemetryActual.Duration.TotalMilliseconds} must be smaller than time between before-start and after-end: '{maxDependencyDurationMs}'");

            Assert.IsTrue(
                remoteDependencyTelemetryActual.Duration.TotalMilliseconds >= minDependencyDurationMs,
                $"Dependency duration {remoteDependencyTelemetryActual.Duration.TotalMilliseconds} must be bigger than time between after-start and before-end '{minDependencyDurationMs}'");

            Assert.AreEqual(expectedVersion, remoteDependencyTelemetryActual.Context.GetInternalContext().SdkVersion);
        }

        private string GetCorrelationIdValue(string appId)
        {
            return string.Format(CultureInfo.InvariantCulture, "cid-v1:{0}", appId);
        }

        private string GetCorrelationIdHeaderValue(string appId)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}=cid-v1:{1}", RequestResponseHeaders.RequestContextCorrelationTargetKey, appId);
        }
    }
}
#endif