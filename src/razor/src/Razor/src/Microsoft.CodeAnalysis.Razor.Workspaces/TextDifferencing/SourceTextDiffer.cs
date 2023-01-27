// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.TextDifferencing;

internal abstract partial class SourceTextDiffer : TextDiffer, IDisposable
{
    protected readonly SourceText OldText;
    protected readonly SourceText NewText;

    protected SourceTextDiffer(SourceText oldText, SourceText newText)
    {
        OldText = oldText ?? throw new ArgumentNullException(nameof(oldText));
        NewText = newText ?? throw new ArgumentNullException(nameof(newText));
    }

    public abstract void Dispose();

    protected abstract int GetEditPosition(DiffEdit edit);
    protected abstract int AppendEdit(DiffEdit edit, StringBuilder builder);

    /// <summary>
    ///  Rents a char array of at least <paramref name="minimumLength"/> from the shared array pool.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static char[] RentArray(int minimumLength)
        => ArrayPool<char>.Shared.Rent(minimumLength);

    /// <summary>
    ///  Returns a char array to the shared array pool.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static void ReturnArray(char[] array, bool clearArray = false)
        => ArrayPool<char>.Shared.Return(array, clearArray);

    /// <summary>
    ///  Ensures that <paramref name="array"/> references a char array of at least <paramref name="minimumLength"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static char[] EnsureBuffer(ref char[] array, int minimumLength)
    {
        return array.Length >= minimumLength
            ? array
            : GetNewBuffer(ref array, minimumLength);

        static char[] GetNewBuffer(ref char[] array, int minimumLength)
        {
            // We need a larger buffer. Return this array to the pool
            // and rent a new one.
            ReturnArray(array);
            array = RentArray(minimumLength);

            return array;
        }
    }

    private IReadOnlyList<TextChange> ConsolidateEdits(List<DiffEdit> edits)
    {
        // Scan through the list of edits and collapse them into a minimal set of TextChanges.
        // This method assumes that there are no overlapping changes and the changes are sorted.

        var minimalChanges = new List<TextChange>();

        var start = 0;
        var end = 0;

        using var _ = StringBuilderPool.GetPooledObject(out var builder);

        foreach (var edit in edits)
        {
            var startPosition = GetEditPosition(edit);
            if (startPosition != end)
            {
                // Previous edit's end doesn't match the new edit's start.
                // Output the text change we were tracking.
                if (start != end || builder.Length > 0)
                {
                    minimalChanges.Add(new TextChange(TextSpan.FromBounds(start, end), builder.ToString()));
                    builder.Clear();
                }

                start = startPosition;
            }

            end = AppendEdit(edit, builder);
        }

        if (start != end || builder.Length > 0)
        {
            minimalChanges.Add(new TextChange(TextSpan.FromBounds(start, end), builder.ToString()));
        }

        return minimalChanges;
    }

    public static IReadOnlyList<TextChange> GetMinimalTextChanges(SourceText oldText, SourceText newText, DiffKind kind = DiffKind.Line)
    {
        if (oldText is null)
        {
            throw new ArgumentNullException(nameof(oldText));
        }

        if (newText is null)
        {
            throw new ArgumentNullException(nameof(newText));
        }

        if (oldText.ContentEquals(newText))
        {
            return Array.Empty<TextChange>();
        }
        else if (oldText.Length == 0 || newText.Length == 0)
        {
            return newText.GetTextChanges(oldText);
        }

        using SourceTextDiffer differ = kind == DiffKind.Line
            ? new LineDiffer(oldText, newText)
            : new CharDiffer(oldText, newText);

        var edits = differ.ComputeDiff();

        var changes = differ.ConsolidateEdits(edits);

        Debug.Assert(oldText.WithChanges(changes).ContentEquals(newText), "Incorrect minimal changes");

        return changes;
    }
}
