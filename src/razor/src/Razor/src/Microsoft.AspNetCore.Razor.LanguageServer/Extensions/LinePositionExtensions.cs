// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class LinePositionExtensions
{
    public static Position ToPosition(this LinePosition linePosition)
        => new Position(linePosition.Line, linePosition.Character);

    public static bool TryGetAbsoluteIndex(this LinePosition position, SourceText sourceText, ILogger logger, out int absoluteIndex)
        => sourceText.TryGetAbsoluteIndex(position.Line, position.Character, logger, out absoluteIndex);

    public static int GetRequiredAbsoluteIndex(this LinePosition position, SourceText sourceText, ILogger? logger = null)
        => sourceText.GetRequiredAbsoluteIndex(position.Line, position.Character, logger);
}
