// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Cci.Differs;
using Microsoft.Cci.Extensions;

namespace Microsoft.Cci.Mappings
{
    public class TypeMapping : AttributesMapping<ITypeDefinition>
    {
        private readonly Dictionary<ITypeDefinitionMember, MemberMapping> _members;
        private readonly Dictionary<INestedTypeDefinition, TypeMapping> _nestedTypes;

        public TypeMapping(MappingSettings settings)
            : base(settings)
        {
            _members = new Dictionary<ITypeDefinitionMember, MemberMapping>(settings.MemberComparer);
            _nestedTypes = new Dictionary<INestedTypeDefinition, TypeMapping>(settings.TypeComparer);
        }

        public bool ShouldDiffMembers
        {
            get
            {
                // Lets include all members if we are passed unchanged option
                if (Settings.DiffFilter.Include(DifferenceType.Unchanged))
                    return true;

                // First we check whether we are expected to return all members
                if (Settings.AlwaysDiffMembers)
                    return true;

                // Otherwise, Added or Removed types simply give a one-sided diff for all the members.
                return Difference != DifferenceType.Added && Difference != DifferenceType.Removed;
            }
        }

        public IEnumerable<MemberMapping> Members { get { return _members.Values; } }

        public IEnumerable<MemberMapping> Fields { get { return _members.Values.Where(m => m.Representative is IFieldDefinition); } }
        public IEnumerable<MemberMapping> Properties { get { return _members.Values.Where(m => m.Representative is IPropertyDefinition); } }
        public IEnumerable<MemberMapping> Events { get { return _members.Values.Where(m => m.Representative is IEventDefinition); } }
        public IEnumerable<MemberMapping> Methods { get { return _members.Values.Where(m => m.Representative is IMethodDefinition); } }

        public IEnumerable<TypeMapping> NestedTypes { get { return _nestedTypes.Values; } }

        protected override void OnMappingAdded(int index, ITypeDefinition type)
        {
            foreach (var nestedType in type.NestedTypes)
            {
                TypeMapping mapping;
                if (!_nestedTypes.TryGetValue(nestedType, out mapping))
                {
                    mapping = new TypeMapping(this.Settings);
                    _nestedTypes.Add(nestedType, mapping);
                }

                mapping.AddMapping(index, nestedType);
            }

            foreach (var member in GetMembers(type))
            {
                MemberMapping mapping;
                if (!_members.TryGetValue(member, out mapping))
                {
                    mapping = new MemberMapping(this, this.Settings);
                    _members.Add(member, mapping);
                }

                mapping.AddMapping(index, member);
            }
        }

        public MemberMapping FindMember(ITypeDefinitionMember member)
        {
            MemberMapping mapping = null;
            _members.TryGetValue(member, out mapping);
            return mapping;
        }

        private IEnumerable<ITypeDefinitionMember> GetMembers(ITypeDefinition type)
        {
            if (this.Settings.FlattenTypeMembers)
            {
                // Get the base members first.
                foreach (var baseType in type.GetAllBaseTypes())
                {
                    foreach (var m in GetOnlyMembers(baseType).Where(this.Filter.Include))
                        yield return m;
                }
            }

            foreach (var m in GetOnlyMembers(type).Where(this.Filter.Include))
                yield return m;
        }

        private IEnumerable<ITypeDefinitionMember> GetOnlyMembers(ITypeDefinition type)
        {
            // Get everything that is not a NestedType

            foreach (var m in type.Fields)
                yield return m;

            foreach (var m in type.Properties)
                yield return m;

            foreach (var m in type.Events)
                yield return m;

            foreach (var m in type.Methods)
                yield return m;
        }
    }
}
