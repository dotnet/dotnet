// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor.PooledObjects;

internal ref partial struct PooledArrayBuilder<T>
{
    public struct Enumerator : IEnumerator<T>
    {
        private readonly ImmutableArray<T>.Builder? _builder;
        private int _index;
        private T? _current;

        public Enumerator(ImmutableArray<T>.Builder builder)
        {
            _builder = builder;
            _index = 0;
            _current = default;
        }

        public T Current => _current!;

        object? IEnumerator.Current => Current;

        public readonly void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (_builder is { } builder && _index < builder.Count)
            {
                _current = builder[_index];
                _index++;
                return true;
            }

            return false;
        }

        void IEnumerator.Reset()
        {
            _index = 0;
            _current = default;
        }
    }
}
