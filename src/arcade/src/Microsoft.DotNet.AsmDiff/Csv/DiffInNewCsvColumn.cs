// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Cci.Differs;
using Microsoft.Cci.Mappings;

namespace Microsoft.DotNet.AsmDiff.CSV
{
    public sealed class DiffInNewCsvColumn : DiffCsvColumn
    {
        public DiffInNewCsvColumn(DiffConfiguration diffConfiguration)
            : base(diffConfiguration)
        {
        }

        public override bool IsVisible
        {
            get { return DiffConfiguration.IsDiff; }
        }

        public override string Name
        {
            get { return "In " + DiffConfiguration.Right.Name; }
        }

        private static string InNew(DifferenceType differenceType)
        {
            switch (differenceType)
            {
                case DifferenceType.Unchanged:
                case DifferenceType.Added:
                case DifferenceType.Changed:
                    return "Yes";

                case DifferenceType.Removed:
                    return "No";

                default:
                    throw new ArgumentOutOfRangeException("differenceType");
            }
        }

        public override string GetValue(NamespaceMapping mapping)
        {
            return InNew(mapping.Difference);
        }

        public override string GetValue(TypeMapping mapping)
        {
            return InNew(mapping.Difference);
        }

        public override string GetValue(MemberMapping mapping)
        {
            return InNew(mapping.Difference);
        }
    }
}
