// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.ProjectSystem.Legacy;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal sealed class DocumentSnapshot(ProjectSnapshot project, DocumentState state) : IDocumentSnapshot, ILegacyDocumentSnapshot
#if !FORMAT_FUSE
    , IDesignTimeCodeGenerator
#endif
{
    public ProjectSnapshot Project { get; } = project;

    private readonly DocumentState _state = state;

    public HostDocument HostDocument => _state.HostDocument;

    public string FileKind => _state.HostDocument.FileKind;
    public string FilePath => _state.HostDocument.FilePath;
    public string TargetPath => _state.HostDocument.TargetPath;
    public int Version => _state.Version;

    IProjectSnapshot IDocumentSnapshot.Project => Project;

    public bool TryGetText([NotNullWhen(true)] out SourceText? result)
        => _state.TryGetText(out result);

    public ValueTask<SourceText> GetTextAsync(CancellationToken cancellationToken)
        => _state.GetTextAsync(cancellationToken);

    public bool TryGetTextVersion(out VersionStamp result)
        => _state.TryGetTextVersion(out result);

    public ValueTask<VersionStamp> GetTextVersionAsync(CancellationToken cancellationToken)
        => _state.GetTextVersionAsync(cancellationToken);

    public bool TryGetGeneratedOutput([NotNullWhen(true)] out RazorCodeDocument? result)
        => _state.TryGetGeneratedOutput(out result);

    public ValueTask<RazorCodeDocument> GetGeneratedOutputAsync(CancellationToken cancellationToken)
        => _state.GetGeneratedOutputAsync(this, cancellationToken);

    public IDocumentSnapshot WithText(SourceText text)
    {
        return new DocumentSnapshot(Project, _state.WithText(text, VersionStamp.Create()));
    }

    public ValueTask<SyntaxTree> GetCSharpSyntaxTreeAsync(CancellationToken cancellationToken)
    {
        return TryGetGeneratedOutput(out var codeDocument)
            ? new(codeDocument.GetOrParseCSharpSyntaxTree(cancellationToken))
            : new(GetCSharpSyntaxTreeCoreAsync(cancellationToken));

        async Task<SyntaxTree> GetCSharpSyntaxTreeCoreAsync(CancellationToken cancellationToken)
        {
            var codeDocument = await GetGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);
            return codeDocument.GetOrParseCSharpSyntaxTree(cancellationToken);
        }
    }

#if !FORMAT_FUSE
    public Task<RazorCodeDocument> GenerateDesignTimeOutputAsync(CancellationToken cancellationToken)
        => CompilationHelpers.GenerateDesignTimeCodeDocumentAsync(this, Project.ProjectEngine, cancellationToken);
#endif

    #region ILegacyDocumentSnapshot support

    string ILegacyDocumentSnapshot.FileKind => FileKind;

    #endregion
}
