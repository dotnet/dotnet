// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
#if NET
using Microsoft.AspNetCore.Http.Metadata;
#endif
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace RouteTests.TestApplication;

internal class RouteInfo
{
    public static RouteInfo Current { get; set; } = new();

    public string? HttpMethod { get; set; }

    public string? Path { get; set; }

    [JsonPropertyName("RoutePattern.RawText")]
    public string? RawText { get; set; }

    [JsonPropertyName("IRouteDiagnosticsMetadata.Route")]
    public string? RouteDiagnosticMetadata { get; set; }

    [JsonPropertyName("HttpContext.GetRouteData()")]
#pragma warning disable CA2227
    public IDictionary<string, string?>? RouteData { get; set; }
#pragma warning restore CA2227

    public ActionDescriptorInfo? ActionDescriptor { get; set; }

    public void SetValues(HttpContext context)
    {
        this.HttpMethod = context.Request.Method;
        this.Path = $"{context.Request.Path}{context.Request.QueryString}";
        var endpoint = context.GetEndpoint();
        this.RawText = (endpoint as RouteEndpoint)?.RoutePattern.RawText;
#if NET
        this.RouteDiagnosticMetadata = endpoint?.Metadata.GetMetadata<IRouteDiagnosticsMetadata>()?.Route;
#endif
        this.RouteData = new Dictionary<string, string?>();
        foreach (var value in context.GetRouteData().Values)
        {
            this.RouteData[value.Key] = value.Value?.ToString();
        }
    }

    public void SetValues(ActionDescriptor actionDescriptor)
    {
        this.ActionDescriptor ??= new ActionDescriptorInfo(actionDescriptor);
    }
}
