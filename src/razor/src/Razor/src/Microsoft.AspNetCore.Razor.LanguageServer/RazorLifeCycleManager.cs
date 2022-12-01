// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class RazorLifeCycleManager : ILifeCycleManager
{
    private readonly RazorLanguageServer _languageServer;
    private readonly TaskCompletionSource<int> _tcs = new TaskCompletionSource<int>();

    public RazorLifeCycleManager(RazorLanguageServer languageServer)
    {
        _languageServer = languageServer;
    }

    public Task ExitAsync()
    {
        var services = _languageServer.GetLspServices();
        services.Dispose();
        _tcs.TrySetResult(0);

        return Task.CompletedTask;
    }

    public Task ShutdownAsync(string message = "Shutting down")
    {
        return Task.CompletedTask;
    }

    public Task WaitForExit => _tcs.Task;
}
