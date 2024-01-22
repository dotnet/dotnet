// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Cohost;

internal class CohostDocumentSnapshot(TextDocument textDocument, IProjectSnapshot projectSnapshot) : IDocumentSnapshot
{
    private readonly TextDocument _textDocument = textDocument;
    private readonly IProjectSnapshot _projectSnapshot = projectSnapshot;

    private RazorCodeDocument? _codeDocument;

    public string? FileKind => FileKinds.GetFileKindFromFilePath(FilePath);

    public string? FilePath => _textDocument.FilePath;

    public string? TargetPath => _textDocument.FilePath;

    public IProjectSnapshot Project => _projectSnapshot;

    public bool SupportsOutput => true;

    public Task<SourceText> GetTextAsync() => _textDocument.GetTextAsync();

    public Task<VersionStamp> GetTextVersionAsync() => _textDocument.GetTextVersionAsync();

    public bool TryGetText([NotNullWhen(true)] out SourceText? result) => _textDocument.TryGetText(out result);

    public bool TryGetTextVersion(out VersionStamp result) => _textDocument.TryGetTextVersion(out result);

    public ImmutableArray<IDocumentSnapshot> GetImports()
    {
        return DocumentState.GetImportsCore(Project, FilePath.AssumeNotNull(), FileKind.AssumeNotNull());
    }

    public async Task<RazorCodeDocument> GetGeneratedOutputAsync()
    {
        // TODO: We don't need to worry about locking if we get called from the didOpen/didChange LSP requests, as CLaSP
        //       takes care of that for us, and blocks requests until those are complete. If that doesn't end up happening,
        //       then a locking mechanism here would prevent concurrent compilations.
        if (_codeDocument is not null)
        {
            return _codeDocument;
        }

        // The non-cohosted DocumentSnapshot implementation uses DocumentState to get the generated output, and we could do that too
        // but most of that code is optimized around caching pre-computed results when things change that don't affect the compilation.
        // We can't do that here because we are using Roslyn's project snapshots, which don't contain the info that Razor needs. We could
        // in future provide a side-car mechanism so we can cache things, but still take advantage of snapshots etc. but the working
        // assumption for this code is that the source generator will be used, and it will do all of that, so this implementation is naive
        // and simply compiles when asked, and if a new document snapshot comes in, we compile again. This is presumably worse for perf
        // but since we don't expect users to ever use cohosting without source generators, it's fine for now.

        var imports = await DocumentState.ComputedStateTracker.GetImportsAsync(this).ConfigureAwait(false);
        _codeDocument = await DocumentState.ComputedStateTracker.GenerateCodeDocumentAsync(Project, this, imports).ConfigureAwait(false);

        return _codeDocument;
    }

    public bool TryGetGeneratedOutput([NotNullWhen(true)] out RazorCodeDocument? result)
    {
        result = _codeDocument;
        return result is not null;
    }

    public IDocumentSnapshot WithText(SourceText text)
    {
        var id = _textDocument.Id;
        var newDocument = _textDocument.Project.Solution.WithAdditionalDocumentText(id, text).GetAdditionalDocument(id).AssumeNotNull();

        return new CohostDocumentSnapshot(newDocument, _projectSnapshot);
    }
}
