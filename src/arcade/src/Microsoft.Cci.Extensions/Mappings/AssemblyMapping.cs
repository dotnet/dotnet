// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Cci.Extensions;

namespace Microsoft.Cci.Mappings
{
    public class AssemblyMapping : AttributesMapping<IAssembly>
    {
        private readonly Dictionary<INamespaceDefinition, NamespaceMapping> _namespaces;
        private Dictionary<string, ElementMapping<AssemblyProperty>> _properties;

        public AssemblyMapping(MappingSettings settings)
            : base(settings)
        {
            _namespaces = new Dictionary<INamespaceDefinition, NamespaceMapping>(settings.NamespaceComparer);
        }

        public IEnumerable<NamespaceMapping> Namespaces { get { return _namespaces.Values; } }

        public IEnumerable<ElementMapping<AssemblyProperty>> Properties
        {
            get
            {
                if (_properties == null)
                {
                    _properties = new Dictionary<string, ElementMapping<AssemblyProperty>>();

                    for (int i = 0; i < this.ElementCount; i++)
                    {
                        if (this[i] != null)
                        {
                            foreach (var prop in GetAssemblyProperties(this[i]))
                            {
                                ElementMapping<AssemblyProperty> mapping;
                                if (!_properties.TryGetValue(prop.Key, out mapping))
                                {
                                    mapping = new ElementMapping<AssemblyProperty>(this.Settings);
                                    _properties.Add(prop.Key, mapping);
                                }
                                mapping.AddMapping(i, prop);
                            }
                        }
                    }
                }
                return _properties.Values;
            }
        }

        protected override void OnMappingAdded(int index, IAssembly element)
        {
            // BUG: We need to handle type forwards here as well.
            //
            // For example, consider an assembly which contains only type forwards for
            // a given assembly. In that case, we'd not even try to map it's members.

            foreach (var ns in element.GetAllNamespaces().Where(this.Filter.Include))
            {
                NamespaceMapping mapping;
                if (!_namespaces.TryGetValue(ns, out mapping))
                {
                    mapping = new NamespaceMapping(this.Settings);
                    _namespaces.Add(ns, mapping);
                }
                mapping.AddMapping(index, ns);
            }
        }

        private static IEnumerable<AssemblyProperty> GetAssemblyProperties(IAssembly assembly)
        {
            yield return new AssemblyProperty("TargetRuntimeVersion", assembly.TargetRuntimeVersion);
            yield return new AssemblyProperty("Version", assembly.Version.ToString());
            yield return new AssemblyProperty("PublicKeyToken", assembly.GetPublicKeyToken());

            foreach (IAliasForType alias in assembly.ExportedTypes.OfType<IAliasForType>())
            {
                yield return new AssemblyProperty(alias);
            }
        }

        public class AssemblyProperty : IEquatable<AssemblyProperty>
        {
            public AssemblyProperty(string key, string value)
            {
                this.Key = key;
                this.Name = key;
                this.Value = value;
                this.Delimiter = " : ";
            }

            public AssemblyProperty(IAliasForType alias)
            {
                this.Key = alias.AliasedType.RefDocId();
                this.Name = "Forwarder: " + this.Key;
                this.Value = alias.AliasedType.GetAssemblyReference().ToString();
                this.Delimiter = " => ";
            }

            public string Key { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }

            public string Delimiter { get; set; }

            public override string ToString()
            {
                return string.Format("{0}: {1}", Name, Value);
            }

            public bool Equals(AssemblyProperty other)
            {
                if (this.Value == null)
                    return false;

                return this.Value.Equals(other.Value);
            }
        }
    }
}
