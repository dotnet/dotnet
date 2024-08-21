// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.AutoInsert;

internal sealed class CloseTextTagOnAutoInsertProvider(RazorLSPOptionsMonitor optionsMonitor) : IOnAutoInsertProvider
{
    private readonly RazorLSPOptionsMonitor _optionsMonitor = optionsMonitor;

    public string TriggerCharacter => ">";

    public bool TryResolveInsertion(Position position, FormattingContext context, [NotNullWhen(true)] out TextEdit? edit, out InsertTextFormat format)
    {
        if (!_optionsMonitor.CurrentValue.AutoClosingTags)
        {
            // We currently only support auto-closing tags our onType formatter.
            format = default;
            edit = default;
            return false;
        }

        if (!IsAtTextTag(context, position))
        {
            format = default;
            edit = default;
            return false;
        }

        // This is a text tag.
        format = InsertTextFormat.Snippet;
        edit = VsLspFactory.CreateTextEdit(position, $"$0</{SyntaxConstants.TextTagName}>");

        return true;
    }

    private static bool IsAtTextTag(FormattingContext context, Position position)
    {
        var syntaxTree = context.CodeDocument.GetSyntaxTree();

        if (!context.SourceText.TryGetAbsoluteIndex(position, out var absoluteIndex))
        {
            return false;
        }

        var owner = syntaxTree.Root.FindToken(absoluteIndex - 1);
        // Make sure the end </text> tag doesn't already exist
        if (owner?.Parent is MarkupStartTagSyntax
            {
                IsMarkupTransition: true,
                Parent: MarkupElementSyntax { EndTag: null }
            } startTag)
        {
            Debug.Assert(string.Equals(startTag.Name.Content, SyntaxConstants.TextTagName, StringComparison.Ordinal), "MarkupTransition that is not a <text> tag.");

            return true;
        }

        return false;
    }
}
