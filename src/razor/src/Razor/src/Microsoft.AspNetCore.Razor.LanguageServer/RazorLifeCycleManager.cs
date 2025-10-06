// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class RazorLifeCycleManager(RazorLanguageServer languageServer, ILspServerActivationTracker? lspServerActivationTracker) : ILifeCycleManager
{
    private readonly RazorLanguageServer _languageServer = languageServer;
    private readonly ILspServerActivationTracker? _lspServerActivationTracker = lspServerActivationTracker;
    private readonly TaskCompletionSource<int> _tcs = new TaskCompletionSource<int>();

    public Task ExitAsync()
    {
        _lspServerActivationTracker?.Deactivated();

        var services = _languageServer.GetLspServices();
        services.Dispose();
        _tcs.TrySetResult(0);

        return Task.CompletedTask;
    }

    public Task ShutdownAsync(string message = "Shutting down")
    {
        _lspServerActivationTracker?.Deactivated();

        return Task.CompletedTask;
    }

    public Task WaitForExit => _tcs.Task;
}
