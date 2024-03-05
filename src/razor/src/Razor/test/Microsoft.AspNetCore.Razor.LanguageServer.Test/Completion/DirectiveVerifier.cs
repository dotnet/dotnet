// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

internal static class DirectiveVerifier
{
    private static readonly Action<CompletionItem>[] s_defaultDirectiveCollectionVerifiers;

    public static Action<CompletionItem>[] DefaultDirectiveCollectionVerifiers => s_defaultDirectiveCollectionVerifiers;

    static DirectiveVerifier()
    {
        var defaultDirectiveVerifierList = new List<Action<CompletionItem>>(DirectiveCompletionItemProvider.DefaultDirectives.Count() * 2);

        foreach (var directive in DirectiveCompletionItemProvider.DefaultDirectives)
        {
            defaultDirectiveVerifierList.Add(item => Assert.Equal(directive.Directive, item.InsertText));
            defaultDirectiveVerifierList.Add(item => AssertDirectiveSnippet(item, directive.Directive));
        }

        s_defaultDirectiveCollectionVerifiers = defaultDirectiveVerifierList.ToArray();
    }

    private static void AssertDirectiveSnippet(CompletionItem completionItem, string directive)
    {
        Assert.StartsWith(directive, completionItem.InsertText);
        Assert.Equal(DirectiveCompletionItemProvider.s_singleLineDirectiveSnippets[directive].InsertText, completionItem.InsertText);
        Assert.Equal(CompletionItemKind.Snippet, completionItem.Kind);
    }
}
