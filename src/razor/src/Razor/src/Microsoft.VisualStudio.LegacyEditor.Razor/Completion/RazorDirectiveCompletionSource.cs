// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.VisualStudio.Core.Imaging;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data;
using Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Completion;

internal class RazorDirectiveCompletionSource : IAsyncCompletionSource
{
    // Internal for testing
    internal static readonly object DescriptionKey = new();
    // Hardcoding the Guid here to avoid a reference to Microsoft.VisualStudio.ImageCatalog.dll
    // that is not present in Visual Studio for Mac
    internal static readonly Guid ImageCatalogGuid = new("{ae27a6b0-e345-4288-96df-5eaf394ee369}");
    internal static readonly ImageElement DirectiveImageGlyph = new(
        new ImageId(ImageCatalogGuid, 3233), // KnownImageIds.Type = 3233
        "Razor Directive.");
    internal static readonly ImmutableArray<CompletionFilter> DirectiveCompletionFilters = new[] {
        new CompletionFilter("Razor Directive", "r", DirectiveImageGlyph)
    }.ToImmutableArray();

    // Internal for testing
    internal readonly IVisualStudioRazorParser Parser;
    private readonly IRazorCompletionFactsService _completionFactsService;

    public RazorDirectiveCompletionSource(
        IVisualStudioRazorParser parser,
        IRazorCompletionFactsService completionFactsService)
    {
        if (parser is null)
        {
            throw new ArgumentNullException(nameof(parser));
        }

        if (completionFactsService is null)
        {
            throw new ArgumentNullException(nameof(completionFactsService));
        }

        Parser = parser;
        _completionFactsService = completionFactsService;
    }

    public async Task<CompletionContext> GetCompletionContextAsync(
        IAsyncCompletionSession session,
        CompletionTrigger trigger,
        SnapshotPoint triggerLocation,
        SnapshotSpan applicableSpan,
        CancellationToken token)
    {
        try
        {
            Debug.Assert(triggerLocation.Snapshot.TextBuffer.IsLegacyCoreRazorBuffer());

            var codeDocument = await Parser.GetLatestCodeDocumentAsync(triggerLocation.Snapshot, token);
            if (codeDocument is null)
            {
                return CompletionContext.Empty;
            }

            var location = new SourceSpan(triggerLocation.Position, 0);
            var syntaxTree = codeDocument.GetSyntaxTree();
            var tagHelperDocumentContext = codeDocument.GetTagHelperContext();
            var absoluteIndex = triggerLocation.Position;
            var queryableChange = new SourceChange(absoluteIndex, length: 0, newText: string.Empty);
#pragma warning disable CS0618 // Type or member is obsolete, will be removed in an upcoming change
            var owner = syntaxTree.Root.LocateOwner(queryableChange);
#pragma warning restore CS0618 // Type or member is obsolete
            var razorCompletionContext = new RazorCompletionContext(absoluteIndex, owner, syntaxTree, tagHelperDocumentContext);
            var razorCompletionItems = _completionFactsService.GetCompletionItems(razorCompletionContext);

            using var _ = ArrayBuilderPool<CompletionItem>.GetPooledObject(out var completionItems);

            foreach (var razorCompletionItem in razorCompletionItems)
            {
                if (razorCompletionItem.Kind != RazorCompletionItemKind.Directive)
                {
                    // Don't support any other types of completion kinds other than directives.
                    continue;
                }

                var completionItem = new CompletionItem(
                    displayText: razorCompletionItem.DisplayText,
                    filterText: razorCompletionItem.DisplayText,
                    insertText: razorCompletionItem.InsertText,
                    source: this,
                    icon: DirectiveImageGlyph,
                    filters: DirectiveCompletionFilters,
                    suffix: string.Empty,
                    sortText: razorCompletionItem.DisplayText,
                    attributeIcons: ImmutableArray<ImageElement>.Empty);

                var completionDescription = razorCompletionItem.DescriptionInfo as DirectiveCompletionDescription;
                completionItem.Properties.AddProperty(DescriptionKey, completionDescription);
                completionItems.Add(completionItem);
            }

            return new CompletionContext(completionItems.ToImmutable());
        }
        catch (OperationCanceledException)
        {
            return CompletionContext.Empty;
        }
    }

    public Task<object> GetDescriptionAsync(IAsyncCompletionSession session, CompletionItem item, CancellationToken token)
    {
        if (!item.Properties.TryGetProperty(DescriptionKey, out DirectiveCompletionDescription directiveDescription))
        {
            return Task.FromResult<object>(string.Empty);
        }

        return Task.FromResult<object>(directiveDescription.Description);
    }

    public CompletionStartData InitializeCompletion(CompletionTrigger trigger, SnapshotPoint triggerLocation, CancellationToken token)
    {
        // The applicable span for completion is the piece of text a completion is for. For example:
        //      @Date|Time.Now
        // If you trigger completion at the | then the applicable span is the region of 'DateTime'; however, Razor
        // doesn't know this information so we rely on Roslyn to define what the applicable span for a completion is.
        return CompletionStartData.ParticipatesInCompletionIfAny;
    }
}
