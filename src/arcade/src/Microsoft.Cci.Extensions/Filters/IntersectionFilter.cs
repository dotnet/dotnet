// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Cci.Filters
{
    /// <summary>
    /// Combines multiple filters together to only include if all filters include.
    /// </summary>
    public class IntersectionFilter : ICciFilter
    {
        public IntersectionFilter(params ICciFilter[] filters)
        {
            // Flatten Filters collection for efficient use below and when querying.
            var filterList = new List<ICciFilter>();
            foreach (var filter in filters)
            {
                if (filter is IntersectionFilter intersection)
                {
                    filterList.AddRange(intersection.Filters);
                    continue;
                }

                filterList.Add(filter);
            }

            Filters = filterList;
        }

        public IList<ICciFilter> Filters { get; }

        public bool Include(ITypeDefinitionMember member)
        {
            return Filters.All(filter => filter.Include(member));
        }

        public bool Include(ICustomAttribute attribute)
        {
            return Filters.All(filter => filter.Include(attribute));
        }

        public bool Include(ITypeDefinition type)
        {
            return Filters.All(filter => filter.Include(type));
        }

        public bool Include(INamespaceDefinition ns)
        {
            return Filters.All(filter => filter.Include(ns));
        }
    }
}
