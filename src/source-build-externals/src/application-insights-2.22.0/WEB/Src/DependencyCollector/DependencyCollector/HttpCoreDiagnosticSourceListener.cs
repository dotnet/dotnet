namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// Version of the HttpClient instrumentation.
    /// </summary>
    internal enum HttpInstrumentationVersion
    {
        /// <summary>
        /// Version is not identified.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// NET Core 1.* - deprecated events
        /// </summary>
        V1 = 1,

        /// <summary>
        /// .NET Core 2.* - Activity and new events
        /// </summary>
        V2 = 2,

        /// <summary>
        /// .NET Core 3.* - W3C
        /// </summary>
        V3 = 3,
    }

    internal class HttpCoreDiagnosticSourceListener : IObserver<KeyValuePair<string, object>>, IDisposable
    {
        private const string HttpOutEventName = "System.Net.Http.HttpRequestOut";
        private const string HttpOutStartEventName = "System.Net.Http.HttpRequestOut.Start";
        private const string HttpOutStopEventName = "System.Net.Http.HttpRequestOut.Stop";
        private const string HttpExceptionEventName = "System.Net.Http.Exception";
        private const string DeprecatedRequestEventName = "System.Net.Http.Request";
        private const string DeprecatedResponseEventName = "System.Net.Http.Response";

        private static readonly ActiveSubsciptionManager SubscriptionManager = new ActiveSubsciptionManager();

        private readonly IEnumerable<string> correlationDomainExclusionList;
        private readonly ApplicationInsightsUrlFilter applicationInsightsUrlFilter;
        private readonly bool setComponentCorrelationHttpHeaders;
        private readonly TelemetryClient client;
        private readonly TelemetryConfiguration configuration;
        private readonly HttpCoreDiagnosticSourceSubscriber subscriber;

        #region fetchers

        private readonly PropertyFetcher startRequestFetcher = new PropertyFetcher("Request");
        private readonly PropertyFetcher stopRequestFetcher = new PropertyFetcher("Request");
        private readonly PropertyFetcher stopResponseFetcher = new PropertyFetcher("Response");
        private readonly PropertyFetcher stopRequestStatusFetcher = new PropertyFetcher("RequestTaskStatus");
        private readonly PropertyFetcher deprecatedRequestFetcher = new PropertyFetcher("Request");
        private readonly PropertyFetcher deprecatedResponseFetcher = new PropertyFetcher("Response");
        private readonly PropertyFetcher deprecatedRequestGuidFetcher = new PropertyFetcher("LoggingRequestId");
        private readonly PropertyFetcher deprecatedResponseGuidFetcher = new PropertyFetcher("LoggingRequestId");

        #endregion

        private readonly ConcurrentDictionary<string, Exception> pendingExceptions =
            new ConcurrentDictionary<string, Exception>();

        private readonly HttpInstrumentationVersion httpInstrumentationVersion = HttpInstrumentationVersion.Unknown;
        private readonly bool injectLegacyHeaders = false;
        private readonly bool injectRequestIdInW3CMode = true;

        public HttpCoreDiagnosticSourceListener(
            TelemetryConfiguration configuration, 
            bool setComponentCorrelationHttpHeaders, 
            IEnumerable<string> correlationDomainExclusionList,
            bool injectLegacyHeaders,
            bool injectRequestIdInW3CMode,
            HttpInstrumentationVersion instrumentationVersion)
        {
            this.client = new TelemetryClient(configuration);
            this.client.Context.GetInternalContext().SdkVersion = SdkVersionUtils.GetSdkVersion("rdd" + RddSource.DiagnosticSourceCore + ":");

            this.configuration = configuration;
            this.applicationInsightsUrlFilter = new ApplicationInsightsUrlFilter(configuration);
            this.setComponentCorrelationHttpHeaders = setComponentCorrelationHttpHeaders;
            this.correlationDomainExclusionList = correlationDomainExclusionList ?? Enumerable.Empty<string>();
            this.injectLegacyHeaders = injectLegacyHeaders;
            this.httpInstrumentationVersion = instrumentationVersion != HttpInstrumentationVersion.Unknown ? 
                instrumentationVersion :
                GetInstrumentationVersion();
            this.injectRequestIdInW3CMode = injectRequestIdInW3CMode;
            this.subscriber = new HttpCoreDiagnosticSourceSubscriber(
                this,
                this.applicationInsightsUrlFilter,
                this.httpInstrumentationVersion);
        }

        /// <summary>
        /// Gets the DependencyTelemetry objects that are still waiting for a response from the dependency. This will most likely only be used for testing purposes.
        /// </summary>
        internal ConditionalWeakTable<HttpRequestMessage, IOperationHolder<DependencyTelemetry>> PendingDependencyTelemetry { get; } = new ConditionalWeakTable<HttpRequestMessage, IOperationHolder<DependencyTelemetry>>();

        /// <summary>
        /// Notifies the observer that the provider has finished sending push-based notifications.
        /// <seealso cref="IObserver{T}.OnCompleted()"/>
        /// </summary>
        public void OnCompleted()
        {
        }

        /// <summary>
        /// Notifies the observer that the provider has experienced an error condition.
        /// <seealso cref="IObserver{T}.OnError(Exception)"/>
        /// </summary>
        /// <param name="error">An object that provides additional information about the error.</param>
        public void OnError(Exception error)
        {
        }

        /// <summary>
        /// Provides the observer with new data.
        /// <seealso cref="IObserver{T}.OnNext(T)"/>
        /// </summary>
        /// <param name="evnt">The current notification information.</param>
        public void OnNext(KeyValuePair<string, object> evnt)
        {
            try
            {
                // It's possible to host multiple apps (ASP.NET Core or generic hosts) in the same process
                // Each of this apps has it's own DependencyTrackingModule and corresponding Http listener.
                // We should ignore events for all of them except one
                if (!SubscriptionManager.IsActive(this))
                {
                    DependencyCollectorEventSource.Log.NotActiveListenerNoTracking(evnt.Key, Activity.Current?.Id);
                    return;
                }

                const string errorTemplateTypeCast = "Event {0}: cannot cast {1} to expected type {2}";
                const string errorTemplateValueParse = "Event {0}: cannot parse '{1}' as type {2}";

                switch (evnt.Key)
                {
                    case HttpOutStartEventName:
                        {
                            var request = this.startRequestFetcher.Fetch(evnt.Value) as HttpRequestMessage;

                            if (request == null)
                            {
                                var error = string.Format(CultureInfo.InvariantCulture, errorTemplateTypeCast, evnt.Key, "request", "HttpRequestMessage");
                                DependencyCollectorEventSource.Log.HttpCoreDiagnosticSourceListenerOnNextFailed(error);
                            }
                            else
                            {
                                this.OnActivityStart(request);
                            }

                            break;
                        }

                    case HttpOutStopEventName:
                        {
                            var response = this.stopResponseFetcher.Fetch(evnt.Value) as HttpResponseMessage;
                            var request = this.stopRequestFetcher.Fetch(evnt.Value) as HttpRequestMessage;
                            var requestTaskStatusString = this.stopRequestStatusFetcher.Fetch(evnt.Value).ToString();

                            if (request == null)
                            {
                                var error = string.Format(CultureInfo.InvariantCulture, errorTemplateTypeCast, evnt.Key, "request", "HttpRequestMessage");
                                DependencyCollectorEventSource.Log.HttpCoreDiagnosticSourceListenerOnNextFailed(error);
                            }
                            else if (!Enum.TryParse(requestTaskStatusString, out TaskStatus requestTaskStatus))
                            {
                                var error = string.Format(CultureInfo.InvariantCulture, errorTemplateValueParse, evnt.Key, requestTaskStatusString, "TaskStatus");
                                DependencyCollectorEventSource.Log.HttpCoreDiagnosticSourceListenerOnNextFailed(error);
                            }
                            else
                            {
                                this.OnActivityStop(response, request, requestTaskStatus);
                            }

                            break;
                        }

                    case DeprecatedRequestEventName:
                        {
                            if (this.httpInstrumentationVersion != HttpInstrumentationVersion.V1)
                            {
                                // 2.0+ publishes new events, and this should be just ignored to prevent duplicates.
                                break;
                            }

                            var request = this.deprecatedRequestFetcher.Fetch(evnt.Value) as HttpRequestMessage;
                            var loggingRequestIdString = this.deprecatedRequestGuidFetcher.Fetch(evnt.Value).ToString();

                            if (request == null)
                            {
                                var error = string.Format(CultureInfo.InvariantCulture, errorTemplateTypeCast, evnt.Key, "request", "HttpRequestMessage");
                                DependencyCollectorEventSource.Log.HttpCoreDiagnosticSourceListenerOnNextFailed(error);
                            }
                            else if (!Guid.TryParse(loggingRequestIdString, out Guid loggingRequestId))
                            {
                                var error = string.Format(CultureInfo.InvariantCulture, errorTemplateValueParse, evnt.Key, loggingRequestIdString, "Guid");
                                DependencyCollectorEventSource.Log.HttpCoreDiagnosticSourceListenerOnNextFailed(error);
                            }
                            else
                            {
                                this.OnRequest(request, loggingRequestId);
                            }

                            break;
                        }

                    case DeprecatedResponseEventName:
                        {
                            if (this.httpInstrumentationVersion != HttpInstrumentationVersion.V1)
                            {
                                // 2.0+ publishes new events, and this should be just ignored to prevent duplicates.
                                break;
                            }

                            var response = this.deprecatedResponseFetcher.Fetch(evnt.Value) as HttpResponseMessage;
                            var loggingRequestIdString = this.deprecatedResponseGuidFetcher.Fetch(evnt.Value).ToString();

                            if (response == null)
                            {
                                var error = string.Format(CultureInfo.InvariantCulture, errorTemplateTypeCast, evnt.Key, "response", "HttpResponseMessage");
                                DependencyCollectorEventSource.Log.HttpCoreDiagnosticSourceListenerOnNextFailed(error);
                            }
                            else if (!Guid.TryParse(loggingRequestIdString, out Guid loggingRequestId))
                            {
                                var error = string.Format(CultureInfo.InvariantCulture, errorTemplateValueParse, evnt.Key, loggingRequestIdString, "Guid");
                                DependencyCollectorEventSource.Log.HttpCoreDiagnosticSourceListenerOnNextFailed(error);
                            }
                            else
                            {
                                this.OnResponse(response, loggingRequestId);
                            }

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                DependencyCollectorEventSource.Log.HttpCoreDiagnosticSourceListenerOnNextFailed(ExceptionUtilities.GetExceptionDetailString(ex));
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        //// netcoreapp 2.0 event

        /// <summary>
        /// Handler for Activity start event (outgoing request is about to be sent).
        /// </summary>
        internal void OnActivityStart(HttpRequestMessage request)
        {
            // Even though we have the IsEnabled filter to reject ApplicationInsights URLs before any events are fired, if there
            // are multiple subscribers and one subscriber returns true to IsEnabled then all subscribers will receive the event.
            if (this.applicationInsightsUrlFilter.IsApplicationInsightsUrl(request.RequestUri))
            {
                return;
            }

            var currentActivity = Activity.Current;
            if (currentActivity == null)
            {
                DependencyCollectorEventSource.Log.CurrentActivityIsNull(HttpOutStartEventName);
                return;
            }

            if (request.Headers.Contains(W3C.W3CConstants.TraceParentHeader) && Activity.DefaultIdFormat == ActivityIdFormat.W3C)
            {
                DependencyCollectorEventSource.Log.HttpRequestAlreadyInstrumented(currentActivity.Id);
                return;
            }

            DependencyCollectorEventSource.Log.HttpCoreDiagnosticSourceListenerStart(currentActivity.Id);

            this.InjectRequestHeaders(request, this.configuration.InstrumentationKey);
        }

        //// netcoreapp 2.0 event

        /// <summary>
        /// Handler for Activity stop event (response is received for the outgoing request).
        /// </summary>
        internal void OnActivityStop(HttpResponseMessage response, HttpRequestMessage request, TaskStatus requestTaskStatus)
        {
            // Even though we have the IsEnabled filter to reject ApplicationInsights URLs before any events are fired, if there
            // are multiple subscribers and one subscriber returns true to IsEnabled then all subscribers will receive the event.
            if (this.applicationInsightsUrlFilter.IsApplicationInsightsUrl(request.RequestUri))
            {
                return;
            }

            Activity currentActivity = Activity.Current;
            if (currentActivity == null)
            {
                DependencyCollectorEventSource.Log.CurrentActivityIsNull(HttpOutStopEventName);
                return;
            }

            if (Activity.DefaultIdFormat == ActivityIdFormat.W3C &&
                request.Headers.TryGetValues(W3C.W3CConstants.TraceParentHeader, out var parents) && 
                parents.FirstOrDefault() != currentActivity.Id)
            {
                DependencyCollectorEventSource.Log.HttpRequestAlreadyInstrumented();
                return;
            }

            DependencyCollectorEventSource.Log.HttpCoreDiagnosticSourceListenerStop(currentActivity.Id);

            Uri requestUri = request.RequestUri;
            var resourceName = request.Method.Method + " " + requestUri.AbsolutePath;

            DependencyTelemetry telemetry = new DependencyTelemetry();
            telemetry.SetOperationDetail(OperationDetailConstants.HttpRequestOperationDetailName, request);

            // properly fill dependency telemetry operation context: OperationCorrelationTelemetryInitializer initializes child telemetry
            if (currentActivity.IdFormat == ActivityIdFormat.W3C)
            {
                var traceId = currentActivity.TraceId.ToHexString();
                telemetry.Context.Operation.Id = traceId;
                if (currentActivity.ParentSpanId != default)
                {
                    telemetry.Context.Operation.ParentId = currentActivity.ParentSpanId.ToHexString();
                }

                telemetry.Id = currentActivity.SpanId.ToHexString();
            }
            else
            {
                telemetry.Context.Operation.Id = currentActivity.RootId;
                telemetry.Context.Operation.ParentId = currentActivity.ParentId;
                telemetry.Id = currentActivity.Id;
            }

            foreach (var item in currentActivity.Baggage)
            {
                if (!telemetry.Properties.ContainsKey(item.Key))
                {
                    telemetry.Properties[item.Key] = item.Value;
                }
            }

            this.client.Initialize(telemetry);

            // If we started auxiliary Activity before to override the Id with W3C compatible one,
            // now it's time to set end time on it
            if (currentActivity.Duration == TimeSpan.Zero)
            {
                currentActivity.SetEndTime(DateTime.UtcNow);
            }

            telemetry.Timestamp = currentActivity.StartTimeUtc;
            telemetry.Name = resourceName;
            telemetry.Target = DependencyTargetNameHelper.GetDependencyTargetName(requestUri);
            telemetry.Type = RemoteDependencyConstants.HTTP;
            telemetry.Data = requestUri.OriginalString;
            telemetry.Duration = currentActivity.Duration;
            if (response != null)
            {
                this.ParseResponse(response, telemetry);
                telemetry.SetOperationDetail(OperationDetailConstants.HttpResponseOperationDetailName, response);
            }
            else
            {
                if (this.pendingExceptions.TryRemove(currentActivity.Id, out Exception exception))
                {
                    telemetry.Properties[RemoteDependencyConstants.DependencyErrorPropertyKey] = exception.GetBaseException().Message;
                }

                telemetry.ResultCode = requestTaskStatus.ToString();
                telemetry.Success = false;
            }

            this.client.TrackDependency(telemetry);
        }

        //// netcoreapp1.1 and prior event. See https://github.com/dotnet/corefx/blob/release/1.0.0-rc2/src/Common/src/System/Net/Http/HttpHandlerDiagnosticListenerExtensions.cs.

        /// <summary>
        /// Diagnostic event handler method for 'System.Net.Http.Request' event.
        /// </summary>
        internal void OnRequest(HttpRequestMessage request, Guid loggingRequestId)
        {
            if (request != null && request.RequestUri != null &&
                !this.applicationInsightsUrlFilter.IsApplicationInsightsUrl(request.RequestUri))
            {
                DependencyCollectorEventSource.Log.HttpCoreDiagnosticSourceListenerRequest(loggingRequestId);

                Uri requestUri = request.RequestUri;
                var resourceName = request.Method.Method + " " + requestUri.AbsolutePath;

                var dependency = this.client.StartOperation<DependencyTelemetry>(resourceName);

                dependency.Telemetry.Target = DependencyTargetNameHelper.GetDependencyTargetName(requestUri);
                dependency.Telemetry.Type = RemoteDependencyConstants.HTTP;
                dependency.Telemetry.Data = requestUri.OriginalString;
                dependency.Telemetry.SetOperationDetail(OperationDetailConstants.HttpRequestOperationDetailName, request);
                this.PendingDependencyTelemetry.AddIfNotExists(request, dependency);

                this.InjectRequestHeaders(request, dependency.Telemetry.Context.InstrumentationKey);
            }
        }

        //// netcoreapp1.1 and prior event. See https://github.com/dotnet/corefx/blob/release/1.0.0-rc2/src/Common/src/System/Net/Http/HttpHandlerDiagnosticListenerExtensions.cs.

        /// <summary>
        /// Diagnostic event handler method for 'System.Net.Http.Response' event.
        /// This event will be fired only if response was received (and not called for faulted or canceled requests).
        /// </summary>
        internal void OnResponse(HttpResponseMessage response, Guid loggingRequestId)
        {
            if (response != null)
            {
                DependencyCollectorEventSource.Log.HttpCoreDiagnosticSourceListenerResponse(loggingRequestId);
                var request = response.RequestMessage;

                if (this.PendingDependencyTelemetry.TryGetValue(request, out IOperationHolder<DependencyTelemetry> dependency))
                {
                    dependency.Telemetry.SetOperationDetail(OperationDetailConstants.HttpResponseOperationDetailName, response);
                    if (request != null)
                    {
                        this.ParseResponse(response, dependency.Telemetry);
                        this.client.StopOperation(dependency);
                        this.PendingDependencyTelemetry.Remove(request);
                    }
                }
            }
        }

        private static void InjectCorrelationContext(HttpRequestHeaders requestHeaders, Activity currentActivity)
        {
            if (!requestHeaders.Contains(RequestResponseHeaders.CorrelationContextHeader))
            {
                // we expect baggage to be empty or contain a few items
                using (IEnumerator<KeyValuePair<string, string>> e = currentActivity.Baggage.GetEnumerator())
                {
                    if (e.MoveNext())
                    {
                        var baggage = new List<string>();
                        do
                        {
                            KeyValuePair<string, string> item = e.Current;
                            baggage.Add(new NameValueHeaderValue(item.Key, item.Value).ToString());
                        }
                        while (e.MoveNext());

                        requestHeaders.Add(RequestResponseHeaders.CorrelationContextHeader, baggage);
                    }
                }
            }
        }

        private static void InjectW3CHeaders(Activity currentActivity, HttpRequestHeaders requestHeaders)
        {
            if (!requestHeaders.Contains(W3C.W3CConstants.TraceParentHeader))
            {
                requestHeaders.Add(W3C.W3CConstants.TraceParentHeader, currentActivity.Id);
            }

            if (!requestHeaders.Contains(W3C.W3CConstants.TraceStateHeader) &&
                currentActivity.TraceStateString != null)
            {
                requestHeaders.Add(W3C.W3CConstants.TraceStateHeader,
                    currentActivity.TraceStateString);
            }
        }

        private static void InjectBackCompatibleRequestId(Activity currentActivity, HttpRequestHeaders requestHeaders)
        {
            if (!requestHeaders.Contains(RequestResponseHeaders.RequestIdHeader))
            {
                requestHeaders.Add(RequestResponseHeaders.RequestIdHeader, string.Concat('|', currentActivity.TraceId.ToHexString(), '.', currentActivity.SpanId.ToHexString(), '.'));
            }
        }

        private static HttpInstrumentationVersion GetInstrumentationVersion()
        {
            HttpInstrumentationVersion version = HttpInstrumentationVersion.Unknown;

            var httpClientAssembly = typeof(HttpClient).GetTypeInfo().Assembly;
            var httpClientVersion = httpClientAssembly.GetName().Version;
            string httpClientInformationalVersion =
                httpClientAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ??
                string.Empty;

            if (httpClientInformationalVersion.StartsWith("3.", StringComparison.Ordinal))
            {
                version = HttpInstrumentationVersion.V3;
            }
            else if (httpClientVersion.Major == 4 && httpClientVersion.Minor == 2)
            {
                // .NET Core 3.0 has the same version of http client lib as 2.*
                // but AssemblyInformationalVersionAttribute is different.
                version = HttpInstrumentationVersion.V2;
            }
            else if (httpClientVersion.Major == 4 && httpClientVersion.Minor < 2)
            {
                version = HttpInstrumentationVersion.V1;
            }
            else
            {
                // fallback to V3 assuming unknown SDKs are from future versions
                version = HttpInstrumentationVersion.V3;
            }

            DependencyCollectorEventSource.Log.HttpCoreDiagnosticListenerInstrumentationVersion(
                (int)version,
                httpClientVersion.Major,
                httpClientVersion.Minor,
                httpClientInformationalVersion);

            return version;
        }

        private void InjectRequestHeaders(HttpRequestMessage request, string instrumentationKey)
        {
            try
            {
                HttpRequestHeaders requestHeaders = request.Headers;
                if (requestHeaders != null && this.setComponentCorrelationHttpHeaders && !this.correlationDomainExclusionList.Contains(request.RequestUri.Host))
                {
                    string sourceApplicationId = null;
                    try
                    {
                        if (!string.IsNullOrEmpty(instrumentationKey)
                            && !HttpHeadersUtilities.ContainsRequestContextKeyValue(requestHeaders, RequestResponseHeaders.RequestContextCorrelationSourceKey)
                            && (this.configuration.ApplicationIdProvider?.TryGetApplicationId(instrumentationKey, out sourceApplicationId) ?? false))
                        {
                            HttpHeadersUtilities.SetRequestContextKeyValue(requestHeaders, RequestResponseHeaders.RequestContextCorrelationSourceKey, sourceApplicationId);
                        }
                    }
                    catch (Exception e)
                    {
                        AppMapCorrelationEventSource.Log.UnknownError(ExceptionUtilities.GetExceptionDetailString(e));
                    }

                    var currentActivity = Activity.Current;

                    switch (this.httpInstrumentationVersion)
                    {
                        case HttpInstrumentationVersion.V1:
                            // HttpClient does not add any headers
                            // add W3C or Request-Id depending on Activity format
                            // add correlation-context anyway
                            if (currentActivity.IdFormat == ActivityIdFormat.W3C)
                            {
                                InjectW3CHeaders(currentActivity, requestHeaders);
                                if (this.injectRequestIdInW3CMode)
                                {
                                    InjectBackCompatibleRequestId(currentActivity, requestHeaders);
                                }
                            }
                            else
                            {
                                if (!requestHeaders.Contains(RequestResponseHeaders.RequestIdHeader))
                                {
                                    requestHeaders.Add(RequestResponseHeaders.RequestIdHeader, currentActivity.Id);
                                }
                            }

                            InjectCorrelationContext(requestHeaders, currentActivity);
                            break;
                        case HttpInstrumentationVersion.V2:
                            // On V2, HttpClient adds Request-Id and Correlation-Context
                            // but not W3C
                            if (currentActivity.IdFormat == ActivityIdFormat.W3C)
                            {
                                // we are going to add W3C and Request-Id (in W3C-compatible format)
                                // as a result HttpClient will not add Request-Id AND Correlation-Context
                                InjectW3CHeaders(currentActivity, requestHeaders);
                                if (this.injectRequestIdInW3CMode)
                                {
                                    InjectBackCompatibleRequestId(currentActivity, requestHeaders);
                                }

                                InjectCorrelationContext(requestHeaders, currentActivity);
                            }

                            break;
                        case HttpInstrumentationVersion.V3:
                            // on V3, HttpClient adds either W3C or Request-Id depending on Activity format
                            // and adds Correlation-Context
                            if (currentActivity.IdFormat == ActivityIdFormat.W3C && this.injectRequestIdInW3CMode)
                            {
                                // we are going to override Request-Id to be in W3C compatible mode
                                InjectBackCompatibleRequestId(currentActivity, requestHeaders);
                            }

                            break;
                    }

                    if (this.injectLegacyHeaders)
                    {
                        // Add the root ID (Activity.RootId works with W3C and Hierarchical format)
                        string rootId = currentActivity.RootId;
                        if (!string.IsNullOrEmpty(rootId) && !requestHeaders.Contains(RequestResponseHeaders.StandardRootIdHeader))
                        {
                            requestHeaders.Add(RequestResponseHeaders.StandardRootIdHeader, rootId);
                        }

                        // Add the parent ID
                        string parentId = currentActivity.IdFormat == ActivityIdFormat.W3C ? 
                            currentActivity.SpanId.ToHexString() :
                            currentActivity.Id;

                        if (!string.IsNullOrEmpty(parentId) && !requestHeaders.Contains(RequestResponseHeaders.StandardParentIdHeader))
                        {
                            requestHeaders.Add(RequestResponseHeaders.StandardParentIdHeader, parentId);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                AppMapCorrelationEventSource.Log.UnknownError(ExceptionUtilities.GetExceptionDetailString(e));
            }
        }

        private void ParseResponse(HttpResponseMessage response, DependencyTelemetry telemetry)
        {
            try
            {
                string targetApplicationId = HttpHeadersUtilities.GetRequestContextKeyValue(response.Headers, RequestResponseHeaders.RequestContextCorrelationTargetKey);
                if (!string.IsNullOrEmpty(targetApplicationId) && !string.IsNullOrEmpty(telemetry.Context.InstrumentationKey))
                {
                    // We only add the cross component correlation key if the key does not represent the current component.
                    string sourceApplicationId = null;
                    if (this.configuration.ApplicationIdProvider?.TryGetApplicationId(telemetry.Context.InstrumentationKey, out sourceApplicationId) ?? false)
                    {
                        if (targetApplicationId != sourceApplicationId)
                        {
                            telemetry.Type = RemoteDependencyConstants.AI;
                            telemetry.Target += " | " + targetApplicationId;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                AppMapCorrelationEventSource.Log.UnknownError(ExceptionUtilities.GetExceptionDetailString(e));
            }

            int statusCode = (int)response.StatusCode;
            telemetry.ResultCode = (statusCode > 0) ? statusCode.ToString(CultureInfo.InvariantCulture) : string.Empty;
            telemetry.Success = (statusCode > 0) && (statusCode < 400);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.subscriber != null)
                {
                    this.subscriber.Dispose();
                }
            }
        }

        /// <summary>
        /// Diagnostic listener implementation that listens for events specific to outgoing dependency requests.
        /// </summary>
        private class HttpCoreDiagnosticSourceSubscriber : IObserver<DiagnosticListener>, IDisposable
        {
            private readonly HttpCoreDiagnosticSourceListener httpDiagnosticListener;
            private readonly IDisposable listenerSubscription;
            private readonly ApplicationInsightsUrlFilter applicationInsightsUrlFilter;
            private readonly HttpInstrumentationVersion httpInstrumentationVersion;

            private IDisposable eventSubscription;

            internal HttpCoreDiagnosticSourceSubscriber(
                HttpCoreDiagnosticSourceListener listener,
                ApplicationInsightsUrlFilter applicationInsightsUrlFilter,
                HttpInstrumentationVersion httpInstrumentationVersion)
            {
                this.httpDiagnosticListener = listener;
                this.applicationInsightsUrlFilter = applicationInsightsUrlFilter;

                this.httpInstrumentationVersion = httpInstrumentationVersion;

                try
                {
                    this.listenerSubscription = DiagnosticListener.AllListeners.Subscribe(this);
                }
                catch (Exception ex)
                {
                    DependencyCollectorEventSource.Log.HttpCoreDiagnosticSubscriberFailedToSubscribe(ex.ToInvariantString());
                }

                SubscriptionManager.Attach(this.httpDiagnosticListener);
            }

            /// <summary>
            /// This method gets called once for each existing DiagnosticListener when this
            /// DiagnosticListener is added to the list of DiagnosticListeners
            /// (<see cref="System.Diagnostics.DiagnosticListener.AllListeners"/>). This method will
            /// also be called for each subsequent DiagnosticListener that is added to the list of
            /// DiagnosticListeners.
            /// <seealso cref="IObserver{T}.OnNext(T)"/>
            /// </summary>
            /// <param name="value">The DiagnosticListener that exists when this listener was added to
            /// the list, or a DiagnosticListener that got added after this listener was added.</param>
            public void OnNext(DiagnosticListener value)
            {
                if (value != null)
                {
                    // Comes from https://github.com/dotnet/corefx/blob/master/src/System.Net.Http/src/System/Net/Http/DiagnosticsHandlerLoggingStrings.cs#L12
                    if (value.Name == "HttpHandlerDiagnosticListener")
                    {
                        this.eventSubscription = value.Subscribe(
                            this.httpDiagnosticListener,
                            (evnt, r, _) =>
                            {
                                if (this.httpInstrumentationVersion != HttpInstrumentationVersion.V1)
                                {
                                    if (evnt == HttpExceptionEventName)
                                    {
                                        return false;
                                    }

                                    if (!evnt.StartsWith(HttpOutEventName, StringComparison.Ordinal))
                                    {
                                        return false;
                                    }

                                    if (evnt == HttpOutEventName && r != null)
                                    {
                                        var request = (HttpRequestMessage)r;
                                        return !this.applicationInsightsUrlFilter.IsApplicationInsightsUrl(request.RequestUri);
                                    }
                                }

                                return true;
                            });
                    }
                }
            }

            /// <summary>
            /// Notifies the observer that the provider has finished sending push-based notifications.
            /// <seealso cref="IObserver{T}.OnCompleted()"/>
            /// </summary>
            public void OnCompleted()
            {
            }

            /// <summary>
            /// Notifies the observer that the provider has experienced an error condition.
            /// <seealso cref="IObserver{T}.OnError(Exception)"/>
            /// </summary>
            /// <param name="error">An object that provides additional information about the error.</param>
            public void OnError(Exception error)
            {
            }

            public void Dispose()
            {
                SubscriptionManager.Detach(this.httpDiagnosticListener);
                this.eventSubscription?.Dispose();
                this.listenerSubscription?.Dispose();
            }
        }
    }
}