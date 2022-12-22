// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using Microsoft.Cci;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Mappings;

namespace Microsoft.DotNet.AsmDiff.CSV
{
    public sealed class DiffObsoletionCsvColumn : DiffCsvColumn
    {
        public DiffObsoletionCsvColumn(DiffConfiguration diffConfiguration)
            : base(diffConfiguration)
        {
        }

        public override string Name
        {
            get { return "Obsoleted"; }
        }

        private static bool IsObsoleted(IReference reference)
        {
            if (reference == null)
                return false;

            return (from a in reference.Attributes
                    where a.Type.FullName() == "System.ObsoleteAttribute"
                    select a).Any();
        }

        private string GetObsoletionMarker<T>(ElementMapping<T> mapping)
            where T : class, IReference
        {
            if (!DiffConfiguration.IsDiff)
            {
                var obsoleted = IsObsoleted(mapping.Representative);
                return obsoleted ? "Yes" : "No";
            }

            var oldObsoleted = IsObsoleted(mapping[0]);
            var newObsoleted = IsObsoleted(mapping[1]);
            if (oldObsoleted && newObsoleted)
                return "Both";
            else if (!oldObsoleted && !newObsoleted)
                return "None";
            else if (oldObsoleted)
                return "Old";
            else
                return "New";
        }

        public override string GetValue(NamespaceMapping mapping)
        {
            return GetObsoletionMarker(mapping);
        }

        public override string GetValue(TypeMapping mapping)
        {
            return GetObsoletionMarker(mapping);
        }

        public override string GetValue(MemberMapping mapping)
        {
            return GetObsoletionMarker(mapping);
        }
    }
}
