// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using StreamJsonRpc;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

// We need to wait until everything is initialized before we make any client requests, so
// this class takes a TCS which will complete when everything is initialized.
internal sealed class ClientConnection(JsonRpc jsonRpc) : IClientConnection, IOnInitialized
{
    private readonly JsonRpc _jsonRpc = jsonRpc ?? throw new ArgumentNullException(nameof(jsonRpc));
    private readonly TaskCompletionSource<bool> _initializedCompletionSource = new TaskCompletionSource<bool>();

    public async Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
    {
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
        await _initializedCompletionSource.Task.ConfigureAwait(false);
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks

        return await _jsonRpc.InvokeWithParameterObjectAsync<TResponse>(method, @params, cancellationToken).ConfigureAwait(false);
    }

    public async Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
    {
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
        await _initializedCompletionSource.Task.ConfigureAwait(false);
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks

        await _jsonRpc.NotifyWithParameterObjectAsync(method, @params).ConfigureAwait(false);
    }

    public async Task SendNotificationAsync(string method, CancellationToken cancellationToken)
    {
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
        await _initializedCompletionSource.Task.ConfigureAwait(false);
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks

        await _jsonRpc.NotifyAsync(method).ConfigureAwait(false);
    }

    /// <summary>
    /// Fires when the language server is set to "Started".
    /// </summary>
    public Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        _initializedCompletionSource.TrySetResult(true);
        return Task.CompletedTask;
    }
}
