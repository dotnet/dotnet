// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Extensions;
using Microsoft.Cci.Extensions.CSharp;

namespace Microsoft.Cci.Differs.Rules
{
    [ExportDifferenceRule]
    internal class CannotAddAbstractMembers : CompatDifferenceRule
    {
        public override DifferenceType Diff(IDifferences differences, Mappings.MemberMapping mapping)
        {
            ITypeDefinitionMember impl = mapping[0];
            ITypeDefinitionMember contract = mapping[1];

            if (impl == null)
                return DifferenceType.Unknown;

            if (contract == null && impl.IsAbstract())
            {
                // If the type is effectively sealed then it is ok to remove abstract members
                ITypeDefinition contractType = mapping.ContainingType[1];
                // We check that interfaces have the same number of members in another rule so there is no need to check that here.
                if (contractType != null && (contractType.IsEffectivelySealed() || (contractType.IsInterface && mapping.ContainingType[0].IsInterface)))
                    return DifferenceType.Unknown;

                differences.AddIncompatibleDifference(this, impl.GetMemberViolationMessage("Member", $"is abstract in the {Implementation}", $"is missing in the {Contract}"));
                return DifferenceType.Changed;
            }
            return DifferenceType.Unknown;
        }
    }
}
