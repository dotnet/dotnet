// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Css.Parser.Utilities
{
    /// <summary>
    /// This list automatically keeps its children in sorted order (duplicates are allowed)
    /// The sorting is based on the value of IRange.Start for each child.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Public API")]
    public class SortedRangeList<T> : IList<T>, IReadOnlyList<T> where T : class, IRange
    {
        private static readonly GapBuffer<T> s_emptyList = new GapBuffer<T>();

        private GapBuffer<T> _list;

        public SortedRangeList()
            : this(0)
        {
        }

        public SortedRangeList(int capacity)
        {
            if (capacity > 0)
            {
                _list = new GapBuffer<T>(capacity);
            }
            else
            {
                _list = s_emptyList;
            }
        }

        /// <summary>
        /// This is incremented whenever anything changes in this list
        /// </summary>
        internal int ChangeStamp { get; private set; }

        protected virtual SortedRangeList<T> CreateForClone()
        {
            return new SortedRangeList<T>();
        }

        internal SortedRangeList<T> Clone()
        {
            SortedRangeList<T> copy = CreateForClone();
            copy.AddRange(this);
            copy.ChangeStamp = ChangeStamp;
            return copy;
        }

        internal int ForceUpdateChangeStamp()
        {
            return ++ChangeStamp;
        }

        /// <summary>
        /// Makes sure that an object is propertly sorted
        /// </summary>
        [Conditional("DEBUG")]
        private void VerifyInsertedObject(int index)
        {
            Debug.Assert(this[index] != null);

            if (index > 0)
            {
                Debug.Assert(this[index].Start >= this[index - 1].Start);
            }

            if (index + 1 < Count)
            {
                Debug.Assert(this[index].Start <= this[index + 1].Start);
            }
        }

        /// <summary>
        /// Override this in derived classes to deal with newly added items
        /// </summary>
        protected virtual void OnAdded(int index)
        {
        }

        /// <summary>
        /// Binary search to find the insert position
        /// </summary>
        internal int FindInsertIndex(int rangeStart, bool beforeExisting)
        {
            int min = 0;
            int max = _list.Count - 1;

            while (min <= max)
            {
                int mid = (min + max) / 2;
                int start = _list[mid].Start;

                if (rangeStart < start || (beforeExisting && rangeStart == start))
                {
                    max = mid - 1;
                }
                else
                {
                    min = mid + 1;
                }
            }

            return max + 1;
        }

        internal T[] ToArray()
        {
            return _list.ToArray();
        }

        public int IndexOf(T item)
        {
            if (item == null)
            {
                Debug.Fail("Can't find a null item");
            }
            else
            {
                // Multiple items could start at the same position, check each one for a match
                for (int index = FindInsertIndex(item.Start, beforeExisting: true); index < Count; index++)
                {
                    T foundItem = _list[index];

                    if (foundItem == item)
                    {
                        return index;
                    }
                    else if (foundItem.Start != item.Start)
                    {
                        break;
                    }
                }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            if (item == null)
            {
                Debug.Fail("Can't insert a null item");
            }
            else
            {
                if (_list == s_emptyList)
                {
                    _list = new GapBuffer<T>();
                }

                _list.Insert(index, item);
                ChangeStamp++;

                OnAdded(index);
                VerifyInsertedObject(index);
            }
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
            ChangeStamp++;
        }

        internal void RemoveRange(int index, int length)
        {
            Debug.Assert(length >= 0);

            if (length > 0)
            {
                _list.RemoveRange(index, length);
                ChangeStamp++;
            }
        }

        internal void RemoveRange(int index, int length, IList<T> removedItems)
        {
            for (int i = index; i < index + length; i++)
            {
                removedItems.Add(this[i]);
            }

            RemoveRange(index, length);
        }

        public T this[int index]
        {
            get
            {
                return _list[index];
            }

            set
            {
                if (value == null)
                {
                    Debug.Fail("Can't set a item to null in a sorted item collection");
                }
                else
                {
                    _list[index] = value;
                    ChangeStamp++;

                    OnAdded(index);
                    VerifyInsertedObject(index);
                }
            }
        }

        public void Add(T item)
        {
            if (item == null)
            {
                Debug.Fail("Can't insert a null item");
            }
            else if ((_list.Gap == 0 || _list[_list.Gap - 1].Start <= item.Start) &&
                (_list.Gap == _list.Count || _list[_list.Gap].Start > item.Start))
            {
                // Adding into the gap is very common, so it's always checked first

                Insert(_list.Gap, item);
            }
            else
            {
                Insert(FindInsertIndex(item.Start, beforeExisting: false), item);
            }
        }

        internal void AddRange(IEnumerable<T> items)
        {
            if (items == this)
            {
                Debug.Fail("Can't add a sorted list to itself");
            }
            else
            {
                // Don't call _list.AddRange since that doesn't keep things sorted

                foreach (T item in items)
                {
                    Add(item);
                }
            }
        }

        public void Clear()
        {
            if (_list.Count != 0)
            {
                _list.Clear();
                ChangeStamp++;
            }
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);

            if (index == -1)
            {
                return false;
            }
            else
            {
                RemoveAt(index);
                return true;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
