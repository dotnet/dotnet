// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

// The VSCode OmniSharp client starts the RazorServer before all of its handlers are registered
// because of this we need to wait until everthing is initialized to make some client requests.
// This class takes a TCS which will complete when everything is initialized
// ensuring that no requests are sent before the client is ready.
internal abstract class ClientNotifierServiceBase : IOnInitialized
{
    public abstract Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken);

    public abstract Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken);

    public abstract Task SendNotificationAsync(string method, CancellationToken cancellationToken);

    public abstract Task OnInitializedAsync(VSInternalClientCapabilities clientCapabilities, CancellationToken cancellationToken);
}
