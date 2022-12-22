// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using Microsoft.Cci;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Filters;

namespace Microsoft.DotNet.AsmDiff
{
    internal sealed class DiffCciFilter : ICciFilter
    {
        public DiffCciFilter(bool includeAttributes = false, bool includeInternals = false, bool includePrivates = false, bool includeGenerated = false)
        {
            IncludeAttributes = includeAttributes;
            IncludeInternals = includeInternals;
            IncludePrivates = includePrivates;
            IncludeGenerated = includeGenerated;
        }

        public bool Include(ICustomAttribute attribute)
        {
            // If we can't resolve an attribute, we err on the side of including it.
            // This makes sure we don't accidentally hide all the attributes just
            // because we can't resolve the core assembly.
            if (!IncludeAttributes)
                return false;

            if (attribute.Type == null)
                return true;

            var resolvedType = attribute.Type.ResolvedType;
            return resolvedType is Dummy || Include(resolvedType);
        }

        public bool Include(INamespaceDefinition ns)
        {
            return ns.GetTypes(IncludeForwardedTypes).Any(Include);
        }

        public bool Include(ITypeDefinition type)
        {
            if (type == null || type is Dummy)
                return false;

            if (!IncludeGenerated && IsCompilerGenerated(type))
                return false;

            switch (type.GetVisibility())
            {
                case TypeMemberVisibility.Private:
                    return IncludePrivates;

                case TypeMemberVisibility.Assembly:
                case TypeMemberVisibility.FamilyAndAssembly:
                    return IncludeInternals;

                case TypeMemberVisibility.Family:
                case TypeMemberVisibility.FamilyOrAssembly:
                    return true;

                case TypeMemberVisibility.Public:
                    return true;
            }

            return type.IsVisibleOutsideAssembly();
        }

        public bool Include(ITypeDefinitionMember member)
        {
            if (member == null || member is Dummy)
                return false;

            if (!Include(member.ContainingTypeDefinition))
                return false;

            if (!IncludeGenerated && IsCompilerGenerated(member))
            {
                // If it's an accessor, we want to fall-through.

                var propertyAccessors = from p in member.ContainingTypeDefinition.Properties
                                        from a in p.Accessors
                                        select a;

                var eventAccessors = from e in member.ContainingTypeDefinition.Events
                                     from a in e.Accessors
                                     select a;

                var accessors = propertyAccessors.Concat(eventAccessors);

                var isAccessor = (from a in accessors
                                  where a.ResolvedMethod == member
                                  select a).Any();

                if (!isAccessor)
                    return false;
            }

            switch (member.Visibility)
            {
                case TypeMemberVisibility.Private:
                    return IncludePrivates  || MemberHelper.IsVisibleOutsideAssembly(member);;

                case TypeMemberVisibility.Assembly:
                case TypeMemberVisibility.FamilyAndAssembly:
                    return IncludeInternals;

                case TypeMemberVisibility.Family:
                case TypeMemberVisibility.FamilyOrAssembly:
                    return true;

                case TypeMemberVisibility.Public:
                    return true;
            }

            return member.IsVisibleOutsideAssembly();
        }

        private static bool IsCompilerGenerated(ITypeDefinition typeDefinition)
        {
            var generatedAttribute = typeDefinition.PlatformType.SystemRuntimeCompilerServicesCompilerGeneratedAttribute;
            return typeDefinition.Attributes.Where(a => TypeHelper.TypesAreEquivalent(a.Type, generatedAttribute)).Any();
        }

        private static bool IsCompilerGenerated(ITypeDefinitionMember typeDefinitionMember)
        {
            var generatedAttribute = typeDefinitionMember.ContainingTypeDefinition.PlatformType.SystemRuntimeCompilerServicesCompilerGeneratedAttribute;
            return typeDefinitionMember.Attributes.Where(a => TypeHelper.TypesAreEquivalent(a.Type, generatedAttribute)).Any();
        }

        public bool IncludeAttributes { get; set; }
        
        public bool IncludeInternals { get; set; }
        
        public bool IncludePrivates { get; set; }

        public bool IncludeGenerated { get; set; }

        public bool IncludeForwardedTypes { get; set; }
    }
}
