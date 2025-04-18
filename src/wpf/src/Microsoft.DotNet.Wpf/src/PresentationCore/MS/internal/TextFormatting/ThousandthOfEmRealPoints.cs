﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Windows;

namespace MS.Internal.TextFormatting
{
    /// <summary>
    /// This is a fixed-size implementation of IList&lt;Point&gt;. It is aimed to reduce the double values storage 
    /// while providing enough precision for glyph run operations. Current usage pattern suggests that there is no
    /// need to support resizing functionality (i.e. Add(), Insert(), Remove(), RemoveAt()).
    ///  
    /// For each point being stored, it will store the X and Y coordinates of the point into two seperate ThousandthOfEmRealDoubles, 
    /// which scales double to 16-bit integer if possible. 
    /// 
    /// </summary>
    internal sealed class ThousandthOfEmRealPoints : IList<Point>
    {        
        //----------------------------------
        // Constructor
        //----------------------------------
        internal ThousandthOfEmRealPoints(
            double emSize,
            int    capacity
            )
        {
            Debug.Assert(capacity >= 0);
            InitArrays(emSize, capacity);
        }
        
        internal ThousandthOfEmRealPoints(
            double       emSize,
            IList<Point> pointValues
            )
        {
            Debug.Assert(pointValues != null);            
            InitArrays(emSize, pointValues.Count);            

            // do the setting
            for (int i = 0; i < Count; i++)
            {
                _xArray[i] = pointValues[i].X;
                _yArray[i] = pointValues[i].Y;
            }
}

        //-------------------------------------
        // Internal properties
        //-------------------------------------
        public int Count
        {
            get
            {
                return _xArray.Count;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }        

        public Point this[int index]
        {
            get
            {
                // underlying array does boundary check
                return new Point(_xArray[index], _yArray[index]);               
            }

            set
            {
                // underlying array does boundary check
                _xArray[index] = value.X;
                _yArray[index] = value.Y;
            }
        }

        //------------------------------------
        // internal methods
        //------------------------------------
        public int IndexOf(Point item)
        {
            // linear search 
            for (int i = 0; i < Count; i++)
            {
                if (_xArray[i] == item.X && _yArray[i] == item.Y)
                {
                    return i;
                }
            }            
            
            return -1;
        }

        public void Clear()
        {
            _xArray.Clear();
            _yArray.Clear();
        }

        public bool Contains(Point item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(Point[] array, int arrayIndex)
        {
            // parameter validations
            ArgumentNullException.ThrowIfNull(array);

            if (array.Rank != 1)
            {
                throw new ArgumentException(
                    SR.Collection_CopyTo_ArrayCannotBeMultidimensional, 
                    nameof(array));                
            }

            ArgumentOutOfRangeException.ThrowIfNegative(arrayIndex);

            if (arrayIndex >= array.Length)
            {
                throw new ArgumentException(
                    SR.Format(
                        SR.Collection_CopyTo_IndexGreaterThanOrEqualToArrayLength, 
                        "arrayIndex", 
                        "array"),
                    nameof(arrayIndex));
            }

            if ((array.Length - Count - arrayIndex) < 0)
            {
                throw new ArgumentException(
                    SR.Format(
                        SR.Collection_CopyTo_NumberOfElementsExceedsArrayLength,
                        "arrayIndex",
                        "array"));
            }           
            

            // do the copying here
            for (int i = 0; i < Count; i++)
            {
                array[arrayIndex + i] = this[i];
            }
        }

        public IEnumerator<Point> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }        

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Point>)this).GetEnumerator();
        }

        public void Add(Point value)
        {
            // not supported, same as Point[] 
            throw new NotSupportedException(SR.CollectionIsFixedSize);                           
        }

        public void Insert(int index, Point item)
        {
            // not supported, same as Point[] 
            throw new NotSupportedException(SR.CollectionIsFixedSize);                           
        }

        public bool Remove(Point item)
        {
            // not supported, same as Point[]             
            throw new NotSupportedException(SR.CollectionIsFixedSize);                           
        }

        public void RemoveAt(int index)
        {
            // not supported, same as Point[]             
            throw new NotSupportedException(SR.CollectionIsFixedSize);                           
        }

        //---------------------------------------------
        // Private methods
        //---------------------------------------------       
        private void InitArrays(double emSize, int capacity)
        {
            _xArray = new ThousandthOfEmRealDoubles(emSize, capacity);
            _yArray = new ThousandthOfEmRealDoubles(emSize, capacity);
        }

        //----------------------------------------
        // Private members
        //----------------------------------------
        private ThousandthOfEmRealDoubles _xArray; // scaled double array for X coordinates
        private ThousandthOfEmRealDoubles _yArray; // scaled double array for Y coordinates
}    
}
