// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Concurrent;
using System.Diagnostics;
#if NETFRAMEWORK
using System.Net.Http;
#endif
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using OpenTelemetry.Internal;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation.ElasticsearchClient.Implementation;

internal partial class ElasticsearchRequestPipelineDiagnosticListener : ListenerHandler
{
    internal const string DatabaseSystemName = "elasticsearch";
    internal const string ExceptionCustomPropertyName = "OTel.Elasticsearch.Exception";
    internal const string AttributeDbMethod = "db.method";

    internal static readonly Assembly Assembly = typeof(ElasticsearchRequestPipelineDiagnosticListener).Assembly;
    internal static readonly AssemblyName AssemblyName = Assembly.GetName();
    internal static readonly string ActivitySourceName = AssemblyName.Name!;
    internal static readonly ActivitySource ActivitySource = new(ActivitySourceName, Assembly.GetPackageVersion());

    private const string RequestRegexPattern = @"\n# Request:\r?\n(\{.*)\n# Response";

#if !NET
    private static readonly Regex ParseRequest = new(RequestRegexPattern, RegexOptions.Compiled | RegexOptions.Singleline);
#endif
    private static readonly ConcurrentDictionary<object, string> MethodNameCache = new();

    private readonly ElasticsearchClientInstrumentationOptions options;
    private readonly MultiTypePropertyFetcher<Uri> uriFetcher = new("Uri");
    private readonly MultiTypePropertyFetcher<object> methodFetcher = new("Method");
    private readonly MultiTypePropertyFetcher<string> debugInformationFetcher = new("DebugInformation");
    private readonly MultiTypePropertyFetcher<int?> httpStatusFetcher = new("HttpStatusCode");
    private readonly MultiTypePropertyFetcher<Exception> originalExceptionFetcher = new("OriginalException");
    private readonly MultiTypePropertyFetcher<object> failureReasonFetcher = new("FailureReason");
    private readonly MultiTypePropertyFetcher<byte[]> responseBodyFetcher = new("ResponseBodyInBytes");

    public ElasticsearchRequestPipelineDiagnosticListener(ElasticsearchClientInstrumentationOptions options)
        : base("Elasticsearch.Net.RequestPipeline")
    {
        this.options = options;
    }

    public override void OnEventWritten(string name, object? payload)
    {
        var activity = Activity.Current;
        Guard.ThrowIfNull(activity);
        switch (name)
        {
            case "CallElasticsearch.Start":
                this.OnStartActivity(activity, payload);
                break;
            case "CallElasticsearch.Stop":
                this.OnStopActivity(activity, payload);
                break;
            default:
                break;
        }
    }

    private static string GetDisplayName(Activity activity, object? method, string? elasticType = null)
    {
        switch (activity.OperationName)
        {
            case "Ping":
                return "Elasticsearch Ping";
            case "CallElasticsearch" when method != null:
                {
                    var methodName = MethodNameCache.GetOrAdd(method, $"Elasticsearch {method}");
                    return elasticType == null ? methodName : $"{methodName} {elasticType}";
                }

            default:
                return "Elasticsearch";
        }
    }

    private static string? GetElasticIndex(Uri uri)
    {
        // first segment is always /
        if (uri.Segments.Length < 2)
        {
            return null;
        }

        // operations starting with _ are not indices (_cat, _search, etc.)
#if NET
        if (uri.Segments[1].StartsWith('_'))
#else
        if (uri.Segments[1].StartsWith("_", StringComparison.Ordinal))
#endif
        {
            return null;
        }

        var elasticType = Uri.UnescapeDataString(uri.Segments[1]);

        // multiple indices used, return null to avoid high cardinality
#if NET
        if (elasticType.Contains(',', StringComparison.Ordinal))
#else
        if (elasticType.Contains(','))
#endif
        {
            return null;
        }

#if NET
        if (elasticType.EndsWith('/'))
#else
        if (elasticType.EndsWith("/", StringComparison.Ordinal))
#endif
        {
            elasticType = elasticType.Substring(0, elasticType.Length - 1);
        }

        return elasticType;
    }

#if NET
    [GeneratedRegex(RequestRegexPattern, RegexOptions.Compiled | RegexOptions.Singleline)]
    private static partial Regex RequestParser();
#else
    private static Regex RequestParser() => ParseRequest;
#endif

    private string ParseAndFormatRequest(string debugInformation)
    {
        if (!this.options.ParseAndFormatRequest)
        {
            return debugInformation;
        }

        var request = RequestParser().Match(debugInformation);
        if (request.Success)
        {
            var body = request.Groups[1]?.Value?.Trim();
            if (body == null)
            {
                return debugInformation;
            }

            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(body));
            if (!JsonDocument.TryParseValue(ref reader, out var doc))
            {
                return debugInformation;
            }

            using (doc)
            using (var stream = new MemoryStream())
            {
                using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
                doc.WriteTo(writer);
                writer.Flush();
                body = Encoding.UTF8.GetString(stream.ToArray());
            }

            return body;
        }

