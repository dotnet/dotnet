// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.Instrumentation.Owin;

namespace Owin;

/// <summary>
/// Provides extension methods for the <see cref="IAppBuilder"/> class.
/// </summary>
public static class AppBuilderExtensions
{
    /// <summary>Adds a component to the OWIN pipeline for instrumenting
    /// incoming request with <see cref="Activity"/> and notifying listeners
    /// with <see cref="ActivitySource"/>.</summary>
    /// <param name="appBuilder"><see cref="IAppBuilder"/>.</param>
    /// <returns>Supplied <see cref="IAppBuilder"/> for chaining.</returns>
    public static IAppBuilder UseOpenTelemetry(this IAppBuilder appBuilder)
        => appBuilder.Use<DiagnosticsMiddleware>();
}
