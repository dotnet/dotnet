// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.Language;

/// <summary>
///  Generally, there are only a handful of metadata items stored on the various tag helper
///  objects. Often, it's just one or two items. To improve memory usage, MetadataCollection provides
///  multiple implementations to avoid creating a hash table.
/// </summary>
internal abstract class MetadataCollection : IReadOnlyDictionary<string, string>, IEquatable<MetadataCollection>
{
    public static readonly MetadataCollection Empty = NoItems.Instance;

    private int? _hashCode;

    protected MetadataCollection()
    {
    }

    public abstract string this[string key] { get; }

    public abstract IEnumerable<string> Keys { get; }
    public abstract IEnumerable<string> Values { get; }
    public abstract int Count { get; }

    public abstract bool ContainsKey(string key);
    public abstract IEnumerator<KeyValuePair<string, string>> GetEnumerator();
    public abstract bool TryGetValue(string key, out string value);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public sealed override bool Equals(object obj)
        => obj is MetadataCollection other && Equals(other);

    public abstract bool Equals(MetadataCollection other);

    protected abstract int ComputeHashCode();

    public sealed override int GetHashCode() => _hashCode ??= ComputeHashCode();

    public static MetadataCollection Create(KeyValuePair<string, string> pair)
        => new OneToThreeItems(pair.Key, pair.Value);

    public static MetadataCollection Create(KeyValuePair<string, string> pair1, KeyValuePair<string, string> pair2)
        => new OneToThreeItems(pair1.Key, pair1.Value, pair2.Key, pair2.Value);

    public static MetadataCollection Create(KeyValuePair<string, string> pair1, KeyValuePair<string, string> pair2, KeyValuePair<string, string> pair3)
        => new OneToThreeItems(pair1.Key, pair1.Value, pair2.Key, pair2.Value, pair3.Key, pair3.Value);

    public static MetadataCollection Create(params KeyValuePair<string, string>[] pairs)
        => pairs switch
        {
            [] => Empty,
            [var pair] => new OneToThreeItems(pair.Key, pair.Value),
            [var pair1, var pair2] => new OneToThreeItems(pair1.Key, pair1.Value, pair2.Key, pair2.Value),
            [var pair1, var pair2, var pair3] => new OneToThreeItems(pair1.Key, pair1.Value, pair2.Key, pair2.Value, pair3.Key, pair3.Value),
            _ => new FourOrMoreItems(pairs),
        };

    public static MetadataCollection Create(IReadOnlyDictionary<string, string> map)
    {
        switch (map.Count)
        {
            case 0:
                return Empty;

            case 1:
                {
                    using var enumerable = map.GetEnumerator();

                    if (!enumerable.MoveNext())
                    {
                        throw new InvalidOperationException();
                    }

                    var pair = enumerable.Current;

                    return new OneToThreeItems(pair.Key, pair.Value);
                }

            case 2:
                {
                    using var enumerable = map.GetEnumerator();

                    if (!enumerable.MoveNext())
                    {
                        throw new InvalidOperationException();
                    }

                    var pair1 = enumerable.Current;


                    if (!enumerable.MoveNext())
                    {
                        throw new InvalidOperationException();
                    }

                    var pair2 = enumerable.Current;

                    return new OneToThreeItems(pair1.Key, pair1.Value, pair2.Key, pair2.Value);
                }

            case 3:
                {
                    using var enumerable = map.GetEnumerator();

                    if (!enumerable.MoveNext())
                    {
                        throw new InvalidOperationException();
                    }

                    var pair1 = enumerable.Current;

                    if (!enumerable.MoveNext())
                    {
                        throw new InvalidOperationException();
                    }

                    var pair2 = enumerable.Current;

                    if (!enumerable.MoveNext())
                    {
                        throw new InvalidOperationException();
                    }

                    var pair3 = enumerable.Current;

                    return new OneToThreeItems(pair1.Key, pair1.Value, pair2.Key, pair2.Value, pair3.Key, pair3.Value);
                }

            default:
                return new FourOrMoreItems(map);
        }

    }

    public static MetadataCollection CreateOrEmpty(IReadOnlyDictionary<string, string>? map)
        => map is not null ? Create(map) : Empty;

    private class NoItems : MetadataCollection
    {
        public static readonly NoItems Instance = new();

        private static readonly IEnumerable<KeyValuePair<string, string>> s_pairs = Array.Empty<KeyValuePair<string, string>>();

        private NoItems()
        {
        }

        public override string this[string key] => throw new KeyNotFoundException(Resources.FormatThe_given_key_0_was_not_present(key));

        public override IEnumerable<string> Keys => Array.Empty<string>();
        public override IEnumerable<string> Values => Array.Empty<string>();
        public override int Count => 0;

        public override bool ContainsKey(string key) => false;

        public override IEnumerator<KeyValuePair<string, string>> GetEnumerator()
            => s_pairs.GetEnumerator();

        public override bool TryGetValue(string key, out string value)
        {
            value = null!;
            return false;
        }

        protected override int ComputeHashCode() => 0;
        public override bool Equals(MetadataCollection other) => ReferenceEquals(this, other);
    }

