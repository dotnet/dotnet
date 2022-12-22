// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;

using Microsoft.Cci.Differs;
using Microsoft.Cci.Mappings;

namespace Microsoft.Cci.Filters
{
    public sealed class CommonTypesMappingDifferenceFilter : IMappingDifferenceFilter
    {
        private readonly IMappingDifferenceFilter _baseFilter;
        private readonly bool _includeAddedTypes;
        private readonly bool _includeRemovedTypes;

        public CommonTypesMappingDifferenceFilter(IMappingDifferenceFilter baseFilter, bool includeAddedTypes, bool includeRemovedTypes)
        {
            _baseFilter = baseFilter;
            _includeAddedTypes = includeAddedTypes;
            _includeRemovedTypes = includeRemovedTypes;
        }

        public bool Include(AssemblyMapping assembly)
        {
            return _baseFilter.Include(assembly) && assembly.Namespaces.Any(Include);
        }

        public bool Include(NamespaceMapping ns)
        {
            return _baseFilter.Include(ns) && ns.Types.Any(Include);
        }

        public bool Include(TypeMapping type)
        {
            var isAdded = type.Difference == DifferenceType.Added;
            var isRemoved = type.Difference == DifferenceType.Removed;
            var onBothSides = !isAdded && !isRemoved;
            var include = onBothSides ||
                          isAdded && _includeAddedTypes ||
                          isRemoved && _includeRemovedTypes;
            return _baseFilter.Include(type) && include;
        }

        public bool Include(MemberMapping member)
        {
            return _baseFilter.Include(member) && Include(member.ContainingType);
        }

        public bool Include(DifferenceType difference)
        {
            return _baseFilter.Include(difference);
        }
    }
}
