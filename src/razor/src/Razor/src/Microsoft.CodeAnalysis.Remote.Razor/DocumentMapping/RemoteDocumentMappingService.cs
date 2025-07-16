// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Remote.Razor.DocumentMapping;

[Export(typeof(IDocumentMappingService)), Shared]
[method: ImportingConstructor]
internal sealed class RemoteDocumentMappingService(
    IFilePathService filePathService,
    RemoteSnapshotManager snapshotManager,
    ILoggerFactory loggerFactory)
    : AbstractDocumentMappingService(loggerFactory.GetOrCreateLogger<RemoteDocumentMappingService>())
{
    private readonly IFilePathService _filePathService = filePathService;
    private readonly RemoteSnapshotManager _snapshotManager = snapshotManager;

    public async Task<(Uri MappedDocumentUri, LinePositionSpan MappedRange)> MapToHostDocumentUriAndRangeAsync(
        RemoteDocumentSnapshot originSnapshot,
        Uri generatedDocumentUri,
        LinePositionSpan generatedDocumentRange,
        CancellationToken cancellationToken)
    {
        // For Html we just map the Uri, the range will be the same
        if (_filePathService.IsVirtualHtmlFile(generatedDocumentUri))
        {
            var razorDocumentUri = _filePathService.GetRazorDocumentUri(generatedDocumentUri);
            return (razorDocumentUri, generatedDocumentRange);
        }

        // We only map from C# files
        if (!_filePathService.IsVirtualCSharpFile(generatedDocumentUri))
        {
            return (generatedDocumentUri, generatedDocumentRange);
        }

        var project = originSnapshot.TextDocument.Project;
        var razorCodeDocument = await _snapshotManager.GetSnapshot(project).TryGetCodeDocumentFromGeneratedDocumentUriAsync(generatedDocumentUri, cancellationToken).ConfigureAwait(false);
        if (razorCodeDocument is null)
        {
            return (generatedDocumentUri, generatedDocumentRange);
        }

        if (TryMapToRazorDocumentRange(razorCodeDocument.GetRequiredCSharpDocument(), generatedDocumentRange, MappingBehavior.Strict, out var mappedRange))
        {
            var solution = project.Solution;
            var filePath = razorCodeDocument.Source.FilePath;
            var documentId = solution.GetDocumentIdsWithFilePath(filePath).First();
            var document = solution.GetAdditionalDocument(documentId).AssumeNotNull();
            return (document.CreateUri(), mappedRange);
        }

        return (generatedDocumentUri, generatedDocumentRange);
    }
}
