// <copyright file="SnapshottingCollection.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    internal abstract class SnapshottingCollection<TItem, TCollection> : ICollection<TItem>
        where TCollection : class, ICollection<TItem>
    {
        protected readonly TCollection Collection;
        protected TCollection snapshot;

        protected SnapshottingCollection(TCollection collection)
        {
            Debug.Assert(collection != null, "collection");
            this.Collection = collection;
        }

        public int Count
        {
            get { return this.GetSnapshot().Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Add(TItem item)
        {
            lock (this.Collection)
            {
                this.Collection.Add(item);
                this.snapshot = default(TCollection);
            }
        }

        public virtual void Clear()
        {
            lock (this.Collection)
            {
                this.Collection.Clear();
                this.snapshot = default(TCollection);
            }
        }

        public bool Contains(TItem item)
        {
            return this.GetSnapshot().Contains(item);
        }

        public void CopyTo(TItem[] array, int arrayIndex)
        {
            this.GetSnapshot().CopyTo(array, arrayIndex);
        }

        public virtual bool Remove(TItem item)
        {
            lock (this.Collection)
            {
                bool removed = this.Collection.Remove(item);
                if (removed)
                {
                    this.snapshot = default(TCollection);
                }

                return removed;
            }
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return this.GetSnapshot().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        protected abstract TCollection CreateSnapshot(TCollection collection);

        protected TCollection GetSnapshot()
        {
            TCollection localSnapshot = this.snapshot;
            if (localSnapshot == null)
            {
                lock (this.Collection)
                {
                    this.snapshot = this.CreateSnapshot(this.Collection);
                    localSnapshot = this.snapshot;
                }
            }

            return localSnapshot;
        }
    }
}
