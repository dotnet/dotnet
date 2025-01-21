// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

internal sealed class RemoteDocumentSnapshot : IDocumentSnapshot
#if !FORMAT_FUSE
    , IDesignTimeCodeGenerator
#endif
{
    public TextDocument TextDocument { get; }
    public RemoteProjectSnapshot ProjectSnapshot { get; }

    // TODO: Delete this field when the source generator is hooked up
    private readonly AsyncLazy<Document> _lazyDocument;
    private readonly AsyncLazy<RazorCodeDocument> _lazyCodeDocument;

    public RemoteDocumentSnapshot(TextDocument textDocument, RemoteProjectSnapshot projectSnapshot)
    {
        if (!textDocument.IsRazorDocument())
        {
            throw new ArgumentException(SR.Document_is_not_a_Razor_document);
        }

        TextDocument = textDocument;
        ProjectSnapshot = projectSnapshot;

        _lazyDocument = AsyncLazy.Create(HACK_ComputeDocumentAsync);
        _lazyCodeDocument = AsyncLazy.Create(ComputeGeneratedOutputAsync);
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

    public bool TryGetGeneratedOutput([NotNullWhen(true)] out RazorCodeDocument? result)
        => _lazyCodeDocument.TryGetValue(out result);

    public ValueTask<RazorCodeDocument> GetGeneratedOutputAsync(CancellationToken cancellationToken)
    {
        if (TryGetGeneratedOutput(out var result))
        {
            return new(result);
        }

        return new(_lazyCodeDocument.GetValueAsync(cancellationToken));
    }

    private async Task<RazorCodeDocument> ComputeGeneratedOutputAsync(CancellationToken cancellationToken)
    {
        var projectEngine = await ProjectSnapshot.GetProjectEngineAsync(cancellationToken).ConfigureAwait(false);
        var compilerOptions = ProjectSnapshot.SolutionSnapshot.SnapshotManager.CompilerOptions;

        return await CompilationHelpers
            .GenerateCodeDocumentAsync(this, projectEngine, compilerOptions, cancellationToken)
            .ConfigureAwait(false);
    }

#if !FORMAT_FUSE
    public async Task<RazorCodeDocument> GenerateDesignTimeOutputAsync(CancellationToken cancellationToken)
    {
        var projectEngine = await ProjectSnapshot.GetProjectEngineAsync(cancellationToken).ConfigureAwait(false);

        return await CompilationHelpers
            .GenerateDesignTimeCodeDocumentAsync(this, projectEngine, cancellationToken)
            .ConfigureAwait(false);
    }
#endif

    private async Task<Document> HACK_ComputeDocumentAsync(CancellationToken cancellationToken)
    {
        // TODO: A real implementation needs to get the SourceGeneratedDocument from the solution

        var solution = TextDocument.Project.Solution;
        var filePathService = ProjectSnapshot.SolutionSnapshot.SnapshotManager.FilePathService;
        var generatedFilePath = filePathService.GetRazorCSharpFilePath(Project.Key, FilePath);
        var generatedDocumentId = solution
            .GetDocumentIdsWithFilePath(generatedFilePath)
            .First(TextDocument.Project.Id, static (d, projectId) => d.ProjectId == projectId);

        var generatedDocument = solution.GetRequiredDocument(generatedDocumentId);

        var codeDocument = await GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);
        var csharpSourceText = codeDocument.GetCSharpSourceText();

        // HACK: We're not in the same solution fork as the LSP server that provides content for this document
        return generatedDocument.WithText(csharpSourceText);
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

    public bool TryGetGeneratedDocument([NotNullWhen(true)] out Document? result)
        => _lazyDocument.TryGetValue(out result);

    public ValueTask<Document> GetGeneratedDocumentAsync(CancellationToken cancellationToken)
    {
        if (TryGetGeneratedDocument(out var result))
        {
            return new(result);
        }

        return new(_lazyDocument.GetValueAsync(cancellationToken));
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
