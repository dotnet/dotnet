// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.VisualStudio.Composition;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Test.Cohost;

/// <summary>
/// An implementation of IRemoteServiceProvider that doesn't actually do anything remote, but rather directly calls service methods
/// </summary>
internal class TestRemoteServiceProvider(ExportProvider exportProvider) : IRemoteServiceProvider, IDisposable
{
    private readonly TestServiceBroker _testServiceBroker = new TestServiceBroker();
    private readonly Dictionary<Type, IDisposable> _services = new Dictionary<Type, IDisposable>();

    private TService GetOrCreateService<TService>()
        where TService : class, IDisposable
    {
        if (!_services.TryGetValue(typeof(TService), out var service))
        {
            var factory = ServiceFactoryMap.GetServiceFactory<TService>();
            service = factory.GetTestAccessor().CreateService(_testServiceBroker, exportProvider);
            _services.Add(typeof(TService), service);
        }

        return (TService)service;
    }

    public async ValueTask<TResult?> TryInvokeAsync<TService, TResult>(
        Solution solution,
        Func<TService, RazorPinnedSolutionInfoWrapper, CancellationToken, ValueTask<TResult>> invocation,
        CancellationToken cancellationToken,
        [CallerFilePath] string? callerFilePath = null,
        [CallerMemberName] string? callerMemberName = null)
        where TService : class, IDisposable
    {
        var service = GetOrCreateService<TService>();

        // In an ideal world we'd be able to maintain a dictionary of solution checksums in TestServiceBroker, and use
        // the RazorPinnedSolutionInfoWrapper properly, but we need Roslyn changes for that. For now, this works fine
        // as we don't have any code that makes multiple parallel calls to TryInvokeAsync in the same test.
        var solutionInfo = new RazorPinnedSolutionInfoWrapper();
        _testServiceBroker.UpdateSolution(solution);
        return await invocation(service, solutionInfo, cancellationToken);
    }

    public void Dispose()
    {
        foreach (var service in _services.Values)
        {
            service.Dispose();
        }
    }
}
