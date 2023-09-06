// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class RazorConfigurationEndpoint : IDidChangeConfigurationEndpoint, IOnInitialized
{
    private readonly RazorLSPOptionsMonitor _optionsMonitor;

    public RazorConfigurationEndpoint(RazorLSPOptionsMonitor optionsMonitor)
    {
        if (optionsMonitor is null)
        {
            throw new ArgumentNullException(nameof(optionsMonitor));
        }

        _optionsMonitor = optionsMonitor;
    }

    public bool MutatesSolutionState => true;

    public async Task HandleNotificationAsync(DidChangeConfigurationParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        requestContext.Logger.LogInformation("Settings changed. Updating the server.");

        await _optionsMonitor.UpdateAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task OnInitializedAsync(VSInternalClientCapabilities clientCapabilities, CancellationToken cancellationToken)
    {
        if (clientCapabilities.Workspace?.Configuration == true)
        {
            await _optionsMonitor.UpdateAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
