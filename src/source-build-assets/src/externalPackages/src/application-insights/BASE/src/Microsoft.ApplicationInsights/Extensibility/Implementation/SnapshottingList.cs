// <copyright file="SnapshottingList.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System.Collections.Generic;

    internal class SnapshottingList<T> : SnapshottingCollection<T, IList<T>>, IList<T>
    {
        public SnapshottingList()
            : base(new List<T>())
        {
        }

        public virtual T this[int index]
        {
            get
            {
                return this.GetSnapshot()[index];
            }

            set
            {
                lock (this.Collection)
                {
                    this.Collection[index] = value;
                    this.snapshot = null;
                }
            }
        }

        public int IndexOf(T item)
        {
            return this.GetSnapshot().IndexOf(item);
        }

        public virtual void Insert(int index, T item)
        {
            lock (this.Collection)
            {
                this.Collection.Insert(index, item);
                this.snapshot = null;
            }
        }

        public virtual void RemoveAt(int index)
        {
            lock (this.Collection)
            {
                this.Collection.RemoveAt(index);
                this.snapshot = null;
            }
        }

        protected sealed override IList<T> CreateSnapshot(IList<T> collection)
        {
            return new List<T>(collection);
        }
    }
}
