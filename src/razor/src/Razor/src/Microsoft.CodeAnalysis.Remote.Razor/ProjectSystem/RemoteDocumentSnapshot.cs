// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

internal class RemoteDocumentSnapshot(TextDocument textDocument, RemoteProjectSnapshot projectSnapshot, IFilePathService filePathService) : IDocumentSnapshot
{
    private readonly TextDocument _textDocument = textDocument;
    private readonly RemoteProjectSnapshot _projectSnapshot = projectSnapshot;
    private readonly IFilePathService _filePathService = filePathService;

    // TODO: Delete this field when the source generator is hooked up
    private Document? _generatedDocument;

    private RazorCodeDocument? _codeDocument;

    public TextDocument TextDocument => _textDocument;

    public string? FileKind => FileKinds.GetFileKindFromFilePath(FilePath);

    public string? FilePath => _textDocument.FilePath;

    public string? TargetPath => _textDocument.FilePath;

    public IProjectSnapshot Project => _projectSnapshot;

    public bool SupportsOutput => true;

    public int Version => -999; // We don't expect to use this in cohosting, but plenty of existing code logs it's value

    public Task<SourceText> GetTextAsync() => _textDocument.GetTextAsync();

    public Task<VersionStamp> GetTextVersionAsync() => _textDocument.GetTextVersionAsync();

    public bool TryGetText([NotNullWhen(true)] out SourceText? result) => _textDocument.TryGetText(out result);

    public bool TryGetTextVersion(out VersionStamp result) => _textDocument.TryGetTextVersion(out result);

    public async Task<RazorCodeDocument> GetGeneratedOutputAsync(bool _)
    {
        // TODO: We don't need to worry about locking if we get called from the didOpen/didChange LSP requests, as CLaSP
        //       takes care of that for us, and blocks requests until those are complete. If that doesn't end up happening,
        //       then a locking mechanism here would prevent concurrent compilations.
        if (TryGetGeneratedOutput(out var codeDocument))
        {
            return codeDocument;
        }

        // The non-cohosted DocumentSnapshot implementation uses DocumentState to get the generated output, and we could do that too
        // but most of that code is optimized around caching pre-computed results when things change that don't affect the compilation.
        // We can't do that here because we are using Roslyn's project snapshots, which don't contain the info that Razor needs. We could
        // in future provide a side-car mechanism so we can cache things, but still take advantage of snapshots etc. but the working
        // assumption for this code is that the source generator will be used, and it will do all of that, so this implementation is naive
        // and simply compiles when asked, and if a new document snapshot comes in, we compile again. This is presumably worse for perf
        // but since we don't expect users to ever use cohosting without source generators, it's fine for now.

        var projectEngine = await _projectSnapshot.GetProjectEngine_CohostOnlyAsync(CancellationToken.None).ConfigureAwait(false);
        var tagHelpers = await _projectSnapshot.GetTagHelpersAsync(CancellationToken.None).ConfigureAwait(false);
        var imports = await DocumentState.GetImportsAsync(this, projectEngine).ConfigureAwait(false);

        // TODO: Get the configuration for forceRuntimeCodeGeneration
        // var forceRuntimeCodeGeneration = _projectSnapshot.Configuration.LanguageServerFlags?.ForceRuntimeCodeGeneration ?? false;

        codeDocument = await DocumentState
            .GenerateCodeDocumentAsync(this, projectEngine, imports, tagHelpers, forceRuntimeCodeGeneration: false)
            .ConfigureAwait(false);

        return InterlockedOperations.Initialize(ref _codeDocument, codeDocument);
    }

    public IDocumentSnapshot WithText(SourceText text)
    {
        var id = _textDocument.Id;
        var newDocument = _textDocument.Project.Solution.WithAdditionalDocumentText(id, text).GetAdditionalDocument(id).AssumeNotNull();

        return new RemoteDocumentSnapshot(newDocument, _projectSnapshot, _filePathService);
    }

    public bool TryGetGeneratedOutput([NotNullWhen(true)] out RazorCodeDocument? result)
    {
        result = _codeDocument;
        return result is not null;
    }

    public async Task<Document> GetGeneratedDocumentAsync()
    {
        if (_generatedDocument is Document generatedDocument)
        {
            return generatedDocument;
        }

        generatedDocument = await HACK_GenerateDocumentAsync().ConfigureAwait(false);
        return InterlockedOperations.Initialize(ref _generatedDocument, generatedDocument);
    }

    private async Task<Document> HACK_GenerateDocumentAsync()
    {
        // TODO: A real implementation needs to get the SourceGeneratedDocument from the solution

        var solution = TextDocument.Project.Solution;
        var generatedFilePath = _filePathService.GetRazorCSharpFilePath(Project.Key, FilePath.AssumeNotNull());
        var projectId = TextDocument.Project.Id;
        var generatedDocumentId = solution.GetDocumentIdsWithFilePath(generatedFilePath).First(d => d.ProjectId == projectId);
        var generatedDocument = solution.GetRequiredDocument(generatedDocumentId);

        var codeDocument = await this.GetGeneratedOutputAsync().ConfigureAwait(false);
        var csharpSourceText = codeDocument.GetCSharpSourceText();

        // HACK: We're not in the same solution fork as the LSP server that provides content for this document
        return generatedDocument.WithText(csharpSourceText);
    }

    public async Task<SyntaxTree> GetCSharpSyntaxTreeAsync(CancellationToken cancellationToken)
    {
        var document = await GetGeneratedDocumentAsync().ConfigureAwait(false);
        var tree = await document.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
        return tree.AssumeNotNull();
    }
}
