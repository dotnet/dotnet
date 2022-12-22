// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Microsoft.Cci;

namespace Microsoft.Cci.Extensions
{
#pragma warning disable 612,618
    public class APIClosureTypeReferenceVisitor : BaseMetadataTraverser
#pragma warning restore 612,618
    {
        private readonly ICollection<ITypeReference> _typeReferences;
        private readonly ICollection<IAssemblyReference> _assemblyReferences;

        public APIClosureTypeReferenceVisitor()
        {
            _typeReferences = new HashSet<ITypeReference>();
            _assemblyReferences = new HashSet<IAssemblyReference>(new AssemblyReferenceComparer());
        }

        public ICollection<ITypeReference> TypeReferences { get { return _typeReferences; } }

        public ICollection<IAssemblyReference> AssemblyReferences { get { return _assemblyReferences; } }

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

        public override void Visit(ICustomAttribute attribute)
        {
            Visit(attribute.Type); // For some reason the base visitor doesn't visit the attribute type
            base.Visit(attribute);
        }

        public override void Visit(IMethodDefinition method)
        {
            base.Visit(method);
            Visit(method.Body);
        }

        public override void Visit(IMethodReference method)
        {
            base.Visit(method);
            Visit(method.ContainingType);
        }

        private void AddTypeReference(ITypeReference type)
        {
            Contract.Assert(type == type.UnWrap());

            _typeReferences.Add(type);
            IAssemblyReference asmRef = type.GetAssemblyReference();

            if (asmRef != null)
                _assemblyReferences.Add(asmRef);
        }

        private class AssemblyReferenceComparer : IEqualityComparer<IAssemblyReference>
        {
            public bool Equals(IAssemblyReference x, IAssemblyReference y)
            {
                return x.AssemblyIdentity.Equals(y.AssemblyIdentity);
            }

            public int GetHashCode(IAssemblyReference obj)
            {
                return obj.AssemblyIdentity.GetHashCode();
            }
        }
    }
}
