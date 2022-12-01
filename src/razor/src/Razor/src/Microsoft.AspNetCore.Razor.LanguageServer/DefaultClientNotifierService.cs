// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

// The VSCode OmniSharp client starts the RazorServer before all of its handlers are registered
// because of this we need to wait until everthing is initialized to make some client requests.
// This class takes a TCS which will complete when everything is initialized
// ensuring that no requests are sent before the client is ready.
internal class DefaultClientNotifierService : ClientNotifierServiceBase
{
    private readonly TaskCompletionSource<bool> _initializedCompletionSource;
    private readonly StreamJsonRpc.JsonRpc _jsonRpc;

    public DefaultClientNotifierService(StreamJsonRpc.JsonRpc jsonRpc)
    {
        if (jsonRpc is null)
        {
            throw new ArgumentNullException(nameof(jsonRpc));
        }

        _jsonRpc = jsonRpc;
        _initializedCompletionSource = new TaskCompletionSource<bool>();
    }

    public override async Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
    {
        await _initializedCompletionSource.Task;
        var result = await _jsonRpc.InvokeAsync<TResponse>(method, @params);

        return result;
    }

    public override async Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
    {
        await _initializedCompletionSource.Task;

        await _jsonRpc.NotifyAsync(method, @params);
    }

    public override async Task SendNotificationAsync(string method, CancellationToken cancellationToken)
    {
        await _initializedCompletionSource.Task;

        await _jsonRpc.NotifyAsync(method);
    }

    /// <summary>
    /// Fires when the language server is set to "Started".
    /// </summary>
    /// <param name="clientCapabilities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override Task OnInitializedAsync(VSInternalClientCapabilities clientCapabilities, CancellationToken cancellationToken)
    {
        _initializedCompletionSource.TrySetResult(true);
        return Task.CompletedTask;
    }
}
