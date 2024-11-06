// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Workspaces.Telemetry;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

[RazorLanguageServerEndpoint(Methods.TextDocumentCompletionName)]
internal class RazorCompletionEndpoint(
    CompletionListProvider completionListProvider,
    ITelemetryReporter? telemetryReporter,
    RazorLSPOptionsMonitor optionsMonitor)
    : IRazorRequestHandler<CompletionParams, VSInternalCompletionList?>, ICapabilitiesProvider
{
    private readonly CompletionListProvider _completionListProvider = completionListProvider;
    private readonly ITelemetryReporter? _telemetryReporter = telemetryReporter;
    private readonly RazorLSPOptionsMonitor _optionsMonitor = optionsMonitor;

    private VSInternalClientCapabilities? _clientCapabilities;

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        _clientCapabilities = clientCapabilities;

        serverCapabilities.CompletionProvider = new CompletionOptions()
        {
            ResolveProvider = true,
            TriggerCharacters = CompletionTriggerAndCommitCharacters.AllTriggerCharacters,
            AllCommitCharacters = CompletionTriggerAndCommitCharacters.AllCommitCharacters
        };
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(CompletionParams request)
    {
        return request.TextDocument;
    }

    public async Task<VSInternalCompletionList?> HandleRequestAsync(CompletionParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;

        if (request.Context is null || documentContext is null)
        {
            return null;
        }

        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        if (!sourceText.TryGetAbsoluteIndex(request.Position, out var hostDocumentIndex))
        {
            return null;
        }

        if (request.Context is not VSInternalCompletionContext completionContext)
        {
            Debug.Fail("Completion context should never be null in practice");
            return null;
        }

        var autoShownCompletion = completionContext.InvokeKind != VSInternalCompletionInvokeKind.Explicit;
        if (autoShownCompletion && !_optionsMonitor.CurrentValue.AutoShowCompletion)
        {
            return null;
        }

        var correlationId = Guid.NewGuid();
        using var _ = _telemetryReporter?.TrackLspRequest(Methods.TextDocumentCompletionName, LanguageServerConstants.RazorLanguageServerName, TelemetryThresholds.CompletionRazorTelemetryThreshold, correlationId);

        var razorCompletionOptions = new RazorCompletionOptions(
            SnippetsSupported: true,
            AutoInsertAttributeQuotes: _optionsMonitor.CurrentValue.AutoInsertAttributeQuotes,
            CommitElementsWithSpace: _optionsMonitor.CurrentValue.CommitElementsWithSpace);
        var completionList = await _completionListProvider.GetCompletionListAsync(
            hostDocumentIndex,
            completionContext,
            documentContext,
            _clientCapabilities!,
            razorCompletionOptions,
            correlationId,
            cancellationToken).ConfigureAwait(false);
        return completionList;
    }
}
