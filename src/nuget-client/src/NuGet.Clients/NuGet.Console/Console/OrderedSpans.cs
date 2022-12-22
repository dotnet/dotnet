// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;

namespace NuGetConsole.Implementation.Console
{
    internal interface IGetSpan<T>
    {
        Span GetSpan(T t);
    }

    internal class OrderedSpans<T>
    {
        private List<T> _items = new List<T>();
        private readonly IGetSpan<T> _getSpan;

        public OrderedSpans(IGetSpan<T> getSpan)
        {
            UtilityMethods.ThrowIfArgumentNull(getSpan);
            _getSpan = getSpan;
        }

        private Span GetSpan(T t)
        {
            return _getSpan.GetSpan(t);
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public T this[int i]
        {
            get { return _items[i]; }
        }

        public void Clear()
        {
            _items.Clear();
        }

        public void Add(T t)
        {
            if (_items.Count > 0
                && GetSpan(t).Start < GetSpan(_items[_items.Count - 1]).End)
            {
                throw new InvalidOperationException();
            }
            _items.Add(t);
        }

        public void PopLast()
        {
            _items.RemoveAt(_items.Count - 1);
        }

        public int FindFirstOverlap(T t)
        {
            if (_items.Count > 0)
            {
                Span span = GetSpan(t);

                // Check most recently added item first
                int index = _items.Count - 1;
                Span lastSpan = GetSpan(_items[index]);
                if (lastSpan.Start <= span.Start)
                {
                    return lastSpan.OverlapsWith(span) ? index : -1;
                }

                // Otherwise start general search
                index = _items.BinarySearch(t, new SpanStartComparer(_getSpan));
                if (index < 0)
                {
                    int prior = ~index - 1; // the prior Span whose Start < span.Start
                    index = Math.Max(0, prior);
                }

                while (index < _items.Count)
                {
                    Span curSpan = GetSpan(_items[index]);
                    if (curSpan.OverlapsWith(span))
                    {
                        return index;
                    }

                    if (curSpan.Start >= span.End)
                    {
                        return -1;
                    }

                    index++;
                }
            }

            return -1;
        }

        public IEnumerable<T> Overlap(T t)
        {
            int index = FindFirstOverlap(t);
            if (index >= 0)
            {
                Span span = GetSpan(t);
                while (index < _items.Count
                       && GetSpan(_items[index]).OverlapsWith(span))
                {
                    yield return _items[index];
                    index++;
                }
            }
        }

        private class SpanStartComparer : Comparer<T>
        {
            private readonly IGetSpan<T> _getSpan;

            public SpanStartComparer(IGetSpan<T> getSpan)
            {
                _getSpan = getSpan;
            }

            public override int Compare(T x, T y)
            {
                return _getSpan.GetSpan(x).Start.CompareTo(
                    _getSpan.GetSpan(y).Start);
            }
        }
    }

    internal class OrderedSpans : OrderedSpans<Span>
    {
        public OrderedSpans()
            : base(new SpanGetSapn())
        {
        }

        private class SpanGetSapn : IGetSpan<Span>
        {
            public Span GetSpan(Span t)
            {
                return t;
            }
        }
    }

    internal class OrderedTupleSpans<T> : OrderedSpans<Tuple<Span, T>>
    {
        public OrderedTupleSpans()
            : base(new TupleGetSpan())
        {
        }

        public IEnumerable<Tuple<Span, T>> Overlap(Span span)
        {
            return base.Overlap(Tuple.Create(span, default(T)));
        }

        private class TupleGetSpan : IGetSpan<Tuple<Span, T>>
        {
            public Span GetSpan(Tuple<Span, T> t)
            {
                return t.Item1;
            }
        }
    }

    internal class ComplexCommandSpans : OrderedTupleSpans<bool>
    {
        public void Add(Span lineSpan, bool endCommand)
        {
            base.Add(Tuple.Create(lineSpan, endCommand));
        }

        public int FindCommandStart(int i)
        {
            while (i - 1 >= 0
                   && !this[i - 1].Item2)
            {
                i--;
            }
            return i;
        }

        public new IEnumerable<IList<Span>> Overlap(Span span)
        {
            int i = base.FindFirstOverlap(Tuple.Create(span, true));
            if (i >= 0)
            {
                // Find first line of this complex command
                i = FindCommandStart(i);

                while (true)
                {
                    // Collect and return one complex command
                    List<Span> spans = new List<Span>();
                    while (i < this.Count)
                    {
                        spans.Add(this[i].Item1);
                        if (this[i++].Item2)
                        {
                            break;
                        }
                    }
                    yield return spans;

                    if (i >= Count
                        || !this[i].Item1.OverlapsWith(span))
                    {
                        break; // Done
                    }
                }
            }
        }
    }
}