        return debugInformation;
    }

    private void OnStartActivity(Activity activity, object? payload)
    {
        // By this time, samplers have already run and
        // activity.IsAllDataRequested populated accordingly.

        if (Sdk.SuppressInstrumentation)
        {
            return;
        }

        Guard.ThrowIfNull(activity);
        if (activity.IsAllDataRequested)
        {
            var uri = this.uriFetcher.Fetch(payload);

            if (uri == null)
            {
                ElasticsearchInstrumentationEventSource.Log.NullPayload(nameof(ElasticsearchRequestPipelineDiagnosticListener), nameof(this.OnStartActivity));
                return;
            }

            // remove sensitive information like user and password information
            uri = UriHelper.ScrubUserInfo(uri);

            ActivityInstrumentationHelper.SetActivitySourceProperty(activity, ActivitySource);
            ActivityInstrumentationHelper.SetKindProperty(activity, ActivityKind.Client);

            var method = this.methodFetcher.Fetch(payload);

            if (this.options.SuppressDownstreamInstrumentation)
            {
                SuppressInstrumentationScope.Enter();
            }

            var elasticIndex = GetElasticIndex(uri);
            activity.DisplayName = GetDisplayName(activity, method, elasticIndex);
            activity.SetTag(SemanticConventions.AttributeDbSystem, DatabaseSystemName);

            if (elasticIndex != null)
            {
                activity.SetTag(SemanticConventions.AttributeDbName, elasticIndex);
            }

            var uriHostNameType = Uri.CheckHostName(uri.Host);
            if (uriHostNameType is UriHostNameType.IPv4 or UriHostNameType.IPv6)
            {
                activity.SetTag(SemanticConventions.AttributeNetPeerIp, uri.Host);
            }
            else
            {
                activity.SetTag(SemanticConventions.AttributeNetPeerName, uri.Host);
            }

            if (uri.Port > 0)
            {
                activity.SetTag(SemanticConventions.AttributeNetPeerPort, uri.Port);
            }

            if (method != null)
            {
                activity.SetTag(AttributeDbMethod, method.ToString());
            }

            activity.SetTag(SemanticConventions.AttributeUrlFull, uri.OriginalString);

            try
            {
                this.options.Enrich?.Invoke(activity, "OnStartActivity", payload);
            }
            catch (Exception ex)
            {
                ElasticsearchInstrumentationEventSource.Log.EnrichmentException(ex);
            }
        }
    }

    private void OnStopActivity(Activity activity, object? payload)
    {
        if (activity.IsAllDataRequested)
        {
            var statusCode = this.httpStatusFetcher.Fetch(payload);
            activity.SetStatus(SpanHelper.ResolveActivityStatusForHttpStatusCode(activity.Kind, statusCode.GetValueOrDefault()));

            if (statusCode.HasValue)
            {
                activity.SetTag(SemanticConventions.AttributeHttpStatusCode, (int)statusCode);
            }

            var debugInformation = this.debugInformationFetcher.Fetch(payload);
            if (debugInformation != null && this.options.SetDbStatementForRequest)
            {
                var dbStatement = this.ParseAndFormatRequest(debugInformation);
                if (this.options.MaxDbStatementLength > 0 && dbStatement.Length > this.options.MaxDbStatementLength)
                {
                    dbStatement = dbStatement.Substring(0, this.options.MaxDbStatementLength);
                }

                activity.SetTag(SemanticConventions.AttributeDbStatement, dbStatement);
            }

            var originalException = this.originalExceptionFetcher.Fetch(payload);
            if (originalException != null)
            {
                activity.SetCustomProperty(ExceptionCustomPropertyName, originalException);

                var failureReason = this.failureReasonFetcher.Fetch(originalException);
                if (failureReason != null)
                {
                    activity.SetStatus(ActivityStatusCode.Error, description: $"{failureReason} {originalException.Message}");
                }

                var responseBody = this.responseBodyFetcher.Fetch(payload);
                if (responseBody != null && responseBody.Length > 0)
                {
                    var response = Encoding.UTF8.GetString(responseBody);
                    activity.SetStatus(ActivityStatusCode.Error, description: $"{failureReason} {originalException.Message}\r\n{response}");
                }

                if (originalException is HttpRequestException)
                {
                    if (originalException.InnerException is SocketException { SocketErrorCode: SocketError.HostNotFound })
                    {
                        activity.SetStatus(ActivityStatusCode.Error, description: originalException.Message);
                        return;
                    }

                    if (originalException.InnerException != null)
                    {
                        activity.SetStatus(ActivityStatusCode.Error, description: originalException.Message);
                    }
                }
            }

            try
            {
                this.options.Enrich?.Invoke(activity, "OnStopActivity", payload);
            }
            catch (Exception ex)
            {
                ElasticsearchInstrumentationEventSource.Log.EnrichmentException(ex);
            }
        }
    }
}
