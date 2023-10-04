// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

internal static class PositionExtensions
{
    public static LinePosition ToLinePosition(this Position position)
        => new LinePosition(position.Line, position.Character);

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

        return sourceText.TryGetAbsoluteIndex(position.Line, position.Character, logger, out absoluteIndex);
    }

    public static int GetRequiredAbsoluteIndex(this Position position, SourceText sourceText, ILogger logger)
    {
        if (!position.TryGetAbsoluteIndex(sourceText, logger, out var absoluteIndex))
        {
            throw new InvalidOperationException();
        }

        return absoluteIndex;
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

        return sourceText.TryGetAbsoluteIndex(position.Line, position.Character, out _);
    }
}