    private class OneToThreeItems : MetadataCollection
    {
        private readonly string _key1;
        private readonly string _value1;

        [AllowNull]
        private readonly string _key2;
        [AllowNull]
        private readonly string _value2;

        [AllowNull]
        private readonly string _key3;
        [AllowNull]
        private readonly string _value3;

        private readonly int _count;

        private string[]? _keys;
        private string[]? _values;
        private IEnumerable<KeyValuePair<string, string>>? _pairs;

        public OneToThreeItems(string key1, string value1, string key2, string value2, string key3, string value3)
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

        public OneToThreeItems(string key1, string value1, string key2, string value2)
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

        public OneToThreeItems(string key, string value)
        {
            _key1 = key ?? throw new ArgumentNullException(nameof(key));
            _value1 = value;

            _count = 1;
        }

        public override string this[string key]
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

        public override IEnumerable<string> Values
        {
            get
            {
                return _values ??= CreateValues();

                string[] CreateValues()
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

        public override IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            _pairs ??= CreatePairs();

            return _pairs.GetEnumerator();

            KeyValuePair<string, string>[] CreatePairs()
            {
                return _count switch
                {
                    1 => new[] { new KeyValuePair<string, string>(_key1, _value1) },
                    2 => new[] { new KeyValuePair<string, string>(_key1, _value1), new KeyValuePair<string, string>(_key2, _value2) },
                    3 => new[] { new KeyValuePair<string, string>(_key1, _value1), new KeyValuePair<string, string>(_key2, _value2), new KeyValuePair<string, string>(_key3, _value3) },
                    _ => throw new InvalidOperationException()
                };
            }
        }

        public override bool TryGetValue(string key, out string value)
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

            value = null!;
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

            if (_count != otherCollection._count)
            {
                return false;
            }

            if (_count == 1)
            {
                return _key1 == otherCollection._key1 && _value1 == otherCollection._value1;
            }

            if (_count == 2)
            {
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
            }

            if (_count == 3)
            {
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
                1 => ComputeHashCodeForOneItem(),
                2 => ComputeHashCodeForTwoItems(),
                _ => ComputeHashCodeForThreeItems()
            };

            int ComputeHashCodeForOneItem()
            {
                var hash = HashCodeCombiner.Start();

                hash.Add(_key1, StringComparer.Ordinal);
                hash.Add(_value1, StringComparer.Ordinal);

                return hash.CombinedHash;
            }

            int ComputeHashCodeForTwoItems()
            {
                var hash = HashCodeCombiner.Start();

                if (string.CompareOrdinal(_key1, _key2) < 0)
                {
                    hash.Add(_key1, StringComparer.Ordinal);
                    hash.Add(_value1, StringComparer.Ordinal);
                    hash.Add(_key2, StringComparer.Ordinal);
                    hash.Add(_value2, StringComparer.Ordinal);
                }
                else
                {
                    hash.Add(_key2, StringComparer.Ordinal);
                    hash.Add(_value2, StringComparer.Ordinal);
                    hash.Add(_key1, StringComparer.Ordinal);
                    hash.Add(_value1, StringComparer.Ordinal);
                }

                return hash.CombinedHash;
            }

            int ComputeHashCodeForThreeItems()
            {
                var hash = HashCodeCombiner.Start();

                // Note: Because we've already eliminated duplicate strings, all of the
                // CompareOrdinal calls below should be either less than zero or greater
                // than zero.
                var key1LessThanKey2 = string.CompareOrdinal(_key1, _key2) < 0;
                var key1LessThanKey3 = string.CompareOrdinal(_key1, _key3) < 0;
                var key2LessThanKey3 = string.CompareOrdinal(_key2, _key3) < 0;

                var key1Added = false;
                var key2Added = false;

                // Add the first item
                if (key1LessThanKey2 && key1LessThanKey3)
                {
                    // If key1 is less than key2 and key3, it must go first.
                    hash.Add(_key1, StringComparer.Ordinal);
                    hash.Add(_value1, StringComparer.Ordinal);
                    key1Added = true;
                }
                else if (!key1LessThanKey2 && key2LessThanKey3)
                {
                    // Since key1 isn't first, add key2 if it is less than key1 and key3
                    hash.Add(_key2, StringComparer.Ordinal);
                    hash.Add(_value2, StringComparer.Ordinal);
                    key2Added = true;
                }
                else
                {
                    // Otherwise, key3 must go first.
                    hash.Add(_key3, StringComparer.Ordinal);
                    hash.Add(_value3, StringComparer.Ordinal);
                }

                // Add the second item
                if (!key1Added && (key1LessThanKey2 || key1LessThanKey3))
                {
                    // If we haven't added key1 and it is less than key2 or key3, it must be second.
                    hash.Add(_key1, StringComparer.Ordinal);
                    hash.Add(_value1, StringComparer.Ordinal);
                    key1Added = true;
                }
                else if (!key2Added && (!key1LessThanKey2 || key2LessThanKey3))
                {
                    // If we haven't added key2 and it is less than key1 or key3, it must be second.
                    hash.Add(_key2, StringComparer.Ordinal);
                    hash.Add(_value2, StringComparer.Ordinal);
                    key2Added = true;
                }
                else
                {
                    // Otherwise, key3 must be go first.
                    hash.Add(_key3, StringComparer.Ordinal);
                    hash.Add(_value3, StringComparer.Ordinal);
                }

                // Add the final item
                if (!key1Added)
                {
                    // If we haven't added key, it must go last.
                    hash.Add(_key1, StringComparer.Ordinal);
                    hash.Add(_value1, StringComparer.Ordinal);
                }
                else if (!key2Added)
                {
                    // If we haven't added key2, it must go last.
                    hash.Add(_key2, StringComparer.Ordinal);
                    hash.Add(_value2, StringComparer.Ordinal);
                }
                else
                {
                    // Otherwise, key3 must go last.
                    hash.Add(_key3, StringComparer.Ordinal);
                    hash.Add(_value3, StringComparer.Ordinal);
                }

                return hash.CombinedHash;
            }
        }
    }

