// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading;
using Microsoft.Cci;
using Microsoft.Cci.Mappings;
using Microsoft.Cci.Traversers;
using Microsoft.Cci.Writers;
using Microsoft.DotNet.AsmDiff.CSV;

namespace Microsoft.DotNet.AsmDiff
{
    internal sealed class DiffCsvWriter : MappingsTypeMemberTraverser, ICciDifferenceWriter
    {
        private CsvWriter _writer;
        private IEnumerable<IDiffCsvColumn> _columns;
        private CancellationToken _cancellationToken;

        public DiffCsvWriter(CsvWriter writer, MappingSettings settings, IEnumerable<IDiffCsvColumn> columns, CancellationToken cancellationToken)
            : base(settings)
        {
            _writer = writer;
            _columns = columns;
            _cancellationToken = cancellationToken;
        }

        public void Write(string oldAssembliesName, IEnumerable<IAssembly> oldAssemblies, string newAssembliesName, IEnumerable<IAssembly> newAssemblies)
        {
            AssemblySetMapping mapping;
            if (!string.IsNullOrEmpty(newAssembliesName))
            {
                Settings.ElementCount = 2;
                mapping = new AssemblySetMapping(Settings);
                mapping.AddMappings(oldAssemblies, newAssemblies);
            }
            else
            {
                Settings.ElementCount = 1;
                mapping = new AssemblySetMapping(Settings);
                mapping.AddMapping(0, oldAssemblies);
            }

            Visit(mapping);
        }

        private void WriteRow<TElement>(ElementMapping<TElement> mapping) where TElement : class
        {
            foreach (var mappingCsvColumn in _columns)
            {
                var value = mappingCsvColumn.GetValue(mapping);
                _writer.Write(value);
            }

            _writer.WriteLine();
        }

        public override void Visit(NamespaceMapping mapping)
        {
            if (_cancellationToken.IsCancellationRequested)
                return;

            if (DiffFilter.Include(mapping.Difference))
                WriteRow(mapping);

            base.Visit(mapping);
        }

        public override void Visit(TypeMapping mapping)
        {
            if (_cancellationToken.IsCancellationRequested)
                return;

            if (DiffFilter.Include(mapping.Difference))
                WriteRow(mapping);

            if (mapping.ShouldDiffMembers)
                base.Visit(mapping);
        }

        public override void Visit(MemberMapping mapping)
        {
            if (_cancellationToken.IsCancellationRequested)
                return;

            if (DiffFilter.Include(mapping.Difference))
                WriteRow(mapping);

            base.Visit(mapping);
        }
    }
}
