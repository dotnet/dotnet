// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.Cci.Filters
{
    public static partial class CciFilterExtensions
    {
        private sealed class NegatedFilter : ICciFilter
        {
            private readonly ICciFilter _filter;

            public NegatedFilter(ICciFilter filter)
            {
                _filter = filter;
            }

            public bool Include(INamespaceDefinition ns)
            {
                return !_filter.Include(ns);
            }

            public bool Include(ITypeDefinition type)
            {
                return !_filter.Include(type);
            }

            public bool Include(ITypeDefinitionMember member)
            {
                return !_filter.Include(member);
            }

            public bool Include(ICustomAttribute attribute)
            {
                return !_filter.Include(attribute);
            }
        }
    }
}
