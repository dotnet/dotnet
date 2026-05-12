// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Owin;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Instrumentation.Owin.Implementation;
using OpenTelemetry.Internal;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation.Owin;

/// <summary>
/// Instruments incoming request with <see cref="Activity"/> and notifies listeners with <see cref="ActivitySource"/>.
/// </summary>
internal sealed class DiagnosticsMiddleware : OwinMiddleware
{
    private const string ContextKey = "__OpenTelemetry.Context__";
    private static readonly Func<IOwinRequest, string, IEnumerable<string>> OwinRequestHeaderValuesGetter
        = (request, name) => request.Headers.GetValues(name);

    private static readonly RequestDataHelper RequestDataHelper = new(configureByHttpKnownMethodsEnvironmentalVariable: false);

    /// <summary>
    /// Initializes a new instance of the <see cref="DiagnosticsMiddleware"/> class.
    /// </summary>
    /// <param name="next">An optional pointer to the next component.</param>
    public DiagnosticsMiddleware(OwinMiddleware next)
        : base(next)
    {
    }

    /// <inheritdoc />
    public override async Task Invoke(IOwinContext owinContext)
    {
        long startTimestamp = -1;

        try
        {
            BeginRequest(owinContext);

            if (OwinInstrumentationMetrics.HttpServerDuration.Enabled && !owinContext.Environment.ContainsKey(ContextKey))
            {
                startTimestamp = Stopwatch.GetTimestamp();
            }

            await this.Next.Invoke(owinContext).ConfigureAwait(false);
            RequestEnd(owinContext, null, startTimestamp);
        }
        catch (Exception ex)
        {
            RequestEnd(owinContext, ex, startTimestamp);
            throw;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void BeginRequest(IOwinContext owinContext)
    {
        try
        {
            if (OwinInstrumentationActivitySource.Options == null || OwinInstrumentationActivitySource.Options.Filter?.Invoke(owinContext) == false)
            {
                OwinInstrumentationEventSource.Log.RequestIsFilteredOut();
                return;
            }
        }
#pragma warning disable CA1031 // Do not catch general exception types
        catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
        {
            OwinInstrumentationEventSource.Log.RequestFilterException(ex);
            return;
        }

        var textMapPropagator = Propagators.DefaultTextMapPropagator;
        var ctx = textMapPropagator.Extract(default, owinContext.Request, OwinRequestHeaderValuesGetter);

        var activity = OwinInstrumentationActivitySource.ActivitySource.StartActivity(
            OwinInstrumentationActivitySource.IncomingRequestActivityName,
            ActivityKind.Server,
            ctx.ActivityContext);

        if (activity != null)
        {
            var request = owinContext.Request;

            // Note: Display name is intentionally set to a low cardinality
            // value because OWIN does not expose any kind of
            // route/template. See:
            // https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/trace/semantic_conventions/http.md#name
            RequestDataHelper.SetActivityDisplayName(activity, request.Method);

            if (activity.IsAllDataRequested)
            {
                var queryString = OwinInstrumentationActivitySource.Options.DisableUrlQueryRedaction
                    ? request.QueryString.Value
                    : RedactionHelper.GetRedactedQueryString(request.QueryString.Value);

                RequestDataHelper.SetHttpMethodTag(activity, request.Method);
                activity.SetTag(SemanticConventions.AttributeServerAddress, request.Uri.Host);
                activity.SetTag(SemanticConventions.AttributeServerPort, request.Uri.Port);
                activity.SetTag(SemanticConventions.AttributeNetworkProtocolVersion, request.Protocol);

                activity.SetTag(SemanticConventions.AttributeUrlPath, request.Uri.AbsolutePath);
                activity.SetTag(SemanticConventions.AttributeUrlQuery, queryString);
                activity.SetTag(SemanticConventions.AttributeUrlScheme, owinContext.Request.Scheme);

                if (request.Headers.TryGetValue("User-Agent", out var userAgent) && userAgent.Length > 0)
                {
                    activity.SetTag(SemanticConventions.AttributeUserAgentOriginal, userAgent[0]);
                }

                try
                {
                    OwinInstrumentationActivitySource.Options?.Enrich?.Invoke(
                        activity,
                        OwinEnrichEventType.BeginRequest,
                        owinContext,
                        null);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    OwinInstrumentationEventSource.Log.EnrichmentException(ex);
                }
            }

            if (textMapPropagator is not TraceContextPropagator)
            {
                Baggage.Current = ctx.Baggage;
            }

            owinContext.Environment[ContextKey] = activity;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void RequestEnd(IOwinContext owinContext, Exception? exception, long startTimestamp)
    {
        if (owinContext.Environment.TryGetValue(ContextKey, out var context)
            && context is Activity activity)
        {
            if (Activity.Current != activity)
            {
                Activity.Current = activity;
            }

            if (activity.IsAllDataRequested)
            {
                var response = owinContext.Response;

                if (exception != null)
                {
                    activity.SetStatus(ActivityStatusCode.Error);

                    if (OwinInstrumentationActivitySource.Options?.RecordException == true)
                    {
                        activity.AddException(exception);
                    }
                }
                else if (activity.Status == ActivityStatusCode.Unset)
                {
                    activity.SetStatus(SpanHelper.ResolveActivityStatusForHttpStatusCode(activity.Kind, response.StatusCode));
                }

                activity.SetTag(SemanticConventions.AttributeHttpResponseStatusCode, response.StatusCode);

                try
                {
                    OwinInstrumentationActivitySource.Options?.Enrich?.Invoke(
                        activity,
                        OwinEnrichEventType.EndRequest,
                        owinContext,
                        exception);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    OwinInstrumentationEventSource.Log.EnrichmentException(ex);
                }
            }

            activity.Stop();

            if (OwinInstrumentationMetrics.HttpServerDuration.Enabled)
            {
                OwinInstrumentationMetrics.HttpServerDuration.Record(
                    activity.Duration.TotalSeconds,
                    new(SemanticConventions.AttributeHttpRequestMethod, owinContext.Request.Method),
                    new(SemanticConventions.AttributeUrlScheme, owinContext.Request.Scheme),
                    new(SemanticConventions.AttributeHttpResponseStatusCode, owinContext.Response.StatusCode));
            }

            if (Propagators.DefaultTextMapPropagator is not TraceContextPropagator)
            {
                Baggage.Current = default;
            }
        }
        else if (OwinInstrumentationMetrics.HttpServerDuration.Enabled)
        {
            var endTimestamp = Stopwatch.GetTimestamp();
            var duration = endTimestamp - startTimestamp;
            var durationS = duration / (double)Stopwatch.Frequency;

            OwinInstrumentationMetrics.HttpServerDuration.Record(
                durationS,
                new(SemanticConventions.AttributeHttpRequestMethod, owinContext.Request.Method),
                new(SemanticConventions.AttributeUrlScheme, owinContext.Request.Scheme),
                new(SemanticConventions.AttributeHttpResponseStatusCode, owinContext.Response.StatusCode));
        }
    }
}
