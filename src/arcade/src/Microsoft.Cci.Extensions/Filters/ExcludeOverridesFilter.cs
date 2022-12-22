// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;

namespace Microsoft.Cci.Filters
{
    public sealed class ExcludeOverridesFilter : ICciFilter
    {
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
            var method = member as IMethodDefinition;
            if (method != null)
                return Include(method);

            var property = member as IPropertyDefinition;
            if (property != null)
                return Include(property);

            var evnt = member as IEventDefinition;
            if (evnt != null)
                return Include(evnt);

            return true;
        }

        private bool Include(IMethodDefinition method)
        {
            var isOverride = method.IsVirtual && !method.IsNewSlot;
            return !isOverride;
        }

        private bool Include(IPropertyDefinition property)
        {
            return property.Accessors.Any(a => Include((IMethodDefinition) a.ResolvedMethod));
        }

        private bool Include(IEventDefinition evnt)
        {
            return evnt.Accessors.Any(a => Include(a.ResolvedMethod));
        }

        public bool Include(ICustomAttribute attribute)
        {
            return true;
        }
    }
}
