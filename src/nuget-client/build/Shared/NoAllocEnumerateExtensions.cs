// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;

namespace NuGet;

internal static class NoAllocEnumerateExtensions
{
    #region IList

    /// <summary>
    /// Avoids allocating an enumerator when enumerating an <see cref="IList{T}"/> where the concrete type
    /// has a well known struct enumerator, such as for <see cref="List{T}"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Several collection types (e.g. <see cref="List{T}"/>) provide a struct-based enumerator type
    /// (e.g. <see cref="List{T}.Enumerator"/>) which the compiler can use in <see langword="foreach" /> statements.
    /// When using a struct-based enumerator, no heap allocation occurs during such enumeration.
    /// This is in contrast to the interface-based enumerator <see cref="IEnumerator{T}"/> which will
    /// always be allocated on the heap.
    /// </para>
    /// <para>
    /// This method returns a custom struct enumerator that will avoid any heap allocation if <paramref name="list"/>
    /// (which is declared via interface <see cref="IList{T}"/>) is actually of known concrete type that
    /// provides its own struct enumerator. If so, it delegates to that type's enumerator without any boxing
    /// or other heap allocation.
    /// </para>
    /// <para>
    /// If <paramref name="list"/> is not of a known concrete type, the returned enumerator falls back to the
    /// interface-based enumerator, which will be allocated on the heap. Benchmarking shows the overhead in
    /// such cases is low enough to be within the measurement error, meaning this is an inexpensive optimization
    /// that won't regress behavior and with low downside for cases where it cannot apply an optimization.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[IList<string> list = ...;
    ///
    /// foreach (string item in list.NoAllocEnumerate())
    /// {
    ///     // ...
    /// }]]>
    /// </code>
    /// </example>
    public static OptimisticallyNonAllocatingListEnumerable<T> NoAllocEnumerate<T>(this IList<T> list)
        where T : notnull
    {
#pragma warning disable CS0618 // Type or member is obsolete
        return new(list);
#pragma warning restore CS0618 // Type or member is obsolete
    }

    /// <summary>
    /// Provides a struct-based enumerator for use with <see cref="IList{T}"/>.
    /// Do not use this type directly. Use <see cref="NoAllocEnumerate{T}(IList{T})"/> instead.
    /// </summary>
    public readonly ref struct OptimisticallyNonAllocatingListEnumerable<T>
        where T : notnull
    {
        private readonly IList<T> _list;

        [Obsolete("Do not construct directly. Use internal static method NoAllocEnumerateExtensions.NoAllocEnumerate instead.")]
        internal OptimisticallyNonAllocatingListEnumerable(IList<T> list) => _list = list;

        public Enumerator GetEnumerator() => new(_list);

        /// <summary>
        /// A struct-based enumerator for use with <see cref="IList{T}"/>.
        /// Do not use this type directly. Use <see cref="NoAllocEnumerate{T}(IList{T})"/> instead.
        /// </summary>
        public struct Enumerator : IDisposable
        {
            private readonly int _enumeratorType;
            private readonly IEnumerator<T>? _fallbackEnumerator;
            private List<T>.Enumerator _concreteEnumerator;

            internal Enumerator(IList<T> list)
            {
                _concreteEnumerator = default;
                _fallbackEnumerator = null;

                if (list.Count == 0)
                {
                    // The collection is empty, just return false from MoveNext.
                    _enumeratorType = 100;
                }
                else if (list is List<T> concrete)
                {
                    _enumeratorType = 0;
                    _concreteEnumerator = concrete.GetEnumerator();
                }
                else
                {
                    _enumeratorType = 99;
                    _fallbackEnumerator = list.GetEnumerator();
                }
            }

            public T Current
            {
                get
                {
                    return _enumeratorType switch
                    {
                        0 => _concreteEnumerator.Current,
                        99 => _fallbackEnumerator!.Current,
                        _ => default!,
                    };
                }
            }

            public bool MoveNext()
            {
                return _enumeratorType switch
                {
                    0 => _concreteEnumerator.MoveNext(),
                    99 => _fallbackEnumerator!.MoveNext(),
                    _ => false,
                };
            }

            public void Dispose()
            {
                switch (_enumeratorType)
                {
                    case 0:
                        _concreteEnumerator.Dispose();
                        break;
                    case 99:
                        _fallbackEnumerator!.Dispose();
                        break;
                }
            }
        }
    }

    #endregion

    #region IEnumerable

