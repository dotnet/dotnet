namespace Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners.Implementation;
    using Microsoft.ApplicationInsights.AspNetCore.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.ApplicationInsights.AspNetCore.Implementation;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Experimental;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Extensibility.W3C;
    using Microsoft.ApplicationInsights.W3C;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// <see cref="IApplicationInsightDiagnosticListener"/> implementation that listens for events specific to AspNetCore hosting layer.
    /// </summary>
    internal class HostingDiagnosticListener : IApplicationInsightDiagnosticListener
    {
        /// <summary>
        /// Name of custom property to store the legacy RootId when operating in W3C mode. Backend/UI understands this property.
        /// </summary>
        internal const string LegacyRootIdProperty = "ai_legacyRootId";

        private const string ActivityCreatedByHostingDiagnosticListener = "ActivityCreatedByHostingDiagnosticListener";
        private const string ProactiveSamplingFeatureFlagName = "proactiveSampling";
        private const string ConditionalAppIdFeatureFlagName = "conditionalAppId";

        private static readonly ActiveSubsciptionManager SubscriptionManager = new ActiveSubsciptionManager();

        /// <summary>
        /// This class need to be aware of the AspNetCore major version.
        /// This will affect what DiagnosticSource events we receive.
        /// To support AspNetCore 1.0,2.0,3.0 we listen to both old and new events.
        /// If the running AspNetCore version is 2.0 or 3.0, both old and new events will be sent. In this case, we will ignore the old events.
        /// Also 3.0 is W3C Tracing Aware (i.e it populates Activity from traceparent headers) and hence SDK need to be aware.
        /// </summary>
        private readonly AspNetCoreMajorVersion aspNetCoreMajorVersion;

        private readonly bool proactiveSamplingEnabled = false;
        private readonly bool conditionalAppIdEnabled = false;

        private readonly TelemetryConfiguration configuration;
        private readonly TelemetryClient client;
        private readonly IApplicationIdProvider applicationIdProvider;
        private readonly string sdkVersion = SdkVersionUtils.GetVersion();
        private readonly bool injectResponseHeaders;
        private readonly bool trackExceptions;
        private readonly bool enableW3CHeaders;

        // fetch is unique per event and per property
        private readonly PropertyFetcher httpContextFetcherOnBeforeAction = new PropertyFetcher("httpContext");
        private readonly PropertyFetcher httpContextFetcherOnBeforeAction30 = new PropertyFetcher("HttpContext");
        private readonly PropertyFetcher routeDataFetcher = new PropertyFetcher("routeData");
        private readonly PropertyFetcher routeDataFetcher30 = new PropertyFetcher("RouteData");
        private readonly PropertyFetcher routeValuesFetcher = new PropertyFetcher("Values");
        private readonly PropertyFetcher httpContextFetcherStart = new PropertyFetcher("HttpContext");
        private readonly PropertyFetcher httpContextFetcherStop = new PropertyFetcher("HttpContext");
        private readonly PropertyFetcher httpContextFetcherDiagExceptionUnhandled = new PropertyFetcher("httpContext");
        private readonly PropertyFetcher httpContextFetcherDiagExceptionHandled = new PropertyFetcher("httpContext");
        private readonly PropertyFetcher exceptionFetcherDiagExceptionUnhandled = new PropertyFetcher("exception");
        private readonly PropertyFetcher exceptionFetcherDiagExceptionHandled = new PropertyFetcher("exception");

        private string lastIKeyLookedUp;
        private string lastAppIdUsed;

        /// <summary>
        /// Initializes a new instance of the <see cref="HostingDiagnosticListener"/> class.
        /// </summary>
        /// <param name="client"><see cref="TelemetryClient"/> to post traces to.</param>
        /// <param name="applicationIdProvider">Provider for resolving application Id to be used in multiple instruemntation keys scenarios.</param>
        /// <param name="injectResponseHeaders">Flag that indicates that response headers should be injected.</param>
        /// <param name="trackExceptions">Flag that indicates that exceptions should be tracked.</param>
        /// <param name="enableW3CHeaders">Flag that indicates that W3C header parsing should be enabled.</param>
        /// <param name="aspNetCoreMajorVersion">Major version of AspNetCore.</param>
        public HostingDiagnosticListener(
            TelemetryClient client,
            IApplicationIdProvider applicationIdProvider,
            bool injectResponseHeaders,
            bool trackExceptions,
            bool enableW3CHeaders,
            AspNetCoreMajorVersion aspNetCoreMajorVersion)
        {
            this.aspNetCoreMajorVersion = aspNetCoreMajorVersion;
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.applicationIdProvider = applicationIdProvider;
            this.injectResponseHeaders = injectResponseHeaders;
            this.trackExceptions = trackExceptions;
            this.enableW3CHeaders = enableW3CHeaders;
            AspNetCoreEventSource.Instance.HostingListenerInformational(this.aspNetCoreMajorVersion, "HostingDiagnosticListener constructed.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HostingDiagnosticListener"/> class.
        /// </summary>
        /// <param name="configuration"><see cref="TelemetryConfiguration"/> as a settings source.</param>
        /// <param name="client"><see cref="TelemetryClient"/> to post traces to.</param>
        /// <param name="applicationIdProvider">Provider for resolving application Id to be used in multiple instruemntation keys scenarios.</param>
        /// <param name="injectResponseHeaders">Flag that indicates that response headers should be injected.</param>
        /// <param name="trackExceptions">Flag that indicates that exceptions should be tracked.</param>
        /// <param name="enableW3CHeaders">Flag that indicates that W3C header parsing should be enabled.</param>
        /// <param name="aspNetCoreMajorVersion">Major version of AspNetCore.</param>
        public HostingDiagnosticListener(
            TelemetryConfiguration configuration,
            TelemetryClient client,
            IApplicationIdProvider applicationIdProvider,
            bool injectResponseHeaders,
            bool trackExceptions,
            bool enableW3CHeaders,
            AspNetCoreMajorVersion aspNetCoreMajorVersion)
            : this(client, applicationIdProvider, injectResponseHeaders, trackExceptions, enableW3CHeaders, aspNetCoreMajorVersion)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.proactiveSamplingEnabled = this.configuration.EvaluateExperimentalFeature(ProactiveSamplingFeatureFlagName);
            this.conditionalAppIdEnabled = this.configuration.EvaluateExperimentalFeature(ConditionalAppIdFeatureFlagName);
        }

        /// <inheritdoc/>
        public string ListenerName { get; } = "Microsoft.AspNetCore";

        /// <summary>
        /// Diagnostic event handler method for 'Microsoft.AspNetCore.Mvc.BeforeAction' event.
        /// </summary>
        /// <param name="httpContext">HttpContext is used to retrieve information about the Request and Response.</param>
        /// <param name="routeValues">Used to get the name of the request.</param>
        public static void OnBeforeAction(HttpContext httpContext, IDictionary<string, object> routeValues)
        {
            var telemetry = httpContext.Features.Get<RequestTelemetry>();

            if (telemetry != null && string.IsNullOrEmpty(telemetry.Name))
            {
                string name = GetNameFromRouteContext(routeValues);
                if (!string.IsNullOrEmpty(name))
                {
                    name = httpContext.Request.Method + " " + name;
                    telemetry.Name = name;
                }
            }
        }

        /// <inheritdoc />
        public void OnSubscribe()
        {
            SubscriptionManager.Attach(this);
        }

        /// <summary>
        /// Diagnostic event handler method for 'Microsoft.AspNetCore.Hosting.HttpRequestIn.Start' event.
        /// This is from 2.XX and higher runtime.
        /// </summary>
        /// <param name="httpContext">HttpContext is used to retrieve information about the Request and Response.</param>
        public void OnHttpRequestInStart(HttpContext httpContext)
        {
            if (this.client.IsEnabled())
            {
                // It's possible to host multiple apps (ASP.NET Core or generic hosts) in the same process
                // Each of this apps has it's own HostingDiagnosticListener and corresponding Http listener.
                // We should ignore events for all of them except one
                if (!SubscriptionManager.IsActive(this))
                {
                    AspNetCoreEventSource.Instance.NotActiveListenerNoTracking("Microsoft.AspNetCore.Hosting.HttpRequestIn.Start", Activity.Current?.Id);
                    return;
                }

                if (Activity.Current == null)
                {
                    AspNetCoreEventSource.Instance.LogHostingDiagnosticListenerOnHttpRequestInStartActivityNull();
                    return;
                }

                var currentActivity = Activity.Current;
                Activity newActivity = null;
                string originalParentId = currentActivity.ParentId;
                string legacyRootId = null;
                bool traceParentPresent = false;
                var headers = httpContext.Request.Headers;

                // Update the static RoleName while we have access to the httpContext.
                RoleNameContainer.Instance?.Set(headers);

                // 3 possibilities when TelemetryConfiguration.EnableW3CCorrelation = true
                // 1. No incoming headers. originalParentId will be null. Simply use the Activity as such.
                // 2. Incoming Request-ID Headers. originalParentId will be request-id, but Activity ignores this for ID calculations.
                //    If incoming ID is W3C compatible, ignore current Activity. Create new one with parent set to incoming W3C compatible rootid.
                //    If incoming ID is not W3C compatible, we can use Activity as such, but need to store originalParentID in custom property 'legacyRootId'
                // 3. Incoming TraceParent header.
                //    3a - 2.XX Need to ignore current Activity, and create new from incoming W3C TraceParent header.
                //    3b - 3.XX Use Activity as such because 3.XX is W3C Aware.

                // Another 3 possibilities when TelemetryConfiguration.EnableW3CCorrelation = false
                // 1. No incoming headers. originalParentId will be null. Simply use the Activity as such.
                // 2. Incoming Request-ID Headers. originalParentId will be request-id, Activity uses this for ID calculations.
                // 3. Incoming TraceParent header. Will simply Ignore W3C headers, and Current Activity used as such.

                // Attempt to find parent from incoming W3C Headers which 2.XX Hosting is unaware of.
                if (this.aspNetCoreMajorVersion != AspNetCoreMajorVersion.Three
                     && currentActivity.IdFormat == ActivityIdFormat.W3C
                     && headers.TryGetValue(W3CConstants.TraceParentHeader, out StringValues traceParentValues)
                     && traceParentValues != StringValues.Empty)
                {
                    var parentTraceParent = StringUtilities.EnforceMaxLength(
                        traceParentValues.First(),
                        InjectionGuardConstants.TraceParentHeaderMaxLength);
                    originalParentId = parentTraceParent;
                    traceParentPresent = true;
                    AspNetCoreEventSource.Instance.HostingListenerInformational(this.aspNetCoreMajorVersion, "Retrieved trace parent from headers.");
                }

                // Scenario #1. No incoming correlation headers.
                if (originalParentId == null)
                {
                    // Nothing to do here.
                    AspNetCoreEventSource.Instance.HostingListenerInformational(this.aspNetCoreMajorVersion, "OriginalParentId is null.");
                }
                else if (traceParentPresent)
                {
                    // Scenario #3. W3C-TraceParent
                    // We need to ignore the Activity created by Hosting, as it did not take W3CTraceParent into consideration.
#pragma warning disable CA2000 // Dispose objects before losing scope
                    // Even though we lose activity scope here, its retrieved using Activity.Current in end call back, and disposed/ended there
                    newActivity = new Activity(ActivityCreatedByHostingDiagnosticListener);
#pragma warning restore CA2000 // Dispose objects before losing scope
                    CopyActivityPropertiesFromAspNetCore(currentActivity, newActivity);

                    newActivity.SetParentId(originalParentId);
                    AspNetCoreEventSource.Instance.HostingListenerInformational(this.aspNetCoreMajorVersion, "Ignoring original Activity from Hosting to create new one using traceparent header retrieved by sdk.");

                    // read and populate tracestate
                    ReadTraceState(httpContext.Request.Headers, newActivity);
                }
                else if (this.aspNetCoreMajorVersion == AspNetCoreMajorVersion.Three && headers.ContainsKey(W3CConstants.TraceParentHeader))
                {
                    AspNetCoreEventSource.Instance.HostingListenerInformational(this.aspNetCoreMajorVersion, "Incoming request has traceparent. Using Activity created from Hosting.");

                    // scenario #3b Use Activity created by Hosting layer when W3C Headers Present.
                    // but ignore parent if user disabled w3c.
                    if (currentActivity.IdFormat != ActivityIdFormat.W3C)
                    {
                        originalParentId = null;
                    }
                }
                else
                {
                    // Scenario #2. RequestID
                    if (currentActivity.IdFormat == ActivityIdFormat.W3C)
                    {
                        if (TryGetW3CCompatibleTraceId(originalParentId, out var traceId))
                        {
#pragma warning disable CA2000 // Dispose objects before losing scope
                            // Even though we lose activity scope here, its retrieved using Activity.Current in end call back, and disposed/ended there
                            newActivity = new Activity(ActivityCreatedByHostingDiagnosticListener);
#pragma warning restore CA2000 // Dispose objects before losing scope
                            CopyActivityPropertiesFromAspNetCore(currentActivity, newActivity);
                            newActivity.SetParentId(ActivityTraceId.CreateFromString(traceId), default(ActivitySpanId), ActivityTraceFlags.None);
                            AspNetCoreEventSource.Instance.HostingListenerInformational(this.aspNetCoreMajorVersion, "Ignoring original Activity from Hosting to create new one using w3c compatible request-id.");
                        }
                        else
                        {
                            // store rootIdFromOriginalParentId in custom Property
                            legacyRootId = ExtractOperationIdFromRequestId(originalParentId);
                            AspNetCoreEventSource.Instance.HostingListenerInformational(this.aspNetCoreMajorVersion, "Incoming Request-ID is not W3C Compatible, and hence will be ignored for ID generation, but stored in custom property legacy_rootID.");
                        }
                    }
                }

                if (newActivity != null)
                {
                    newActivity.Start();
                    currentActivity = newActivity;
                }

                // Read Correlation-Context is all scenarios irrespective of presence of either request-id or traceparent headers.
                ReadCorrelationContext(httpContext.Request.Headers, currentActivity);

                var requestTelemetry = this.InitializeRequestTelemetry(httpContext, currentActivity, Stopwatch.GetTimestamp(), legacyRootId);

                requestTelemetry.Context.Operation.ParentId = GetParentId(currentActivity, originalParentId);

                this.AddAppIdToResponseIfRequired(httpContext, requestTelemetry);
            }
        }

        /// <summary>
        /// Diagnostic event handler method for 'Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop' event.
        /// This is from 2.XX and higher runtime.
        /// </summary>
        /// <param name="httpContext">HttpContext is used to retrieve information about the Request and Response.</param>
        public void OnHttpRequestInStop(HttpContext httpContext)
        {
            this.EndRequest(httpContext, Stopwatch.GetTimestamp());
        }

        /// <summary>
        /// Diagnostic event handler method for 'Microsoft.AspNetCore.Diagnostics.HandledException' event.
        /// </summary>
        /// <param name="httpContext">HttpContext is used to retrieve information about the Request and Response.</param>
        /// <param name="exception">Used to create exception telemetry.</param>
        public void OnDiagnosticsHandledException(HttpContext httpContext, Exception exception)
        {
            this.OnException(httpContext, exception);
        }

        /// <summary>
        /// Diagnostic event handler method for 'Microsoft.AspNetCore.Diagnostics.UnhandledException' event.
        /// </summary>
        /// <param name="httpContext">HttpContext is used to retrieve information about the Request and Response.</param>
        /// <param name="exception">Used to create exception telemetry.</param>
        public void OnDiagnosticsUnhandledException(HttpContext httpContext, Exception exception)
        {
            this.OnException(httpContext, exception);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            SubscriptionManager.Detach(this);
        }

        /// <inheritdoc />
        public void OnNext(KeyValuePair<string, object> value)
        {
            HttpContext httpContext = null;
            Exception exception = null;

            try
            {
                //// Top messages in if-else are the most often used messages.
                //// Switch is compiled into GetHashCode() and binary search, if-else without GetHashCode()
                //// is faster if 2.0 or higher events are used.
                if (value.Key == "Microsoft.AspNetCore.Hosting.HttpRequestIn.Start")
                {
                    httpContext = this.httpContextFetcherStart.Fetch(value.Value) as HttpContext;
                    if (httpContext != null)
                    {
                        this.OnHttpRequestInStart(httpContext);
                    }
                }
                else if (value.Key == "Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop")
                {
                    httpContext = this.httpContextFetcherStop.Fetch(value.Value) as HttpContext;
                    if (httpContext != null)
                    {
                        this.OnHttpRequestInStop(httpContext);
                    }
                }
                else if (value.Key == "Microsoft.AspNetCore.Mvc.BeforeAction")
                {
                    HttpContext context = null;
                    object routeData = null;

                    // Asp.Net Core 3.0 changed the field name to "RouteData" from "routeData and "HttpContext" from "httpContext"
                    if (this.aspNetCoreMajorVersion == AspNetCoreMajorVersion.Three)
                    {
                        context = this.httpContextFetcherOnBeforeAction30.Fetch(value.Value) as HttpContext;
                        if (context == null)
                        {
                            context = this.httpContextFetcherOnBeforeAction.Fetch(value.Value) as HttpContext;
                        }

                        routeData = this.routeDataFetcher30.Fetch(value.Value);
                        if (routeData == null)
                        {
                            routeData = this.routeDataFetcher.Fetch(value.Value);
                        }
                    }
                    else
                    {
                        context = this.httpContextFetcherOnBeforeAction.Fetch(value.Value) as HttpContext;
                        if (context == null)
                        {
                            context = this.httpContextFetcherOnBeforeAction30.Fetch(value.Value) as HttpContext;
                        }

                        routeData = this.routeDataFetcher.Fetch(value.Value);
                        if (routeData == null)
                        {
                            routeData = this.routeDataFetcher30.Fetch(value.Value);
                        }
                    }

                    var routeValues = this.routeValuesFetcher.Fetch(routeData) as IDictionary<string, object>;

                    if (context != null && routeValues != null)
                    {
                        OnBeforeAction(context, routeValues);
                    }
                }
                else if (this.trackExceptions && value.Key == "Microsoft.AspNetCore.Diagnostics.UnhandledException")
                {
                    httpContext = this.httpContextFetcherDiagExceptionUnhandled.Fetch(value.Value) as HttpContext;
                    exception = this.exceptionFetcherDiagExceptionUnhandled.Fetch(value.Value) as Exception;
                    if (httpContext != null && exception != null)
                    {
                        this.OnDiagnosticsUnhandledException(httpContext, exception);
                    }
                }
                else if (this.trackExceptions && value.Key == "Microsoft.AspNetCore.Diagnostics.HandledException")
                {
                    httpContext = this.httpContextFetcherDiagExceptionHandled.Fetch(value.Value) as HttpContext;
                    exception = this.exceptionFetcherDiagExceptionHandled.Fetch(value.Value) as Exception;
                    if (httpContext != null && exception != null)
                    {
                        this.OnDiagnosticsHandledException(httpContext, exception);
                    }
                }
            }
            catch (Exception ex)
            {
                AspNetCoreEventSource.Instance.DiagnosticListenerWarning(value.Key, ex.ToInvariantString());
            }
        }

        /// <inheritdoc />
        public void OnError(Exception error)
        {
        }

        /// <inheritdoc />
        public void OnCompleted()
        {
        }

        private static string GetParentId(Activity activity, string originalParentId)
        {
            if (activity.IdFormat == ActivityIdFormat.W3C && activity.ParentSpanId != default)
            {
                var parentSpanId = activity.ParentSpanId.ToHexString();
                if (parentSpanId != "0000000000000000")
                {
                    return parentSpanId;
                }
            }

            return originalParentId;
        }

        private static void CopyActivityPropertiesFromAspNetCore(Activity currentActivity, Activity newActivity)
        {
            foreach (var tag in currentActivity.Tags)
            {
                newActivity.AddTag(tag.Key, tag.Value);
            }

            foreach (var baggage in currentActivity.Baggage)
            {
                newActivity.AddBaggage(baggage.Key, baggage.Value);
            }
        }

        private static string ExtractOperationIdFromRequestId(string originalParentId)
        {
            if (originalParentId[0] == '|')
            {
                int indexDot = originalParentId.IndexOf('.');
                if (indexDot > 1)
                {
                    return originalParentId.Substring(1, indexDot - 1);
                }
                else
                {
                    return originalParentId;
                }
            }
            else
            {
                return originalParentId;
            }
        }

        private static bool TryGetW3CCompatibleTraceId(string requestId, out ReadOnlySpan<char> result)
        {
            if (requestId[0] == '|')
            {
                if (requestId.Length > 33 && requestId[33] == '.')
                {
                    for (int i = 1; i < 33; i++)
                    {
                        if (!char.IsLetterOrDigit(requestId[i]))
                        {
                            result = null;
                            return false;
                        }
                    }

                    result = requestId.AsSpan().Slice(1, 32);
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }
            else
            {
                result = null;
                return false;
            }
        }

        private static void ReadCorrelationContext(IHeaderDictionary requestHeaders, Activity activity)
        {
            try
            {
                if (!activity.Baggage.Any())
                {
                    string[] baggage = requestHeaders.GetCommaSeparatedValues(RequestResponseHeaders.CorrelationContextHeader);
                    if (baggage != StringValues.Empty)
                    {
                        foreach (var item in baggage)
                        {
                            var parts = item.Split('=');
                            if (parts.Length == 2)
                            {
                                var itemName = StringUtilities.EnforceMaxLength(parts[0], InjectionGuardConstants.ContextHeaderKeyMaxLength);
                                var itemValue = StringUtilities.EnforceMaxLength(parts[1], InjectionGuardConstants.ContextHeaderValueMaxLength);
                                activity.AddBaggage(itemName.Trim(), itemValue.Trim());
                            }
                        }

                        AspNetCoreEventSource.Instance.HostingListenerVerbose("Correlation-Context retrived from header and stored into activity baggage.");
                    }
                }
            }
            catch (Exception ex)
            {
                AspNetCoreEventSource.Instance.HostingListenerWarning("CorrelationContext read failed.", ex.ToInvariantString());
            }
        }

        private static void ReadTraceState(IHeaderDictionary requestHeaders, Activity activity)
        {
            if (requestHeaders.TryGetValue(W3CConstants.TraceStateHeader, out var traceState))
            {
                // SDK is not relying on anything from tracestate.
                // It simply sets activity tracestate, so that outbound calls
                // make in the request context can continue propogation
                // of tracestate.
                activity.TraceStateString = traceState;
                AspNetCoreEventSource.Instance.HostingListenerVerbose("TraceState retrived from header and stored into activity.TraceState");
            }
        }

        private static string GetNameFromRouteContext(IDictionary<string, object> routeValues)
        {
            string name = null;

            if (routeValues.Count > 0)
            {
                object controller;
                routeValues.TryGetValue("controller", out controller);
                string controllerString = (controller == null) ? string.Empty : controller.ToString();

                if (!string.IsNullOrEmpty(controllerString))
                {
                    name = controllerString;

                    if (routeValues.TryGetValue("action", out var action) && action != null)
                    {
                        name += "/" + action.ToString();
                    }

                    if (routeValues.Keys.Count > 2)
                    {
                        // Add parameters
                        var sortedKeys = routeValues.Keys
                            .Where(key =>
                                !string.Equals(key, "controller", StringComparison.OrdinalIgnoreCase) &&
                                !string.Equals(key, "action", StringComparison.OrdinalIgnoreCase) &&
                                !string.Equals(key, "!__route_group", StringComparison.OrdinalIgnoreCase))
                            .OrderBy(key => key, StringComparer.OrdinalIgnoreCase)
                            .ToArray();

                        if (sortedKeys.Length > 0)
                        {
                            string arguments = string.Join(@"/", sortedKeys);
                            name += " [" + arguments + "]";
                        }
                    }
                }
                else
                {
                    object page;
                    routeValues.TryGetValue("page", out page);
                    string pageString = (page == null) ? string.Empty : page.ToString();
                    if (!string.IsNullOrEmpty(pageString))
                    {
                        name = pageString;
                    }
                }
            }

            return name;
        }

        private void AddAppIdToResponseIfRequired(HttpContext httpContext, RequestTelemetry requestTelemetry)
        {
            if (this.conditionalAppIdEnabled)
            {
                // Only reply back with AppId if we got an indication that we need to set one
                if (!string.IsNullOrWhiteSpace(requestTelemetry.Source))
                {
                    this.SetAppIdInResponseHeader(httpContext, requestTelemetry);
                }
            }
            else
            {
                this.SetAppIdInResponseHeader(httpContext, requestTelemetry);
            }
        }

        private RequestTelemetry InitializeRequestTelemetry(HttpContext httpContext, Activity activity, long timestamp, string legacyRootId = null)
        {
            var requestTelemetry = new RequestTelemetry();

            if (activity.IdFormat == ActivityIdFormat.W3C)
            {
                var traceId = activity.TraceId.ToHexString();
                requestTelemetry.Id = activity.SpanId.ToHexString();
                requestTelemetry.Context.Operation.Id = traceId;
                AspNetCoreEventSource.Instance.RequestTelemetryCreated("W3C", requestTelemetry.Id, traceId);
            }
            else
            {
                requestTelemetry.Context.Operation.Id = activity.RootId;
                requestTelemetry.Id = activity.Id;
                AspNetCoreEventSource.Instance.RequestTelemetryCreated("Hierarchical", requestTelemetry.Id, requestTelemetry.Context.Operation.Id);
            }

            if (this.proactiveSamplingEnabled
                && !activity.Recorded
                && this.configuration != null
                && !string.IsNullOrEmpty(requestTelemetry.Context.Operation.Id)
                && SamplingScoreGenerator.GetSamplingScore(requestTelemetry.Context.Operation.Id) >= this.configuration.GetLastObservedSamplingPercentage(requestTelemetry.ItemTypeFlag))
            {
                requestTelemetry.ProactiveSamplingDecision = SamplingDecision.SampledOut;
                AspNetCoreEventSource.Instance.TelemetryItemWasSampledOutAtHead(requestTelemetry.Context.Operation.Id);
            }

            //// When the item is proactively sampled out, we can avoid heavy operations that do not have known dependency later in the pipeline.
            //// We mostly exclude operations that were deemed heavy as per the corresponding profiler trace of this code path.

            if (requestTelemetry.ProactiveSamplingDecision != SamplingDecision.SampledOut)
            {
                foreach (var prop in activity.Baggage)
                {
                    if (!requestTelemetry.Properties.ContainsKey(prop.Key))
                    {
                        requestTelemetry.Properties[prop.Key] = prop.Value;
                    }
                }

                if (!string.IsNullOrEmpty(legacyRootId))
                {
                    requestTelemetry.Properties[LegacyRootIdProperty] = legacyRootId;
                }
            }

            this.client.InitializeInstrumentationKey(requestTelemetry);
            requestTelemetry.Source = this.GetAppIdFromRequestHeader(httpContext.Request.Headers, requestTelemetry.Context.InstrumentationKey);

            requestTelemetry.Start(timestamp);
            httpContext.Features.Set(requestTelemetry);

            return requestTelemetry;
        }

        private string GetAppIdFromRequestHeader(IHeaderDictionary requestHeaders, string instrumentationKey)
        {
            // set Source
            string headerCorrelationId = HttpHeadersUtilities.GetRequestContextKeyValue(requestHeaders, RequestResponseHeaders.RequestContextSourceKey);

            // If the source header is present on the incoming request, and it is an external component (not the same ikey as the one used by the current component), populate the source field.
            if (!string.IsNullOrEmpty(headerCorrelationId))
            {
                headerCorrelationId = StringUtilities.EnforceMaxLength(headerCorrelationId, InjectionGuardConstants.AppIdMaxLength);
                if (string.IsNullOrEmpty(instrumentationKey))
                {
                    return headerCorrelationId;
                }

                string applicationId = null;
                if ((this.applicationIdProvider?.TryGetApplicationId(instrumentationKey, out applicationId) ?? false)
                         && applicationId != headerCorrelationId)
                {
                    return headerCorrelationId;
                }
            }

            return null;
        }

        private void SetAppIdInResponseHeader(HttpContext httpContext, RequestTelemetry requestTelemetry)
        {
            if (this.injectResponseHeaders)
            {
                IHeaderDictionary responseHeaders = httpContext.Response?.Headers;
                if (responseHeaders != null &&
                    !string.IsNullOrEmpty(requestTelemetry.Context.InstrumentationKey) &&
                    (!responseHeaders.ContainsKey(RequestResponseHeaders.RequestContextHeader) ||
                     HttpHeadersUtilities.ContainsRequestContextKeyValue(
                         responseHeaders,
                         RequestResponseHeaders.RequestContextTargetKey)))
                {
                    if (this.lastIKeyLookedUp != requestTelemetry.Context.InstrumentationKey)
                    {
                        var appIdResolved = this.applicationIdProvider?.TryGetApplicationId(requestTelemetry.Context.InstrumentationKey, out this.lastAppIdUsed);
                        if (appIdResolved.HasValue && appIdResolved.Value)
                        {
                            this.lastIKeyLookedUp = requestTelemetry.Context.InstrumentationKey;
                        }
                    }

                    HttpHeadersUtilities.SetRequestContextKeyValue(
                        responseHeaders,
                        RequestResponseHeaders.RequestContextTargetKey,
                        this.lastAppIdUsed);
                }
            }
        }

        private void EndRequest(HttpContext httpContext, long timestamp)
        {
            if (this.client.IsEnabled())
            {
                // It's possible to host multiple apps (ASP.NET Core or generic hosts) in the same process
                // Each of this apps has it's own HostingDiagnosticListener and corresponding Http listener.
                // We should ignore events for all of them except one
                if (!SubscriptionManager.IsActive(this))
                {
                    AspNetCoreEventSource.Instance.NotActiveListenerNoTracking(
                        "EndRequest", Activity.Current?.Id);
                    return;
                }

                var telemetry = httpContext?.Features.Get<RequestTelemetry>();

                if (telemetry == null)
                {
                    // Log we are not tracking this request as it cannot be found in context.
                    return;
                }

                var activity = Activity.Current;
                
                // Suppress long running SignalR requests
                // Ref: https://github.com/dotnet/aspnetcore/pull/32084
                var httpLongRunningRequest = activity?.Tags.FirstOrDefault(tag => tag.Key == "http.long_running").Value;

                if (httpLongRunningRequest == "true")
                {
                    return;
                }

                telemetry.Stop(timestamp);
                telemetry.ResponseCode = httpContext.Response.StatusCode.ToString(CultureInfo.InvariantCulture);

                var successExitCode = httpContext.Response.StatusCode < 400;
                if (telemetry.Success == null)
                {
                    telemetry.Success = successExitCode;
                }
                else
                {
                    telemetry.Success &= successExitCode;
                }

                if (string.IsNullOrEmpty(telemetry.Name))
                {
                    telemetry.Name = httpContext.Request.Method + " " + httpContext.Request.Path.Value;
                }

                if (telemetry.ProactiveSamplingDecision != SamplingDecision.SampledOut)
                {
                    telemetry.Url = httpContext.Request.GetUri();
                    telemetry.Context.GetInternalContext().SdkVersion = this.sdkVersion;
                }

                this.client.TrackRequest(telemetry);

                // Stop what we started.
                if (activity != null && activity.OperationName == ActivityCreatedByHostingDiagnosticListener)
                {
                    activity.Stop();
                }
            }
        }

        private void OnException(HttpContext httpContext, Exception exception)
        {
            if (this.client.IsEnabled())
            {
                // It's possible to host multiple apps (ASP.NET Core or generic hosts) in the same process
                // Each of this apps has it's own HostingDiagnosticListener and corresponding Http listener.
                // We should ignore events for all of them except one
                if (!SubscriptionManager.IsActive(this))
                {
                    AspNetCoreEventSource.Instance.NotActiveListenerNoTracking(
                        "Exception", Activity.Current?.Id);
                    return;
                }

                var telemetry = httpContext?.Features.Get<RequestTelemetry>();
                if (telemetry != null)
                {
                    telemetry.Success = false;
                }

                var exceptionTelemetry = new ExceptionTelemetry(exception);
                exceptionTelemetry.Properties["handledAt"] = "Platform";
                exceptionTelemetry.Context.GetInternalContext().SdkVersion = this.sdkVersion;
                this.client.Track(exceptionTelemetry);
            }
        }
    }
}