// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class DocumentContext
{
    private RazorCodeDocument? _codeDocument;
    private SourceText? _sourceText;
    private readonly VSProjectContext? _projectContext;

    public DocumentContext(
        Uri uri,
        IDocumentSnapshot snapshot,
        VSProjectContext? projectContext)
    {
        Uri = uri;
        Snapshot = snapshot;
        _projectContext = projectContext;
    }

    public virtual Uri Uri { get; }

    public virtual IDocumentSnapshot Snapshot { get; }

    public virtual string FilePath => Snapshot.FilePath.AssumeNotNull();

    public virtual string FileKind => Snapshot.FileKind.AssumeNotNull();

    public virtual IProjectSnapshot Project => Snapshot.Project;

    public virtual TextDocumentIdentifier Identifier => new VSTextDocumentIdentifier()
    {
        Uri = Uri,
        ProjectContext = _projectContext,
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
        var syntaxTree = await GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
        if (syntaxTree.Root is null)
        {
            return null;
        }

        return syntaxTree.Root.FindInnermostNode(absoluteIndex);
    }
}
