// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.CodeAnalysis.Razor.Workspaces.Telemetry;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

[RazorLanguageServerEndpoint(LspEndpointName)]
internal sealed class CodeActionEndpoint(
    ICodeActionsService codeActionsService,
    IDelegatedCodeActionsProvider delegatedCodeActionProvider,
    ITelemetryReporter telemetryReporter)
    : IRazorRequestHandler<VSCodeActionParams, SumType<Command, CodeAction>[]?>, ICapabilitiesProvider
{
    private const string LspEndpointName = Methods.TextDocumentCodeActionName;

    private readonly ICodeActionsService _codeActionsService = codeActionsService;
    private readonly IDelegatedCodeActionsProvider _delegatedCodeActionProvider = delegatedCodeActionProvider;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;

    private bool _supportsCodeActionResolve = false;

    public bool MutatesSolutionState { get; } = false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        _supportsCodeActionResolve = clientCapabilities.TextDocument?.CodeAction?.ResolveSupport is not null;

        serverCapabilities.EnableCodeActions();
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(VSCodeActionParams request)
    {
        return request.TextDocument;
    }

    public async Task<SumType<Command, CodeAction>[]?> HandleRequestAsync(VSCodeActionParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        // VS Provides `CodeActionParams.Context.SelectionRange` in addition to
        // `CodeActionParams.Range`. The `SelectionRange` is relative to where the
        // code action was invoked (ex. line 14, char 3) whereas the `Range` is
        // always at the start of the line (ex. line 14, char 0). We want to utilize
        // the relative positioning to ensure we provide code actions for the appropriate
        // context.
        //
        // Note: VS Code doesn't provide a `SelectionRange`.
        var vsCodeActionContext = request.Context;
        if (vsCodeActionContext.SelectionRange != null)
        {
            request.Range = vsCodeActionContext.SelectionRange;
        }

        var correlationId = Guid.NewGuid();
        using var __ = _telemetryReporter.TrackLspRequest(LspEndpointName, LanguageServerConstants.RazorLanguageServerName, TelemetryThresholds.CodeActionRazorTelemetryThreshold, correlationId);
        cancellationToken.ThrowIfCancellationRequested();

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (!codeDocument.Source.Text.TryGetAbsoluteIndex(request.Range.Start, out var absoluteIndex))
        {
            return null;
        }

        var languageKind = codeDocument.GetLanguageKind(absoluteIndex, rightAssociative: false);
        var documentSnapshot = documentContext.Snapshot;

        var delegatedCodeActions = languageKind switch
        {
            RazorLanguageKind.Html => await GetHtmlCodeActionsAsync(documentSnapshot, request, correlationId, cancellationToken).ConfigureAwait(false),
            RazorLanguageKind.CSharp => await GetCSharpCodeActionsAsync(documentSnapshot, request, correlationId, cancellationToken).ConfigureAwait(false),
            _ => []
        };

        return await _codeActionsService.GetCodeActionsAsync(
            request,
            documentSnapshot,
            delegatedCodeActions,
            delegatedDocumentUri: null, // We don't use delegatedDocumentUri in the LSP server, as we can trivially recalculate it
            _supportsCodeActionResolve,
            cancellationToken).ConfigureAwait(false);
    }

    private async Task<RazorVSInternalCodeAction[]> GetHtmlCodeActionsAsync(IDocumentSnapshot documentSnapshot, VSCodeActionParams request, Guid correlationId, CancellationToken cancellationToken)
    {
        var htmlCodeActions = await _delegatedCodeActionProvider.GetDelegatedCodeActionsAsync(RazorLanguageKind.Html, request, documentSnapshot.Version, correlationId, cancellationToken).ConfigureAwait(false);
        return htmlCodeActions ?? [];
    }

    private async Task<RazorVSInternalCodeAction[]> GetCSharpCodeActionsAsync(IDocumentSnapshot documentSnapshot, VSCodeActionParams request, Guid correlationId, CancellationToken cancellationToken)
    {
        var csharpRequest = await _codeActionsService.GetCSharpCodeActionsRequestAsync(documentSnapshot, request, cancellationToken).ConfigureAwait(false);
        if (csharpRequest is null)
        {
            return [];
        }

        var csharpCodeActions = await _delegatedCodeActionProvider.GetDelegatedCodeActionsAsync(RazorLanguageKind.CSharp, csharpRequest, documentSnapshot.Version, correlationId, cancellationToken).ConfigureAwait(false);
        return csharpCodeActions ?? [];
    }

    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(CodeActionEndpoint instance)
    {
        public void SetSupportsCodeActionResolve(bool value)
        {
            instance._supportsCodeActionResolve = value;
        }

        public Task<RazorVSInternalCodeAction[]> GetCSharpCodeActionsAsync(IDocumentSnapshot documentSnapshot, VSCodeActionParams request, Guid correlationId, CancellationToken cancellationToken)
            => instance.GetCSharpCodeActionsAsync(documentSnapshot, request, correlationId, cancellationToken);
    }
}
