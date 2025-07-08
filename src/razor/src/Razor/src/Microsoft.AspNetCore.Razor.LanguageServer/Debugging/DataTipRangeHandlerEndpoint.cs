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

[RazorLanguageServerEndpoint(VSInternalMethods.TextDocumentDataTipRangeName)]
internal sealed class DataTipRangeHandlerEndpoint(
    IDocumentMappingService documentMappingService,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    IClientConnection clientConnection,
    ILoggerFactory loggerFactory)
    : AbstractRazorDelegatingEndpoint<TextDocumentPositionParams, VSInternalDataTip?>(
        languageServerFeatureOptions,
        documentMappingService,
        clientConnection,
        loggerFactory.GetOrCreateLogger<DataTipRangeHandlerEndpoint>()), ICapabilitiesProvider
{
    protected override bool OnlySingleServer => false;

    protected override string CustomMessageTarget => CustomMessageNames.RazorDataTipRangeName;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.EnableDataTipRangeProvider();
    }

    protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(TextDocumentPositionParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        // only C# supports breakpoints
        if (positionInfo.LanguageKind != RazorLanguageKind.CSharp)
        {
            return SpecializedTasks.Null<IDelegatedParams>();
        }

        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return SpecializedTasks.Null<IDelegatedParams>();
        }

        return Task.FromResult<IDelegatedParams?>(new DelegatedPositionParams(
            documentContext.GetTextDocumentIdentifierAndVersion(),
            positionInfo.Position,
            positionInfo.LanguageKind));
    }

    protected override async Task<VSInternalDataTip?> HandleDelegatedResponseAsync(VSInternalDataTip? delegatedResponse, TextDocumentPositionParams originalRequest, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        if (delegatedResponse is null)
        {
            return null;
        }

        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var csharpDocument = codeDocument.GetRequiredCSharpDocument();

        if (!DocumentMappingService.TryMapToRazorDocumentRange(csharpDocument, delegatedResponse.HoverRange, out var hoverRange))
        {
            return null;
        }

        LspRange? expressionRange = null;
        if (delegatedResponse.ExpressionRange != null && !DocumentMappingService.TryMapToRazorDocumentRange(csharpDocument, delegatedResponse.ExpressionRange, out expressionRange))
        {
            return null;
        }

        return new VSInternalDataTip()
        {
            HoverRange = hoverRange,
            ExpressionRange = expressionRange,
            DataTipTags = delegatedResponse.DataTipTags,
        };
    }
}
