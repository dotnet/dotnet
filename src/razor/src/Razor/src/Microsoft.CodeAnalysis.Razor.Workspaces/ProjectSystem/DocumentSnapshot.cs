// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal sealed class DocumentSnapshot(ProjectSnapshot project, DocumentState state) : IDocumentSnapshot
{
    private static readonly object s_csharpSyntaxTreeKey = new();

    private readonly DocumentState _state = state;
    private readonly ProjectSnapshot _project = project;

    public HostDocument HostDocument => _state.HostDocument;

    public string FileKind => _state.HostDocument.FileKind;
    public string FilePath => _state.HostDocument.FilePath;
    public string TargetPath => _state.HostDocument.TargetPath;
    public IProjectSnapshot Project => _project;
    public int Version => _state.Version;

    public ValueTask<SourceText> GetTextAsync(CancellationToken cancellationToken)
        => _state.GetTextAsync(cancellationToken);

    public ValueTask<VersionStamp> GetTextVersionAsync(CancellationToken cancellationToken)
        => _state.GetTextVersionAsync(cancellationToken);

    public bool TryGetText([NotNullWhen(true)] out SourceText? result)
        => _state.TryGetText(out result);

    public bool TryGetTextVersion(out VersionStamp result)
        => _state.TryGetTextVersion(out result);

    public bool TryGetGeneratedOutput([NotNullWhen(true)] out RazorCodeDocument? result)
    {
        if (_state.TryGetGeneratedOutputAndVersion(out var outputAndVersion))
        {
            result = outputAndVersion.output;
            return true;
        }

        result = null;
        return false;
    }

    public IDocumentSnapshot WithText(SourceText text)
    {
        return new DocumentSnapshot(_project, _state.WithText(text, VersionStamp.Create()));
    }

    public ValueTask<SyntaxTree> GetCSharpSyntaxTreeAsync(CancellationToken cancellationToken)
    {
        return TryGetGeneratedOutput(out var codeDocument)
            ? new(GetOrParseCSharpSyntaxTree(codeDocument, cancellationToken))
            : new(GetCSharpSyntaxTreeCoreAsync(cancellationToken));

        async Task<SyntaxTree> GetCSharpSyntaxTreeCoreAsync(CancellationToken cancellationToken)
        {
            var codeDocument = await GetGeneratedOutputAsync(forceDesignTimeGeneratedOutput: false, cancellationToken).ConfigureAwait(false);
            return GetOrParseCSharpSyntaxTree(codeDocument, cancellationToken);
        }
    }

    public async ValueTask<RazorCodeDocument> GetGeneratedOutputAsync(bool forceDesignTimeGeneratedOutput, CancellationToken cancellationToken)
    {
        if (forceDesignTimeGeneratedOutput)
        {
            return await GetDesignTimeGeneratedOutputAsync(cancellationToken).ConfigureAwait(false);
        }

        var (output, _) = await _state
            .GetGeneratedOutputAndVersionAsync(_project, this, cancellationToken)
            .ConfigureAwait(false);

        return output;
    }

    private Task<RazorCodeDocument> GetDesignTimeGeneratedOutputAsync(CancellationToken cancellationToken)
        => DocumentState.GenerateCodeDocumentAsync(this, Project.GetProjectEngine(), forceRuntimeCodeGeneration: false, cancellationToken);

    /// <summary>
    ///  Retrieves a cached Roslyn <see cref="SyntaxTree"/> from the generated C# document.
    ///  If a tree has not yet been cached, a new one will be parsed and added to the cache.
    /// </summary>
    public static SyntaxTree GetOrParseCSharpSyntaxTree(RazorCodeDocument document, CancellationToken cancellationToken)
    {
        if (!document.Items.TryGetValue(s_csharpSyntaxTreeKey, out SyntaxTree? syntaxTree))
        {
            var csharpText = document.GetCSharpSourceText();
            syntaxTree = CSharpSyntaxTree.ParseText(csharpText, cancellationToken: cancellationToken);
            document.Items[s_csharpSyntaxTreeKey] = syntaxTree;

            return syntaxTree;
        }

        return syntaxTree.AssumeNotNull();
    }
}
