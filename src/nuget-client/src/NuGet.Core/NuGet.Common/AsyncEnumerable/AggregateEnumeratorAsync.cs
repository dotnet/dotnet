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
    public class AggregateEnumeratorAsync<T> : IEnumeratorAsync<T>
    {

        private readonly HashSet<T> _seen;
        private readonly IComparer<T> _orderingComparer;
        private readonly List<IEnumeratorAsync<T>> _asyncEnumerators = new List<IEnumeratorAsync<T>>();
        private IEnumeratorAsync<T>? _currentEnumeratorAsync;
        private IEnumeratorAsync<T>? _lastAwaitedEnumeratorAsync;
        private bool firstPass = true;

        public AggregateEnumeratorAsync(IList<IEnumerableAsync<T>> asyncEnumerables, IComparer<T>? orderingComparer, IEqualityComparer<T>? equalityComparer)
        {
            if (asyncEnumerables == null)
            {
                throw new ArgumentNullException(nameof(asyncEnumerables));
            }

            foreach (IEnumerableAsync<T> asyncEnum in asyncEnumerables)
            {
                var enumerator = asyncEnum.GetEnumeratorAsync();
                _asyncEnumerators.Add(enumerator);
            }
            _orderingComparer = orderingComparer == null ? Comparer<T>.Default : orderingComparer;
            _seen = equalityComparer == null ? new HashSet<T>() : new HashSet<T>(equalityComparer);
        }

        public T Current
        {
            get
            {
                if (_currentEnumeratorAsync == null)
                {
                    // From the interface definition, if MoveNextAsync hasn't yet been called, this property's behavior is undefined.
                    return default(T)!;
                }
                return _currentEnumeratorAsync.Current;
            }
        }

        public async Task<bool> MoveNextAsync()
        {
            while (_asyncEnumerators.Count > 0)
            {
                T? currentValue = default(T);
                var hasValue = false;
                List<IEnumeratorAsync<T>>? completedEnums = null;

                if (firstPass)
                {
                    foreach (IEnumeratorAsync<T> enumerator in _asyncEnumerators)
                    {
                        if (!await enumerator.MoveNextAsync())
                        {
                            if (completedEnums == null)
                            {
                                completedEnums = new List<IEnumeratorAsync<T>>();
                            }
                            completedEnums.Add(enumerator);

                        }
                        else
                        {
                            if (!hasValue || _orderingComparer.Compare(currentValue!, enumerator.Current) > 0)
                            {
                                hasValue = true;
                                currentValue = enumerator.Current;
                                _currentEnumeratorAsync = enumerator;
                            }
                        }
                    }
                    firstPass = false;
                }
                else
                {
                    foreach (IEnumeratorAsync<T> enumerator in _asyncEnumerators)
                    {
                        bool hasNext = true;
                        if (enumerator == _lastAwaitedEnumeratorAsync)
                        {
                            hasNext = await enumerator.MoveNextAsync();
                            if (!hasNext)
                            {
                                if (completedEnums == null)
                                {
                                    completedEnums = new List<IEnumeratorAsync<T>>();
                                }
                                completedEnums.Add(enumerator);
                            }
                        }

                        if (hasNext && (!hasValue || _orderingComparer.Compare(currentValue!, enumerator.Current) > 0))
                        {
                            hasValue = true;
                            currentValue = enumerator.Current;
                            _currentEnumeratorAsync = enumerator;
                        }
                    }
                }
                _lastAwaitedEnumeratorAsync = _currentEnumeratorAsync;
                //Remove all the enums that don't have a next value
                if (completedEnums != null)
                {
                    _asyncEnumerators.RemoveAll(obj => completedEnums.Contains(obj));
                }

                if (hasValue)
                {
                    if (_seen.Add(currentValue!))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
