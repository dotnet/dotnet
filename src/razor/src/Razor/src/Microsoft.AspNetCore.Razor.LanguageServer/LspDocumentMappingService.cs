// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class LspDocumentMappingService(
    IFilePathService filePathService,
    IDocumentContextFactory documentContextFactory,
    ILoggerFactory loggerFactory)
    : AbstractDocumentMappingService(loggerFactory.GetOrCreateLogger<LspDocumentMappingService>())
{
    private readonly IFilePathService _filePathService = filePathService;
    private readonly IDocumentContextFactory _documentContextFactory = documentContextFactory;

    public async Task<(Uri MappedDocumentUri, LinePositionSpan MappedRange)> MapToHostDocumentUriAndRangeAsync(
        Uri generatedDocumentUri,
        LinePositionSpan generatedDocumentRange,
        CancellationToken cancellationToken)
    {
        var razorDocumentUri = _filePathService.GetRazorDocumentUri(generatedDocumentUri);

        // For Html we just map the Uri, the range will be the same
        if (_filePathService.IsVirtualHtmlFile(generatedDocumentUri))
        {
            return (razorDocumentUri, generatedDocumentRange);
        }

        // We only map from C# files
        if (!_filePathService.IsVirtualCSharpFile(generatedDocumentUri))
        {
            return (generatedDocumentUri, generatedDocumentRange);
        }

        if (!_documentContextFactory.TryCreate(razorDocumentUri, out var documentContext))
        {
            return (generatedDocumentUri, generatedDocumentRange);
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (TryMapToRazorDocumentRange(codeDocument.GetRequiredCSharpDocument(), generatedDocumentRange, MappingBehavior.Strict, out var mappedRange))
        {
            return (razorDocumentUri, mappedRange);
        }

        return (generatedDocumentUri, generatedDocumentRange);
    }
}
