// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal class DefaultCSharpCodeActionProvider : CSharpCodeActionProvider
{
    // Internal for testing
    internal static readonly HashSet<string> SupportedDefaultCodeActionNames = new HashSet<string>()
    {
        RazorPredefinedCodeRefactoringProviderNames.GenerateEqualsAndGetHashCodeFromMembers,
        RazorPredefinedCodeRefactoringProviderNames.AddAwait,
        RazorPredefinedCodeRefactoringProviderNames.AddDebuggerDisplay,
        RazorPredefinedCodeRefactoringProviderNames.InitializeMemberFromParameter, // Create and assign (property|field)
        RazorPredefinedCodeRefactoringProviderNames.AddParameterCheck, // Add Null checks
        RazorPredefinedCodeRefactoringProviderNames.AddConstructorParametersFromMembers,
        RazorPredefinedCodeRefactoringProviderNames.GenerateDefaultConstructors,
        RazorPredefinedCodeRefactoringProviderNames.GenerateConstructorFromMembers,
        RazorPredefinedCodeRefactoringProviderNames.UseExpressionBody,
        RazorPredefinedCodeFixProviderNames.ImplementAbstractClass,
        RazorPredefinedCodeFixProviderNames.ImplementInterface,
        RazorPredefinedCodeFixProviderNames.RemoveUnusedVariable,
    };

    // We don't support any code actions in implicit expressions at the moment, but rather than simply returning early
    // I thought it best to create an allow list, empty, so that we can easily add them later if we identify any big
    // hitters that we want to enable.
    // The one example commented out here should not be taken as an opinion as to what that allow list should look like.
    internal static readonly HashSet<string> SupportedImplicitExpressionCodeActionNames = new HashSet<string>()
    {
        // RazorPredefinedCodeFixProviderNames.RemoveUnusedVariable,
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

        // Used to identify if this is VSCode which doesn't support
        // code action resolve.
        if (!context.SupportsCodeActionResolve)
        {
            return EmptyResult;
        }

        var tree = context.CodeDocument.GetSyntaxTree();
        var node = tree.GetOwner(context.Location.AbsoluteIndex);
        var isInImplicitExpression = node?.AncestorsAndSelf().Any(n => n is CSharpImplicitExpressionSyntax) ?? false;

        var allowList = isInImplicitExpression
            ? SupportedImplicitExpressionCodeActionNames
            : SupportedDefaultCodeActionNames;

        var results = new List<RazorVSInternalCodeAction>();

        foreach (var codeAction in codeActions)
        {
            if (codeAction.Name is not null && allowList.Contains(codeAction.Name))
            {
                results.Add(codeAction.WrapResolvableCodeAction(context));
            }
        }

        return Task.FromResult<IReadOnlyList<RazorVSInternalCodeAction>?>(results);
    }
}