    private class FourOrMoreItems : MetadataCollection
    {
        private readonly string[] _keys;
        private readonly string[] _values;

        private readonly int _count;

        private IEnumerable<KeyValuePair<string, string>>? _pairs;

        public FourOrMoreItems(IReadOnlyDictionary<string, string> map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            using var _1 = ListPool<string>.GetPooledObject(out var keys);
            using var _2 = ListPool<string>.GetPooledObject(out var values);

            var count = map.Count;
            keys.SetCapacityIfLarger(count);
            values.SetCapacityIfLarger(count);

            foreach (var (key, value) in map)
            {
                // Because the keys are strings that are already in a dictionary, we are
                // guaranteed that there won't ever be a match. So, we can assume that
                // the result of BinarySearch will always be negative and can immediately
                // convert from the bitwise complement.
                var index = ~keys.BinarySearch(key);

                keys.Insert(index, key);
                values.Insert(index, value);
            }

            _keys = keys.ToArrayOrEmpty();
            _values = values.ToArrayOrEmpty();
            _count = count;
        }

        public FourOrMoreItems(KeyValuePair<string, string>[] pairs)
        {
            if (pairs is null)
            {
                throw new ArgumentNullException(nameof(pairs));
            }

            using var _1 = ListPool<string>.GetPooledObject(out var keys);
            using var _2 = ListPool<string>.GetPooledObject(out var values);

            var count = pairs.Length;
            keys.SetCapacityIfLarger(count);
            values.SetCapacityIfLarger(count);

            foreach (var (key, value) in pairs)
            {
                var index = keys.BinarySearch(key);

                if (index >= 0)
                {
                    throw new ArgumentException(
                        Resources.FormatAn_item_with_the_same_key_has_already_been_added_Key_0(key), nameof(pairs));
                }

                index = ~index;

                keys.Insert(index, key);
                values.Insert(index, value);
            }

            _keys = keys.ToArrayOrEmpty();
            _values = values.ToArrayOrEmpty();
            _count = count;
        }

        public override string this[string key]
        {
            get
            {
                var index = Array.BinarySearch(_keys, key);

                return index >= 0
                    ? _values[index]
                    : throw new KeyNotFoundException(Resources.FormatThe_given_key_0_was_not_present(key));
            }
        }

        public override IEnumerable<string> Keys => _keys;
        public override IEnumerable<string> Values => _values;

        public override int Count => _count;

        public override bool ContainsKey(string key)
        {
            var index = Array.BinarySearch(_keys, key);

            return index >= 0;
        }

        public override IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            _pairs ??= CreatePairs(_keys, _values);

            return _pairs.GetEnumerator();

            static IEnumerable<KeyValuePair<string, string>> CreatePairs(string[] keys, string[] values)
            {
                for (var i = 0; i < keys.Length; i++)
                {
                    yield return new KeyValuePair<string, string>(keys[i], values[i]);
                }
            }
        }

        public override bool TryGetValue(string key, out string value)
        {
            var index = Array.BinarySearch(_keys, key);

            if (index >= 0)
            {
                value = _values[index];
                return true;
            }

            value = null!;
            return false;
        }

        public override bool Equals(MetadataCollection other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other is not FourOrMoreItems)
            {
                return false;
            }

            if (_count != other.Count)
            {
                return false;
            }

            var keys = _keys;
            var values = _values;
            var count = keys.Length;

            for (var i = 0; i < count; i++)
            {
                if (!other.TryGetValue(keys[i], out var otherValue) ||
                    values[i] != otherValue)
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

            using var _ = ListPool<KeyValuePair<string, string>>.GetPooledObject(out var list);

            list.SetCapacityIfLarger(count);

            for (var i = 0; i < count; i++)
            {
                list.Add(new KeyValuePair<string, string>(keys[i], values[i]));
            }

            list.Sort((kvp1, kvp2) => string.CompareOrdinal(kvp1.Key, kvp2.Key));

            foreach (var (key, value) in list)
            {
                hash.Add(key, StringComparer.Ordinal);
                hash.Add(value, StringComparer.Ordinal);
            }

            return hash.CombinedHash;
        }
    }
}
