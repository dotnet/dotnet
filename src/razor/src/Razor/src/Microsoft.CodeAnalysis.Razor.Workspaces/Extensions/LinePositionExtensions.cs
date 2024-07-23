// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using RLSP = Roslyn.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal static class LinePositionExtensions
{
    public static Position ToPosition(this LinePosition linePosition)
        => new Position(linePosition.Line, linePosition.Character);

    public static RLSP.Position ToRLSPPosition(this LinePosition linePosition)
        => new RLSP.Position(linePosition.Line, linePosition.Character);

    public static bool TryGetAbsoluteIndex(this LinePosition position, SourceText sourceText, ILogger logger, out int absoluteIndex)
        => sourceText.TryGetAbsoluteIndex(position.Line, position.Character, logger, out absoluteIndex);

    public static int GetRequiredAbsoluteIndex(this LinePosition position, SourceText sourceText, ILogger? logger = null)
        => sourceText.GetRequiredAbsoluteIndex(position.Line, position.Character, logger);
}
