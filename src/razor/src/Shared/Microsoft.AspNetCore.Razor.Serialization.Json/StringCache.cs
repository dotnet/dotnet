// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

/// <summary>
/// This class helps de-duplicate dynamically created strings which might otherwise lead to memory bloat.
/// </summary>
internal sealed partial class StringCache(int capacity = 1024)
{
    // ConditionalWeakTable won't work for us because it only compares keys to Object.ReferenceEquals
    // (which won't be true because our values are loaded from JSON, not a constant).
    private readonly HashSet<Entry> _hashSet = new(new ExfiltratingEqualityComparer());
    private readonly object _lock = new();
    private int _capacity = capacity;

    public int ApproximateSize => _hashSet.Count;

    public string GetOrAddValue(string key)
    {
        lock (_lock)
        {
            ArgHelper.ThrowIfNull(key);

            if (TryGetValue(key, out var result))
            {
                return result;
            }

            _hashSet.Add(new(key));

            // Whenever we expand lets clean up dead references
            if (_capacity <= _hashSet.Count)
            {
                Cleanup();
                _capacity *= 2;
            }

            return key;
        }
    }

    private void Cleanup()
    {
        _hashSet.RemoveWhere(static weakRef => !weakRef.IsAlive);
    }

    private bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
    {
        if (_hashSet.Contains(new Entry(key)))
        {
            var comparer = (ExfiltratingEqualityComparer)_hashSet.Comparer;
            var val = comparer.LastEqualValue!;
            if (val.Value.TryGetTarget(out var target))
            {
                value = target!;
                return true;
            }

            ThrowHelper.ThrowInvalidOperationException("HashSet contained entry but we were unable to get the value from it. This should be impossible");
        }

        value = default;
        return false;
    }
}
