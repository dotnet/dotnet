// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Extensions;
using Microsoft.Cci.Mappings;

namespace Microsoft.DotNet.AsmDiff.CSV
{
    public sealed class DiffNamespaceCsvColumn : DiffCsvColumn
    {
        public DiffNamespaceCsvColumn(DiffConfiguration diffConfiguration)
            : base(diffConfiguration)
        {
        }

        public override string Name
        {
            get { return "Namespace"; }
        }

        public override string GetValue(NamespaceMapping mapping)
        {
            var namespaceDefinition = mapping.Representative;
            return namespaceDefinition == null
                       ? string.Empty
                       : namespaceDefinition.FullName();
        }

        public override string GetValue(TypeMapping mapping)
        {
            var typeDefinition = mapping.Representative;
            var namespaceDefinition = typeDefinition.GetNamespace();
            return namespaceDefinition == null
                       ? string.Empty
                       : namespaceDefinition.FullName();
        }

        public override string GetValue(MemberMapping mapping)
        {
            var typeDefinitionMember = mapping.Representative;
            var typeDefinition = typeDefinitionMember.ContainingTypeDefinition;
            var namespaceDefinition = typeDefinition.GetNamespace();
            return namespaceDefinition == null
                       ? string.Empty
                       : namespaceDefinition.FullName();
        }
    }
}
