// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Protocol.DevTools;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Remote.Razor;

using SyntaxNode = AspNetCore.Razor.Language.Syntax.SyntaxNode;

internal sealed class RemoteDevToolsService(in ServiceArgs args) : RazorDocumentServiceBase(in args), IRemoteDevToolsService
{
    internal sealed class Factory : FactoryBase<IRemoteDevToolsService>
    {
        protected override IRemoteDevToolsService CreateService(in ServiceArgs args)
            => new RemoteDevToolsService(in args);
    }

    public ValueTask<string> GetCSharpDocumentTextAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        DocumentId razorDocumentId,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            razorDocumentId,
            async context =>
            {
                var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
                return codeDocument.GetCSharpSourceText().ToString();
            },
            cancellationToken);

    public ValueTask<string> GetHtmlDocumentTextAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        DocumentId razorDocumentId,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            razorDocumentId,
            async context =>
            {
                var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
                return codeDocument.GetHtmlSourceText().ToString();
            },
            cancellationToken);

    public ValueTask<string> GetFormattingDocumentTextAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        DocumentId razorDocumentId,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            razorDocumentId,
            async context =>
            {
                var codeDocument = await context.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
#pragma warning disable CS0618 // Type or member is obsolete
                return CSharpFormattingPass.GetFormattingDocumentContentsForSyntaxVisualizer(codeDocument);
#pragma warning restore CS0618 // Type or member is obsolete
            },
            cancellationToken);

    public ValueTask<FetchTagHelpersResult> GetTagHelpersJsonAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        DocumentId razorDocumentId,
        TagHelpersKind kind,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            razorDocumentId,
            context => GetTagHelpersJsonAsync(context, kind, cancellationToken),
            cancellationToken);

    private static async ValueTask<FetchTagHelpersResult> GetTagHelpersJsonAsync(RemoteDocumentContext documentContext, TagHelpersKind kind, CancellationToken cancellationToken)
    {
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var tagHelpers = kind switch
        {
            TagHelpersKind.All => codeDocument.GetTagHelpers(),
            TagHelpersKind.InScope => codeDocument.GetRequiredTagHelperContext().TagHelpers,
            TagHelpersKind.Referenced => codeDocument.GetReferencedTagHelpers(),
            _ => []
        };

        tagHelpers ??= [];
        return new FetchTagHelpersResult(tagHelpers);
    }

    public ValueTask<SyntaxVisualizerTree?> GetRazorSyntaxTreeAsync(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        DocumentId razorDocumentId,
        CancellationToken cancellationToken)
        => RunServiceAsync(
            solutionInfo,
            razorDocumentId,
            context => GetRazorSyntaxTreeAsync(context, cancellationToken),
            cancellationToken);

    private static async ValueTask<SyntaxVisualizerTree?> GetRazorSyntaxTreeAsync(RemoteDocumentContext documentContext, CancellationToken cancellationToken)
    {
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var razorSyntaxTree = codeDocument.GetSyntaxTree();

        if (razorSyntaxTree?.Root == null)
            return null;

        return new SyntaxVisualizerTree
        {
            Root = ConvertSyntaxNode(razorSyntaxTree.Root)
        };
    }

    private static SyntaxVisualizerNode ConvertSyntaxNode(SyntaxNode node)
        => new SyntaxVisualizerNode
        {
            Kind = node.Kind.ToString(),
            SpanStart = node.SpanStart,
            SpanEnd = node.Span.End,
            Children = [.. node.ChildNodes().Select(ConvertSyntaxNode)]
        };
}
