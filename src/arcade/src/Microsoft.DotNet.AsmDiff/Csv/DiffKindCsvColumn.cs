// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Mappings;

namespace Microsoft.DotNet.AsmDiff.CSV
{
    public sealed class DiffKindCsvColumn : DiffCsvColumn
    {
        public DiffKindCsvColumn(DiffConfiguration diffConfiguration)
            : base(diffConfiguration)
        {
        }

        public override string Name
        {
            get { return "Kind"; }
        }

        public override string GetValue(NamespaceMapping mapping)
        {
            return "Namespace";
        }

        public override string GetValue(TypeMapping mapping)
        {
            return "Type";
        }

        public override string GetValue(MemberMapping mapping)
        {
            return "Member";
        }
    }
}
