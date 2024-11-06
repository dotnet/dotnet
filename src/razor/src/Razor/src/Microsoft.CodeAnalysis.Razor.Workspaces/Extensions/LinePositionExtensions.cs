// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.CodeAnalysis.Text;

internal static class LinePositionExtensions
{
    public static void Deconstruct(this LinePosition linePosition, out int line, out int character)
        => (line, character) = (linePosition.Line, linePosition.Character);

    public static LinePositionSpan ToZeroWidthSpan(this LinePosition linePosition)
        => new(linePosition, linePosition);

    public static LinePosition WithLine(this LinePosition linePosition, int newLine)
        => new(newLine, linePosition.Character);

    public static LinePosition WithLine(this LinePosition linePosition, Func<int, int> computeNewLine)
        => new(computeNewLine(linePosition.Line), linePosition.Character);

    public static LinePosition WithCharacter(this LinePosition linePosition, int newCharacter)
        => new(linePosition.Line, newCharacter);

    public static LinePosition WithCharacter(this LinePosition linePosition, Func<int, int> computeNewCharacter)
        => new(linePosition.Line, computeNewCharacter(linePosition.Character));
}
