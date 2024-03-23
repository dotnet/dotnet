// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost.Handlers;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Remote.Razor.SemanticTokens;

[Export(typeof(ICSharpSemanticTokensProvider)), Shared]
[method: ImportingConstructor]
internal class RemoteCSharpSemanticTokensProvider(IFilePathService filePathService, ITelemetryReporter telemetryReporter) : ICSharpSemanticTokensProvider
{
    private readonly IFilePathService _filePathService = filePathService;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;

    public async Task<int[]?> GetCSharpSemanticTokensResponseAsync(VersionedDocumentContext documentContext, ImmutableArray<LinePositionSpan> csharpRanges, Guid correlationId, CancellationToken cancellationToken)
    {
        using var _ = _telemetryReporter.TrackLspRequest(nameof(SemanticTokensRange.GetSemanticTokensAsync), Constants.ExternalAccessServerName, correlationId);

        // We have a razor document, lets find the generated C# document
        var generatedDocument = GetGeneratedDocument(documentContext);

        var data = await SemanticTokensRange.GetSemanticTokensAsync(
            generatedDocument,
            csharpRanges,
            supportsVisualStudioExtensions: true,
            cancellationToken).ConfigureAwait(false);

        return data;
    }

    private Document GetGeneratedDocument(VersionedDocumentContext documentContext)
    {
        var snapshot = (RemoteDocumentSnapshot)documentContext.Snapshot;
        var razorDocument = snapshot.TextDocument;
        var solution = razorDocument.Project.Solution;

        // TODO: A real implementation needs to get the SourceGeneratedDocument from the solution

        var projectKey = ProjectKey.From(razorDocument.Project);
        var generatedFilePath = _filePathService.GetRazorCSharpFilePath(projectKey, razorDocument.FilePath.AssumeNotNull());
        var generatedDocumentId = solution.GetDocumentIdsWithFilePath(generatedFilePath).First(d => d.ProjectId == razorDocument.Project.Id);
        var generatedDocument = solution.GetDocument(generatedDocumentId).AssumeNotNull();
        return generatedDocument;
    }
}
