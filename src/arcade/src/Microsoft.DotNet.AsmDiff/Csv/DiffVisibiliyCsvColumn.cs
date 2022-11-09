// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci;
using Microsoft.Cci.Mappings;

namespace Microsoft.DotNet.AsmDiff.CSV
{
    public sealed class DiffVisibilityCsvColumn : DiffCsvColumn
    {
        public DiffVisibilityCsvColumn(DiffConfiguration diffConfiguration)
            : base(diffConfiguration)
        {
        }

        public override string Name
        {
            get { return "Visibility"; }
        }

        public override string GetValue(TypeMapping mapping)
        {
            var visibility = TypeHelper.TypeVisibilityAsTypeMemberVisibility(mapping.Representative);
            return visibility.ToString();
        }

        public override string GetValue(MemberMapping mapping)
        {
            var visibility = mapping.Representative.Visibility;
            return visibility.ToString();
        }
    }
}
