// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using RazorSyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class DocumentContext(Uri uri, IDocumentSnapshot snapshot, VSProjectContext? projectContext)
{
    private readonly VSProjectContext? _projectContext = projectContext;
    private RazorCodeDocument? _codeDocument;
    private SourceText? _sourceText;

    public Uri Uri { get; } = uri;
    public IDocumentSnapshot Snapshot { get; } = snapshot;
    public string FilePath => Snapshot.FilePath;
    public string FileKind => Snapshot.FileKind;
    public IProjectSnapshot Project => Snapshot.Project;

    public TextDocumentIdentifier GetTextDocumentIdentifier()
        => new VSTextDocumentIdentifier()
        {
            Uri = Uri,
            ProjectContext = _projectContext,
        };

    public TextDocumentIdentifierAndVersion GetTextDocumentIdentifierAndVersion()
       => new(GetTextDocumentIdentifier(), Snapshot.Version);

    private bool TryGetCodeDocument([NotNullWhen(true)] out RazorCodeDocument? codeDocument)
    {
        codeDocument = _codeDocument;
        return codeDocument is not null;
    }

    public ValueTask<RazorCodeDocument> GetCodeDocumentAsync(CancellationToken cancellationToken)
    {
        return TryGetCodeDocument(out var codeDocument)
            ? new(codeDocument)
            : GetCodeDocumentCoreAsync(cancellationToken);

        async ValueTask<RazorCodeDocument> GetCodeDocumentCoreAsync(CancellationToken cancellationToken)
        {
            var codeDocument = await Snapshot
                .GetGeneratedOutputAsync(cancellationToken)
                .ConfigureAwait(false);

            // Interlock to ensure that we only ever return one instance of RazorCodeDocument.
            // In race scenarios, when more than one RazorCodeDocument is produced, we want to
            // return whichever RazorCodeDocument is cached.
            return InterlockedOperations.Initialize(ref _codeDocument, codeDocument);
        }
    }

    public ValueTask<SourceText> GetSourceTextAsync(CancellationToken cancellationToken)
    {
        return _sourceText is SourceText sourceText
            ? new(sourceText)
            : GetSourceTextCoreAsync(cancellationToken);

        async ValueTask<SourceText> GetSourceTextCoreAsync(CancellationToken cancellationToken)
        {
            var sourceText = await Snapshot.GetTextAsync(cancellationToken).ConfigureAwait(false);

            // Interlock to ensure that we only ever return one instance of RazorCodeDocument.
            // In race scenarios, when more than one RazorCodeDocument is produced, we want to
            // return whichever RazorCodeDocument is cached.
            return InterlockedOperations.Initialize(ref _sourceText, sourceText);
        }
    }

    public ValueTask<RazorSyntaxTree> GetSyntaxTreeAsync(CancellationToken cancellationToken)
    {
        return TryGetCodeDocument(out var codeDocument)
            ? new(GetSyntaxTreeCore(codeDocument))
            : GetSyntaxTreeCoreAsync(cancellationToken);

        static RazorSyntaxTree GetSyntaxTreeCore(RazorCodeDocument codeDocument)
        {
            return codeDocument.GetSyntaxTree().AssumeNotNull();
        }

        async ValueTask<RazorSyntaxTree> GetSyntaxTreeCoreAsync(CancellationToken cancellationToken)
        {
            var codeDocument = await GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
            return GetSyntaxTreeCore(codeDocument);
        }
    }

    public ValueTask<TagHelperDocumentContext> GetTagHelperContextAsync(CancellationToken cancellationToken)
    {
        return TryGetCodeDocument(out var codeDocument)
            ? new(GetTagHelperContextCore(codeDocument))
            : GetTagHelperContextCoreAsync(cancellationToken);

        static TagHelperDocumentContext GetTagHelperContextCore(RazorCodeDocument codeDocument)
        {
            return codeDocument.GetTagHelperContext().AssumeNotNull();
        }

        async ValueTask<TagHelperDocumentContext> GetTagHelperContextCoreAsync(CancellationToken cancellationToken)
        {
            var codeDocument = await GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
            return GetTagHelperContextCore(codeDocument);
        }
    }

    public ValueTask<SourceText> GetCSharpSourceTextAsync(CancellationToken cancellationToken)
    {
        return TryGetCodeDocument(out var codeDocument)
            ? new(GetCSharpSourceTextCore(codeDocument))
            : GetCSharpSourceTextCoreAsync(cancellationToken);

        static SourceText GetCSharpSourceTextCore(RazorCodeDocument codeDocument)
        {
            return codeDocument.GetCSharpSourceText();
        }

        async ValueTask<SourceText> GetCSharpSourceTextCoreAsync(CancellationToken cancellationToken)
        {
            var codeDocument = await GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
            return GetCSharpSourceTextCore(codeDocument);
        }
    }

    public ValueTask<SourceText> GetHtmlSourceTextAsync(CancellationToken cancellationToken)
    {
        return TryGetCodeDocument(out var codeDocument)
            ? new(GetHtmlSourceTextCore(codeDocument))
            : GetHtmlSourceTextCoreAsync(cancellationToken);

        static SourceText GetHtmlSourceTextCore(RazorCodeDocument codeDocument)
        {
            return codeDocument.GetHtmlSourceText();
        }

        async ValueTask<SourceText> GetHtmlSourceTextCoreAsync(CancellationToken cancellationToken)
        {
            var codeDocument = await GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
            return GetHtmlSourceTextCore(codeDocument);
        }
    }

    public ValueTask<RazorSyntaxNode?> GetSyntaxNodeAsync(int absoluteIndex, CancellationToken cancellationToken)
    {
        return TryGetCodeDocument(out var codeDocument)
            ? new(GetSyntaxNodeCore(codeDocument, absoluteIndex))
            : GetSyntaxNodeCoreAsync(absoluteIndex, cancellationToken);

        static RazorSyntaxNode? GetSyntaxNodeCore(RazorCodeDocument codeDocument, int absoluteIndex)
        {
            var syntaxTree = codeDocument.GetSyntaxTree().AssumeNotNull();

            return syntaxTree.Root is RazorSyntaxNode root
                ? root.FindInnermostNode(absoluteIndex)
                : null;
        }

        async ValueTask<RazorSyntaxNode?> GetSyntaxNodeCoreAsync(int absoluteIndex, CancellationToken cancellationToken)
        {
            var codeDocument = await GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
            return GetSyntaxNodeCore(codeDocument, absoluteIndex);
        }
    }
}
