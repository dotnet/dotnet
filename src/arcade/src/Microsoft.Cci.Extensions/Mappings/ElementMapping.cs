// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.Contracts;
using Microsoft.Cci.Differs;
using Microsoft.Cci.Filters;
using System.Diagnostics;
using System.Collections.Generic;

namespace Microsoft.Cci.Mappings
{
    public class ElementMapping<TElement> where TElement : class
    {
        private readonly TElement[] _elements;
        private readonly bool _allowDuplicates;
        private readonly MappingSettings _settings;
        private IDifferences _differ;

        public ElementMapping(MappingSettings settings, bool allowDuplicateMatchingAdds = false)
        {
            // While this theoretically could work for more than 2 elements the 
            // diffing of more than 2 is tricky and as such I'm not supporting yet.
            Contract.Requires(settings.ElementCount >= 0 && settings.ElementCount <= 2);
            _elements = new TElement[settings.ElementCount];
            _settings = settings;
            _allowDuplicates = allowDuplicateMatchingAdds;
        }

        public TElement Representative
        {
            get
            {
                // Return the first non-null element.
                foreach (var element in _elements)
                    if (element != null)
                        return element;

                throw new InvalidOperationException("At least one element should be non-null!");
            }
        }

        public int ElementCount { get { return _elements.Length; } }

        public TElement this[int index] { get { return _elements[index]; } }

        public void AddMapping(int index, TElement element)
        {
            if (index < 0 || index >= this.ElementCount)
                throw new ArgumentOutOfRangeException("index");

            if (element == null)
                throw new ArgumentNullException("element");

            if (!_allowDuplicates && !this.Settings.IncludeForwardedTypes && _elements[index] != null)
            {
                // Could be useful under debugging but not an error because we have case like WPF
                // where the same type lives in multiple assemblies and we also have cases where members
                // appear to be the same because of modreq.
                Trace.TraceWarning("Duplicate element {0} in set!", element.ToString());
            }


            _differ = null;
            _elements[index] = element;

            OnMappingAdded(index, element);
        }

        public void AddMappings(TElement element1, TElement element2)
        {
            AddMapping(0, element1);
            AddMapping(1, element2);
        }

        protected virtual void OnMappingAdded(int index, TElement element)
        {
        }

        public MappingSettings Settings { get { return _settings; } }

        protected ICciFilter Filter { get { return _settings.Filter; } }

        public IDifferences Differences
        {
            get
            {
                if (_differ == null)
                {
                    if (_settings.DiffFactory == null)
                        throw new NotSupportedException("Diffing is not supported without a IDifferFactory!");

                    _differ = _settings.DiffFactory.GetDiffer<TElement>(this);
                }
                return _differ;
            }
        }

        public virtual DifferenceType Difference
        {
            get { return Differences.DifferenceType; }
        }
    }
}
