// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Css.Parser.Utilities
{
    /// <summary>
    /// This is an array with an empty gap inside of it. The item indexes
    /// in this array ignore the gap (unless you're dealing with "real" indexes).
    /// The gap always moves to the location of changes, so be sure to keep
    /// changes as localized as possible.
    /// </summary>
    internal sealed class GapBuffer<T> : IList<T>
    {
        private T[] _buffer;

        internal GapBuffer()
        {
        }

        internal GapBuffer(int capacity)
        {
            Reserve(capacity);
        }

        /// <summary>
        /// Convert a real index to a virtual index
        /// </summary>
        internal int ToVirtual(int realIndex)
        {
            Debug.Assert(realIndex >= 0 && realIndex <= AllocatedCount);

            return (realIndex < Gap) ? realIndex : realIndex - GapCount;
        }

        /// <summary>
        /// Convert a virtual index to a real index
        /// </summary>
        internal int ToReal(int virtualIndex)
        {
            Debug.Assert(virtualIndex >= 0 && virtualIndex <= Count);

            return (virtualIndex < Gap) ? virtualIndex : virtualIndex + GapCount;
        }

        /// <summary>
        /// Real index of the gap
        /// </summary>
        internal int Gap { get; private set; }

        /// <summary>
        /// Real index after the gap
        /// </summary>
        internal int AfterGap
        {
            get { return Gap + AllocatedCount - Count; }
        }

        /// <summary>
        /// The amount of empty space left before a buffer reallocation is needed
        /// </summary>
        internal int GapCount
        {
            get { return AllocatedCount - Count; }
        }

        /// <summary>
        /// How much total space is allocated?
        /// </summary>
        internal int AllocatedCount
        {
            get { return (_buffer != null) ? _buffer.Length : 0; }
        }

        /// <summary>
        /// The items must implement IComparable(T)
        /// </summary>
        public void Sort()
        {
            if (Count > 0)
            {
                MoveGap(Count);
                Array.Sort(_buffer, 0, Count);
            }
        }

        /// <summary>
        /// Returns the virtual index of "item", or -1 if not found
        /// </summary>
        public int IndexOf(T item)
        {
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;

            for (int virtualIndex = 0; virtualIndex < Count; virtualIndex++)
            {
                if (comparer.Equals(this[virtualIndex], item))
                {
                    return virtualIndex;
                }
            }

            return -1;
        }

        public void Insert(int virtualIndex, T item)
        {
            if (virtualIndex < 0 || virtualIndex > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(virtualIndex));
            }

            Reserve(Count + 1);
            MoveGap(virtualIndex);

            _buffer[Gap++] = item;
            Count++;
        }

        public void RemoveAt(int virtualIndex)
        {
            RemoveRange(virtualIndex, 1);
        }

        public void RemoveRange(int virtualIndex, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (count > 0)
            {
                if (virtualIndex < 0 || virtualIndex + count > Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(virtualIndex));
                }

                // Try hard not to actually move the data we are removing

                if (Gap > virtualIndex + count)
                {
                    // The gap is currently after the range to remove. Move the gap to the end of the range.

                    MoveGap(virtualIndex + count);
                    Array.Clear(_buffer, Gap - count, count);

                    Gap -= count;
                }
                else if (Gap > virtualIndex)
                {
                    // The gap is in the middle of the range to remove. Remove items on each end.

                    Array.Clear(_buffer, AfterGap, virtualIndex + count - Gap);
                    Array.Clear(_buffer, virtualIndex, Gap - virtualIndex);

                    Gap = virtualIndex;
                }
                else
                {
                    // The gap is before the range to remove. Move the range to the end of the gap.

                    MoveGap(virtualIndex);
                    Array.Clear(_buffer, AfterGap, count);
                }

                Count -= count;
            }
        }

        public T this[int virtualIndex]
        {
            // Rely on _buffer to throw "out of range" exceptions
            get { return _buffer[ToReal(virtualIndex)]; }
            set { _buffer[ToReal(virtualIndex)] = value; }
        }

        public void Add(T item)
        {
            Insert(Count, item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (items == this)
            {
                throw new ArgumentException("Can't add a range into itself", nameof(items));
            }

            foreach (T item in items)
            {
                Insert(Count, item);
            }
        }

        public void Clear()
        {
            Gap = 0;
            Count = 0;
            _buffer = null;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public T[] ToArray()
        {
            T[] copy = new T[Count];
            CopyTo(copy, 0);

            return copy;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (Count > 0)
            {
                Array.Copy(_buffer, 0, array, arrayIndex, Gap);
                Array.Copy(_buffer, AfterGap, array, arrayIndex + Gap, AllocatedCount - AfterGap);
            }
        }

        public int Count { get; private set; }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            int virtualIndex = IndexOf(item);

            if (virtualIndex != -1)
            {
                RemoveAt(virtualIndex);
                return true;
            }

            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ListEnumerator<T>(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<T>(this);
        }

        private void Reserve(int desiredAllocCount)
        {
            if (desiredAllocCount > AllocatedCount)
            {
                const int minimumBufferSize = 4;

                // Use powers of two for the actual allocation count
                int newAllocCount = Math.Max(AllocatedCount * 2, minimumBufferSize);

                while (newAllocCount < desiredAllocCount)
                {
                    newAllocCount *= 2;
                }

                T[] newBuffer = new T[newAllocCount];

                // Copy data to the new buffer

                if (Count > 0)
                {
                    Array.Copy(_buffer, 0, newBuffer, 0, Gap);
                    Array.Copy(_buffer, AfterGap, newBuffer, newAllocCount - (AllocatedCount - AfterGap), AllocatedCount - AfterGap);
                }

                // Start using the new buffer (the gap position doesn't change)

                _buffer = newBuffer;
            }
        }

        /// <summary>
        /// This moves the gap so that it's right before virtualIndex
        /// </summary>
        private void MoveGap(int virtualIndex)
        {
            if (Gap != virtualIndex)
            {
                // I suggest you draw this out on paper to verify that these are set correctly
                int rStartMove, moveCount, offset;
                int rClearStart, clearCount;

                if (virtualIndex < Gap)
                {
                    // Move items before the beginning of the gap to the end of the gap

                    rStartMove = ToReal(virtualIndex);
                    moveCount = Gap - rStartMove;
                    offset = GapCount;

                    // might not need to clear the whole gap
                    clearCount = Math.Min(GapCount, moveCount);
                    rClearStart = rStartMove;
                }
                else
                {
                    // Move items after the end of the gap to the beginning of the gap

                    rStartMove = AfterGap;
                    moveCount = ToReal(virtualIndex) - rStartMove;
                    offset = -GapCount;

                    // might not need to clear the whole gap
                    clearCount = Math.Min(GapCount, moveCount);
                    rClearStart = rStartMove + moveCount - clearCount;
                }

                Array.Copy(_buffer, rStartMove, _buffer, rStartMove + offset, moveCount);
                Array.Clear(_buffer, rClearStart, clearCount);

                Gap = virtualIndex;
            }
        }
    }
}
