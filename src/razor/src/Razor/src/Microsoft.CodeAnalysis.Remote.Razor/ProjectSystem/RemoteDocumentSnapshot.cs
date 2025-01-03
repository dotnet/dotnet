// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

internal sealed class RemoteDocumentSnapshot : IDocumentSnapshot
{
    public TextDocument TextDocument { get; }
    public RemoteProjectSnapshot ProjectSnapshot { get; }

    // TODO: Delete this field when the source generator is hooked up
    private Document? _generatedDocument;

    private RazorCodeDocument? _codeDocument;

    public RemoteDocumentSnapshot(TextDocument textDocument, RemoteProjectSnapshot projectSnapshot)
    {
        if (!textDocument.IsRazorDocument())
        {
            throw new ArgumentException(SR.Document_is_not_a_Razor_document);
        }

        TextDocument = textDocument;
        ProjectSnapshot = projectSnapshot;
    }

    public string FileKind => FileKinds.GetFileKindFromFilePath(FilePath);
    public string FilePath => TextDocument.FilePath.AssumeNotNull();
    public string TargetPath => TextDocument.FilePath.AssumeNotNull();

    public IProjectSnapshot Project => ProjectSnapshot;

    public int Version => -999; // We don't expect to use this in cohosting, but plenty of existing code logs it's value

    public ValueTask<SourceText> GetTextAsync(CancellationToken cancellationToken)
    {
        return TryGetText(out var result)
            ? new(result)
            : new(TextDocument.GetTextAsync(cancellationToken));
    }

    public ValueTask<VersionStamp> GetTextVersionAsync(CancellationToken cancellationToken)
    {
        return TryGetTextVersion(out var result)
            ? new(result)
            : new(TextDocument.GetTextVersionAsync(cancellationToken));
    }

    public bool TryGetText([NotNullWhen(true)] out SourceText? result)
        => TextDocument.TryGetText(out result);

    public bool TryGetTextVersion(out VersionStamp result)
        => TextDocument.TryGetTextVersion(out result);

