// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

internal static class IServiceCollectionExtensions
{
    public static void AddHandlerWithCapabilities<T>(this IServiceCollection services)
        where T : class, IMethodHandler, ICapabilitiesProvider
    {
        services.AddSingleton<T>();
        services.AddSingleton<IMethodHandler, T>(s => s.GetRequiredService<T>());
        // Transient because it should only be used once and I'm hoping it doesn't stick around.
        services.AddTransient<ICapabilitiesProvider, T>(s => s.GetRequiredService<T>());
    }

    public static void AddHandler<T>(this IServiceCollection services)
        where T : class, IMethodHandler
    {
        if (typeof(ICapabilitiesProvider).IsAssignableFrom(typeof(T)))
        {
            throw new InvalidOperationException($"{nameof(T)} is not using {nameof(AddHandlerWithCapabilities)} when it implements {nameof(ICapabilitiesProvider)}");
        }

        services.AddSingleton<IMethodHandler, T>();
    }
}
