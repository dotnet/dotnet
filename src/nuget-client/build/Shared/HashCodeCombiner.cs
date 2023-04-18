// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace NuGet.Shared
{
    /// <summary>
    /// Hash code creator, based on the original NuGet hash code combiner/ASP hash code combiner implementations
    /// </summary>
    internal struct HashCodeCombiner
    {
        // seed from String.GetHashCode()
        private const long Seed = 0x1505L;

        private bool _initialized;
        private long _combinedHash;

        internal int CombinedHash
        {
            get { return _combinedHash.GetHashCode(); }
        }

        private void AddHashCode(int i)
        {
            _combinedHash = ((_combinedHash << 5) + _combinedHash) ^ i;
        }

        internal void AddObject(int i)
        {
            CheckInitialized();
            AddHashCode(i);
        }

        internal void AddObject(bool b)
        {
            CheckInitialized();
            AddHashCode(b ? 1 : 0);
        }

        internal void AddObject<TValue>(TValue? o, IEqualityComparer<TValue> comparer)
            where TValue : class
        {
            CheckInitialized();
            if (o != null)
            {
                AddHashCode(comparer.GetHashCode(o));
            }
        }

        internal void AddObject<T>(T? o)
            where T : class
        {
            CheckInitialized();
            if (o != null)
            {
                AddHashCode(o.GetHashCode());
            }
        }

        // Optimization: for value types, we can avoid boxing "o" by skipping the null check
        internal void AddStruct<T>(T? o)
            where T : struct
        {
            CheckInitialized();

            if (o.HasValue)
            {
                AddHashCode(o.GetHashCode());
            }
        }

        // Optimization: for value types, we can avoid boxing "o" by skipping the null check
        internal void AddStruct<T>(T o)
            where T : struct
        {
            CheckInitialized();

            AddHashCode(o.GetHashCode());
        }

        internal void AddStringIgnoreCase(string s)
        {
            CheckInitialized();
            if (s != null)
            {
                AddHashCode(StringComparer.OrdinalIgnoreCase.GetHashCode(s));
            }
        }

        internal void AddSequence<T>(IEnumerable<T> sequence) where T : notnull
        {
            if (sequence != null)
            {
                CheckInitialized();
                foreach (var item in sequence)
                {
                    AddHashCode(item.GetHashCode());
                }
            }
        }

        internal void AddSequence<T>(T[] array) where T : notnull
        {
            if (array != null)
            {
                CheckInitialized();
                foreach (var item in array)
                {
                    AddHashCode(item.GetHashCode());
                }
            }
        }

        internal void AddSequence<T>(IList<T> list) where T : notnull
        {
            if (list != null)
            {
                CheckInitialized();
                var count = list.Count;
                for (var i = 0; i < count; i++)
                {
                    AddHashCode(list[i].GetHashCode());
                }
            }
        }

        internal void AddSequence<T>(IReadOnlyList<T> list) where T : notnull
        {
            if (list != null)
            {
                CheckInitialized();
                var count = list.Count;
                for (var i = 0; i < count; i++)
                {
                    AddHashCode(list[i].GetHashCode());
                }
            }
        }

        internal void AddDictionary<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
            where TKey : notnull
            where TValue : notnull
        {
            if (dictionary != null)
            {
                CheckInitialized();
                foreach (var pair in dictionary.OrderBy(x => x.Key))
                {
                    AddHashCode(pair.Key.GetHashCode());
                    AddHashCode(pair.Value.GetHashCode());
                }
            }
        }

        /// <summary>
        /// Create a unique hash code for the given set of items
        /// </summary>
        internal static int GetHashCode<T1, T2>(T1 o1, T2 o2)
            where T1 : notnull
            where T2 : notnull
        {
            var combiner = new HashCodeCombiner();
            combiner.CheckInitialized();

            combiner.AddHashCode(o1.GetHashCode());
            combiner.AddHashCode(o2.GetHashCode());

            return combiner.CombinedHash;
        }

        /// <summary>
        /// Create a unique hash code for the given set of items
        /// </summary>
        internal static int GetHashCode<T1, T2, T3>(T1 o1, T2 o2, T3 o3)
            where T1 : notnull
            where T2 : notnull
            where T3 : notnull
        {
            var combiner = new HashCodeCombiner();
            combiner.CheckInitialized();

            combiner.AddHashCode(o1.GetHashCode());
            combiner.AddHashCode(o2.GetHashCode());
            combiner.AddHashCode(o3.GetHashCode());

            return combiner.CombinedHash;
        }

        private void CheckInitialized()
        {
            if (!_initialized)
            {
                _combinedHash = Seed;
                _initialized = true;
            }
        }
    }
}
