// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal sealed class AddUsingsCodeActionResolver(IDocumentContextFactory documentContextFactory) : IRazorCodeActionResolver
{
    private readonly IDocumentContextFactory _documentContextFactory = documentContextFactory;

    public string Action => LanguageServerConstants.CodeActions.AddUsing;

    public async Task<WorkspaceEdit?> ResolveAsync(JsonElement data, CancellationToken cancellationToken)
    {
        var actionParams = data.Deserialize<AddUsingsCodeActionParams>();
        if (actionParams is null)
        {
            return null;
        }

        if (!_documentContextFactory.TryCreate(actionParams.Uri, out var documentContext))
        {
            return null;
        }

        var documentSnapshot = documentContext.Snapshot;

        var codeDocument = await documentSnapshot.GetGeneratedOutputAsync().ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            return null;
        }

        var codeDocumentIdentifier = new OptionalVersionedTextDocumentIdentifier() { Uri = actionParams.Uri };
        return CreateAddUsingWorkspaceEdit(actionParams.Namespace, actionParams.AdditionalEdit, codeDocument, codeDocumentIdentifier);
    }

    internal static WorkspaceEdit CreateAddUsingWorkspaceEdit(string @namespace, TextDocumentEdit? additionalEdit, RazorCodeDocument codeDocument, OptionalVersionedTextDocumentIdentifier codeDocumentIdentifier)
    {
        /* The heuristic is as follows:
         *
         * - If no @using, @namespace, or @page directives are present, insert the statements at the top of the
         *   file in alphabetical order.
         * - If a @namespace or @page are present, the statements are inserted after the last line-wise in
         *   alphabetical order.
         * - If @using directives are present and alphabetized with System directives at the top, the statements
         *   will be placed in the correct locations according to that ordering.
         * - Otherwise it's kind of undefined; it's only geared to insert based on alphabetization.
         *
         * This is generally sufficient for our current situation (inserting a single @using statement to include a
         * component), however it has holes if we eventually use it for other purposes. If we want to deal with
         * that now I can come up with a more sophisticated heuristic (something along the lines of checking if
         * there's already an ordering, etc.).
         */
        using var documentChanges = new PooledArrayBuilder<TextDocumentEdit>();

        // Need to add the additional edit first, as the actual usings go at the top of the file, and would
        // change the ranges needed in the additional edit if they went in first
        if (additionalEdit is not null)
        {
            documentChanges.Add(additionalEdit);
        }

        using var usingDirectives = new PooledArrayBuilder<RazorUsingDirective>();
        CollectUsingDirectives(codeDocument, ref usingDirectives.AsRef());
        if (usingDirectives.Count > 0)
        {
            // Interpolate based on existing @using statements
            var edits = GenerateSingleUsingEditsInterpolated(codeDocument, codeDocumentIdentifier, @namespace, in usingDirectives);
            documentChanges.Add(edits);
        }
        else
        {
            // Just throw them at the top
            var edits = GenerateSingleUsingEditsAtTop(codeDocument, codeDocumentIdentifier, @namespace);
            documentChanges.Add(edits);
        }

        return new WorkspaceEdit()
        {
            DocumentChanges = documentChanges.ToArray(),
        };
    }

    private static TextDocumentEdit GenerateSingleUsingEditsInterpolated(
        RazorCodeDocument codeDocument,
        OptionalVersionedTextDocumentIdentifier codeDocumentIdentifier,
        string newUsingNamespace,
        ref readonly PooledArrayBuilder<RazorUsingDirective> existingUsingDirectives)
    {
        Debug.Assert(existingUsingDirectives.Count > 0);

        using var edits = new PooledArrayBuilder<TextEdit>();
        var newText = $"@using {newUsingNamespace}{Environment.NewLine}";

        foreach (var usingDirective in existingUsingDirectives)
        {
            // Skip System directives; if they're at the top we don't want to insert before them
            var usingDirectiveNamespace = usingDirective.Statement.ParsedNamespace;
            if (usingDirectiveNamespace.StartsWith("System", StringComparison.Ordinal))
            {
                continue;
            }

            if (string.CompareOrdinal(newUsingNamespace, usingDirectiveNamespace) < 0)
            {
                var usingDirectiveLineIndex = codeDocument.Source.Text.GetLinePosition(usingDirective.Node.Span.Start).Line;
                var edit = VsLspFactory.CreateTextEdit(line: usingDirectiveLineIndex, character: 0, newText);
                edits.Add(edit);
                break;
            }
        }

        // If we haven't actually found a place to insert the using directive, do so at the end
        if (edits.Count == 0)
        {
            var endIndex = existingUsingDirectives[^1].Node.Span.End;
            var lineIndex = GetLineIndexOrEnd(codeDocument, endIndex - 1) + 1;
            var edit = VsLspFactory.CreateTextEdit(line: lineIndex, character: 0, newText);
            edits.Add(edit);
        }

        return new TextDocumentEdit()
        {
            TextDocument = codeDocumentIdentifier,
            Edits = edits.ToArray()
        };
    }

    private static TextDocumentEdit GenerateSingleUsingEditsAtTop(
        RazorCodeDocument codeDocument,
        OptionalVersionedTextDocumentIdentifier codeDocumentIdentifier,
        string newUsingNamespace)
    {
        var insertPosition = (0, 0);

        // If we don't have usings, insert after the last namespace or page directive, which ever comes later
        var syntaxTreeRoot = codeDocument.GetSyntaxTree().Root;
        var lastNamespaceOrPageDirective = syntaxTreeRoot
            .DescendantNodes()
            .LastOrDefault(IsNamespaceOrPageDirective);

        if (lastNamespaceOrPageDirective != null)
        {
            var lineIndex = GetLineIndexOrEnd(codeDocument, lastNamespaceOrPageDirective.Span.End - 1) + 1;
            insertPosition = (lineIndex, 0);
        }

        // Insert all usings at the given point
        return new TextDocumentEdit
        {
            TextDocument = codeDocumentIdentifier,
            Edits = [VsLspFactory.CreateTextEdit(insertPosition, newText: $"@using {newUsingNamespace}{Environment.NewLine}")]
        };
    }

    private static int GetLineIndexOrEnd(RazorCodeDocument codeDocument, int endIndex)
    {
        if (endIndex < codeDocument.Source.Text.Length)
        {
            return codeDocument.Source.Text.GetLinePosition(endIndex).Line;
        }
        else
        {
            return codeDocument.Source.Text.Lines.Count;
        }
    }

    private static void CollectUsingDirectives(RazorCodeDocument codeDocument, ref PooledArrayBuilder<RazorUsingDirective> directives)
    {
        var syntaxTreeRoot = codeDocument.GetSyntaxTree().Root;
        foreach (var node in syntaxTreeRoot.DescendantNodes())
        {
            if (node is RazorDirectiveSyntax directiveNode)
            {
                foreach (var child in directiveNode.DescendantNodes())
                {
                    if (child.GetChunkGenerator() is AddImportChunkGenerator { IsStatic: false } usingStatement)
                    {
                        directives.Add(new RazorUsingDirective(directiveNode, usingStatement));
                    }
                }
            }
        }
    }

    private static bool IsNamespaceOrPageDirective(SyntaxNode node)
    {
        if (node is RazorDirectiveSyntax directiveNode)
        {
            return directiveNode.DirectiveDescriptor == ComponentPageDirective.Directive ||
                directiveNode.DirectiveDescriptor == NamespaceDirective.Directive ||
                directiveNode.DirectiveDescriptor == PageDirective.Directive;
        }

        return false;
    }

    private readonly record struct RazorUsingDirective(RazorDirectiveSyntax Node, AddImportChunkGenerator Statement);
}
