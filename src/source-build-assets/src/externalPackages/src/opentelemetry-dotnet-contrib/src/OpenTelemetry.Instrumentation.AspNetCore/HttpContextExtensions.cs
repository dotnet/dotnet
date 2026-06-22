// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NET

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Routing;

namespace OpenTelemetry.Instrumentation.AspNetCore;

// Adapted from the following ASP.NET Core code:
// - https://github.com/dotnet/aspnetcore/blob/1855c53140e5254d54078ccea41e2b21bd264309/src/Hosting/Hosting/src/Internal/HostingApplicationDiagnostics.cs#L533-L536
// - https://github.com/dotnet/aspnetcore/blob/1855c53140e5254d54078ccea41e2b21bd264309/src/Shared/HttpExtensions.cs#L36-L47
// - https://github.com/dotnet/aspnetcore/blob/1855c53140e5254d54078ccea41e2b21bd264309/src/Shared/Diagnostics/RouteDiagnosticsHelpers.cs#L8-L16.

internal static class HttpContextExtensions
{
    public static string? GetHttpRoute(this HttpContext context)
    {
        var endpoint = GetOriginalEndpoint(context);
        var route = endpoint?.Metadata.GetMetadata<IRouteDiagnosticsMetadata>()?.Route;

#if !NET11_0_OR_GREATER
        if (string.IsNullOrEmpty(route))
        {
            // For Razor Pages, the route template may be empty (e.g., for the Index page mapped to root "/").
            // Fall back to the "page" route value which contains the page name (e.g., "/Index").
            var pageValue = context.GetRouteValue("page") as string;
            if (!string.IsNullOrEmpty(pageValue))
            {
                return pageValue;
            }
        }
#endif

        return route is not null ? ResolveHttpRoute(route) : null;
    }

    private static Endpoint? GetOriginalEndpoint(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        // Some middleware re-execute the middleware pipeline with the HttpContext. Before they do this, they clear state from context, such as the previously matched endpoint.
        // The original endpoint is stashed with a known key in HttpContext.Items. Use it as a fallback.
        if (endpoint is null && context.Items.TryGetValue("__OriginalEndpoint", out var e) && e is Endpoint originalEndpoint)
        {
            endpoint = originalEndpoint;
        }

        return endpoint;
    }

    private static string ResolveHttpRoute(string route)
        => string.IsNullOrEmpty(route) ? "/" : route;
}
#endif
