// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using LanguageServerSR = Microsoft.AspNetCore.Razor.LanguageServer.Resources.SR;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;

internal static class PositionExtensions
{
    public static bool TryGetAbsoluteIndex(this Position position, SourceText sourceText, ILogger logger, out int absoluteIndex)
    {
        if (position is null)
        {
            throw new ArgumentNullException(nameof(position));
        }

        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        var linePosition = new LinePosition(position.Line, position.Character);
        if (linePosition.Line >= sourceText.Lines.Count)
        {
#pragma warning disable CA2254 // Template should be a static expression
            // Resources.GetResourceString won't vary, so this is fine
            logger?.LogError(LanguageServerSR.FormatPositionIndex_Outside_Range(
                position.Line,
                nameof(sourceText),
                sourceText.Lines.Count));
#pragma warning restore CA2254 // Template should be a static expression
            absoluteIndex = -1;
            return false;
        }

        var index = sourceText.Lines.GetPosition(linePosition);
        absoluteIndex = index;
        return true;
    }

    public static int CompareTo(this Position position, Position other)
    {
        if (position is null)
        {
            throw new ArgumentNullException(nameof(position));
        }

        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        var result = position.Line.CompareTo(other.Line);
        return result != 0 ? result : position.Character.CompareTo(other.Character);
    }

    public static bool IsValid(this Position position, SourceText sourceText)
    {
        if (position is null)
        {
            throw new ArgumentNullException(nameof(position));
        }

        if (sourceText is null)
        {
            throw new ArgumentNullException(nameof(sourceText));
        }

        return position.Line >= 0 &&
            position.Character >= 0 &&
            position.Line < sourceText.Lines.Count &&
            sourceText.Lines[position.Line].Start + position.Character <= sourceText.Length;
    }
}
