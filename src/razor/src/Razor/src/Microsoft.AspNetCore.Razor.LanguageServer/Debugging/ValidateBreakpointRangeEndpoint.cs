// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Debugging;

[LanguageServerEndpoint(VSInternalMethods.TextDocumentValidateBreakableRangeName)]
internal class ValidateBreakpointRangeEndpoint : AbstractRazorDelegatingEndpoint<ValidateBreakpointRangeParams, Range?>, ICapabilitiesProvider
{
    private readonly IRazorDocumentMappingService _documentMappingService;

    public ValidateBreakpointRangeEndpoint(
        IRazorDocumentMappingService documentMappingService,
        LanguageServerFeatureOptions languageServerFeatureOptions,
        ClientNotifierServiceBase languageServer,
        ILoggerFactory loggerFactory)
        : base(languageServerFeatureOptions, documentMappingService, languageServer, loggerFactory.CreateLogger<ValidateBreakpointRangeEndpoint>())
    {
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
    }

    protected override bool OnlySingleServer => false;

    protected override string CustomMessageTarget => CustomMessageNames.RazorValidateBreakpointRangeName;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.BreakableRangeProvider = true;
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

        if (_documentMappingService.TryMapToHostDocumentRange(codeDocument.GetCSharpDocument(), delegatedResponse, out var projectedRange))
        {
            return projectedRange;
        }

        return null;
    }
}
