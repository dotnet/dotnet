// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.AutoInsert;

internal sealed class AutoClosingTagOnAutoInsertProvider : IOnAutoInsertProvider
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

    private readonly IOptionsMonitor<RazorLSPOptions> _optionsMonitor;
    private readonly ILogger<IOnAutoInsertProvider> _logger;

    public AutoClosingTagOnAutoInsertProvider(IOptionsMonitor<RazorLSPOptions> optionsMonitor, ILoggerFactory loggerFactory)
    {
        if (optionsMonitor is null)
        {
            throw new ArgumentNullException(nameof(optionsMonitor));
        }

        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _optionsMonitor = optionsMonitor;
        _logger = loggerFactory.CreateLogger<IOnAutoInsertProvider>();
    }

    public string TriggerCharacter => ">";

    public bool TryResolveInsertion(Position position, FormattingContext context, [NotNullWhen(true)] out TextEdit? edit, out InsertTextFormat format)
    {
        if (!_optionsMonitor.CurrentValue.AutoClosingTags)
        {
            format = default;
            edit = default;
            return false;
        }

        if (!position.TryGetAbsoluteIndex(context.SourceText, _logger, out var afterCloseAngleIndex))
        {
            format = default;
            edit = default;
            return false;
        }

        if (!TryResolveAutoClosingBehavior(context, afterCloseAngleIndex, out var tagName, out var autoClosingBehavior))
        {
            format = default;
            edit = default;
            return false;
        }

        if (autoClosingBehavior == AutoClosingBehavior.EndTag)
        {
            format = InsertTextFormat.Snippet;
            edit = new TextEdit()
            {
                NewText = $"$0</{tagName}>",
                Range = new Range { Start = position, End = position },
            };

            return true;
        }

        Debug.Assert(autoClosingBehavior == AutoClosingBehavior.SelfClosing);

        format = InsertTextFormat.Plaintext;

        // Need to replace the `>` with ' />$0' or '/>$0' depending on if there's prefixed whitespace.
        var insertionText = char.IsWhiteSpace(context.SourceText[afterCloseAngleIndex - 2]) ? "/" : " /";
        var insertionPosition = new Position(position.Line, position.Character - 1);
        edit = new TextEdit()
        {
            NewText = insertionText,
            Range = new Range
            {
                Start = insertionPosition,
                End = insertionPosition
            }
        };

        return true;
    }

    private static bool TryResolveAutoClosingBehavior(FormattingContext context, int afterCloseAngleIndex, [NotNullWhen(true)] out string? name, out AutoClosingBehavior autoClosingBehavior)
    {
        var syntaxTree = context.CodeDocument.GetSyntaxTree();
        var closeAngle = syntaxTree.Root.FindToken(afterCloseAngleIndex - 1);

        if (closeAngle.Parent is MarkupStartTagSyntax
            {
                ForwardSlash: null,
                Parent: MarkupElementSyntax htmlElement
            } startTag)
        {
            var unescapedTagName = startTag.Name.Content;
            autoClosingBehavior = InferAutoClosingBehavior(unescapedTagName, caseSensitive: false);

            if (autoClosingBehavior == AutoClosingBehavior.EndTag && !CouldAutoCloseParentOrSelf(unescapedTagName, htmlElement))
            {
                // Auto-closing behavior is end-tag; however, we already have and end-tag therefore we don't need to do anything!
                autoClosingBehavior = default;
                name = null;
                return false;
            }

            // Finally capture the entire tag name with the potential escape operator.
            name = startTag.GetTagNameWithOptionalBang();
            return true;
        }

        if (closeAngle.Parent is MarkupTagHelperStartTagSyntax
            {
                ForwardSlash: null,
                Parent: MarkupTagHelperElementSyntax tagHelperElement
            } startTagHelper)
        {
            name = startTagHelper.Name.Content;

            if (!TryGetTagHelperAutoClosingBehavior(tagHelperElement.TagHelperInfo.BindingResult, out autoClosingBehavior))
            {
                autoClosingBehavior = InferAutoClosingBehavior(name, caseSensitive: true);
            }

            if (autoClosingBehavior == AutoClosingBehavior.EndTag && !CouldAutoCloseParentOrSelf(name, tagHelperElement))
            {
                // Auto-closing behavior is end-tag; however, we already have and end-tag therefore we don't need to do anything!
                autoClosingBehavior = default;
                name = null;
                return false;
            }

            return true;
        }

        autoClosingBehavior = default;
        name = null;
        return false;
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
            var tagMatchingRules = bindingResult.GetBoundRules(descriptor);
            for (var i = 0; i < tagMatchingRules.Count; i++)
            {
                var tagMatchingRule = tagMatchingRules[i];

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

    private static bool CouldAutoCloseParentOrSelf(string currentTagName, SyntaxNode node)
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
}
