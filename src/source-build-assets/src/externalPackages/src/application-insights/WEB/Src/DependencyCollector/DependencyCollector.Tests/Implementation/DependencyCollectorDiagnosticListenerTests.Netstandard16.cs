namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Http;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.ApplicationInsights.DependencyCollector.Implementation;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for DependencyCollectorDiagnosticListener.
    /// </summary>
    [TestClass]
    public partial class DependencyCollectorDiagnosticListenerTests
    {
        private const string RequestUrl = "www.example.com";
        private const string RequestUrlWithScheme = "https://" + RequestUrl;
        private const string HttpOkResultCode = "200";
        private const string NotFoundResultCode = "404";

        private readonly OperationDetailsInitializer operationDetailsInitializer = new OperationDetailsInitializer();
        private readonly List<ITelemetry> sentTelemetry = new List<ITelemetry>();

        private TelemetryConfiguration configuration;
        private string testInstrumentationKey1 = nameof(testInstrumentationKey1);
        private string testApplicationId1 = "cid-v1:" + nameof(testApplicationId1);
        private string testApplicationId2 = "cid-v1:" + nameof(testApplicationId2);
        private StubTelemetryChannel telemetryChannel;

        /// <summary>
        /// Initialize function that gets called once before any tests in this class are run.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;

            this.telemetryChannel = new StubTelemetryChannel
            {
                EndpointAddress = "https://endpointaddress",
                OnSend = telemetry => this.sentTelemetry.Add(telemetry)
            };

            this.testInstrumentationKey1 = Guid.NewGuid().ToString();

            this.configuration = new TelemetryConfiguration
            {
                TelemetryChannel = this.telemetryChannel,
                InstrumentationKey = this.testInstrumentationKey1,
                ApplicationIdProvider = new MockApplicationIdProvider(this.testInstrumentationKey1, this.testApplicationId1)
            };

            this.configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            this.configuration.TelemetryInitializers.Add(this.operationDetailsInitializer);
        }

        /// <summary>
        /// Cleans up.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            while (Activity.Current != null)
            {
                Activity.Current.Stop();
            }
        }

        /// <summary>
        /// Call OnRequest() with no uri in the HttpRequestMessage.
        /// </summary>
        [TestMethod]
        public void OnRequestWithRequestEventWithNoRequestUri()
        {
            var requestMsg = new HttpRequestMessage();

            using (var listener = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V1))
            {
                listener.OnRequest(requestMsg, Guid.NewGuid());

                Assert.IsFalse(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out _));
                Assert.AreEqual(0, this.sentTelemetry.Count);
            }
        }

        /// <summary>
        /// Very second request does not throw an exception.
        /// </summary>
        [TestMethod]
        public void VerifyOnRequestWithSameDoesNotThrowException()
        {
            var loggingRequestId = Guid.NewGuid();

            var requestMsg = new HttpRequestMessage(HttpMethod.Get, RequestUrlWithScheme);

            using (var listener = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V1))
            {
                listener.OnRequest(requestMsg, loggingRequestId);
                listener.OnRequest(requestMsg, loggingRequestId);
            }
        }

        /// <summary>
        /// Scenario: Assume a retry request after first request fails.
        /// Very second request does not throw an exception.
        /// Verify that telemetry is collected for both requests.
        /// </summary>
        /// <remarks>
        /// IGNORE
        /// THE FOLLOWING ASSERTS ARE EXPECTED TO PASS, BUT CURRENTLY FAIL DUE TO A KNOWN ISSUE: #724 
        /// Because two identical requests were sent, whichever completes first will remove the request from pending telemetry.
        /// </remarks>
        [Ignore]
        [TestMethod]
        public void VerifyOnRequestWithDuplicateRequestCreatesValidTelemetry()
        {
            Guid loggingRequestId = Guid.NewGuid();

            var requestMsg = new HttpRequestMessage(HttpMethod.Get, RequestUrlWithScheme);

            using (var listener = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V1))
            {
                // first request, expected to fail
                listener.OnRequest(requestMsg, loggingRequestId);

                // second request (BEFORE HANDLING FIRST RESPONSE), expected to pass 
                listener.OnRequest(requestMsg, loggingRequestId);

                Assert.IsTrue(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out _));
                Assert.AreEqual(0, this.sentTelemetry.Count);

                // first request fails
                HttpResponseMessage responseMsg1 = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    RequestMessage = requestMsg
                };

                listener.OnResponse(responseMsg1, loggingRequestId);
                Assert.IsFalse(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out _));
                Assert.AreEqual(1, this.sentTelemetry.Count);
                Assert.AreEqual(false,
                    ((DependencyTelemetry)this.sentTelemetry.Last()).Success); // first request fails

                Assert.Inconclusive();

                // THE FOLLOWING ASSERTS ARE EXPECTED TO PASS, BUT CURRENTLY FAIL DUE TO A KNOWN ISSUE: #724 
                // Because two identical requests were sent, whichever completes first will remove the request from pending telemetry.

                // second request is success
                HttpResponseMessage responseMsg2 = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    RequestMessage = requestMsg
                };

                listener.OnResponse(responseMsg2, loggingRequestId);
                Assert.IsFalse(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out _));
                Assert.AreEqual(2, this.sentTelemetry.Count);
                Assert.AreEqual(true,
                    ((DependencyTelemetry)this.sentTelemetry.Last()).Success); // second request is success
            }
        }

        /// <summary>
        /// Call OnRequest() with uri that is in the excluded domain list.
        /// </summary>
        [TestMethod]
        public void OnRequestWithUriInExcludedDomainList()
        {
            using (var listener = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V1))
            {
                Guid loggingRequestId = Guid.NewGuid();
                HttpRequestMessage requestMsg =
                    new HttpRequestMessage(HttpMethod.Post, "http://excluded.host.com/path/to/file.html");
                listener.OnRequest(requestMsg, loggingRequestId);

                Assert.IsTrue(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out _));
                Assert.AreEqual(0, this.sentTelemetry.Count);

                Assert.IsNull(HttpHeadersUtilities.GetRequestContextKeyValue(requestMsg.Headers,
                    RequestResponseHeaders.RequestContextCorrelationSourceKey));
                Assert.IsNull(HttpHeadersUtilities.GetRequestContextKeyValue(requestMsg.Headers,
                    RequestResponseHeaders.RequestIdHeader));
                Assert.IsNull(HttpHeadersUtilities.GetRequestContextKeyValue(requestMsg.Headers,
                    RequestResponseHeaders.StandardParentIdHeader));
                Assert.IsNull(HttpHeadersUtilities.GetRequestContextKeyValue(requestMsg.Headers,
                    RequestResponseHeaders.StandardRootIdHeader));
            }
        }

        /// <summary>
        /// OnRequest() injects legacy headers when configured to do so.
        /// </summary>
        [TestMethod]
        public void OnRequestInjectsLegacyHeaders()
        {
            var listenerWithLegacyHeaders = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new[] { "excluded.host.com" },
                injectLegacyHeaders: true,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V1);

            using (listenerWithLegacyHeaders)
            {
                Guid loggingRequestId = Guid.NewGuid();
                HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);
                listenerWithLegacyHeaders.OnRequest(requestMsg, loggingRequestId);

                Assert.IsTrue(
                    listenerWithLegacyHeaders.PendingDependencyTelemetry.TryGetValue(requestMsg, out var dependency));
                Assert.AreEqual(0, this.sentTelemetry.Count);

                var legacyRootIdHeader = GetRequestHeaderValues(requestMsg, RequestResponseHeaders.StandardRootIdHeader)
                    .Single();
                var legacyParentIdHeader =
                    GetRequestHeaderValues(requestMsg, RequestResponseHeaders.StandardParentIdHeader).Single();
                var requestIdHeader = GetRequestHeaderValues(requestMsg, RequestResponseHeaders.RequestIdHeader).Single();

                Assert.AreEqual(dependency.Telemetry.Id, legacyParentIdHeader);
                Assert.AreEqual(dependency.Telemetry.Context.Operation.Id, legacyRootIdHeader);
                Assert.AreEqual($"|{dependency.Telemetry.Context.Operation.Id}.{dependency.Telemetry.Id}.", requestIdHeader);
            }
        }

        /// <summary>
        /// OnRequest() injects legacy headers when configured to do so.
        /// </summary>
        [TestMethod]
        public void OnRequestInjectsLegacyHeadersW3COff()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
            Activity.ForceDefaultIdFormat = true;

            var listenerWithLegacyHeaders = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new[] { "excluded.host.com" },
                injectLegacyHeaders: true,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V1);

            using (listenerWithLegacyHeaders)
            {
                Guid loggingRequestId = Guid.NewGuid();
                HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);
                listenerWithLegacyHeaders.OnRequest(requestMsg, loggingRequestId);

                Assert.IsTrue(
                    listenerWithLegacyHeaders.PendingDependencyTelemetry.TryGetValue(requestMsg, out var dependency));
                Assert.AreEqual(0, this.sentTelemetry.Count);

                var legacyRootIdHeader = GetRequestHeaderValues(requestMsg, RequestResponseHeaders.StandardRootIdHeader)
                    .Single();
                var legacyParentIdHeader =
                    GetRequestHeaderValues(requestMsg, RequestResponseHeaders.StandardParentIdHeader).Single();
                var requestIdHeader = GetRequestHeaderValues(requestMsg, RequestResponseHeaders.RequestIdHeader).Single();

                Assert.AreEqual(dependency.Telemetry.Id, legacyParentIdHeader);
                Assert.AreEqual(dependency.Telemetry.Context.Operation.Id, legacyRootIdHeader);
                Assert.AreEqual(dependency.Telemetry.Id, requestIdHeader);
            }
        }

        /// <summary>
        /// OnRequest() does not inject legacy headers when configured to do so.
        /// </summary>
        [TestMethod]
        public void OnRequestDoesNotInjectLegacyHeaders()
        {
            Guid loggingRequestId = Guid.NewGuid();
            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);
            using (var listener = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V1))
            {
                listener.OnRequest(requestMsg, loggingRequestId);

                Assert.IsTrue(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out var dependency));
                Assert.AreEqual(0, this.sentTelemetry.Count);

                var requestIdHeader =
                    GetRequestHeaderValues(requestMsg, RequestResponseHeaders.RequestIdHeader).Single();
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.StandardRootIdHeader));
                Assert.IsFalse(requestMsg.Headers.Contains(RequestResponseHeaders.StandardParentIdHeader));
                Assert.AreEqual($"|{dependency.Telemetry.Context.Operation.Id}.{dependency.Telemetry.Id}.", requestIdHeader);
            }
        }

        /// <summary>
        /// Call OnRequest() with valid arguments.
        /// </summary>
        [TestMethod]
        public void OnRequestWithRequestEvent()
        {
            Guid loggingRequestId = Guid.NewGuid();
            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V1))
            {
                listener.OnRequest(requestMsg, loggingRequestId);

                Assert.IsTrue(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out var dependency));

                DependencyTelemetry telemetry = dependency.Telemetry;
                Assert.AreEqual("POST /", telemetry.Name);
                Assert.AreEqual(RequestUrl, telemetry.Target);
                Assert.AreEqual(RemoteDependencyConstants.HTTP, telemetry.Type);
                Assert.AreEqual(RequestUrlWithScheme, telemetry.Data);
                Assert.AreEqual(string.Empty, telemetry.ResultCode);
                Assert.AreEqual(true, telemetry.Success);

                Assert.AreEqual(this.testApplicationId1,
                    GetRequestContextKeyValue(requestMsg, RequestResponseHeaders.RequestContextCorrelationSourceKey));

                var requestIdHeader =
                    GetRequestHeaderValues(requestMsg, RequestResponseHeaders.RequestIdHeader).Single();
                Assert.IsFalse(string.IsNullOrEmpty(requestIdHeader));
                Assert.AreEqual(0, this.sentTelemetry.Count);
            }
        }

        /// <summary>
        /// Call OnResponse() when no matching OnRequest() call has been made.
        /// </summary>
        [TestMethod]
        public void OnResponseWithResponseEventButNoMatchingRequest()
        {
            var responseMsg = new HttpResponseMessage
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Get, RequestUrlWithScheme)
            };

            using (var listener = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V1))
            {
                listener.OnResponse(responseMsg, Guid.NewGuid());
                Assert.AreEqual(0, this.sentTelemetry.Count);
            }
        }

        /// <summary>
        /// Call OnResponse() with a successful request but no target instrumentation key in the response headers.
        /// </summary>
        [TestMethod]
        public void OnResponseWithSuccessfulResponseEventWithMatchingRequestAndNoTargetInstrumentationKeyHasHeader()
        {
            Guid loggingRequestId = Guid.NewGuid();
            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V1))
            {
                listener.OnRequest(requestMsg, loggingRequestId);

                Assert.IsTrue(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out var dependency));

                Assert.AreEqual(0, this.sentTelemetry.Count);

                DependencyTelemetry telemetry = dependency.Telemetry;
                Assert.AreEqual(string.Empty, telemetry.ResultCode);
                Assert.AreEqual(true, telemetry.Success);

                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    RequestMessage = requestMsg
                };

                listener.OnResponse(responseMsg, loggingRequestId);
                Assert.IsFalse(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out dependency));
                Assert.AreEqual(1, this.sentTelemetry.Count);
                Assert.AreSame(telemetry, this.sentTelemetry.Single());

                Assert.AreEqual(RemoteDependencyConstants.HTTP, telemetry.Type);
                Assert.AreEqual(RequestUrl, telemetry.Target);
                Assert.AreEqual(HttpOkResultCode, telemetry.ResultCode);
                Assert.AreEqual(true, telemetry.Success);

                string expectedVersion =
                    SdkVersionHelper.GetExpectedSdkVersion(typeof(DependencyTrackingTelemetryModule),
                        prefix: "rdddsc:");
                Assert.AreEqual(expectedVersion, telemetry.Context.GetInternalContext().SdkVersion);

                // Check the operation details
                this.operationDetailsInitializer.ValidateOperationDetailsCore(telemetry);
            }
        }

        /// <summary>
        /// Call OnResponse() with a not found request result but no target instrumentation key in the response headers.
        /// </summary>
        [TestMethod]
        public void OnResponseWithNotFoundResponseEventWithMatchingRequestAndNoTargetInstrumentationKeyHasHeader()
        {
            Guid loggingRequestId = Guid.NewGuid();
            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V1))
            {
                listener.OnRequest(requestMsg, loggingRequestId);

                Assert.IsTrue(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out var dependency));

                Assert.AreEqual(0, this.sentTelemetry.Count);

                DependencyTelemetry telemetry = dependency.Telemetry;
                Assert.AreEqual(string.Empty, telemetry.ResultCode);
                Assert.AreEqual(true, telemetry.Success);

                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    RequestMessage = requestMsg
                };

                listener.OnResponse(responseMsg, loggingRequestId);
                Assert.IsFalse(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out dependency));
                Assert.AreEqual(1, this.sentTelemetry.Count);
                Assert.AreSame(telemetry, this.sentTelemetry.Single());

                Assert.AreEqual(RemoteDependencyConstants.HTTP, telemetry.Type);
                Assert.AreEqual(RequestUrl, telemetry.Target);
                Assert.AreEqual(NotFoundResultCode, telemetry.ResultCode);
                Assert.AreEqual(false, telemetry.Success);

                // Check the operation details
                this.operationDetailsInitializer.ValidateOperationDetailsCore(telemetry);
            }
        }

        /// <summary>
        /// Call OnResponse() with a successful request and same target instrumentation key in the response headers as the source instrumentation key.
        /// </summary>
        [TestMethod]
        public void OnResponseWithSuccessfulResponseEventWithMatchingRequestAndSameTargetInstrumentationKeyHashHeader()
        {
            Guid loggingRequestId = Guid.NewGuid();
            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V1))
            {
                listener.OnRequest(requestMsg, loggingRequestId);
                Assert.IsTrue(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out var dependency));

                Assert.AreEqual(0, this.sentTelemetry.Count);

                DependencyTelemetry telemetry = dependency.Telemetry;
                Assert.AreEqual(string.Empty, telemetry.ResultCode);
                Assert.AreEqual(true, telemetry.Success, "request was not successful");

                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    RequestMessage = requestMsg
                };

                responseMsg.Headers.Add(RequestResponseHeaders.RequestContextCorrelationTargetKey,
                    this.testApplicationId1);

                listener.OnResponse(responseMsg, loggingRequestId);
                Assert.IsFalse(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out dependency));
                Assert.AreEqual(1, this.sentTelemetry.Count);
                Assert.AreSame(telemetry, this.sentTelemetry.Single());

                Assert.AreEqual(RemoteDependencyConstants.HTTP, telemetry.Type);
                Assert.AreEqual(RequestUrl, telemetry.Target);
                Assert.AreEqual(HttpOkResultCode, telemetry.ResultCode);
                Assert.AreEqual(true, telemetry.Success, "response was not successful");

                // Check the operation details
                this.operationDetailsInitializer.ValidateOperationDetailsCore(telemetry);
            }
        }

        /// <summary>
        /// Call OnResponse() with a not found request result code and same target instrumentation key in the response headers as the source instrumentation key.
        /// </summary>
        [TestMethod]
        public void OnResponseWithFailedResponseEventWithMatchingRequestAndSameTargetInstrumentationKeyHashHeader()
        {
            Guid loggingRequestId = Guid.NewGuid();
            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V1))
            {
                listener.OnRequest(requestMsg, loggingRequestId);
                Assert.IsTrue(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out var dependency));
                Assert.AreEqual(0, this.sentTelemetry.Count);

                DependencyTelemetry telemetry = dependency.Telemetry;
                Assert.AreEqual(string.Empty, telemetry.ResultCode);
                Assert.AreEqual(true, telemetry.Success);

                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    RequestMessage = requestMsg
                };

                responseMsg.Headers.Add(RequestResponseHeaders.RequestContextCorrelationTargetKey,
                    this.testApplicationId1);

                listener.OnResponse(responseMsg, loggingRequestId);
                Assert.IsFalse(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out dependency));
                Assert.AreEqual(1, this.sentTelemetry.Count);
                Assert.AreSame(telemetry, this.sentTelemetry.Single());

                Assert.AreEqual(RemoteDependencyConstants.HTTP, telemetry.Type);
                Assert.AreEqual(RequestUrl, telemetry.Target);
                Assert.AreEqual(NotFoundResultCode, telemetry.ResultCode);
                Assert.AreEqual(false, telemetry.Success);

                // Check the operation details
                this.operationDetailsInitializer.ValidateOperationDetailsCore(telemetry);
            }
        }

        /// <summary>
        /// Call OnResponse() with a successful request and different target instrumentation key in the response headers than the source instrumentation key.
        /// </summary>
        [TestMethod]
        public void OnResponseWithSuccessfulResponseEventWithMatchingRequestAndDifferentTargetInstrumentationKeyHashHeader()
        {
            Guid loggingRequestId = Guid.NewGuid();
            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V1))
            {
                listener.OnRequest(requestMsg, loggingRequestId);
                Assert.IsTrue(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out var dependency));
                Assert.AreEqual(0, this.sentTelemetry.Count);

                DependencyTelemetry telemetry = dependency.Telemetry;
                Assert.AreEqual(string.Empty, telemetry.ResultCode);
                Assert.AreEqual(true, telemetry.Success);

                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    RequestMessage = requestMsg
                };

                string targetApplicationId = this.testApplicationId2;
                HttpHeadersUtilities.SetRequestContextKeyValue(responseMsg.Headers,
                    RequestResponseHeaders.RequestContextCorrelationTargetKey, targetApplicationId);

                listener.OnResponse(responseMsg, loggingRequestId);
                Assert.IsFalse(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out dependency));
                Assert.AreEqual(1, this.sentTelemetry.Count);
                Assert.AreSame(telemetry, this.sentTelemetry.Single());

                Assert.AreEqual(RemoteDependencyConstants.AI, telemetry.Type);
                Assert.AreEqual(GetApplicationInsightsTarget(targetApplicationId), telemetry.Target);
                Assert.AreEqual(HttpOkResultCode, telemetry.ResultCode);
                Assert.AreEqual(true, telemetry.Success);

                // Check the operation details
                this.operationDetailsInitializer.ValidateOperationDetailsCore(telemetry);
            }
        }

        /// <summary>
        /// Call OnResponse() with a not found request result code and different target instrumentation key in the response headers than the source instrumentation key.
        /// </summary>
        [TestMethod]
        public void OnResponseWithFailedResponseEventWithMatchingRequestAndDifferentTargetInstrumentationKeyHashHeader()
        {
            Guid loggingRequestId = Guid.NewGuid();
            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V1))
            {
                listener.OnRequest(requestMsg, loggingRequestId);
                Assert.IsTrue(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out var dependency));
                Assert.AreEqual(0, this.sentTelemetry.Count);

                DependencyTelemetry telemetry = dependency.Telemetry;
                Assert.AreEqual(string.Empty, telemetry.ResultCode);
                Assert.AreEqual(true, telemetry.Success);

                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    RequestMessage = requestMsg
                };

                string targetApplicationId = this.testApplicationId2;
                HttpHeadersUtilities.SetRequestContextKeyValue(responseMsg.Headers,
                    RequestResponseHeaders.RequestContextCorrelationTargetKey, targetApplicationId);

                listener.OnResponse(responseMsg, loggingRequestId);
                Assert.IsFalse(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out dependency));
                Assert.AreEqual(1, this.sentTelemetry.Count);
                Assert.AreSame(telemetry, this.sentTelemetry.Single());

                Assert.AreEqual(RemoteDependencyConstants.AI, telemetry.Type);
                Assert.AreEqual(GetApplicationInsightsTarget(targetApplicationId), telemetry.Target);
                Assert.AreEqual(NotFoundResultCode, telemetry.ResultCode);
                Assert.AreEqual(false, telemetry.Success);

                // Check the operation details
                this.operationDetailsInitializer.ValidateOperationDetailsCore(telemetry);
            }
        }

        /// <summary>
        /// Tests that outgoing request has proper context when done in scope of incoming request (incoming request activity).
        /// </summary>
        [TestMethod]
        public void OnResponseWithParentActivity()
        {
            var parentActivity = new Activity("incoming_request");
            parentActivity.AddBaggage("k1", "v1");
            parentActivity.AddBaggage("k2", "v2");
            parentActivity.Start();
            parentActivity.TraceStateString = "state=some";

            Guid loggingRequestId = Guid.NewGuid();
            HttpRequestMessage requestMsg = new HttpRequestMessage(HttpMethod.Post, RequestUrlWithScheme);

            using (var listener = new HttpCoreDiagnosticSourceListener(
                this.configuration,
                setComponentCorrelationHttpHeaders: true,
                correlationDomainExclusionList: new string[] { "excluded.host.com" },
                injectLegacyHeaders: false,
                injectRequestIdInW3CMode: true,
                HttpInstrumentationVersion.V1))
            {
                listener.OnRequest(requestMsg, loggingRequestId);
                Assert.IsNotNull(Activity.Current);

                var requestIdHeader = requestMsg.Headers.GetValues(RequestResponseHeaders.RequestIdHeader).Single();
                var traceparentHeader = requestMsg.Headers.GetValues("traceparent").Single();
                var tracestateHeader = requestMsg.Headers.GetValues("tracestate").Single();

                Assert.AreEqual($"|{Activity.Current.TraceId.ToHexString()}.{Activity.Current.SpanId.ToHexString()}.", requestIdHeader);
                Assert.AreEqual(Activity.Current.Id, traceparentHeader);
                Assert.AreEqual("state=some", tracestateHeader);

                var correlationContextHeader =
                    requestMsg.Headers.GetValues(RequestResponseHeaders.CorrelationContextHeader).ToArray();
                Assert.AreEqual(2, correlationContextHeader.Length);
                Assert.IsTrue(correlationContextHeader.Contains("k1=v1"));
                Assert.IsTrue(correlationContextHeader.Contains("k2=v2"));

                Assert.IsTrue(listener.PendingDependencyTelemetry.TryGetValue(requestMsg, out var dependency));

                DependencyTelemetry telemetry = dependency.Telemetry;

                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    RequestMessage = requestMsg
                };

                listener.OnResponse(responseMsg, loggingRequestId);
                Assert.AreEqual(parentActivity, Activity.Current);
                Assert.AreEqual(requestIdHeader.Substring(34, 16), telemetry.Id);
                Assert.AreEqual(parentActivity.RootId, telemetry.Context.Operation.Id);
                Assert.AreEqual(parentActivity.SpanId.ToHexString(), telemetry.Context.Operation.ParentId);
                Assert.AreEqual("state=some", telemetry.Properties["tracestate"]);

                // Check the operation details
                this.operationDetailsInitializer.ValidateOperationDetailsCore(telemetry);
            }

            parentActivity.Stop();
        }

        private static string GetApplicationInsightsTarget(string targetApplicationId)
        {
            return $"{RequestUrl} | {targetApplicationId}";
        }

        private static IEnumerable<string> GetRequestHeaderValues(HttpRequestMessage request, string headerName)
        {
            return HttpHeadersUtilities.GetHeaderValues(request.Headers, headerName);
        }

        private static string GetRequestContextKeyValue(HttpRequestMessage request, string keyName)
        {
            return HttpHeadersUtilities.GetRequestContextKeyValue(request.Headers, keyName);
        }
    }
}