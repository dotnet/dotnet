// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Razor.Language;

public abstract partial class MetadataCollection
{
    public struct Enumerator : IEnumerator<KeyValuePair<string, string?>>
    {
        private readonly MetadataCollection _collection;
        private int _index;
        private KeyValuePair<string, string?> _current;

        internal Enumerator(MetadataCollection collection)
        {
            _collection = collection;
            _index = 0;
            _current = default;
        }

        public readonly KeyValuePair<string, string?> Current => _current;

        readonly object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            var collection = _collection;
            if (_index < collection.Count)
            {
                _current = _collection.GetEntry(_index);
                _index++;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            _index = 0;
            _current = default;
        }

        readonly void IDisposable.Dispose()
        {
        }
    }
}
