// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace OpenTelemetry.Instrumentation.AspNet.Implementation;

/// <summary>
/// Helper class for processing http requests.
/// </summary>
internal sealed class HttpRequestRouteHelper
{
    private readonly PropertyFetcher<object> routeFetcher = new("Route");
    private readonly PropertyFetcher<string> routeTemplateFetcher = new("RouteTemplate");

    /// <summary>
    /// Extracts the route template from the <see cref="HttpRequest"/>.
    /// </summary>
    /// <param name="request">The <see cref="HttpRequest"/> being processed.</param>
    /// <returns>The route template or <see langword="null"/>.</returns>
    internal string? GetRouteTemplate(HttpRequestBase request)
    {
        var routeData = request.RequestContext.RouteData;

        string? template = null;
        if (routeData.Values.TryGetValue("MS_SubRoutes", out var msSubRoutes))
        {
            // WebAPI attribute routing flows here. Use reflection to not take a dependency on microsoft.aspnet.webapi.core\[version]\lib\[framework]\System.Web.Http.

            if (msSubRoutes is Array { Length: >= 1 } attributeRouting)
            {
                // There could be more than one subroute, each with a different method.
                // But the template is the same across them, so we simply take the template
                // from the first route.
                var subRouteData = attributeRouting.GetValue(0);

                _ = this.routeFetcher.TryFetch(subRouteData, out var route);
                _ = this.routeTemplateFetcher.TryFetch(route, out template);
            }
        }
        else if (routeData.Route is Route route)
        {
            // MVC + WebAPI traditional routing & MVC attribute routing flow here.
            template = PrepareRouteTemplate(route, routeData);
        }

        return template;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool CompareStringToSubstring(string example, string target, int start)
    {
        for (int i = 0; i < example.Length; i++)
        {
            if (target[start + 1 + i] != example[i])
            {
                return false;
            }
        }

        return true;
    }

    private static string PrepareRouteTemplate(Route route, RouteData routeData)
    {
        const string controllerToken = "controller";
        const string actionToken = "action";

        var template = route.Url;
        var controller = (string)routeData.Values[controllerToken];
        var action = (string)routeData.Values[actionToken];
        var hasController = !string.IsNullOrWhiteSpace(controller);
        var hasAction = !string.IsNullOrWhiteSpace(action);
        var sb = new StringBuilder(template.Length);

        int i = 0;
        while (i < template.Length)
        {
            if (template[i] == '{')
            {
                int end = template.IndexOf('}', i + 1);
                if (end != -1)
                {
                    if (hasController && CompareStringToSubstring(controllerToken, template, i))
                    {
                        sb.Append(controller);
                    }
                    else if (hasAction && CompareStringToSubstring(actionToken, template, i))
                    {
                        sb.Append(action);
                    }
                    else
                    {
                        var defaults = route.Defaults;
                        var values = routeData.Values;
                        var token = template.Substring(i + 1, end - i - 1);

                        if (defaults.ContainsKey(token) && !values.ContainsKey(token))
                        {
                            // Ignore defaults with no values.
                        }
                        else
                        {
                            sb.Append('{').Append(token).Append('}');
                        }
                    }

                    i = end + 1;
                    continue;
                }
            }

            sb.Append(template[i]);
            i++;
        }

        // Normalizes endings by removing trailing slashes.
        int len = sb.Length;
        while (len > 0 && sb[len - 1] == '/')
        {
            len--;
        }

        return sb.ToString(0, len);
    }
}
