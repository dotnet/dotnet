// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Internal;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation.AspNetCore.Implementation;

internal class HttpInListener : ListenerHandler
{
    internal const string ActivityOperationName = "Microsoft.AspNetCore.Hosting.HttpRequestIn";
    internal const string OnStartEvent = "Microsoft.AspNetCore.Hosting.HttpRequestIn.Start";
    internal const string OnStopEvent = "Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop";
    internal const string OnUnhandledHostingExceptionEvent = "Microsoft.AspNetCore.Hosting.UnhandledException";
    internal const string OnUnHandledDiagnosticsExceptionEvent = "Microsoft.AspNetCore.Diagnostics.UnhandledException";

    // https://github.com/dotnet/aspnetcore/blob/8d6554e655b64da75b71e0e20d6db54a3ba8d2fb/src/Hosting/Hosting/src/GenericHost/GenericWebHostBuilder.cs#L85
    internal const string AspNetCoreActivitySourceName = "Microsoft.AspNetCore";

    internal static readonly ActivitySource ActivitySource = ActivitySourceFactory.Create<HttpInListener>(AspNetCoreInstrumentation.SemanticConventionsVersion);
    internal static readonly bool Net7OrGreater = Environment.Version.Major >= 7;
    internal static readonly bool Net10OrGreater = Environment.Version.Major >= 10;
    internal static readonly bool Net11OrGreater = Environment.Version.Major >= 11;

    private const string DiagnosticSourceName = "Microsoft.AspNetCore";
    private const string CreatedByInstrumentationPropertyName = "OpenTelemetry.AspNetCore.CreatedByInstrumentation";
    private const string FrameworkActivityPropertyName = "OpenTelemetry.AspNetCore.FrameworkActivity";

    // The gRPC .NET library adds these tags to the Activity created by ASP.NET Core
    // (and not necessarily to Activity.Current). When the instrumentation creates a
    // sibling Activity these tags must be copied from the original (framework) Activity.
    // See https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/1778
    private static readonly string[] GrpcSourceTagNames =
    [
        GrpcTagHelper.GrpcMethodTagName,
        GrpcTagHelper.GrpcStatusCodeTagName,
        GrpcTagHelper.GrpcStatusTagName,
        GrpcTagHelper.GrpcTargetTagName,
    ];

    private static readonly Func<HttpRequest, string, IEnumerable<string>> HttpRequestHeaderValuesGetter = (request, name) =>
    {
        if (request.Headers.TryGetValue(name, out var value))
        {
            // This causes allocation as the `StringValues` struct has to be casted to an `IEnumerable<string>` object.
            return value;
        }

        return [];
    };

    private static readonly PropertyFetcher<Exception> ExceptionPropertyFetcher = new("Exception");
    private static readonly object CreatedByInstrumentationMarker = new();

    private readonly AspNetCoreTraceInstrumentationOptions options;
    private readonly bool nativeAspNetCoreOpenTelemetryEnabled;

    public HttpInListener(AspNetCoreTraceInstrumentationOptions options)
        : base(DiagnosticSourceName)
    {
        Guard.ThrowIfNull(options);

        this.options = options;
        this.nativeAspNetCoreOpenTelemetryEnabled = AspNetCoreHasNativeOpenTelemetryTags();
    }

    public override void OnEventWritten(string name, object? payload)
    {
        var activity = Activity.Current!;

        switch (name)
        {
            case OnStartEvent:
                this.OnStartActivity(activity, payload);
                break;

            case OnStopEvent:
                this.OnStopActivity(activity, payload);
                break;

            case OnUnhandledHostingExceptionEvent:
            case OnUnHandledDiagnosticsExceptionEvent:
                this.OnException(activity, payload);
                break;

            default:
                break;
        }
    }

