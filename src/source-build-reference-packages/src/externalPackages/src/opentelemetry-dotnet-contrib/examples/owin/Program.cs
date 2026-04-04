// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Owin;

namespace Examples.Owin;

internal static class Program
{
    public static void Main()
    {
        using var host = WebApp.Start(
            "http://localhost:9000",
            appBuilder =>
            {
                // Add OpenTelemetry early in the pipeline to start timing
                // the request as soon as possible.
                appBuilder.UseOpenTelemetry();

                var config = new HttpConfiguration();

                config.MessageHandlers.Add(new ActivityDisplayNameRouteEnrichingHandler());

                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional });

                appBuilder.UseWebApi(config);
            });

        using var openTelemetry = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Owin-Example"))
            .AddOwinInstrumentation()
            .AddConsoleExporter()
            .Build();

        Console.WriteLine("Service listening. Press enter to exit.");
        Console.ReadLine();
    }

    private class ActivityDisplayNameRouteEnrichingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                var activity = Activity.Current;
                if (activity != null)
                {
                    var routeData = request.GetRouteData();
                    if (routeData != null)
                    {
                        activity.DisplayName = routeData.Route.RouteTemplate;
                    }
                }
            }
        }
    }
}
