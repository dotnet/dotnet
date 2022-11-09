// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.Cci.Differs;
using Microsoft.Cci.Filters;
using Microsoft.Cci.Mappings;

namespace Microsoft.Cci.Traversers
{
    public class DifferenceTraverser : MappingsTypeMemberTraverser
    {
        private readonly IDifferenceFilter _filter;

        public DifferenceTraverser(MappingSettings settings, IDifferenceFilter filter)
            : base(settings)
        {
            _filter = filter;
        }

        public void Visit(IEnumerable<IAssembly> oldAssemblies, IEnumerable<IAssembly> newAssemblies)
        {
            Contract.Requires(oldAssemblies != null);
            Contract.Requires(newAssemblies != null);

            AssemblySetMapping mapping = new AssemblySetMapping(this.Settings);
            mapping.AddMappings(oldAssemblies, newAssemblies);

            Visit(mapping);
        }

        public override void Visit(AssemblySetMapping mapping)
        {
            Visit(mapping.Differences);
            base.Visit(mapping);
        }

        public override void Visit(AssemblyMapping mapping)
        {
            Visit(mapping.Differences);
            base.Visit(mapping);
        }

        public override void Visit(NamespaceMapping mapping)
        {
            Visit(mapping.Differences);
            base.Visit(mapping);
        }

        public override void Visit(TypeMapping mapping)
        {
            Visit(mapping.Differences);
            if (mapping.ShouldDiffMembers)
                base.Visit(mapping);
        }

        public override void Visit(MemberMapping mapping)
        {
            Visit(mapping.Differences);
            base.Visit(mapping);
        }

        public virtual void Visit(IEnumerable<Difference> differences)
        {
            differences = differences.Where(_filter.Include);

            foreach (var difference in differences)
                Visit(difference);
        }

        public virtual void Visit(Difference difference)
        {
        }

        protected IDifferenceFilter DifferenceFilter => _filter;
    }
}
