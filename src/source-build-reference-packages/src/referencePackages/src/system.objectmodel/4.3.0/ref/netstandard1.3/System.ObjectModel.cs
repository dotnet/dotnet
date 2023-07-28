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
[assembly: System.Reflection.AssemblyTitle("System.ObjectModel")]
[assembly: System.Reflection.AssemblyDescription("System.ObjectModel")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.ObjectModel")]
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
namespace System.Collections.ObjectModel
{
    public abstract partial class KeyedCollection<TKey, TItem> : Collection<TItem>
    {
        protected KeyedCollection() { }

        protected KeyedCollection(Generic.IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold) { }

        protected KeyedCollection(Generic.IEqualityComparer<TKey> comparer) { }

        public Generic.IEqualityComparer<TKey> Comparer { get { throw null; } }

        protected Generic.IDictionary<TKey, TItem> Dictionary { get { throw null; } }

        public TItem this[TKey key] { get { throw null; } }

        protected void ChangeItemKey(TItem item, TKey newKey) { }

        protected override void ClearItems() { }

        public bool Contains(TKey key) { throw null; }

        protected abstract TKey GetKeyForItem(TItem item);
        protected override void InsertItem(int index, TItem item) { }

        public bool Remove(TKey key) { throw null; }

        protected override void RemoveItem(int index) { }

        protected override void SetItem(int index, TItem item) { }
    }

    public partial class ObservableCollection<T> : Collection<T>, Specialized.INotifyCollectionChanged, ComponentModel.INotifyPropertyChanged
    {
        public ObservableCollection() { }

        public ObservableCollection(Generic.IEnumerable<T> collection) { }

        public virtual event Specialized.NotifyCollectionChangedEventHandler CollectionChanged { add { } remove { } }

        protected virtual event ComponentModel.PropertyChangedEventHandler PropertyChanged { add { } remove { } }

        event System.ComponentModel.PropertyChangedEventHandler System.ComponentModel.INotifyPropertyChanged.PropertyChanged { add { } remove { } }

        protected IDisposable BlockReentrancy() { throw null; }

        protected void CheckReentrancy() { }

        protected override void ClearItems() { }

        protected override void InsertItem(int index, T item) { }

        public void Move(int oldIndex, int newIndex) { }

        protected virtual void MoveItem(int oldIndex, int newIndex) { }

        protected virtual void OnCollectionChanged(Specialized.NotifyCollectionChangedEventArgs e) { }

        protected virtual void OnPropertyChanged(ComponentModel.PropertyChangedEventArgs e) { }

        protected override void RemoveItem(int index) { }

        protected override void SetItem(int index, T item) { }
    }

    public partial class ReadOnlyDictionary<TKey, TValue> : Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>, Generic.IEnumerable<Generic.KeyValuePair<TKey, TValue>>, IEnumerable, Generic.IDictionary<TKey, TValue>, Generic.IReadOnlyCollection<Generic.KeyValuePair<TKey, TValue>>, Generic.IReadOnlyDictionary<TKey, TValue>, ICollection, IDictionary
    {
        public ReadOnlyDictionary(Generic.IDictionary<TKey, TValue> dictionary) { }

        public int Count { get { throw null; } }

        protected Generic.IDictionary<TKey, TValue> Dictionary { get { throw null; } }

        public TValue this[TKey key] { get { throw null; } }

        public ReadOnlyDictionary<TKey, TValue>.KeyCollection Keys { get { throw null; } }

        bool Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.IsReadOnly { get { throw null; } }

        TValue System.Collections.Generic.IDictionary<TKey,TValue>.this[TKey key] { get { throw null; } set { } }

        Generic.ICollection<TKey> Generic.IDictionary<TKey, TValue>.Keys { get { throw null; } }

        Generic.ICollection<TValue> Generic.IDictionary<TKey, TValue>.Values { get { throw null; } }

        Generic.IEnumerable<TKey> Generic.IReadOnlyDictionary<TKey, TValue>.Keys { get { throw null; } }

        Generic.IEnumerable<TValue> Generic.IReadOnlyDictionary<TKey, TValue>.Values { get { throw null; } }

        bool ICollection.IsSynchronized { get { throw null; } }

        object ICollection.SyncRoot { get { throw null; } }

        bool IDictionary.IsFixedSize { get { throw null; } }

        bool IDictionary.IsReadOnly { get { throw null; } }

        object System.Collections.IDictionary.this[object key] { get { throw null; } set { } }

        ICollection IDictionary.Keys { get { throw null; } }

        ICollection IDictionary.Values { get { throw null; } }

        public ReadOnlyDictionary<TKey, TValue>.ValueCollection Values { get { throw null; } }

        public bool ContainsKey(TKey key) { throw null; }

        public Generic.IEnumerator<Generic.KeyValuePair<TKey, TValue>> GetEnumerator() { throw null; }

        void Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.Add(Generic.KeyValuePair<TKey, TValue> item) { }

        void Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.Clear() { }

        bool Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.Contains(Generic.KeyValuePair<TKey, TValue> item) { throw null; }

        void Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.CopyTo(Generic.KeyValuePair<TKey, TValue>[] array, int arrayIndex) { }

        bool Generic.ICollection<Generic.KeyValuePair<TKey, TValue>>.Remove(Generic.KeyValuePair<TKey, TValue> item) { throw null; }

        void Generic.IDictionary<TKey, TValue>.Add(TKey key, TValue value) { }

        bool Generic.IDictionary<TKey, TValue>.Remove(TKey key) { throw null; }

        void ICollection.CopyTo(Array array, int index) { }

        void IDictionary.Add(object key, object value) { }

        void IDictionary.Clear() { }

        bool IDictionary.Contains(object key) { throw null; }

        IDictionaryEnumerator IDictionary.GetEnumerator() { throw null; }

        void IDictionary.Remove(object key) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        public bool TryGetValue(TKey key, out TValue value) { throw null; }

        public sealed partial class KeyCollection : Generic.ICollection<TKey>, Generic.IEnumerable<TKey>, IEnumerable, Generic.IReadOnlyCollection<TKey>, ICollection
        {
            internal KeyCollection() { }

            public int Count { get { throw null; } }

            bool Generic.ICollection<TKey>.IsReadOnly { get { throw null; } }

            bool ICollection.IsSynchronized { get { throw null; } }

            object ICollection.SyncRoot { get { throw null; } }

            public void CopyTo(TKey[] array, int arrayIndex) { }

            public Generic.IEnumerator<TKey> GetEnumerator() { throw null; }

            void Generic.ICollection<TKey>.Add(TKey item) { }

            void Generic.ICollection<TKey>.Clear() { }

            bool Generic.ICollection<TKey>.Contains(TKey item) { throw null; }

            bool Generic.ICollection<TKey>.Remove(TKey item) { throw null; }

            void ICollection.CopyTo(Array array, int index) { }

            IEnumerator IEnumerable.GetEnumerator() { throw null; }
        }

        public sealed partial class ValueCollection : Generic.ICollection<TValue>, Generic.IEnumerable<TValue>, IEnumerable, Generic.IReadOnlyCollection<TValue>, ICollection
        {
            internal ValueCollection() { }

            public int Count { get { throw null; } }

            bool Generic.ICollection<TValue>.IsReadOnly { get { throw null; } }

            bool ICollection.IsSynchronized { get { throw null; } }

            object ICollection.SyncRoot { get { throw null; } }

            public void CopyTo(TValue[] array, int arrayIndex) { }

            public Generic.IEnumerator<TValue> GetEnumerator() { throw null; }

            void Generic.ICollection<TValue>.Add(TValue item) { }

            void Generic.ICollection<TValue>.Clear() { }

            bool Generic.ICollection<TValue>.Contains(TValue item) { throw null; }

            bool Generic.ICollection<TValue>.Remove(TValue item) { throw null; }

            void ICollection.CopyTo(Array array, int index) { }

            IEnumerator IEnumerable.GetEnumerator() { throw null; }
        }
    }

    public partial class ReadOnlyObservableCollection<T> : ReadOnlyCollection<T>, Specialized.INotifyCollectionChanged, ComponentModel.INotifyPropertyChanged
    {
        public ReadOnlyObservableCollection(ObservableCollection<T> list) : base(default!) { }

        protected virtual event Specialized.NotifyCollectionChangedEventHandler CollectionChanged { add { } remove { } }

        protected virtual event ComponentModel.PropertyChangedEventHandler PropertyChanged { add { } remove { } }

        event System.Collections.Specialized.NotifyCollectionChangedEventHandler System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged { add { } remove { } }
        event System.ComponentModel.PropertyChangedEventHandler System.ComponentModel.INotifyPropertyChanged.PropertyChanged { add { } remove { } }


        protected virtual void OnCollectionChanged(Specialized.NotifyCollectionChangedEventArgs args) { }

        protected virtual void OnPropertyChanged(ComponentModel.PropertyChangedEventArgs args) { }
    }
}

namespace System.Collections.Specialized
{
    public partial interface INotifyCollectionChanged
    {
        event NotifyCollectionChangedEventHandler CollectionChanged;
    }

    public enum NotifyCollectionChangedAction
    {
        Add = 0,
        Remove = 1,
        Replace = 2,
        Move = 3,
        Reset = 4
    }

    public partial class NotifyCollectionChangedEventArgs : EventArgs
    {
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex) { }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems) { }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex) { }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int startingIndex) { }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems) { }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex) { }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index) { }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index) { }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem) { }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem) { }

        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action) { }

        public NotifyCollectionChangedAction Action { get { throw null; } }

        public IList NewItems { get { throw null; } }

        public int NewStartingIndex { get { throw null; } }

        public IList OldItems { get { throw null; } }

        public int OldStartingIndex { get { throw null; } }
    }

    public delegate void NotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e);
}

