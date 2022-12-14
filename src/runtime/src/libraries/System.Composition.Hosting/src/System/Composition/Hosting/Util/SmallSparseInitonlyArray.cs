// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Composition.Hosting.Util
{
    // Extremely performance-sensitive.
    // Always safe for reading, even under concurrent writes,
    // only one writer at a time allowed.
    internal sealed class SmallSparseInitonlyArray
    {
        private sealed class Element { public int Index; public object Value; }

        private const int ElementsCapacity = 128;
        private const int ElementIndexMask = 127;
        private const int LocalOffsetMax = 3;

        private Element[] _elements;
        private SmallSparseInitonlyArray _overflow;

        public void Add(int index, object value)
        {
            _elements ??= new Element[ElementsCapacity];

            var newElement = new Element { Index = index, Value = value };

            var elementIndex = index & ElementIndexMask;
            var e = _elements[elementIndex];
            if (e == null)
            {
                _elements[elementIndex] = newElement;
                return;
            }

            if (e.Index == index)
            {
                throw new ArgumentException(SR.Format(SR.Key_Already_Exist, index), nameof(index));
            }

            for (int offset = 1; offset <= LocalOffsetMax; ++offset)
            {
                var nextIndex = (index + offset) & ElementIndexMask;
                e = _elements[nextIndex];
                if (e == null)
                {
                    _elements[nextIndex] = newElement;
                    return;
                }

                if (e.Index == index)
                {
                    throw new ArgumentException(SR.Format(SR.Key_Already_Exist, index), nameof(index));
                }
            }

            _overflow ??= new SmallSparseInitonlyArray();

            _overflow.Add(index, value);
        }

        public bool TryGetValue(int index, out object value)
        {
            if (_elements == null)
            {
                value = null;
                return false;
            }

            var elementIndex = index & ElementIndexMask;
            var e = _elements[elementIndex];
            if (e != null && e.Index == index)
            {
                value = e.Value;
                return true;
            }

            for (int offset = 1; offset <= LocalOffsetMax; ++offset)
            {
                e = _elements[(index + offset) & ElementIndexMask];
                if (e == null)
                {
                    value = null;
                    return false;
                }

                if (e.Index == index)
                {
                    value = e.Value;
                    return true;
                }
            }

            if (_overflow != null)
                return _overflow.TryGetValue(index, out value);

            value = null;
            return false;
        }
    }
}
