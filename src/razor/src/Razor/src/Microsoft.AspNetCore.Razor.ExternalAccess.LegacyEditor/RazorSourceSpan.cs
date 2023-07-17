// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal record struct RazorSourceSpan(
    string? FilePath,
    int AbsoluteIndex,
    int LineIndex,
    int CharacterIndex,
    int Length,
    int LineCount,
    int EndCharacterIndex)
{
    public RazorSourceSpan(int absoluteIndex, int length)
        : this(filePath: null, absoluteIndex, lineIndex: -1, characterIndex: -1, length)
    {
    }

    public RazorSourceSpan(string? filePath, int absoluteIndex, int lineIndex, int characterIndex, int length)
        : this(filePath, absoluteIndex, lineIndex, characterIndex, length, LineCount: 0, EndCharacterIndex: 0)
    {
    }

    public RazorSourceSpan(int absoluteIndex, int lineIndex, int characterIndex, int length)
        : this(filePath: null, absoluteIndex, lineIndex, characterIndex, length)
    {
    }

    // Note: Equality for this record does not include LineCount or EndCharacterIndex to match
    // the equality defined by Microsoft.AspNetCore.Razor.Language.SourceSpan.
    public readonly bool Equals(RazorSourceSpan other)
        => FilePath == other.FilePath &&
           AbsoluteIndex == other.AbsoluteIndex &&
           LineIndex == other.LineIndex &&
           CharacterIndex == other.CharacterIndex &&
           Length == other.Length;

    public override readonly int GetHashCode()
    {
        var hash = HashCodeCombiner.Start();
        hash.Add(FilePath, StringComparer.Ordinal);
        hash.Add(AbsoluteIndex);
        hash.Add(LineIndex);
        hash.Add(CharacterIndex);
        hash.Add(Length);

        return hash;
    }
}
