// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

[RazorLanguageServerEndpoint(Methods.InitializedName)]
internal class RazorInitializedEndpoint : INotificationHandler<InitializedParams, RazorRequestContext>
{
    public bool MutatesSolutionState { get; } = true;

    public async Task HandleNotificationAsync(InitializedParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var onStartedItems = requestContext.LspServices.GetRequiredServices<IOnInitialized>();

        foreach (var onStartedItem in onStartedItems)
        {
            await onStartedItem.OnInitializedAsync(requestContext.LspServices, cancellationToken).ConfigureAwait(false);
        }
    }
}
