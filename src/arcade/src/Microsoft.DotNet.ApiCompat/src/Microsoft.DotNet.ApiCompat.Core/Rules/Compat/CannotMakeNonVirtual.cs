// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Extensions;
using Microsoft.Cci.Extensions.CSharp;

namespace Microsoft.Cci.Differs.Rules
{
    [ExportDifferenceRule]
    internal class CannotMakeNonVirtual : CompatDifferenceRule
    {
        public override DifferenceType Diff(IDifferences differences, ITypeDefinitionMember impl, ITypeDefinitionMember contract)
        {
            if (impl == null || contract == null)
                return DifferenceType.Unknown;

            bool isImplOverridable = IsOverridable(impl);
            bool isContractOverridable = IsOverridable(contract);

            /*
            //@todo: Move to a separate rule that's only run in "strict" mode.
            if (isImplInhertiable && !isContractInheritiable)
            {
                // This is separate because it can be noisy and is generally allowed as long as it is reviewed properly.
                differences.AddIncompatibleDifference("CannotMakeMemberVirtual",
                    "Member '{0}' is virtual in the implementation but non-virtual in the contract.",
                    impl.FullName());

                return DifferenceType.Changed;
            }
            */

            if (isContractOverridable && !isImplOverridable)
            {
                differences.AddIncompatibleDifference("CannotMakeMemberNonVirtual", impl.GetMemberViolationMessage("Member", $"is non-virtual in the {Implementation}", $"is virtual in the {Contract}"));
                return DifferenceType.Changed;
            }

            return DifferenceType.Unknown;
        }

        private bool IsOverridable(ITypeDefinitionMember member)
        {
            if (!member.IsVirtual())
                return false;

            // member virtual final is not overridable
            if (member.IsSealed())
                return false;

            // if member type is Effectively sealed and cannot be extended, then the member cannot be inherited
            if (member.ContainingTypeDefinition != null && member.ContainingTypeDefinition.IsEffectivelySealed())
                return false;

            return true;
        }
    }
}
