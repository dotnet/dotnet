// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

[RazorLanguageServerEndpoint(Methods.CodeActionResolveName)]
internal sealed class CodeActionResolveEndpoint(
    ICodeActionResolveService codeActionResolveService,
    IDelegatedCodeActionResolver delegatedCodeActionResolver,
    RazorLSPOptionsMonitor razorLSPOptionsMonitor) : IRazorRequestHandler<CodeAction, CodeAction>
{
    private readonly ICodeActionResolveService _codeActionResolveService = codeActionResolveService;
    private readonly IDelegatedCodeActionResolver _delegatedCodeActionResolver = delegatedCodeActionResolver;
    private readonly RazorLSPOptionsMonitor _razorLSPOptionsMonitor = razorLSPOptionsMonitor;

    public bool MutatesSolutionState => false;

    public TextDocumentIdentifier GetTextDocumentIdentifier(CodeAction request)
        => CodeActionResolveService.GetRazorCodeActionResolutionParams(request).TextDocument;

    public async Task<CodeAction> HandleRequestAsync(CodeAction request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var options = new RazorFormattingOptions
        {
            TabSize = _razorLSPOptionsMonitor.CurrentValue.TabSize,
            InsertSpaces = _razorLSPOptionsMonitor.CurrentValue.InsertSpaces,
            CodeBlockBraceOnNextLine = _razorLSPOptionsMonitor.CurrentValue.CodeBlockBraceOnNextLine
        };
        var documentContext = requestContext.DocumentContext.AssumeNotNull();

        var context = CodeActionResolveService.GetRazorCodeActionResolutionParams(request);

        var resolvedDelegatedCodeAction = context.Language != RazorLanguageKind.Razor
            ? await ResolveDelegatedCodeActionAsync(documentContext, request, context, cancellationToken).ConfigureAwait(false)
            : null;

        return await _codeActionResolveService.ResolveCodeActionAsync(documentContext, request, resolvedDelegatedCodeAction, options, cancellationToken).ConfigureAwait(false);
    }

    private async Task<CodeAction> ResolveDelegatedCodeActionAsync(DocumentContext documentContext, CodeAction request, RazorCodeActionResolutionParams context, CancellationToken cancellationToken)
    {
        var originalData = request.Data;
        request.Data = context.Data;

        try
        {
            var resolvedCodeAction = await _delegatedCodeActionResolver.ResolveCodeActionAsync(documentContext.GetTextDocumentIdentifier(), documentContext.Snapshot.Version, context.Language, request, cancellationToken).ConfigureAwait(false);
            return resolvedCodeAction ?? request;
        }
        finally
        {
            request.Data = originalData;
        }
    }
}
