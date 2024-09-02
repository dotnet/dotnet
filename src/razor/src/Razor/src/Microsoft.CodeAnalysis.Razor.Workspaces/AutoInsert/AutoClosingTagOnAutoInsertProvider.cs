// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.VisualStudio.LanguageServer.Protocol;

using RazorSyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;

namespace Microsoft.CodeAnalysis.Razor.AutoInsert;

internal class AutoClosingTagOnAutoInsertProvider : IOnAutoInsertProvider
{
    // From http://dev.w3.org/html5/spec/Overview.html#elements-0
    private static readonly ImmutableHashSet<string> s_voidElements = ImmutableHashSet.Create(StringComparer.OrdinalIgnoreCase,
        "area",
        "base",
        "br",
        "col",
        "command",
        "embed",
        "hr",
        "img",
        "input",
        "keygen",
        "link",
        "meta",
        "menuitem",
        "param",
        "source",
        "track",
        "wbr"
    );

    private static readonly ImmutableHashSet<string> s_voidElementsCaseSensitive = s_voidElements.WithComparer(StringComparer.Ordinal);

    public string TriggerCharacter => ">";

    public bool TryResolveInsertion(
        Position position,
        RazorCodeDocument codeDocument,
        bool enableAutoClosingTags,
        [NotNullWhen(true)] out VSInternalDocumentOnAutoInsertResponseItem? autoInsertEdit)
    {
        autoInsertEdit = null;

        if (!(enableAutoClosingTags
            && codeDocument.Source.Text is { } sourceText
            && sourceText.TryGetAbsoluteIndex(position, out var afterCloseAngleIndex)
            && TryResolveAutoClosingBehavior(codeDocument, afterCloseAngleIndex) is { } tagNameWithClosingBehavior))
        {
            return false;
        }

        if (tagNameWithClosingBehavior.AutoClosingBehavior == AutoClosingBehavior.EndTag)
        {
            var formatForEndTag = InsertTextFormat.Snippet;
            var editForEndTag = VsLspFactory.CreateTextEdit(position, $"$0</{tagNameWithClosingBehavior.TagName}>");

            autoInsertEdit = new()
            {
                TextEdit = editForEndTag,
                TextEditFormat = formatForEndTag
            };

            return true;
        }

        Debug.Assert(tagNameWithClosingBehavior.AutoClosingBehavior == AutoClosingBehavior.SelfClosing);

        var format = InsertTextFormat.Plaintext;

        // Need to replace the `>` with ' />$0' or '/>$0' depending on if there's prefixed whitespace.
        var insertionText = char.IsWhiteSpace(sourceText[afterCloseAngleIndex - 2]) ? "/" : " /";
        var edit = VsLspFactory.CreateTextEdit(position.Line, position.Character - 1, insertionText);

        autoInsertEdit = new()
        {
            TextEdit = edit,
            TextEditFormat = format
        };

        return true;
    }

    private static TagNameWithClosingBehavior? TryResolveAutoClosingBehavior(RazorCodeDocument codeDocument, int afterCloseAngleIndex)
    {
        var syntaxTree = codeDocument.GetSyntaxTree();
        var closeAngle = syntaxTree.Root.FindToken(afterCloseAngleIndex - 1);

        if (closeAngle.Parent is MarkupStartTagSyntax
            {
                ForwardSlash: null,
                Parent: MarkupElementSyntax htmlElement
            } startTag)
        {
            var unescapedTagName = startTag.Name.Content;
            var autoClosingBehavior = InferAutoClosingBehavior(unescapedTagName, caseSensitive: false);

            if (autoClosingBehavior == AutoClosingBehavior.EndTag && !CouldAutoCloseParentOrSelf(unescapedTagName, htmlElement))
            {
                // Auto-closing behavior is end-tag; however, we already have and end-tag therefore we don't need to do anything!
                return default;
            }

            // Finally capture the entire tag name with the potential escape operator.
            var name = startTag.GetTagNameWithOptionalBang();
            return new TagNameWithClosingBehavior(name, autoClosingBehavior);
        }

        if (closeAngle.Parent is MarkupTagHelperStartTagSyntax
            {
                ForwardSlash: null,
                Parent: MarkupTagHelperElementSyntax { TagHelperInfo.BindingResult: var binding } tagHelperElement
            } startTagHelper)
        {
            var name = startTagHelper.Name.Content;

            if (!TryGetTagHelperAutoClosingBehavior(binding, out var autoClosingBehavior))
            {
                autoClosingBehavior = InferAutoClosingBehavior(name, caseSensitive: true);
            }

            if (autoClosingBehavior == AutoClosingBehavior.EndTag && !CouldAutoCloseParentOrSelf(name, tagHelperElement))
            {
                // Auto-closing behavior is end-tag; however, we already have and end-tag therefore we don't need to do anything!
                return default;
            }

            return new TagNameWithClosingBehavior(name, autoClosingBehavior);
        }

        return default;
    }

