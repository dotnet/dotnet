// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.CodeAnalysis.Remote.Razor.Completion;

[Export(typeof(RazorCompletionListProvider)), Shared]
[method: ImportingConstructor]
internal sealed class OOPRazorCompletionListProvider(
    IRazorCompletionFactsService completionFactsService,
    CompletionListCache completionListCache,
    ILoggerFactory loggerFactory)
    : RazorCompletionListProvider(completionFactsService, completionListCache, loggerFactory)
{
}
