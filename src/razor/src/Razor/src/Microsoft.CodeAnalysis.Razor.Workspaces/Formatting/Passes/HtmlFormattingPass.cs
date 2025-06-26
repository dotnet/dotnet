// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

/// <summary>
/// Gets edits in Razor files, and returns edits to Razor files, with nicely formatted Html
/// </summary>
internal sealed class HtmlFormattingPass(ILoggerFactory loggerFactory) : HtmlFormattingPassBase(loggerFactory.GetOrCreateLogger<HtmlFormattingPass>())
{
}
