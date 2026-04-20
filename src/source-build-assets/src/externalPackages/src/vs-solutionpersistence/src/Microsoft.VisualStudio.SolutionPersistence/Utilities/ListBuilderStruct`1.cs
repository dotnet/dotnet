// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Utilities;

/// <summary>
/// Provides a list builder that can be used to build a list of items without allocating
/// on the heap if the list is small.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
internal ref struct ListBuilderStruct<T>
{
    private List<T>? items;

    [MaybeNull]
    private T item0;

    [MaybeNull]
    private T item1;

    [MaybeNull]
    private T item2;

    [MaybeNull]
    private T item3;

    public ListBuilderStruct()
    {
    }

    internal ListBuilderStruct(int capacity)
    {
        if (capacity > 4)
        {
            this.items = new List<T>(capacity - 4);
        }
    }

    internal int Count { get; private set; }

    internal T this[int index]
    {
        readonly get
        {
            return index switch
            {
                0 => this.item0,
                1 => this.item1,
                2 => this.item2,
                3 => this.item3,
                _ => this.items![index - 4],
            };
        }

        set
        {
            switch (index)
            {
                case 0: this.item0 = value; break;
                case 1: this.item1 = value; break;
                case 2: this.item2 = value; break;
                case 3: this.item3 = value; break;
                default: this.items![index - 4] = value; break;
            }
        }
    }

    public readonly Enumerator GetEnumerator()
    {
        return new Enumerator(this);
    }

    internal void Add(T item)
    {
        switch (this.Count)
        {
            case 0: this.item0 = item; break;
            case 1: this.item1 = item; break;
            case 2: this.item2 = item; break;
            case 3: this.item3 = item; break;
            default:
                this.items ??= [];
                this.items.Add(item);
                break;
        }

        this.Count++;
    }

    internal void AddRange(IReadOnlyCollection<T> items)
    {
        int newCapacity = this.Count + items.Count;
        if (newCapacity > 4)
        {
            this.items ??= new List<T>(newCapacity - 4);
            this.items.Capacity = newCapacity - 4;
        }

        foreach (T item in items)
        {
            this.Add(item);
        }
    }

    internal readonly T[] ToArray()
    {
        return this.Count switch
        {
            0 => [],
            1 => [this.item0],
            2 => [this.item0, this.item1],
            3 => [this.item0, this.item1, this.item2],
            4 => [this.item0, this.item1, this.item2, this.item3],
            _ => [this.item0, this.item1, this.item2, this.item3, .. this.items!],
        };
    }

    internal void Clear()
    {
        this.Count = 0;
        this.items = null;
    }

    internal ref struct Enumerator(ListBuilderStruct<T> builder)
    {
        private readonly ListBuilderStruct<T> builder = builder;
        private int index = -1;

        public readonly T Current => this.builder[this.index];

        public bool MoveNext()
        {
            this.index++;
            return this.index < this.builder.Count;
        }
    }
}
