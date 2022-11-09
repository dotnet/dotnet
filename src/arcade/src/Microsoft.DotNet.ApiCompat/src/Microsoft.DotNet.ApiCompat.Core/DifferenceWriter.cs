// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Cci.Differs;
using Microsoft.Cci.Filters;
using Microsoft.Cci.Mappings;
using Microsoft.Cci.Traversers;

namespace Microsoft.Cci.Writers
{
    public class DifferenceWriter : DifferenceTraverser, ICciDifferenceWriter
    {
        private readonly List<Difference> _differences;
        private readonly TextWriter _writer;
        private int _totalDifferences = 0;
        private readonly bool _usesMSBuildLog;

        public static int ExitCode { get; set; }

        [Import]
        public IDifferenceOperands Operands { get; set; }

        public DifferenceWriter(TextWriter writer, MappingSettings settings, IDifferenceFilter filter, bool usesMSBuildLog = false)
            : base(settings, filter)
        {
            _writer = writer;
            _differences = new List<Difference>();
            _usesMSBuildLog = usesMSBuildLog;
        }

        public void Write(string oldAssembliesName, IEnumerable<IAssembly> oldAssemblies, string newAssembliesName, IEnumerable<IAssembly> newAssemblies)
        {
            this.Visit(oldAssemblies, newAssemblies);

            if (!this.Settings.GroupByAssembly)
            {
                if (_differences.Count > 0)
                {
                    string header = $"Compat issues between {Operands.Implementation} set {oldAssembliesName} and {Operands.Contract} set {newAssembliesName}:";
                    OutputDifferences(header, _differences);
                    _totalDifferences += _differences.Count;
                    _differences.Clear();
                }
            }

            if (DifferenceFilter is BaselineDifferenceFilter filter)
            {
                var unusedBaselineDifferences = filter.GetUnusedBaselineDifferences();
                if (unusedBaselineDifferences.Any())
                {
                    _writer.WriteLine($"{Environment.NewLine}*** Invalid/Unused baseline differences ***");
                    foreach (var diff in unusedBaselineDifferences)
                    {
                        _writer.WriteLine(diff);
                        _totalDifferences++;
                    }
                }
            }

            // Don't use the msbuild logging infrastructure to write as that would be promoted as an error.
            // We don't want the task to log anything except failures when not writing into a baseline file.
            if (!_usesMSBuildLog)
            {
                _writer.WriteLine("Total Issues: {0}", _totalDifferences);
            }

            _totalDifferences = 0;
        }

        public override void Visit(AssemblyMapping mapping)
        {
            Debug.Assert(_differences.Count == 0);

            base.Visit(mapping);

            if (this.Settings.GroupByAssembly)
            {
                if (_differences.Count > 0)
                {
                    string header = string.Format("Compat issues with assembly {0}:", mapping.Representative.Name.Value);
                    OutputDifferences(header, _differences);
                    _totalDifferences += _differences.Count;
                    _differences.Clear();
                }
            }
        }

        private void OutputDifferences(string header, IEnumerable<Difference> differences)
        {
            _writer.WriteLine(header);

            foreach (var diff in differences)
                _writer.WriteLine(diff.ToString());
        }

        public override void Visit(Difference difference)
        {
            _differences.Add(difference);

            // For now use this to set the ExitCode to 2 if there are any differences
            DifferenceWriter.ExitCode = 2;
        }
    }
}
