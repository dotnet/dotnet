// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class DocumentContext
{
    private RazorCodeDocument? _codeDocument;
    private SourceText? _sourceText;

    public DocumentContext(
        Uri uri,
        DocumentSnapshot snapshot)
    {
        Uri = uri;
        Snapshot = snapshot;
    }

    public virtual Uri Uri { get; }

    public virtual DocumentSnapshot Snapshot { get; }

    public virtual string FilePath => Snapshot.FilePath;

    public virtual string FileKind => Snapshot.FileKind;

    public virtual ProjectSnapshot Project => Snapshot.Project;

    public virtual TextDocumentIdentifier Identifier => new VersionedTextDocumentIdentifier()
    {
        Uri = Uri
    };

    public virtual async Task<RazorCodeDocument> GetCodeDocumentAsync(CancellationToken cancellationToken)
    {
        if (_codeDocument is null)
        {
            var codeDocument = await Snapshot.GetGeneratedOutputAsync().ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            _codeDocument = codeDocument;
        }

        return _codeDocument;
    }

    public virtual async Task<SourceText> GetSourceTextAsync(CancellationToken cancellationToken)
    {
        if (_sourceText is null)
        {
            var sourceText = await Snapshot.GetTextAsync().ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            _sourceText = sourceText;
        }

        return _sourceText;
    }

    public virtual async Task<RazorSyntaxTree> GetSyntaxTreeAsync(CancellationToken cancellationToken)
    {
        var codeDocument = await GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var syntaxTree = codeDocument.GetSyntaxTree();

        return syntaxTree;
    }

    public virtual async Task<TagHelperDocumentContext> GetTagHelperContextAsync(CancellationToken cancellationToken)
    {
        var codeDocument = await GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var tagHelperContext = codeDocument.GetTagHelperContext();

        return tagHelperContext;
    }

    public virtual async Task<SourceText> GetCSharpSourceTextAsync(CancellationToken cancellationToken)
    {
        var codeDocument = await GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var sourceText = codeDocument.GetCSharpSourceText();

        return sourceText;
    }

    public virtual async Task<SourceText> GetHtmlSourceTextAsync(CancellationToken cancellationToken)
    {
        var codeDocument = await GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var sourceText = codeDocument.GetHtmlSourceText();

        return sourceText;
    }

    public async Task<SyntaxNode?> GetSyntaxNodeAsync(int absoluteIndex, CancellationToken cancellationToken)
    {
        var change = new SourceChange(absoluteIndex, length: 0, newText: string.Empty);
        var syntaxTree = await GetSyntaxTreeAsync(cancellationToken);
        if (syntaxTree.Root is null)
        {
            return null;
        }

        return syntaxTree.Root.LocateOwner(change);
    }
}
