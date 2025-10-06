// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Debugging;

[RazorLanguageServerEndpoint(VSInternalMethods.TextDocumentValidateBreakableRangeName)]
internal class ValidateBreakpointRangeEndpoint(
    IDocumentMappingService documentMappingService,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    IClientConnection clientConnection,
    ILoggerFactory loggerFactory)
    : AbstractRazorDelegatingEndpoint<ValidateBreakpointRangeParams, LspRange?>(
        languageServerFeatureOptions,
        documentMappingService,
        clientConnection,
        loggerFactory.GetOrCreateLogger<ValidateBreakpointRangeEndpoint>()), ICapabilitiesProvider
{
    private readonly IDocumentMappingService _documentMappingService = documentMappingService;

    protected override bool OnlySingleServer => false;

    protected override string CustomMessageTarget => CustomMessageNames.RazorValidateBreakpointRangeName;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.EnableValidateBreakpointRange();
    }

    protected override Task<LspRange?> TryHandleAsync(ValidateBreakpointRangeParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        // no such thing as Razor breakpoints (yet?!)
        return SpecializedTasks.Null<LspRange>();
    }

    protected async override Task<IDelegatedParams?> CreateDelegatedParamsAsync(ValidateBreakpointRangeParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        // only C# supports breakpoints
        if (positionInfo.LanguageKind != RazorLanguageKind.CSharp)
        {
            return null;
        }

        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        // We've already mapped the position, but sadly we need a range for breakpoints, so we have to do it again
        if (!_documentMappingService.TryMapToCSharpDocumentRange(codeDocument.GetRequiredCSharpDocument(), request.Range, out var projectedRange))
        {
            return null;
        }

        return new DelegatedValidateBreakpointRangeParams(
            documentContext.GetTextDocumentIdentifierAndVersion(),
            projectedRange,
            positionInfo.LanguageKind);
    }

    protected async override Task<LspRange?> HandleDelegatedResponseAsync(LspRange? delegatedResponse, ValidateBreakpointRangeParams originalRequest, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        if (delegatedResponse is null)
        {
            return null;
        }

        if (delegatedResponse == LspFactory.UndefinedRange)
        {
            Logger.LogInformation($"Delegation could not get a valid answer, so returning original range so we don't lose user data.");
            return originalRequest.Range;
        }

        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        if (_documentMappingService.TryMapToRazorDocumentRange(codeDocument.GetRequiredCSharpDocument(), delegatedResponse, MappingBehavior.Inclusive, out var projectedRange))
        {
            return projectedRange;
        }

        return null;
    }
}
