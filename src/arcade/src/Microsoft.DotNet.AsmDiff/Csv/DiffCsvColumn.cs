// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.ObjectModel;
using Microsoft.Cci.Mappings;

namespace Microsoft.DotNet.AsmDiff.CSV
{
    public abstract class DiffCsvColumn : IDiffCsvColumn
    {
        protected DiffCsvColumn(DiffConfiguration diffConfiguration)
        {
            DiffConfiguration = diffConfiguration;
        }

        public DiffConfiguration DiffConfiguration { get; private set; }

        public virtual bool IsVisible
        {
            get { return true; }
        }

        public abstract string Name { get; }

        public string GetValue<TElement>(ElementMapping<TElement> mapping) where TElement : class
        {
            var namespaceMapping = mapping as NamespaceMapping;
            if (namespaceMapping != null)
                return GetValue(namespaceMapping);

            var typeMapping = mapping as TypeMapping;
            if (typeMapping != null)
                return GetValue(typeMapping);

            var memberMapping = mapping as MemberMapping;
            if (memberMapping != null)
                return GetValue(memberMapping);

            throw new ArgumentException("Unexpected mapping", "mapping");
        }

        public virtual string GetValue(NamespaceMapping mapping)
        {
            return null;
        }

        public virtual string GetValue(TypeMapping mapping)
        {
            return null;
        }

        public virtual string GetValue(MemberMapping mapping)
        {
            return null;
        }

        public static ReadOnlyCollection<IDiffCsvColumn> CreateStandardColumns(DiffConfiguration diffConfiguration)
        {
            return new ReadOnlyCollection<IDiffCsvColumn>(new IDiffCsvColumn[]
            {
                new DiffIdCsvColumn(diffConfiguration),
                new DiffKindCsvColumn(diffConfiguration),
                new DiffSubKindCsvColumn(diffConfiguration),
                new DiffStaticCsvColumn(diffConfiguration),
                new DiffVirtualityCsvColumn(diffConfiguration),
                new DiffOverrideCsvColumn(diffConfiguration),
                new DiffUnsafeCsvColumn(diffConfiguration),
                new DiffObsoletionCsvColumn(diffConfiguration), 
                new DiffInOldCsvColumn(diffConfiguration),
                new DiffInNewCsvColumn(diffConfiguration),
                new DiffDifferenceCsvColumn(diffConfiguration),
                new DiffOldAssemblyCsvColumn(diffConfiguration),
                new DiffNewAssemblyCsvColumn(diffConfiguration),
                new DiffNamespaceCsvColumn(diffConfiguration),
                new DiffTypeCsvColumn(diffConfiguration),
                new DiffMemberCsvColumn(diffConfiguration),
                new DiffVisibilityCsvColumn(diffConfiguration),
                new DiffTypeIdCsvColumn(diffConfiguration),
                new DiffTypeIsExposedCsvColumn(diffConfiguration),
                new DiffReturnTypeCsvColumn(diffConfiguration),
                new DiffTokensCsvColumn(diffConfiguration)
            });  
        }
    }
}
