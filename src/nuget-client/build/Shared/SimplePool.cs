// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;

namespace NuGet
{
    internal class SimplePool<T> where T : class
    {
        private readonly object _lockObj = new();
        private readonly Stack<T> _values = new();
        private readonly Func<T> _allocate;

        public SimplePool(Func<T> allocate)
        {
            _allocate = allocate;
        }

        public T Allocate()
        {
            lock (_lockObj)
            {
                if (_values.Count > 0)
                {
                    return _values.Pop();
                }
            }

            return _allocate();
        }

        public void Free(T value)
        {
            lock (_lockObj)
            {
                _values.Push(value);
            }
        }
    }
}
