// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Extensions.CSharp;

namespace Microsoft.Cci.Filters
{
    public class PublicOnlyCciFilter : ICciFilter
    {
        public PublicOnlyCciFilter(bool excludeAttributes = true)
        {
            ExcludeAttributes = excludeAttributes;
        }

        public PublicOnlyCciFilter(bool excludeAttributes, bool includeForwardedTypes)
        {
            ExcludeAttributes = excludeAttributes;
            IncludeForwardedTypes = includeForwardedTypes;
        }

        public bool IncludeForwardedTypes { get; set; }

        public bool ExcludeAttributes { get; set; }

        public virtual bool Include(INamespaceDefinition ns)
        {
            // Only include non-empty namespaces
            return ns.GetTypes(this.IncludeForwardedTypes).Any(Include);
        }

        public virtual bool Include(ITypeDefinition type)
        {
            if (Dummy.Type == type)
                return false;
            return type.IsVisibleOutsideAssembly();
        }

        public virtual bool Include(ITypeDefinitionMember member)
        {
            switch (member.Visibility)
            {
                case TypeMemberVisibility.Public:
                    return true;
                case TypeMemberVisibility.Family:
                case TypeMemberVisibility.FamilyOrAssembly:
                    // CCI's version of IsVisibleOutsideAssembly doesn't
                    // consider protected members as being visible but for
                    // our purposes which is to write CS files that can
                    // be compiled we always need the protected members
                    return true;
            }

            if (!member.IsVisibleOutsideAssembly())
            {
                // If a type is public, abstract and has a public constructor,
                // then it must expose all abstract members.
                if (member.ContainingTypeDefinition.IsAbstract &&
                    member.IsAbstract() &&
                    member.ContainingTypeDefinition.IsConstructorVisible()
                    )
                {
                    return true;
                }
                return false;
            }

            return true;
        }

        public virtual bool Include(ICustomAttribute attribute)
        {
            if (this.ExcludeAttributes)
                return false;

            // Ignore attributes not visible outside the assembly
            var attributeDef = attribute.Type.GetDefinitionOrNull();
            if (attributeDef != null && !attributeDef.IsVisibleOutsideAssembly())
                return false;

            // Ignore attributes with typeof argument of a type invisible outside the assembly
            foreach(var arg in attribute.Arguments.OfType<IMetadataTypeOf>())
            {
                var typeDef = arg.TypeToGet.GetDefinitionOrNull();
                if (typeDef == null)
                    continue;

                if (!typeDef.IsVisibleOutsideAssembly())
                    return false;
            }

            return true;
        }
    }
}
