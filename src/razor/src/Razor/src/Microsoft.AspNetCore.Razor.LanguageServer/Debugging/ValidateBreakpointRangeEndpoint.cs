// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Debugging;

[RazorLanguageServerEndpoint(VSInternalMethods.TextDocumentValidateBreakableRangeName)]
internal class ValidateBreakpointRangeEndpoint(
    IRazorDocumentMappingService documentMappingService,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    IClientConnection clientConnection,
    IRazorLoggerFactory loggerFactory)
    : AbstractRazorDelegatingEndpoint<ValidateBreakpointRangeParams, Range?>(
        languageServerFeatureOptions,
        documentMappingService,
        clientConnection,
        loggerFactory.CreateLogger<ValidateBreakpointRangeEndpoint>()), ICapabilitiesProvider
{
    private readonly IRazorDocumentMappingService _documentMappingService = documentMappingService;

    protected override bool OnlySingleServer => false;

    protected override string CustomMessageTarget => CustomMessageNames.RazorValidateBreakpointRangeName;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.EnableValidateBreakpointRange();
    }

    protected override Task<Range?> TryHandleAsync(ValidateBreakpointRangeParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        // no such thing as Razor breakpoints (yet?!)
        return Task.FromResult<Range?>(null);
    }

    protected async override Task<IDelegatedParams?> CreateDelegatedParamsAsync(ValidateBreakpointRangeParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        // only C# supports breakpoints
        if (positionInfo.LanguageKind != RazorLanguageKind.CSharp)
        {
            return null;
        }

        var documentContext = requestContext.GetRequiredDocumentContext();
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        // We've already mapped the position, but sadly we need a range for breakpoints, so we have to do it again
        if (!_documentMappingService.TryMapToGeneratedDocumentRange(codeDocument.GetCSharpDocument(), request.Range, out var projectedRange))
        {
            return null;
        }

        return new DelegatedValidateBreakpointRangeParams(
            documentContext.Identifier,
            projectedRange,
            positionInfo.LanguageKind);
    }

    protected async override Task<Range?> HandleDelegatedResponseAsync(Range? delegatedResponse, ValidateBreakpointRangeParams originalRequest, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        if (delegatedResponse is null)
        {
            return null;
        }

        var documentContext = requestContext.GetRequiredDocumentContext();
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        if (_documentMappingService.TryMapToHostDocumentRange(codeDocument.GetCSharpDocument(), delegatedResponse, MappingBehavior.Inclusive, out var projectedRange))
        {
            return projectedRange;
        }

        return null;
    }
}
