// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.Language;

/// <summary>
///  Generally, there are only a handful of metadata items stored on the various tag helper
///  objects. Often, it's just one or two items. To improve memory usage, MetadataCollection provides
///  multiple implementations to avoid creating a hash table.
/// </summary>
public abstract partial class MetadataCollection : IReadOnlyDictionary<string, string?>, IEquatable<MetadataCollection>
{
    public static readonly MetadataCollection Empty = NoItems.Instance;

    private int? _hashCode;

    protected MetadataCollection()
    {
    }

    public abstract string? this[string key] { get; }

    public abstract IEnumerable<string> Keys { get; }
    public abstract IEnumerable<string?> Values { get; }
    public abstract int Count { get; }

    public abstract bool ContainsKey(string key);
    public abstract bool TryGetValue(string key, out string? value);

    protected abstract KeyValuePair<string, string?> GetEntry(int index);

    public Enumerator GetEnumerator() => new(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    IEnumerator<KeyValuePair<string, string?>> IEnumerable<KeyValuePair<string, string?>>.GetEnumerator() => GetEnumerator();

    public sealed override bool Equals(object obj)
        => obj is MetadataCollection other && Equals(other);

    public abstract bool Equals(MetadataCollection other);

    protected abstract int ComputeHashCode();

    public sealed override int GetHashCode() => _hashCode ??= ComputeHashCode();

    public static MetadataCollection Create(KeyValuePair<string, string?> pair)
        => new OneToThreeItems(pair.Key, pair.Value);

    public static MetadataCollection Create(KeyValuePair<string, string?> pair1, KeyValuePair<string, string?> pair2)
        => new OneToThreeItems(pair1.Key, pair1.Value, pair2.Key, pair2.Value);

    public static MetadataCollection Create(KeyValuePair<string, string?> pair1, KeyValuePair<string, string?> pair2, KeyValuePair<string, string?> pair3)
        => new OneToThreeItems(pair1.Key, pair1.Value, pair2.Key, pair2.Value, pair3.Key, pair3.Value);

    public static MetadataCollection Create(params KeyValuePair<string, string?>[] pairs)
        => pairs switch
        {
            [] => Empty,
            [var pair] => new OneToThreeItems(pair.Key, pair.Value),
            [var pair1, var pair2] => new OneToThreeItems(pair1.Key, pair1.Value, pair2.Key, pair2.Value),
            [var pair1, var pair2, var pair3] => new OneToThreeItems(pair1.Key, pair1.Value, pair2.Key, pair2.Value, pair3.Key, pair3.Value),
            _ => new FourOrMoreItems(pairs),
        };

    public static MetadataCollection Create<T>(T pairs)
        where T : IReadOnlyList<KeyValuePair<string, string?>>
        => pairs switch
        {
            [] => Empty,
            [var pair] => new OneToThreeItems(pair.Key, pair.Value),
            [var pair1, var pair2] => new OneToThreeItems(pair1.Key, pair1.Value, pair2.Key, pair2.Value),
            [var pair1, var pair2, var pair3] => new OneToThreeItems(pair1.Key, pair1.Value, pair2.Key, pair2.Value, pair3.Key, pair3.Value),
            _ => new FourOrMoreItems(pairs),
        };

    public static MetadataCollection Create(Dictionary<string, string?> map)
    {
        var count = map.Count;

        if (count == 0)
        {
            return Empty;
        }

        if (count < 4)
        {
            // Optimize for the 1-3 case. On this path, we use the enumerator
            // to acquire key/value pairs.

            // Get the first pair.
            using var enumerator = map.GetEnumerator();

            if (!enumerator.MoveNext())
            {
                Assumed.Unreachable();
            }

            var pair1 = enumerator.Current;

            if (count == 1)
            {
                return new OneToThreeItems(pair1.Key, pair1.Value);
            }

            // We know there are at least two pairs, so get the second one.
            if (!enumerator.MoveNext())
            {
                Assumed.Unreachable();
            }

            var pair2 = enumerator.Current;

            if (count == 2)
            {
                return new OneToThreeItems(pair1.Key, pair1.Value, pair2.Key, pair2.Value);
            }

            // We know that there are three pairs, so get the final one.
            if (!enumerator.MoveNext())
            {
                Assumed.Unreachable();
            }

            var pair3 = enumerator.Current;

            return new OneToThreeItems(pair1.Key, pair1.Value, pair2.Key, pair2.Value, pair3.Key, pair3.Value);
        }

        // Finally, if there are four or more items, add the pairs to a list in order to construct
        // a FourOrMoreItems instance. Note that the constructor will copy the key-value pairs and won't
        // hold onto the list we're passing, so it's safe to use a pooled list.
        using var _ = ListPool<KeyValuePair<string, string?>>.GetPooledObject(out var list);
        list.SetCapacityIfLarger(count);

        foreach (var pair in map)
        {
            list.Add(pair);
        }

        return Create(list);
    }

    public static MetadataCollection Create(IReadOnlyDictionary<string, string?> map)
    {
        // Take a faster path if Dictionary<string, string?> is passed to us. This ensures that
        // we use Dictionary's struct-based enumerator rather than going through
        // IEnumerable<KeyValuePair<string, string>>.ToArray() below.

        if (map is Dictionary<string, string?> dictionary)
        {
            return Create(dictionary);
        }

        return Create(map.ToArray());
    }

    public static MetadataCollection CreateOrEmpty<T>(T? pairs)
        where T : IReadOnlyList<KeyValuePair<string, string?>>
        => pairs is { } realPairs ? Create(realPairs) : Empty;

    public static MetadataCollection CreateOrEmpty(Dictionary<string, string?>? map)
        => map is not null ? Create(map) : Empty;

    public static MetadataCollection CreateOrEmpty(IReadOnlyDictionary<string, string?>? map)
        => map is not null ? Create(map) : Empty;

    /// <summary>
    ///  This implementation represents an empty MetadataCollection.
    /// </summary>
    private sealed class NoItems : MetadataCollection
    {
        public static readonly NoItems Instance = new();

        private NoItems()
        {
        }

        public override string this[string key] => throw new KeyNotFoundException(Resources.FormatThe_given_key_0_was_not_present(key));

        public override IEnumerable<string> Keys => Array.Empty<string>();
        public override IEnumerable<string> Values => Array.Empty<string>();
        public override int Count => 0;

        public override bool ContainsKey(string key) => false;

        protected override KeyValuePair<string, string?> GetEntry(int index)
            => throw new InvalidOperationException();

        public override bool TryGetValue(string key, out string value)
        {
            value = null!;
            return false;
        }

        protected override int ComputeHashCode() => 0;
        public override bool Equals(MetadataCollection other) => ReferenceEquals(this, other);
    }

    /// <summary>
    ///  This implementation represents a MetadataCollection with 1, 2, or 3 key/value pairs that are
    ///  stored in explicit fields.
    /// </summary>
    private sealed class OneToThreeItems : MetadataCollection
    {
        private readonly string _key1;
        private readonly string? _value1;

        [AllowNull]
        private readonly string _key2;
        private readonly string? _value2;

        [AllowNull]
        private readonly string _key3;
        private readonly string? _value3;

        private readonly int _count;

        private string[]? _keys;
        private string?[]? _values;

        public OneToThreeItems(string key1, string? value1, string key2, string? value2, string key3, string? value3)
        {
            _key1 = key1 ?? throw new ArgumentNullException(nameof(key1));
            _value1 = value1;
            _key2 = key2 ?? throw new ArgumentNullException(nameof(key2));
            _value2 = value2;
            _key3 = key3 ?? throw new ArgumentNullException(nameof(key3));
            _value3 = value3;

            if (key1 == key2)
            {
                throw new ArgumentException(
                    Resources.FormatAn_item_with_the_same_key_has_already_been_added_Key_0(key2), nameof(key2));
            }

            if (key1 == key3)
            {
                throw new ArgumentException(
                    Resources.FormatAn_item_with_the_same_key_has_already_been_added_Key_0(key3), nameof(key3));
            }

            if (key2 == key3)
            {
                throw new ArgumentException(
                    Resources.FormatAn_item_with_the_same_key_has_already_been_added_Key_0(key3), nameof(key3));
            }

            _count = 3;
        }

        public OneToThreeItems(string key1, string? value1, string key2, string? value2)
        {
            _key1 = key1 ?? throw new ArgumentNullException(nameof(key1));
            _value1 = value1;
            _key2 = key2 ?? throw new ArgumentNullException(nameof(key2));
            _value2 = value2;

            if (key1 == key2)
            {
                throw new ArgumentException(
                    Resources.FormatAn_item_with_the_same_key_has_already_been_added_Key_0(key2), nameof(key2));
            }

            _count = 2;
        }

        public OneToThreeItems(string key, string? value)
        {
            _key1 = key ?? throw new ArgumentNullException(nameof(key));
            _value1 = value;

            _count = 1;
        }

        public override string? this[string key]
        {
            get
            {
                if (key == _key1)
                {
                    return _value1;
                }

                if (_count > 1 && key == _key2)
                {
                    return _value2;
                }

                if (_count > 2 && key == _key3)
                {
                    return _value3;
                }

                throw new KeyNotFoundException(
                    Resources.FormatThe_given_key_0_was_not_present(key));
            }
        }

        public override IEnumerable<string> Keys
        {
            get
            {
                return _keys ??= CreateKeys();

                string[] CreateKeys()
                {
                    return _count switch
                    {
                        1 => new[] { _key1 },
                        2 => new[] { _key1, _key2 },
                        3 => new[] { _key1, _key2, _key3 },
                        _ => throw new InvalidOperationException()
                    };
                }
            }
        }

        public override IEnumerable<string?> Values
        {
            get
            {
                return _values ??= CreateValues();

                string?[] CreateValues()
                {
                    return _count switch
                    {
                        1 => new[] { _value1 },
                        2 => new[] { _value1, _value2 },
                        3 => new[] { _value1, _value2, _value3 },
                        _ => throw new InvalidOperationException()
                    };
                }
            }
        }

        public override int Count => _count;

        public override bool ContainsKey(string key)
            => key == _key1 || (_count > 1 && key == _key2) || (_count > 2 && key == _key3);

        protected override KeyValuePair<string, string?> GetEntry(int index)
            => index switch
            {
                0 => new(_key1, _value1),
                1 => new(_key2, _value2),
                _ => new(_key3, _value3)
            };

        public override bool TryGetValue(string key, out string? value)
        {
            if (key == _key1)
            {
                value = _value1;
                return true;
            }

            if (_count > 1 && key == _key2)
            {
                value = _value2;
                return true;
            }

            if (_count > 2 && key == _key3)
            {
                value = _value3;
                return true;
            }

            value = null;
            return false;
        }

        public override bool Equals(MetadataCollection other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other is not OneToThreeItems otherCollection)
            {
                return false;
            }

            var count = _count;

            if (count != otherCollection._count)
            {
                return false;
            }

            switch (count)
            {
                case 1:
                    return _key1 == otherCollection._key1 && _value1 == otherCollection._value1;

                case 2:
                    if (_key1 == otherCollection._key1)
                    {
                        return _value1 == otherCollection._value1 &&
                               _key2 == otherCollection._key2 &&
                               _value2 == otherCollection._value2;
                    }

                    return _key1 == otherCollection._key2 &&
                           _value1 == otherCollection._value2 &&
                           _key2 == otherCollection._key1 &&
                           _value2 == otherCollection._value1;

                case 3:
                    return otherCollection.TryGetValue(_key1, out var otherValue1) &&
                           _value1 == otherValue1 &&
                           otherCollection.TryGetValue(_key2, out var otherValue2) &&
                           _value2 == otherValue2 &&
                           otherCollection.TryGetValue(_key3, out var otherValue3) &&
                           _value3 == otherValue3;
            }

            return false;
        }

        protected override int ComputeHashCode()
        {
            return _count switch
            {
                1 => ComputeHashCodeForOneItem(_key1, _value1),
                2 => ComputeHashCodeForTwoItems(_key1, _value1, _key2, _value2),
                _ => ComputeHashCodeForThreeItems(_key1, _value1, _key2, _value2, _key3, _value3)
            };

            static int ComputeHashCodeForOneItem(string key1, string? value1)
            {
                var hash = HashCodeCombiner.Start();

                hash.Add(key1, StringComparer.Ordinal);
                hash.Add(value1, StringComparer.Ordinal);

                return hash.CombinedHash;
            }

            static int ComputeHashCodeForTwoItems(string key1, string? value1, string key2, string? value2)
            {
                var hash = HashCodeCombiner.Start();

                if (string.CompareOrdinal(key1, key2) < 0)
                {
                    hash.Add(key1, StringComparer.Ordinal);
                    hash.Add(value1, StringComparer.Ordinal);
                    hash.Add(key2, StringComparer.Ordinal);
                    hash.Add(value2, StringComparer.Ordinal);
                }
                else
                {
                    hash.Add(key2, StringComparer.Ordinal);
                    hash.Add(value2, StringComparer.Ordinal);
                    hash.Add(key1, StringComparer.Ordinal);
                    hash.Add(value1, StringComparer.Ordinal);
                }

                return hash.CombinedHash;
            }

            static int ComputeHashCodeForThreeItems(string key1, string? value1, string key2, string? value2, string key3, string? value3)
            {
                var hash = HashCodeCombiner.Start();

                // Note: Because we've already eliminated duplicate strings, all of the
                // CompareOrdinal calls below should be either less than zero or greater
                // than zero.
                var key1LessThanKey2 = string.CompareOrdinal(key1, key2) < 0;
                var key1LessThanKey3 = string.CompareOrdinal(key1, key3) < 0;
                var key2LessThanKey3 = string.CompareOrdinal(key2, key3) < 0;

                var key1Added = false;
                var key2Added = false;

                // Add the first item
                if (key1LessThanKey2 && key1LessThanKey3)
                {
                    // If key1 is less than key2 and key3, it must go first.
                    hash.Add(key1, StringComparer.Ordinal);
                    hash.Add(value1, StringComparer.Ordinal);
                    key1Added = true;
                }
                else if (!key1LessThanKey2 && key2LessThanKey3)
                {
                    // Since key1 isn't first, add key2 if it is less than key1 and key3
                    hash.Add(key2, StringComparer.Ordinal);
                    hash.Add(value2, StringComparer.Ordinal);
                    key2Added = true;
                }
                else
                {
                    // Otherwise, key3 must go first.
                    hash.Add(key3, StringComparer.Ordinal);
                    hash.Add(value3, StringComparer.Ordinal);
                }

                // Add the second item
                if (!key1Added && (key1LessThanKey2 || key1LessThanKey3))
                {
                    // If we haven't added key1 and it is less than key2 or key3, it must be second.
                    hash.Add(key1, StringComparer.Ordinal);
                    hash.Add(value1, StringComparer.Ordinal);
                    key1Added = true;
                }
                else if (!key2Added && (!key1LessThanKey2 || key2LessThanKey3))
                {
                    // If we haven't added key2 and it is less than key1 or key3, it must be second.
                    hash.Add(key2, StringComparer.Ordinal);
                    hash.Add(value2, StringComparer.Ordinal);
                    key2Added = true;
                }
                else
                {
                    // Otherwise, key3 must be go first.
                    hash.Add(key3, StringComparer.Ordinal);
                    hash.Add(value3, StringComparer.Ordinal);
                }

                // Add the final item
                if (!key1Added)
                {
                    // If we haven't added key, it must go last.
                    hash.Add(key1, StringComparer.Ordinal);
                    hash.Add(value1, StringComparer.Ordinal);
                }
                else if (!key2Added)
                {
                    // If we haven't added key2, it must go last.
                    hash.Add(key2, StringComparer.Ordinal);
                    hash.Add(value2, StringComparer.Ordinal);
                }
                else
                {
                    // Otherwise, key3 must go last.
                    hash.Add(key3, StringComparer.Ordinal);
                    hash.Add(value3, StringComparer.Ordinal);
                }

                return hash.CombinedHash;
            }
        }
    }

