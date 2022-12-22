// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal class ComponentAccessibilityCodeActionProvider : RazorCodeActionProvider
{
    private static readonly Task<IReadOnlyList<RazorVSInternalCodeAction>?> s_emptyResult = Task.FromResult<IReadOnlyList<RazorVSInternalCodeAction>?>(null);

    private readonly TagHelperFactsService _tagHelperFactsService;

    public ComponentAccessibilityCodeActionProvider(TagHelperFactsService tagHelperFactsService)
    {
        _tagHelperFactsService = tagHelperFactsService ?? throw new ArgumentNullException(nameof(tagHelperFactsService));
    }

    public override Task<IReadOnlyList<RazorVSInternalCodeAction>?> ProvideAsync(RazorCodeActionContext context, CancellationToken cancellationToken)
    {
        var codeActions = new List<RazorVSInternalCodeAction>();

        // Locate cursor
        var change = new SourceChange(context.Location.AbsoluteIndex, length: 0, newText: string.Empty);
        var node = context.CodeDocument.GetSyntaxTree().Root.LocateOwner(change);
        if (node is null)
        {
            return s_emptyResult;
        }

        // Find start tag
        var startTag = node.Ancestors().FirstOrDefault(n => n is MarkupStartTagSyntax) as MarkupStartTagSyntax;
        if (startTag is null)
        {
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

        return Task.FromResult<IReadOnlyList<RazorVSInternalCodeAction>?>(codeActions);
    }

    private static bool IsApplicableTag(MarkupStartTagSyntax startTag)
    {
        if (startTag.Name.FullWidth == 0)
        {
            // Empty tag name, we shouldn't show a light bulb just to create an empty file.
            return false;
        }

        return true;
    }

    private void AddCreateComponentFromTag(RazorCodeActionContext context, MarkupStartTagSyntax startTag, List<RazorVSInternalCodeAction> container)
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

    private void AddComponentAccessFromTag(RazorCodeActionContext context, MarkupStartTagSyntax startTag, List<RazorVSInternalCodeAction> container)
    {
        var matching = FindMatchingTagHelpers(context, startTag);

        // For all the matches, add options for add @using and fully qualify
        foreach (var tagHelperPair in matching.Values)
        {
            if (tagHelperPair._fullyQualified is null)
            {
                continue;
            }

            var fullyQualifiedName = tagHelperPair._short.Name;

            // Insert @using
            if (AddUsingsCodeActionProviderHelper.TryCreateAddUsingResolutionParams(fullyQualifiedName, context.Request.TextDocument.Uri, out var @namespace, out var resolutionParams))
            {
                var addUsingCodeAction = RazorCodeActionFactory.CreateAddComponentUsing(@namespace, resolutionParams);
                container.Add(addUsingCodeAction);
            }

            // Fully qualify
            var renameTagWorkspaceEdit = CreateRenameTagEdit(context, startTag, fullyQualifiedName);
            var fullyQualifiedCodeAction = RazorCodeActionFactory.CreateFullyQualifyComponent(fullyQualifiedName, renameTagWorkspaceEdit);
            container.Add(fullyQualifiedCodeAction);
        }
    }

    private Dictionary<string, TagHelperPair> FindMatchingTagHelpers(RazorCodeActionContext context, MarkupStartTagSyntax startTag)
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

        var attributes = _tagHelperFactsService.StringifyAttributes(startTag.Attributes).ToList();

        // Find all matching tag helpers
        var matching = new Dictionary<string, TagHelperPair>();
        foreach (var tagHelper in context.DocumentSnapshot.Project.TagHelpers)
        {
            if (tagHelper.TagMatchingRules.All(rule => TagHelperMatchingConventions.SatisfiesRule(tagName, parentTagName, attributes, rule)))
            {
                matching.Add(tagHelper.Name, new TagHelperPair(@short: tagHelper));
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

        return matching;
    }

    private static WorkspaceEdit CreateRenameTagEdit(RazorCodeActionContext context, MarkupStartTagSyntax startTag, string newTagName)
    {
        var textEdits = new List<TextEdit>();
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
            DocumentChanges = new List<TextDocumentEdit>()
            {
                    new TextDocumentEdit()
                    {
                        TextDocument = codeDocumentIdentifier,
                        Edits = textEdits.ToArray(),
                    }
            }.ToArray(),
        };
    }

    private static bool IsTagUnknown(MarkupStartTagSyntax startTag, RazorCodeActionContext context)
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
        public TagHelperDescriptor _short;
        public TagHelperDescriptor? _fullyQualified = null;

        public TagHelperPair(TagHelperDescriptor @short, TagHelperDescriptor? fullyQualified = null)
        {
            _short = @short;
            _fullyQualified = fullyQualified;
        }
    }
}
