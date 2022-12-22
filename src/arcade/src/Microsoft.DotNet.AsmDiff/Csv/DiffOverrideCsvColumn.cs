// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Extensions.CSharp;
using Microsoft.Cci.Mappings;

namespace Microsoft.DotNet.AsmDiff.CSV
{
    public sealed class DiffOverrideCsvColumn : DiffCsvColumn
    {
        public DiffOverrideCsvColumn(DiffConfiguration diffConfiguration)
            : base(diffConfiguration)
        {
        }

        public override string Name
        {
            get { return "Override"; }
        }

        public override string GetValue(NamespaceMapping mapping)
        {
            return string.Empty;
        }

        public override string GetValue(TypeMapping mapping)
        {
            return string.Empty;
        }

        public override string GetValue(MemberMapping mapping)
        {
            var isOverride = mapping.Representative.IsOverride();
            return isOverride
                       ? "Yes"
                       : "No";
        }
    }
}
