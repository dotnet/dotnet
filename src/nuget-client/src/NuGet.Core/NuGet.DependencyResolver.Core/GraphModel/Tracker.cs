// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
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

                foreach (var known in entry)
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
            var entry = TryGetEntry(item);
            if (entry is null)
            {
                return Enumerable.Empty<GraphItem<TItem>>();
            }

            return entry;
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

        private sealed class Entry : IEnumerable<GraphItem<TItem>>
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

            public Enumerator GetEnumerator()
            {
                return new Enumerator(this);
            }

            IEnumerator<GraphItem<TItem>> IEnumerable<GraphItem<TItem>>.GetEnumerator()
            {
                return new Enumerator(this);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new Enumerator(this);
            }

            public bool HasMultipleItems => _storage is HashSet<GraphItem<TItem>>;

            public struct Enumerator : IEnumerator<GraphItem<TItem>>, IDisposable, IEnumerator
            {
                private enum Type : byte
                {
                    Empty,
                    SingleItem,
                    MultipleItems,
                }

                private readonly Type _type;
                private readonly Entry _entry;
                private GraphItem<TItem> _current;
                private bool _done;
                private HashSet<GraphItem<TItem>>.Enumerator _setEnumerator;

                public Enumerator(Entry entry)
                {
                    _entry = entry;
                    _current = default!;
                    _done = false;

                    if (_entry._storage is null)
                    {
                        _type = Type.Empty;
                    }
                    else if (_entry._storage is GraphItem<TItem>)
                    {
                        _type = Type.SingleItem;
                    }
                    else
                    {
                        _type = Type.MultipleItems;
                        _setEnumerator = ((HashSet<GraphItem<TItem>>)_entry._storage).GetEnumerator();
                    }
                }

                public readonly GraphItem<TItem> Current => _current;

                readonly object IEnumerator.Current => Current;

                public bool MoveNext()
                {
                    if (_done || _type == Type.Empty)
                    {
                        return false;
                    }
                    else if (_type == Type.SingleItem)
                    {
                        _done = true;
                        _current = (GraphItem<TItem>)_entry._storage!;

                        return true;
                    }
                    else
                    {
                        bool result = _setEnumerator.MoveNext();
                        _current = _setEnumerator.Current;

                        return result;
                    }
                }

                public void Dispose()
                {
                    if (_type == Type.MultipleItems)
                    {
                        _setEnumerator.Dispose();
                    }
                }

                void IEnumerator.Reset()
                {
                    if (_type == Type.SingleItem)
                    {
                        _done = false;
                    }
                    else
                    {
                        ((IEnumerator)_setEnumerator).Reset();
                    }

                    _current = default!;
                }
            }
        }
    }
}
