// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Reflection.AssemblyTitle("System.Collections.dll")]
[assembly: System.Reflection.AssemblyDescription("System.Collections.dll")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Collections.dll")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.0.30319.17929")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.0.30319.17929")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Collections
{
    public sealed partial class BitArray : ICollection, IEnumerable
    {
        public BitArray(bool[] values) { }

        public BitArray(byte[] bytes) { }

        public BitArray(BitArray bits) { }

        public BitArray(int length, bool defaultValue) { }

        public BitArray(int length) { }

        public BitArray(int[] values) { }

        public bool this[int index] { get { throw null; } set { } }

        public int Length { get { throw null; } set { } }

        int ICollection.Count { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        public BitArray And(BitArray value) { throw null; }

        public bool Get(int index) { throw null; }

        public IEnumerator GetEnumerator() { throw null; }

        public BitArray Not() { throw null; }

        public BitArray Or(BitArray value) { throw null; }

        public void Set(int index, bool value) { }

        public void SetAll(bool value) { }

        void ICollection.CopyTo(Array array, int index) { }

        public BitArray Xor(BitArray value) { throw null; }
    }

    public static partial class StructuralComparisons
    {
        public static IComparer StructuralComparer { get { throw null; } }

        public static IEqualityComparer StructuralEqualityComparer { get { throw null; } }
    }
}

namespace System.Collections.Generic
{
    public abstract partial class Comparer<T> : IComparer<T>, IComparer
    {
        public static Comparer<T> Default { get { throw null; } }

        public abstract int Compare(T x, T y);
        public static Comparer<T> Create(Comparison<T> comparison) { throw null; }

        int IComparer.Compare(object x, object y) { throw null; }
    }

    public partial class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, ICollection, IEnumerable
    {
        public Dictionary() { }

        public Dictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) { }

        public Dictionary(IDictionary<TKey, TValue> dictionary) { }

        public Dictionary(IEqualityComparer<TKey> comparer) { }

        public Dictionary(int capacity, IEqualityComparer<TKey> comparer) { }

        public Dictionary(int capacity) { }

        public IEqualityComparer<TKey> Comparer { get { throw null; } }

        public int Count { get { throw null; } }

        public TValue this[TKey key] { get { throw null; } set { } }

        public KeyCollection Keys { get { throw null; } }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly { get { throw null; } }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys { get { throw null; } }

        ICollection<TValue> IDictionary<TKey, TValue>.Values { get { throw null; } }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys { get { throw null; } }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        bool IDictionary.IsFixedSize { get { throw null; } }

        bool IDictionary.IsReadOnly { get { throw null; } }

        object IDictionary.this[object key] { get { throw null; } set { } }

        ICollection IDictionary.Keys { get { throw null; } }

        ICollection IDictionary.Values { get { throw null; } }

        public ValueCollection Values { get { throw null; } }

        public void Add(TKey key, TValue value) { }

        public void Clear() { }

        public bool ContainsKey(TKey key) { throw null; }

        public bool ContainsValue(TValue value) { throw null; }

        public Enumerator GetEnumerator() { throw null; }

        public bool Remove(TKey key) { throw null; }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair) { }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair) { throw null; }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index) { }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair) { throw null; }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() { throw null; }

        void ICollection.CopyTo(Array array, int index) { }

        void IDictionary.Add(object key, object value) { }

        bool IDictionary.Contains(object key) { throw null; }

        IDictionaryEnumerator IDictionary.GetEnumerator() { throw null; }

        void IDictionary.Remove(object key) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        public bool TryGetValue(TKey key, out TValue value) { throw null; }

        public partial struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator, IEnumerator, IDisposable
        {
            public KeyValuePair<TKey, TValue> Current { get { throw null; } }

            DictionaryEntry IDictionaryEnumerator.Entry { get { throw null; } }

            object IDictionaryEnumerator.Key { get { throw null; } }

            object IDictionaryEnumerator.Value { get { throw null; } }

            object IEnumerator.Current { get { throw null; } }

            public void Dispose() { }

            public bool MoveNext() { throw null; }

            void IEnumerator.Reset() { }
        }

        public sealed partial class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, ICollection, IEnumerable
        {
            public KeyCollection(Dictionary<TKey, TValue> dictionary) { }

            public int Count { get { throw null; } }

            bool ICollection<TKey>.IsReadOnly { get { throw null; } }

            bool ICollection.IsSynchronized { get { throw null; } }

            object ICollection.SyncRoot { get { throw null; } }

            public void CopyTo(TKey[] array, int index) { }

            public Enumerator GetEnumerator() { throw null; }

            void ICollection<TKey>.Add(TKey item) { }

            void ICollection<TKey>.Clear() { }

            bool ICollection<TKey>.Contains(TKey item) { throw null; }

            bool ICollection<TKey>.Remove(TKey item) { throw null; }

            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() { throw null; }

            void ICollection.CopyTo(Array array, int index) { }

            IEnumerator IEnumerable.GetEnumerator() { throw null; }

            public partial struct Enumerator : IEnumerator<TKey>, IEnumerator, IDisposable
            {
                public TKey Current { get { throw null; } }

                object IEnumerator.Current { get { throw null; } }

                public void Dispose() { }

                public bool MoveNext() { throw null; }

                void IEnumerator.Reset() { }
            }
        }

        public sealed partial class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, ICollection, IEnumerable
        {
            public ValueCollection(Dictionary<TKey, TValue> dictionary) { }

            public int Count { get { throw null; } }

            bool ICollection<TValue>.IsReadOnly { get { throw null; } }

            bool ICollection.IsSynchronized { get { throw null; } }

            object ICollection.SyncRoot { get { throw null; } }

            public void CopyTo(TValue[] array, int index) { }

            public Enumerator GetEnumerator() { throw null; }

            void ICollection<TValue>.Add(TValue item) { }

            void ICollection<TValue>.Clear() { }

            bool ICollection<TValue>.Contains(TValue item) { throw null; }

            bool ICollection<TValue>.Remove(TValue item) { throw null; }

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() { throw null; }

            void ICollection.CopyTo(Array array, int index) { }

            IEnumerator IEnumerable.GetEnumerator() { throw null; }

            public partial struct Enumerator : IEnumerator<TValue>, IEnumerator, IDisposable
            {
                public TValue Current { get { throw null; } }

                object IEnumerator.Current { get { throw null; } }

                public void Dispose() { }

                public bool MoveNext() { throw null; }

                void IEnumerator.Reset() { }
            }
        }
    }

    public abstract partial class EqualityComparer<T> : IEqualityComparer<T>, IEqualityComparer
    {
        public static EqualityComparer<T> Default { get { throw null; } }

        public abstract bool Equals(T x, T y);
        public abstract int GetHashCode(T obj);
        bool IEqualityComparer.Equals(object x, object y) { throw null; }

        int IEqualityComparer.GetHashCode(object obj) { throw null; }
    }

    public partial class HashSet<T> : ISet<T>, ICollection<T>, IEnumerable<T>, IEnumerable
    {
        public HashSet() { }

        public HashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer) { }

        public HashSet(IEnumerable<T> collection) { }

        public HashSet(IEqualityComparer<T> comparer) { }

        public IEqualityComparer<T> Comparer { get { throw null; } }

        public int Count { get { throw null; } }

        bool ICollection<T>.IsReadOnly { get { throw null; } }

        public bool Add(T item) { throw null; }

        public void Clear() { }

        public bool Contains(T item) { throw null; }

        public void CopyTo(T[] array, int arrayIndex, int count) { }

        public void CopyTo(T[] array, int arrayIndex) { }

        public void CopyTo(T[] array) { }

        public void ExceptWith(IEnumerable<T> other) { }

        public Enumerator GetEnumerator() { throw null; }

        public void IntersectWith(IEnumerable<T> other) { }

        public bool IsProperSubsetOf(IEnumerable<T> other) { throw null; }

        public bool IsProperSupersetOf(IEnumerable<T> other) { throw null; }

        public bool IsSubsetOf(IEnumerable<T> other) { throw null; }

        public bool IsSupersetOf(IEnumerable<T> other) { throw null; }

        public bool Overlaps(IEnumerable<T> other) { throw null; }

        public bool Remove(T item) { throw null; }

        public int RemoveWhere(Predicate<T> match) { throw null; }

        public bool SetEquals(IEnumerable<T> other) { throw null; }

        public void SymmetricExceptWith(IEnumerable<T> other) { }

        void ICollection<T>.Add(T item) { }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() { throw null; }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        public void TrimExcess() { }

        public void UnionWith(IEnumerable<T> other) { }

        public partial struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
        {
            public T Current { get { throw null; } }

            object IEnumerator.Current { get { throw null; } }

            public void Dispose() { }

            public bool MoveNext() { throw null; }

            void IEnumerator.Reset() { }
        }
    }

    public sealed partial class LinkedListNode<T>
    {
        public LinkedListNode(T value) { }

        public LinkedList<T> List { get { throw null; } }

        public LinkedListNode<T> Next { get { throw null; } }

        public LinkedListNode<T> Previous { get { throw null; } }

        public T Value { get { throw null; } set { } }
    }

    public partial class LinkedList<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable
    {
        public LinkedList() { }

        public LinkedList(IEnumerable<T> collection) { }

        public int Count { get { throw null; } }

        public LinkedListNode<T> First { get { throw null; } }

        public LinkedListNode<T> Last { get { throw null; } }

        bool ICollection<T>.IsReadOnly { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value) { throw null; }

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode) { }

        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value) { throw null; }

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode) { }

        public LinkedListNode<T> AddFirst(T value) { throw null; }

        public void AddFirst(LinkedListNode<T> node) { }

        public LinkedListNode<T> AddLast(T value) { throw null; }

        public void AddLast(LinkedListNode<T> node) { }

        public void Clear() { }

        public bool Contains(T value) { throw null; }

        public void CopyTo(T[] array, int index) { }

        public LinkedListNode<T> Find(T value) { throw null; }

        public LinkedListNode<T> FindLast(T value) { throw null; }

        public Enumerator GetEnumerator() { throw null; }

        public bool Remove(T value) { throw null; }

        public void Remove(LinkedListNode<T> node) { }

        public void RemoveFirst() { }

        public void RemoveLast() { }

        void ICollection<T>.Add(T value) { }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() { throw null; }

        void ICollection.CopyTo(Array array, int index) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
        {
            public T Current { get { throw null; } }

            object IEnumerator.Current { get { throw null; } }

            public void Dispose() { }

            public bool MoveNext() { throw null; }

            void IEnumerator.Reset() { }
        }
    }

    public partial class List<T> : IList<T>, ICollection<T>, IReadOnlyList<T>, IReadOnlyCollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
    {
        public List() { }

        public List(IEnumerable<T> collection) { }

        public List(int capacity) { }

        public int Capacity { get { throw null; } set { } }

        public int Count { get { throw null; } }

        public T this[int index] { get { throw null; } set { } }

        bool ICollection<T>.IsReadOnly { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        bool IList.IsFixedSize { get { throw null; } }

        bool IList.IsReadOnly { get { throw null; } }

        object IList.this[int index] { get { throw null; } set { } }

        public void Add(T item) { }

        public void AddRange(IEnumerable<T> collection) { }

        public int BinarySearch(T item, IComparer<T> comparer) { throw null; }

        public int BinarySearch(T item) { throw null; }

        public int BinarySearch(int index, int count, T item, IComparer<T> comparer) { throw null; }

        public void Clear() { }

        public bool Contains(T item) { throw null; }

        public void CopyTo(T[] array, int arrayIndex) { }

        public void CopyTo(T[] array) { }

        public void CopyTo(int index, T[] array, int arrayIndex, int count) { }

        public bool Exists(Predicate<T> match) { throw null; }

        public T Find(Predicate<T> match) { throw null; }

        public List<T> FindAll(Predicate<T> match) { throw null; }

        public int FindIndex(int startIndex, int count, Predicate<T> match) { throw null; }

        public int FindIndex(int startIndex, Predicate<T> match) { throw null; }

        public int FindIndex(Predicate<T> match) { throw null; }

        public T FindLast(Predicate<T> match) { throw null; }

        public int FindLastIndex(int startIndex, int count, Predicate<T> match) { throw null; }

        public int FindLastIndex(int startIndex, Predicate<T> match) { throw null; }

        public int FindLastIndex(Predicate<T> match) { throw null; }

        public Enumerator GetEnumerator() { throw null; }

        public List<T> GetRange(int index, int count) { throw null; }

        public int IndexOf(T item, int index, int count) { throw null; }

        public int IndexOf(T item, int index) { throw null; }

        public int IndexOf(T item) { throw null; }

        public void Insert(int index, T item) { }

        public void InsertRange(int index, IEnumerable<T> collection) { }

        public int LastIndexOf(T item, int index, int count) { throw null; }

        public int LastIndexOf(T item, int index) { throw null; }

        public int LastIndexOf(T item) { throw null; }

        public bool Remove(T item) { throw null; }

        public int RemoveAll(Predicate<T> match) { throw null; }

        public void RemoveAt(int index) { }

        public void RemoveRange(int index, int count) { }

        public void Reverse() { }

        public void Reverse(int index, int count) { }

        public void Sort() { }

        public void Sort(IComparer<T> comparer) { }

        public void Sort(Comparison<T> comparison) { }

        public void Sort(int index, int count, IComparer<T> comparer) { }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() { throw null; }

        void ICollection.CopyTo(Array array, int arrayIndex) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        int IList.Add(object item) { throw null; }

        bool IList.Contains(object item) { throw null; }

        int IList.IndexOf(object item) { throw null; }

        void IList.Insert(int index, object item) { }

        void IList.Remove(object item) { }

        public T[] ToArray() { throw null; }

        public void TrimExcess() { }

        public bool TrueForAll(Predicate<T> match) { throw null; }

        public partial struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
        {
            public T Current { get { throw null; } }

            object IEnumerator.Current { get { throw null; } }

            public void Dispose() { }

            public bool MoveNext() { throw null; }

            void IEnumerator.Reset() { }
        }
    }

    public partial class Queue<T> : IEnumerable<T>, ICollection, IEnumerable
    {
        public Queue() { }

        public Queue(IEnumerable<T> collection) { }

        public Queue(int capacity) { }

        public int Count { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        public void Clear() { }

        public bool Contains(T item) { throw null; }

        public void CopyTo(T[] array, int arrayIndex) { }

        public T Dequeue() { throw null; }

        public void Enqueue(T item) { }

        public Enumerator GetEnumerator() { throw null; }

        public T Peek() { throw null; }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() { throw null; }

        void ICollection.CopyTo(Array array, int index) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        public T[] ToArray() { throw null; }

        public void TrimExcess() { }

        public partial struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
        {
            public T Current { get { throw null; } }

            object IEnumerator.Current { get { throw null; } }

            public void Dispose() { }

            public bool MoveNext() { throw null; }

            void IEnumerator.Reset() { }
        }
    }

    public partial class SortedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, ICollection, IEnumerable
    {
        public SortedDictionary() { }

        public SortedDictionary(IComparer<TKey> comparer) { }

        public SortedDictionary(IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer) { }

        public SortedDictionary(IDictionary<TKey, TValue> dictionary) { }

        public IComparer<TKey> Comparer { get { throw null; } }

        public int Count { get { throw null; } }

        public TValue this[TKey key] { get { throw null; } set { } }

        public KeyCollection Keys { get { throw null; } }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly { get { throw null; } }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys { get { throw null; } }

        ICollection<TValue> IDictionary<TKey, TValue>.Values { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        bool IDictionary.IsFixedSize { get { throw null; } }

        bool IDictionary.IsReadOnly { get { throw null; } }

        object IDictionary.this[object key] { get { throw null; } set { } }

        ICollection IDictionary.Keys { get { throw null; } }

        ICollection IDictionary.Values { get { throw null; } }

        public ValueCollection Values { get { throw null; } }

        public void Add(TKey key, TValue value) { }

        public void Clear() { }

        public bool ContainsKey(TKey key) { throw null; }

        public bool ContainsValue(TValue value) { throw null; }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index) { }

        public Enumerator GetEnumerator() { throw null; }

        public bool Remove(TKey key) { throw null; }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair) { }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair) { throw null; }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair) { throw null; }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() { throw null; }

        void ICollection.CopyTo(Array array, int index) { }

        void IDictionary.Add(object key, object value) { }

        bool IDictionary.Contains(object key) { throw null; }

        IDictionaryEnumerator IDictionary.GetEnumerator() { throw null; }

        void IDictionary.Remove(object key) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        public bool TryGetValue(TKey key, out TValue value) { throw null; }

        public partial struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator, IEnumerator, IDisposable
        {
            public KeyValuePair<TKey, TValue> Current { get { throw null; } }

            DictionaryEntry IDictionaryEnumerator.Entry { get { throw null; } }

            object IDictionaryEnumerator.Key { get { throw null; } }

            object IDictionaryEnumerator.Value { get { throw null; } }

            object IEnumerator.Current { get { throw null; } }

            public void Dispose() { }

            public bool MoveNext() { throw null; }

            void IEnumerator.Reset() { }
        }

        public sealed partial class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, ICollection, IEnumerable
        {
            public KeyCollection(SortedDictionary<TKey, TValue> dictionary) { }

            public int Count { get { throw null; } }

            bool ICollection<TKey>.IsReadOnly { get { throw null; } }

            bool ICollection.IsSynchronized { get { throw null; } }

            object ICollection.SyncRoot { get { throw null; } }

            public void CopyTo(TKey[] array, int index) { }

            public Enumerator GetEnumerator() { throw null; }

            void ICollection<TKey>.Add(TKey item) { }

            void ICollection<TKey>.Clear() { }

            bool ICollection<TKey>.Contains(TKey item) { throw null; }

            bool ICollection<TKey>.Remove(TKey item) { throw null; }

            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() { throw null; }

            void ICollection.CopyTo(Array array, int index) { }

            IEnumerator IEnumerable.GetEnumerator() { throw null; }

            public partial struct Enumerator : IEnumerator<TKey>, IEnumerator, IDisposable
            {
                public TKey Current { get { throw null; } }

                object IEnumerator.Current { get { throw null; } }

                public void Dispose() { }

                public bool MoveNext() { throw null; }

                void IEnumerator.Reset() { }
            }
        }

        public sealed partial class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, ICollection, IEnumerable
        {
            public ValueCollection(SortedDictionary<TKey, TValue> dictionary) { }

            public int Count { get { throw null; } }

            bool ICollection<TValue>.IsReadOnly { get { throw null; } }

            bool ICollection.IsSynchronized { get { throw null; } }

            object ICollection.SyncRoot { get { throw null; } }

            public void CopyTo(TValue[] array, int index) { }

            public Enumerator GetEnumerator() { throw null; }

            void ICollection<TValue>.Add(TValue item) { }

            void ICollection<TValue>.Clear() { }

            bool ICollection<TValue>.Contains(TValue item) { throw null; }

            bool ICollection<TValue>.Remove(TValue item) { throw null; }

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() { throw null; }

            void ICollection.CopyTo(Array array, int index) { }

            IEnumerator IEnumerable.GetEnumerator() { throw null; }

            public partial struct Enumerator : IEnumerator<TValue>, IEnumerator, IDisposable
            {
                public TValue Current { get { throw null; } }

                object IEnumerator.Current { get { throw null; } }

                public void Dispose() { }

                public bool MoveNext() { throw null; }

                void IEnumerator.Reset() { }
            }
        }
    }

    public partial class SortedSet<T> : ISet<T>, ICollection<T>, IEnumerable<T>, ICollection, IEnumerable
    {
        public SortedSet() { }

        public SortedSet(IComparer<T> comparer) { }

        public SortedSet(IEnumerable<T> collection, IComparer<T> comparer) { }

        public SortedSet(IEnumerable<T> collection) { }

        public IComparer<T> Comparer { get { throw null; } }

        public int Count { get { throw null; } }

        public T Max { get { throw null; } }

        public T Min { get { throw null; } }

        bool ICollection<T>.IsReadOnly { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        public bool Add(T item) { throw null; }

        public virtual void Clear() { }

        public virtual bool Contains(T item) { throw null; }

        public void CopyTo(T[] array, int index, int count) { }

        public void CopyTo(T[] array, int index) { }

        public void CopyTo(T[] array) { }

        public void ExceptWith(IEnumerable<T> other) { }

        public Enumerator GetEnumerator() { throw null; }

        public virtual SortedSet<T> GetViewBetween(T lowerValue, T upperValue) { throw null; }

        public virtual void IntersectWith(IEnumerable<T> other) { }

        public bool IsProperSubsetOf(IEnumerable<T> other) { throw null; }

        public bool IsProperSupersetOf(IEnumerable<T> other) { throw null; }

        public bool IsSubsetOf(IEnumerable<T> other) { throw null; }

        public bool IsSupersetOf(IEnumerable<T> other) { throw null; }

        public bool Overlaps(IEnumerable<T> other) { throw null; }

        public bool Remove(T item) { throw null; }

        public int RemoveWhere(Predicate<T> match) { throw null; }

        public IEnumerable<T> Reverse() { throw null; }

        public bool SetEquals(IEnumerable<T> other) { throw null; }

        public void SymmetricExceptWith(IEnumerable<T> other) { }

        void ICollection<T>.Add(T item) { }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() { throw null; }

        void ICollection.CopyTo(Array array, int index) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        public void UnionWith(IEnumerable<T> other) { }

        public partial struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
        {
            public T Current { get { throw null; } }

            object IEnumerator.Current { get { throw null; } }

            public void Dispose() { }

            public bool MoveNext() { throw null; }

            void IEnumerator.Reset() { }
        }
    }

    public partial class Stack<T> : IEnumerable<T>, ICollection, IEnumerable
    {
        public Stack() { }

        public Stack(IEnumerable<T> collection) { }

        public Stack(int capacity) { }

        public int Count { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        public void Clear() { }

        public bool Contains(T item) { throw null; }

        public void CopyTo(T[] array, int arrayIndex) { }

        public Enumerator GetEnumerator() { throw null; }

        public T Peek() { throw null; }

        public T Pop() { throw null; }

        public void Push(T item) { }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() { throw null; }

        void ICollection.CopyTo(Array array, int arrayIndex) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        public T[] ToArray() { throw null; }

        public void TrimExcess() { }

        public partial struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
        {
            public T Current { get { throw null; } }

            object IEnumerator.Current { get { throw null; } }

            public void Dispose() { }

            public bool MoveNext() { throw null; }

            void IEnumerator.Reset() { }
        }
    }
}