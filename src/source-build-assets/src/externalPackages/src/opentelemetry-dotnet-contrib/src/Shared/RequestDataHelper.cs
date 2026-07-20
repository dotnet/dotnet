// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET
using System.Collections.Concurrent;
using System.Collections.Frozen;
#endif
using System.Diagnostics;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Internal;

internal sealed class RequestDataHelper
{
    private const string KnownHttpMethodsEnvironmentVariable = "OTEL_INSTRUMENTATION_HTTP_KNOWN_METHODS";

    // The value "_OTHER" is used for non-standard HTTP methods.
    // https://github.com/open-telemetry/semantic-conventions/blob/v1.23.0/docs/http/http-spans.md#common-attributes
    private const string OtherHttpMethod = "_OTHER";

    private static readonly char[] SplitChars = [','];

#if NET
    private readonly FrozenDictionary<string, string> knownHttpMethods;
#else
    private readonly Dictionary<string, string> knownHttpMethods;
#endif

#if NET
    // Caches the final display name string for each (namePrefix, httpRoute) pair.
    // The number of distinct combinations is bounded by the number of (HTTP method name prefixes * routes) in the app.
    private readonly ConcurrentDictionary<(string Method, string Route), string> displayNameCache = new();
#endif

    public RequestDataHelper(bool configureByHttpKnownMethodsEnvironmentalVariable)
    {
        var suppliedKnownMethods = configureByHttpKnownMethodsEnvironmentalVariable ? Environment.GetEnvironmentVariable(KnownHttpMethodsEnvironmentVariable)
            ?.Split(SplitChars, StringSplitOptions.RemoveEmptyEntries) : null;

        Dictionary<string, string> knownMethodSet;

        if (suppliedKnownMethods?.Length > 0)
        {
            knownMethodSet = suppliedKnownMethods.ToDictionary(x => x, x => x, StringComparer.OrdinalIgnoreCase);
        }
        else
        {
            knownMethodSet = new(StringComparer.OrdinalIgnoreCase)
            {
                // See https://github.com/open-telemetry/semantic-conventions/blob/v1.38.0/model/http/registry.yaml
                ["GET"] = "GET",
                ["POST"] = "POST",
                ["PUT"] = "PUT",
                ["DELETE"] = "DELETE",
                ["HEAD"] = "HEAD",
                ["OPTIONS"] = "OPTIONS",
                ["TRACE"] = "TRACE",
                ["PATCH"] = "PATCH",
                ["CONNECT"] = "CONNECT",
            };

            // .NET 9+ has native support for instrumentation, so our custom code doesn't run,
            // but we don't target net9.0 so we cannot use conditional compilation to light-up
            // support for QUERY and only .NET 10+ has support for QUERY itself.
            if (Environment.Version.Major is not 9)
            {
                knownMethodSet["QUERY"] = "QUERY";
            }
        }

#if NET
        this.knownHttpMethods = knownMethodSet.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
#else
        this.knownHttpMethods = knownMethodSet;
#endif
    }

    public void SetHttpMethodTag(Activity activity, string originalHttpMethod)
    {
        var normalizedHttpMethod = this.GetNormalizedHttpMethod(originalHttpMethod);
        activity.SetTag(SemanticConventions.AttributeHttpRequestMethod, normalizedHttpMethod);

        if (originalHttpMethod != normalizedHttpMethod)
        {
            activity.SetTag(SemanticConventions.AttributeHttpRequestMethodOriginal, originalHttpMethod);
        }
    }

    public void SetActivityDisplayNameAndHttpMethodTag(Activity activity, string originalHttpMethod)
    {
        var normalizedHttpMethod = this.GetNormalizedHttpMethod(originalHttpMethod);

        activity.DisplayName = normalizedHttpMethod == OtherHttpMethod ? "HTTP" : normalizedHttpMethod;
        activity.SetTag(SemanticConventions.AttributeHttpRequestMethod, normalizedHttpMethod);

        if (originalHttpMethod != normalizedHttpMethod)
        {
            activity.SetTag(SemanticConventions.AttributeHttpRequestMethodOriginal, originalHttpMethod);
        }
    }

    public void SetHttpMethodTag(ref TagList tags, string originalHttpMethod)
    {
        var normalizedHttpMethod = this.GetNormalizedHttpMethod(originalHttpMethod);
        tags.Add(SemanticConventions.AttributeHttpRequestMethod, normalizedHttpMethod);

        if (originalHttpMethod != normalizedHttpMethod)
        {
            tags.Add(SemanticConventions.AttributeHttpRequestMethodOriginal, originalHttpMethod);
        }
    }

    public string GetNormalizedHttpMethod(string method)
        => this.knownHttpMethods.TryGetValue(method, out var normalizedMethod)
            ? normalizedMethod
            : OtherHttpMethod;

    public void SetActivityDisplayName(Activity activity, string originalHttpMethod, string? httpRoute = null)
        => activity.DisplayName = this.GetActivityDisplayName(originalHttpMethod, httpRoute);

    public string GetActivityDisplayName(string originalHttpMethod, string? httpRoute = null)
    {
        // https://github.com/open-telemetry/semantic-conventions/blob/v1.24.0/docs/http/http-spans.md#name

        var normalizedHttpMethod = this.GetNormalizedHttpMethod(originalHttpMethod);
        var namePrefix = normalizedHttpMethod == OtherHttpMethod ? "HTTP" : normalizedHttpMethod;

        if (string.IsNullOrEmpty(httpRoute))
        {
            return namePrefix;
        }

#if NET
        return this.displayNameCache.GetOrAdd((namePrefix, httpRoute), static kv => $"{kv.Method} {kv.Route}");
#else
        return $"{namePrefix} {httpRoute}";
#endif
    }

    internal static string GetHttpProtocolVersion(Version httpVersion) => httpVersion switch
    {
        { Major: 1, Minor: 0 } => "1.0",
        { Major: 1, Minor: 1 } => "1.1",
        { Major: 2, Minor: 0 } => "2",
        { Major: 3, Minor: 0 } => "3",
        _ => httpVersion.ToString(),
    };

    internal static string GetHttpProtocolVersion(string protocol) => protocol switch
    {
        "HTTP/1.1" => "1.1",
        "HTTP/2" => "2",
        "HTTP/3" => "3",
        _ => protocol,
    };
}
