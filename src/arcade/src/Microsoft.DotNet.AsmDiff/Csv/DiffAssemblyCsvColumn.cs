// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Mappings;

namespace Microsoft.DotNet.AsmDiff.CSV
{
    public abstract class DiffAssemblyCsvColumn : DiffCsvColumn
    {
        private int _index;

        protected DiffAssemblyCsvColumn(DiffConfiguration diffConfiguration, int index)
            : base(diffConfiguration)
        {
            _index = index;
        }

        public override bool IsVisible
        {
            get { return _index == 0 || DiffConfiguration.IsDiff; }
        }

        public override string Name
        {
            get
            {
                switch (_index)
                {
                    case 0:
                        return DiffConfiguration.IsDiff
                                   ? "OldAssembly"
                                   : "Assembly";
                    case 1:
                        return "NewAssembly";

                    default:
                        throw new ArgumentException();
                }
            }
        }

        public override string GetValue(TypeMapping mapping)
        {
            var assembly = _index < mapping.ElementCount && mapping[_index] != null
                               ? mapping[_index].GetAssembly()
                               : null;
            return assembly == null ? string.Empty : assembly.Name.Value;
        }

        public override string GetValue(MemberMapping mapping)
        {
            var containingTypeDefinition = _index < mapping.ElementCount && mapping[_index] != null
                               ? mapping[_index].ContainingTypeDefinition
                               : null;

            var assembly = containingTypeDefinition == null
                               ? null
                               : containingTypeDefinition.GetAssembly();

            return assembly == null ? string.Empty : assembly.Name.Value;
        }
    }
}