    public ValueTask<RazorCodeDocument> GetGeneratedOutputAsync(
        bool forceDesignTimeGeneratedOutput,
        CancellationToken cancellationToken)
    {
        // We don't cache if we're forcing, as that would break everything else
#if !FORMAT_FUSE
        if (forceDesignTimeGeneratedOutput)
        {
            return new ValueTask<RazorCodeDocument>(GetRazorCodeDocumentAsync(forceRuntimeCodeGeneration: false, cancellationToken));
        }
#endif

        var forceRuntimeCodeGeneration = ProjectSnapshot.SolutionSnapshot.SnapshotManager.LanguageServerFeatureOptions.ForceRuntimeCodeGeneration;

        // TODO: We don't need to worry about locking if we get called from the didOpen/didChange LSP requests, as CLaSP
        //       takes care of that for us, and blocks requests until those are complete. If that doesn't end up happening,
        //       then a locking mechanism here would prevent concurrent compilations.
        return TryGetGeneratedOutput(out var codeDocument)
            ? new(codeDocument)
            : new(GetGeneratedOutputCoreAsync(forceRuntimeCodeGeneration, cancellationToken));

        async Task<RazorCodeDocument> GetGeneratedOutputCoreAsync(bool forceRuntimeCodeGeneration, CancellationToken cancellationToken)
        {
            codeDocument = await GetRazorCodeDocumentAsync(forceRuntimeCodeGeneration, cancellationToken).ConfigureAwait(false);

            return _codeDocument ??= InterlockedOperations.Initialize(ref _codeDocument, codeDocument);
        }

        async Task<RazorCodeDocument> GetRazorCodeDocumentAsync(bool forceRuntimeCodeGeneration, CancellationToken cancellationToken)
        {
            // The non-cohosted DocumentSnapshot implementation uses DocumentState to get the generated output, and we could do that too
            // but most of that code is optimized around caching pre-computed results when things change that don't affect the compilation.
            // We can't do that here because we are using Roslyn's project snapshots, which don't contain the info that Razor needs. We could
            // in future provide a side-car mechanism so we can cache things, but still take advantage of snapshots etc. but the working
            // assumption for this code is that the source generator will be used, and it will do all of that, so this implementation is naive
            // and simply compiles when asked, and if a new document snapshot comes in, we compile again. This is presumably worse for perf
            // but since we don't expect users to ever use cohosting without source generators, it's fine for now.

            var projectEngine = await ProjectSnapshot.GetProjectEngine_CohostOnlyAsync(cancellationToken).ConfigureAwait(false);

            return await DocumentState
                .GenerateCodeDocumentAsync(this, projectEngine, forceRuntimeCodeGeneration, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    public IDocumentSnapshot WithText(SourceText text)
    {
        var id = TextDocument.Id;
        var newDocument = TextDocument.Project.Solution
            .WithAdditionalDocumentText(id, text)
            .GetAdditionalDocument(id)
            .AssumeNotNull();

        var snapshotManager = ProjectSnapshot.SolutionSnapshot.SnapshotManager;
        return snapshotManager.GetSnapshot(newDocument);
    }

    public bool TryGetGeneratedOutput([NotNullWhen(true)] out RazorCodeDocument? result)
    {
        result = _codeDocument;
        return result is not null;
    }

    public ValueTask<Document> GetGeneratedDocumentAsync(CancellationToken cancellationToken)
    {
        return TryGetGeneratedDocument(out var generatedDocument)
            ? new(generatedDocument)
            : GetGeneratedDocumentCoreAsync(cancellationToken);

        async ValueTask<Document> GetGeneratedDocumentCoreAsync(CancellationToken cancellationToken)
        {
            var generatedDocument = await HACK_GenerateDocumentAsync(cancellationToken).ConfigureAwait(false);
            return _generatedDocument ??= InterlockedOperations.Initialize(ref _generatedDocument, generatedDocument);
        }
    }

    public bool TryGetGeneratedDocument([NotNullWhen(true)] out Document? generatedDocument)
    {
        generatedDocument = _generatedDocument;
        return generatedDocument is not null;
    }

    private async Task<Document> HACK_GenerateDocumentAsync(CancellationToken cancellationToken)
    {
        // TODO: A real implementation needs to get the SourceGeneratedDocument from the solution

        var solution = TextDocument.Project.Solution;
        var filePathService = ProjectSnapshot.SolutionSnapshot.SnapshotManager.FilePathService;
        var generatedFilePath = filePathService.GetRazorCSharpFilePath(Project.Key, FilePath);
        var projectId = TextDocument.Project.Id;
        var generatedDocumentId = solution.GetDocumentIdsWithFilePath(generatedFilePath).First(d => d.ProjectId == projectId);
        var generatedDocument = solution.GetRequiredDocument(generatedDocumentId);

        var codeDocument = await this.GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);
        var csharpSourceText = codeDocument.GetCSharpSourceText();

        // HACK: We're not in the same solution fork as the LSP server that provides content for this document
        return generatedDocument.WithText(csharpSourceText);
    }

    public ValueTask<SyntaxTree> GetCSharpSyntaxTreeAsync(CancellationToken cancellationToken)
    {
        if (TryGetGeneratedDocument(out var document) &&
            document.TryGetSyntaxTree(out var tree))
        {
            return new(tree.AssumeNotNull());
        }

        return GetCSharpSyntaxTreeCoreAsync(document, cancellationToken);

        async ValueTask<SyntaxTree> GetCSharpSyntaxTreeCoreAsync(Document? document, CancellationToken cancellationToken)
        {
            document ??= await GetGeneratedDocumentAsync(cancellationToken).ConfigureAwait(false);

            var tree = await document.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
            return tree.AssumeNotNull();
        }
    }
}