    /// <summary>
    ///  This implementation represents a MetadataCollection with 4 or more items that are stored
    ///  in a pair of arrays. The keys are sorted so that lookup is O(log n).
    /// </summary>
    private sealed class FourOrMoreItems : MetadataCollection
    {
        private readonly string[] _keys;
        private readonly string?[] _values;

        private readonly int _count;

        public FourOrMoreItems(IReadOnlyList<KeyValuePair<string, string?>> pairs)
        {
            if (pairs is null)
            {
                throw new ArgumentNullException(nameof(pairs));
            }

            var count = pairs.Count;

            // Create a sorted array of keys.
            var keys = new string[count];

            for (var i = 0; i < count; i++)
            {
                keys[i] = pairs[i].Key;
            }

            Array.Sort(keys, StringComparer.Ordinal);

            // Ensure that there are no duplicate keys.
            for (var i = 1; i < count; i++)
            {
                if (keys[i] == keys[i - 1])
                {
                    throw new ArgumentException(
                        Resources.FormatAn_item_with_the_same_key_has_already_been_added_Key_0(keys[i]), nameof(pairs));
                }
            }

            // Create an array for the values.
            var values = new string?[count];

            // Loop through our pairs and add each value at the correct index.
            for (var i = 0; i < count; i++)
            {
                var (key, value) = pairs[i];
                var index = Array.BinarySearch(keys, key, StringComparer.Ordinal);

                // We know that every key is in the array, so we can assume that index is >= 0.
                Debug.Assert(index >= 0);

                values[index] = value;
            }

            _keys = keys;
            _values = values;
            _count = count;
        }

