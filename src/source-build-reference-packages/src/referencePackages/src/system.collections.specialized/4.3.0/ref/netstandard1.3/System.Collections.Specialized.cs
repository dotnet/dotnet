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
[assembly: System.Reflection.AssemblyTitle("System.Collections.Specialized")]
[assembly: System.Reflection.AssemblyDescription("System.Collections.Specialized")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Collections.Specialized")]
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
namespace System.Collections.Specialized
{
    public partial struct BitVector32
    {
        public BitVector32(BitVector32 value) { }

        public BitVector32(int data) { }

        public int Data { get { throw null; } }

        public int this[Section section] { get { throw null; } set { } }

        public bool this[int bit] { get { throw null; } set { } }

        public static int CreateMask() { throw null; }

        public static int CreateMask(int previous) { throw null; }

        public static Section CreateSection(short maxValue, Section previous) { throw null; }

        public static Section CreateSection(short maxValue) { throw null; }

        public override bool Equals(object o) { throw null; }

        public override int GetHashCode() { throw null; }

        public override string ToString() { throw null; }

        public static string ToString(BitVector32 value) { throw null; }

        public partial struct Section
        {
            public short Mask { get { throw null; } }

            public short Offset { get { throw null; } }

            public bool Equals(Section obj) { throw null; }

            public override bool Equals(object o) { throw null; }

            public override int GetHashCode() { throw null; }

            public static bool operator ==(Section a, Section b) { throw null; }

            public static bool operator !=(Section a, Section b) { throw null; }

            public override string ToString() { throw null; }

            public static string ToString(Section value) { throw null; }
        }
    }

    public partial class HybridDictionary : ICollection, IEnumerable, IDictionary
    {
        public HybridDictionary() { }

        public HybridDictionary(bool caseInsensitive) { }

        public HybridDictionary(int initialSize, bool caseInsensitive) { }

        public HybridDictionary(int initialSize) { }

        public int Count { get { throw null; } }

        public bool IsFixedSize { get { throw null; } }

        public bool IsReadOnly { get { throw null; } }

        public bool IsSynchronized { get { throw null; } }

        public object this[object key] { get { throw null; } set { } }

        public ICollection Keys { get { throw null; } }

        public object SyncRoot { get { throw null; } }

        public ICollection Values { get { throw null; } }

        public void Add(object key, object value) { }

        public void Clear() { }

        public bool Contains(object key) { throw null; }

        public void CopyTo(Array array, int index) { }

        public IDictionaryEnumerator GetEnumerator() { throw null; }

        public void Remove(object key) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }
    }

    public partial interface IOrderedDictionary : ICollection, IEnumerable, IDictionary
    {
        object this[int index] { get; set; }

        IDictionaryEnumerator GetEnumerator();
        void Insert(int index, object key, object value);
        void RemoveAt(int index);
    }

    public partial class ListDictionary : ICollection, IEnumerable, IDictionary
    {
        public ListDictionary() { }

        public ListDictionary(IComparer comparer) { }

        public int Count { get { throw null; } }

        public bool IsFixedSize { get { throw null; } }

        public bool IsReadOnly { get { throw null; } }

        public bool IsSynchronized { get { throw null; } }

        public object this[object key] { get { throw null; } set { } }

        public ICollection Keys { get { throw null; } }

        public object SyncRoot { get { throw null; } }

        public ICollection Values { get { throw null; } }

        public void Add(object key, object value) { }

        public void Clear() { }

        public bool Contains(object key) { throw null; }

        public void CopyTo(Array array, int index) { }

        public IDictionaryEnumerator GetEnumerator() { throw null; }

        public void Remove(object key) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }
    }

    public abstract partial class NameObjectCollectionBase : ICollection, IEnumerable
    {
        protected NameObjectCollectionBase() { }

        protected NameObjectCollectionBase(IEqualityComparer equalityComparer) { }

        protected NameObjectCollectionBase(int capacity, IEqualityComparer equalityComparer) { }

        protected NameObjectCollectionBase(int capacity) { }

        public virtual int Count { get { throw null; } }

        protected bool IsReadOnly { get { throw null; } set { } }

        public virtual KeysCollection Keys { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        protected void BaseAdd(string name, object value) { }

        protected void BaseClear() { }

        protected object BaseGet(int index) { throw null; }

        protected object BaseGet(string name) { throw null; }

        protected string[] BaseGetAllKeys() { throw null; }

        protected object[] BaseGetAllValues() { throw null; }

        protected object[] BaseGetAllValues(Type type) { throw null; }

        protected string BaseGetKey(int index) { throw null; }

        protected bool BaseHasKeys() { throw null; }

        protected void BaseRemove(string name) { }

        protected void BaseRemoveAt(int index) { }

        protected void BaseSet(int index, object value) { }

        protected void BaseSet(string name, object value) { }

        public virtual IEnumerator GetEnumerator() { throw null; }

        void ICollection.CopyTo(Array array, int index) { }

        public partial class KeysCollection : ICollection, IEnumerable
        {
            internal KeysCollection() { }

            public int Count { get { throw null; } }

            public string this[int index] { get { throw null; } }

            bool ICollection.IsSynchronized { get { throw null; } }

            object ICollection.SyncRoot { get { throw null; } }

            public virtual string Get(int index) { throw null; }

            public IEnumerator GetEnumerator() { throw null; }

            void ICollection.CopyTo(Array array, int index) { }
        }
    }

    public partial class NameValueCollection : NameObjectCollectionBase
    {
        public NameValueCollection() { }

        public NameValueCollection(IEqualityComparer equalityComparer) { }

        public NameValueCollection(NameValueCollection col) { }

        public NameValueCollection(int capacity, IEqualityComparer equalityComparer) { }

        public NameValueCollection(int capacity, NameValueCollection col) { }

        public NameValueCollection(int capacity) { }

        public virtual string[] AllKeys { get { throw null; } }

        public string this[int index] { get { throw null; } }

        public string this[string name] { get { throw null; } set { } }

        public void Add(NameValueCollection c) { }

        public virtual void Add(string name, string value) { }

        public virtual void Clear() { }

        public void CopyTo(Array dest, int index) { }

        public virtual string Get(int index) { throw null; }

        public virtual string Get(string name) { throw null; }

        public virtual string GetKey(int index) { throw null; }

        public virtual string[] GetValues(int index) { throw null; }

        public virtual string[] GetValues(string name) { throw null; }

        public bool HasKeys() { throw null; }

        protected void InvalidateCachedArrays() { }

        public virtual void Remove(string name) { }

        public virtual void Set(string name, string value) { }
    }

    public partial class OrderedDictionary : ICollection, IEnumerable, IDictionary, IOrderedDictionary
    {
        public OrderedDictionary() { }

        public OrderedDictionary(IEqualityComparer comparer) { }

        public OrderedDictionary(int capacity, IEqualityComparer comparer) { }

        public OrderedDictionary(int capacity) { }

        public int Count { get { throw null; } }

        public bool IsReadOnly { get { throw null; } }

        public object this[int index] { get { throw null; } set { } }

        public object this[object key] { get { throw null; } set { } }

        public ICollection Keys { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        bool IDictionary.IsFixedSize { get { throw null; } }

        public ICollection Values { get { throw null; } }

        public void Add(object key, object value) { }

        public OrderedDictionary AsReadOnly() { throw null; }

        public void Clear() { }

        public bool Contains(object key) { throw null; }

        public void CopyTo(Array array, int index) { }

        public virtual IDictionaryEnumerator GetEnumerator() { throw null; }

        public void Insert(int index, object key, object value) { }

        public void Remove(object key) { }

        public void RemoveAt(int index) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }
    }

    public partial class StringCollection : ICollection, IEnumerable, IList
    {
        public int Count { get { throw null; } }

        public bool IsReadOnly { get { throw null; } }

        public bool IsSynchronized { get { throw null; } }

        public string this[int index] { get { throw null; } set { } }

        public object SyncRoot { get { throw null; } }

        bool IList.IsFixedSize { get { throw null; } }

        bool IList.IsReadOnly { get { throw null; } }

        object IList.this[int index] { get { throw null; } set { } }

        public int Add(string value) { throw null; }

        public void AddRange(string[] value) { }

        public void Clear() { }

        public bool Contains(string value) { throw null; }

        public void CopyTo(string[] array, int index) { }

        public StringEnumerator GetEnumerator() { throw null; }

        public int IndexOf(string value) { throw null; }

        public void Insert(int index, string value) { }

        public void Remove(string value) { }

        public void RemoveAt(int index) { }

        void ICollection.CopyTo(Array array, int index) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        int IList.Add(object value) { throw null; }

        bool IList.Contains(object value) { throw null; }

        int IList.IndexOf(object value) { throw null; }

        void IList.Insert(int index, object value) { }

        void IList.Remove(object value) { }
    }

    public partial class StringDictionary : IEnumerable
    {
        public virtual int Count { get { throw null; } }

        public virtual bool IsSynchronized { get { throw null; } }

        public virtual string this[string key] { get { throw null; } set { } }

        public virtual ICollection Keys { get { throw null; } }

        public virtual object SyncRoot { get { throw null; } }

        public virtual ICollection Values { get { throw null; } }

        public virtual void Add(string key, string value) { }

        public virtual void Clear() { }

        public virtual bool ContainsKey(string key) { throw null; }

        public virtual bool ContainsValue(string value) { throw null; }

        public virtual void CopyTo(Array array, int index) { }

        public virtual IEnumerator GetEnumerator() { throw null; }

        public virtual void Remove(string key) { }
    }

    public partial class StringEnumerator
    {
        internal StringEnumerator() { }

        public string Current { get { throw null; } }

        public bool MoveNext() { throw null; }

        public void Reset() { }
    }
}