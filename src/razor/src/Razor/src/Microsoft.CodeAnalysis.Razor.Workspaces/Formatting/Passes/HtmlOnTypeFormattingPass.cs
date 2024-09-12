// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

/// <summary>
/// Gets edits in Html files, and returns edits to Razor files, with nicely formatted Html
/// </summary>
internal sealed class HtmlOnTypeFormattingPass(ILoggerFactory loggerFactory) : HtmlFormattingPassBase(loggerFactory.GetOrCreateLogger<HtmlOnTypeFormattingPass>())
{
    public override Task<ImmutableArray<TextChange>> ExecuteAsync(FormattingContext context, ImmutableArray<TextChange> changes, CancellationToken cancellationToken)
    {
        if (changes.Length == 0)
        {
            // There are no HTML edits for us to apply. No op.
            return SpecializedTasks.EmptyImmutableArray<TextChange>();
        }

        return base.ExecuteAsync(context, changes, cancellationToken);
    }
}
