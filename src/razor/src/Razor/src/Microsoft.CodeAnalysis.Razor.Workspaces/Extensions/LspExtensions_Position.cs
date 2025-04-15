// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Text;

namespace Roslyn.LanguageServer.Protocol;

internal static partial class LspExtensions
{
    public static void Deconstruct(this Position position, out int line, out int character)
        => (line, character) = (position.Line, position.Character);

    public static LinePosition ToLinePosition(this Position position)
        => new(position.Line, position.Character);

    public static LspRange ToZeroWidthRange(this Position position)
        => LspFactory.CreateZeroWidthRange(position);

    public static int CompareTo(this Position position, Position other)
    {
        var result = position.Line.CompareTo(other.Line);
        return result != 0 ? result : position.Character.CompareTo(other.Character);
    }

    public static string ToDisplayString(this Position position)
        => $"({position.Line}, {position.Character})";
}
