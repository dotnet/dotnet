// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

internal class RazorCompletionEndpoint : IVSCompletionEndpoint
{
    private readonly CompletionListProvider _completionListProvider;
    private readonly ITelemetryReporter? _telemetryReporter;
    private VSInternalClientCapabilities? _clientCapabilities;

    public RazorCompletionEndpoint(CompletionListProvider completionListProvider, ITelemetryReporter? telemetryReporter)
    {
        _completionListProvider = completionListProvider;
        _telemetryReporter = telemetryReporter;
    }

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        _clientCapabilities = clientCapabilities;

        serverCapabilities.CompletionProvider = new CompletionOptions()
        {
            ResolveProvider = true,
            TriggerCharacters = _completionListProvider.AggregateTriggerCharacters.ToArray(),
            AllCommitCharacters = new[] { ":", ">", " ", "=" },
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
        if (!request.Position.TryGetAbsoluteIndex(sourceText, requestContext.Logger, out var hostDocumentIndex))
        {
            return null;
        }

        if (request.Context is not VSInternalCompletionContext completionContext)
        {
            Debug.Fail("Completion context should never be null in practice");
            return null;
        }

        var correlationId = Guid.NewGuid();
        using var _ = _telemetryReporter?.TrackLspRequest(Methods.TextDocumentCompletionName, LanguageServerConstants.RazorLanguageServerName, correlationId);
        var completionList = await _completionListProvider.GetCompletionListAsync(
            hostDocumentIndex,
            completionContext,
            documentContext,
            _clientCapabilities!,
            correlationId,
            cancellationToken).ConfigureAwait(false);
        return completionList;
    }
}
