// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.AutoInsert;

internal class AutoClosingTagOnAutoInsertProvider : RazorOnAutoInsertProvider
{
    // From http://dev.w3.org/html5/spec/Overview.html#elements-0
    private static readonly IReadOnlyList<string> s_voidElements = new[]
    {
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
    };

    private readonly IOptionsMonitor<RazorLSPOptions> _optionsMonitor;

    public AutoClosingTagOnAutoInsertProvider(IOptionsMonitor<RazorLSPOptions> optionsMonitor, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        if (optionsMonitor is null)
        {
            throw new ArgumentNullException(nameof(optionsMonitor));
        }

        _optionsMonitor = optionsMonitor;
    }

    public override string TriggerCharacter => ">";

    public override bool TryResolveInsertion(Position position, FormattingContext context, [NotNullWhen(true)] out TextEdit? edit, out InsertTextFormat format)
    {
        if (position is null)
        {
            throw new ArgumentNullException(nameof(position));
        }

        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (!_optionsMonitor.CurrentValue.AutoClosingTags)
        {
            format = default;
            edit = default;
            return false;
        }

        if (!position.TryGetAbsoluteIndex(context.SourceText, Logger, out var afterCloseAngleIndex))
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
        }
        else
        {
            Debug.Assert(autoClosingBehavior == AutoClosingBehavior.SelfClosing);

            format = InsertTextFormat.Plaintext;

            // Need to replace the `>` with ' />$0' or '/>$0' depending on if there's prefixed whitespace.
            var insertionText = char.IsWhiteSpace(context.SourceText[afterCloseAngleIndex - 2]) ? "/" : " /";
            var insertionPosition = new Position(position.Line, position.Character - 1);
            var insertionRange = new Range
            {
                Start = insertionPosition,
                End = insertionPosition
            };
            edit = new TextEdit()
            {
                NewText = insertionText,
                Range = insertionRange
            };

        }

