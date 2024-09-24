// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.VisualStudio.LanguageServer.Protocol;

internal static partial class VsLspExtensions
{
    public static int GetPosition(this SourceText text, Position position)
        => text.GetPosition(position.ToLinePosition());

    public static Position GetPosition(this SourceText text, int position)
        => text.GetLinePosition(position).ToPosition();

    public static Range GetRange(this SourceText text, TextSpan span)
        => text.GetLinePositionSpan(span).ToRange();

    public static Range GetRange(this SourceText text, SourceSpan span)
        => text.GetLinePositionSpan(span).ToRange();

    public static Range GetRange(this SourceText text, int start, int end)
        => text.GetLinePositionSpan(start, end).ToRange();

    public static Range GetZeroWidthRange(this SourceText text, int position)
        => text.GetLinePosition(position).ToZeroWidthRange();

    public static bool IsValidPosition(this SourceText text, Position position)
        => text.IsValidPosition(position.Line, position.Character);

    public static bool TryGetAbsoluteIndex(this SourceText text, Position position, out int absoluteIndex)
        => text.TryGetAbsoluteIndex(position.Line, position.Character, out absoluteIndex);

    public static int GetRequiredAbsoluteIndex(this SourceText text, Position position)
        => text.GetRequiredAbsoluteIndex(position.Line, position.Character);

    public static TextSpan GetTextSpan(this SourceText text, Range range)
        => text.GetTextSpan(range.Start.Line, range.Start.Character, range.End.Line, range.End.Character);

    public static bool TryGetSourceLocation(this SourceText text, Position position, out SourceLocation location)
        => text.TryGetSourceLocation(position.Line, position.Character, out location);

    public static TextChange GetTextChange(this SourceText text, TextEdit edit)
        => new(text.GetTextSpan(edit.Range), edit.NewText);

    public static TextEdit GetTextEdit(this SourceText text, TextChange change)
        => VsLspFactory.CreateTextEdit(text.GetRange(change.Span), change.NewText ?? "");
}
