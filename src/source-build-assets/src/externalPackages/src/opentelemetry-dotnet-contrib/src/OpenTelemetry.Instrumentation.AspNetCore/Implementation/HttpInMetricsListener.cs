// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using System.Reflection;
using Microsoft.AspNetCore.Http;
#if NET
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Routing;
#endif
using OpenTelemetry.Internal;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation.AspNetCore.Implementation;

internal sealed class HttpInMetricsListener : ListenerHandler
{
    internal const string HttpServerRequestDurationMetricName = "http.server.request.duration";

    internal const string OnUnhandledHostingExceptionEvent = "Microsoft.AspNetCore.Hosting.UnhandledException";
    internal const string OnUnhandledDiagnosticsExceptionEvent = "Microsoft.AspNetCore.Diagnostics.UnhandledException";

    internal static readonly AssemblyName AssemblyName = typeof(HttpInListener).Assembly.GetName();
    internal static readonly string InstrumentationName = AssemblyName.Name!;
    internal static readonly string InstrumentationVersion = AssemblyName.Version!.ToString();
    internal static readonly Meter Meter = new(InstrumentationName, InstrumentationVersion);

    private const string OnStopEvent = "Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop";

    private static readonly PropertyFetcher<Exception> ExceptionPropertyFetcher = new("Exception");
    private static readonly PropertyFetcher<HttpContext> HttpContextPropertyFetcher = new("HttpContext");
    private static readonly object ErrorTypeHttpContextItemsKey = new();

    private static readonly Histogram<double> HttpServerRequestDuration = Meter.CreateHistogram(
        HttpServerRequestDurationMetricName,
        unit: "s",
        description: " Duration of HTTP server requests.",
        advice: new InstrumentAdvice<double> { HistogramBucketBoundaries = [0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10] });

    internal HttpInMetricsListener(string name)
        : base(name)
    {
    }

    public static void OnExceptionEventWritten(string name, object? payload)
    {
        // We need to use reflection here as the payload type is not a defined public type.
        if (!TryFetchException(payload, out var exc) || !TryFetchHttpContext(payload, out var ctx))
        {
            AspNetCoreInstrumentationEventSource.Log.NullPayload(nameof(HttpInMetricsListener), nameof(OnExceptionEventWritten), HttpServerRequestDurationMetricName);
            return;
        }

        ctx.Items.Add(ErrorTypeHttpContextItemsKey, exc.GetType().FullName);

        // See https://github.com/dotnet/aspnetcore/blob/690d78279e940d267669f825aa6627b0d731f64c/src/Hosting/Hosting/src/Internal/HostingApplicationDiagnostics.cs#L252
        // and https://github.com/dotnet/aspnetcore/blob/690d78279e940d267669f825aa6627b0d731f64c/src/Middleware/Diagnostics/src/DeveloperExceptionPage/DeveloperExceptionPageMiddlewareImpl.cs#L174
        // this makes sure that top-level properties on the payload object are always preserved.
#if NET
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "The ASP.NET Core framework guarantees that top level properties are preserved")]
#endif
        static bool TryFetchException(object? payload, [NotNullWhen(true)] out Exception? exc)
        {
            return ExceptionPropertyFetcher.TryFetch(payload, out exc) && exc != null;
        }
#if NET
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "The ASP.NET Core framework guarantees that top level properties are preserved")]
#endif
        static bool TryFetchHttpContext(object? payload, [NotNullWhen(true)] out HttpContext? ctx)
        {
            return HttpContextPropertyFetcher.TryFetch(payload, out ctx) && ctx != null;
        }
    }

    public static void OnStopEventWritten(string name, object? payload)
    {
        if (payload is not HttpContext context)
        {
            AspNetCoreInstrumentationEventSource.Log.NullPayload(nameof(HttpInMetricsListener), nameof(OnStopEventWritten), HttpServerRequestDurationMetricName);
            return;
        }

        TagList tags = default;

        // see the spec https://github.com/open-telemetry/semantic-conventions/blob/v1.21.0/docs/http/http-spans.md
        tags.Add(new KeyValuePair<string, object?>(SemanticConventions.AttributeNetworkProtocolVersion, RequestDataHelper.GetHttpProtocolVersion(context.Request.Protocol)));
        tags.Add(new KeyValuePair<string, object?>(SemanticConventions.AttributeUrlScheme, context.Request.Scheme));
        tags.Add(new KeyValuePair<string, object?>(SemanticConventions.AttributeHttpResponseStatusCode, TelemetryHelper.GetBoxedStatusCode(context.Response.StatusCode)));

        var httpMethod = TelemetryHelper.RequestDataHelper.GetNormalizedHttpMethod(context.Request.Method);
        tags.Add(new KeyValuePair<string, object?>(SemanticConventions.AttributeHttpRequestMethod, httpMethod));

#if NET
        // Check the exception handler feature first in case the endpoint was overwritten
        var route = (context.Features.Get<IExceptionHandlerPathFeature>()?.Endpoint as RouteEndpoint ??
                     context.GetEndpoint() as RouteEndpoint)?.RoutePattern.RawText;
        if (!string.IsNullOrEmpty(route))
        {
            tags.Add(new KeyValuePair<string, object?>(SemanticConventions.AttributeHttpRoute, route));
        }
#endif
        if (context.Items.TryGetValue(ErrorTypeHttpContextItemsKey, out var errorType))
        {
            tags.Add(new KeyValuePair<string, object?>(SemanticConventions.AttributeErrorType, errorType));
        }

        // We are relying here on ASP.NET Core to set duration before writing the stop event.
        // https://github.com/dotnet/aspnetcore/blob/d6fa351048617ae1c8b47493ba1abbe94c3a24cf/src/Hosting/Hosting/src/Internal/HostingApplicationDiagnostics.cs#L449
        // TODO: Follow up with .NET team if we can continue to rely on this behavior.
        HttpServerRequestDuration.Record(Activity.Current!.Duration.TotalSeconds, tags);
    }

    public override void OnEventWritten(string name, object? payload)
    {
        switch (name)
        {
            case OnUnhandledDiagnosticsExceptionEvent:
            case OnUnhandledHostingExceptionEvent:
                {
                    OnExceptionEventWritten(name, payload);
                }

                break;
            case OnStopEvent:
                {
                    OnStopEventWritten(name, payload);
                }

                break;
            default:
                break;
        }
    }
}
