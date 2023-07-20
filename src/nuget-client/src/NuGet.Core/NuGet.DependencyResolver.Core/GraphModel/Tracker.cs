// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace NuGet.DependencyResolver
{
    public class Tracker<TItem>
    {
        private readonly Dictionary<string, Entry> _entryByLibraryName = new(StringComparer.OrdinalIgnoreCase);

        public void Track(GraphItem<TItem> item)
        {
            GetOrAddEntry(item).AddItem(item);
        }

        public bool IsDisputed(GraphItem<TItem> item)
        {
            return TryGetEntry(item)?.HasMultipleItems ?? false;
        }

        public bool IsAmbiguous(GraphItem<TItem> item)
        {
            return TryGetEntry(item)?.Ambiguous ?? false;
        }

        public void MarkAmbiguous(GraphItem<TItem> item)
        {
            GetOrAddEntry(item).Ambiguous = true;
        }

        /// <remarks>
        /// Note, this method returns <see langword="true"/> for items that were never tracked.
        /// </remarks>
        public bool IsBestVersion(GraphItem<TItem> item)
        {
            var entry = TryGetEntry(item);

            if (entry is not null)
            {
                var version = item.Key.Version;

                foreach (var known in entry.Items)
                {
                    if (version < known.Key.Version)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public IEnumerable<GraphItem<TItem>> GetDisputes(GraphItem<TItem> item)
        {
            return TryGetEntry(item)?.Items ?? Enumerable.Empty<GraphItem<TItem>>();
        }

        internal void Clear()
        {
            _entryByLibraryName.Clear();
        }

        private Entry? TryGetEntry(GraphItem<TItem> item)
        {
            _entryByLibraryName.TryGetValue(item.Key.Name, out Entry? entry);
            return entry;
        }

        private Entry GetOrAddEntry(GraphItem<TItem> item)
        {
            if (!_entryByLibraryName.TryGetValue(item.Key.Name, out Entry? entry))
            {
                entry = new Entry();
                _entryByLibraryName[item.Key.Name] = entry;
            }

            return entry;
        }

        private sealed class Entry
        {
            /// <summary>
            /// This field can have one of three values:
            ///
            /// - null: when no graph items exist in this entry
            /// - a graph item: when only a single graph item exists in this entry
            /// - a hash set of graph items: when multiple graph items exist in this entry
            ///
            /// This packing exists in order to reduce the size of this object in
            /// memory. For large graphs, this can amount to a significant saving in memory,
            /// which helps overall performance.
            /// </summary>
            private object? _storage;

            public bool Ambiguous { get; set; }

            public void AddItem(GraphItem<TItem> item)
            {
                if (_storage is null)
                {
                    _storage = item;
                }
                else if (_storage is GraphItem<TItem> existingItem)
                {
                    if (!existingItem.Equals(item))
                    {
#if NETSTANDARD2_0
                        _storage = new HashSet<GraphItem<TItem>>() { existingItem, item };
#else
                        _storage = new HashSet<GraphItem<TItem>>(capacity: 3) { existingItem, item };
#endif
                    }
                }
                else
                {
                    ((HashSet<GraphItem<TItem>>)_storage).Add(item);
                }
            }

            public bool HasMultipleItems => _storage is HashSet<GraphItem<TItem>>;

            public IEnumerable<GraphItem<TItem>> Items
            {
                get
                {
                    if (_storage is null)
                        return Enumerable.Empty<GraphItem<TItem>>();
                    if (_storage is GraphItem<TItem> item)
                        return new[] { item };
                    return (HashSet<GraphItem<TItem>>)_storage;
                }
            }
        }
    }
}
