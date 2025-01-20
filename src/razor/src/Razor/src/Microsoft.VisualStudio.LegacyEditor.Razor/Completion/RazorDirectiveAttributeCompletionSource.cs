// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.Core.Imaging;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data;
using Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Completion;

internal class RazorDirectiveAttributeCompletionSource : IAsyncCompletionSource
{
    // Internal for testing
    internal static readonly object DescriptionKey = new();

    // Hardcoding the Guid here to avoid a reference to Microsoft.VisualStudio.ImageCatalog.dll
    // that is not present in Visual Studio for Mac
    private static readonly Guid s_imageCatalogGuid = new("{ae27a6b0-e345-4288-96df-5eaf394ee369}");
    private static readonly ImageElement s_directiveAttributeImageGlyph = new(
        new ImageId(s_imageCatalogGuid, 3564), // KnownImageIds.Type = 3564
        "Razor Directive Attribute.");
    private static readonly ImmutableArray<CompletionFilter> s_directiveAttributeCompletionFilters = new[] {
        new CompletionFilter("Razor Directive Attrsibute", "r", s_directiveAttributeImageGlyph)
    }.ToImmutableArray();

    private readonly IVisualStudioRazorParser _parser;
    private readonly IRazorCompletionFactsService _completionFactsService;
    private readonly ICompletionBroker _completionBroker;
    private readonly IVisualStudioDescriptionFactory _descriptionFactory;
    private readonly JoinableTaskFactory _joinableTaskFactory;

    public RazorDirectiveAttributeCompletionSource(
        IVisualStudioRazorParser parser,
        IRazorCompletionFactsService completionFactsService,
        ICompletionBroker completionBroker,
        IVisualStudioDescriptionFactory descriptionFactory,
        JoinableTaskFactory joinableTaskFactory)
    {
        _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        _completionFactsService = completionFactsService ?? throw new ArgumentNullException(nameof(completionFactsService));
        _completionBroker = completionBroker;
        _descriptionFactory = descriptionFactory ?? throw new ArgumentNullException(nameof(descriptionFactory));
        _joinableTaskFactory = joinableTaskFactory;
    }

    public async Task<CompletionContext> GetCompletionContextAsync(IAsyncCompletionSession session, CompletionTrigger trigger, SnapshotPoint triggerLocation, SnapshotSpan applicableToSpan, CancellationToken token)
    {
        try
        {
            Debug.Assert(triggerLocation.Snapshot.TextBuffer.IsLegacyCoreRazorBuffer());

            var codeDocument = await _parser.GetLatestCodeDocumentAsync(triggerLocation.Snapshot, token);
            if (codeDocument is null)
            {
                // Code document not available yet.
                return CompletionContext.Empty;
            }

            var syntaxTree = codeDocument.GetSyntaxTree();
            var tagHelperDocumentContext = codeDocument.GetTagHelperContext();
            var absoluteIndex = triggerLocation.Position;
            var queryableChange = new SourceChange(absoluteIndex, length: 0, newText: string.Empty);
#pragma warning disable CS0618 // Type or member is obsolete, will be removed in an upcoming change
            var owner = syntaxTree.Root.LocateOwner(queryableChange);
#pragma warning restore CS0618 // Type or member is obsolete
            var razorCompletionContext = new RazorCompletionContext(absoluteIndex, owner, syntaxTree, tagHelperDocumentContext);
            var razorCompletionItems = _completionFactsService.GetCompletionItems(razorCompletionContext);

            if (razorCompletionItems.Length == 0)
            {
                return CompletionContext.Empty;
            }

            // Check if we're providing completion items while a legacy completion session is active. If so
            // we'll need to dismiss the legacy completion session to ensure we don't get two completion lists.
            var activeSessions = _completionBroker.GetSessions(session.TextView);
            foreach (var activeSession in activeSessions)
            {
                if (activeSession.Properties.ContainsProperty(nameof(IAsyncCompletionSession)))
                {
                    continue;
                }

                // Legacy completion is also active, we need to dismiss it.

                await _joinableTaskFactory.SwitchToMainThreadAsync(token);

                activeSession.Dismiss();
            }

            using var _ = ArrayBuilderPool<CompletionItem>.GetPooledObject(out var completionItems);
            var completionItemKinds = new HashSet<RazorCompletionItemKind>();

            foreach (var razorCompletionItem in razorCompletionItems)
            {
                if (razorCompletionItem.Kind != RazorCompletionItemKind.DirectiveAttribute &&
                    razorCompletionItem.Kind != RazorCompletionItemKind.DirectiveAttributeParameter)
                {
                    // Don't support any other types of completion kinds other than directive attributes and their parameters.
                    continue;
                }

                var completionItem = new CompletionItem(
                    displayText: razorCompletionItem.DisplayText,
                    filterText: razorCompletionItem.DisplayText,
                    insertText: razorCompletionItem.InsertText,
                    source: this,
                    icon: s_directiveAttributeImageGlyph,
                    filters: s_directiveAttributeCompletionFilters,
                    suffix: string.Empty,
                    sortText: razorCompletionItem.DisplayText,
                    attributeIcons: ImmutableArray<ImageElement>.Empty);
                completionItems.Add(completionItem);
                completionItemKinds.Add(razorCompletionItem.Kind);

                var completionDescription = razorCompletionItem.DescriptionInfo as AggregateBoundAttributeDescription;
                completionItem.Properties[DescriptionKey] = completionDescription;
            }

            session.Properties.SetCompletionItemKinds(completionItemKinds);

            completionItems.Sort(CompletionItemDisplayTextComparer.Instance);

            return new CompletionContext(completionItems.ToImmutable());
        }
        catch (OperationCanceledException)
        {
            return CompletionContext.Empty;
        }
    }

