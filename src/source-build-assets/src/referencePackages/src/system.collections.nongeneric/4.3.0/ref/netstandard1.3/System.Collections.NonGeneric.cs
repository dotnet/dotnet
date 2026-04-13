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
[assembly: System.Reflection.AssemblyTitle("System.Collections.NonGeneric")]
[assembly: System.Reflection.AssemblyDescription("System.Collections.NonGeneric")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Collections.NonGeneric")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.1.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Collections
{
    public partial class ArrayList : IEnumerable, IList, ICollection
    {
        public ArrayList() { }

        public ArrayList(ICollection c) { }

        public ArrayList(int capacity) { }

        public virtual int Capacity { get { throw null; } set { } }

        public virtual int Count { get { throw null; } }

        public virtual bool IsFixedSize { get { throw null; } }

        public virtual bool IsReadOnly { get { throw null; } }

        public virtual bool IsSynchronized { get { throw null; } }

        public virtual object this[int index] { get { throw null; } set { } }

        public virtual object SyncRoot { get { throw null; } }

        public static ArrayList Adapter(IList list) { throw null; }

        public virtual int Add(object value) { throw null; }

        public virtual void AddRange(ICollection c) { }

        public virtual int BinarySearch(int index, int count, object value, IComparer comparer) { throw null; }

        public virtual int BinarySearch(object value, IComparer comparer) { throw null; }

        public virtual int BinarySearch(object value) { throw null; }

        public virtual void Clear() { }

        public virtual object Clone() { throw null; }

        public virtual bool Contains(object item) { throw null; }

        public virtual void CopyTo(Array array, int arrayIndex) { }

        public virtual void CopyTo(Array array) { }

        public virtual void CopyTo(int index, Array array, int arrayIndex, int count) { }

        public static ArrayList FixedSize(ArrayList list) { throw null; }

        public static IList FixedSize(IList list) { throw null; }

        public virtual IEnumerator GetEnumerator() { throw null; }

        public virtual IEnumerator GetEnumerator(int index, int count) { throw null; }

        public virtual ArrayList GetRange(int index, int count) { throw null; }

        public virtual int IndexOf(object value, int startIndex, int count) { throw null; }

        public virtual int IndexOf(object value, int startIndex) { throw null; }

        public virtual int IndexOf(object value) { throw null; }

        public virtual void Insert(int index, object value) { }

        public virtual void InsertRange(int index, ICollection c) { }

        public virtual int LastIndexOf(object value, int startIndex, int count) { throw null; }

        public virtual int LastIndexOf(object value, int startIndex) { throw null; }

        public virtual int LastIndexOf(object value) { throw null; }

        public static ArrayList ReadOnly(ArrayList list) { throw null; }

        public static IList ReadOnly(IList list) { throw null; }

        public virtual void Remove(object obj) { }

        public virtual void RemoveAt(int index) { }

        public virtual void RemoveRange(int index, int count) { }

        public static ArrayList Repeat(object value, int count) { throw null; }

        public virtual void Reverse() { }

        public virtual void Reverse(int index, int count) { }

        public virtual void SetRange(int index, ICollection c) { }

        public virtual void Sort() { }

        public virtual void Sort(IComparer comparer) { }

        public virtual void Sort(int index, int count, IComparer comparer) { }

        public static ArrayList Synchronized(ArrayList list) { throw null; }

        public static IList Synchronized(IList list) { throw null; }

        public virtual object[] ToArray() { throw null; }

        public virtual Array ToArray(Type type) { throw null; }

        public virtual void TrimToSize() { }
    }

    public partial class CaseInsensitiveComparer : IComparer
    {
        public CaseInsensitiveComparer() { }

        public CaseInsensitiveComparer(Globalization.CultureInfo culture) { }

        public static CaseInsensitiveComparer Default { get { throw null; } }

        public static CaseInsensitiveComparer DefaultInvariant { get { throw null; } }

        public int Compare(object a, object b) { throw null; }
    }

    public abstract partial class CollectionBase : ICollection, IEnumerable, IList
    {
        protected CollectionBase() { }

        protected CollectionBase(int capacity) { }

        public int Capacity { get { throw null; } set { } }

        public int Count { get { throw null; } }

        protected ArrayList InnerList { get { throw null; } }

        protected IList List { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        bool IList.IsFixedSize { get { throw null; } }

        bool IList.IsReadOnly { get { throw null; } }

        object IList.this[int index] { get { throw null; } set { } }

        public void Clear() { }

        public IEnumerator GetEnumerator() { throw null; }

        protected virtual void OnClear() { }

        protected virtual void OnClearComplete() { }

        protected virtual void OnInsert(int index, object value) { }

        protected virtual void OnInsertComplete(int index, object value) { }

        protected virtual void OnRemove(int index, object value) { }

        protected virtual void OnRemoveComplete(int index, object value) { }

        protected virtual void OnSet(int index, object oldValue, object newValue) { }

        protected virtual void OnSetComplete(int index, object oldValue, object newValue) { }

        protected virtual void OnValidate(object value) { }

        public void RemoveAt(int index) { }

        void ICollection.CopyTo(Array array, int index) { }

        int IList.Add(object value) { throw null; }

        bool IList.Contains(object value) { throw null; }

        int IList.IndexOf(object value) { throw null; }

        void IList.Insert(int index, object value) { }

        void IList.Remove(object value) { }
    }

    public sealed partial class Comparer : IComparer
    {
        public static readonly Comparer Default;
        public static readonly Comparer DefaultInvariant;
        public Comparer(Globalization.CultureInfo culture) { }

        public int Compare(object a, object b) { throw null; }
    }

    public abstract partial class DictionaryBase : ICollection, IEnumerable, IDictionary
    {
        public int Count { get { throw null; } }

        protected IDictionary Dictionary { get { throw null; } }

        protected Hashtable InnerHashtable { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        bool IDictionary.IsFixedSize { get { throw null; } }

        bool IDictionary.IsReadOnly { get { throw null; } }

        object IDictionary.this[object key] { get { throw null; } set { } }

        ICollection IDictionary.Keys { get { throw null; } }

        ICollection IDictionary.Values { get { throw null; } }

        public void Clear() { }

        public void CopyTo(Array array, int index) { }

        public IDictionaryEnumerator GetEnumerator() { throw null; }

        protected virtual void OnClear() { }

        protected virtual void OnClearComplete() { }

        protected virtual object OnGet(object key, object currentValue) { throw null; }

        protected virtual void OnInsert(object key, object value) { }

        protected virtual void OnInsertComplete(object key, object value) { }

        protected virtual void OnRemove(object key, object value) { }

        protected virtual void OnRemoveComplete(object key, object value) { }

        protected virtual void OnSet(object key, object oldValue, object newValue) { }

        protected virtual void OnSetComplete(object key, object oldValue, object newValue) { }

        protected virtual void OnValidate(object key, object value) { }

        void IDictionary.Add(object key, object value) { }

        bool IDictionary.Contains(object key) { throw null; }

        void IDictionary.Remove(object key) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }
    }

    public partial class Hashtable : ICollection, IEnumerable, IDictionary
    {
        public Hashtable() { }

        public Hashtable(IDictionary d, IEqualityComparer equalityComparer) { }

        public Hashtable(IDictionary d, float loadFactor, IEqualityComparer equalityComparer) { }

        public Hashtable(IDictionary d, float loadFactor) { }

        public Hashtable(IDictionary d) { }

        public Hashtable(IEqualityComparer equalityComparer) { }

        public Hashtable(int capacity, IEqualityComparer equalityComparer) { }

        public Hashtable(int capacity, float loadFactor, IEqualityComparer equalityComparer) { }

        public Hashtable(int capacity, float loadFactor) { }

        public Hashtable(int capacity) { }

        public virtual int Count { get { throw null; } }

        protected IEqualityComparer EqualityComparer { get { throw null; } }

        public virtual bool IsFixedSize { get { throw null; } }

        public virtual bool IsReadOnly { get { throw null; } }

        public virtual bool IsSynchronized { get { throw null; } }

        public virtual object this[object key] { get { throw null; } set { } }

        public virtual ICollection Keys { get { throw null; } }

        public virtual object SyncRoot { get { throw null; } }

        public virtual ICollection Values { get { throw null; } }

        public virtual void Add(object key, object value) { }

        public virtual void Clear() { }

        public virtual object Clone() { throw null; }

        public virtual bool Contains(object key) { throw null; }

        public virtual bool ContainsKey(object key) { throw null; }

        public virtual bool ContainsValue(object value) { throw null; }

        public virtual void CopyTo(Array array, int arrayIndex) { }

        public virtual IDictionaryEnumerator GetEnumerator() { throw null; }

        protected virtual int GetHash(object key) { throw null; }

        protected virtual bool KeyEquals(object item, object key) { throw null; }

        public virtual void Remove(object key) { }

        public static Hashtable Synchronized(Hashtable table) { throw null; }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }
    }

    public partial class Queue : ICollection, IEnumerable
    {
        public Queue() { }

        public Queue(ICollection col) { }

        public Queue(int capacity, float growFactor) { }

        public Queue(int capacity) { }

        public virtual int Count { get { throw null; } }

        public virtual bool IsSynchronized { get { throw null; } }

        public virtual object SyncRoot { get { throw null; } }

        public virtual void Clear() { }

        public virtual object Clone() { throw null; }

        public virtual bool Contains(object obj) { throw null; }

        public virtual void CopyTo(Array array, int index) { }

        public virtual object Dequeue() { throw null; }

        public virtual void Enqueue(object obj) { }

        public virtual IEnumerator GetEnumerator() { throw null; }

        public virtual object Peek() { throw null; }

        public static Queue Synchronized(Queue queue) { throw null; }

        public virtual object[] ToArray() { throw null; }

        public virtual void TrimToSize() { }
    }

    public abstract partial class ReadOnlyCollectionBase : ICollection, IEnumerable
    {
        public virtual int Count { get { throw null; } }

        protected ArrayList InnerList { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        public virtual IEnumerator GetEnumerator() { throw null; }

        void ICollection.CopyTo(Array array, int index) { }
    }

    public partial class SortedList : ICollection, IEnumerable, IDictionary
    {
        public SortedList() { }

        public SortedList(IComparer comparer, int capacity) { }

        public SortedList(IComparer comparer) { }

        public SortedList(IDictionary d, IComparer comparer) { }

        public SortedList(IDictionary d) { }

        public SortedList(int initialCapacity) { }

        public virtual int Capacity { get { throw null; } set { } }

        public virtual int Count { get { throw null; } }

        public virtual bool IsFixedSize { get { throw null; } }

        public virtual bool IsReadOnly { get { throw null; } }

        public virtual bool IsSynchronized { get { throw null; } }

        public virtual object this[object key] { get { throw null; } set { } }

        public virtual ICollection Keys { get { throw null; } }

        public virtual object SyncRoot { get { throw null; } }

        public virtual ICollection Values { get { throw null; } }

        public virtual void Add(object key, object value) { }

        public virtual void Clear() { }

        public virtual object Clone() { throw null; }

        public virtual bool Contains(object key) { throw null; }

        public virtual bool ContainsKey(object key) { throw null; }

        public virtual bool ContainsValue(object value) { throw null; }

        public virtual void CopyTo(Array array, int arrayIndex) { }

        public virtual object GetByIndex(int index) { throw null; }

        public virtual IDictionaryEnumerator GetEnumerator() { throw null; }

        public virtual object GetKey(int index) { throw null; }

        public virtual IList GetKeyList() { throw null; }

        public virtual IList GetValueList() { throw null; }

        public virtual int IndexOfKey(object key) { throw null; }

        public virtual int IndexOfValue(object value) { throw null; }

        public virtual void Remove(object key) { }

        public virtual void RemoveAt(int index) { }

        public virtual void SetByIndex(int index, object value) { }

        public static SortedList Synchronized(SortedList list) { throw null; }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        public virtual void TrimToSize() { }
    }

    public partial class Stack : ICollection, IEnumerable
    {
        public Stack() { }

        public Stack(ICollection col) { }

        public Stack(int initialCapacity) { }

        public virtual int Count { get { throw null; } }

        public virtual bool IsSynchronized { get { throw null; } }

        public virtual object SyncRoot { get { throw null; } }

        public virtual void Clear() { }

        public virtual object Clone() { throw null; }

        public virtual bool Contains(object obj) { throw null; }

        public virtual void CopyTo(Array array, int index) { }

        public virtual IEnumerator GetEnumerator() { throw null; }

        public virtual object Peek() { throw null; }

        public virtual object Pop() { throw null; }

        public virtual void Push(object obj) { }

        public static Stack Synchronized(Stack stack) { throw null; }

        public virtual object[] ToArray() { throw null; }
    }
}

namespace System.Collections.Specialized
{
    public partial class CollectionsUtil
    {
        public static Hashtable CreateCaseInsensitiveHashtable() { throw null; }

        public static Hashtable CreateCaseInsensitiveHashtable(IDictionary d) { throw null; }

        public static Hashtable CreateCaseInsensitiveHashtable(int capacity) { throw null; }

        public static SortedList CreateCaseInsensitiveSortedList() { throw null; }
    }
}