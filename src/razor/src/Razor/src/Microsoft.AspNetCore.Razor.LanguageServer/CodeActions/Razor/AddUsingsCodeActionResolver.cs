// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal sealed class AddUsingsCodeActionResolver : IRazorCodeActionResolver
{
    private readonly IDocumentContextFactory _documentContextFactory;

    public AddUsingsCodeActionResolver(IDocumentContextFactory documentContextFactory)
    {
        _documentContextFactory = documentContextFactory ?? throw new ArgumentNullException(nameof(documentContextFactory));
    }

    public string Action => LanguageServerConstants.CodeActions.AddUsing;

    public async Task<WorkspaceEdit?> ResolveAsync(JObject data, CancellationToken cancellationToken)
    {
        if (data is null)
        {
            return null;
        }

        var actionParams = data.ToObject<AddUsingsCodeActionParams>();
        if (actionParams is null)
        {
            return null;
        }

        var documentContext = _documentContextFactory.TryCreate(actionParams.Uri);
        if (documentContext is null)
        {
            return null;
        }

        var documentSnapshot = documentContext.Snapshot;

        var text = await documentSnapshot.GetTextAsync().ConfigureAwait(false);
        if (text is null)
        {
            return null;
        }

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
        using var _ = ListPool<TextDocumentEdit>.GetPooledObject(out var documentChanges);

        // Need to add the additional edit first, as the actual usings go at the top of the file, and would
        // change the ranges needed in the additional edit if they went in first
        if (additionalEdit is not null)
        {
            documentChanges.Add(additionalEdit);
        }

        var usingDirectives = FindUsingDirectives(codeDocument);
        if (usingDirectives.Count > 0)
        {
            // Interpolate based on existing @using statements
            var edits = GenerateSingleUsingEditsInterpolated(codeDocument, codeDocumentIdentifier, @namespace, usingDirectives);
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
        List<RazorUsingDirective> existingUsingDirectives)
    {
        var edits = new List<TextEdit>();
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
                var usingDirectiveLineIndex = codeDocument.Source.Text.Lines.GetLinePosition(usingDirective.Node.Span.Start).Line;
                var head = new Position(usingDirectiveLineIndex, 0);
                var edit = new TextEdit() { Range = new Range { Start = head, End = head }, NewText = newText };
                edits.Add(edit);
                break;
            }
        }

        // If we haven't actually found a place to insert the using directive, do so at the end
        if (edits.Count == 0)
        {
            var endIndex = existingUsingDirectives.Last().Node.Span.End;
            var lineIndex = GetLineIndexOrEnd(codeDocument, endIndex - 1) + 1;
            var head = new Position(lineIndex, 0);
            var edit = new TextEdit() { Range = new Range { Start = head, End = head }, NewText = newText };
            edits.Add(edit);
        }

        return new TextDocumentEdit()
        {
            TextDocument = codeDocumentIdentifier,
            Edits = edits.ToArray(),
        };
    }

    private static TextDocumentEdit GenerateSingleUsingEditsAtTop(
        RazorCodeDocument codeDocument,
        OptionalVersionedTextDocumentIdentifier codeDocumentIdentifier,
        string newUsingNamespace)
    {
        var head = new Position(0, 0);

        // If we don't have usings, insert after the last namespace or page directive, which ever comes later
        var syntaxTreeRoot = codeDocument.GetSyntaxTree().Root;
        var lastNamespaceOrPageDirective = syntaxTreeRoot
            .DescendantNodes()
            .Where(n => IsNamespaceOrPageDirective(n))
            .LastOrDefault();
        if (lastNamespaceOrPageDirective != null)
        {
            var lineIndex = GetLineIndexOrEnd(codeDocument, lastNamespaceOrPageDirective.Span.End - 1) + 1;
            head = new Position(lineIndex, 0);
        }

        // Insert all usings at the given point
        var range = new Range { Start = head, End = head };
        return new TextDocumentEdit
        {
            TextDocument = codeDocumentIdentifier,
            Edits = new[]
            {
                new TextEdit()
                {
                    NewText = string.Concat($"@using {newUsingNamespace}{Environment.NewLine}"),
                    Range = range,
                }
            }
        };
    }

    private static int GetLineIndexOrEnd(RazorCodeDocument codeDocument, int endIndex)
    {
        if (endIndex < codeDocument.Source.Text.Length)
        {
            return codeDocument.Source.Text.Lines.GetLinePosition(endIndex).Line;
        }
        else
        {
            return codeDocument.Source.Text.Lines.Count;
        }
    }

    private static List<RazorUsingDirective> FindUsingDirectives(RazorCodeDocument codeDocument)
    {
        var directives = new List<RazorUsingDirective>();
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

        return directives;
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

    private readonly struct RazorUsingDirective
    {
        readonly public RazorDirectiveSyntax Node { get; }
        readonly public AddImportChunkGenerator Statement { get; }

        public RazorUsingDirective(RazorDirectiveSyntax node, AddImportChunkGenerator statement)
        {
            Node = node;
            Statement = statement;
        }
    }
}
