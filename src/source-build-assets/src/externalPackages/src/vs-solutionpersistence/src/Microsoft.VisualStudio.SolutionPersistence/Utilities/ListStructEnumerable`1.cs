// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence;

/// <summary>
/// Creates a enumerable struct wrapper around a list that might be null.
/// </summary>
internal readonly ref struct ListStructEnumerable<T>(List<T>? list)
{
    private static readonly List<T> EmptyList = [];

    internal int Count => list?.Count ?? 0;

    internal List<T>.Enumerator GetEnumerator() => (list ?? EmptyList).GetEnumerator();
}

internal readonly ref struct ReadOnlyListStructEnumerable<T>(IReadOnlyList<T>? list)
{
    public ReadOnlyListStructEnumerator<T> GetEnumerator() => new ReadOnlyListStructEnumerator<T>(list);
}

internal ref struct ReadOnlyListStructEnumerator<T>(IReadOnlyList<T>? list)
{
    private int index = -1;

    public readonly T Current => list![this.index];

    public bool MoveNext() => ++this.index < (list?.Count ?? 0);
}

internal readonly ref struct ReadOnlyListStructReverseEnumerable<T>(IReadOnlyList<T>? list)
{
    public ReadOnlyListStructReverseEnumerator<T> GetEnumerator() => new ReadOnlyListStructReverseEnumerator<T>(list);
}

internal ref struct ReadOnlyListStructReverseEnumerator<T>(IReadOnlyList<T>? list)
{
    private int index = list?.Count ?? 0;

    public readonly T Current => list![this.index];

    public bool MoveNext()
    {
        this.index--;
        return this.index >= 0;
    }
}
