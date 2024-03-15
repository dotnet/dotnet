// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class LspServices : ILspServices
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEnumerable<IRazorStartupService> _startupServices;
    public bool IsDisposed = false;

    public LspServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ILspServices>(this);
        _serviceProvider = serviceCollection.BuildServiceProvider();

        // Create all startup services
        _startupServices = _serviceProvider.GetServices<IRazorStartupService>();
    }

    public ImmutableArray<Type> GetRegisteredServices()
    {
        throw new NotImplementedException();
    }

    public T GetRequiredService<T>() where T : notnull
    {
        return _serviceProvider.GetRequiredService<T>();
    }

    public IEnumerable<T> GetRequiredServices<T>()
    {
        var services = _serviceProvider.GetServices<T>();
        if (services is null)
        {
            throw new ArgumentNullException($"Missing services {nameof(T)}");
        }

        return services;
    }

    public object? TryGetService(Type type)
    {
        var service = _serviceProvider.GetService(type);

        return service;
    }

    public bool SupportsGetRegisteredServices()
    {
        return false;
    }

    public void Dispose()
    {
        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
            IsDisposed = true;
        }
    }
}
