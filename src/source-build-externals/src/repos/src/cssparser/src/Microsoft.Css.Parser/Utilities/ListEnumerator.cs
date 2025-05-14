// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Css.Parser.Utilities
{
    /// <summary>
    /// This can enumerate through any IList.
    /// It's only meant to help with a custom implementations of IList.GetEnumerator
    /// </summary>
    internal sealed class ListEnumerator<T> : IEnumerator<T>
    {
        private IList<T> _list;
        private int _pos;
        private int _start;
        private int _afterEnd;

        public ListEnumerator(IList<T> list)
            : this(list, 0, list.Count)
        {
        }

        public ListEnumerator(IList<T> list, int start, int count)
        {
            _list = list;
            _start = start;
            _afterEnd = start + count;
            _pos = start - 1;
        }

        T IEnumerator<T>.Current
        {
            get { return _list[_pos]; }
        }

        object System.Collections.IEnumerator.Current
        {
            get { return _list[_pos]; }
        }

        bool System.Collections.IEnumerator.MoveNext()
        {
            return ++_pos < _afterEnd;
        }

        void System.Collections.IEnumerator.Reset()
        {
            _pos = _start - 1;
        }

        void IDisposable.Dispose()
        {
            _list = null;
            _pos = -1;
            _start = 0;
            _afterEnd = 0;
        }
    }
}
