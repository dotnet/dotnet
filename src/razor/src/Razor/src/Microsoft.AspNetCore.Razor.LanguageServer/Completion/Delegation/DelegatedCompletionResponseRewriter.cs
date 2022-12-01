// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion.Delegation;

internal abstract class DelegatedCompletionResponseRewriter
{
    /// <summary>
    /// Defines the order in which the rewriter will run. Implementors of <see cref="DelegatedCompletionResponseRewriter"/> should utilize
    /// the <see cref="ExecutionBehaviorOrder"/> type to determine order.
    ///
    /// <see cref="Order"/> is only called once to determine order (needs to represent a static order).
    /// </summary>
    public abstract int Order { get; }

    public abstract Task<VSInternalCompletionList> RewriteAsync(
        VSInternalCompletionList completionList,
        int hostDocumentIndex,
        DocumentContext hostDocumentContext,
        DelegatedCompletionParams delegatedParameters,
        CancellationToken cancellationToken);

    protected static class ExecutionBehaviorOrder
    {
        public static readonly int FiltersCompletionItems = -20;

        public static readonly int AddsCompletionItems = -10;

        public static readonly int Default = 0;

        public static readonly int ChangesCompletionItems = 10;
    }
}
