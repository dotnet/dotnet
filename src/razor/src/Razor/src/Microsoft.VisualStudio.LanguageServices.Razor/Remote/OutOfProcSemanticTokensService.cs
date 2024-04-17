// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Editor.Razor.Settings;

namespace Microsoft.CodeAnalysis.Remote.Razor;

[Export(typeof(IOutOfProcSemanticTokensService))]
[method: ImportingConstructor]
internal class OutOfProcSemanticTokensService(IRemoteClientProvider remoteClientProvider, IClientSettingsManager clientSettingsManager, IRazorLoggerFactory loggerFactory) : IOutOfProcSemanticTokensService
{
    private readonly IRemoteClientProvider _remoteClientProvider = remoteClientProvider;
    private readonly IClientSettingsManager _clientSettingsManager = clientSettingsManager;
    private readonly ILogger _logger = loggerFactory.CreateLogger<OutOfProcSemanticTokensService>();

    public async ValueTask<int[]?> GetSemanticTokensDataAsync(TextDocument razorDocument, LinePositionSpan span, Guid correlationId, CancellationToken cancellationToken)
    {
        // We're being overly defensive here because the OOP host can return null for the client/session/operation
        // when it's disconnected (user stops the process).
        //
        // This will change in the future to an easier to consume API but for VS RTM this is what we have.
        var remoteClient = await _remoteClientProvider.TryGetClientAsync(cancellationToken).ConfigureAwait(false);

        if (remoteClient is null)
        {
            _logger.LogWarning("Couldn't get remote client");
            // Could not resolve
            return null;
        }

        try
        {
            var colorBackground = _clientSettingsManager.GetClientSettings().AdvancedSettings.ColorBackground;

            var data = await remoteClient.TryInvokeAsync<IRemoteSemanticTokensService, int[]?>(
                razorDocument.Project.Solution,
                (service, solutionInfo, cancellationToken) => service.GetSemanticTokensDataAsync(solutionInfo, razorDocument.Id, span, colorBackground, correlationId, cancellationToken),
                cancellationToken).ConfigureAwait(false);

            if (!data.HasValue)
            {
                return null;
            }

            return data.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling remote");
            return null;
        }
    }
}
