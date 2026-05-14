// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.Cci.Extensions;

namespace Microsoft.Cci.Comparers
{
    public class CciComparers : ICciComparers
    {
        private StringKeyComparer<IAssembly> _assemblyComparer;
        private StringKeyComparer<INamespaceDefinition> _namespaceComparer;
        private StringKeyComparer<ITypeReference> _typeComparer;
        private StringKeyComparer<ITypeDefinitionMember> _memberComparer;
        private AttributeComparer _attributeComparer;

        public CciComparers()
        {
            _assemblyComparer = new StringKeyComparer<IAssembly>(GetKey);
            _namespaceComparer = new StringKeyComparer<INamespaceDefinition>(GetKey);
            _typeComparer = new StringKeyComparer<ITypeReference>(GetKey);
            _memberComparer = new StringKeyComparer<ITypeDefinitionMember>(GetKey);
            _attributeComparer = new AttributeComparer();
        }

        private static CciComparers s_comparers;
        public static ICciComparers Default
        {
            get
            {
                if (s_comparers == null)
                {
                    s_comparers = new CciComparers();
                }
                return s_comparers;
            }
        }

        public virtual IEqualityComparer<T> GetEqualityComparer<T>()
        {
            if (typeof(T) == typeof(IAssembly))
                return (IEqualityComparer<T>)_assemblyComparer;

            if (typeof(T) == typeof(INamespaceDefinition))
                return (IEqualityComparer<T>)_namespaceComparer;

            if (typeof(T) == typeof(ITypeDefinition) || typeof(T) == typeof(ITypeReference))
                return (IEqualityComparer<T>)_typeComparer;

            if (typeof(T) == typeof(ITypeDefinitionMember))
                return (IEqualityComparer<T>)_memberComparer;

            if (typeof(T) == typeof(ICustomAttribute))
                return (IEqualityComparer<T>)_attributeComparer;

            throw new NotSupportedException("Comparer not supported for type " + typeof(T).FullName);
        }

        public virtual IComparer<T> GetComparer<T>()
        {
            if (typeof(T) == typeof(IAssembly))
                return (IComparer<T>)_assemblyComparer;

            if (typeof(T) == typeof(INamespaceDefinition))
                return (IComparer<T>)_namespaceComparer;

            if (typeof(T) == typeof(ITypeDefinition) || typeof(T) == typeof(ITypeReference))
                return (IComparer<T>)_typeComparer;

            if (typeof(T) == typeof(ITypeDefinitionMember))
                return (IComparer<T>)_memberComparer;

            if (typeof(T) == typeof(ICustomAttribute))
                return (IComparer<T>)_attributeComparer;

            throw new NotSupportedException("Comparer not supported for type " + typeof(T).FullName);
        }

        public virtual string GetKey(IAssembly assembly)
        {
            return assembly.Name.Value;
        }

        public virtual string GetKey(INamespaceDefinition ns)
        {
            return TypeHelper.GetNamespaceName((IUnitNamespaceReference)ns, NameFormattingOptions.None);
        }

        public virtual string GetKey(ITypeReference type)
        {
            // Type name just needs to be unique within the namespace
            return TypeHelper.GetTypeName(type,
                NameFormattingOptions.OmitContainingType |
                NameFormattingOptions.TypeParameters);
        }

        public virtual string GetKey(ITypeDefinitionMember member)
        {
            // Member key just needs to be unique within the type.
            return MemberHelper.GetMemberSignature(member,
                NameFormattingOptions.OmitContainingType |
                NameFormattingOptions.TypeParameters |
                NameFormattingOptions.Signature |
                NameFormattingOptions.ReturnType    // Needed to distinguish operators
                );
        }
    }
}
