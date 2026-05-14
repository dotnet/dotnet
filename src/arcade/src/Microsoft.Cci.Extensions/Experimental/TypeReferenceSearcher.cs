// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Collections;
using Microsoft.Cci;

namespace Microsoft.Cci.Extensions
{
    public class TypeReferenceDependency
    {
        public TypeReferenceDependency(ITypeReference type, ITypeDefinitionMember member)
        {
            this.TypeReference = type;
            this.DependentMember = member;
            this.DependentType = member.ContainingTypeDefinition;
        }

        public TypeReferenceDependency(ITypeReference type, ITypeDefinition typeDef)
        {
            this.TypeReference = type;
            this.DependentMember = null;
            this.DependentType = typeDef;
        }

        public ITypeReference TypeReference { get; private set; }

        public ITypeDefinitionMember DependentMember { get; private set; }

        public ITypeDefinition DependentType { get; private set; }

        internal class TypeReferenceDependencyComparer : IEqualityComparer<TypeReferenceDependency>
        {
            public bool Equals(TypeReferenceDependency x, TypeReferenceDependency y)
            {
                return object.Equals(x.TypeReference, y.TypeReference)
                    && object.Equals(x.DependentMember, y.DependentMember)
                    && object.Equals(x.DependentType, y.DependentMember);
            }

            public int GetHashCode(TypeReferenceDependency obj)
            {
                return obj.TypeReference.GetHashCode() ^ obj.DependentType.GetHashCode();
            }
        }
    }

#pragma warning disable 612,618
    public class TypeReferenceSearcher : BaseMetadataTraverser
#pragma warning restore 612,618
    {
        private Func<ITypeReference, bool> _typePredicate;
        private readonly ICollection<TypeReferenceDependency> _dependencies;

        public TypeReferenceSearcher()
        {
            _dependencies = new HashSet<TypeReferenceDependency>(new TypeReferenceDependency.TypeReferenceDependencyComparer());
        }

        public ICollection<TypeReferenceDependency> Dependencies { get { return _dependencies; } }

        public void Search(Func<ITypeReference, bool> typePredicate, IAssembly assembly)
        {
            _typePredicate = typePredicate;
            _dependencies.Clear();
            this.Visit(assembly);
        }

        public override void Visit(INamespaceTypeReference type)
        {
            AddTypeReference(type);
            base.Visit(type);
        }

        public override void Visit(INestedTypeReference type)
        {
            AddTypeReference(type);
            base.Visit(type);
        }
        public override void Visit(IPropertyDefinition property)
        {
            base.Visit(property);
        }

        public override void Visit(ITypeDefinition type)
        {
            if (type.IsVisibleOutsideAssembly())
                base.Visit(type);
        }

        public override void Visit(ITypeDefinitionMember member)
        {
            if (member.IsVisibleOutsideAssembly())
                base.Visit(member);
        }

        public override void Visit(ICustomAttribute attribute)
        {
            //TODO: For now ignore attribute dependencies
            //Visit(attribute.Type); // For some reason the base visitor doesn't visit the attribute type
            //base.Visit(attribute); 
        }

        private void AddTypeReference(ITypeReference type)
        {
            Contract.Assert(type == type.UnWrap());

            if (_typePredicate(type))
            {
                IPropertyDefinition property = GetCurrent<IPropertyDefinition>();
                if (property != null)
                {
                    if (property.IsVisibleOutsideAssembly())
                    {
                        if (property.Getter != null)
                            _dependencies.Add(new TypeReferenceDependency(type, property.Getter.ResolvedTypeDefinitionMember));
                        if (property.Setter != null)
                            _dependencies.Add(new TypeReferenceDependency(type, property.Setter.ResolvedTypeDefinitionMember));
                    }
                }

                ITypeDefinitionMember member = GetCurrent<ITypeDefinitionMember>();
                if (member != null)
                {
                    if (member.IsVisibleOutsideAssembly())
                        _dependencies.Add(new TypeReferenceDependency(type, member));

                    return;
                }

                ITypeDefinition typeDef = GetCurrentType();
                if (typeDef != null)
                {
                    if (typeDef.IsVisibleOutsideAssembly())
                        _dependencies.Add(new TypeReferenceDependency(type, typeDef));
                    return;
                }
            }
        }

        private T GetCurrent<T>() where T : class
        {
            foreach (var p in path)
            {
                var type = p as T;

                if (type != null)
                    return type;
            }
            return null;
        }

        private ITypeDefinition GetCurrentType()
        {
            foreach (var p in path)
            {
                var type = p as ITypeDefinition;

                // We want to skip over generic parameter types
                if (type != null && !(type is IGenericParameter))
                    return type;
            }
            return null;
        }
    }
}
