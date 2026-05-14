// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;

namespace Microsoft.Cci.Mappings
{
    public class AssemblySetMapping : ElementMapping<IEnumerable<IAssembly>>
    {
        private readonly Dictionary<IAssembly, AssemblyMapping> _assemblies;
        private readonly Dictionary<INamespaceDefinition, NamespaceMapping> _namespaces;

        public AssemblySetMapping(MappingSettings settings)
            : base(settings)
        {
            _namespaces = new Dictionary<INamespaceDefinition, NamespaceMapping>(settings.NamespaceComparer);
            _assemblies = new Dictionary<IAssembly, AssemblyMapping>(settings.AssemblyComparer);
        }

        public IEnumerable<AssemblyMapping> Assemblies { get { return _assemblies.Values; } }

        public IEnumerable<NamespaceMapping> Namespaces { get { return _namespaces.Values; } }

        protected override void OnMappingAdded(int index, IEnumerable<IAssembly> element)
        {
            foreach (var assembly in element)
            {
                if (assembly == null)
                    throw new ArgumentNullException("element", "Element contained a null entry.");

                AssemblyMapping mapping;
                if (!_assemblies.TryGetValue(assembly, out mapping))
                {
                    mapping = new AssemblyMapping(this.Settings);
                    _assemblies.Add(assembly, mapping);
                }
                mapping.AddMapping(index, assembly);

                foreach (var ns in mapping.Namespaces)
                {
                    INamespaceDefinition nspace = ns[index];
                    if (nspace == null)
                        continue;

                    NamespaceMapping nsMapping;
                    if (!_namespaces.TryGetValue(nspace, out nsMapping))
                    {
                        nsMapping = new NamespaceMapping(this.Settings, true);
                        _namespaces.Add(nspace, nsMapping);
                    }
                    nsMapping.AddMapping(index, nspace);
                }
            }
        }
    }
}
