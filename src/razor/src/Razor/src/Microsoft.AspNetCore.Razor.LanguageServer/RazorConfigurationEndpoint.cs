// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class RazorConfigurationEndpoint(
    LspServices services,
    RazorLSPOptionsMonitor optionsMonitor,
    ILoggerFactory loggerFactory)
    : IDidChangeConfigurationEndpoint, IOnInitialized
{
    private readonly LspServices _services = services;
    private readonly RazorLSPOptionsMonitor _optionsMonitor = optionsMonitor;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RazorConfigurationEndpoint>();

    public bool MutatesSolutionState => true;

    public async Task HandleNotificationAsync(DidChangeConfigurationParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Settings changed. Updating the server.");

        await _optionsMonitor.UpdateAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        var capabilitiesService = _services.GetRequiredService<IClientCapabilitiesService>();
        var clientCapabilities = capabilitiesService.ClientCapabilities;

        if (clientCapabilities.Workspace?.Configuration == true)
        {
            await _optionsMonitor.UpdateAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
