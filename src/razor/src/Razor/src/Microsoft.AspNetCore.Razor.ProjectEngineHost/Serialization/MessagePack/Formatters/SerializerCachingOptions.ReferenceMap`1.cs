// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.Extensions.ObjectPool;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal partial class SerializerCachingOptions
{
    public struct ReferenceMap<T> : IDisposable
        where T : notnull
    {
        private readonly ObjectPool<Dictionary<T, int>> _dictionaryPool;
        private List<T> _values;
        private Dictionary<T, int> _valueToIdMap;

        public ReferenceMap(ObjectPool<Dictionary<T, int>> dictionaryPool)
        {
            _dictionaryPool = dictionaryPool;
            _values = ListPool<T>.Default.Get();
            _valueToIdMap = _dictionaryPool.Get();
        }

        public void Dispose()
        {
            if (_values is { } values)
            {
                ListPool<T>.Default.Return(values);
                _values = null!;
            }

            if (_valueToIdMap is { } valueToIdMap)
            {
                _dictionaryPool.Return(valueToIdMap);
                _valueToIdMap = null!;
            }
        }

        public T GetValue(int referenceId)
            => _values[referenceId];

        public readonly bool TryGetReferenceId(T value, out int referenceId)
            => _valueToIdMap.TryGetValue(value, out referenceId);

        public void Add(T value)
        {
            var id = _values.Count;
            _values.Add(value);
            _valueToIdMap.Add(value, id);
        }
    }
}
