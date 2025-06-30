// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class LspServices : ILspServices
{
    private sealed class EmptyServiceProvider : IServiceProvider
    {
        public object? GetService(Type serviceType) => null;
    }

    public static LspServices Empty { get; } = new(new EmptyServiceProvider());

    private readonly IServiceProvider _serviceProvider;

    private readonly object _disposeLock = new();
    private bool _isDisposed = false;

    public bool IsDisposed => _isDisposed;

    public LspServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ILspServices>(this);
        serviceCollection.AddSingleton<LspServices>(this);

        _serviceProvider = serviceCollection.BuildServiceProvider();

        // By requesting the startup services, we ensure that they are created.
        // This gives them an opportunity to set up any necessary state or perform.
        _ = _serviceProvider.GetServices<IRazorStartupService>();
    }

    private LspServices(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Dispose()
    {
        lock (_disposeLock)
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
        }

        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
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

    public T? GetService<T>() where T : notnull
        => _serviceProvider.GetService<T>();

    public bool TryGetService(Type type, [NotNullWhen(true)] out object? service)
    {
        service = _serviceProvider.GetService(type);

        return service is not null;
    }
}
