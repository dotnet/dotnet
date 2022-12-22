// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Microsoft.Cci.Extensions;

namespace Microsoft.Cci.Filters
{
    public class ExcludeAttributesFilter : ICciFilter
    {
        private readonly HashSet<string> _attributeDocIds;

        public ExcludeAttributesFilter(IEnumerable<string> attributeDocIds)
        {
            _attributeDocIds = new HashSet<string>(attributeDocIds);
        }

        public ExcludeAttributesFilter(string attributeDocIdFile)
        {
            _attributeDocIds = new HashSet<string>(DocIdExtensions.ReadDocIds(attributeDocIdFile));
        }

        public bool Include(INamespaceDefinition ns) => true;

        public bool Include(ITypeDefinition type) => true;

        public bool Include(ITypeDefinitionMember member) => true;

        public bool Include(ICustomAttribute attribute)
        {
            return !_attributeDocIds.Contains(attribute.DocId());
        }
    }
}
