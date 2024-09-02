// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

/// <summary>
/// Gets edits in Html files, and returns edits to Razor files, with nicely formatted Html
/// </summary>
internal sealed class HtmlOnTypeFormattingPass(ILoggerFactory loggerFactory) : HtmlFormattingPassBase(loggerFactory.GetOrCreateLogger<HtmlOnTypeFormattingPass>())
{
    public override Task<TextEdit[]> ExecuteAsync(FormattingContext context, TextEdit[] edits, CancellationToken cancellationToken)
    {
        if (edits.Length == 0)
        {
            // There are no HTML edits for us to apply. No op.
            return Task.FromResult<TextEdit[]>([]);
        }

        return base.ExecuteAsync(context, edits, cancellationToken);
    }
}
