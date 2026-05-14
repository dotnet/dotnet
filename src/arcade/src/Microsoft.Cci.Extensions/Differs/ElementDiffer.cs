// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Cci.Mappings;

namespace Microsoft.Cci.Differs
{
    internal class ElementDiffer<T> : IDifferences where T : class
    {
        private readonly ElementMapping<T> _mapping;
        private readonly IDifferenceRule[] _differenceRules;

        private List<Difference> _differences;
        private DifferenceType _difference;

        public ElementDiffer(ElementMapping<T> mapping, IDifferenceRule[] differenceRules)
        {
            _mapping = mapping;
            _differenceRules = differenceRules;
        }

        public void Add(Difference difference)
        {
            EnsureDiff();
            _differences.Add(difference);
        }

        public DifferenceType DifferenceType
        {
            get
            {
                EnsureDiff();
                return _difference;
            }
        }

        public IEnumerable<Difference> Differences
        {
            get
            {
                return _differences;
            }
        }

        private void EnsureDiff()
        {
            if (_differences != null)
                return;

            _differences = new List<Difference>();
            _difference = GetMappingDifference();
        }

        private DifferenceType GetMappingDifference()
        {
            Contract.Assert(_mapping.ElementCount <= 2);

            if (_mapping.ElementCount < 2)
                return DifferenceType.Unchanged;

            return Diff();
        }

        private DifferenceType Diff()
        {
            DifferenceType difference = DifferenceType.Unknown;

            if (_differenceRules != null)
            {
                foreach (IDifferenceRule differenceRule in _differenceRules)
                {
                    DifferenceType tempDiff = differenceRule.Diff<T>(this, _mapping);

                    if (tempDiff > difference)
                        difference = tempDiff;
                }
            }

            // Fallback the default add/remove rules
            if (difference == DifferenceType.Unknown)
            {
                T item1 = _mapping[0];
                T item2 = _mapping[1];

                if (item1 != null && item2 == null)
                    difference = DifferenceType.Removed;
                else if (item1 == null && item2 != null)
                    difference = DifferenceType.Added;
                else
                {
                    IEquatable<T> equatable = item1 as IEquatable<T>;
                    if (equatable != null && !equatable.Equals(item2))
                    {
                        difference = DifferenceType.Changed;
                    }
                    else
                    {
                        // If no differs found an issue assume unchanged
                        difference = DifferenceType.Unchanged;
                    }
                }
            }

            return difference;
        }

        public IEnumerator<Difference> GetEnumerator()
        {
            EnsureDiff();
            return _differences.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
