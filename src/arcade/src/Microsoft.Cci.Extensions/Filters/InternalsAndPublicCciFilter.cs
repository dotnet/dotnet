// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Extensions.CSharp;

namespace Microsoft.Cci.Filters
{
    /// <summary>
    /// An <see cref="ICciFilter"/> to include <c>internal</c> and <c>public</c> members.
    /// </summary>
    /// <remarks>
    /// This is a variant of <see cref="PublicOnlyCciFilter"/>. This <see cref="ICciFilter"/> has the following
    /// differences:
    /// <list type="number">
    /// <item>Includes <c>internal</c> members.</item>
    /// <item>Reorders a few checks.</item>
    /// </list>
    /// </remarks>
    public class InternalsAndPublicCciFilter : ICciFilter
    {
        public InternalsAndPublicCciFilter(bool excludeAttributes = true)
            : this(excludeAttributes, includeForwardedTypes: false)
        {
        }

        public InternalsAndPublicCciFilter(bool excludeAttributes, bool includeForwardedTypes)
        {
            ExcludeAttributes = excludeAttributes;
            IncludeForwardedTypes = includeForwardedTypes;
        }

        public bool IncludeForwardedTypes { get; set; }

        public bool ExcludeAttributes { get; set; }

        public virtual bool Include(INamespaceDefinition ns)
        {
            // Only include non-empty namespaces.
            return ns.GetTypes(IncludeForwardedTypes).Any(Include);
        }

        public virtual bool Include(ITypeDefinition type)
        {
            if (Dummy.Type == type)
            {
                return false;
            }

            return TypeHelper.IsVisibleToFriendAssemblies(type);
        }

        public virtual bool Include(ITypeDefinitionMember member)
        {
            // Include internal and private protected members.
            if (member.Visibility == TypeMemberVisibility.Family ||
                member.Visibility == TypeMemberVisibility.FamilyAndAssembly)
            {
                // Similar to special case in PublicOnlyCciFilter, include protected members even of a sealed type.
                // This is necessary to generate compilable code e.g. callers with IVT dependencies on this assembly
                // may call internal methods in a sealed type. (IsVisibleToFriendAssemblies() includes the IsSealed
                // check for other use cases besides this one.)
                return true;
            }

            // Include public(-ish) members and explicit interface implementations.
            if (member.IsVisibleToFriendAssemblies())
            {
                return true;
            }

            // If a type is abstract and has an internal or public constructor, it must expose all abstract members.
            var containingType = member.ContainingTypeDefinition;
            if (containingType.IsAbstract &&
                member.IsAbstract() &&
                containingType.IsConstructorVisibleToFriendAssemblies())
            {
                return true;
            }

            // Otherwise...
            return false;
        }

        public virtual bool Include(ICustomAttribute attribute)
        {
            if (ExcludeAttributes)
            {
                return false;
            }

            // Exclude attributes not visible outside the assembly.
            var attributeDef = attribute.Type.GetDefinitionOrNull();
            if (attributeDef != null && !TypeHelper.IsVisibleToFriendAssemblies(attributeDef))
            {
                return false;
            }

            // Exclude attributes with typeof() argument of a type invisible to friend assemblies.
            foreach (var arg in attribute.Arguments.OfType<IMetadataTypeOf>())
            {
                var typeDef = arg.TypeToGet.GetDefinitionOrNull();
                if (typeDef != null && !TypeHelper.IsVisibleToFriendAssemblies(typeDef))
                {
                    return false;
                }
            }

            // Otherwise...
            return true;
        }
    }
}