    private static AutoClosingBehavior InferAutoClosingBehavior(string name, bool caseSensitive)
    {
        var voidElements = caseSensitive ? s_voidElementsCaseSensitive : s_voidElements;

        if (voidElements.Contains(name))
        {
            return AutoClosingBehavior.SelfClosing;
        }

        return AutoClosingBehavior.EndTag;
    }

    private static bool TryGetTagHelperAutoClosingBehavior(TagHelperBinding bindingResult, out AutoClosingBehavior autoClosingBehavior)
    {
        var resolvedTagStructure = TagStructure.Unspecified;

        foreach (var descriptor in bindingResult.Descriptors)
        {
            var tagMatchingRules = bindingResult.Mappings[descriptor];
            foreach (var tagMatchingRule in tagMatchingRules)
            {
                if (tagMatchingRule.TagStructure == TagStructure.Unspecified)
                {
                    // The current tag matching rule isn't specified so it should never be used as the resolved tag structure since it
                    // says it doesn't have an opinion.
                }
                else if (tagMatchingRule.TagStructure == TagStructure.NormalOrSelfClosing)
                {
                    // We have a rule that indicates it can be normal or self-closing, that always wins because
                    // it's all encompassing. Meaning, even if all previous rules indicate "no children" and at least
                    // one says it supports children we render the tag as having the potential to have children.
                    autoClosingBehavior = AutoClosingBehavior.EndTag;
                    return true;
                }
                else
                {
                    resolvedTagStructure = tagMatchingRule.TagStructure;
                }
            }
        }

        Debug.Assert(resolvedTagStructure != TagStructure.NormalOrSelfClosing, "Normal tag structure should already have been preferred");

        if (resolvedTagStructure == TagStructure.WithoutEndTag)
        {
            autoClosingBehavior = AutoClosingBehavior.SelfClosing;
            return true;
        }

        autoClosingBehavior = default;
        return false;
    }

    private static bool CouldAutoCloseParentOrSelf(string currentTagName, RazorSyntaxNode node)
    {
        do
        {
            string? potentialStartTagName = null;
            RazorSyntaxNode? endTag = null;
            if (node is MarkupTagHelperElementSyntax parentTagHelper)
            {
                potentialStartTagName = parentTagHelper.StartTag?.Name.Content ?? parentTagHelper.EndTag?.Name.Content;
                endTag = parentTagHelper.EndTag;
            }
            else if (node is MarkupElementSyntax parentElement)
            {
                potentialStartTagName = parentElement.StartTag?.Name.Content ?? parentElement.EndTag?.Name.Content;
                endTag = parentElement.EndTag;
            }

            var isNonTagStructure = potentialStartTagName is null;
            if (isNonTagStructure)
            {
                // We don't want to look outside of our immediate parent for potential parents that we could auto-close because
                // auto-closing one of those parents wouldn't actually auto-close them. For instance:
                //
                // <div>
                //     @if (true)
                //     {
                //         <div>|</div>
                //     }
                //
                // If we re-type the `>` in the inner-div we don't want to add another </div> because it would be out of scope
                // for the parent <div>
                return false;
            }

            if (string.Equals(potentialStartTagName, currentTagName, StringComparison.Ordinal))
            {
                // Tag names equal, if the parent is missing an end-tag it could apply to that
                // i.e. <div><div>|</div>
                if (endTag is null)
                {
                    return true;
                }

                // Has an end-tag; however, it could be another level of parent which is OK lets keep going up
            }
            else
            {
                // Different tag name, can't apply
                return false;
            }

            node = node.Parent;
        } while (node is not null);

        return false;
    }

    private enum AutoClosingBehavior
    {
        EndTag,
        SelfClosing,
    }

    private readonly record struct TagNameWithClosingBehavior(string TagName, AutoClosingBehavior AutoClosingBehavior);
}
