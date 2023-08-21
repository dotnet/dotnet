// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuGet.Common
{
    /// <summary>
    /// Aggregates from a list of already ordered enumerables
    /// The ordered result will contain only unique values
    /// If comparer/EqualityComparer are not provided the default ones for that type will be used.
    /// If the provided enumerables are not sorted already, the behavior is undefined
    /// </summary>

    public class AggregateEnumerableAsync<T> : IEnumerableAsync<T>
    {

        private readonly IList<IEnumerableAsync<T>> _asyncEnumerables;
        private readonly IComparer<T>? _comparer;
        private readonly IEqualityComparer<T>? _equalityComparer;

        public AggregateEnumerableAsync(IList<IEnumerableAsync<T>> asyncEnumerables, IComparer<T>? comparer, IEqualityComparer<T>? equalityComparer)
        {
            _asyncEnumerables = asyncEnumerables;
            _comparer = comparer;
            _equalityComparer = equalityComparer;
        }

        public IEnumeratorAsync<T> GetEnumeratorAsync()
        {
            return new AggregateEnumeratorAsync<T>(_asyncEnumerables, _comparer, _equalityComparer);
        }
    }
}