namespace System.ComponentModel
{
    public partial class DataErrorsChangedEventArgs : EventArgs
    {
        public DataErrorsChangedEventArgs(string propertyName) { }

        public virtual string PropertyName { get { throw null; } }
    }

    public partial interface INotifyDataErrorInfo
    {
        bool HasErrors { get; }

        event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        Collections.IEnumerable GetErrors(string propertyName);
    }

    public partial interface INotifyPropertyChanged
    {
        event PropertyChangedEventHandler PropertyChanged;
    }

    public partial interface INotifyPropertyChanging
    {
        event PropertyChangingEventHandler PropertyChanging;
    }

    public partial class PropertyChangedEventArgs : EventArgs
    {
        public PropertyChangedEventArgs(string propertyName) { }

        public virtual string PropertyName { get { throw null; } }
    }

    public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);
    public partial class PropertyChangingEventArgs : EventArgs
    {
        public PropertyChangingEventArgs(string propertyName) { }

        public virtual string PropertyName { get { throw null; } }
    }

    public delegate void PropertyChangingEventHandler(object sender, PropertyChangingEventArgs e);
}

namespace System.Windows.Input
{
    public partial interface ICommand
    {
        event EventHandler CanExecuteChanged;
        bool CanExecute(object parameter);
        void Execute(object parameter);
    }
}