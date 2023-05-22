// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

[LanguageServerEndpoint(Methods.InitializedName)]
internal class RazorInitializedEndpoint : INotificationHandler<InitializedParams, RazorRequestContext>
{
    public bool MutatesSolutionState { get; } = true;

    public async Task HandleNotificationAsync(InitializedParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var onStartedItems = requestContext.LspServices.GetRequiredServices<IOnInitialized>();
        var capabilitiesManager = requestContext.GetRequiredService<IInitializeManager<InitializeParams, InitializeResult>>();

        var fileChangeDetectorManager = requestContext.LspServices.GetRequiredService<RazorFileChangeDetectorManager>();
        await fileChangeDetectorManager.InitializedAsync().ConfigureAwait(false);
        var clientCapabilities = capabilitiesManager.GetInitializeParams().Capabilities.ToVSInternalClientCapabilities();

        foreach (var onStartedItem in onStartedItems)
        {
            await onStartedItem.OnInitializedAsync(clientCapabilities, cancellationToken).ConfigureAwait(false);
        }
    }
}
