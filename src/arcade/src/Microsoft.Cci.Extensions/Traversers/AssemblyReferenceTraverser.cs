// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci;

namespace Microsoft.Cci.Extensions
{
#pragma warning disable 612,618
    public class AssemblyReferenceTraverser : BaseMetadataTraverser
#pragma warning restore 612,618
    {
        private HashSet<AssemblyIdentity> _usedAssemblyReferences = new HashSet<AssemblyIdentity>();
        public HashSet<AssemblyIdentity> UsedAssemblyReferences { get { return _usedAssemblyReferences; } }

        public override void Visit(INestedTypeReference type)
        {
            base.Visit(type);
            AddAssemblyReference(type.GetAssemblyReference());
        }

        public override void Visit(INamespaceTypeReference type)
        {
            base.Visit(type);
            AddAssemblyReference(type.GetAssemblyReference());
        }

        public override void Visit(ICustomAttribute attribute)
        {
            Visit(attribute.Type); // For some reason the base visitor doesn't visit the attribute type
            base.Visit(attribute);
        }

        public override void Visit(IMethodDefinition method)
        {
            Visit(method.Body); // Visit the implementation as well.
            base.Visit(method);
        }

        protected void AddAssemblyReference(IAssemblyReference assembly)
        {
            if (assembly == null)
                return;

            AssemblyIdentity id = assembly.AssemblyIdentity;
            if (!UsedAssemblyReferences.Contains(id)) // Only checking for contains for so can easily see new additions with a breakpoint in the debugger.
                UsedAssemblyReferences.Add(id);
        }
    }

    public class AssemblyReferenceIgnoreTypeAliasTraverser : AssemblyReferenceTraverser
    {
        private HashSet<AssemblyIdentity> _aliasedAssemblyReferences = new HashSet<AssemblyIdentity>();
        public HashSet<AssemblyIdentity> AliasedAssemblyReferences { get { return _aliasedAssemblyReferences; } }

        public override void Visit(IAliasForType aliasForType)
        {
            // Do nothing.
            AssemblyIdentity id = aliasForType.AliasedType.GetAssemblyReference().AssemblyIdentity;
            if (!AliasedAssemblyReferences.Contains(id))
                AliasedAssemblyReferences.Add(id);
        }
    }
}
