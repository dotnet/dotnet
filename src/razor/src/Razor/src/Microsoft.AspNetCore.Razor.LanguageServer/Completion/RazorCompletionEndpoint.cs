// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

[RazorLanguageServerEndpoint(Methods.TextDocumentCompletionName)]
internal class RazorCompletionEndpoint(
    CompletionListProvider completionListProvider,
    CompletionTriggerAndCommitCharacters triggerAndCommitCharacters,
    ITelemetryReporter telemetryReporter,
    RazorLSPOptionsMonitor optionsMonitor)
    : IRazorRequestHandler<CompletionParams, RazorVSInternalCompletionList?>, ICapabilitiesProvider
{
    private readonly CompletionListProvider _completionListProvider = completionListProvider;
    private readonly CompletionTriggerAndCommitCharacters _triggerAndCommitCharacters = triggerAndCommitCharacters;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;
    private readonly RazorLSPOptionsMonitor _optionsMonitor = optionsMonitor;

    private VSInternalClientCapabilities? _clientCapabilities;

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        _clientCapabilities = clientCapabilities;

        serverCapabilities.CompletionProvider = new CompletionOptions()
        {
            ResolveProvider = true,
            TriggerCharacters = [.. _triggerAndCommitCharacters.AllTriggerCharacters],
            AllCommitCharacters = [.. _triggerAndCommitCharacters.AllCommitCharacters]
        };
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(CompletionParams request)
    {
        return request.TextDocument;
    }

    public async Task<RazorVSInternalCompletionList?> HandleRequestAsync(CompletionParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        if (request.Context is not VSInternalCompletionContext completionContext ||
            requestContext.DocumentContext is not { } documentContext)
        {
            return null;
        }

        var autoShownCompletion = completionContext.InvokeKind != VSInternalCompletionInvokeKind.Explicit;
        var options = _optionsMonitor.CurrentValue;
        if (autoShownCompletion && !options.AutoShowCompletion)
        {
            return null;
        }

        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        if (!sourceText.TryGetAbsoluteIndex(request.Position, out var hostDocumentIndex))
        {
            return null;
        }

        var correlationId = Guid.NewGuid();
        using (_telemetryReporter.TrackLspRequest(Methods.TextDocumentCompletionName, LanguageServerConstants.RazorLanguageServerName, TelemetryThresholds.CompletionRazorTelemetryThreshold, correlationId))
        {
            var razorCompletionOptions = new RazorCompletionOptions(
                SnippetsSupported: true,
                AutoInsertAttributeQuotes: options.AutoInsertAttributeQuotes,
                CommitElementsWithSpace: options.CommitElementsWithSpace);

            var result = await _completionListProvider
                .GetCompletionListAsync(
                    hostDocumentIndex,
                    completionContext,
                    documentContext,
                    _clientCapabilities.AssumeNotNull(),
                    razorCompletionOptions,
                    correlationId,
                    cancellationToken)
                .ConfigureAwait(false);

            if (result is null)
            {
                return null;
            }

            RazorCompletionResolveData.Wrap(result, request.TextDocument, _clientCapabilities);
            return result;
        }
    }
}
