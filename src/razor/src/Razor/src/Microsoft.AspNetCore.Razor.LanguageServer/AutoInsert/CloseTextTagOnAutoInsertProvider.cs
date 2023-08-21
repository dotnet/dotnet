// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.AutoInsert;

internal sealed class CloseTextTagOnAutoInsertProvider : IOnAutoInsertProvider
{
    private readonly IOptionsMonitor<RazorLSPOptions> _optionsMonitor;
    private readonly ILogger<IOnAutoInsertProvider> _logger;

    public CloseTextTagOnAutoInsertProvider(IOptionsMonitor<RazorLSPOptions> optionsMonitor, ILoggerFactory loggerFactory)
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
            // We currently only support auto-closing tags our onType formatter.
            format = default;
            edit = default;
            return false;
        }

        if (!IsAtTextTag(context, position, _logger))
        {
            format = default;
            edit = default;
            return false;
        }

        // This is a text tag.
        format = InsertTextFormat.Snippet;
        edit = new TextEdit()
        {
            NewText = $"$0</{SyntaxConstants.TextTagName}>",
            Range = new Range { Start = position, End = position },
        };

        return true;
    }

    private static bool IsAtTextTag(FormattingContext context, Position position, ILogger logger)
    {
        var syntaxTree = context.CodeDocument.GetSyntaxTree();

        if (!position.TryGetAbsoluteIndex(context.SourceText, logger, out var absoluteIndex))
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
