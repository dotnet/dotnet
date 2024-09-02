// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

/// <summary>
/// Gets edits in Razor files, and returns edits to Razor files, with nicely formatted Html
/// </summary>
internal sealed class HtmlFormattingPass(ILoggerFactory loggerFactory) : HtmlFormattingPassBase(loggerFactory.GetOrCreateLogger<HtmlFormattingPass>())
{
}
