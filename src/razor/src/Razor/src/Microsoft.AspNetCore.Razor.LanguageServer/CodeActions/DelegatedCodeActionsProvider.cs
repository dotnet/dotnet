// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using StreamJsonRpc;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal sealed class DelegatedCodeActionsProvider(
    IClientConnection clientConnection,
    ITelemetryReporter telemetryReporter,
    ILoggerFactory loggerFactory) : IDelegatedCodeActionsProvider
{
    private readonly IClientConnection _clientConnection = clientConnection;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<DelegatedCodeActionsProvider>();

    public async Task<RazorVSInternalCodeAction[]> GetDelegatedCodeActionsAsync(RazorLanguageKind languageKind, VSCodeActionParams request, int hostDocumentVersion, Guid correlationId, CancellationToken cancellationToken)
    {
        var delegatedParams = new DelegatedCodeActionParams()
        {
            HostDocumentVersion = hostDocumentVersion,
            CodeActionParams = request,
            LanguageKind = languageKind,
            CorrelationId = correlationId
        };

        try
        {
            return await _clientConnection.SendRequestAsync<DelegatedCodeActionParams, RazorVSInternalCodeAction[]>(CustomMessageNames.RazorProvideCodeActionsEndpoint, delegatedParams, cancellationToken).ConfigureAwait(false);
        }
        catch (RemoteInvocationException e)
        {
            _telemetryReporter.ReportFault(e, "Error getting code actions from delegate language server for {languageKind}", languageKind);
            _logger.LogError(e, $"Error getting code actions from delegate language server for {languageKind}");
            return [];
        }
    }
}
