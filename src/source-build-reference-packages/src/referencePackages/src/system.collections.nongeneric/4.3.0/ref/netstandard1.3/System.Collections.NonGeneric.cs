// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Collections.NonGeneric")]
[assembly: AssemblyDescription("System.Collections.NonGeneric")]
[assembly: AssemblyDefaultAlias("System.Collections.NonGeneric")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.1.0")]




namespace System.Collections
{
    public partial class ArrayList : System.Collections.ICollection, System.Collections.IEnumerable, System.Collections.IList
    {
        public ArrayList() { }
        public ArrayList(System.Collections.ICollection c) { }
        public ArrayList(int capacity) { }
        public virtual int Capacity { get { throw null; } set { } }
        public virtual int Count { get { throw null; } }
        public virtual bool IsFixedSize { get { throw null; } }
        public virtual bool IsReadOnly { get { throw null; } }
        public virtual bool IsSynchronized { get { throw null; } }
        public virtual object this[int index] { get { throw null; } set { } }
        public virtual object SyncRoot { get { throw null; } }
        public static System.Collections.ArrayList Adapter(System.Collections.IList list) { throw null; }
        public virtual int Add(object value) { throw null; }
        public virtual void AddRange(System.Collections.ICollection c) { }
        public virtual int BinarySearch(int index, int count, object value, System.Collections.IComparer comparer) { throw null; }
        public virtual int BinarySearch(object value) { throw null; }
        public virtual int BinarySearch(object value, System.Collections.IComparer comparer) { throw null; }
        public virtual void Clear() { }
        public virtual object Clone() { throw null; }
        public virtual bool Contains(object item) { throw null; }
        public virtual void CopyTo(System.Array array) { }
        public virtual void CopyTo(System.Array array, int arrayIndex) { }
        public virtual void CopyTo(int index, System.Array array, int arrayIndex, int count) { }
        public static System.Collections.ArrayList FixedSize(System.Collections.ArrayList list) { throw null; }
        public static System.Collections.IList FixedSize(System.Collections.IList list) { throw null; }
        public virtual System.Collections.IEnumerator GetEnumerator() { throw null; }
        public virtual System.Collections.IEnumerator GetEnumerator(int index, int count) { throw null; }
        public virtual System.Collections.ArrayList GetRange(int index, int count) { throw null; }
        public virtual int IndexOf(object value) { throw null; }
        public virtual int IndexOf(object value, int startIndex) { throw null; }
        public virtual int IndexOf(object value, int startIndex, int count) { throw null; }
        public virtual void Insert(int index, object value) { }
        public virtual void InsertRange(int index, System.Collections.ICollection c) { }
        public virtual int LastIndexOf(object value) { throw null; }
        public virtual int LastIndexOf(object value, int startIndex) { throw null; }
        public virtual int LastIndexOf(object value, int startIndex, int count) { throw null; }
        public static System.Collections.ArrayList ReadOnly(System.Collections.ArrayList list) { throw null; }
        public static System.Collections.IList ReadOnly(System.Collections.IList list) { throw null; }
        public virtual void Remove(object obj) { }
        public virtual void RemoveAt(int index) { }
        public virtual void RemoveRange(int index, int count) { }
        public static System.Collections.ArrayList Repeat(object value, int count) { throw null; }
        public virtual void Reverse() { }
        public virtual void Reverse(int index, int count) { }
        public virtual void SetRange(int index, System.Collections.ICollection c) { }
        public virtual void Sort() { }
        public virtual void Sort(System.Collections.IComparer comparer) { }
        public virtual void Sort(int index, int count, System.Collections.IComparer comparer) { }
        public static System.Collections.ArrayList Synchronized(System.Collections.ArrayList list) { throw null; }
        public static System.Collections.IList Synchronized(System.Collections.IList list) { throw null; }
        public virtual object[] ToArray() { throw null; }
        public virtual System.Array ToArray(System.Type type) { throw null; }
        public virtual void TrimToSize() { }
    }
    public partial class CaseInsensitiveComparer : System.Collections.IComparer
    {
        public CaseInsensitiveComparer() { }
        public CaseInsensitiveComparer(System.Globalization.CultureInfo culture) { }
        public static System.Collections.CaseInsensitiveComparer Default { get { throw null; } }
        public static System.Collections.CaseInsensitiveComparer DefaultInvariant { get { throw null; } }
        public int Compare(object a, object b) { throw null; }
    }
    public abstract partial class CollectionBase : System.Collections.ICollection, System.Collections.IEnumerable, System.Collections.IList
    {
        protected CollectionBase() { }
        protected CollectionBase(int capacity) { }
        public int Capacity { get { throw null; } set { } }
        public int Count { get { throw null; } }
        protected System.Collections.ArrayList InnerList { get { throw null; } }
        protected System.Collections.IList List { get { throw null; } }
        bool System.Collections.ICollection.IsSynchronized { get { throw null; } }
        object System.Collections.ICollection.SyncRoot { get { throw null; } }
        bool System.Collections.IList.IsFixedSize { get { throw null; } }
        bool System.Collections.IList.IsReadOnly { get { throw null; } }
        object System.Collections.IList.this[int index] { get { throw null; } set { } }
        public void Clear() { }
        public System.Collections.IEnumerator GetEnumerator() { throw null; }
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
        void System.Collections.ICollection.CopyTo(System.Array array, int index) { }
        int System.Collections.IList.Add(object value) { throw null; }
        bool System.Collections.IList.Contains(object value) { throw null; }
        int System.Collections.IList.IndexOf(object value) { throw null; }
        void System.Collections.IList.Insert(int index, object value) { }
        void System.Collections.IList.Remove(object value) { }
    }
    public sealed partial class Comparer : System.Collections.IComparer
    {
        public static readonly System.Collections.Comparer Default;
        public static readonly System.Collections.Comparer DefaultInvariant;
        public Comparer(System.Globalization.CultureInfo culture) { }
        public int Compare(object a, object b) { throw null; }
    }
    public abstract partial class DictionaryBase : System.Collections.ICollection, System.Collections.IDictionary, System.Collections.IEnumerable
    {
        protected DictionaryBase() { }
        public int Count { get { throw null; } }
        protected System.Collections.IDictionary Dictionary { get { throw null; } }
        protected System.Collections.Hashtable InnerHashtable { get { throw null; } }
        bool System.Collections.ICollection.IsSynchronized { get { throw null; } }
        object System.Collections.ICollection.SyncRoot { get { throw null; } }
        bool System.Collections.IDictionary.IsFixedSize { get { throw null; } }
        bool System.Collections.IDictionary.IsReadOnly { get { throw null; } }
        object System.Collections.IDictionary.this[object key] { get { throw null; } set { } }
        System.Collections.ICollection System.Collections.IDictionary.Keys { get { throw null; } }
        System.Collections.ICollection System.Collections.IDictionary.Values { get { throw null; } }
        public void Clear() { }
        public void CopyTo(System.Array array, int index) { }
        public System.Collections.IDictionaryEnumerator GetEnumerator() { throw null; }
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
        void System.Collections.IDictionary.Add(object key, object value) { }
        bool System.Collections.IDictionary.Contains(object key) { throw null; }
        void System.Collections.IDictionary.Remove(object key) { }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
    }
    public partial class Hashtable : System.Collections.ICollection, System.Collections.IDictionary, System.Collections.IEnumerable
    {
        public Hashtable() { }
        public Hashtable(System.Collections.IDictionary d) { }
        public Hashtable(System.Collections.IDictionary d, System.Collections.IEqualityComparer equalityComparer) { }
        public Hashtable(System.Collections.IDictionary d, float loadFactor) { }
        public Hashtable(System.Collections.IDictionary d, float loadFactor, System.Collections.IEqualityComparer equalityComparer) { }
        public Hashtable(System.Collections.IEqualityComparer equalityComparer) { }
        public Hashtable(int capacity) { }
        public Hashtable(int capacity, System.Collections.IEqualityComparer equalityComparer) { }
        public Hashtable(int capacity, float loadFactor) { }
        public Hashtable(int capacity, float loadFactor, System.Collections.IEqualityComparer equalityComparer) { }
        public virtual int Count { get { throw null; } }
        protected System.Collections.IEqualityComparer EqualityComparer { get { throw null; } }
        public virtual bool IsFixedSize { get { throw null; } }
        public virtual bool IsReadOnly { get { throw null; } }
        public virtual bool IsSynchronized { get { throw null; } }
        public virtual object this[object key] { get { throw null; } set { } }
        public virtual System.Collections.ICollection Keys { get { throw null; } }
        public virtual object SyncRoot { get { throw null; } }
        public virtual System.Collections.ICollection Values { get { throw null; } }
        public virtual void Add(object key, object value) { }
        public virtual void Clear() { }
        public virtual object Clone() { throw null; }
        public virtual bool Contains(object key) { throw null; }
        public virtual bool ContainsKey(object key) { throw null; }
        public virtual bool ContainsValue(object value) { throw null; }
        public virtual void CopyTo(System.Array array, int arrayIndex) { }
        public virtual System.Collections.IDictionaryEnumerator GetEnumerator() { throw null; }
        protected virtual int GetHash(object key) { throw null; }
        protected virtual bool KeyEquals(object item, object key) { throw null; }
        public virtual void Remove(object key) { }
        public static System.Collections.Hashtable Synchronized(System.Collections.Hashtable table) { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
    }
    public partial class Queue : System.Collections.ICollection, System.Collections.IEnumerable
    {
        public Queue() { }
        public Queue(System.Collections.ICollection col) { }
        public Queue(int capacity) { }
        public Queue(int capacity, float growFactor) { }
        public virtual int Count { get { throw null; } }
        public virtual bool IsSynchronized { get { throw null; } }
        public virtual object SyncRoot { get { throw null; } }
        public virtual void Clear() { }
        public virtual object Clone() { throw null; }
        public virtual bool Contains(object obj) { throw null; }
        public virtual void CopyTo(System.Array array, int index) { }
        public virtual object Dequeue() { throw null; }
        public virtual void Enqueue(object obj) { }
        public virtual System.Collections.IEnumerator GetEnumerator() { throw null; }
        public virtual object Peek() { throw null; }
        public static System.Collections.Queue Synchronized(System.Collections.Queue queue) { throw null; }
        public virtual object[] ToArray() { throw null; }
        public virtual void TrimToSize() { }
    }
    public abstract partial class ReadOnlyCollectionBase : System.Collections.ICollection, System.Collections.IEnumerable
    {
        protected ReadOnlyCollectionBase() { }
        public virtual int Count { get { throw null; } }
        protected System.Collections.ArrayList InnerList { get { throw null; } }
        bool System.Collections.ICollection.IsSynchronized { get { throw null; } }
        object System.Collections.ICollection.SyncRoot { get { throw null; } }
        public virtual System.Collections.IEnumerator GetEnumerator() { throw null; }
        void System.Collections.ICollection.CopyTo(System.Array array, int index) { }
    }
    public partial class SortedList : System.Collections.ICollection, System.Collections.IDictionary, System.Collections.IEnumerable
    {
        public SortedList() { }
        public SortedList(System.Collections.IComparer comparer) { }
        public SortedList(System.Collections.IComparer comparer, int capacity) { }
        public SortedList(System.Collections.IDictionary d) { }
        public SortedList(System.Collections.IDictionary d, System.Collections.IComparer comparer) { }
        public SortedList(int initialCapacity) { }
        public virtual int Capacity { get { throw null; } set { } }
        public virtual int Count { get { throw null; } }
        public virtual bool IsFixedSize { get { throw null; } }
        public virtual bool IsReadOnly { get { throw null; } }
        public virtual bool IsSynchronized { get { throw null; } }
        public virtual object this[object key] { get { throw null; } set { } }
        public virtual System.Collections.ICollection Keys { get { throw null; } }
        public virtual object SyncRoot { get { throw null; } }
        public virtual System.Collections.ICollection Values { get { throw null; } }
        public virtual void Add(object key, object value) { }
        public virtual void Clear() { }
        public virtual object Clone() { throw null; }
        public virtual bool Contains(object key) { throw null; }
        public virtual bool ContainsKey(object key) { throw null; }
        public virtual bool ContainsValue(object value) { throw null; }
        public virtual void CopyTo(System.Array array, int arrayIndex) { }
        public virtual object GetByIndex(int index) { throw null; }
        public virtual System.Collections.IDictionaryEnumerator GetEnumerator() { throw null; }
        public virtual object GetKey(int index) { throw null; }
        public virtual System.Collections.IList GetKeyList() { throw null; }
        public virtual System.Collections.IList GetValueList() { throw null; }
        public virtual int IndexOfKey(object key) { throw null; }
        public virtual int IndexOfValue(object value) { throw null; }
        public virtual void Remove(object key) { }
        public virtual void RemoveAt(int index) { }
        public virtual void SetByIndex(int index, object value) { }
        public static System.Collections.SortedList Synchronized(System.Collections.SortedList list) { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public virtual void TrimToSize() { }
    }
    public partial class Stack : System.Collections.ICollection, System.Collections.IEnumerable
    {
        public Stack() { }
        public Stack(System.Collections.ICollection col) { }
        public Stack(int initialCapacity) { }
        public virtual int Count { get { throw null; } }
        public virtual bool IsSynchronized { get { throw null; } }
        public virtual object SyncRoot { get { throw null; } }
        public virtual void Clear() { }
        public virtual object Clone() { throw null; }
        public virtual bool Contains(object obj) { throw null; }
        public virtual void CopyTo(System.Array array, int index) { }
        public virtual System.Collections.IEnumerator GetEnumerator() { throw null; }
        public virtual object Peek() { throw null; }
        public virtual object Pop() { throw null; }
        public virtual void Push(object obj) { }
        public static System.Collections.Stack Synchronized(System.Collections.Stack stack) { throw null; }
        public virtual object[] ToArray() { throw null; }
    }
}
namespace System.Collections.Specialized
{
    public partial class CollectionsUtil
    {
        public CollectionsUtil() { }
        public static System.Collections.Hashtable CreateCaseInsensitiveHashtable() { throw null; }
        public static System.Collections.Hashtable CreateCaseInsensitiveHashtable(System.Collections.IDictionary d) { throw null; }
        public static System.Collections.Hashtable CreateCaseInsensitiveHashtable(int capacity) { throw null; }
        public static System.Collections.SortedList CreateCaseInsensitiveSortedList() { throw null; }
    }
}