    /// <summary>
    /// Avoids allocating an enumerator when enumerating an <see cref="IEnumerable{T}"/> where the concrete type
    /// has a well known struct enumerator, such as for <see cref="List{T}"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Several collection types (e.g. <see cref="List{T}"/>) provide a struct-based enumerator type
    /// (e.g. <see cref="List{T}.Enumerator"/>) which the compiler can use in <see langword="foreach" /> statements.
    /// When using a struct-based enumerator, no heap allocation occurs during such enumeration.
    /// This is in contrast to the interface-based enumerator <see cref="IEnumerator{T}"/> which will
    /// always be allocated on the heap.
    /// </para>
    /// <para>
    /// This method returns a custom struct enumerator that will avoid any heap allocation if <paramref name="source"/>
    /// (which is declared via interface <see cref="IEnumerable{T}"/>) is actually of known concrete type that
    /// provides its own struct enumerator. If so, it delegates to that type's enumerator without any boxing
    /// or other heap allocation.
    /// </para>
    /// <para>
    /// If <paramref name="source"/> is not of a known concrete type, the returned enumerator falls back to the
    /// interface-based enumerator, which will be allocated on the heap. Benchmarking shows the overhead in
    /// such cases is low enough to be within the measurement error, meaning this is an inexpensive optimization
    /// that won't regress behavior and with low downside for cases where it cannot apply an optimization.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[IEnumerable<string> source = ...;
    ///
    /// foreach (string item in source.NoAllocEnumerate())
    /// {
    ///     // ...
    /// }]]>
    /// </code>
    /// </example>
    public static OptimisticallyNonAllocatingEnumerable<T> NoAllocEnumerate<T>(this IEnumerable<T> source)
        where T : notnull
    {
#pragma warning disable CS0618 // Type or member is obsolete
        return new(source);
#pragma warning restore CS0618 // Type or member is obsolete
    }

    /// <summary>
    /// Provides a struct-based enumerator for use with <see cref="IEnumerable{T}"/>.
    /// Do not use this type directly. Use <see cref="NoAllocEnumerate{T}(IEnumerable{T})"/> instead.
    /// </summary>
    public readonly ref struct OptimisticallyNonAllocatingEnumerable<T>
        where T : notnull
    {
        private readonly IEnumerable<T> _source;

        [Obsolete("Do not construct directly. Use internal static method NoAllocEnumerateExtensions.NoAllocEnumerate instead.")]
        internal OptimisticallyNonAllocatingEnumerable(IEnumerable<T> source) => _source = source;

        public Enumerator GetEnumerator() => new(_source);

        /// <summary>
        /// A struct-based enumerator for use with <see cref="IEnumerable{T}"/>.
        /// Do not use this type directly. Use <see cref="NoAllocEnumerate{T}(IEnumerable{T})"/> instead.
        /// </summary>
        public struct Enumerator : IDisposable
        {
            private readonly int _enumeratorType;
            private readonly IEnumerator<T>? _fallbackEnumerator;
            private List<T>.Enumerator _concreteEnumerator;

            internal Enumerator(IEnumerable<T> source)
            {
                _concreteEnumerator = default;
                _fallbackEnumerator = null;

                if (source is ICollection<T> { Count: 0 } or IReadOnlyCollection<T> { Count: 0 })
                {
                    // The collection is empty, just return false from MoveNext.
                    _enumeratorType = 100;
                }
                else if (source is List<T> concrete)
                {
                    _enumeratorType = 0;
                    _concreteEnumerator = concrete.GetEnumerator();
                }
                else
                {
                    _enumeratorType = 99;
                    _fallbackEnumerator = source.GetEnumerator();
                }
            }

            public T Current
            {
                get
                {
                    return _enumeratorType switch
                    {
                        0 => _concreteEnumerator.Current,
                        99 => _fallbackEnumerator!.Current,
                        _ => default!,
                    };
                }
            }

            public bool MoveNext()
            {
                return _enumeratorType switch
                {
                    0 => _concreteEnumerator.MoveNext(),
                    99 => _fallbackEnumerator!.MoveNext(),
                    _ => false,
                };
            }

            public void Dispose()
            {
                switch (_enumeratorType)
                {
                    case 0:
                        _concreteEnumerator.Dispose();
                        break;
                    case 99:
                        _fallbackEnumerator!.Dispose();
                        break;
                }
            }
        }
    }

    #endregion

    #region IDictionary

