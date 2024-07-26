// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.CodeAnalysis.Text;

internal static class LinePositionExtensions
{
    public static void Deconstruct(this LinePosition linePosition, out int line, out int character)
        => (line, character) = (linePosition.Line, linePosition.Character);

    public static LinePositionSpan ToZeroWidthSpan(this LinePosition linePosition)
        => new(linePosition, linePosition);
}
