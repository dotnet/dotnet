// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal sealed class ComponentAccessibilityCodeActionProvider : IRazorCodeActionProvider
{
    private static readonly Task<IReadOnlyList<RazorVSInternalCodeAction>?> s_emptyResult = Task.FromResult<IReadOnlyList<RazorVSInternalCodeAction>?>(null);

    private readonly ITagHelperFactsService _tagHelperFactsService;

    public ComponentAccessibilityCodeActionProvider(ITagHelperFactsService tagHelperFactsService)
    {
        _tagHelperFactsService = tagHelperFactsService ?? throw new ArgumentNullException(nameof(tagHelperFactsService));
    }

    public Task<IReadOnlyList<RazorVSInternalCodeAction>?> ProvideAsync(RazorCodeActionContext context, CancellationToken cancellationToken)
    {
        using var _ = ListPool<RazorVSInternalCodeAction>.GetPooledObject(out var codeActions);

        // Locate cursor
        var node = context.CodeDocument.GetSyntaxTree().Root.FindInnermostNode(context.Location.AbsoluteIndex);
        if (node is null)
        {
            return s_emptyResult;
        }

        // Find start tag. We allow this code action to work from anywhere in the start tag, which includes
        // embedded C#, so we just have to traverse up the tree to find a start tag if there is one.
        // We also check for tag helper start tags here, because an invalid start tag with a valid tag helper
        // anywhere in it  would otherwise not match. We rely on the IsTagUnknown method below, to ensure we
        // only offer on actual potential component tags (because it checks for the compiler diagnostic)
        var startTag = (IStartTagSyntaxNode?)node.FirstAncestorOrSelf<SyntaxNode>(n => n is IStartTagSyntaxNode);
        if (startTag is null)
        {
            return s_emptyResult;
        }

        if (context.Location.AbsoluteIndex < startTag.SpanStart)
        {
            // Cursor is before the start tag, so we shouldn't show a light bulb. This can happen
            // in cases where the cursor is in whitespace at the beginning of the document
            // eg: $$ <Component></Component>
            return s_emptyResult;
        }

        // Ignore if start tag has dots, as we only handle short tags
        if (startTag.Name.Content.Contains("."))
        {
            return s_emptyResult;
        }

        if (!IsApplicableTag(startTag))
        {
            return s_emptyResult;
        }

        if (IsTagUnknown(startTag, context))
        {
            AddComponentAccessFromTag(context, startTag, codeActions);
            AddCreateComponentFromTag(context, startTag, codeActions);
        }

        return Task.FromResult<IReadOnlyList<RazorVSInternalCodeAction>?>(codeActions.ToArray());
    }

    private static bool IsApplicableTag(IStartTagSyntaxNode startTag)
    {
        if (startTag.Name.FullWidth == 0)
        {
            // Empty tag name, we shouldn't show a light bulb just to create an empty file.
            return false;
        }

        return true;
    }

    private void AddCreateComponentFromTag(RazorCodeActionContext context, IStartTagSyntaxNode startTag, List<RazorVSInternalCodeAction> container)
    {
        if (context is null)
        {
            return;
        }

        if (!context.SupportsFileCreation)
        {
            return;
        }

        var path = context.Request.TextDocument.Uri.GetAbsoluteOrUNCPath();
        path = FilePathNormalizer.Normalize(path);

        var directoryName = Path.GetDirectoryName(path);
        Assumes.NotNull(directoryName);

        var newComponentPath = Path.Combine(directoryName, $"{startTag.Name.Content}.razor");
        if (File.Exists(newComponentPath))
        {
            return;
        }

        var actionParams = new CreateComponentCodeActionParams
        {
            Uri = context.Request.TextDocument.Uri,
            Path = newComponentPath,
        };

        var resolutionParams = new RazorCodeActionResolutionParams
        {
            Action = LanguageServerConstants.CodeActions.CreateComponentFromTag,
            Language = LanguageServerConstants.CodeActions.Languages.Razor,
            Data = actionParams,
        };

        var codeAction = RazorCodeActionFactory.CreateComponentFromTag(resolutionParams);
        container.Add(codeAction);
    }

    private void AddComponentAccessFromTag(RazorCodeActionContext context, IStartTagSyntaxNode startTag, List<RazorVSInternalCodeAction> container)
    {
        var haveAddedNonQualifiedFix = false;

        // First see if there are any components that match in name, but not case, without qualification
        foreach (var t in context.CodeDocument.GetTagHelperContext().TagHelpers)
        {
            if (t.TagMatchingRules is [{ CaseSensitive: true } rule] &&
                rule.TagName.Equals(startTag.Name.Content, StringComparison.OrdinalIgnoreCase) &&
                rule.TagName != startTag.Name.Content)
            {
                var renameTagWorkspaceEdit = CreateRenameTagEdit(context, startTag, rule.TagName);
                var fixCasingCodeAction = RazorCodeActionFactory.CreateFullyQualifyComponent(rule.TagName, renameTagWorkspaceEdit);
                container.Add(fixCasingCodeAction);
                haveAddedNonQualifiedFix = true;
                break;
            }
        }

        var matching = FindMatchingTagHelpers(context, startTag);

        // For all the matches, add options for add @using and fully qualify
        foreach (var tagHelperPair in matching)
        {
            if (tagHelperPair._fullyQualified is null)
            {
                continue;
            }

            // If they have a typo, eg <CounTer /> and we've offered them <Counter /> above, then it would be odd to offer
            // them <BlazorApp.Pages.Counter /> as well. We will offer them <BlazorApp.MisTypedPages.CounTer /> though, if it
            // exists.
            if (!haveAddedNonQualifiedFix || !tagHelperPair._caseInsensitiveMatch)
            {
                // if fqn contains a generic typeparam, we should strip it out. Otherwise, replacing tag name will leave generic parameters in razor code, which are illegal
                // e.g. <Component /> -> <Component<T> />
                var fullyQualifiedName = DefaultRazorComponentSearchEngine.RemoveGenericContent(tagHelperPair._short.Name.AsMemory()).ToString();

                // If the match was case insensitive, then see if we can work out a new tag name to use as part of adding a using statement
                TextDocumentEdit? additionalEdit = null;
                string? newTagName = null;
                if (tagHelperPair._caseInsensitiveMatch)
                {
                    newTagName = tagHelperPair._short.TagMatchingRules.FirstOrDefault()?.TagName;
                    if (newTagName is not null)
                    {
                        additionalEdit = CreateRenameTagEdit(context, startTag, newTagName).DocumentChanges!.Value.First().First.AssumeNotNull();
                    }
                }

                // We only want to add a using statement if this was a case sensitive match, or if we were able to determine a new tag
                // name to give the tag.
                if (!tagHelperPair._caseInsensitiveMatch || newTagName is not null)
                {
                    if (AddUsingsCodeActionProviderHelper.TryCreateAddUsingResolutionParams(fullyQualifiedName, context.Request.TextDocument.Uri, additionalEdit, out var @namespace, out var resolutionParams))
                    {
                        var addUsingCodeAction = RazorCodeActionFactory.CreateAddComponentUsing(@namespace, newTagName, resolutionParams);
                        container.Add(addUsingCodeAction);
                    }
                }

                // Fully qualify
                var renameTagWorkspaceEdit = CreateRenameTagEdit(context, startTag, fullyQualifiedName);
                var fullyQualifiedCodeAction = RazorCodeActionFactory.CreateFullyQualifyComponent(fullyQualifiedName, renameTagWorkspaceEdit);
                container.Add(fullyQualifiedCodeAction);
            }
        }
    }

    private List<TagHelperPair> FindMatchingTagHelpers(RazorCodeActionContext context, IStartTagSyntaxNode startTag)
    {
        // Get all data necessary for matching
        var tagName = startTag.Name.Content;
        string? parentTagName = null;
        if (startTag.Parent?.Parent is MarkupElementSyntax parentElement)
        {
            parentTagName = parentElement.StartTag?.Name.Content ?? parentElement.EndTag?.Name.Content;
        }
        else if (startTag.Parent?.Parent is MarkupTagHelperElementSyntax parentTagHelperElement)
        {
            parentTagName = parentTagHelperElement.StartTag?.Name.Content ?? parentTagHelperElement.EndTag?.Name.Content;
        }

        var attributes = _tagHelperFactsService.StringifyAttributes(startTag.Attributes);

        // Find all matching tag helpers
        using var _ = DictionaryPool<string, TagHelperPair>.GetPooledObject(out var matching);

        foreach (var tagHelper in context.DocumentSnapshot.Project.TagHelpers)
        {
            if (SatisfiesRules(tagHelper.TagMatchingRules, tagName.AsSpan(), parentTagName.AsSpan(), attributes, out var caseInsensitiveMatch))
            {
                matching.Add(tagHelper.Name, new TagHelperPair(@short: tagHelper, caseInsensitiveMatch));
            }
        }

        // Iterate and find the fully qualified version
        foreach (var tagHelper in context.DocumentSnapshot.Project.TagHelpers)
        {
            if (matching.TryGetValue(tagHelper.Name, out var tagHelperPair))
            {
                if (tagHelperPair != null && tagHelper != tagHelperPair._short)
                {
                    tagHelperPair._fullyQualified = tagHelper;
                }
            }
        }

        return new List<TagHelperPair>(matching.Values);
    }

    private bool SatisfiesRules(ImmutableArray<TagMatchingRuleDescriptor> tagMatchingRules, ReadOnlySpan<char> tagNameWithoutPrefix, ReadOnlySpan<char> parentTagNameWithoutPrefix, ImmutableArray<KeyValuePair<string, string>> tagAttributes, out bool caseInsensitiveMatch)
    {
        caseInsensitiveMatch = false;

        foreach (var rule in tagMatchingRules)
        {
            // We have to match parent tag and attributes regardless, so check them first and exit early if there is a fail
            if (!TagHelperMatchingConventions.SatisfiesParentTag(parentTagNameWithoutPrefix, rule) ||
               !TagHelperMatchingConventions.SatisfiesAttributes(tagAttributes, rule))
            {
                return false;
            }

            // Tag helpers that target catch-all will come back as satisfying the rule, but we don't want to use them for the code action
            // so we have to check that ourselves
            if (rule.TagName is null or TagHelperMatchingConventions.ElementCatchAllName)
            {
                return false;
            }
            else if (TagHelperMatchingConventions.SatisfiesTagName(tagNameWithoutPrefix, rule))
            {
                // Nothing to do, just loop around to the next rule
            }
            else if (TagHelperMatchingConventions.SatisfiesTagName(tagNameWithoutPrefix, rule, StringComparison.OrdinalIgnoreCase))
            {
                // Because the code action will be fixing the casing of the tag, we don't need all the rules to be consistent.
                caseInsensitiveMatch = true;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    private static WorkspaceEdit CreateRenameTagEdit(RazorCodeActionContext context, IStartTagSyntaxNode startTag, string newTagName)
    {
        using var _ = ListPool<TextEdit>.GetPooledObject(out var textEdits);
        var codeDocumentIdentifier = new OptionalVersionedTextDocumentIdentifier() { Uri = context.Request.TextDocument.Uri };

        var startTagTextEdit = new TextEdit
        {
            Range = startTag.Name.GetRange(context.CodeDocument.Source),
            NewText = newTagName,
        };

        textEdits.Add(startTagTextEdit);

        var endTag = (startTag.Parent as MarkupElementSyntax)?.EndTag;
        if (endTag != null)
        {
            var endTagTextEdit = new TextEdit
            {
                Range = endTag.Name.GetRange(context.CodeDocument.Source),
                NewText = newTagName,
            };

            textEdits.Add(endTagTextEdit);
        }

        return new WorkspaceEdit
        {
            DocumentChanges = new TextDocumentEdit[]
            {
                    new TextDocumentEdit()
                    {
                        TextDocument = codeDocumentIdentifier,
                        Edits = textEdits.ToArray(),
                    }
            },
        };
    }

    private static bool IsTagUnknown(IStartTagSyntaxNode startTag, RazorCodeActionContext context)
    {
        foreach (var diagnostic in context.CodeDocument.GetCSharpDocument().Diagnostics)
        {
            // Check that the diagnostic is to do with our start tag
            if (!(diagnostic.Span.AbsoluteIndex > startTag.Span.End
                || startTag.Span.Start > diagnostic.Span.AbsoluteIndex + diagnostic.Span.Length))
            {
                // Component is not recognized in environment
                if (diagnostic.Id == ComponentDiagnosticFactory.UnexpectedMarkupElement.Id)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private class TagHelperPair
    {
        public readonly TagHelperDescriptor _short;
        public readonly bool _caseInsensitiveMatch;
        public TagHelperDescriptor? _fullyQualified = null;

        public TagHelperPair(TagHelperDescriptor @short, bool caseInsensitiveMatch)
        {
            _short = @short;
            _caseInsensitiveMatch = caseInsensitiveMatch;
        }
    }
}
