// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.CodeAnalysis;
using SyntaxFacts = Microsoft.CodeAnalysis.CSharp.SyntaxFacts;
using SyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Razor;

internal class GenerateMethodCodeActionProvider : IRazorCodeActionProvider
{
    private static readonly Task<IReadOnlyList<RazorVSInternalCodeAction>?> s_emptyResult = Task.FromResult<IReadOnlyList<RazorVSInternalCodeAction>?>(null);

    public Task<IReadOnlyList<RazorVSInternalCodeAction>?> ProvideAsync(RazorCodeActionContext context, CancellationToken cancellationToken)
    {
        var nameNotExistDiagnostics = context.Request.Context.Diagnostics.Any(d => d.Code == "CS0103");
        if (!nameNotExistDiagnostics)
        {
            return s_emptyResult;
        }

        var syntaxTree = context.CodeDocument.GetSyntaxTree();
        var owner = syntaxTree.Root.FindToken(context.Location.AbsoluteIndex).Parent;
        Assumes.NotNull(owner);

        if (IsGenerateEventHandlerValid(owner, out var methodName, out var eventName))
        {
            var uri = context.Request.TextDocument.Uri;
            var codeActions = new List<RazorVSInternalCodeAction>()
            {
                RazorCodeActionFactory.CreateGenerateMethod(uri, methodName, eventName),
                RazorCodeActionFactory.CreateAsyncGenerateMethod(uri, methodName, eventName)
            };
            return Task.FromResult<IReadOnlyList<RazorVSInternalCodeAction>?>(codeActions);
        }

        return s_emptyResult;
    }

    private static bool IsGenerateEventHandlerValid(
        SyntaxNode owner,
        [NotNullWhen(true)] out string? methodName,
        [NotNullWhen(true)] out string? eventName)
    {
        methodName = null;
        eventName = null;

        // The owner should have a SyntaxKind of CSharpExpressionLiteral or MarkupTextLiteral.
        // MarkupTextLiteral if the cursor is directly before the first letter of the method name.
        // CSharpExpressionalLiteral if cursor is anywhere else in the method name.
        if (owner.Kind != SyntaxKind.CSharpExpressionLiteral && owner.Kind != SyntaxKind.MarkupTextLiteral)
        {
            return false;
        }

        // We want to get MarkupTagHelperDirectiveAttribute since this has information about the event name.
        // Hierarchy:
        // MarkupTagHelperDirectiveAttribute > MarkupTextLiteral
        // or
        // MarkupTagHelperDirectiveAttribute > MarkupTagHelperAttributeValue > CSharpExpressionLiteral
        var commonParent = owner.Kind == SyntaxKind.CSharpExpressionLiteral ? owner.Parent.Parent : owner.Parent;
        if (commonParent is not MarkupTagHelperDirectiveAttributeSyntax markupTagHelperDirectiveAttribute)
        {
            return false;
        }

        // MarkupTagHelperElement > MarkupTagHelperStartTag > MarkupTagHelperDirectiveAttribute 
        var tagHelperElement = commonParent.Parent.Parent as MarkupTagHelperElementSyntax;
        if (tagHelperElement is null)
        {
            return false;
        }

        foreach (var tagHelperDescriptor in tagHelperElement.TagHelperInfo.BindingResult.Descriptors)
        {
            foreach (var attribute in tagHelperDescriptor.BoundAttributes)
            {
                if (attribute.Name == markupTagHelperDirectiveAttribute.TagHelperAttributeInfo.Name)
                {
                    // We found the attribute that matches the directive attribute, now we need to check if the
                    // tag helper it's bound to is an event handler. This filters out things like @ref and @rendermode
                    if (!tagHelperDescriptor.IsEventHandlerTagHelper())
                    {
                        return false;
                    }

                    break;
                }
            }
        }

        if (markupTagHelperDirectiveAttribute.TagHelperAttributeInfo.ParameterName is not null)
        {
            // An event parameter is being set instead of the event handler e.g.
            // <button @onclick:preventDefault=SomeValue/>, this is not a generate event handler scenario.
            return false;
        }

        // The TagHelperAttributeInfo Name property includes the '@' in the beginning so exclude it.
        eventName = markupTagHelperDirectiveAttribute.TagHelperAttributeInfo.Name[1..];

        var content = markupTagHelperDirectiveAttribute.Value.GetContent();
        if (!SyntaxFacts.IsValidIdentifier(content))
        {
            return false;
        }

        methodName = content;
        return true;
    }
}
