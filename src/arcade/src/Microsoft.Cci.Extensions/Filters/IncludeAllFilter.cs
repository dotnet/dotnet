// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.Cci.Filters
{
    public class IncludeAllFilter : ICciFilter
    {
        public virtual bool Include(INamespaceDefinition ns)
        {
            return true;
        }

        public virtual bool Include(ITypeDefinition type)
        {
            return true;
        }

        public virtual bool Include(ITypeDefinitionMember member)
        {
            return true;
        }

        public virtual bool Include(ICustomAttribute attribute)
        {
            return true;
        }
    }
}
