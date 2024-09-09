// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.Syntax;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;

internal class RazorSyntaxTreePartialParser
{
    private SyntaxNode? _lastChangeOwner;
    private bool _lastResultProvisional;

    public RazorSyntaxTreePartialParser(RazorSyntaxTree syntaxTree)
    {
        if (syntaxTree is null)
        {
            throw new ArgumentNullException(nameof(syntaxTree));
        }

        OriginalSyntaxTree = syntaxTree;
        ModifiedSyntaxTreeRoot = syntaxTree.Root;
    }

    // Internal for testing
    internal RazorSyntaxTree OriginalSyntaxTree { get; }

    // Internal for testing
    internal SyntaxNode ModifiedSyntaxTreeRoot { get; private set; }

    /// <summary>
    /// Partially parses the provided <paramref name="change"/>.
    /// </summary>
    /// <param name="change">The <see cref="SourceChange"/> that should be partially parsed.</param>
    /// <returns>The <see cref="PartialParseResultInternal"/> and <see cref="RazorSyntaxTree"/> of the partial parse.</returns>
    /// <remarks>
    /// The returned <see cref="RazorSyntaxTree"/> has the <see cref="OriginalSyntaxTree"/>'s <see cref="RazorSyntaxTree.Source"/> and <see cref="RazorSyntaxTree.Diagnostics"/>.
    /// </remarks>
    public (PartialParseResultInternal, RazorSyntaxTree) Parse(SourceChange change)
    {
        var result = GetPartialParseResult(change);

        // Remember if this was provisionally accepted for next partial parse.
        _lastResultProvisional = (result & PartialParseResultInternal.Provisional) == PartialParseResultInternal.Provisional;
        var newSyntaxTree = new RazorSyntaxTree(ModifiedSyntaxTreeRoot, OriginalSyntaxTree.Source, OriginalSyntaxTree.Diagnostics, OriginalSyntaxTree.Options);

        return (result, newSyntaxTree);
    }

    private PartialParseResultInternal GetPartialParseResult(SourceChange change)
    {
        var result = PartialParseResultInternal.Rejected;

        // Try the last change owner
        if (_lastChangeOwner is not null)
        {
            var editHandler = _lastChangeOwner.GetEditHandler() ?? SpanEditHandler.CreateDefault(AcceptedCharactersInternal.Any);
            if (editHandler.OwnsChange(_lastChangeOwner, change))
            {
                var editResult = editHandler.ApplyChange(_lastChangeOwner, change);
                result = editResult.Result;
                if ((editResult.Result & PartialParseResultInternal.Rejected) != PartialParseResultInternal.Rejected)
                {
                    ReplaceLastChangeOwner(editResult.EditedNode);
                }
            }

            return result;
        }

        // Locate the span responsible for this change
#pragma warning disable CS0618 // Type or member is obsolete, RazorSyntaxTreePartialParser is only used in legacy scenarios
        _lastChangeOwner = ModifiedSyntaxTreeRoot.LocateOwner(change);
#pragma warning restore CS0618 // Type or member is obsolete

        if (_lastResultProvisional)
        {
            // Last change owner couldn't accept this, so we must do a full reparse
            result = PartialParseResultInternal.Rejected;
        }
        else if (_lastChangeOwner is not null)
        {
            var editHandler = _lastChangeOwner.GetEditHandler() ?? SpanEditHandler.CreateDefault(AcceptedCharactersInternal.Any);
            var editResult = editHandler.ApplyChange(_lastChangeOwner, change);
            result = editResult.Result;
            if ((editResult.Result & PartialParseResultInternal.Rejected) != PartialParseResultInternal.Rejected)
            {
                ReplaceLastChangeOwner(editResult.EditedNode);
            }
        }

        return result;
    }

    private void ReplaceLastChangeOwner(SyntaxNode editedNode)
    {
        ModifiedSyntaxTreeRoot = ModifiedSyntaxTreeRoot.ReplaceNode(_lastChangeOwner, editedNode);
        foreach (var node in ModifiedSyntaxTreeRoot.DescendantNodes())
        {
            if (node.Green == editedNode.Green)
            {
                _lastChangeOwner = node;
                break;
            }
        }
    }
}
