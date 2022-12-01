// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Razor;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal class TypeAccessibilityCodeActionProvider : CSharpCodeActionProvider
{
    private static readonly IEnumerable<string> s_supportedDiagnostics = new[]
    {
        // `The type or namespace name 'type/namespace' could not be found
        //  (are you missing a using directive or an assembly reference?)`
        // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs0246
        "CS0246",

        // `The name 'identifier' does not exist in the current context`
        // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs0103
        "CS0103",

        // `The name 'identifier' does not exist in the current context`
        "IDE1007"
    };

    public override Task<IReadOnlyList<RazorVSInternalCodeAction>?> ProvideAsync(
        RazorCodeActionContext context,
        IEnumerable<RazorVSInternalCodeAction> codeActions,
        CancellationToken cancellationToken)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (codeActions is null)
        {
            throw new ArgumentNullException(nameof(codeActions));
        }

        if (context.Request?.Context?.Diagnostics is null)
        {
            return EmptyResult;
        }

        if (codeActions is null || !codeActions.Any())
        {
            return EmptyResult;
        }

        var results = context.SupportsCodeActionResolve
            ? ProcessCodeActionsVS(context, codeActions)
            : ProcessCodeActionsVSCode(context, codeActions);

        var orderedResults = results.OrderBy(codeAction => codeAction.Title).ToArray();
        return Task.FromResult<IReadOnlyList<RazorVSInternalCodeAction>?>(orderedResults);
    }

    private static IEnumerable<RazorVSInternalCodeAction> ProcessCodeActionsVSCode(
        RazorCodeActionContext context,
        IEnumerable<RazorVSInternalCodeAction> codeActions)
    {
        var diagnostics = context.Request.Context.Diagnostics.Where(diagnostic =>
            diagnostic is { Severity: DiagnosticSeverity.Error, Code: { } code } &&
            code.TryGetSecond(out var str) &&
            s_supportedDiagnostics.Any(d => str.Equals(d, StringComparison.OrdinalIgnoreCase)));

        if (diagnostics is null || !diagnostics.Any())
        {
            return Array.Empty<RazorVSInternalCodeAction>();
        }

        var typeAccessibilityCodeActions = new List<RazorVSInternalCodeAction>();

        foreach (var diagnostic in diagnostics)
        {
            // Corner case handling for diagnostics which (momentarily) linger after
            // @code block is cleared out
            if (diagnostic.Range.End.Line > context.SourceText.Lines.Count ||
                diagnostic.Range.End.Character > context.SourceText.Lines[diagnostic.Range.End.Line].End)
            {
                continue;
            }

            var diagnosticSpan = diagnostic.Range.AsTextSpan(context.SourceText);

            // Based on how we compute `Range.AsTextSpan` it's possible to have a span
            // which goes beyond the end of the source text. Something likely changed
            // between the capturing of the diagnostic (by the platform) and the retrieval of the
            // document snapshot / source text. In such a case, we skip processing of the diagnostic.
            if (diagnosticSpan.End > context.SourceText.Length)
            {
                continue;
            }

            foreach (var codeAction in codeActions)
            {
                var name = codeAction.Name;
                if (name is null || !name.Equals(LanguageServerConstants.CodeActions.CodeActionFromVSCode, StringComparison.Ordinal))
                {
                    continue;
                }

                var associatedValue = context.SourceText.GetSubTextString(diagnosticSpan);

                var fqn = string.Empty;

                // When there's only one FQN suggestion, code action title is of the form:
                // `System.Net.Dns`
                if (!codeAction.Title.Any(c => char.IsWhiteSpace(c)) &&
                    codeAction.Title.EndsWith(associatedValue, StringComparison.OrdinalIgnoreCase))
                {
                    fqn = codeAction.Title;
                }
                // When there are multiple FQN suggestions, the code action title is of the form:
                // `Fully qualify 'Dns' -> System.Net.Dns`
                else
                {
                    var expectedCodeActionPrefix = $"Fully qualify '{associatedValue}' -> ";
                    if (codeAction.Title.StartsWith(expectedCodeActionPrefix, StringComparison.OrdinalIgnoreCase))
                    {
                        fqn = codeAction.Title.Substring(expectedCodeActionPrefix.Length);
                    }
                }

                if (string.IsNullOrEmpty(fqn))
                {
                    continue;
                }

                var fqnCodeAction = CreateFQNCodeAction(context, diagnostic, codeAction, fqn);
                typeAccessibilityCodeActions.Add(fqnCodeAction);

                if (AddUsingsCodeActionProviderHelper.TryCreateAddUsingResolutionParams(fqn, context.Request.TextDocument.Uri, out var @namespace, out var resolutionParams))
                {
                    var addUsingCodeAction = RazorCodeActionFactory.CreateAddComponentUsing(@namespace, resolutionParams);
                    typeAccessibilityCodeActions.Add(addUsingCodeAction);
                }
            }
        }

        return typeAccessibilityCodeActions;
    }

    private static IEnumerable<RazorVSInternalCodeAction> ProcessCodeActionsVS(
        RazorCodeActionContext context,
        IEnumerable<RazorVSInternalCodeAction> codeActions)
    {
        var typeAccessibilityCodeActions = new List<RazorVSInternalCodeAction>(1);

        foreach (var codeAction in codeActions)
        {
            if (codeAction.Name is not null && codeAction.Name.Equals(RazorPredefinedCodeFixProviderNames.FullyQualify, StringComparison.Ordinal))
            {
                string action;

                if (!TryGetOwner(context, out var owner))
                {
                    // Failed to locate a valid owner for the light bulb
                    continue;
                }
                else if (IsSingleLineDirectiveNode(owner))
                {
                    // Don't support single line directives
                    continue;
                }
                else if (IsExplicitExpressionNode(owner))
                {
                    // Don't support explicit expressions
                    continue;
                }
                else if (IsImplicitExpressionNode(owner))
                {
                    action = LanguageServerConstants.CodeActions.UnformattedRemap;
                }
                else
                {
                    // All other scenarios we support default formatted code action behavior
                    action = LanguageServerConstants.CodeActions.Default;
                }

                typeAccessibilityCodeActions.Add(codeAction.WrapResolvableCodeAction(context, action));
            }
            // For add using suggestions, the code action title is of the form:
            // `using System.Net;`
            else if (codeAction.Name is not null && codeAction.Name.Equals(RazorPredefinedCodeFixProviderNames.AddImport, StringComparison.Ordinal) &&
                AddUsingsCodeActionProviderHelper.TryExtractNamespace(codeAction.Title, out var @namespace))
            {
                codeAction.Title = $"@using {@namespace}";
                typeAccessibilityCodeActions.Add(codeAction.WrapResolvableCodeAction(context, LanguageServerConstants.CodeActions.AddUsing));
            }
            // Not a type accessibility code action
            else
            {
                continue;
            }
        }

        return typeAccessibilityCodeActions;

        static bool TryGetOwner(RazorCodeActionContext context, [NotNullWhen(true)] out SyntaxNode? owner)
        {
            var change = new SourceChange(context.Location.AbsoluteIndex, length: 0, newText: string.Empty);
            var syntaxTree = context.CodeDocument.GetSyntaxTree();
            if (syntaxTree?.Root is null)
            {
                owner = null;
                return false;
            }

            owner = syntaxTree.Root.LocateOwner(change);
            if (owner is null)
            {
                Debug.Fail("Owner should never be null.");
                return false;
            }

            return true;
        }

        static bool IsImplicitExpressionNode(SyntaxNode owner)
        {
            // E.g, (| is position)
            //
            // `@|foo` - true
            //
            return owner.AncestorsAndSelf().Any(n => n is CSharpImplicitExpressionSyntax);
        }

        static bool IsExplicitExpressionNode(SyntaxNode owner)
        {
            // E.g, (| is position)
            //
            // `@(|foo)` - true
            //
            return owner.AncestorsAndSelf().Any(n => n is CSharpExplicitExpressionBodySyntax);
        }

        static bool IsSingleLineDirectiveNode(SyntaxNode owner)
        {
            // E.g, (| is position)
            //
            // `@inject |SomeType SomeName` - true
            //
            return owner.AncestorsAndSelf().Any(
                n => n is RazorDirectiveSyntax directive && directive.DirectiveDescriptor.Kind == DirectiveKind.SingleLine);
        }
    }

    private static RazorVSInternalCodeAction CreateFQNCodeAction(
        RazorCodeActionContext context,
        Diagnostic fqnDiagnostic,
        RazorVSInternalCodeAction nonFQNCodeAction,
        string fullyQualifiedName)
    {
        var codeDocumentIdentifier = new OptionalVersionedTextDocumentIdentifier() { Uri = context.Request.TextDocument.Uri };

        var fqnTextEdit = new TextEdit()
        {
            NewText = fullyQualifiedName,
            Range = fqnDiagnostic.Range
        };

        var fqnWorkspaceEditDocumentChange = new TextDocumentEdit()
        {
            TextDocument = codeDocumentIdentifier,
            Edits = new[] { fqnTextEdit },
        };

        var fqnWorkspaceEdit = new WorkspaceEdit()
        {
            DocumentChanges = new[] { fqnWorkspaceEditDocumentChange }
        };

        var codeAction = RazorCodeActionFactory.CreateFullyQualifyComponent(nonFQNCodeAction.Title, fqnWorkspaceEdit);
        return codeAction;
    }
}
