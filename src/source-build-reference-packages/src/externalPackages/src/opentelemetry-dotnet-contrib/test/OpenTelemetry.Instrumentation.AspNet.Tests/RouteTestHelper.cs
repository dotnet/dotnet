// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Net.Http;
using System.Web;
using System.Web.Routing;

namespace OpenTelemetry.Instrumentation.AspNet.Tests;

internal static class RouteTestHelper
{
    private const string DefaultController = "Home";
    private const string DefaultAction = "Index";

    public static HttpContext BuildHttpContext(string url, int routeType, string? routeTemplate, string requestMethod)
    {
        RouteData routeData;
        bool isMvcRoute = routeType is 1 or 2;
        switch (routeType)
        {
            case 0: // WebForm, no route data.
                routeData = new RouteData();
                break;
            case 1: // Traditional MVC.
            case 2: // Attribute routing MVC.
            case 3: // Traditional WebAPI.

                var routeDefaults = new RouteValueDictionary()
                {
                    // This marks id parameter as optional.
                    // Parameter name should match with the route template used in tests.
                    ["id"] = new { },
                };
                routeData = new RouteData
                {
                    Route = new Route(routeTemplate, routeDefaults, null),
                };
                AddRouteValueTokens(routeData, url, routeTemplate, requestMethod, isMvcRoute);
                break;
            case 4: // Attribute routing WebAPI.
                routeData = new RouteData();
                var value = new[]
                {
                    new
                    {
                        Route = new
                        {
                            Defaults = new RouteValueDictionary(),
                            RouteTemplate = routeTemplate,
                        },
                    },
                };
                routeData.Values.Add(
                    "MS_SubRoutes",
                    value);
                break;
            case 5: // Multi-method attribute routing WebAPI.
                routeData = new RouteData();
                var httpMethods = new[] { HttpMethod.Get, HttpMethod.Put, HttpMethod.Delete };

                var multiMethodSubroutes = httpMethods.Select(method => new
                {
                    Route = new
                    {
                        RouteTemplate = routeTemplate,
                        Defaults = new RouteValueDictionary(),
                        DataTokens = new Dictionary<string, object>
                        {
                            ["actions"] = new[]
                            {
                                new
                                {
                                    SupportedHttpMethods = new[] { method },
                                },
                            },
                        },
                    },
                }).ToArray();

                routeData.Values.Add(
                    "MS_SubRoutes",
                    multiMethodSubroutes);
                break;
            default:
                throw new NotSupportedException();
        }

        var request = new HttpRequest(string.Empty, url, string.Empty)
        {
            RequestContext = new RequestContext
            {
                RouteData = routeData,
            },
        };

        typeof(HttpRequest).GetField("_httpMethod", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(request, requestMethod);

        return new HttpContext(request, new HttpResponse(new StringWriter()));
    }

    // Simulates the behavior of ASP.NET routing engine that fills in route parameters.
    private static void AddRouteValueTokens(RouteData data, string url, string? template, string requestMethod, bool isMvcRoute)
    {
        if (string.IsNullOrWhiteSpace(template))
        {
            return;
        }

        var uri = new Uri(url);
        var pathSegments = uri.AbsolutePath.Trim('/').Split(['/'], StringSplitOptions.RemoveEmptyEntries);
        var templateSegments = template!.Trim('/').Split(['/'], StringSplitOptions.RemoveEmptyEntries);

        int pathIndex = 0;

        // start with defaults
        var values = new RouteValueDictionary
        {
            ["controller"] = DefaultController,
            ["action"] = isMvcRoute
                ? DefaultAction
                : requestMethod, // WebAPI: Default action is the HTTP method, if executed without action in the route template.
        };

        foreach (var segment in templateSegments)
        {
            // parameter segment
            if (segment.StartsWith("{", StringComparison.InvariantCultureIgnoreCase) &&
                segment.EndsWith("}", StringComparison.InvariantCultureIgnoreCase))
            {
                var key = segment.Trim('{', '}');

                if (pathIndex < pathSegments.Length)
                {
                    values[key] = pathSegments[pathIndex];
                    pathIndex++;
                }
                else if (!values.ContainsKey(key))
                {
                    // No URL value and no default provided -> null
                    continue;
                }
            }

            // literal segment
            else
            {
                if (pathIndex < pathSegments.Length &&
                    string.Equals(pathSegments[pathIndex], segment, StringComparison.OrdinalIgnoreCase))
                {
                    pathIndex++;
                }
                else
                {
                    // mismatch, bail out (route doesn't match)
                    return;
                }
            }
        }

        // copy values into RouteData
        foreach (var kvp in values)
        {
            data.Values[kvp.Key] = kvp.Value;
        }
    }
}
