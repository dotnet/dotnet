// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;
using RazorSyntaxNode = Microsoft.AspNetCore.Razor.Language.Syntax.SyntaxNode;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal record RazorCompletionContext(
    int AbsoluteIndex,
    RazorSyntaxNode? Owner,
    RazorSyntaxTree SyntaxTree,
    TagHelperDocumentContext TagHelperDocumentContext,
    CompletionReason Reason = CompletionReason.Invoked,
    RazorCompletionOptions Options = default,
    HashSet<string>? ExistingCompletions = null)
{
}
