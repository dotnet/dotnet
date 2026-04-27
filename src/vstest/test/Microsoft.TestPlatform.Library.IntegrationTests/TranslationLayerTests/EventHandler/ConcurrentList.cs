// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

namespace Microsoft.TestPlatform.Library.IntegrationTests.TranslationLayerTests.EventHandler;

public sealed class ConcurrentList<T> : IList<T>
{
    private readonly object _lock = new();
    private readonly List<T> _list = new();

    public T this[int index]
    {
        get { lock (_lock) { return _list[index]; } }
        set { lock (_lock) { _list[index] = value; } }
    }

    public int Count { get { lock (_lock) { return _list.Count; } } }
    public bool IsReadOnly => false;

    public void AddRange(IEnumerable<T> items) { lock (_lock) { _list.AddRange(items); } }
    public void Add(T item) { lock (_lock) { _list.Add(item); } }
    public void Clear() { lock (_lock) { _list.Clear(); } }
    public bool Contains(T item) { lock (_lock) { return _list.Contains(item); } }
    public void CopyTo(T[] array, int arrayIndex) { lock (_lock) { _list.CopyTo(array, arrayIndex); } }
    public int IndexOf(T item) { lock (_lock) { return _list.IndexOf(item); } }
    public void Insert(int index, T item) { lock (_lock) { _list.Insert(index, item); } }
    public bool Remove(T item) { lock (_lock) { return _list.Remove(item); } }
    public void RemoveAt(int index) { lock (_lock) { _list.RemoveAt(index); } }

    // Enumerates over a snapshot — safe even if the list is modified during enumeration
    public IEnumerator<T> GetEnumerator() { lock (_lock) { return new List<T>(_list).GetEnumerator(); } }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Returns an independent copy of the internal list
    public List<T> ToList() { lock (_lock) { return new List<T>(_list); } }
}