    public Task<object> GetDescriptionAsync(IAsyncCompletionSession session, CompletionItem item, CancellationToken token)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        if (!item.Properties.TryGetProperty(DescriptionKey, out AggregateBoundAttributeDescription completionDescription))
        {
            return Task.FromResult<object>(string.Empty);
        }

        var description = _descriptionFactory.CreateClassifiedDescription(completionDescription);

        return Task.FromResult<object>(description);
    }

    public CompletionStartData InitializeCompletion(CompletionTrigger trigger, SnapshotPoint triggerLocation, CancellationToken token)
    {
        // We can't retrieve the correct SyntaxTree/CodeDocument at this time because this extension point is synchronous so we need
        // to make our "do we participate in completion and what do we apply to" decision without one. We'll look to see if what
        // we're operating on potentially looks like a directive attribute. We care about syntax that looks like an expression when
        // providing directive attribute completions. Basically anything starting with a transition (@).

        var snapshot = triggerLocation.Snapshot;

        // 4 because of the minimal situation possible "<a @|"
        if (snapshot.Length < 4)
        {
            // Empty document, can not provide completions.
            return CompletionStartData.DoesNotParticipateInCompletion;
        }

        if (triggerLocation.Position == 0)
        {
            // Completion triggered at beginning of document, can't possibly be an attribute.
            return CompletionStartData.DoesNotParticipateInCompletion;
        }

        var leftEnd = triggerLocation.Position - 1;
        for (; leftEnd > 0; leftEnd--)
        {
            var currentCharacter = snapshot[leftEnd];

            if (char.IsWhiteSpace(currentCharacter))
            {
                // Valid left end attribute delimiter
                leftEnd++;
                break;
            }
            else if (IsInvalidAttributeDelimiter(currentCharacter))
            {
                return CompletionStartData.DoesNotParticipateInCompletion;
            }
        }

        if (leftEnd >= snapshot.Length)
        {
            // Left part of the trigger is at the very end of the document without a possible transition
            return CompletionStartData.DoesNotParticipateInCompletion;
        }

        var leftMostCharacter = snapshot[leftEnd];
        if (leftMostCharacter != '@')
        {
            // The left side of our simple expression should always be a Razor transition. We have this restriction to
            // ensure that we don't provide directive attribute completions that override WTE's legacy completion.
            // Since WTE's legacy completion also provides HTML completions and is not yet on modern completion we'd
            // end up removing all HTML completions without this restriction.
            return CompletionStartData.DoesNotParticipateInCompletion;
        }

        var rightEnd = triggerLocation.Position;
        for (; rightEnd < snapshot.Length; rightEnd++)
        {
            var currentCharacter = snapshot[rightEnd];

            if (char.IsWhiteSpace(currentCharacter) || currentCharacter == '=' || IsInvalidAttributeDelimiter(currentCharacter))
            {
                // Right hand side of the current attribute
                break;
            }
        }

        var parameterDelimiter = -1;
        for (var i = leftEnd; i < rightEnd; i++)
        {
            if (snapshot[i] == ':')
            {
                parameterDelimiter = i;
            }
        }

        if (parameterDelimiter != -1)
        {
            // There's a parameter delimiter in the expression that we've triggered on. We need to decide which side will
            // be the applicable to span.

            if (triggerLocation.Position <= parameterDelimiter)
            {
                // The trigger location falls on the left hand side of the directive attribute parameter delimiter (:)
                //
                // <InputSelect |@bind-foo|:something
                rightEnd = parameterDelimiter;

                // Next we need to move our left end to not include the "@"
                //
                // <InputSelect |@bind-foo|:something   =>   <InputSelect @|bind-foo|:something
                leftEnd++;
            }
            else
            {
                // The trigger location falls on the right hand side of the directive attribute parameter delimiter (:)
                //
                // <InputSelect @bind-foo:|something|
                leftEnd = parameterDelimiter + 1;
            }
        }
        else
        {
            // Our directive attribute does not have parameters and our leftEnd -> rightEnd bounds encompass:
            //
            // <InputSelect |@bind-foo|  =>  <InputSelect @|bind-foo|
            //
            // We don't want leftEnd to include the "@"
            leftEnd++;
        }

        for (var i = leftEnd; i < rightEnd; i++)
        {
            if (!IsValidDirectiveAttributeCharacter(snapshot[i]))
            {
                return CompletionStartData.DoesNotParticipateInCompletion;
            }
        }

        var applicableSpanLength = rightEnd - leftEnd;
        var applicableToSpan = new SnapshotSpan(triggerLocation.Snapshot, leftEnd, applicableSpanLength);

        return new CompletionStartData(CompletionParticipation.ProvidesItems, applicableToSpan);
    }

    private static bool IsInvalidAttributeDelimiter(char currentCharacter)
    {
        return currentCharacter == '<' || currentCharacter == '>' || currentCharacter == '\'' || currentCharacter == '"' || currentCharacter == '/';
    }

    private static bool IsValidDirectiveAttributeCharacter(char currentCharacter)
    {
        return char.IsLetter(currentCharacter) || currentCharacter == '-';
    }
}