        return true;
    }

    private static bool TryResolveAutoClosingBehavior(FormattingContext context, int afterCloseAngleIndex, [NotNullWhen(true)] out string? name, out AutoClosingBehavior autoClosingBehavior)
    {
        var change = new SourceChange(afterCloseAngleIndex, 0, string.Empty);
        var syntaxTree = context.CodeDocument.GetSyntaxTree();
        var originalOwner = syntaxTree.Root.LocateOwner(change);

        if (!TryEnsureOwner_WorkaroundCompilerQuirks(afterCloseAngleIndex, syntaxTree, originalOwner, out var owner))
        {
            name = null;
            autoClosingBehavior = default;
            return false;
        }

        if (owner.Parent is MarkupStartTagSyntax startTag &&
            startTag.ForwardSlash is null &&
            startTag.Parent is MarkupElementSyntax htmlElement)
        {
            var unescapedTagName = startTag.Name.Content;
            autoClosingBehavior = InferAutoClosingBehavior(unescapedTagName);

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

        if (owner.Parent is MarkupTagHelperStartTagSyntax startTagHelper &&
            startTagHelper.ForwardSlash is null &&
            startTagHelper.Parent is MarkupTagHelperElementSyntax tagHelperElement)
        {
            name = startTagHelper.Name.Content;

            if (!TryGetTagHelperAutoClosingBehavior(tagHelperElement.TagHelperInfo.BindingResult, out autoClosingBehavior))
            {
                autoClosingBehavior = InferAutoClosingBehavior(name, tagNameComparer: StringComparer.Ordinal);
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

    private static bool TryEnsureOwner_WorkaroundCompilerQuirks(int afterCloseAngleIndex, RazorSyntaxTree syntaxTree, SyntaxNode currentOwner, [NotNullWhen(true)] out SyntaxNode? newOwner)
    {
        // All of these owner modifications are to account for https://github.com/dotnet/aspnetcore/issues/33919

        if (currentOwner?.Parent is null)
        {
            newOwner = null;
            return false;
        }

        if (currentOwner.Parent is MarkupElementSyntax parentElement &&
            parentElement.StartTag != null)
        {
            // In cases where a user types ">" in a C# code block there can be uncertainty as to "who owns" the edge of the element. Reason being that the tag
            // could be malformed and you could be in a situation like this:
            //
            // @{
            //     <div>|
            // }
            //
            // In this situation <div> is unclosed and is overriding the `}` understanding of `@{`. Because of this we're in an errored state and the syntax tree
            // doesn't indicate the appropriate language delimiter.

            if (parentElement.StartTag.EndPosition == afterCloseAngleIndex)
            {
                currentOwner = parentElement.StartTag.CloseAngle;
            }
        }
        else if (currentOwner.Parent is MarkupTagHelperElementSyntax parentTagHelperElement &&
            parentTagHelperElement.StartTag != null)
        {
            // Same reasoning as the above block here.

            if (afterCloseAngleIndex == parentTagHelperElement.StartTag.EndPosition)
            {
                currentOwner = parentTagHelperElement.StartTag.CloseAngle;
            }
        }
        else if (currentOwner is CSharpStatementLiteralSyntax)
        {
            // In cases where a user types ">" in a C# code block there can be uncertainty as to "who owns" the edge of the element in void element
            // scenarios. In a situation like this:
            //
            // @{
            //     <input>|
            // }
            //
            // In this situation <input> is a 100% valid HTML element but after it the C# context begins. When querying owner for the location after
            // the ">" we get the C# statement block instead of the end close-angle (Razor compiler quirk).

            var closeAngleIndex = afterCloseAngleIndex - 1;
            var closeAngleSourceChange = new SourceChange(closeAngleIndex, length: 0, newText: string.Empty);
            currentOwner = syntaxTree.Root.LocateOwner(closeAngleSourceChange);
        }
        else if (currentOwner.Parent is MarkupEndTagSyntax ||
                 currentOwner.Parent is MarkupTagHelperEndTagSyntax)
        {
            // Quirk: https://github.com/dotnet/aspnetcore/issues/33919#issuecomment-870233627
            // When tags are nested within each other within a C# block like:
            //
            // @if (true)
            // {
            //     <div><em>|</div>
            // }
            //
            // The owner will be the </div>. Note this does not happen outside of C# blocks.

            var closeAngleSourceChange = new SourceChange(afterCloseAngleIndex - 1, length: 0, newText: string.Empty);
            currentOwner = syntaxTree.Root.LocateOwner(closeAngleSourceChange);

            // Get the real closing angle if we get the quote from an attribute syntax. See https://github.com/dotnet/razor-tooling/issues/5694
            switch (currentOwner)
            {
                case MarkupTextLiteralSyntax { Parent.Parent: MarkupStartTagSyntax startTag }:
                    currentOwner = startTag.CloseAngle;
                    break;
                case MarkupTextLiteralSyntax { Parent.Parent: MarkupTagHelperStartTagSyntax startTagHelper }:
                    currentOwner = startTagHelper.CloseAngle;
                    break;
            }
        }
        else if (currentOwner.Parent is MarkupStartTagSyntax startTag &&
            startTag.OpenAngle.Position == afterCloseAngleIndex)
        {
            // We found the wrong owner. We really need a SyntaxTree API which is "get me the token at position x" :(
            // This can happen when you are at the following:
            //
            // @if (true)
            // {
            //     <em>|<div></div>
            // }
            //
            // It's picking up the trailing <div> as the owner

            var startElement = startTag.Parent;
            if (startElement.TryGetPreviousSibling(out var previousSibling))
            {
                var potentialCloseAngle = previousSibling.GetLastToken();
                if (potentialCloseAngle.Kind == SyntaxKind.CloseAngle &&
                    potentialCloseAngle.Position == afterCloseAngleIndex - 1)
                {
                    currentOwner = potentialCloseAngle;
                }
            }
        }
        else if (currentOwner.Parent is MarkupTagHelperStartTagSyntax startTagHelperSyntax &&
            startTagHelperSyntax.OpenAngle.Position == afterCloseAngleIndex)
        {
            // We found the wrong owner. We really need a SyntaxTree API which is "get me the token at position x" :(
            // This can happen when you are at the following:
            //
            // @if (true)
            // {
            //     <em>|<th2></th2>
            // }
            //
            // It's picking up the trailing <th2> as the owner

            var startTagHelperElement = startTagHelperSyntax.Parent;
            if (startTagHelperElement.TryGetPreviousSibling(out var previousSibling))
            {
                var potentialCloseAngle = previousSibling.GetLastToken();
                if (potentialCloseAngle.Kind == SyntaxKind.CloseAngle &&
                    potentialCloseAngle.Position == afterCloseAngleIndex - 1)
                {
                    currentOwner = potentialCloseAngle;
                }
            }
        }

        if (currentOwner?.Parent is null)
        {
            newOwner = null;
            return false;
        }

        newOwner = currentOwner;
        return true;
    }

    private static AutoClosingBehavior InferAutoClosingBehavior(string name, StringComparer? tagNameComparer = null)
    {
        tagNameComparer ??= StringComparer.OrdinalIgnoreCase;

        if (s_voidElements.Contains(name, tagNameComparer))
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
        } while (node != null);

        return false;
    }

    private enum AutoClosingBehavior
    {
        EndTag,
        SelfClosing,
    }
}
