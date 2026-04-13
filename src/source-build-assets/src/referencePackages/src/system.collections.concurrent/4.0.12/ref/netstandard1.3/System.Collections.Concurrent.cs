// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyTitle("System.Collections.Concurrent")]
[assembly: System.Reflection.AssemblyDescription("System.Collections.Concurrent")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Collections.Concurrent")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.10.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Collections.Concurrent
{
    public partial class BlockingCollection<T> : Generic.IEnumerable<T>, IEnumerable, Generic.IReadOnlyCollection<T>, ICollection, IDisposable
    {
        public BlockingCollection() { }

        public BlockingCollection(IProducerConsumerCollection<T> collection, int boundedCapacity) { }

        public BlockingCollection(IProducerConsumerCollection<T> collection) { }

        public BlockingCollection(int boundedCapacity) { }

        public int BoundedCapacity { get { throw null; } }

        public int Count { get { throw null; } }

        public bool IsAddingCompleted { get { throw null; } }

        public bool IsCompleted { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        public void Add(T item, Threading.CancellationToken cancellationToken) { }

        public void Add(T item) { }

        public static int AddToAny(BlockingCollection<T>[] collections, T item, Threading.CancellationToken cancellationToken) { throw null; }

        public static int AddToAny(BlockingCollection<T>[] collections, T item) { throw null; }

        public void CompleteAdding() { }

        public void CopyTo(T[] array, int index) { }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public Generic.IEnumerable<T> GetConsumingEnumerable() { throw null; }

        public Generic.IEnumerable<T> GetConsumingEnumerable(Threading.CancellationToken cancellationToken) { throw null; }

        Generic.IEnumerator<T> Generic.IEnumerable<T>.GetEnumerator() { throw null; }

        void ICollection.CopyTo(Array array, int index) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        public T Take() { throw null; }

        public T Take(Threading.CancellationToken cancellationToken) { throw null; }

        public static int TakeFromAny(BlockingCollection<T>[] collections, out T item, Threading.CancellationToken cancellationToken) { throw null; }

        public static int TakeFromAny(BlockingCollection<T>[] collections, out T item) { throw null; }

        public T[] ToArray() { throw null; }

        public bool TryAdd(T item, int millisecondsTimeout, Threading.CancellationToken cancellationToken) { throw null; }

        public bool TryAdd(T item, int millisecondsTimeout) { throw null; }

        public bool TryAdd(T item, TimeSpan timeout) { throw null; }

        public bool TryAdd(T item) { throw null; }

        public static int TryAddToAny(BlockingCollection<T>[] collections, T item, int millisecondsTimeout, Threading.CancellationToken cancellationToken) { throw null; }

        public static int TryAddToAny(BlockingCollection<T>[] collections, T item, int millisecondsTimeout) { throw null; }

        public static int TryAddToAny(BlockingCollection<T>[] collections, T item, TimeSpan timeout) { throw null; }

        public static int TryAddToAny(BlockingCollection<T>[] collections, T item) { throw null; }

        public bool TryTake(out T item, int millisecondsTimeout, Threading.CancellationToken cancellationToken) { throw null; }

        public bool TryTake(out T item, int millisecondsTimeout) { throw null; }

        public bool TryTake(out T item, TimeSpan timeout) { throw null; }

        public bool TryTake(out T item) { throw null; }

        public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T item, int millisecondsTimeout, Threading.CancellationToken cancellationToken) { throw null; }

        public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T item, int millisecondsTimeout) { throw null; }

        public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T item, TimeSpan timeout) { throw null; }

        public static int TryTakeFromAny(BlockingCollection<T>[] collections, out T item) { throw null; }
    }

    public partial class ConcurrentBag<T> : IProducerConsumerCollection<T>, Generic.IEnumerable<T>, IEnumerable, ICollection, Generic.IReadOnlyCollection<T>
    {
        public ConcurrentBag() { }

        public ConcurrentBag(Generic.IEnumerable<T> collection) { }

        public int Count { get { throw null; } }

        public bool IsEmpty { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        public void Add(T item) { }

        public void CopyTo(T[] array, int index) { }

        public Generic.IEnumerator<T> GetEnumerator() { throw null; }

        bool IProducerConsumerCollection<T>.TryAdd(T item) { throw null; }

        void ICollection.CopyTo(Array array, int index) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        public T[] ToArray() { throw null; }

        public bool TryPeek(out T result) { throw null; }

        public bool TryTake(out T result) { throw null; }
    }

    public partial class ConcurrentDictionary<TKey, TValue> : Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>, Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>>, IEnumerable, Generic.IDictionary<TKey, TValue>, Generic.IReadOnlyCollection<Generic.KeyValuePair<TKey, TValue>>, Generic.IReadOnlyDictionary<TKey, TValue>, ICollection, IDictionary
    {
        public ConcurrentDictionary() { }

        public ConcurrentDictionary(Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> collection, Generic.IEqualityComparer<TKey> comparer) { }

        public ConcurrentDictionary(Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> collection) { }

        public ConcurrentDictionary(Generic.IEqualityComparer<TKey> comparer) { }

        public ConcurrentDictionary(int concurrencyLevel, Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>> collection, Generic.IEqualityComparer<TKey> comparer) { }

        public ConcurrentDictionary(int concurrencyLevel, int capacity, Generic.IEqualityComparer<TKey> comparer) { }

        public ConcurrentDictionary(int concurrencyLevel, int capacity) { }

        public int Count { get { throw null; } }

        public bool IsEmpty { get { throw null; } }

        public TValue this[TKey key] { get { throw null; } set { } }

        public Generic.ICollection<TKey> Keys { get { throw null; } }

        bool Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.IsReadOnly { get { throw null; } }

        Generic.IEnumerable<TKey> Generic.IReadOnlyDictionary<TKey, TValue>.Keys { get { throw null; } }

        Generic.IEnumerable<TValue> Generic.IReadOnlyDictionary<TKey, TValue>.Values { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        bool IDictionary.IsFixedSize { get { throw null; } }

        bool IDictionary.IsReadOnly { get { throw null; } }

        object IDictionary.this[object key] { get { throw null; } set { } }

        ICollection IDictionary.Keys { get { throw null; } }

        ICollection IDictionary.Values { get { throw null; } }

        public Generic.ICollection<TValue> Values { get { throw null; } }

        public TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory) { throw null; }

        public TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory) { throw null; }

        public void Clear() { }

        public bool ContainsKey(TKey key) { throw null; }

        public Generic.IEnumerator<Generic.KeyValuePair<TKey, TValue>> GetEnumerator() { throw null; }

        public TValue GetOrAdd(TKey key, TValue value) { throw null; }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory) { throw null; }

        void Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.Add(Generic.KeyValuePair<TKey, TValue> keyValuePair) { }

        bool Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.Contains(Generic.KeyValuePair<TKey, TValue> keyValuePair) { throw null; }

        void Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.CopyTo(Generic.KeyValuePair<TKey, TValue>[] array, int index) { }

        bool Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.Remove(Generic.KeyValuePair<TKey, TValue> keyValuePair) { throw null; }

        void Generic.IDictionary<TKey, TValue>.Add(TKey key, TValue value) { }

        bool Generic.IDictionary<TKey, TValue>.Remove(TKey key) { throw null; }

        void ICollection.CopyTo(Array array, int index) { }

        void IDictionary.Add(object key, object value) { }

        bool IDictionary.Contains(object key) { throw null; }

        IDictionaryEnumerator IDictionary.GetEnumerator() { throw null; }

        void IDictionary.Remove(object key) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        public Generic.KeyValuePair<TKey, TValue>[] ToArray() { throw null; }

        public bool TryAdd(TKey key, TValue value) { throw null; }

        public bool TryGetValue(TKey key, out TValue value) { throw null; }

        public bool TryRemove(TKey key, out TValue value) { throw null; }

        public bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue) { throw null; }
    }

    public partial class ConcurrentQueue<T> : IProducerConsumerCollection<T>, Generic.IEnumerable<T>, IEnumerable, ICollection, Generic.IReadOnlyCollection<T>
    {
        public ConcurrentQueue() { }

        public ConcurrentQueue(Generic.IEnumerable<T> collection) { }

        public int Count { get { throw null; } }

        public bool IsEmpty { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        public void CopyTo(T[] array, int index) { }

        public void Enqueue(T item) { }

        public Generic.IEnumerator<T> GetEnumerator() { throw null; }

        bool IProducerConsumerCollection<T>.TryAdd(T item) { throw null; }

        bool IProducerConsumerCollection<T>.TryTake(out T item) { throw null; }

        void ICollection.CopyTo(Array array, int index) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        public T[] ToArray() { throw null; }

        public bool TryDequeue(out T result) { throw null; }

        public bool TryPeek(out T result) { throw null; }
    }

    public partial class ConcurrentStack<T> : IProducerConsumerCollection<T>, Generic.IEnumerable<T>, IEnumerable, ICollection, Generic.IReadOnlyCollection<T>
    {
        public ConcurrentStack() { }

        public ConcurrentStack(Generic.IEnumerable<T> collection) { }

        public int Count { get { throw null; } }

        public bool IsEmpty { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        public void Clear() { }

        public void CopyTo(T[] array, int index) { }

        public Generic.IEnumerator<T> GetEnumerator() { throw null; }

        public void Push(T item) { }

        public void PushRange(T[] items, int startIndex, int count) { }

        public void PushRange(T[] items) { }

        bool IProducerConsumerCollection<T>.TryAdd(T item) { throw null; }

        bool IProducerConsumerCollection<T>.TryTake(out T item) { throw null; }

        void ICollection.CopyTo(Array array, int index) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        public T[] ToArray() { throw null; }

        public bool TryPeek(out T result) { throw null; }

        public bool TryPop(out T result) { throw null; }

        public int TryPopRange(T[] items, int startIndex, int count) { throw null; }

        public int TryPopRange(T[] items) { throw null; }
    }

    [Flags]
    public enum EnumerablePartitionerOptions
    {
        None = 0,
        NoBuffering = 1
    }

    public partial interface IProducerConsumerCollection<T> : Generic.IEnumerable<T>, IEnumerable, ICollection
    {
        void CopyTo(T[] array, int index);
        T[] ToArray();
        bool TryAdd(T item);
        bool TryTake(out T item);
    }

    public abstract partial class OrderablePartitioner<TSource> : Partitioner<TSource>
    {
        protected OrderablePartitioner(bool keysOrderedInEachPartition, bool keysOrderedAcrossPartitions, bool keysNormalized) { }

        public bool KeysNormalized { get { throw null; } }

        public bool KeysOrderedAcrossPartitions { get { throw null; } }

        public bool KeysOrderedInEachPartition { get { throw null; } }

        public override Generic.IEnumerable<TSource> GetDynamicPartitions() { throw null; }

        public virtual Generic.IEnumerable<Generic.KeyValuePair<long, TSource>> GetOrderableDynamicPartitions() { throw null; }

        public abstract Generic.IList<Generic.IEnumerator<Generic.KeyValuePair<long, TSource>>> GetOrderablePartitions(int partitionCount);
        public override Generic.IList<Generic.IEnumerator<TSource>> GetPartitions(int partitionCount) { throw null; }
    }

    public static partial class Partitioner
    {
        public static OrderablePartitioner<Tuple<int, int>> Create(int fromInclusive, int toExclusive, int rangeSize) { throw null; }

        public static OrderablePartitioner<Tuple<int, int>> Create(int fromInclusive, int toExclusive) { throw null; }

        public static OrderablePartitioner<Tuple<long, long>> Create(long fromInclusive, long toExclusive, long rangeSize) { throw null; }

        public static OrderablePartitioner<Tuple<long, long>> Create(long fromInclusive, long toExclusive) { throw null; }

        public static OrderablePartitioner<TSource> Create<TSource>(TSource[] array, bool loadBalance) { throw null; }

        public static OrderablePartitioner<TSource> Create<TSource>(Generic.IEnumerable<TSource> source, EnumerablePartitionerOptions partitionerOptions) { throw null; }

        public static OrderablePartitioner<TSource> Create<TSource>(Generic.IEnumerable<TSource> source) { throw null; }

        public static OrderablePartitioner<TSource> Create<TSource>(Generic.IList<TSource> list, bool loadBalance) { throw null; }
    }

    public abstract partial class Partitioner<TSource>
    {
        public virtual bool SupportsDynamicPartitions { get { throw null; } }

        public virtual Generic.IEnumerable<TSource> GetDynamicPartitions() { throw null; }

        public abstract Generic.IList<Generic.IEnumerator<TSource>> GetPartitions(int partitionCount);
    }
}