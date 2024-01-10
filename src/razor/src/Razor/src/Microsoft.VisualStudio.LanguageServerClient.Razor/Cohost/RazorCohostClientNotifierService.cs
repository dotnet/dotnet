// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Cohost;

internal sealed class RazorCohostClientConnection(IRazorCohostClientLanguageServerManager clientNotifier) : IClientConnection
{
    private readonly IRazorCohostClientLanguageServerManager _clientNotifier = clientNotifier;

    public Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
        => _clientNotifier.SendNotificationAsync<TParams>(method, @params, cancellationToken).AsTask();

    public Task SendNotificationAsync(string method, CancellationToken cancellationToken)
        => _clientNotifier.SendNotificationAsync(method, cancellationToken).AsTask();

    public Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
        => _clientNotifier.SendRequestAsync<TParams, TResponse>(method, @params, cancellationToken);
}
