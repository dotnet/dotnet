// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CodeAnalysis;
using SyntaxFacts = Microsoft.CodeAnalysis.CSharp.SyntaxFacts;
using SyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Razor;

internal class GenerateMethodCodeActionProvider : IRazorCodeActionProvider
{
    private static readonly Task<IReadOnlyList<RazorVSInternalCodeAction>?> s_emptyResult = Task.FromResult<IReadOnlyList<RazorVSInternalCodeAction>?>(null);

    public Task<IReadOnlyList<RazorVSInternalCodeAction>?> ProvideAsync(RazorCodeActionContext context, CancellationToken cancellationToken)
    {
        var nameNotExistDiagnostics = context.Request.Context.Diagnostics.Where(d => d.Code == "CS0103");
        if (!nameNotExistDiagnostics.Any())
        {
            return s_emptyResult;
        }

        var change = new SourceChange(context.Location.AbsoluteIndex, length: 0, newText: string.Empty);
        var syntaxTree = context.CodeDocument.GetSyntaxTree();
        var owner = syntaxTree.Root.LocateOwner(change);
        if (owner is null)
        {
            return s_emptyResult;
        }

        if (IsGenerateEventHandlerValid(owner, context, out var @params))
        {
            return Task.FromResult<IReadOnlyList<RazorVSInternalCodeAction>?>(CreateCodeAction(@params));
        }

        return s_emptyResult;
    }

    private static List<RazorVSInternalCodeAction> CreateCodeAction(GenerateMethodCodeActionParams @params)
    {
        var resolutionParams = new RazorCodeActionResolutionParams()
        {
            Action = LanguageServerConstants.CodeActions.GenerateEventHandler,
            Language = LanguageServerConstants.CodeActions.Languages.Razor,
            Data = @params,
        };

        var codeAction = RazorCodeActionFactory.CreateGenerateMethod(resolutionParams);
        return new List<RazorVSInternalCodeAction> { codeAction };
    }

    private static bool IsGenerateEventHandlerValid(
        SyntaxNode owner,
        RazorCodeActionContext context,
        [NotNullWhen(true)] out GenerateMethodCodeActionParams? @params)
    {
        @params = null;

        // The owner should have a SyntaxKind of CSharpExpressionLiteral or MarkupTextLiteral.
        // MarkupTextLiteral if the cursor is directly before the first letter of the method name.
        // CSharpExpressionalLiteral if cursor is anywhere else in the method name.
        if (owner.Kind != SyntaxKind.CSharpExpressionLiteral && owner.Kind != SyntaxKind.MarkupTextLiteral)
        {
            return false;
        }

        var parent = owner.Kind == SyntaxKind.CSharpExpressionLiteral ? owner.Parent.Parent : owner.Parent;
        if (parent.Kind != SyntaxKind.MarkupTagHelperDirectiveAttribute)
        {
            return false;
        }

        var methodName = string.Empty;
        if (owner.Kind == SyntaxKind.CSharpExpressionLiteral)
        {
            var content = owner.GetContent();
            if (!SyntaxFacts.IsValidIdentifier(content))
            {
                return false;
            }

            methodName = content;
        }
        else
        {
            var children = parent.ChildNodes();
            foreach (var child in children)
            {
                if (child.Kind == SyntaxKind.MarkupTagHelperAttributeValue)
                {
                    var content = child.GetContent();
                    if (SyntaxFacts.IsValidIdentifier(content))
                    {
                        methodName = content;
                        break;
                    }
                }
            }
        }

        if (methodName.IsNullOrEmpty())
        {
            return false;
        }

        @params = new GenerateMethodCodeActionParams()
        {
            Uri = context.Request.TextDocument.Uri,
            MethodName = methodName,
        };

        return true;
    }
}
