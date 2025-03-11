// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal class CompletionTriggerAndCommitCharacters
{
    /// <summary>
    ///  Trigger character that can trigger both Razor and Delegation completion
    /// </summary>
    private const char TransitionCharacter = '@';

    private static readonly char[] s_vsHtmlTriggerCharacters = [':', '#', '.', '!', '*', ',', '(', '[', '-', '<', '&', '\\', '/', '\'', '"', '=', ':', ' ', '`'];
    private static readonly char[] s_vsCodeHtmlTriggerCharacters = ['#', '.', '!', ',', '-', '<'];
    private static readonly char[] s_razorTriggerCharacters = ['<', ':', ' '];
    private static readonly char[] s_csharpTriggerCharacters = [' ', '(', '=', '#', '.', '<', '[', '{', '"', '/', ':', '~'];
    private static readonly char[] s_commitCharacters = [' ', '>', ';', '='];

    private readonly HashSet<char> _csharpTriggerCharacters;
    private readonly HashSet<char> _delegationTriggerCharacters;
    private readonly HashSet<char> _htmlTriggerCharacters;
    private readonly HashSet<char> _razorTriggerCharacters;

    public ImmutableArray<string> AllTriggerCharacters { get; }

    /// <summary>
    /// This is the intersection of C# and HTML commit characters.
    /// </summary>
    // We need to specify it so that platform can correctly calculate ApplicableToSpan in
    // https://devdiv.visualstudio.com/DevDiv/_git/VSLanguageServerClient?path=/src/product/RemoteLanguage/Impl/Features/Completion/AsyncCompletionSource.cs&version=GBdevelop&line=855&lineEnd=855&lineStartColumn=9&lineEndColumn=49&lineStyle=plain&_a=contents
    // This is needed to fix https://github.com/dotnet/razor/issues/10787 in particular
    public ImmutableArray<string> AllCommitCharacters { get; }

    public CompletionTriggerAndCommitCharacters(LanguageServerFeatureOptions languageServerFeatureOptions)
    {
        // C# trigger characters (do NOT include '@')
        var csharpTriggerCharacters = new HashSet<char>();
        csharpTriggerCharacters.UnionWith(s_csharpTriggerCharacters);

        // HTML trigger characters (include '@' + HTML trigger characters)
        var htmlTriggerCharacters = new HashSet<char>() { TransitionCharacter };

        if (languageServerFeatureOptions.UseVsCodeCompletionTriggerCharacters)
        {
            htmlTriggerCharacters.UnionWith(s_vsCodeHtmlTriggerCharacters);
        }
        else
        {
            htmlTriggerCharacters.UnionWith(s_vsHtmlTriggerCharacters);
        }

        // Delegation trigger characters (include '@' + C# and HTML trigger characters)
        var delegationTriggerCharacters = new HashSet<char> { TransitionCharacter };
        delegationTriggerCharacters.UnionWith(csharpTriggerCharacters);
        delegationTriggerCharacters.UnionWith(htmlTriggerCharacters);

        // Razor trigger characters (include '@' + Razor trigger characters)
        var razorTriggerCharacters = new HashSet<char>() { TransitionCharacter };
        razorTriggerCharacters.UnionWith(s_razorTriggerCharacters);

        // All trigger characters (include Razor + Delegation trigger characters)
        var allTriggerCharacters = new HashSet<char>();
        allTriggerCharacters.UnionWith(razorTriggerCharacters);
        allTriggerCharacters.UnionWith(delegationTriggerCharacters);

        var commitCharacters = new HashSet<char>();
        commitCharacters.UnionWith(s_commitCharacters);

        _csharpTriggerCharacters = csharpTriggerCharacters;
        _htmlTriggerCharacters = htmlTriggerCharacters;
        _razorTriggerCharacters = razorTriggerCharacters;
        _delegationTriggerCharacters = delegationTriggerCharacters;
        AllTriggerCharacters = allTriggerCharacters.SelectAsArray(static c => c.ToString());
        AllCommitCharacters = commitCharacters.SelectAsArray(static c => c.ToString());
    }

    public bool IsValidCSharpTrigger(CompletionContext completionContext)
        => IsValidTrigger(completionContext, _csharpTriggerCharacters);

    public bool IsValidDelegationTrigger(CompletionContext completionContext)
        => IsValidTrigger(completionContext, _delegationTriggerCharacters);

    public bool IsValidHtmlTrigger(CompletionContext completionContext)
        => IsValidTrigger(completionContext, _htmlTriggerCharacters);

    public bool IsValidRazorTrigger(CompletionContext completionContext)
        => IsValidTrigger(completionContext, _razorTriggerCharacters);

    private static bool IsValidTrigger(CompletionContext completionContext, HashSet<char> triggerCharacters)
        => completionContext.TriggerKind != CompletionTriggerKind.TriggerCharacter ||
           completionContext.TriggerCharacter is not [var c] ||
           triggerCharacters.Contains(c);

    public bool IsCSharpTriggerCharacter(string ch)
        => ch is [var c] && _csharpTriggerCharacters.Contains(c);

    public bool IsDelegationTriggerCharacter(string ch)
        => ch is [var c] && _delegationTriggerCharacters.Contains(c);

    public bool IsHtmlTriggerCharacter(string ch)
        => ch is [var c] && _htmlTriggerCharacters.Contains(c);

    public bool IsRazorTriggerCharacter(string ch)
        => ch is [var c] && _razorTriggerCharacters.Contains(c);

    public bool IsTransitionCharacter(string ch)
        => ch is [TransitionCharacter];
}
