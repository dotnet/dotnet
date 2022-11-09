// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Extensions;
using Microsoft.Cci.Mappings;

namespace Microsoft.DotNet.AsmDiff.CSV
{
    public sealed class DiffTypeIdCsvColumn : DiffCsvColumn
    {
        public DiffTypeIdCsvColumn(DiffConfiguration diffConfiguration)
            : base(diffConfiguration)
        {
        }

        public override string Name
        {
            get { return "TypeId"; }
        }

        public override string GetValue(NamespaceMapping mapping)
        {
            return string.Empty;
        }

        public override string GetValue(TypeMapping mapping)
        {
            var typeDefinition = mapping.Representative;
            return typeDefinition.DocId();
        }

        public override string GetValue(MemberMapping mapping)
        {
            var typeDefinitionMember = mapping.Representative;
            var typeDefinition = typeDefinitionMember.ContainingTypeDefinition;
            return typeDefinition.DocId();
        }
    }
}
