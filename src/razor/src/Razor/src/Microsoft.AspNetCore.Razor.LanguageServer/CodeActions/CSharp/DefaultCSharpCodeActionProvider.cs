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
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal sealed class DefaultCSharpCodeActionProvider : ICSharpCodeActionProvider
{
    private static readonly Task<IReadOnlyList<RazorVSInternalCodeAction>?> s_emptyResult =
        Task.FromResult<IReadOnlyList<RazorVSInternalCodeAction>?>(Array.Empty<RazorVSInternalCodeAction>());

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
        RazorPredefinedCodeRefactoringProviderNames.IntroduceVariable,
        RazorPredefinedCodeRefactoringProviderNames.ConvertBetweenRegularAndVerbatimInterpolatedString,
        RazorPredefinedCodeRefactoringProviderNames.ConvertBetweenRegularAndVerbatimString,
        RazorPredefinedCodeRefactoringProviderNames.ConvertConcatenationToInterpolatedString,
        RazorPredefinedCodeRefactoringProviderNames.ConvertPlaceholderToInterpolatedString,
        RazorPredefinedCodeRefactoringProviderNames.ConvertToInterpolatedString,
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

    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;

    public DefaultCSharpCodeActionProvider(LanguageServerFeatureOptions languageServerFeatureOptions)
    {
        _languageServerFeatureOptions = languageServerFeatureOptions;
    }

    public Task<IReadOnlyList<RazorVSInternalCodeAction>?> ProvideAsync(
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
            return s_emptyResult;
        }

        var tree = context.CodeDocument.GetSyntaxTree();
        var node = tree.Root.FindInnermostNode(context.Location.AbsoluteIndex);
        var isInImplicitExpression = node?.AncestorsAndSelf().Any(n => n is CSharpImplicitExpressionSyntax) ?? false;

        var allowList = isInImplicitExpression
            ? SupportedImplicitExpressionCodeActionNames
            : SupportedDefaultCodeActionNames;

        var results = new List<RazorVSInternalCodeAction>();

        foreach (var codeAction in codeActions)
        {
            var isOnAllowList = codeAction.Name is not null && allowList.Contains(codeAction.Name);

            if (_languageServerFeatureOptions.ShowAllCSharpCodeActions && codeAction.Data is not null)
            {
                // If this code action isn't on the allow list, it might have been handled by another provider, which means
                // it will already have been wrapped, so we have to check not to double-wrap it. Unfortunately there isn't a
                // good way to do this, but to try and deserialize some Json. Since this only needs to happen if the feature
                // flag is on, any perf hit here isn't going to affect real users.
                try
                {
                    if (((JToken)codeAction.Data).ToObject<RazorCodeActionResolutionParams>() is not null)
                    {
                        // This code action has already been wrapped by something else, so skip it here, or it could
                        // be marked as experimental when its not, and more importantly would be duplicated in the list.
                        continue;
                    }
                }
                catch
                {
                }
            }

            if (_languageServerFeatureOptions.ShowAllCSharpCodeActions || isOnAllowList)
            {
                results.Add(codeAction.WrapResolvableCodeAction(context, isOnAllowList: isOnAllowList));
            }
        }

        return Task.FromResult<IReadOnlyList<RazorVSInternalCodeAction>?>(results);
    }
}
