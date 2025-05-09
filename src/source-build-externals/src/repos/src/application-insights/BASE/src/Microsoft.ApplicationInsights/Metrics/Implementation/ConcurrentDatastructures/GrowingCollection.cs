namespace Microsoft.ApplicationInsights.Metrics.ConcurrentDatastructures
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    using static System.FormattableString;

    /// <summary>A very fast, lock free, unordered collection to which items can be added, but never removed.</summary>
    /// <typeparam name="T">Type of collection elements.</typeparam>
    internal class GrowingCollection<T> : IEnumerable<T>
    {
        private const int SegmentSize = 32;

        private Segment dataHead;

        /// <summary>Creates a new <c>GrowingCollection</c>.</summary>
        public GrowingCollection()
        {
            this.dataHead = new Segment(null);
        }

        /// <summary>Gets the current number of items in the collection.</summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                Segment currHead = Volatile.Read(ref this.dataHead);
                return currHead.GlobalCount;
            }
        }

        /// <summary>Adds an item to the collection.</summary>
        /// <param name="item">Item to be added.</param>
        public void Add(T item)
        {
            Segment currHead = Volatile.Read(ref this.dataHead);

            bool added = currHead.TryAdd(item);
            while (false == added)
            {
                Segment newHead = new Segment(currHead);
                Segment prevHead = Interlocked.CompareExchange(ref this.dataHead, newHead, currHead);

                Segment updatedHead = (prevHead == currHead) ? newHead : prevHead;
                added = updatedHead.TryAdd(item);
            }
        }

        /// <summary>Gets an enumerator over this colletion. No particular element order is guaranteed.
        /// The enumerator is resilient to concurrent additions to the collection.</summary>
        /// <returns>A new enumerator that will cover all items already in the collection.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GrowingCollection<T>.Enumerator GetEnumerator()
        {
            var enumerator = new GrowingCollection<T>.Enumerator(this.dataHead);
            return enumerator;
        }

        /// <summary>Gets an enumerator over this colletion. No particular element order is guaranteed.
        /// The enumerator is resilient to concurrent additions to the collection.</summary>
        /// <returns>A new enumerator that will cover all items already in the collection.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Gets an enumerator over this colletion. No particular element order is guaranteed.
        /// The enumerator is resilient to concurrent additions to the collection.</summary>
        /// <returns>A new enumerator that will cover all items already in the collection.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #region class Enumerator 

        /// <summary>An enumerator implementation for a <see cref="GrowingCollection{T}"/>.
        /// The enumerator is resilient to concurrent additions to the collection.
        /// No particular element order is guaranteed.</summary>
        public class Enumerator : IEnumerator<T>
        {
            private readonly Segment head;
            private readonly int headOffset;
            private readonly int count;
            private Segment currentSegment;
            private int currentSegmentOffset;

            internal Enumerator(Segment head)
            {
                Util.ValidateNotNull(head, nameof(head));
                
                this.head = this.currentSegment = head;
                this.headOffset = this.currentSegmentOffset = head.LocalCount;
                this.count = this.headOffset + (this.head.NextSegment == null ? 0 : this.head.NextSegment.GlobalCount);
            }

            /// <summary>Gets the total number of elements returned by this enumerator.</summary>
            public int Count
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    return this.count;
                }
            }

            /// <summary>Gets the current element.</summary>
            public T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    return this.currentSegment[this.currentSegmentOffset];
                }
            }

            object IEnumerator.Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    return this.Current;
                }
            }

            /// <summary>DIsposes this enumerator.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>Move to the next element in the underlying colection.</summary>
            /// <returns>The next element in the underlying collection.</returns>
            public bool MoveNext()
            {
                if (this.currentSegmentOffset == 0)
                {
                    if (this.currentSegment.NextSegment == null)
                    {
                        return false;
                    }
                    else
                    {
                        this.currentSegment = this.currentSegment.NextSegment;
                        this.currentSegmentOffset = this.currentSegment.LocalCount - 1;
                        return true;
                    }
                }
                else
                {
                    this.currentSegmentOffset--;
                    return true;
                }
            }

            /// <summary>Restarts this enumerator to the same state as it was created in.</summary>
            public void Reset()
            {
                this.currentSegment = this.head;
                this.currentSegmentOffset = this.headOffset;
            }

            private static void Dispose(bool disposing)
            {
                if (disposing)
                {
                }
            }
        }
        #endregion class Enumerator 

        #region class Segment
        internal class Segment
        {
            private readonly Segment nextSegment;
            private readonly int nextSegmentGlobalCount;
            private readonly T[] data = new T[SegmentSize];
            private int localCount = 0;

            public Segment(Segment nextSegment)
            {
                this.nextSegment = nextSegment;
                this.nextSegmentGlobalCount = (nextSegment == null) ? 0 : nextSegment.GlobalCount;
            }

            public int LocalCount
            {
                get
                {
                    int lc = Volatile.Read(ref this.localCount);
                    if (lc > SegmentSize)
                    {
                        return SegmentSize;
                    }
                    else
                    {
                        return lc;
                    }
                }
            }

            public Segment NextSegment
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    return this.nextSegment;
                }
            }

            public int GlobalCount
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    return this.LocalCount + this.nextSegmentGlobalCount;
                }
            }

            public T this[int index]
            {
                get
                {
                    if (index < 0 || this.localCount <= index || SegmentSize <= index)
                    {
                        throw new ArgumentOutOfRangeException(nameof(index), Invariant($"Invalid index ({index})"));
                    }

                    return this.data[index];
                }
            }

            internal bool TryAdd(T item)
            {
                int index = Interlocked.Increment(ref this.localCount) - 1;
                if (index >= SegmentSize)
                {
                    Interlocked.Decrement(ref this.localCount);
                    return false;
                }

                this.data[index] = item;
                return true;
            }
        }
        #endregion class Segment
    }
}
