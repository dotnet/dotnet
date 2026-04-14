namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Web;

    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Experimental;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Web.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Web.Implementation;

    /// <summary>
    /// Telemetry module tracking requests using http module.
    /// </summary>
    public class RequestTrackingTelemetryModule : ITelemetryModule
    {
        /// <summary>Tracks if given type should be included in telemetry. ConcurrentDictionary is used as a concurrent hashset.</summary>
        private readonly ConcurrentDictionary<Type, bool> includedHttpHandlerTypes = new ConcurrentDictionary<Type, bool>();

        private TelemetryClient telemetryClient;
        private TelemetryConfiguration telemetryConfiguration;
        private bool initializationErrorReported;
        private ChildRequestTrackingSuppressionModule childRequestTrackingSuppressionModule = null;
        
        /// <summary>
        /// Gets or sets a value indicating whether child request suppression is enabled or disabled. 
        /// True by default.
        /// This value is evaluated in Initialize().
        /// </summary>
        /// <remarks>
        /// See also <see cref="ChildRequestTrackingSuppressionModule" />.
        /// Child requests caused by <see cref="System.Web.Handlers.TransferRequestHandler" />.
        /// Unit tests should disable this.
        /// </remarks>
        public bool EnableChildRequestTrackingSuppression { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether Request-Id header is added to Access-Control-Expose-Headers or not. 
        /// True by default.
        /// </summary>
        public bool EnableAccessControlExposeHeader { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating the size of internal tracking dictionary.
        /// Must be a positive integer.
        /// </summary>
        /// <remarks>
        /// See also <see cref="ChildRequestTrackingSuppressionModule" />.
        /// </remarks>
        public int ChildRequestTrackingInternalDictionarySize { get; set; }

        /// <summary>
        /// Gets the list of handler types for which requests telemetry will not be collected
        /// if request was successful.
        /// </summary>
        public IList<string> Handlers { get; } = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether the component correlation headers would be set on http responses.
        /// </summary>
        public bool SetComponentCorrelationHttpHeaders { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to enable W3C distributed tracing headers support.
        /// </summary>
        [Obsolete("This flag is obsolete and noop. Use System.Diagnostics.Activity.DefaultIdFormat (along with ForceDefaultIdFormat) flags instead.")] 
        public bool EnableW3CHeadersExtraction { get; set; } = true;

        /// <summary>
        /// Gets or sets the endpoint that is to be used to get the application insights resource's profile (appId etc.).
        /// </summary>
        [Obsolete("This field has been deprecated. Please set TelemetryConfiguration.Active.ApplicationIdProvider = new ApplicationInsightsApplicationIdProvider() and customize ApplicationInsightsApplicationIdProvider.ProfileQueryEndpoint.")]
        public string ProfileQueryEndpoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether requestTelemetry.Url and requestTelemetry.Source are disabled.
        /// Customers would need to use the <see cref="PostSamplingTelemetryProcessor" /> to defer setting these properties.
        /// </summary>
        /// <remarks>
        /// This feature is still being evaluated and not recommended for end users.
        /// This setting is not browsable at this time.
        /// </remarks>
        internal bool DisableTrackingProperties { get; set; } = false;

        /// <summary>
        /// Implements on begin callback of http module.
        /// </summary>
        public void OnBeginRequest(HttpContext context)
        {
            if (this.telemetryClient == null)
            {
                if (!this.initializationErrorReported)
                {
                    this.initializationErrorReported = true;
                    WebEventSource.Log.InitializeHasNotBeenCalledOnModuleYetError();
                }
                else
                {
                    WebEventSource.Log.InitializeHasNotBeenCalledOnModuleYetVerbose();
                }

                return;
            }

            if (context == null)
            {
                WebEventSource.Log.NoHttpContextWarning();
                return;
            }

            this.childRequestTrackingSuppressionModule?.OnBeginRequest_IdRequest(context);

            var telemetry = context.ReadOrCreateRequestTelemetryPrivate();

            // NB! Whatever is saved in RequestTelemetry on Begin is not guaranteed to be sent because Begin may not be called; Keep it in context
            // In WCF there will be 2 Begins and 1 End. We need time from the first one
            if (telemetry.Timestamp == DateTimeOffset.MinValue)
            {
                telemetry.Start();
            }
        }

        /// <summary>
        /// Implements on end callback of http module.
        /// </summary>
        public void OnEndRequest(HttpContext context)
        {
            if (this.telemetryClient == null)
            {
                if (!this.initializationErrorReported)
                {
                    this.initializationErrorReported = true;
                    WebEventSource.Log.InitializeHasNotBeenCalledOnModuleYetError();
                }
                else
                {
                    WebEventSource.Log.InitializeHasNotBeenCalledOnModuleYetVerbose();
                }

                return;
            }

            if (!this.NeedProcessRequest(context))
            {
                return;
            }

            var requestTelemetry = context.ReadOrCreateRequestTelemetryPrivate();
            requestTelemetry.Stop();

            var success = true;
            if (string.IsNullOrEmpty(requestTelemetry.ResponseCode))
            {
                var statusCode = context.Response.StatusCode;
                requestTelemetry.ResponseCode = statusCode == 200 ? "200" : statusCode.ToString(CultureInfo.InvariantCulture);

                if (statusCode >= 400 && statusCode != 401)
                {
                    success = false;
                }
            }

            if (!requestTelemetry.Success.HasValue)
            {
                requestTelemetry.Success = success;
            }

            if (string.IsNullOrEmpty(requestTelemetry.Context.InstrumentationKey))
            {
                // Instrumentation key is probably empty, because the context has not yet had a chance to associate the requestTelemetry to the telemetry client yet.
                // and get they instrumentation key from all possible sources in the process. Let's do that now.
                this.telemetryClient.InitializeInstrumentationKey(requestTelemetry);
            }

            // Setting requestTelemetry.Url and requestTelemetry.Source can be deferred until after sampling
            if (this.DisableTrackingProperties == false)
            {
                RequestTrackingUtilities.UpdateRequestTelemetryFromRequest(requestTelemetry, context.Request, this.telemetryConfiguration?.ApplicationIdProvider);
            }

            if (this.childRequestTrackingSuppressionModule?.OnEndRequest_ShouldLog(context) ?? true)
            {
                this.telemetryClient.TrackRequest(requestTelemetry);
            }
            else
            {
                WebEventSource.Log.RequestTrackingTelemetryModuleRequestWasNotLoggedInformational();
            }
        }

        /// <summary>
        /// Adds target response header response object.
        /// </summary>
        public void AddTargetHashForResponseHeader(HttpContext context)
        {
            if (this.telemetryClient == null)
            {
                throw new InvalidOperationException();
            }

            var requestTelemetry = context.GetRequestTelemetry();

            if (string.IsNullOrEmpty(requestTelemetry.Context.InstrumentationKey))
            {
                // Instrumentation key is probably empty, because the context has not yet had a chance to associate the requestTelemetry to the telemetry client yet.
                // and get they instrumentation key from all possible sources in the process. Let's do that now.
                this.telemetryClient.InitializeInstrumentationKey(requestTelemetry);
            }

            try
            {
                if (!string.IsNullOrEmpty(requestTelemetry.Context.InstrumentationKey)
                    && context.Response.Headers.GetNameValueHeaderValue(
                        RequestResponseHeaders.RequestContextHeader, 
                        RequestResponseHeaders.RequestContextCorrelationTargetKey) == null)
                {
                    string applicationId = null;
                    if (this.telemetryConfiguration.ApplicationIdProvider?.TryGetApplicationId(requestTelemetry.Context.InstrumentationKey, out applicationId) ?? false)
                    {
                        context.Response.Headers.SetNameValueHeaderValue(RequestResponseHeaders.RequestContextHeader, RequestResponseHeaders.RequestContextCorrelationTargetKey, applicationId);

                        if (this.EnableAccessControlExposeHeader)
                        {
                            // set additional header that allows to read this Request-Context from Javascript SDK
                            // append this header with additional value to the potential ones defined by customer and they will be concatenated on client-side
                            context.Response.AppendHeader(RequestResponseHeaders.AccessControlExposeHeadersHeader, RequestResponseHeaders.RequestContextHeader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppMapCorrelationEventSource.Log.SetCrossComponentCorrelationHeaderFailed(ex.ToInvariantString());
            }
        }

        /// <summary>
        /// Initializes the telemetry module.
        /// </summary>
        /// <param name="configuration">Telemetry configuration to use for initialization.</param>
        public void Initialize(TelemetryConfiguration configuration)
        {
            this.telemetryConfiguration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.telemetryClient = new TelemetryClient(configuration);
            this.telemetryClient.Context.GetInternalContext().SdkVersion = SdkVersionUtils.GetSdkVersion("web:");

            this.DisableTrackingProperties = configuration.EvaluateExperimentalFeature(Microsoft.ApplicationInsights.Common.Internal.ExperimentalConstants.DeferRequestTrackingProperties);
            if (this.DisableTrackingProperties)
            {
                configuration.DefaultTelemetrySink.TelemetryProcessorChainBuilder.Use(next => new PostSamplingTelemetryProcessor(next));
                configuration.DefaultTelemetrySink.TelemetryProcessorChainBuilder.Build();
            }

            // Headers will be read-only in a classic iis pipeline
            // Exception System.PlatformNotSupportedException: This operation requires IIS integrated pipeline mode.
            if (HttpRuntime.UsingIntegratedPipeline && this.EnableChildRequestTrackingSuppression)
            {
                this.childRequestTrackingSuppressionModule = new ChildRequestTrackingSuppressionModule(maxRequestsTracked: this.ChildRequestTrackingInternalDictionarySize);
            }
        }

        /// <summary>
        /// Verifies context to detect whether or not request needs to be processed.
        /// </summary>
        /// <param name="httpContext">Current http context.</param>
        /// <returns>True if request needs to be processed, otherwise - False.</returns>
        internal bool NeedProcessRequest(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                WebEventSource.Log.NoHttpContextWarning();
                return false;
            }

            if (httpContext.Response.StatusCode < 400)
            {
                if (this.IsHandlerToFilter(httpContext.Handler))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks whether or not handler is a transfer handler.
        /// </summary>
        /// <param name="handler">An instance of handler to validate.</param>
        /// <returns>True if handler is a transfer handler, otherwise - False.</returns>
        private bool IsHandlerToFilter(IHttpHandler handler)
        {
            if (handler != null)
            {
                var handlerType = handler.GetType();
                if (!this.includedHttpHandlerTypes.ContainsKey(handlerType))
                {
                    var handlerName = handlerType.FullName;
                    foreach (var h in this.Handlers)
                    {
                        if (string.Equals(handlerName, h, StringComparison.Ordinal))
                        {
                            WebEventSource.Log.WebRequestFilteredOutByRequestHandler();
                            return true;
                        }
                    }

                    this.includedHttpHandlerTypes.TryAdd(handlerType, true);
                }
            }

            return false;
        }

        /// <summary>
        /// <see cref="System.Web.Handlers.TransferRequestHandler"/> can create a Child request to route extension-less requests to a controller.
        /// (ex: site/home -> site/HomeController.cs)
        /// We do not want duplicate telemetry logged for both the Parent and Child requests, so the activeRequests will be created OnBeginRequest.
        /// When the child request OnEndRequest, the id will be removed from this dictionary and telemetry will not be logged for the parent.
        /// </summary>
        /// <remarks>
        /// Unit tests should disable the ChildRequestTrackingSuppressionModule.
        /// Unit test projects cannot create an [internal] IIS7WorkerRequest object.
        /// Without this object, we cannot modify the Request.Headers without throwing a PlatformNotSupportedException.
        /// Unit tests will have to initialize the RequestIdHeader.
        /// The second IF will ensure the id is added to the activeRequests.
        /// </remarks>
        /// <remarks>
        /// IIS Classic Pipeline should disable the ChildRequestTrackingSuppressionModule.
        /// Classic does not create IIS7WorkerRequest object and Headers will be read-only.
        /// (Exception System.PlatformNotSupportedException: This operation requires IIS integrated pipeline mode.)
        /// </remarks>
        private class ChildRequestTrackingSuppressionModule
        {
            private const int DEFAULTMAXVALUE = 100000;

            private const string HeaderRootRequestId = "ApplicationInsights-RequestTrackingTelemetryModule-RootRequest-Id";
#if DEBUG
            private const string HeaderParentRequestId = "ApplicationInsights-RequestTrackingTelemetryModule-ParentRequest-Id";
            private const string HeaderRequestId = "ApplicationInsights-RequestTrackingTelemetryModule-Request-Id";
#endif

            private static object semaphore = new object();

            /// <summary>
            /// Using this as a hash-set of current active requests. The value of the Dictionary is not used.
            /// </summary>
            private static ConcurrentDictionary<string, bool> activeRequestsA = new ConcurrentDictionary<string, bool>(System.Environment.ProcessorCount, DEFAULTMAXVALUE);
            private static ConcurrentDictionary<string, bool> activeRequestsB = new ConcurrentDictionary<string, bool>(System.Environment.ProcessorCount, DEFAULTMAXVALUE);

            /// <summary>
            /// Initializes a new instance of the <see cref="ChildRequestTrackingSuppressionModule" /> class.
            /// </summary>
            /// <param name="maxRequestsTracked">The maximum number of active requests to be tracked before resetting the dictionary.</param>
            internal ChildRequestTrackingSuppressionModule(int maxRequestsTracked)
            {
                this.MAXSIZE = maxRequestsTracked > 0 ? maxRequestsTracked : DEFAULTMAXVALUE;
            }

            /// <summary>
            /// Gets the Max number of request ids to cache.
            /// </summary>
            internal int MAXSIZE { get; private set; }

            /// <summary>
            /// Request will be tagged with an id to identify if it should be logged later.
            /// </summary>
            internal void OnBeginRequest_IdRequest(HttpContext context)
            {
                if (context?.Request?.Headers == null)
                {
                    return;
                }

                try
                {
                    TagRequest(context);
                }
                catch (Exception ex)
                {
                   WebEventSource.Log.ChildRequestUnknownException(nameof(this.OnBeginRequest_IdRequest), ex.ToInvariantString());
                }
            }

            /// <summary>
            /// OnEndRequest - Should this request be logged?
            /// Will compare a request id against a hash-set of known requests.
            /// If this request is not known, add it to hash-set and return true (safe to log).
            /// If this request is known, return false (do not log twice).
            /// Additional requests with the same id will return false.
            /// </summary>
            internal bool OnEndRequest_ShouldLog(HttpContext context)
            {
                var headers = context?.Request?.Headers;
                if (headers == null)
                {
                    return false;
                }

                try
                {
                    var rootRequestId = headers[HeaderRootRequestId];
                    if (rootRequestId != null)
                    {
                        rootRequestId = StringUtilities.EnforceMaxLength(rootRequestId, InjectionGuardConstants.RequestHeaderMaxLength);
                        if (!IsRequestKnown(rootRequestId))
                        {
                            // doesn't exist add to dictionary and return true
                            this.AddRequestToDictionary(rootRequestId);
                            return true;
                        }
                        else
                        {
                            WebEventSource.Log.RequestTrackingTelemetryModuleRequestWasNotLoggedVerbose(rootRequestId, "Request is already known");
                        }
                    }
                    else
                    {
                        WebEventSource.Log.RequestTrackingTelemetryModuleRequestWasNotLoggedVerbose(rootRequestId, "Request id is null");
                    }
                }
                catch (Exception ex)
                {
                    WebEventSource.Log.ChildRequestUnknownException(nameof(this.OnEndRequest_ShouldLog), ex.ToInvariantString());
                }

                return false;
            }

            /// <summary>
            /// Tag new requests.
            /// Transfer Ids to parent requests.
            /// </summary>
            private static void TagRequest(HttpContext context)
            {
                var headers = context.Request.Headers;

                if (headers[HeaderRootRequestId] == null)
                {
                    headers[HeaderRootRequestId] = Guid.NewGuid().ToString();
                }

#if DEBUG
                // additional ids help developer watch request hierarchy while debugging.
                if (headers[HeaderRequestId] != null)
                {
                    headers[HeaderParentRequestId] = headers[HeaderRequestId];
                    headers[HeaderRequestId] = Guid.NewGuid().ToString();
                }
                else
                {
                    headers[HeaderRequestId] = headers[HeaderRootRequestId];
                }
#endif
            }

            /// <summary>
            /// Has this request been tracked.
            /// </summary>
            private static bool IsRequestKnown(string requestId)
            {
                return activeRequestsA.ContainsKey(requestId) || activeRequestsB.ContainsKey(requestId);
            }

            /// <summary>
            /// Track this requestId.
            /// </summary>
            /// <remarks>
            /// Dictionary A will be read/write.
            /// When dictionary A is full, move to B and create new A.
            /// Dictionary B will be read-only.
            /// </remarks>
            private void AddRequestToDictionary(string requestId)
            {
                if (activeRequestsA.Count >= this.MAXSIZE)
                {
                    // only lock around the edge case to avoid locking EVERY request thread
                    lock (semaphore)
                    {
                        // in the event that multiple threads step into the first if, 
                        // check condition again to avoid repeat operations.
                        if (activeRequestsA.Count >= this.MAXSIZE)
                        {
                            activeRequestsB = activeRequestsA;
                            activeRequestsA = new ConcurrentDictionary<string, bool>(System.Environment.ProcessorCount, this.MAXSIZE);
                        }
                    }
                }

                activeRequestsA.TryAdd(requestId, false);
            }
        }
    }
}