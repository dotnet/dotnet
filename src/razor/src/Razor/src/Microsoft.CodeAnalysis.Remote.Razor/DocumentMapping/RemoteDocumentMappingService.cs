// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
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
    DocumentSnapshotFactory documentSnapshotFactory,
    ILoggerFactory loggerFactory)
    : AbstractDocumentMappingService(filePathService, loggerFactory.GetOrCreateLogger<RemoteDocumentMappingService>())
{
    private readonly DocumentSnapshotFactory _documentSnapshotFactory = documentSnapshotFactory;

    public async Task<(Uri MappedDocumentUri, LinePositionSpan MappedRange)> MapToHostDocumentUriAndRangeAsync(
        RemoteDocumentSnapshot originSnapshot,
        Uri generatedDocumentUri,
        LinePositionSpan generatedDocumentRange,
        CancellationToken cancellationToken)
    {
        var razorDocumentUri = FilePathService.GetRazorDocumentUri(generatedDocumentUri);

        // For Html we just map the Uri, the range will be the same
        if (FilePathService.IsVirtualHtmlFile(generatedDocumentUri))
        {
            return (razorDocumentUri, generatedDocumentRange);
        }

        // We only map from C# files
        if (!FilePathService.IsVirtualCSharpFile(generatedDocumentUri))
        {
            return (generatedDocumentUri, generatedDocumentRange);
        }

        var solution = originSnapshot.TextDocument.Project.Solution;

        var razorDocumentId = solution.GetDocumentIdsWithUri(razorDocumentUri).FirstOrDefault();

        // If we couldn't locate the .razor file, just return the generated file.
        if (razorDocumentId is null ||
            solution.GetAdditionalDocument(razorDocumentId) is not TextDocument razorDocument)
        {
            return (generatedDocumentUri, generatedDocumentRange);
        }

        var razorDocumentSnapshot = _documentSnapshotFactory.GetOrCreate(razorDocument);

        var razorCodeDocument = await razorDocumentSnapshot.GetGeneratedOutputAsync().ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();

        if (razorCodeDocument is null)
        {
            return (generatedDocumentUri, generatedDocumentRange);
        }

        if (!razorCodeDocument.TryGetGeneratedDocument(generatedDocumentUri, FilePathService, out var generatedDocument))
        {
            return Assumed.Unreachable<(Uri, LinePositionSpan)>();
        }

        if (TryMapToHostDocumentRange(generatedDocument, generatedDocumentRange, MappingBehavior.Strict, out var mappedRange))
        {
            return (razorDocumentUri, mappedRange);
        }

        return (generatedDocumentUri, generatedDocumentRange);
    }
}
