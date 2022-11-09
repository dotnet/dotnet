// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Cci.Extensions;

namespace Microsoft.Cci.Traversers
{
#pragma warning disable 612,618
    public class ResolveAllReferencesTraverser : BaseMetadataTraverser
#pragma warning restore 612,618
    {
        private Dictionary<IReference, HashSet<IReference>> _missingDependencies;

        public ResolveAllReferencesTraverser()
        {
            _missingDependencies = new Dictionary<IReference, HashSet<IReference>>(new ReferenceEqualityComparer());
        }

        public bool TraverseExternallyVisibleOnly { get; set; }

        public bool TraverseMethodBodies { get; set; }

        public IDictionary<IReference, HashSet<IReference>> MissingDependencies { get { return _missingDependencies; } }

        public override void Visit(IAssembly assembly)
        {
            this.path.Push(assembly);
            base.Visit(assembly);
            this.path.Pop();
        }

        public override void Visit(ITypeDefinition type)
        {
            if (TraverseExternallyVisibleOnly && !type.IsVisibleOutsideAssembly())
                return;

            base.Visit(type);
        }

        public override void Visit(ITypeDefinitionMember member)
        {
            if (TraverseExternallyVisibleOnly && !member.IsVisibleOutsideAssembly())
                return;

            base.Visit(member);
        }

        public override void Visit(INamespaceTypeReference type)
        {
            ITypeDefinition typeDef = type.ResolvedType;

            if (typeDef.IsDummy())
                AddUnresolvedReference(type);

            base.Visit(type);
        }

        public override void Visit(INestedTypeReference type)
        {
            ITypeDefinition typeDef = type.ResolvedType;

            if (typeDef.IsDummy())
                AddUnresolvedReference(type);

            base.Visit(type);
        }

        public override void Visit(ICustomAttribute attribute)
        {
            Visit(attribute.Type); // For some reason the base visitor doesn't visit the attribute type
            base.Visit(attribute);
        }

        public override void Visit(IMethodDefinition method)
        {
            base.Visit(method);

            if (this.TraverseMethodBodies)
                Visit(method.Body);
        }

        public override void Visit(ITypeMemberReference member)
        {
            ITypeDefinitionMember memberDef = member.ResolvedTypeDefinitionMember;

            if (memberDef.IsDummy())
                AddUnresolvedReference(member);

            base.Visit(member);
        }

        public override void Visit(IAliasForType aliasForType)
        {
            base.Visit(aliasForType);

            if (aliasForType.AliasedType is Dummy)
                Contract.Assert(!(aliasForType.AliasedType is Dummy), "The aliased type should not be a dummy");

            if (aliasForType.AliasedType.ResolvedType is Dummy)
                AddUnresolvedReference(aliasForType.AliasedType);
        }

        private void AddUnresolvedReference(IReference reference)
        {
            if (reference is Dummy)
                System.Diagnostics.Debug.Write("Fail");

            HashSet<IReference> dependents;
            if (!_missingDependencies.TryGetValue(reference, out dependents))
            {
                dependents = new HashSet<IReference>(new ReferenceEqualityComparer());
                _missingDependencies.Add(reference, dependents);
            }

            IReference dependent = GetDependent();
            if (dependent != null)
                dependents.Add(dependent);
            else
                System.Diagnostics.Debug.WriteLine("No dependent for " + reference.ToString());
        }

        private IReference GetDependent()
        {
            foreach (var reference in this.path)
            {
                if (reference is ITypeReference)
                    return (IReference)reference;

                if (reference is ITypeMemberReference)
                    return (IReference)reference;

                if (reference is IAssemblyReference)
                    return (IReference)reference;
            }
            return null;
        }

        private class ReferenceEqualityComparer : IEqualityComparer<IReference>
        {
            public bool Equals(IReference x, IReference y)
            {
                return string.Equals(x.UniqueId(), y.UniqueId());
            }

            public int GetHashCode(IReference obj)
            {
                return obj.UniqueId().GetHashCode();
            }
        }
    }
}
