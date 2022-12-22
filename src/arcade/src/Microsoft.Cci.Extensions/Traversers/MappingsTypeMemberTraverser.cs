// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Cci.Comparers;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Filters;
using Microsoft.Cci.Mappings;
using Microsoft.Cci.Differs;
using System.Diagnostics.Contracts;

namespace Microsoft.Cci.Traversers
{
    public class MappingsTypeMemberTraverser
    {
        private readonly MappingSettings _settings;

        public MappingsTypeMemberTraverser(MappingSettings settings)
        {
            Contract.Requires(settings != null);
            _settings = settings;
        }

        public MappingSettings Settings { get { return _settings; } }

        public IMappingDifferenceFilter DiffFilter { get { return _settings.DiffFilter; } }

        public virtual void Visit(AssemblySetMapping assemblySet)
        {
            if (this.Settings.GroupByAssembly)
            {
                Visit(assemblySet.Assemblies);
            }
            else
            {
                Visit(assemblySet.Namespaces);
            }
        }

        public virtual void Visit(IEnumerable<AssemblyMapping> assemblies)
        {
            assemblies = assemblies.Where(this.DiffFilter.Include);
            assemblies = assemblies.OrderBy(GetAssemblyKey, StringComparer.OrdinalIgnoreCase);

            foreach (var assembly in assemblies)
                Visit(assembly);
        }

        public virtual string GetAssemblyKey(AssemblyMapping assembly)
        {
            return assembly.Representative.Name.Value;
        }

        public virtual void Visit(AssemblyMapping assembly)
        {
            Visit(assembly.Namespaces);
        }

        public virtual void Visit(IEnumerable<NamespaceMapping> namespaces)
        {
            namespaces = namespaces.Where(this.DiffFilter.Include);
            namespaces = namespaces.OrderBy(GetNamespaceKey, StringComparer.OrdinalIgnoreCase);

            foreach (var ns in namespaces)
                Visit(ns);
        }

        public virtual string GetNamespaceKey(NamespaceMapping mapping)
        {
            return mapping.Representative.UniqueId();
        }

        public virtual void Visit(NamespaceMapping ns)
        {
            Visit(ns.Types);
        }

        public virtual void Visit(IEnumerable<TypeMapping> types)
        {
            types = types.Where(this.DiffFilter.Include);
            types = types.OrderBy(t => t.Representative, new TypeDefinitionComparer());

            foreach (var type in types)
                Visit(type);
        }

        public virtual void Visit(TypeMapping type)
        {
            Visit(type.Fields);
            Visit(type.Methods.Where(m => ((IMethodDefinition)m.Representative).IsConstructor));
            Visit(type.Properties);
            Visit(type.Events);
            Visit(type.Methods.Where(m => !((IMethodDefinition)m.Representative).IsConstructor));
            Visit((IEnumerable<TypeMapping>)type.NestedTypes);
        }

        public virtual void Visit(IEnumerable<MemberMapping> members)
        {
            members = members.Where(this.DiffFilter.Include);
            members = members.OrderBy(GetMemberKey, StringComparer.OrdinalIgnoreCase);

            foreach (var member in members)
                Visit(member);
        }

        public virtual string GetMemberKey(MemberMapping member)
        {
            return MemberHelper.GetMemberSignature(member.Representative, NameFormattingOptions.Signature | NameFormattingOptions.TypeParameters | NameFormattingOptions.OmitContainingType);
        }

        public virtual void Visit(MemberMapping member)
        {
        }
    }
}
