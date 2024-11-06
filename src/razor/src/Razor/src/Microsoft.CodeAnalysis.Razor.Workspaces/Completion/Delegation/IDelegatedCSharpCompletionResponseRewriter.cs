// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Completion.Delegation;

internal interface IDelegatedCSharpCompletionResponseRewriter
{
    public Task<VSInternalCompletionList> RewriteAsync(
        VSInternalCompletionList completionList,
        int hostDocumentIndex,
        DocumentContext hostDocumentContext,
        Position projectedPosition,
        RazorCompletionOptions completionOptions,
        CancellationToken cancellationToken);
}