    public void OnStartActivity(Activity activity, object? payload)
    {
        // The overall flow of what AspNetCore library does is as below:
        // Activity.Start()
        // DiagnosticSource.WriteEvent("Start", payload)
        // DiagnosticSource.WriteEvent("Stop", payload)
        // Activity.Stop()

        // This method is in the WriteEvent("Start", payload) path.
        // By this time, samplers have already run and
        // activity.IsAllDataRequested populated accordingly.

        if (payload is not HttpContext context)
        {
            AspNetCoreInstrumentationEventSource.Log.NullPayload(nameof(HttpInListener), nameof(this.OnStartActivity), activity.OperationName);
            return;
        }

        string? path = null;

        // Tracks whether the instrumentation created a sibling Activity. When it does,
        // ASP.NET Core 11+ writes its native OpenTelemetry tags to the framework Activity
        // (which is no longer sampled) rather than to the exported sibling, so the
        // instrumentation must set those tags itself instead of deferring to the framework.
        var createdSibling = false;

        // Ensure context extraction irrespective of sampling decision
        var request = context.Request;
        var textMapPropagator = Propagators.DefaultTextMapPropagator;
        if (textMapPropagator is not TraceContextPropagator)
        {
            var ctx = textMapPropagator.Extract(default, request, HttpRequestHeaderValuesGetter);
            if (ctx.ActivityContext.IsValid()
                && !((ctx.ActivityContext.TraceId == activity.TraceId)
                    && (ctx.ActivityContext.SpanId == activity.ParentSpanId)
                    && (ctx.ActivityContext.TraceState == activity.TraceStateString)))
            {
                // Create a new activity with its parent set from the extracted context.
                // This makes the new activity as a "sibling" of the activity created by
                // Asp.Net Core.
                Activity? newOne;
                if (Net7OrGreater)
                {
                    // For NET7.0 onwards activity is created using ActivitySource so,
                    // we will use the source of the activity to create the new one.
                    newOne = activity.Source.CreateActivity(ActivityOperationName, ActivityKind.Server, ctx.ActivityContext);
                }
                else
                {
#pragma warning disable CA2000
                    newOne = new Activity(ActivityOperationName);
#pragma warning restore CA2000
                    newOne.SetParentId(ctx.ActivityContext.TraceId, ctx.ActivityContext.SpanId, ctx.ActivityContext.TraceFlags);
                }

                newOne!.TraceStateString = ctx.ActivityContext.TraceState;

                newOne.SetCustomProperty(CreatedByInstrumentationPropertyName, CreatedByInstrumentationMarker);

                // Keep a reference to the framework Activity. The gRPC .NET library may add
                // its tags to that Activity rather than to Activity.Current, so they need to
                // be copied onto the sibling Activity when it is stopped.
                // See https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/1778
                newOne.SetCustomProperty(FrameworkActivityPropertyName, activity);

                // Starting the new activity make it the Activity.Current one.
                newOne.Start();

                // Set IsAllDataRequested to false for the activity created by the framework to only export the sibling activity and not the framework activity
                activity.IsAllDataRequested = false;
                activity = newOne;
                createdSibling = true;
            }

            Baggage.Current = ctx.Baggage;
        }

        // enrich Activity from payload only if sampling decision
        // is favorable.
        if (activity.IsAllDataRequested)
        {
            if (this.options.Filter is { } filter)
            {
                try
                {
                    if (!filter(context))
                    {
                        AspNetCoreInstrumentationEventSource.Log.RequestIsFilteredOut(nameof(HttpInListener), nameof(this.OnStartActivity), activity.OperationName);
                        DisableActivity(activity);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    AspNetCoreInstrumentationEventSource.Log.RequestFilterException(nameof(HttpInListener), nameof(this.OnStartActivity), activity.OperationName, ex);
                    DisableActivity(activity);
                    return;
                }

                static void DisableActivity(Activity activity)
                {
                    activity.IsAllDataRequested = false;
                    activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
                }
            }

            if (!Net7OrGreater)
            {
                ActivityInstrumentationHelper.SetActivitySourceProperty(activity, ActivitySource);
                ActivityInstrumentationHelper.SetKindProperty(activity, ActivityKind.Server);
            }

            // ASP.NET Core does not support OTEL_INSTRUMENTATION_HTTP_KNOWN_METHODS so we
            // still need to set the display name and HTTP method tag so that any override
            // by the user is honoured. See https://github.com/dotnet/aspnetcore/issues/65873.
            TelemetryHelper.RequestDataHelper.SetActivityDisplayName(activity, request.Method);

            // ASP.NET Core 10 does not support OTEL_INSTRUMENTATION_HTTP_KNOWN_METHODS so we
            // still need to set the HTTP method tag so that any override by the user is honoured.
            // See https://github.com/dotnet/aspnetcore/issues/65873.
            TelemetryHelper.RequestDataHelper.SetActivityDisplayNameAndHttpMethodTag(activity, request.Method);

            // When a sibling Activity was created the framework's native tags land on the
            // (now unsampled) framework Activity, so set them here on the exported sibling.
            if (!Net10OrGreater || !this.nativeAspNetCoreOpenTelemetryEnabled || createdSibling)
            {
                if (request.Host.HasValue)
                {
                    activity.SetTag(SemanticConventions.AttributeServerAddress, request.Host.Host);

                    if (request.Host.Port is { } port)
                    {
                        activity.SetTag(SemanticConventions.AttributeServerPort, port);
                    }
                }

                if (request.Headers.TryGetValue("User-Agent", out var values))
                {
                    var userAgent = values.Count > 0 ? values[0] : null;
                    if (!string.IsNullOrEmpty(userAgent))
                    {
                        activity.SetTag(SemanticConventions.AttributeUserAgentOriginal, userAgent);
                    }
                }

                activity.SetTag(SemanticConventions.AttributeUrlScheme, request.Scheme);

                SetUrlPathAttribute(request, activity);
            }

            if (request.QueryString.HasValue)
            {
                if (this.options.DisableUrlQueryRedaction)
                {
                    activity.SetTag(SemanticConventions.AttributeUrlQuery, request.QueryString.Value);
                }
                else
                {
                    activity.SetTag(SemanticConventions.AttributeUrlQuery, RedactionHelper.GetRedactedQueryString(request.QueryString.Value!));
                }
            }

            activity.SetTag(SemanticConventions.AttributeNetworkProtocolVersion, RequestDataHelper.GetHttpProtocolVersion(request.Protocol));

            if (this.options.EnrichWithHttpRequest is { } enricher)
            {
                try
                {
                    enricher(activity, request);
                }
                catch (Exception ex)
                {
                    AspNetCoreInstrumentationEventSource.Log.EnrichmentException(nameof(HttpInListener), nameof(this.OnStartActivity), activity.OperationName, ex);
                }
            }
        }

        void SetUrlPathAttribute(HttpRequest request, Activity activity)
        {
            // See the spec: https://github.com/open-telemetry/semantic-conventions/blob/v1.40.0/docs/http/http-spans.md
            path ??= (request.PathBase.HasValue || request.Path.HasValue) ? (request.PathBase + request.Path).ToString() : "/";
            activity.SetTag(SemanticConventions.AttributeUrlPath, path);
        }
    }

    public void OnStopActivity(Activity activity, object? payload)
    {
        if (activity.IsAllDataRequested)
        {
            if (payload is not HttpContext context)
            {
                AspNetCoreInstrumentationEventSource.Log.NullPayload(nameof(HttpInListener), nameof(this.OnStopActivity), activity.OperationName);
                return;
            }

            var response = context.Response;
            var statusCodeSet = false;

            // When the instrumentation created a sibling Activity, ASP.NET Core 11+ wrote its
            // native tags (route, display name, status code) to the unsampled framework Activity,
            // so the instrumentation must set them on the exported sibling instead.
            var createdSibling = ReferenceEquals(activity.GetCustomProperty(CreatedByInstrumentationPropertyName), CreatedByInstrumentationMarker);

            if (!Net11OrGreater || !this.nativeAspNetCoreOpenTelemetryEnabled || createdSibling)
            {
#if NET
                var routePattern = context.GetHttpRoute();
                if (!string.IsNullOrEmpty(routePattern))
                {
                    TelemetryHelper.RequestDataHelper.SetActivityDisplayName(activity, context.Request.Method, routePattern);
                    activity.SetTag(SemanticConventions.AttributeHttpRoute, routePattern);
                }
#endif

                activity.SetTag(SemanticConventions.AttributeHttpResponseStatusCode, TelemetryHelper.GetBoxedStatusCode(response.StatusCode));
                statusCodeSet = true;

                if (activity.Status == ActivityStatusCode.Unset)
                {
                    activity.SetStatus(SpanHelper.ResolveActivityStatusForHttpStatusCode(activity.Kind, response.StatusCode));
                }
            }

            // If the instrumentation created a sibling Activity, the gRPC .NET library may
            // have added its grpc.* tags to the original (framework) Activity instead of to
            // the sibling Activity that is exported. Copy them across so that gRPC requests
            // are handled the same regardless of whether a sibling Activity was created.
            // See https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/1778
            CopyGrpcTagsFromFrameworkActivity(activity);

            if (this.options.EnableGrpcAspNetCoreSupport && IsGrpcRequest(activity, out var grpcMethod))
            {
                // Single pass over the tag collection to retrieve both gRPC tags,
                // avoiding separate GetTagValue iterations.
                var grpcStatusCode = -1;
                var hasGrpcStatusCode = false;

                var tagEnumerator = activity.EnumerateTagObjects();
                while (tagEnumerator.MoveNext())
                {
                    ref readonly var tag = ref tagEnumerator.Current;
                    if (grpcMethod is null && tag.Key == GrpcTagHelper.GrpcMethodTagName)
                    {
                        grpcMethod = tag.Value as string;
                    }
                    else if (!hasGrpcStatusCode && tag.Key == GrpcTagHelper.GrpcStatusCodeTagName)
                    {
                        hasGrpcStatusCode = int.TryParse(tag.Value as string, NumberStyles.None, CultureInfo.InvariantCulture, out grpcStatusCode);
                    }

                    if (grpcMethod is not null && hasGrpcStatusCode)
                    {
                        break;
                    }
                }

                if (grpcMethod is { Length: > 0 })
                {
                    if (!statusCodeSet)
                    {
                        activity.SetTag(SemanticConventions.AttributeHttpResponseStatusCode, TelemetryHelper.GetBoxedStatusCode(response.StatusCode));
                    }

                    AddGrpcAttributes(
                        activity,
                        grpcMethod,
                        context,
                        grpcStatusCode,
                        hasGrpcStatusCode);
                }
            }

            if (activity.Status == ActivityStatusCode.Unset)
            {
                activity.SetStatus(SpanHelper.ResolveActivityStatusForHttpStatusCode(activity.Kind, response.StatusCode));
            }

            if (this.options.EnrichWithHttpResponse is { } enricher)
            {
                try
                {
                    enricher(activity, response);
                }
                catch (Exception ex)
                {
                    AspNetCoreInstrumentationEventSource.Log.EnrichmentException(nameof(HttpInListener), nameof(this.OnStopActivity), activity.OperationName, ex);
                }
            }
        }

        if (ReferenceEquals(activity.GetCustomProperty(CreatedByInstrumentationPropertyName), CreatedByInstrumentationMarker))
        {
            // If instrumentation started a new Activity, it must
            // be stopped here.
            activity.Stop();

            // After the activity.Stop() code, Activity.Current becomes null.
            // If Asp.Net Core uses Activity.Current?.Stop() - it'll not stop the activity
            // it created.
            // Currently Asp.Net core does not use Activity.Current, instead it stores a
            // reference to its activity, and calls .Stop on it.

            // TODO: Should we still restore Activity.Current here?
            // If yes, then we need to store the asp.net core activity inside
            // the one created by the instrumentation.
            // And retrieve it here, and set it to Current.
        }
    }

    public void OnException(Activity activity, object? payload)
    {
        if (activity.IsAllDataRequested)
        {
            // We need to use reflection here as the payload type is not a defined public type.
            if (!TryFetchException(payload, out var exc))
            {
                AspNetCoreInstrumentationEventSource.Log.NullPayload(nameof(HttpInListener), nameof(this.OnException), activity.OperationName);
                return;
            }

            activity.SetTag(SemanticConventions.AttributeErrorType, exc.GetType().FullName);

            if (this.options.RecordException)
            {
                activity.AddException(exc);
            }

            activity.SetStatus(ActivityStatusCode.Error);

            if (this.options.EnrichWithException is { } enricher)
            {
                try
                {
                    enricher(activity, exc);
                }
                catch (Exception ex)
                {
                    AspNetCoreInstrumentationEventSource.Log.EnrichmentException(nameof(HttpInListener), nameof(this.OnException), activity.OperationName, ex);
                }
            }
        }

        // See https://github.com/dotnet/aspnetcore/blob/690d78279e940d267669f825aa6627b0d731f64c/src/Hosting/Hosting/src/Internal/HostingApplicationDiagnostics.cs#L252
        // and https://github.com/dotnet/aspnetcore/blob/690d78279e940d267669f825aa6627b0d731f64c/src/Middleware/Diagnostics/src/DeveloperExceptionPage/DeveloperExceptionPageMiddlewareImpl.cs#L174
        // this makes sure that top-level properties on the payload object are always preserved.
#if NET
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "The event source guarantees that top level properties are preserved")]
#endif
        static bool TryFetchException(object? payload, [NotNullWhen(true)] out Exception? exc)
        {
            return ExceptionPropertyFetcher.TryFetch(payload, out exc) && exc != null;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AddGrpcAttributes(
        Activity activity,
        string? grpcMethod,
        HttpContext context,
        int grpcStatusCode,
        bool validStatusCode)
    {
        // See the specs for semantic conventions.
        // https://github.com/open-telemetry/semantic-conventions/blob/v1.42.0/docs/rpc/rpc-spans.md
        GrpcTagHelper.SetGrpcSystemName(activity);
        GrpcTagHelper.SetGrpcMethodAndDisplayNameFromActivity(activity, grpcMethod);

        if (context.Connection.RemoteIpAddress != null)
        {
            activity.SetTag(SemanticConventions.AttributeNetworkPeerAddress, context.Connection.RemoteIpAddress.ToString());
        }

        activity.SetTag(SemanticConventions.AttributeNetworkPeerPort, context.Connection.RemotePort);

        var spanStatus = ActivityStatusCode.Unset;
        if (validStatusCode)
        {
            spanStatus = GrpcTagHelper.ResolveSpanStatusForGrpcStatusCodeOnServer(grpcStatusCode);
            activity.SetStatus(spanStatus);
        }

        // The grpc.method tag has now been mapped to rpc.method, so the source tag can be removed.
        // See https://github.com/open-telemetry/semantic-conventions/blob/v1.42.0/docs/non-normative/compatibility/grpc.md#attribute-mapping
        activity.SetTag(GrpcTagHelper.GrpcMethodTagName, null);
        activity.SetTag(GrpcTagHelper.GrpcTargetTagName, null);

        if (validStatusCode)
        {
            // rpc.response.status_code is the string representation of the gRPC status code, e.g. "OK".
            var grpcStatusName = GrpcTagHelper.GetGrpcStatusCodeName(grpcStatusCode);
            activity.SetTag(SemanticConventions.AttributeRpcResponseStatusCode, grpcStatusName);

            // The grpc.status/grpc.status_code tags have now been mapped to rpc.response.status_code, so they can be removed.
            // The source tags are only removed once mapped so that an unrecognized status code is not silently dropped.
            activity.SetTag(GrpcTagHelper.GrpcStatusTagName, null);
            activity.SetTag(GrpcTagHelper.GrpcStatusCodeTagName, null);

            // error.type is conditionally required when the operation failed; for gRPC it is set to the status code.
            if (spanStatus == ActivityStatusCode.Error)
            {
                activity.SetTag(SemanticConventions.AttributeErrorType, grpcStatusName);
            }
        }
    }

    private static bool AspNetCoreHasNativeOpenTelemetryTags()
    {
        bool? suppressed = null;

        // ASP.NET Core 10 added a feature switch to specify whether to suppress OpenTelemetry
        // tags being added natively by default, so we can take an optimal path if the user has
        // not explicitly opted-out of suppressing the OpenTelemetry data.
        if (AppContext.TryGetSwitch("Microsoft.AspNetCore.Hosting.SuppressActivityOpenTelemetryData", out var configuredValue))
        {
            suppressed = configuredValue;
        }

        if (suppressed is { } suppressedValue)
        {
            return !suppressedValue;
        }

        // In ASP.NET Core 8 and 9 the feature switch does not exist and there are no native OpenTelemetry tags.
        // In ASP.NET Core 10 OpenTelemetry tags are suppressed by default,
        // see https://github.com/dotnet/aspnetcore/blob/7387de91234d3ef751fa50b3d1bfede4130213ff/src/Hosting/Hosting/src/Internal/HostingApplicationDiagnostics.cs#L59-L67.
        // In ASP.NET Core 11+ OpenTelemetry tags are emitted by default,
        // see https://github.com/dotnet/aspnetcore/blob/655f41d52f2fc75992eac41496b8e9cc119e1b54/src/Hosting/Hosting/src/Internal/HostingApplicationDiagnostics.cs#L59-L67.
        return Net11OrGreater;
    }

    private static void CopyGrpcTagsFromFrameworkActivity(Activity activity)
    {
        // Only sibling activities created by the instrumentation have this property set.
        if (activity.GetCustomProperty(FrameworkActivityPropertyName) is not Activity frameworkActivity)
        {
            return;
        }

        foreach (var tagName in GrpcSourceTagNames)
        {
            if (frameworkActivity.GetTagValue(tagName) is { } value)
            {
                activity.SetTag(tagName, value);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsGrpcRequest(Activity activity, [NotNullWhen(true)] out string? grpcMethod)
    {
        // gRPC-Web (https://learn.microsoft.com/aspnet/core/grpc/grpcweb) allows ASP.NET Core
        // to support using gRPC from clients that do not support HTTP/2 or HTTP/3, so we
        // can't just look at the HTTP protocol version to attempt to shortcut the test.
        grpcMethod = GrpcTagHelper.GetGrpcMethodFromActivity(activity);
        return !string.IsNullOrEmpty(grpcMethod);
    }
}
