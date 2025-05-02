// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal interface ISnippetCompletionItemProvider
{
    void AddSnippetCompletions(ref PooledArrayBuilder<VSInternalCompletionItem> builder, RazorLanguageKind projectedKind, VSInternalCompletionInvokeKind invokeKind, string? triggerCharacter);
}
