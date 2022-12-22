// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.Cci.Filters
{
    public sealed class AttributesFilter : ICciFilter
    {
        private readonly bool _includeAttributes;

        public AttributesFilter(bool includeAttributes)
        {
            _includeAttributes = includeAttributes;
        }

        public bool Include(INamespaceDefinition ns)
        {
            return true;
        }

        public bool Include(ITypeDefinition type)
        {
            return true;
        }

        public bool Include(ITypeDefinitionMember member)
        {
            return true;
        }

        public bool Include(ICustomAttribute attribute)
        {
            return _includeAttributes;
        }
    }
}