        public override string? this[string key]
        {
            get
            {
                var index = Array.BinarySearch(_keys, key, StringComparer.Ordinal);

                return index >= 0
                    ? _values[index]
                    : throw new KeyNotFoundException(Resources.FormatThe_given_key_0_was_not_present(key));
            }
        }

        public override IEnumerable<string> Keys => _keys;
        public override IEnumerable<string?> Values => _values;

        public override int Count => _count;

        public override bool ContainsKey(string key)
        {
            var index = Array.BinarySearch(_keys, key, StringComparer.Ordinal);

            return index >= 0;
        }

        protected override KeyValuePair<string, string?> GetEntry(int index)
            => new(_keys[index], _values[index]);

        public override bool TryGetValue(string key, out string? value)
        {
            var index = Array.BinarySearch(_keys, key, StringComparer.Ordinal);

            if (index >= 0)
            {
                value = _values[index];
                return true;
            }

            value = null;
            return false;
        }

        public override bool Equals(MetadataCollection other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other is not FourOrMoreItems otherCollection)
            {
                return false;
            }

            if (_count != otherCollection.Count)
            {
                return false;
            }

            var keys = _keys;
            var values = _values;
            var otherKeys = otherCollection._keys;
            var otherValues = otherCollection._values;
            var count = keys.Length;

            // The keys are pre-sorted by the constructor, so keys/values should be in
            // the same order event if they were added in a different order.

            for (var i = 0; i < count; i++)
            {
                if (keys[i] != otherKeys[i] ||
                    values[i] != otherValues[i])
                {
                    return false;
                }
            }

            return true;
        }

        protected override int ComputeHashCode()
        {
            var hash = HashCodeCombiner.Start();

            var keys = _keys;
            var values = _values;
            var count = keys.Length;

            // The keys are pre-sorted by the constructor, so keys/values should be in
            // the same order event if they were added in a different order.

            for (var i = 0; i < count; i++)
            {
                hash.Add(keys[i], StringComparer.Ordinal);
                hash.Add(values[i], StringComparer.Ordinal);
            }

            return hash.CombinedHash;
        }
    }
}
