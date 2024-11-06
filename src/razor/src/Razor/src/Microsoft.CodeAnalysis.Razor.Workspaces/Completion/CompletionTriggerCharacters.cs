// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Frozen;
using System.Linq;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal static class CompletionTriggerAndCommitCharacters
{
    public static FrozenSet<string> RazorTriggerCharacters { get; } = new[] { "@", "<", ":", " " }.ToFrozenSet();
    /// <summary>
    /// Tigger characters that can trigger both Razor and Delegation completion (e.g."@" triggers Razor directives and C#)
    /// </summary>
    public static FrozenSet<string> RazorDelegationTriggerCharacters { get; } = new[] { "@" }.ToFrozenSet();
    public static FrozenSet<string> CSharpTriggerCharacters { get; } = new[] { " ", "(", "=", "#", ".", "<", "[", "{", "\"", "/", ":", "~" }.ToFrozenSet();
    public static FrozenSet<string> HtmlTriggerCharacters { get; } = new[] { ":", "@", "#", ".", "!", "*", ",", "(", "[", "-", "<", "&", "\\", "/", "'", "\"", "=", ":", " ", "`" }.ToFrozenSet();
    public static FrozenSet<string> AllDelegationTriggerCharacters { get; } = RazorDelegationTriggerCharacters.Union(CSharpTriggerCharacters).Union(HtmlTriggerCharacters).ToFrozenSet();
    public static string[] AllTriggerCharacters { get; } = RazorTriggerCharacters.Union(AllDelegationTriggerCharacters).ToArray();

    /// <summary>
    /// This is the intersection of C# and HTML commit characters.
    /// </summary>
    // We need to specify it so that platform can correctly calculate ApplicableToSpan in
    // https://devdiv.visualstudio.com/DevDiv/_git/VSLanguageServerClient?path=/src/product/RemoteLanguage/Impl/Features/Completion/AsyncCompletionSource.cs&version=GBdevelop&line=855&lineEnd=855&lineStartColumn=9&lineEndColumn=49&lineStyle=plain&_a=contents
    // This is needed to fix https://github.com/dotnet/razor/issues/10787 in particular
    public static string[] AllCommitCharacters = [" ", ">", ";", "="];

    public static bool IsValidTrigger(FrozenSet<string> triggerCharacters, CompletionContext completionContext)
        => completionContext.TriggerKind != CompletionTriggerKind.TriggerCharacter ||
           completionContext.TriggerCharacter is null ||
           triggerCharacters.Contains(completionContext.TriggerCharacter);
}