    /// <summary>
    /// Avoids allocating an enumerator when enumerating an <see cref="IDictionary{TKey,TValue}"/> where the concrete type
    /// has a well known struct enumerator, such as for <see cref="Dictionary{TKey,TValue}"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Several collection types (e.g. <see cref="Dictionary{TKey,TValue}"/>) provide a struct-based enumerator type
    /// (e.g. <see cref="Dictionary{TKey,TValue}.Enumerator"/>) which the compiler can use in <see langword="foreach" /> statements.
    /// When using a struct-based enumerator, no heap allocation occurs during such enumeration.
    /// This is in contrast to the interface-based enumerator <see cref="IEnumerator{T}"/> which will
    /// always be allocated on the heap.
    /// </para>
    /// <para>
    /// This method returns a custom struct enumerator that will avoid any heap allocation if <paramref name="dictionary"/>
    /// (which is declared via interface <see cref="IEnumerable{T}"/>) is actually of known concrete type that
    /// provides its own struct enumerator. If so, it delegates to that type's enumerator without any boxing
    /// or other heap allocation.
    /// </para>
    /// <para>
    /// If <paramref name="dictionary"/> is not of a known concrete type, the returned enumerator falls back to the
    /// interface-based enumerator, which will be allocated on the heap. Benchmarking shows the overhead in
    /// such cases is low enough to be within the measurement error, meaning this is an inexpensive optimization
    /// that won't regress behavior and with low downside for cases where it cannot apply an optimization.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[IDictionary<string, string> dictionary = ...;
    ///
    /// foreach ((string key, string value) in dictionary.NoAllocEnumerate())
    /// {
    ///     // ...
    /// }]]>
    /// </code>
    /// </example>
    public static OptimisticallyNonAllocatingDictionaryEnumerable<TKey, TValue> NoAllocEnumerate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        where TKey : notnull
        where TValue : notnull
    {
#pragma warning disable CS0618 // Type or member is obsolete
        return new(dictionary);
#pragma warning restore CS0618 // Type or member is obsolete
    }

    /// <summary>
    /// Provides a struct-based enumerator for use with <see cref="IDictionary{TKey,TValue}"/>.
    /// Do not use this type directly. Use <see cref="NoAllocEnumerate{TKey, TValue}(IDictionary{TKey, TValue})"/> instead.
    /// </summary>
    public readonly ref struct OptimisticallyNonAllocatingDictionaryEnumerable<TKey, TValue>
        where TKey : notnull
        where TValue : notnull
    {
        private readonly IDictionary<TKey, TValue> _dictionary;

        [Obsolete("Do not construct directly. Use internal static method NoAllocEnumerateExtensions.NoAllocEnumerate instead.")]
        internal OptimisticallyNonAllocatingDictionaryEnumerable(IDictionary<TKey, TValue> dictionary) => _dictionary = dictionary;

        public Enumerator GetEnumerator() => new(_dictionary);

        /// <summary>
        /// A struct-based enumerator for use with <see cref="IEnumerable{T}"/>.
        /// Do not use this type directly. Use <see cref="NoAllocEnumerate{TKey, TValue}(IDictionary{TKey, TValue})"/> instead.
        /// </summary>
        public struct Enumerator : IDisposable
        {
            private readonly int _enumeratorType;
            private readonly IEnumerator<KeyValuePair<TKey, TValue>>? _fallbackEnumerator;
            private Dictionary<TKey, TValue>.Enumerator _concreteEnumerator;

            internal Enumerator(IDictionary<TKey, TValue> dictionary)
            {
                _concreteEnumerator = default;
                _fallbackEnumerator = null;

                if (dictionary.Count == 0)
                {
                    // The collection is empty, just return false from MoveNext.
                    _enumeratorType = 100;
                }
                else if (dictionary is Dictionary<TKey, TValue> concrete)
                {
                    _enumeratorType = 0;
                    _concreteEnumerator = concrete.GetEnumerator();
                }
                else
                {
                    _enumeratorType = 99;
                    _fallbackEnumerator = dictionary.GetEnumerator();
                }
            }

            public KeyValuePair<TKey, TValue> Current
            {
                get
                {
                    return _enumeratorType switch
                    {
                        0 => _concreteEnumerator.Current,
                        99 => _fallbackEnumerator!.Current,
                        _ => default!,
                    };
                }
            }

            public bool MoveNext()
            {
                return _enumeratorType switch
                {
                    0 => _concreteEnumerator.MoveNext(),
                    99 => _fallbackEnumerator!.MoveNext(),
                    _ => false,
                };
            }

            public void Dispose()
            {
                switch (_enumeratorType)
                {
                    case 0:
                        _concreteEnumerator.Dispose();
                        break;
                    case 99:
                        _fallbackEnumerator!.Dispose();
                        break;
                }
            }
        }
    }

    #endregion
}
