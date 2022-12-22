// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using Microsoft.Cci.Extensions;

namespace Microsoft.Cci.Filters
{
    // In contrast to the existing PublicOnlyFilter this filter excludes
    // attributes that use internal types and it excludes explicit interface
    // implementations.
    //
    // In other words, it matches your intuition of the public surface area.

    public sealed class PublicAndProtectedFilter : ICciFilter
    {
        public bool Include(INamespaceDefinition ns)
        {
            return ns.Members.OfType<ITypeDefinition>().Any(Include);
        }

        public bool Include(ITypeDefinition type)
        {
            var nestedType = type as INestedTypeDefinition;
            if (nestedType != null)
                return Include((ITypeDefinitionMember) nestedType);

            var namespaceTypeDefinition = type as INamespaceTypeDefinition;
            if (namespaceTypeDefinition != null)
                return namespaceTypeDefinition.IsPublic;

            return Include(type.UnWrap().ResolvedType);
        }

        public bool Include(ITypeDefinitionMember member)
        {
            return member.Visibility == TypeMemberVisibility.Public ||
                   member.Visibility == TypeMemberVisibility.Family ||
                   member.Visibility == TypeMemberVisibility.FamilyOrAssembly;
        }

        public bool Include(ICustomAttribute attribute)
        {
            return Include(attribute.Type.ResolvedType);
        }
    }
}
