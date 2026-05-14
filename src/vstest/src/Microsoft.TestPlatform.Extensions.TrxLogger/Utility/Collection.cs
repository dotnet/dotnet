// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using System.Xml;

using Microsoft.TestPlatform.Extensions.TrxLogger.ObjectModel;
using Microsoft.TestPlatform.Extensions.TrxLogger.XML;
using Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.Utility;

/// <summary>
/// Base class for Eqt Collections.
/// Fast collection, default implementations (Add/Remove/etc) do not allow null items and ignore duplicates.
/// </summary>
internal class EqtBaseCollection<T> : ICollection<T>, IXmlTestStore where T : notnull
{
    #region private classes
    /// <summary>
    /// Wraps non-generic enumerator.
    /// </summary>
    /// <typeparam name="TemplateType"></typeparam>
    private sealed class EqtBaseCollectionEnumerator<TemplateType> : IEnumerator<TemplateType>
    {
        private readonly IEnumerator _enumerator;

        internal EqtBaseCollectionEnumerator(IEnumerator e)
        {
            TPDebug.Assert(e != null, "e is null");
            _enumerator = e;
        }

        public TemplateType Current
        {
            get { return (TemplateType)_enumerator.Current; }
        }

        object IEnumerator.Current
        {
            get { return _enumerator.Current; }
        }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            _enumerator.Reset();
        }

        public void Dispose()
        {
        }
    }
    #endregion

    protected Hashtable _container;

    private string? _childElementName;
    protected EqtBaseCollection()
    {
        _container = new Hashtable();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="comparer">For case insensitive comparison use StringComparer.InvariantCultureIgnoreCase.</param>
    protected EqtBaseCollection(IEqualityComparer comparer)
    {
        _container = new Hashtable(0, comparer);   // Ad default Hashtable() constructor creates table with 0 items.
    }

    /// <summary>
    /// Copy constructor. Shallow copy.
    /// </summary>
    /// <param name="other">The object to copy items from.</param>
    protected EqtBaseCollection(EqtBaseCollection<T> other)
    {
        EqtAssert.ParameterNotNull(other, nameof(other));
        _container = new Hashtable(other._container);
    }

    #region Methods: ICollection<T>
    // TODO: Consider putting check for null to derived classes.
    public virtual void Add(T item)
    {
        EqtAssert.ParameterNotNull(item, nameof(item));

        if (!_container.Contains(item))
        {
            _container.Add(item!, null);    // Do not want to xml-persist the value.
        }
    }

    public virtual bool Contains(T item)
    {
        return item != null && _container.Contains(item);
    }

    /// <summary>
    /// Removes specified item from collection.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <returns>True if collection contained the item, otherwise false.</returns>
    public virtual bool Remove(T item)
    {
        EqtAssert.ParameterNotNull(item, nameof(item));   // This is to be consistent with Add...
        if (_container.Contains(item))
        {
            _container.Remove(item);
            return true;
        }

        return false;
    }

    public virtual void Clear()
    {
        _container.Clear();
    }

    /// <summary>
    /// Shallow copy. Assumes that items are immutable. Override if your items are mutable.
    /// </summary>
    public virtual object Clone()
    {
        return new EqtBaseCollection<T>(this);
    }

    public virtual int Count
    {
        get { return _container.Count; }
    }

    /// <summary>
    /// Copies all items to the array.
    /// As FxCop recommends, this is an explicit implementation and derived classes need to define strongly typed CopyTo.
    /// </summary>
    public virtual void CopyTo(T[] array, int index)
    {
        EqtAssert.ParameterNotNull(array, nameof(array));
        _container.Keys.CopyTo(array, index);
    }

    public bool IsReadOnly
    {
        get { return false; }
    }
    #endregion

    #region IEnumerable
    public virtual IEnumerator GetEnumerator()
    {
        return _container.Keys.GetEnumerator();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return new EqtBaseCollectionEnumerator<T>(GetEnumerator());
    }
    #endregion

    #region IXmlTestStore Members

    /// <summary>
    /// Default behavior is to create child elements with name same as name of type T.
    /// Does not respect IXmlTestStoreCustom.
    /// </summary>
    public virtual void Save(XmlElement element, XmlTestStoreParameters? parameters)
    {
        XmlPersistence xmlPersistence = new();
        xmlPersistence.SaveHashtable(_container, element, ".", ".", null, ChildElementName, parameters);
    }

    #endregion

    private string ChildElementName
    {
        get
        {
            // All we can do here is to delegate to T. Cannot cast T to IXmlTestStoreCustom as T is a type, not an instance.
            _childElementName ??= typeof(T).Name;
            return _childElementName;
        }
    }
}
