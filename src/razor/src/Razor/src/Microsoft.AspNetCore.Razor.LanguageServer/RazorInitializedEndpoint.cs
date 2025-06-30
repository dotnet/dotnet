// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
            await onStartedItem.OnInitializedAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
