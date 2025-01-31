// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("System.Collections.Immutable.Tests, PublicKey=00240000048000009400000006020000002400005253413100040000010001004b86c4cb78549b34bab61a3b1800e23bfeb5b3ec390074041536a7e3cbd97f5f04cf0f857155a8928eaa29ebfd11cfbbad3ba70efea7bda3226c6a8d370a4cd303f714486b6ebc225985a638471e6ef571cc92a4613c00b8fa65d61ccee0cbe5f36330c9a01f4183559f1bef24cc2917c6d913e3a541333a1d05d9bed22b38cb")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = "")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Collections.Immutable")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("System.Collections.Immutable")]
[assembly: System.Reflection.AssemblyFileVersion("5.0.20.51904")]
[assembly: System.Reflection.AssemblyInformationalVersion("5.0.0+cf258a14b70ad9069470a108f13765e0e5988f51")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Collections.Immutable")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "git://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("5.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Collections.Immutable
{
    public partial interface IImmutableDictionary<TKey, TValue> : Generic.IReadOnlyDictionary<TKey, TValue>, Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>>, IEnumerable, Generic.IReadOnlyCollection<Generic.KeyValuePair<TKey, TValue>>
    {
        IImmutableDictionary<TKey, TValue> Add(TKey key, TValue value);
        IImmutableDictionary<TKey, TValue> AddRange(Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> pairs);
        IImmutableDictionary<TKey, TValue> Clear();
        bool Contains(Generic.KeyValuePair<TKey, TValue> pair);
        IImmutableDictionary<TKey, TValue> Remove(TKey key);
        IImmutableDictionary<TKey, TValue> RemoveRange(Generic.IEnumerable<TKey> keys);
        IImmutableDictionary<TKey, TValue> SetItem(TKey key, TValue value);
        IImmutableDictionary<TKey, TValue> SetItems(Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> items);
        bool TryGetKey(TKey equalKey, out TKey actualKey);
    }

    public partial interface IImmutableList<T> : Generic.IReadOnlyList<T>, Generic.IEnumerable<T>, IEnumerable, Generic.IReadOnlyCollection<T>
    {
        IImmutableList<T> Add(T value);
        IImmutableList<T> AddRange(Generic.IEnumerable<T> items);
        IImmutableList<T> Clear();
        int IndexOf(T item, int index, int count, Generic.IEqualityComparer<T>? equalityComparer);
        IImmutableList<T> Insert(int index, T element);
        IImmutableList<T> InsertRange(int index, Generic.IEnumerable<T> items);
        int LastIndexOf(T item, int index, int count, Generic.IEqualityComparer<T>? equalityComparer);
        IImmutableList<T> Remove(T value, Generic.IEqualityComparer<T>? equalityComparer);
        IImmutableList<T> RemoveAll(Predicate<T> match);
        IImmutableList<T> RemoveAt(int index);
        IImmutableList<T> RemoveRange(Generic.IEnumerable<T> items, Generic.IEqualityComparer<T>? equalityComparer);
        IImmutableList<T> RemoveRange(int index, int count);
        IImmutableList<T> Replace(T oldValue, T newValue, Generic.IEqualityComparer<T>? equalityComparer);
        IImmutableList<T> SetItem(int index, T value);
    }

    public partial interface IImmutableQueue<T> : Generic.IEnumerable<T>, IEnumerable
    {
        bool IsEmpty { get; }

        IImmutableQueue<T> Clear();
        IImmutableQueue<T> Dequeue();
        IImmutableQueue<T> Enqueue(T value);
        T Peek();
    }

    public partial interface IImmutableSet<T> : Generic.IReadOnlyCollection<T>, Generic.IEnumerable<T>, IEnumerable
    {
        IImmutableSet<T> Add(T value);
        IImmutableSet<T> Clear();
        bool Contains(T value);
        IImmutableSet<T> Except(Generic.IEnumerable<T> other);
        IImmutableSet<T> Intersect(Generic.IEnumerable<T> other);
        bool IsProperSubsetOf(Generic.IEnumerable<T> other);
        bool IsProperSupersetOf(Generic.IEnumerable<T> other);
        bool IsSubsetOf(Generic.IEnumerable<T> other);
        bool IsSupersetOf(Generic.IEnumerable<T> other);
        bool Overlaps(Generic.IEnumerable<T> other);
        IImmutableSet<T> Remove(T value);
        bool SetEquals(Generic.IEnumerable<T> other);
        IImmutableSet<T> SymmetricExcept(Generic.IEnumerable<T> other);
        bool TryGetValue(T equalValue, out T actualValue);
        IImmutableSet<T> Union(Generic.IEnumerable<T> other);
    }

    public partial interface IImmutableStack<T> : Generic.IEnumerable<T>, IEnumerable
    {
        bool IsEmpty { get; }

        IImmutableStack<T> Clear();
        T Peek();
        IImmutableStack<T> Pop();
        IImmutableStack<T> Push(T value);
    }

    public static partial class ImmutableArray
    {
        public static int BinarySearch<T>(this ImmutableArray<T> array, T value, Generic.IComparer<T>? comparer) { throw null; }

        public static int BinarySearch<T>(this ImmutableArray<T> array, T value) { throw null; }

        public static int BinarySearch<T>(this ImmutableArray<T> array, int index, int length, T value, Generic.IComparer<T>? comparer) { throw null; }

        public static int BinarySearch<T>(this ImmutableArray<T> array, int index, int length, T value) { throw null; }

        public static ImmutableArray<T> Create<T>() { throw null; }

        public static ImmutableArray<T> Create<T>(T item1, T item2, T item3, T item4) { throw null; }

        public static ImmutableArray<T> Create<T>(T item1, T item2, T item3) { throw null; }

        public static ImmutableArray<T> Create<T>(T item1, T item2) { throw null; }

        public static ImmutableArray<T> Create<T>(T item) { throw null; }

        public static ImmutableArray<T> Create<T>(T[] items, int start, int length) { throw null; }

        public static ImmutableArray<T> Create<T>(params T[]? items) { throw null; }

        public static ImmutableArray<T> Create<T>(ImmutableArray<T> items, int start, int length) { throw null; }

        public static ImmutableArray<T>.Builder CreateBuilder<T>() { throw null; }

        public static ImmutableArray<T>.Builder CreateBuilder<T>(int initialCapacity) { throw null; }

        public static ImmutableArray<T> CreateRange<T>(Generic.IEnumerable<T> items) { throw null; }

        public static ImmutableArray<TResult> CreateRange<TSource, TResult>(ImmutableArray<TSource> items, Func<TSource, TResult> selector) { throw null; }

        public static ImmutableArray<TResult> CreateRange<TSource, TResult>(ImmutableArray<TSource> items, int start, int length, Func<TSource, TResult> selector) { throw null; }

        public static ImmutableArray<TResult> CreateRange<TSource, TArg, TResult>(ImmutableArray<TSource> items, Func<TSource, TArg, TResult> selector, TArg arg) { throw null; }

        public static ImmutableArray<TResult> CreateRange<TSource, TArg, TResult>(ImmutableArray<TSource> items, int start, int length, Func<TSource, TArg, TResult> selector, TArg arg) { throw null; }

        public static ImmutableArray<TSource> ToImmutableArray<TSource>(this Generic.IEnumerable<TSource> items) { throw null; }

        public static ImmutableArray<TSource> ToImmutableArray<TSource>(this ImmutableArray<TSource>.Builder builder) { throw null; }
    }

    public partial struct ImmutableArray<T> : Generic.IReadOnlyList<T>, Generic.IEnumerable<T>, IEnumerable, Generic.IReadOnlyCollection<T>, Generic.IList<T>, Generic.ICollection<T>, IEquatable<ImmutableArray<T>>, IList, ICollection, IStructuralComparable, IStructuralEquatable, IImmutableList<T>
    {
        private object _dummy;
        private int _dummyPrimitive;
        public static readonly ImmutableArray<T> Empty;
        public bool IsDefault { get { throw null; } }

        public bool IsDefaultOrEmpty { get { throw null; } }

        public bool IsEmpty { get { throw null; } }

        public T this[int index] { get { throw null; } }

        public int Length { get { throw null; } }

        int Generic.ICollection<T>.Count { get { throw null; } }

        bool Generic.ICollection<T>.IsReadOnly { get { throw null; } }

        T Generic.IList<T>.this[int index] { get { throw null; } set { } }

        int Generic.IReadOnlyCollection<T>.Count { get { throw null; } }

        T Generic.IReadOnlyList<T>.this[int index] { get { throw null; } }

        int ICollection.Count { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        bool IList.IsFixedSize { get { throw null; } }

        bool IList.IsReadOnly { get { throw null; } }

        object? IList.this[int index] { get { throw null; } set { } }

        public ImmutableArray<T> Add(T item) { throw null; }

        public ImmutableArray<T> AddRange(Generic.IEnumerable<T> items) { throw null; }

        public ImmutableArray<T> AddRange(ImmutableArray<T> items) { throw null; }

        public ImmutableArray<TOther> As<TOther>()
            where TOther : class { throw null; }

        public ReadOnlyMemory<T> AsMemory() { throw null; }

        public ReadOnlySpan<T> AsSpan() { throw null; }

        public ImmutableArray<TOther> CastArray<TOther>()
            where TOther : class { throw null; }

        public static ImmutableArray<T> CastUp<TDerived>(ImmutableArray<TDerived> items)
            where TDerived : class, T { throw null; }

        public ImmutableArray<T> Clear() { throw null; }

        public bool Contains(T item) { throw null; }

        public void CopyTo(T[] destination, int destinationIndex) { }

        public void CopyTo(T[] destination) { }

        public void CopyTo(int sourceIndex, T[] destination, int destinationIndex, int length) { }

        public bool Equals(ImmutableArray<T> other) { throw null; }

        public override bool Equals(object? obj) { throw null; }

        public Enumerator GetEnumerator() { throw null; }

        public override int GetHashCode() { throw null; }

        public int IndexOf(T item, int startIndex, Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

        public int IndexOf(T item, int startIndex, int count, Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

        public int IndexOf(T item, int startIndex, int count) { throw null; }

        public int IndexOf(T item, int startIndex) { throw null; }

        public int IndexOf(T item) { throw null; }

        public ImmutableArray<T> Insert(int index, T item) { throw null; }

        public ImmutableArray<T> InsertRange(int index, Generic.IEnumerable<T> items) { throw null; }

        public ImmutableArray<T> InsertRange(int index, ImmutableArray<T> items) { throw null; }

        public ref readonly T ItemRef(int index) { throw null; }

        public int LastIndexOf(T item, int startIndex, int count, Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

        public int LastIndexOf(T item, int startIndex, int count) { throw null; }

        public int LastIndexOf(T item, int startIndex) { throw null; }

        public int LastIndexOf(T item) { throw null; }

        public Generic.IEnumerable<TResult> OfType<TResult>() { throw null; }

        public static bool operator ==(ImmutableArray<T> left, ImmutableArray<T> right) { throw null; }

        public static bool operator ==(ImmutableArray<T>? left, ImmutableArray<T>? right) { throw null; }

        public static bool operator !=(ImmutableArray<T> left, ImmutableArray<T> right) { throw null; }

        public static bool operator !=(ImmutableArray<T>? left, ImmutableArray<T>? right) { throw null; }

        public ImmutableArray<T> Remove(T item, Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

        public ImmutableArray<T> Remove(T item) { throw null; }

        public ImmutableArray<T> RemoveAll(Predicate<T> match) { throw null; }

        public ImmutableArray<T> RemoveAt(int index) { throw null; }

        public ImmutableArray<T> RemoveRange(Generic.IEnumerable<T> items, Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

        public ImmutableArray<T> RemoveRange(Generic.IEnumerable<T> items) { throw null; }

        public ImmutableArray<T> RemoveRange(ImmutableArray<T> items, Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

        public ImmutableArray<T> RemoveRange(ImmutableArray<T> items) { throw null; }

        public ImmutableArray<T> RemoveRange(int index, int length) { throw null; }

        public ImmutableArray<T> Replace(T oldValue, T newValue, Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

        public ImmutableArray<T> Replace(T oldValue, T newValue) { throw null; }

        public ImmutableArray<T> SetItem(int index, T item) { throw null; }

        public ImmutableArray<T> Sort() { throw null; }

        public ImmutableArray<T> Sort(Generic.IComparer<T>? comparer) { throw null; }

        public ImmutableArray<T> Sort(Comparison<T> comparison) { throw null; }

        public ImmutableArray<T> Sort(int index, int count, Generic.IComparer<T>? comparer) { throw null; }

        void Generic.ICollection<T>.Add(T item) { }

        void Generic.ICollection<T>.Clear() { }

        bool Generic.ICollection<T>.Remove(T item) { throw null; }

        Generic.IEnumerator<T> Generic.IEnumerable<T>.GetEnumerator() { throw null; }

        void Generic.IList<T>.Insert(int index, T item) { }

        void Generic.IList<T>.RemoveAt(int index) { }

        void ICollection.CopyTo(Array array, int index) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        int IList.Add(object value) { throw null; }

        void IList.Clear() { }

        bool IList.Contains(object value) { throw null; }

        int IList.IndexOf(object value) { throw null; }

        void IList.Insert(int index, object value) { }

        void IList.Remove(object value) { }

        void IList.RemoveAt(int index) { }

        IImmutableList<T> IImmutableList<T>.Add(T value) { throw null; }

        IImmutableList<T> IImmutableList<T>.AddRange(Generic.IEnumerable<T> items) { throw null; }

        IImmutableList<T> IImmutableList<T>.Clear() { throw null; }

        IImmutableList<T> IImmutableList<T>.Insert(int index, T element) { throw null; }

        IImmutableList<T> IImmutableList<T>.InsertRange(int index, Generic.IEnumerable<T> items) { throw null; }

        IImmutableList<T> IImmutableList<T>.Remove(T value, Generic.IEqualityComparer<T> equalityComparer) { throw null; }

        IImmutableList<T> IImmutableList<T>.RemoveAll(Predicate<T> match) { throw null; }

        IImmutableList<T> IImmutableList<T>.RemoveAt(int index) { throw null; }

        IImmutableList<T> IImmutableList<T>.RemoveRange(Generic.IEnumerable<T> items, Generic.IEqualityComparer<T> equalityComparer) { throw null; }

        IImmutableList<T> IImmutableList<T>.RemoveRange(int index, int count) { throw null; }

        IImmutableList<T> IImmutableList<T>.Replace(T oldValue, T newValue, Generic.IEqualityComparer<T> equalityComparer) { throw null; }

        IImmutableList<T> IImmutableList<T>.SetItem(int index, T value) { throw null; }

        int IStructuralComparable.CompareTo(object other, IComparer comparer) { throw null; }

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer) { throw null; }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer) { throw null; }

        public Builder ToBuilder() { throw null; }

        public sealed partial class Builder : Generic.IList<T>, Generic.ICollection<T>, Generic.IEnumerable<T>, IEnumerable, Generic.IReadOnlyList<T>, Generic.IReadOnlyCollection<T>
        {
            internal Builder() { }

            public int Capacity { get { throw null; } set { } }

            public int Count { get { throw null; } set { } }

            public T this[int index] { get { throw null; } set { } }

            bool Generic.ICollection<T>.IsReadOnly { get { throw null; } }

            public void Add(T item) { }

            public void AddRange(T[] items, int length) { }

            public void AddRange(params T[] items) { }

            public void AddRange(Generic.IEnumerable<T> items) { }

            public void AddRange(ImmutableArray<T> items, int length) { }

            public void AddRange(Builder items) { }

            public void AddRange(ImmutableArray<T> items) { }

            public void AddRange<TDerived>(TDerived[] items)
                where TDerived : T { }

            public void AddRange<TDerived>(ImmutableArray<TDerived>.Builder items)
                where TDerived : T { }

            public void AddRange<TDerived>(ImmutableArray<TDerived> items)
                where TDerived : T { }

            public void Clear() { }

            public bool Contains(T item) { throw null; }

            public void CopyTo(T[] array, int index) { }

            public Generic.IEnumerator<T> GetEnumerator() { throw null; }

            public int IndexOf(T item, int startIndex, int count, Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

            public int IndexOf(T item, int startIndex, int count) { throw null; }

            public int IndexOf(T item, int startIndex) { throw null; }

            public int IndexOf(T item) { throw null; }

            public void Insert(int index, T item) { }

            public ref readonly T ItemRef(int index) { throw null; }

            public int LastIndexOf(T item, int startIndex, int count, Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

            public int LastIndexOf(T item, int startIndex, int count) { throw null; }

            public int LastIndexOf(T item, int startIndex) { throw null; }

            public int LastIndexOf(T item) { throw null; }

            public ImmutableArray<T> MoveToImmutable() { throw null; }

            public bool Remove(T element) { throw null; }

            public void RemoveAt(int index) { }

            public void Reverse() { }

            public void Sort() { }

            public void Sort(Generic.IComparer<T>? comparer) { }

            public void Sort(Comparison<T> comparison) { }

            public void Sort(int index, int count, Generic.IComparer<T>? comparer) { }

            Generic.IEnumerator<T> Generic.IEnumerable<T>.GetEnumerator() { throw null; }

            IEnumerator IEnumerable.GetEnumerator() { throw null; }

            public T[] ToArray() { throw null; }

            public ImmutableArray<T> ToImmutable() { throw null; }
        }

        public partial struct Enumerator
        {
            private object _dummy;
            private int _dummyPrimitive;
            public T Current { get { throw null; } }

            public bool MoveNext() { throw null; }
        }
    }

    public static partial class ImmutableDictionary
    {
        public static bool Contains<TKey, TValue>(this IImmutableDictionary<TKey, TValue> map, TKey key, TValue value) { throw null; }

        public static ImmutableDictionary<TKey, TValue> Create<TKey, TValue>() { throw null; }

        public static ImmutableDictionary<TKey, TValue> Create<TKey, TValue>(Generic.IEqualityComparer<TKey>? keyComparer, Generic.IEqualityComparer<TValue>? valueComparer) { throw null; }

        public static ImmutableDictionary<TKey, TValue> Create<TKey, TValue>(Generic.IEqualityComparer<TKey>? keyComparer) { throw null; }

        public static ImmutableDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>() { throw null; }

        public static ImmutableDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>(Generic.IEqualityComparer<TKey>? keyComparer, Generic.IEqualityComparer<TValue>? valueComparer) { throw null; }

        public static ImmutableDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>(Generic.IEqualityComparer<TKey>? keyComparer) { throw null; }

        public static ImmutableDictionary<TKey, TValue> CreateRange<TKey, TValue>(Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> items) { throw null; }

        public static ImmutableDictionary<TKey, TValue> CreateRange<TKey, TValue>(Generic.IEqualityComparer<TKey>? keyComparer, Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> items) { throw null; }

        public static ImmutableDictionary<TKey, TValue> CreateRange<TKey, TValue>(Generic.IEqualityComparer<TKey>? keyComparer, Generic.IEqualityComparer<TValue>? valueComparer, Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> items) { throw null; }

        public static TValue GetValueOrDefault<TKey, TValue>(this IImmutableDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue) { throw null; }

        public static TValue? GetValueOrDefault<TKey, TValue>(this IImmutableDictionary<TKey, TValue> dictionary, TKey key) { throw null; }

        public static ImmutableDictionary<TKey, TSource> ToImmutableDictionary<TSource, TKey>(this Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Generic.IEqualityComparer<TKey>? keyComparer) { throw null; }

        public static ImmutableDictionary<TKey, TSource> ToImmutableDictionary<TSource, TKey>(this Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector) { throw null; }

        public static ImmutableDictionary<TKey, TValue> ToImmutableDictionary<TKey, TValue>(this Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> source, Generic.IEqualityComparer<TKey>? keyComparer, Generic.IEqualityComparer<TValue>? valueComparer) { throw null; }

        public static ImmutableDictionary<TKey, TValue> ToImmutableDictionary<TKey, TValue>(this Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> source, Generic.IEqualityComparer<TKey>? keyComparer) { throw null; }

        public static ImmutableDictionary<TKey, TValue> ToImmutableDictionary<TKey, TValue>(this Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> source) { throw null; }

        public static ImmutableDictionary<TKey, TValue> ToImmutableDictionary<TKey, TValue>(this ImmutableDictionary<TKey, TValue>.Builder builder) { throw null; }

        public static ImmutableDictionary<TKey, TValue> ToImmutableDictionary<TSource, TKey, TValue>(this Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector, Generic.IEqualityComparer<TKey>? keyComparer, Generic.IEqualityComparer<TValue>? valueComparer) { throw null; }

        public static ImmutableDictionary<TKey, TValue> ToImmutableDictionary<TSource, TKey, TValue>(this Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector, Generic.IEqualityComparer<TKey>? keyComparer) { throw null; }

        public static ImmutableDictionary<TKey, TValue> ToImmutableDictionary<TSource, TKey, TValue>(this Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector) { throw null; }
    }

    public sealed partial class ImmutableDictionary<TKey, TValue> : IImmutableDictionary<TKey, TValue>, Generic.IReadOnlyDictionary<TKey, TValue>, Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>>, IEnumerable, Generic.IReadOnlyCollection<Generic.KeyValuePair<TKey, TValue>>, Generic.IDictionary<TKey, TValue>, Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>, IDictionary, ICollection
    {
        internal ImmutableDictionary() { }

        public static readonly ImmutableDictionary<TKey, TValue> Empty;
        public int Count { get { throw null; } }

        public bool IsEmpty { get { throw null; } }

        public TValue this[TKey key] { get { throw null; } }

        public Generic.IEqualityComparer<TKey> KeyComparer { get { throw null; } }

        public Generic.IEnumerable<TKey> Keys { get { throw null; } }

        bool Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.IsReadOnly { get { throw null; } }

        TValue Generic.IDictionary<TKey, TValue>.this[TKey key] { get { throw null; } set { } }

        Generic.ICollection<TKey> Generic.IDictionary<TKey, TValue>.Keys { get { throw null; } }

        Generic.ICollection<TValue> Generic.IDictionary<TKey, TValue>.Values { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        bool IDictionary.IsFixedSize { get { throw null; } }

        bool IDictionary.IsReadOnly { get { throw null; } }

        object? IDictionary.this[object key] { get { throw null; } set { } }

        ICollection IDictionary.Keys { get { throw null; } }

        ICollection IDictionary.Values { get { throw null; } }

        public Generic.IEqualityComparer<TValue> ValueComparer { get { throw null; } }

        public Generic.IEnumerable<TValue> Values { get { throw null; } }

        public ImmutableDictionary<TKey, TValue> Add(TKey key, TValue value) { throw null; }

        public ImmutableDictionary<TKey, TValue> AddRange(Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> pairs) { throw null; }

        public ImmutableDictionary<TKey, TValue> Clear() { throw null; }

        public bool Contains(Generic.KeyValuePair<TKey, TValue> pair) { throw null; }

        public bool ContainsKey(TKey key) { throw null; }

        public bool ContainsValue(TValue value) { throw null; }

        public Enumerator GetEnumerator() { throw null; }

        public ImmutableDictionary<TKey, TValue> Remove(TKey key) { throw null; }

        public ImmutableDictionary<TKey, TValue> RemoveRange(Generic.IEnumerable<TKey> keys) { throw null; }

        public ImmutableDictionary<TKey, TValue> SetItem(TKey key, TValue value) { throw null; }

        public ImmutableDictionary<TKey, TValue> SetItems(Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> items) { throw null; }

        void Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.Add(Generic.KeyValuePair<TKey, TValue> item) { }

        void Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.Clear() { }

        void Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.CopyTo(Generic.KeyValuePair<TKey, TValue>[] array, int arrayIndex) { }

        bool Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.Remove(Generic.KeyValuePair<TKey, TValue> item) { throw null; }

        void Generic.IDictionary<TKey, TValue>.Add(TKey key, TValue value) { }

        bool Generic.IDictionary<TKey, TValue>.Remove(TKey key) { throw null; }

        Generic.IEnumerator<Generic.KeyValuePair<TKey, TValue>> Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>>.GetEnumerator() { throw null; }

        void ICollection.CopyTo(Array array, int arrayIndex) { }

        void IDictionary.Add(object key, object value) { }

        void IDictionary.Clear() { }

        bool IDictionary.Contains(object key) { throw null; }

        IDictionaryEnumerator IDictionary.GetEnumerator() { throw null; }

        void IDictionary.Remove(object key) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Add(TKey key, TValue value) { throw null; }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.AddRange(Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> pairs) { throw null; }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Clear() { throw null; }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Remove(TKey key) { throw null; }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.RemoveRange(Generic.IEnumerable<TKey> keys) { throw null; }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.SetItem(TKey key, TValue value) { throw null; }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.SetItems(Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> items) { throw null; }

        public Builder ToBuilder() { throw null; }

        public bool TryGetKey(TKey equalKey, out TKey actualKey) { throw null; }

        public bool TryGetValue(TKey key, out TValue value) { throw null; }

        public ImmutableDictionary<TKey, TValue> WithComparers(Generic.IEqualityComparer<TKey>? keyComparer, Generic.IEqualityComparer<TValue>? valueComparer) { throw null; }

        public ImmutableDictionary<TKey, TValue> WithComparers(Generic.IEqualityComparer<TKey>? keyComparer) { throw null; }

        public sealed partial class Builder : Generic.IDictionary<TKey, TValue>, Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>, Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>>, IEnumerable, Generic.IReadOnlyDictionary<TKey, TValue>, Generic.IReadOnlyCollection<Generic.KeyValuePair<TKey, TValue>>, IDictionary, ICollection
        {
            internal Builder() { }

            public int Count { get { throw null; } }

            public TValue this[TKey key] { get { throw null; } set { } }

            public Generic.IEqualityComparer<TKey> KeyComparer { get { throw null; } set { } }

            public Generic.IEnumerable<TKey> Keys { get { throw null; } }

            bool Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.IsReadOnly { get { throw null; } }

            Generic.ICollection<TKey> Generic.IDictionary<TKey, TValue>.Keys { get { throw null; } }

            Generic.ICollection<TValue> Generic.IDictionary<TKey, TValue>.Values { get { throw null; } }

            bool ICollection.IsSynchronized { get { throw null; } }

            object ICollection.SyncRoot { get { throw null; } }

            bool IDictionary.IsFixedSize { get { throw null; } }

            bool IDictionary.IsReadOnly { get { throw null; } }

            object? IDictionary.this[object key] { get { throw null; } set { } }

            ICollection IDictionary.Keys { get { throw null; } }

            ICollection IDictionary.Values { get { throw null; } }

            public Generic.IEqualityComparer<TValue> ValueComparer { get { throw null; } set { } }

            public Generic.IEnumerable<TValue> Values { get { throw null; } }

            public void Add(TKey key, TValue value) { }

            public void Add(Generic.KeyValuePair<TKey, TValue> item) { }

            public void AddRange(Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> items) { }

            public void Clear() { }

            public bool Contains(Generic.KeyValuePair<TKey, TValue> item) { throw null; }

            public bool ContainsKey(TKey key) { throw null; }

            public bool ContainsValue(TValue value) { throw null; }

            public Enumerator GetEnumerator() { throw null; }

            public TValue GetValueOrDefault(TKey key, TValue defaultValue) { throw null; }

            public TValue? GetValueOrDefault(TKey key) { throw null; }

            public bool Remove(TKey key) { throw null; }

            public bool Remove(Generic.KeyValuePair<TKey, TValue> item) { throw null; }

            public void RemoveRange(Generic.IEnumerable<TKey> keys) { }

            void Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.CopyTo(Generic.KeyValuePair<TKey, TValue>[] array, int arrayIndex) { }

            Generic.IEnumerator<Generic.KeyValuePair<TKey, TValue>> Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>>.GetEnumerator() { throw null; }

            void ICollection.CopyTo(Array array, int arrayIndex) { }

            void IDictionary.Add(object key, object value) { }

            bool IDictionary.Contains(object key) { throw null; }

            IDictionaryEnumerator IDictionary.GetEnumerator() { throw null; }

            void IDictionary.Remove(object key) { }

            IEnumerator IEnumerable.GetEnumerator() { throw null; }

            public ImmutableDictionary<TKey, TValue> ToImmutable() { throw null; }

            public bool TryGetKey(TKey equalKey, out TKey actualKey) { throw null; }

            public bool TryGetValue(TKey key, out TValue value) { throw null; }
        }

        public partial struct Enumerator : Generic.IEnumerator<Generic.KeyValuePair<TKey, TValue>>, IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public Generic.KeyValuePair<TKey, TValue> Current { get { throw null; } }

            object IEnumerator.Current { get { throw null; } }

            public void Dispose() { }

            public bool MoveNext() { throw null; }

            public void Reset() { }
        }
    }

    public static partial class ImmutableHashSet
    {
        public static ImmutableHashSet<T> Create<T>() { throw null; }

        public static ImmutableHashSet<T> Create<T>(T item) { throw null; }

        public static ImmutableHashSet<T> Create<T>(params T[] items) { throw null; }

        public static ImmutableHashSet<T> Create<T>(Generic.IEqualityComparer<T>? equalityComparer, T item) { throw null; }

        public static ImmutableHashSet<T> Create<T>(Generic.IEqualityComparer<T>? equalityComparer, params T[] items) { throw null; }

        public static ImmutableHashSet<T> Create<T>(Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

        public static ImmutableHashSet<T>.Builder CreateBuilder<T>() { throw null; }

        public static ImmutableHashSet<T>.Builder CreateBuilder<T>(Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

        public static ImmutableHashSet<T> CreateRange<T>(Generic.IEnumerable<T> items) { throw null; }

        public static ImmutableHashSet<T> CreateRange<T>(Generic.IEqualityComparer<T>? equalityComparer, Generic.IEnumerable<T> items) { throw null; }

        public static ImmutableHashSet<TSource> ToImmutableHashSet<TSource>(this Generic.IEnumerable<TSource> source, Generic.IEqualityComparer<TSource>? equalityComparer) { throw null; }

        public static ImmutableHashSet<TSource> ToImmutableHashSet<TSource>(this Generic.IEnumerable<TSource> source) { throw null; }

        public static ImmutableHashSet<TSource> ToImmutableHashSet<TSource>(this ImmutableHashSet<TSource>.Builder builder) { throw null; }
    }

    public sealed partial class ImmutableHashSet<T> : IImmutableSet<T>, Generic.IReadOnlyCollection<T>, Generic.IEnumerable<T>, IEnumerable, Generic.ICollection<T>, Generic.ISet<T>, ICollection
    {
        internal ImmutableHashSet() { }

        public static readonly ImmutableHashSet<T> Empty;
        public int Count { get { throw null; } }

        public bool IsEmpty { get { throw null; } }

        public Generic.IEqualityComparer<T> KeyComparer { get { throw null; } }

        bool Generic.ICollection<T>.IsReadOnly { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        public ImmutableHashSet<T> Add(T item) { throw null; }

        public ImmutableHashSet<T> Clear() { throw null; }

        public bool Contains(T item) { throw null; }

        public ImmutableHashSet<T> Except(Generic.IEnumerable<T> other) { throw null; }

        public Enumerator GetEnumerator() { throw null; }

        public ImmutableHashSet<T> Intersect(Generic.IEnumerable<T> other) { throw null; }

        public bool IsProperSubsetOf(Generic.IEnumerable<T> other) { throw null; }

        public bool IsProperSupersetOf(Generic.IEnumerable<T> other) { throw null; }

        public bool IsSubsetOf(Generic.IEnumerable<T> other) { throw null; }

        public bool IsSupersetOf(Generic.IEnumerable<T> other) { throw null; }

        public bool Overlaps(Generic.IEnumerable<T> other) { throw null; }

        public ImmutableHashSet<T> Remove(T item) { throw null; }

        public bool SetEquals(Generic.IEnumerable<T> other) { throw null; }

        public ImmutableHashSet<T> SymmetricExcept(Generic.IEnumerable<T> other) { throw null; }

        void Generic.ICollection<T>.Add(T item) { }

        void Generic.ICollection<T>.Clear() { }

        void Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex) { }

        bool Generic.ICollection<T>.Remove(T item) { throw null; }

        Generic.IEnumerator<T> Generic.IEnumerable<T>.GetEnumerator() { throw null; }

        bool Generic.ISet<T>.Add(T item) { throw null; }

        void Generic.ISet<T>.ExceptWith(Generic.IEnumerable<T> other) { }

        void Generic.ISet<T>.IntersectWith(Generic.IEnumerable<T> other) { }

        void Generic.ISet<T>.SymmetricExceptWith(Generic.IEnumerable<T> other) { }

        void Generic.ISet<T>.UnionWith(Generic.IEnumerable<T> other) { }

        void ICollection.CopyTo(Array array, int arrayIndex) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        IImmutableSet<T> IImmutableSet<T>.Add(T item) { throw null; }

        IImmutableSet<T> IImmutableSet<T>.Clear() { throw null; }

        IImmutableSet<T> IImmutableSet<T>.Except(Generic.IEnumerable<T> other) { throw null; }

        IImmutableSet<T> IImmutableSet<T>.Intersect(Generic.IEnumerable<T> other) { throw null; }

        IImmutableSet<T> IImmutableSet<T>.Remove(T item) { throw null; }

        IImmutableSet<T> IImmutableSet<T>.SymmetricExcept(Generic.IEnumerable<T> other) { throw null; }

        IImmutableSet<T> IImmutableSet<T>.Union(Generic.IEnumerable<T> other) { throw null; }

        public Builder ToBuilder() { throw null; }

        public bool TryGetValue(T equalValue, out T actualValue) { throw null; }

        public ImmutableHashSet<T> Union(Generic.IEnumerable<T> other) { throw null; }

        public ImmutableHashSet<T> WithComparer(Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

        public sealed partial class Builder : Generic.IReadOnlyCollection<T>, Generic.IEnumerable<T>, IEnumerable, Generic.ISet<T>, Generic.ICollection<T>
        {
            internal Builder() { }

            public int Count { get { throw null; } }

            public Generic.IEqualityComparer<T> KeyComparer { get { throw null; } set { } }

            bool Generic.ICollection<T>.IsReadOnly { get { throw null; } }

            public bool Add(T item) { throw null; }

            public void Clear() { }

            public bool Contains(T item) { throw null; }

            public void ExceptWith(Generic.IEnumerable<T> other) { }

            public Enumerator GetEnumerator() { throw null; }

            public void IntersectWith(Generic.IEnumerable<T> other) { }

            public bool IsProperSubsetOf(Generic.IEnumerable<T> other) { throw null; }

            public bool IsProperSupersetOf(Generic.IEnumerable<T> other) { throw null; }

            public bool IsSubsetOf(Generic.IEnumerable<T> other) { throw null; }

            public bool IsSupersetOf(Generic.IEnumerable<T> other) { throw null; }

            public bool Overlaps(Generic.IEnumerable<T> other) { throw null; }

            public bool Remove(T item) { throw null; }

            public bool SetEquals(Generic.IEnumerable<T> other) { throw null; }

            public void SymmetricExceptWith(Generic.IEnumerable<T> other) { }

            void Generic.ICollection<T>.Add(T item) { }

            void Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex) { }

            Generic.IEnumerator<T> Generic.IEnumerable<T>.GetEnumerator() { throw null; }

            IEnumerator IEnumerable.GetEnumerator() { throw null; }

            public ImmutableHashSet<T> ToImmutable() { throw null; }

            public bool TryGetValue(T equalValue, out T actualValue) { throw null; }

            public void UnionWith(Generic.IEnumerable<T> other) { }
        }

        public partial struct Enumerator : Generic.IEnumerator<T>, IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public T Current { get { throw null; } }

            object? IEnumerator.Current { get { throw null; } }

            public void Dispose() { }

            public bool MoveNext() { throw null; }

            public void Reset() { }
        }
    }

    public static partial class ImmutableInterlocked
    {
        public static TValue AddOrUpdate<TKey, TValue>(ref ImmutableDictionary<TKey, TValue> location, TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory) { throw null; }

        public static TValue AddOrUpdate<TKey, TValue>(ref ImmutableDictionary<TKey, TValue> location, TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory) { throw null; }

        public static void Enqueue<T>(ref ImmutableQueue<T> location, T value) { }

        public static TValue GetOrAdd<TKey, TValue>(ref ImmutableDictionary<TKey, TValue> location, TKey key, TValue value) { throw null; }

        public static TValue GetOrAdd<TKey, TValue>(ref ImmutableDictionary<TKey, TValue> location, TKey key, Func<TKey, TValue> valueFactory) { throw null; }

        public static TValue GetOrAdd<TKey, TValue, TArg>(ref ImmutableDictionary<TKey, TValue> location, TKey key, Func<TKey, TArg, TValue> valueFactory, TArg factoryArgument) { throw null; }

        public static ImmutableArray<T> InterlockedCompareExchange<T>(ref ImmutableArray<T> location, ImmutableArray<T> value, ImmutableArray<T> comparand) { throw null; }

        public static ImmutableArray<T> InterlockedExchange<T>(ref ImmutableArray<T> location, ImmutableArray<T> value) { throw null; }

        public static bool InterlockedInitialize<T>(ref ImmutableArray<T> location, ImmutableArray<T> value) { throw null; }

        public static void Push<T>(ref ImmutableStack<T> location, T value) { }

        public static bool TryAdd<TKey, TValue>(ref ImmutableDictionary<TKey, TValue> location, TKey key, TValue value) { throw null; }

        public static bool TryDequeue<T>(ref ImmutableQueue<T> location, out T value) { throw null; }

        public static bool TryPop<T>(ref ImmutableStack<T> location, out T value) { throw null; }

        public static bool TryRemove<TKey, TValue>(ref ImmutableDictionary<TKey, TValue> location, TKey key, out TValue value) { throw null; }

        public static bool TryUpdate<TKey, TValue>(ref ImmutableDictionary<TKey, TValue> location, TKey key, TValue newValue, TValue comparisonValue) { throw null; }

        public static bool Update<T>(ref T location, Func<T, T> transformer)
            where T : class { throw null; }

        public static bool Update<T>(ref ImmutableArray<T> location, Func<ImmutableArray<T>, ImmutableArray<T>> transformer) { throw null; }

        public static bool Update<T, TArg>(ref T location, Func<T, TArg, T> transformer, TArg transformerArgument)
            where T : class { throw null; }

        public static bool Update<T, TArg>(ref ImmutableArray<T> location, Func<ImmutableArray<T>, TArg, ImmutableArray<T>> transformer, TArg transformerArgument) { throw null; }
    }

    public static partial class ImmutableList
    {
        public static ImmutableList<T> Create<T>() { throw null; }

        public static ImmutableList<T> Create<T>(T item) { throw null; }

        public static ImmutableList<T> Create<T>(params T[] items) { throw null; }

        public static ImmutableList<T>.Builder CreateBuilder<T>() { throw null; }

        public static ImmutableList<T> CreateRange<T>(Generic.IEnumerable<T> items) { throw null; }

        public static int IndexOf<T>(this IImmutableList<T> list, T item, Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

        public static int IndexOf<T>(this IImmutableList<T> list, T item, int startIndex, int count) { throw null; }

        public static int IndexOf<T>(this IImmutableList<T> list, T item, int startIndex) { throw null; }

        public static int IndexOf<T>(this IImmutableList<T> list, T item) { throw null; }

        public static int LastIndexOf<T>(this IImmutableList<T> list, T item, Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

        public static int LastIndexOf<T>(this IImmutableList<T> list, T item, int startIndex, int count) { throw null; }

        public static int LastIndexOf<T>(this IImmutableList<T> list, T item, int startIndex) { throw null; }

        public static int LastIndexOf<T>(this IImmutableList<T> list, T item) { throw null; }

        public static IImmutableList<T> Remove<T>(this IImmutableList<T> list, T value) { throw null; }

        public static IImmutableList<T> RemoveRange<T>(this IImmutableList<T> list, Generic.IEnumerable<T> items) { throw null; }

        public static IImmutableList<T> Replace<T>(this IImmutableList<T> list, T oldValue, T newValue) { throw null; }

        public static ImmutableList<TSource> ToImmutableList<TSource>(this Generic.IEnumerable<TSource> source) { throw null; }

        public static ImmutableList<TSource> ToImmutableList<TSource>(this ImmutableList<TSource>.Builder builder) { throw null; }
    }

    public sealed partial class ImmutableList<T> : IImmutableList<T>, Generic.IReadOnlyList<T>, Generic.IEnumerable<T>, IEnumerable, Generic.IReadOnlyCollection<T>, Generic.IList<T>, Generic.ICollection<T>, IList, ICollection
    {
        internal ImmutableList() { }

        public static readonly ImmutableList<T> Empty;
        public int Count { get { throw null; } }

        public bool IsEmpty { get { throw null; } }

        public T this[int index] { get { throw null; } }

        bool Generic.ICollection<T>.IsReadOnly { get { throw null; } }

        T Generic.IList<T>.this[int index] { get { throw null; } set { } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        bool IList.IsFixedSize { get { throw null; } }

        bool IList.IsReadOnly { get { throw null; } }

        object? IList.this[int index] { get { throw null; } set { } }

        public ImmutableList<T> Add(T value) { throw null; }

        public ImmutableList<T> AddRange(Generic.IEnumerable<T> items) { throw null; }

        public int BinarySearch(T item, Generic.IComparer<T>? comparer) { throw null; }

        public int BinarySearch(T item) { throw null; }

        public int BinarySearch(int index, int count, T item, Generic.IComparer<T>? comparer) { throw null; }

        public ImmutableList<T> Clear() { throw null; }

        public bool Contains(T value) { throw null; }

        public ImmutableList<TOutput> ConvertAll<TOutput>(Func<T, TOutput> converter) { throw null; }

        public void CopyTo(T[] array, int arrayIndex) { }

        public void CopyTo(T[] array) { }

        public void CopyTo(int index, T[] array, int arrayIndex, int count) { }

        public bool Exists(Predicate<T> match) { throw null; }

        public T? Find(Predicate<T> match) { throw null; }

        public ImmutableList<T> FindAll(Predicate<T> match) { throw null; }

        public int FindIndex(int startIndex, int count, Predicate<T> match) { throw null; }

        public int FindIndex(int startIndex, Predicate<T> match) { throw null; }

        public int FindIndex(Predicate<T> match) { throw null; }

        public T? FindLast(Predicate<T> match) { throw null; }

        public int FindLastIndex(int startIndex, int count, Predicate<T> match) { throw null; }

        public int FindLastIndex(int startIndex, Predicate<T> match) { throw null; }

        public int FindLastIndex(Predicate<T> match) { throw null; }

        public void ForEach(Action<T> action) { }

        public Enumerator GetEnumerator() { throw null; }

        public ImmutableList<T> GetRange(int index, int count) { throw null; }

        public int IndexOf(T item, int index, int count, Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

        public int IndexOf(T value) { throw null; }

        public ImmutableList<T> Insert(int index, T item) { throw null; }

        public ImmutableList<T> InsertRange(int index, Generic.IEnumerable<T> items) { throw null; }

        public ref readonly T ItemRef(int index) { throw null; }

        public int LastIndexOf(T item, int index, int count, Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

        public ImmutableList<T> Remove(T value, Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

        public ImmutableList<T> Remove(T value) { throw null; }

        public ImmutableList<T> RemoveAll(Predicate<T> match) { throw null; }

        public ImmutableList<T> RemoveAt(int index) { throw null; }

        public ImmutableList<T> RemoveRange(Generic.IEnumerable<T> items, Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

        public ImmutableList<T> RemoveRange(Generic.IEnumerable<T> items) { throw null; }

        public ImmutableList<T> RemoveRange(int index, int count) { throw null; }

        public ImmutableList<T> Replace(T oldValue, T newValue, Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

        public ImmutableList<T> Replace(T oldValue, T newValue) { throw null; }

        public ImmutableList<T> Reverse() { throw null; }

        public ImmutableList<T> Reverse(int index, int count) { throw null; }

        public ImmutableList<T> SetItem(int index, T value) { throw null; }

        public ImmutableList<T> Sort() { throw null; }

        public ImmutableList<T> Sort(Generic.IComparer<T>? comparer) { throw null; }

        public ImmutableList<T> Sort(Comparison<T> comparison) { throw null; }

        public ImmutableList<T> Sort(int index, int count, Generic.IComparer<T>? comparer) { throw null; }

        void Generic.ICollection<T>.Add(T item) { }

        void Generic.ICollection<T>.Clear() { }

        bool Generic.ICollection<T>.Remove(T item) { throw null; }

        Generic.IEnumerator<T> Generic.IEnumerable<T>.GetEnumerator() { throw null; }

        void Generic.IList<T>.Insert(int index, T item) { }

        void Generic.IList<T>.RemoveAt(int index) { }

        void ICollection.CopyTo(Array array, int arrayIndex) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        int IList.Add(object value) { throw null; }

        void IList.Clear() { }

        bool IList.Contains(object value) { throw null; }

        int IList.IndexOf(object value) { throw null; }

        void IList.Insert(int index, object value) { }

        void IList.Remove(object value) { }

        void IList.RemoveAt(int index) { }

        IImmutableList<T> IImmutableList<T>.Add(T value) { throw null; }

        IImmutableList<T> IImmutableList<T>.AddRange(Generic.IEnumerable<T> items) { throw null; }

        IImmutableList<T> IImmutableList<T>.Clear() { throw null; }

        IImmutableList<T> IImmutableList<T>.Insert(int index, T item) { throw null; }

        IImmutableList<T> IImmutableList<T>.InsertRange(int index, Generic.IEnumerable<T> items) { throw null; }

        IImmutableList<T> IImmutableList<T>.Remove(T value, Generic.IEqualityComparer<T> equalityComparer) { throw null; }

        IImmutableList<T> IImmutableList<T>.RemoveAll(Predicate<T> match) { throw null; }

        IImmutableList<T> IImmutableList<T>.RemoveAt(int index) { throw null; }

        IImmutableList<T> IImmutableList<T>.RemoveRange(Generic.IEnumerable<T> items, Generic.IEqualityComparer<T> equalityComparer) { throw null; }

        IImmutableList<T> IImmutableList<T>.RemoveRange(int index, int count) { throw null; }

        IImmutableList<T> IImmutableList<T>.Replace(T oldValue, T newValue, Generic.IEqualityComparer<T> equalityComparer) { throw null; }

        IImmutableList<T> IImmutableList<T>.SetItem(int index, T value) { throw null; }

        public Builder ToBuilder() { throw null; }

        public bool TrueForAll(Predicate<T> match) { throw null; }

        public sealed partial class Builder : Generic.IList<T>, Generic.ICollection<T>, Generic.IEnumerable<T>, IEnumerable, IList, ICollection, Generic.IReadOnlyList<T>, Generic.IReadOnlyCollection<T>
        {
            internal Builder() { }

            public int Count { get { throw null; } }

            public T this[int index] { get { throw null; } set { } }

            bool Generic.ICollection<T>.IsReadOnly { get { throw null; } }

            bool ICollection.IsSynchronized { get { throw null; } }

            object ICollection.SyncRoot { get { throw null; } }

            bool IList.IsFixedSize { get { throw null; } }

            bool IList.IsReadOnly { get { throw null; } }

            object? IList.this[int index] { get { throw null; } set { } }

            public void Add(T item) { }

            public void AddRange(Generic.IEnumerable<T> items) { }

            public int BinarySearch(T item, Generic.IComparer<T>? comparer) { throw null; }

            public int BinarySearch(T item) { throw null; }

            public int BinarySearch(int index, int count, T item, Generic.IComparer<T>? comparer) { throw null; }

            public void Clear() { }

            public bool Contains(T item) { throw null; }

            public ImmutableList<TOutput> ConvertAll<TOutput>(Func<T, TOutput> converter) { throw null; }

            public void CopyTo(T[] array, int arrayIndex) { }

            public void CopyTo(T[] array) { }

            public void CopyTo(int index, T[] array, int arrayIndex, int count) { }

            public bool Exists(Predicate<T> match) { throw null; }

            public T? Find(Predicate<T> match) { throw null; }

            public ImmutableList<T> FindAll(Predicate<T> match) { throw null; }

            public int FindIndex(int startIndex, int count, Predicate<T> match) { throw null; }

            public int FindIndex(int startIndex, Predicate<T> match) { throw null; }

            public int FindIndex(Predicate<T> match) { throw null; }

            public T? FindLast(Predicate<T> match) { throw null; }

            public int FindLastIndex(int startIndex, int count, Predicate<T> match) { throw null; }

            public int FindLastIndex(int startIndex, Predicate<T> match) { throw null; }

            public int FindLastIndex(Predicate<T> match) { throw null; }

            public void ForEach(Action<T> action) { }

            public Enumerator GetEnumerator() { throw null; }

            public ImmutableList<T> GetRange(int index, int count) { throw null; }

            public int IndexOf(T item, int index, int count, Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

            public int IndexOf(T item, int index, int count) { throw null; }

            public int IndexOf(T item, int index) { throw null; }

            public int IndexOf(T item) { throw null; }

            public void Insert(int index, T item) { }

            public void InsertRange(int index, Generic.IEnumerable<T> items) { }

            public ref readonly T ItemRef(int index) { throw null; }

            public int LastIndexOf(T item, int startIndex, int count, Generic.IEqualityComparer<T>? equalityComparer) { throw null; }

            public int LastIndexOf(T item, int startIndex, int count) { throw null; }

            public int LastIndexOf(T item, int startIndex) { throw null; }

            public int LastIndexOf(T item) { throw null; }

            public bool Remove(T item) { throw null; }

            public int RemoveAll(Predicate<T> match) { throw null; }

            public void RemoveAt(int index) { }

            public void Reverse() { }

            public void Reverse(int index, int count) { }

            public void Sort() { }

            public void Sort(Generic.IComparer<T>? comparer) { }

            public void Sort(Comparison<T> comparison) { }

            public void Sort(int index, int count, Generic.IComparer<T>? comparer) { }

            Generic.IEnumerator<T> Generic.IEnumerable<T>.GetEnumerator() { throw null; }

            void ICollection.CopyTo(Array array, int arrayIndex) { }

            IEnumerator IEnumerable.GetEnumerator() { throw null; }

            int IList.Add(object value) { throw null; }

            void IList.Clear() { }

            bool IList.Contains(object value) { throw null; }

            int IList.IndexOf(object value) { throw null; }

            void IList.Insert(int index, object value) { }

            void IList.Remove(object value) { }

            public ImmutableList<T> ToImmutable() { throw null; }

            public bool TrueForAll(Predicate<T> match) { throw null; }
        }

        public partial struct Enumerator : Generic.IEnumerator<T>, IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public T Current { get { throw null; } }

            object? IEnumerator.Current { get { throw null; } }

            public void Dispose() { }

            public bool MoveNext() { throw null; }

            public void Reset() { }
        }
    }

    public static partial class ImmutableQueue
    {
        public static ImmutableQueue<T> Create<T>() { throw null; }

        public static ImmutableQueue<T> Create<T>(T item) { throw null; }

        public static ImmutableQueue<T> Create<T>(params T[] items) { throw null; }

        public static ImmutableQueue<T> CreateRange<T>(Generic.IEnumerable<T> items) { throw null; }

        public static IImmutableQueue<T> Dequeue<T>(this IImmutableQueue<T> queue, out T value) { throw null; }
    }

    public sealed partial class ImmutableQueue<T> : IImmutableQueue<T>, Generic.IEnumerable<T>, IEnumerable
    {
        internal ImmutableQueue() { }

        public static ImmutableQueue<T> Empty { get { throw null; } }

        public bool IsEmpty { get { throw null; } }

        public ImmutableQueue<T> Clear() { throw null; }

        public ImmutableQueue<T> Dequeue() { throw null; }

        public ImmutableQueue<T> Dequeue(out T value) { throw null; }

        public ImmutableQueue<T> Enqueue(T value) { throw null; }

        public Enumerator GetEnumerator() { throw null; }

        public T Peek() { throw null; }

        public ref readonly T PeekRef() { throw null; }

        Generic.IEnumerator<T> Generic.IEnumerable<T>.GetEnumerator() { throw null; }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        IImmutableQueue<T> IImmutableQueue<T>.Clear() { throw null; }

        IImmutableQueue<T> IImmutableQueue<T>.Dequeue() { throw null; }

        IImmutableQueue<T> IImmutableQueue<T>.Enqueue(T value) { throw null; }

        public partial struct Enumerator
        {
            private ImmutableQueue<T> _originalQueue;
            private ImmutableStack<T> _remainingForwardsStack;
            private ImmutableStack<T> _remainingBackwardsStack;
            private object _dummy;
            private int _dummyPrimitive;
            public T Current { get { throw null; } }

            public bool MoveNext() { throw null; }
        }
    }

    public static partial class ImmutableSortedDictionary
    {
        public static ImmutableSortedDictionary<TKey, TValue> Create<TKey, TValue>() { throw null; }

        public static ImmutableSortedDictionary<TKey, TValue> Create<TKey, TValue>(Generic.IComparer<TKey>? keyComparer, Generic.IEqualityComparer<TValue>? valueComparer) { throw null; }

        public static ImmutableSortedDictionary<TKey, TValue> Create<TKey, TValue>(Generic.IComparer<TKey>? keyComparer) { throw null; }

        public static ImmutableSortedDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>() { throw null; }

        public static ImmutableSortedDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>(Generic.IComparer<TKey>? keyComparer, Generic.IEqualityComparer<TValue>? valueComparer) { throw null; }

        public static ImmutableSortedDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>(Generic.IComparer<TKey>? keyComparer) { throw null; }

        public static ImmutableSortedDictionary<TKey, TValue> CreateRange<TKey, TValue>(Generic.IComparer<TKey>? keyComparer, Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> items) { throw null; }

        public static ImmutableSortedDictionary<TKey, TValue> CreateRange<TKey, TValue>(Generic.IComparer<TKey>? keyComparer, Generic.IEqualityComparer<TValue>? valueComparer, Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> items) { throw null; }

        public static ImmutableSortedDictionary<TKey, TValue> CreateRange<TKey, TValue>(Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> items) { throw null; }

        public static ImmutableSortedDictionary<TKey, TValue> ToImmutableSortedDictionary<TKey, TValue>(this Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> source, Generic.IComparer<TKey>? keyComparer, Generic.IEqualityComparer<TValue>? valueComparer) { throw null; }

        public static ImmutableSortedDictionary<TKey, TValue> ToImmutableSortedDictionary<TKey, TValue>(this Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> source, Generic.IComparer<TKey>? keyComparer) { throw null; }

        public static ImmutableSortedDictionary<TKey, TValue> ToImmutableSortedDictionary<TKey, TValue>(this Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> source) { throw null; }

        public static ImmutableSortedDictionary<TKey, TValue> ToImmutableSortedDictionary<TKey, TValue>(this ImmutableSortedDictionary<TKey, TValue>.Builder builder) { throw null; }

        public static ImmutableSortedDictionary<TKey, TValue> ToImmutableSortedDictionary<TSource, TKey, TValue>(this Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector, Generic.IComparer<TKey>? keyComparer, Generic.IEqualityComparer<TValue>? valueComparer) { throw null; }

        public static ImmutableSortedDictionary<TKey, TValue> ToImmutableSortedDictionary<TSource, TKey, TValue>(this Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector, Generic.IComparer<TKey>? keyComparer) { throw null; }

        public static ImmutableSortedDictionary<TKey, TValue> ToImmutableSortedDictionary<TSource, TKey, TValue>(this Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector) { throw null; }
    }

    public sealed partial class ImmutableSortedDictionary<TKey, TValue> : IImmutableDictionary<TKey, TValue>, Generic.IReadOnlyDictionary<TKey, TValue>, Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>>, IEnumerable, Generic.IReadOnlyCollection<Generic.KeyValuePair<TKey, TValue>>, Generic.IDictionary<TKey, TValue>, Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>, IDictionary, ICollection
    {
        internal ImmutableSortedDictionary() { }

        public static readonly ImmutableSortedDictionary<TKey, TValue> Empty;
        public int Count { get { throw null; } }

        public bool IsEmpty { get { throw null; } }

        public TValue this[TKey key] { get { throw null; } }

        public Generic.IComparer<TKey> KeyComparer { get { throw null; } }

        public Generic.IEnumerable<TKey> Keys { get { throw null; } }

        bool Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.IsReadOnly { get { throw null; } }

        TValue Generic.IDictionary<TKey, TValue>.this[TKey key] { get { throw null; } set { } }

        Generic.ICollection<TKey> Generic.IDictionary<TKey, TValue>.Keys { get { throw null; } }

        Generic.ICollection<TValue> Generic.IDictionary<TKey, TValue>.Values { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        bool IDictionary.IsFixedSize { get { throw null; } }

        bool IDictionary.IsReadOnly { get { throw null; } }

        object? IDictionary.this[object key] { get { throw null; } set { } }

        ICollection IDictionary.Keys { get { throw null; } }

        ICollection IDictionary.Values { get { throw null; } }

        public Generic.IEqualityComparer<TValue> ValueComparer { get { throw null; } }

        public Generic.IEnumerable<TValue> Values { get { throw null; } }

        public ImmutableSortedDictionary<TKey, TValue> Add(TKey key, TValue value) { throw null; }

        public ImmutableSortedDictionary<TKey, TValue> AddRange(Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> items) { throw null; }

        public ImmutableSortedDictionary<TKey, TValue> Clear() { throw null; }

        public bool Contains(Generic.KeyValuePair<TKey, TValue> pair) { throw null; }

        public bool ContainsKey(TKey key) { throw null; }

        public bool ContainsValue(TValue value) { throw null; }

        public Enumerator GetEnumerator() { throw null; }

        public ImmutableSortedDictionary<TKey, TValue> Remove(TKey value) { throw null; }

        public ImmutableSortedDictionary<TKey, TValue> RemoveRange(Generic.IEnumerable<TKey> keys) { throw null; }

        public ImmutableSortedDictionary<TKey, TValue> SetItem(TKey key, TValue value) { throw null; }

        public ImmutableSortedDictionary<TKey, TValue> SetItems(Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> items) { throw null; }

        void Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.Add(Generic.KeyValuePair<TKey, TValue> item) { }

        void Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.Clear() { }

        void Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.CopyTo(Generic.KeyValuePair<TKey, TValue>[] array, int arrayIndex) { }

        bool Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.Remove(Generic.KeyValuePair<TKey, TValue> item) { throw null; }

        void Generic.IDictionary<TKey, TValue>.Add(TKey key, TValue value) { }

        bool Generic.IDictionary<TKey, TValue>.Remove(TKey key) { throw null; }

        Generic.IEnumerator<Generic.KeyValuePair<TKey, TValue>> Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>>.GetEnumerator() { throw null; }

        void ICollection.CopyTo(Array array, int index) { }

        void IDictionary.Add(object key, object value) { }

        void IDictionary.Clear() { }

        bool IDictionary.Contains(object key) { throw null; }

        IDictionaryEnumerator IDictionary.GetEnumerator() { throw null; }

        void IDictionary.Remove(object key) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Add(TKey key, TValue value) { throw null; }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.AddRange(Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> pairs) { throw null; }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Clear() { throw null; }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Remove(TKey key) { throw null; }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.RemoveRange(Generic.IEnumerable<TKey> keys) { throw null; }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.SetItem(TKey key, TValue value) { throw null; }

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.SetItems(Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> items) { throw null; }

        public Builder ToBuilder() { throw null; }

        public bool TryGetKey(TKey equalKey, out TKey actualKey) { throw null; }

        public bool TryGetValue(TKey key, out TValue value) { throw null; }

        public ref readonly TValue ValueRef(TKey key) { throw null; }

        public ImmutableSortedDictionary<TKey, TValue> WithComparers(Generic.IComparer<TKey>? keyComparer, Generic.IEqualityComparer<TValue>? valueComparer) { throw null; }

        public ImmutableSortedDictionary<TKey, TValue> WithComparers(Generic.IComparer<TKey>? keyComparer) { throw null; }

        public sealed partial class Builder : Generic.IDictionary<TKey, TValue>, Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>, Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>>, IEnumerable, Generic.IReadOnlyDictionary<TKey, TValue>, Generic.IReadOnlyCollection<Generic.KeyValuePair<TKey, TValue>>, IDictionary, ICollection
        {
            internal Builder() { }

            public int Count { get { throw null; } }

            public TValue this[TKey key] { get { throw null; } set { } }

            public Generic.IComparer<TKey> KeyComparer { get { throw null; } set { } }

            public Generic.IEnumerable<TKey> Keys { get { throw null; } }

            bool Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.IsReadOnly { get { throw null; } }

            Generic.ICollection<TKey> Generic.IDictionary<TKey, TValue>.Keys { get { throw null; } }

            Generic.ICollection<TValue> Generic.IDictionary<TKey, TValue>.Values { get { throw null; } }

            bool ICollection.IsSynchronized { get { throw null; } }

            object ICollection.SyncRoot { get { throw null; } }

            bool IDictionary.IsFixedSize { get { throw null; } }

            bool IDictionary.IsReadOnly { get { throw null; } }

            object? IDictionary.this[object key] { get { throw null; } set { } }

            ICollection IDictionary.Keys { get { throw null; } }

            ICollection IDictionary.Values { get { throw null; } }

            public Generic.IEqualityComparer<TValue> ValueComparer { get { throw null; } set { } }

            public Generic.IEnumerable<TValue> Values { get { throw null; } }

            public void Add(TKey key, TValue value) { }

            public void Add(Generic.KeyValuePair<TKey, TValue> item) { }

            public void AddRange(Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> items) { }

            public void Clear() { }

            public bool Contains(Generic.KeyValuePair<TKey, TValue> item) { throw null; }

            public bool ContainsKey(TKey key) { throw null; }

            public bool ContainsValue(TValue value) { throw null; }

            public Enumerator GetEnumerator() { throw null; }

            public TValue GetValueOrDefault(TKey key, TValue defaultValue) { throw null; }

            public TValue? GetValueOrDefault(TKey key) { throw null; }

            public bool Remove(TKey key) { throw null; }

            public bool Remove(Generic.KeyValuePair<TKey, TValue> item) { throw null; }

            public void RemoveRange(Generic.IEnumerable<TKey> keys) { }

            void Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.CopyTo(Generic.KeyValuePair<TKey, TValue>[] array, int arrayIndex) { }

            Generic.IEnumerator<Generic.KeyValuePair<TKey, TValue>> Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>>.GetEnumerator() { throw null; }

            void ICollection.CopyTo(Array array, int index) { }

            void IDictionary.Add(object key, object value) { }

            bool IDictionary.Contains(object key) { throw null; }

            IDictionaryEnumerator IDictionary.GetEnumerator() { throw null; }

            void IDictionary.Remove(object key) { }

            IEnumerator IEnumerable.GetEnumerator() { throw null; }

            public ImmutableSortedDictionary<TKey, TValue> ToImmutable() { throw null; }

            public bool TryGetKey(TKey equalKey, out TKey actualKey) { throw null; }

            public bool TryGetValue(TKey key, out TValue value) { throw null; }

            public ref readonly TValue ValueRef(TKey key) { throw null; }
        }

        public partial struct Enumerator : Generic.IEnumerator<Generic.KeyValuePair<TKey, TValue>>, IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public Generic.KeyValuePair<TKey, TValue> Current { get { throw null; } }

            object IEnumerator.Current { get { throw null; } }

            public void Dispose() { }

            public bool MoveNext() { throw null; }

            public void Reset() { }
        }
    }

    public static partial class ImmutableSortedSet
    {
        public static ImmutableSortedSet<T> Create<T>() { throw null; }

        public static ImmutableSortedSet<T> Create<T>(T item) { throw null; }

        public static ImmutableSortedSet<T> Create<T>(params T[] items) { throw null; }

        public static ImmutableSortedSet<T> Create<T>(Generic.IComparer<T>? comparer, T item) { throw null; }

        public static ImmutableSortedSet<T> Create<T>(Generic.IComparer<T>? comparer, params T[] items) { throw null; }

        public static ImmutableSortedSet<T> Create<T>(Generic.IComparer<T>? comparer) { throw null; }

        public static ImmutableSortedSet<T>.Builder CreateBuilder<T>() { throw null; }

        public static ImmutableSortedSet<T>.Builder CreateBuilder<T>(Generic.IComparer<T>? comparer) { throw null; }

        public static ImmutableSortedSet<T> CreateRange<T>(Generic.IComparer<T>? comparer, Generic.IEnumerable<T> items) { throw null; }

        public static ImmutableSortedSet<T> CreateRange<T>(Generic.IEnumerable<T> items) { throw null; }

        public static ImmutableSortedSet<TSource> ToImmutableSortedSet<TSource>(this Generic.IEnumerable<TSource> source, Generic.IComparer<TSource>? comparer) { throw null; }

        public static ImmutableSortedSet<TSource> ToImmutableSortedSet<TSource>(this Generic.IEnumerable<TSource> source) { throw null; }

        public static ImmutableSortedSet<TSource> ToImmutableSortedSet<TSource>(this ImmutableSortedSet<TSource>.Builder builder) { throw null; }
    }

    public sealed partial class ImmutableSortedSet<T> : IImmutableSet<T>, Generic.IReadOnlyCollection<T>, Generic.IEnumerable<T>, IEnumerable, Generic.IReadOnlyList<T>, Generic.IList<T>, Generic.ICollection<T>, Generic.ISet<T>, IList, ICollection
    {
        internal ImmutableSortedSet() { }

        public static readonly ImmutableSortedSet<T> Empty;
        public int Count { get { throw null; } }

        public bool IsEmpty { get { throw null; } }

        public T this[int index] { get { throw null; } }

        public Generic.IComparer<T> KeyComparer { get { throw null; } }

        public T? Max { get { throw null; } }

        public T? Min { get { throw null; } }

        bool Generic.ICollection<T>.IsReadOnly { get { throw null; } }

        T Generic.IList<T>.this[int index] { get { throw null; } set { } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        bool IList.IsFixedSize { get { throw null; } }

        bool IList.IsReadOnly { get { throw null; } }

        object? IList.this[int index] { get { throw null; } set { } }

        public ImmutableSortedSet<T> Add(T value) { throw null; }

        public ImmutableSortedSet<T> Clear() { throw null; }

        public bool Contains(T value) { throw null; }

        public ImmutableSortedSet<T> Except(Generic.IEnumerable<T> other) { throw null; }

        public Enumerator GetEnumerator() { throw null; }

        public int IndexOf(T item) { throw null; }

        public ImmutableSortedSet<T> Intersect(Generic.IEnumerable<T> other) { throw null; }

        public bool IsProperSubsetOf(Generic.IEnumerable<T> other) { throw null; }

        public bool IsProperSupersetOf(Generic.IEnumerable<T> other) { throw null; }

        public bool IsSubsetOf(Generic.IEnumerable<T> other) { throw null; }

        public bool IsSupersetOf(Generic.IEnumerable<T> other) { throw null; }

        public ref readonly T ItemRef(int index) { throw null; }

        public bool Overlaps(Generic.IEnumerable<T> other) { throw null; }

        public ImmutableSortedSet<T> Remove(T value) { throw null; }

        public Generic.IEnumerable<T> Reverse() { throw null; }

        public bool SetEquals(Generic.IEnumerable<T> other) { throw null; }

        public ImmutableSortedSet<T> SymmetricExcept(Generic.IEnumerable<T> other) { throw null; }

        void Generic.ICollection<T>.Add(T item) { }

        void Generic.ICollection<T>.Clear() { }

        void Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex) { }

        bool Generic.ICollection<T>.Remove(T item) { throw null; }

        Generic.IEnumerator<T> Generic.IEnumerable<T>.GetEnumerator() { throw null; }

        void Generic.IList<T>.Insert(int index, T item) { }

        void Generic.IList<T>.RemoveAt(int index) { }

        bool Generic.ISet<T>.Add(T item) { throw null; }

        void Generic.ISet<T>.ExceptWith(Generic.IEnumerable<T> other) { }

        void Generic.ISet<T>.IntersectWith(Generic.IEnumerable<T> other) { }

        void Generic.ISet<T>.SymmetricExceptWith(Generic.IEnumerable<T> other) { }

        void Generic.ISet<T>.UnionWith(Generic.IEnumerable<T> other) { }

        void ICollection.CopyTo(Array array, int index) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        int IList.Add(object value) { throw null; }

        void IList.Clear() { }

        bool IList.Contains(object value) { throw null; }

        int IList.IndexOf(object value) { throw null; }

        void IList.Insert(int index, object value) { }

        void IList.Remove(object value) { }

        void IList.RemoveAt(int index) { }

        IImmutableSet<T> IImmutableSet<T>.Add(T value) { throw null; }

        IImmutableSet<T> IImmutableSet<T>.Clear() { throw null; }

        IImmutableSet<T> IImmutableSet<T>.Except(Generic.IEnumerable<T> other) { throw null; }

        IImmutableSet<T> IImmutableSet<T>.Intersect(Generic.IEnumerable<T> other) { throw null; }

        IImmutableSet<T> IImmutableSet<T>.Remove(T value) { throw null; }

        IImmutableSet<T> IImmutableSet<T>.SymmetricExcept(Generic.IEnumerable<T> other) { throw null; }

        IImmutableSet<T> IImmutableSet<T>.Union(Generic.IEnumerable<T> other) { throw null; }

        public Builder ToBuilder() { throw null; }

        public bool TryGetValue(T equalValue, out T actualValue) { throw null; }

        public ImmutableSortedSet<T> Union(Generic.IEnumerable<T> other) { throw null; }

        public ImmutableSortedSet<T> WithComparer(Generic.IComparer<T>? comparer) { throw null; }

        public sealed partial class Builder : Generic.IReadOnlyCollection<T>, Generic.IEnumerable<T>, IEnumerable, Generic.ISet<T>, Generic.ICollection<T>, ICollection
        {
            internal Builder() { }

            public int Count { get { throw null; } }

            public T this[int index] { get { throw null; } }

            public Generic.IComparer<T> KeyComparer { get { throw null; } set { } }

            public T? Max { get { throw null; } }

            public T? Min { get { throw null; } }

            bool Generic.ICollection<T>.IsReadOnly { get { throw null; } }

            bool ICollection.IsSynchronized { get { throw null; } }

            object ICollection.SyncRoot { get { throw null; } }

            public bool Add(T item) { throw null; }

            public void Clear() { }

            public bool Contains(T item) { throw null; }

            public void ExceptWith(Generic.IEnumerable<T> other) { }

            public Enumerator GetEnumerator() { throw null; }

            public void IntersectWith(Generic.IEnumerable<T> other) { }

            public bool IsProperSubsetOf(Generic.IEnumerable<T> other) { throw null; }

            public bool IsProperSupersetOf(Generic.IEnumerable<T> other) { throw null; }

            public bool IsSubsetOf(Generic.IEnumerable<T> other) { throw null; }

            public bool IsSupersetOf(Generic.IEnumerable<T> other) { throw null; }

            public ref readonly T ItemRef(int index) { throw null; }

            public bool Overlaps(Generic.IEnumerable<T> other) { throw null; }

            public bool Remove(T item) { throw null; }

            public Generic.IEnumerable<T> Reverse() { throw null; }

            public bool SetEquals(Generic.IEnumerable<T> other) { throw null; }

            public void SymmetricExceptWith(Generic.IEnumerable<T> other) { }

            void Generic.ICollection<T>.Add(T item) { }

            void Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex) { }

            Generic.IEnumerator<T> Generic.IEnumerable<T>.GetEnumerator() { throw null; }

            void ICollection.CopyTo(Array array, int arrayIndex) { }

            IEnumerator IEnumerable.GetEnumerator() { throw null; }

            public ImmutableSortedSet<T> ToImmutable() { throw null; }

            public bool TryGetValue(T equalValue, out T actualValue) { throw null; }

            public void UnionWith(Generic.IEnumerable<T> other) { }
        }

        public partial struct Enumerator : Generic.IEnumerator<T>, IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public T Current { get { throw null; } }

            object? IEnumerator.Current { get { throw null; } }

            public void Dispose() { }

            public bool MoveNext() { throw null; }

            public void Reset() { }
        }
    }

    public static partial class ImmutableStack
    {
        public static ImmutableStack<T> Create<T>() { throw null; }

        public static ImmutableStack<T> Create<T>(T item) { throw null; }

        public static ImmutableStack<T> Create<T>(params T[] items) { throw null; }

        public static ImmutableStack<T> CreateRange<T>(Generic.IEnumerable<T> items) { throw null; }

        public static IImmutableStack<T> Pop<T>(this IImmutableStack<T> stack, out T value) { throw null; }
    }

    public sealed partial class ImmutableStack<T> : IImmutableStack<T>, Generic.IEnumerable<T>, IEnumerable
    {
        internal ImmutableStack() { }

        public static ImmutableStack<T> Empty { get { throw null; } }

        public bool IsEmpty { get { throw null; } }

        public ImmutableStack<T> Clear() { throw null; }

        public Enumerator GetEnumerator() { throw null; }

        public T Peek() { throw null; }

        public ref readonly T PeekRef() { throw null; }

        public ImmutableStack<T> Pop() { throw null; }

        public ImmutableStack<T> Pop(out T value) { throw null; }

        public ImmutableStack<T> Push(T value) { throw null; }

        Generic.IEnumerator<T> Generic.IEnumerable<T>.GetEnumerator() { throw null; }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        IImmutableStack<T> IImmutableStack<T>.Clear() { throw null; }

        IImmutableStack<T> IImmutableStack<T>.Pop() { throw null; }

        IImmutableStack<T> IImmutableStack<T>.Push(T value) { throw null; }

        public partial struct Enumerator
        {
            private ImmutableStack<T> _originalStack;
            private ImmutableStack<T> _remainingStack;
            private object _dummy;
            private int _dummyPrimitive;
            public T Current { get { throw null; } }

            public bool MoveNext() { throw null; }
        }
    }
}

namespace System.Linq
{
    public static partial class ImmutableArrayExtensions
    {
        public static T? Aggregate<T>(this Collections.Immutable.ImmutableArray<T> immutableArray, Func<T, T, T> func) { throw null; }

        public static TAccumulate Aggregate<TAccumulate, T>(this Collections.Immutable.ImmutableArray<T> immutableArray, TAccumulate seed, Func<TAccumulate, T, TAccumulate> func) { throw null; }

        public static TResult Aggregate<TAccumulate, TResult, T>(this Collections.Immutable.ImmutableArray<T> immutableArray, TAccumulate seed, Func<TAccumulate, T, TAccumulate> func, Func<TAccumulate, TResult> resultSelector) { throw null; }

        public static bool All<T>(this Collections.Immutable.ImmutableArray<T> immutableArray, Func<T, bool> predicate) { throw null; }

        public static bool Any<T>(this Collections.Immutable.ImmutableArray<T> immutableArray, Func<T, bool> predicate) { throw null; }

        public static bool Any<T>(this Collections.Immutable.ImmutableArray<T>.Builder builder) { throw null; }

        public static bool Any<T>(this Collections.Immutable.ImmutableArray<T> immutableArray) { throw null; }

        public static T ElementAt<T>(this Collections.Immutable.ImmutableArray<T> immutableArray, int index) { throw null; }

        public static T? ElementAtOrDefault<T>(this Collections.Immutable.ImmutableArray<T> immutableArray, int index) { throw null; }

        public static T First<T>(this Collections.Immutable.ImmutableArray<T> immutableArray, Func<T, bool> predicate) { throw null; }

        public static T First<T>(this Collections.Immutable.ImmutableArray<T>.Builder builder) { throw null; }

        public static T First<T>(this Collections.Immutable.ImmutableArray<T> immutableArray) { throw null; }

        public static T? FirstOrDefault<T>(this Collections.Immutable.ImmutableArray<T> immutableArray, Func<T, bool> predicate) { throw null; }

        public static T? FirstOrDefault<T>(this Collections.Immutable.ImmutableArray<T>.Builder builder) { throw null; }

        public static T? FirstOrDefault<T>(this Collections.Immutable.ImmutableArray<T> immutableArray) { throw null; }

        public static T Last<T>(this Collections.Immutable.ImmutableArray<T> immutableArray, Func<T, bool> predicate) { throw null; }

        public static T Last<T>(this Collections.Immutable.ImmutableArray<T>.Builder builder) { throw null; }

        public static T Last<T>(this Collections.Immutable.ImmutableArray<T> immutableArray) { throw null; }

        public static T? LastOrDefault<T>(this Collections.Immutable.ImmutableArray<T> immutableArray, Func<T, bool> predicate) { throw null; }

        public static T? LastOrDefault<T>(this Collections.Immutable.ImmutableArray<T>.Builder builder) { throw null; }

        public static T? LastOrDefault<T>(this Collections.Immutable.ImmutableArray<T> immutableArray) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> Select<T, TResult>(this Collections.Immutable.ImmutableArray<T> immutableArray, Func<T, TResult> selector) { throw null; }

        public static Collections.Generic.IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this Collections.Immutable.ImmutableArray<TSource> immutableArray, Func<TSource, Collections.Generic.IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) { throw null; }

        public static bool SequenceEqual<TDerived, TBase>(this Collections.Immutable.ImmutableArray<TBase> immutableArray, Collections.Generic.IEnumerable<TDerived> items, Collections.Generic.IEqualityComparer<TBase>? comparer = null)
            where TDerived : TBase { throw null; }

        public static bool SequenceEqual<TDerived, TBase>(this Collections.Immutable.ImmutableArray<TBase> immutableArray, Collections.Immutable.ImmutableArray<TDerived> items, Collections.Generic.IEqualityComparer<TBase>? comparer = null)
            where TDerived : TBase { throw null; }

        public static bool SequenceEqual<TDerived, TBase>(this Collections.Immutable.ImmutableArray<TBase> immutableArray, Collections.Immutable.ImmutableArray<TDerived> items, Func<TBase, TBase, bool> predicate)
            where TDerived : TBase { throw null; }

        public static T Single<T>(this Collections.Immutable.ImmutableArray<T> immutableArray, Func<T, bool> predicate) { throw null; }

        public static T Single<T>(this Collections.Immutable.ImmutableArray<T> immutableArray) { throw null; }

        public static T? SingleOrDefault<T>(this Collections.Immutable.ImmutableArray<T> immutableArray, Func<T, bool> predicate) { throw null; }

        public static T? SingleOrDefault<T>(this Collections.Immutable.ImmutableArray<T> immutableArray) { throw null; }

        public static T[] ToArray<T>(this Collections.Immutable.ImmutableArray<T> immutableArray) { throw null; }

        public static Collections.Generic.Dictionary<TKey, T> ToDictionary<TKey, T>(this Collections.Immutable.ImmutableArray<T> immutableArray, Func<T, TKey> keySelector, Collections.Generic.IEqualityComparer<TKey>? comparer) { throw null; }

        public static Collections.Generic.Dictionary<TKey, T> ToDictionary<TKey, T>(this Collections.Immutable.ImmutableArray<T> immutableArray, Func<T, TKey> keySelector) { throw null; }

        public static Collections.Generic.Dictionary<TKey, TElement> ToDictionary<TKey, TElement, T>(this Collections.Immutable.ImmutableArray<T> immutableArray, Func<T, TKey> keySelector, Func<T, TElement> elementSelector, Collections.Generic.IEqualityComparer<TKey>? comparer) { throw null; }

        public static Collections.Generic.Dictionary<TKey, TElement> ToDictionary<TKey, TElement, T>(this Collections.Immutable.ImmutableArray<T> immutableArray, Func<T, TKey> keySelector, Func<T, TElement> elementSelector) { throw null; }

        public static Collections.Generic.IEnumerable<T> Where<T>(this Collections.Immutable.ImmutableArray<T> immutableArray, Func<T, bool> predicate) { throw null; }
    }
}