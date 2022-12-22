// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.Cci.Filters
{
    public static partial class CciFilterExtensions
    {
        private sealed class AndFilter : ICciFilter
        {
            private readonly ICciFilter _left;
            private readonly ICciFilter _right;

            public AndFilter(ICciFilter left, ICciFilter right)
            {
                _left = left;
                _right = right;
            }

            public bool Include(INamespaceDefinition ns)
            {
                return _left.Include(ns) && _right.Include(ns);
            }

            public bool Include(ITypeDefinition type)
            {
                return _left.Include(type) && _right.Include(type);
            }

            public bool Include(ITypeDefinitionMember member)
            {
                return _left.Include(member) && _right.Include(member);
            }

            public bool Include(ICustomAttribute attribute)
            {
                return _left.Include(attribute) && _right.Include(attribute);
            }
        }
    }
}
