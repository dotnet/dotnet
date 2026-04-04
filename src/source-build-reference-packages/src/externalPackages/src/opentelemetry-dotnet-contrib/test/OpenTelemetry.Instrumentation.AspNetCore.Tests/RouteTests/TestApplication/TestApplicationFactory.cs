// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace RouteTests.TestApplication;

public enum TestApplicationScenario
{
    /// <summary>
    /// An application that uses conventional routing.
    /// </summary>
    ConventionalRouting,

    /// <summary>
    /// An application that uses attribute routing.
    /// </summary>
    AttributeRouting,

    /// <summary>
    /// A Minimal API application.
    /// </summary>
    MinimalApi,

    /// <summary>
    /// An Razor Pages application.
    /// </summary>
    RazorPages,

    /// <summary>
    /// Application with Exception Handling Middleware.
    /// </summary>
    ExceptionMiddleware,
}

internal class TestApplicationFactory
{
    private static readonly string RouteTestsPath =
        typeof(TestApplicationFactory).Assembly
        .GetCustomAttributes()
        .OfType<AssemblyMetadataAttribute>()
        .FirstOrDefault((p) => p.Key is "RouteTestsPath")?.Value ?? ".";

    private static readonly string ContentRootPath = Path.Combine(RouteTestsPath, "TestApplication");

    public static WebApplication? CreateApplication(TestApplicationScenario config)
    {
        Debug.Assert(Directory.Exists(ContentRootPath), $"Cannot find ContentRootPath: {ContentRootPath}");
        return config switch
        {
            TestApplicationScenario.ConventionalRouting => CreateConventionalRoutingApplication(),
            TestApplicationScenario.AttributeRouting => CreateAttributeRoutingApplication(),
            TestApplicationScenario.MinimalApi => CreateMinimalApiApplication(),
            TestApplicationScenario.RazorPages => CreateRazorPagesApplication(),
            TestApplicationScenario.ExceptionMiddleware => CreateExceptionHandlerApplication(),
            _ => throw new ArgumentException($"Invalid {nameof(TestApplicationScenario)}"),
        };
    }

    private static WebApplication CreateConventionalRoutingApplication()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { ContentRootPath = ContentRootPath });
        builder.Logging.ClearProviders();

        builder.Services
            .AddControllersWithViews()
            .AddApplicationPart(typeof(RoutingTests).Assembly);

        var app = builder.Build();
        app.Urls.Clear();
        app.Urls.Add("http://[::1]:0");
        app.UseStaticFiles();
        app.UseRouting();

        app.MapAreaControllerRoute(
            name: "AnotherArea",
            areaName: "AnotherArea",
            pattern: "SomePrefix/{controller=AnotherArea}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "MyArea",
            pattern: "{area:exists}/{controller=ControllerForMyArea}/{action=Default}/{id?}");

        app.MapControllerRoute(
            name: "FixedRouteWithConstraints",
            pattern: "SomePath/{id}/{num:int}",
            defaults: new { controller = "ConventionalRoute", action = "ActionWithStringParameter" });

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=ConventionalRoute}/{action=Default}/{id?}");

        return app;
    }

    private static WebApplication CreateAttributeRoutingApplication()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Logging.ClearProviders();

        builder.Services
            .AddControllers()
            .AddApplicationPart(typeof(RoutingTests).Assembly);

        var app = builder.Build();
        app.Urls.Clear();
        app.Urls.Add("http://[::1]:0");
        app.MapControllers();

        return app;
    }

    private static WebApplication CreateMinimalApiApplication()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Logging.ClearProviders();

        var app = builder.Build();
        app.Urls.Clear();
        app.Urls.Add("http://[::1]:0");

        app.MapGet("/MinimalApi", () => Results.Ok());
        app.MapGet("/MinimalApi/{id}", (int id) => Results.Ok());

#if NET
        var api = app.MapGroup("/MinimalApiUsingMapGroup");
        api.MapGet("/", () => Results.Ok());
        api.MapGet("/{id}", (int id) => Results.Ok());
#endif

        return app;
    }

    private static WebApplication CreateRazorPagesApplication()
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions { ContentRootPath = ContentRootPath });
        builder.Logging.ClearProviders();

        builder.Services
            .AddRazorPages()
            .AddRazorRuntimeCompilation(options =>
            {
                options.FileProviders.Add(new PhysicalFileProvider(ContentRootPath));
            })
            .AddApplicationPart(typeof(RoutingTests).Assembly);

        var app = builder.Build();
        app.Urls.Clear();
        app.Urls.Add("http://[::1]:0");
        app.UseStaticFiles();
        app.UseRouting();
        app.MapRazorPages();

        return app;
    }

    private static WebApplication CreateExceptionHandlerApplication()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Logging.ClearProviders();

        var app = builder.Build();

        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                await context.Response.WriteAsync(exceptionHandlerPathFeature?.Error.Message ?? "An exception was thrown.");
            });
        });

        app.Urls.Clear();
        app.Urls.Add("http://[::1]:0");

        app.MapGet("/Exception", () => Results.Content(content: "Error", contentType: null, contentEncoding: null, statusCode: 500));

        return app;
    }
}
