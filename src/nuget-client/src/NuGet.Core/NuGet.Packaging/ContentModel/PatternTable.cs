// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace NuGet.ContentModel
{
    /// <summary>
    /// Replacement token table organized by property.
    /// </summary>
    public class PatternTable
    {
        private readonly Dictionary<string, Dictionary<ReadOnlyMemory<char>, object>> _table
            = new Dictionary<string, Dictionary<ReadOnlyMemory<char>, object>>(StringComparer.Ordinal);

        public PatternTable()
            : this(Enumerable.Empty<PatternTableEntry>())
        {
        }

        public PatternTable(IEnumerable<PatternTableEntry> entries)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            foreach (var entry in entries)
            {
                Dictionary<ReadOnlyMemory<char>, object> byProp;
                if (!_table.TryGetValue(entry.PropertyName, out byProp))
                {
                    byProp = new Dictionary<ReadOnlyMemory<char>, object>(ReadOnlyMemoryCharComparerOrdinal.Instance);
                    _table.Add(entry.PropertyName, byProp);
                }

                byProp.Add(entry.Name.AsMemory(), entry.Value);
            }
        }

        /// <summary>
        /// Lookup a token and get the replacement if it exists.
        /// </summary>
        /// <param name="propertyName">Property moniker</param>
        /// <param name="name">Token name</param>
        /// <param name="value">Replacement value</param>
        internal bool TryLookup(string propertyName, ReadOnlyMemory<char> name, out object value)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            Debug.Assert(MemoryMarshal.TryGetString(name, out _, out _, out _));

            Dictionary<ReadOnlyMemory<char>, object> byProp;
            if (_table.TryGetValue(propertyName, out byProp))
            {
                return byProp.TryGetValue(name, out value);
            }

            value = null;
            return false;
        }
    }
}
