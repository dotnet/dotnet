// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Extensions;
using Microsoft.Cci.Extensions.CSharp;
using System.Linq;
using System.Collections.Generic;
using System.Composition;
using Microsoft.Cci.Comparers;
using System;

namespace Microsoft.Cci.Differs.Rules
{
    [ExportDifferenceRule]
    internal class InterfacesShouldHaveSameMembers : CompatDifferenceRule
    {
        [Import]
        public IRuleSettings RuleSettings { get; set; }

        public override DifferenceType Diff(IDifferences differences, ITypeDefinitionMember impl, ITypeDefinitionMember contract)
        {
            if (contract != null && impl == null)
            {
                if (contract.ContainingTypeDefinition.IsInterface)
                {
                    differences.AddIncompatibleDifference(this, contract.GetMemberViolationMessage($"{GetNameOfInterfaceMemberType(contract)}", $"is present in the {Contract}", $"not in the {Implementation}"));
                    return DifferenceType.Changed;
                }
            }

            if (impl != null && contract == null)
            {
                if (impl.ContainingTypeDefinition.IsInterface && !CanIgnoreAddedInterfaceMember(impl))
                {
                    differences.AddIncompatibleDifference(this, impl.GetMemberViolationMessage($"{GetNameOfInterfaceMemberType(impl)}", $"is present in the {Implementation}", $"not in the {Contract}"));
                    return DifferenceType.Changed;
                }
            }

            return base.Diff(differences, impl, contract);
        }

        private string GetNameOfInterfaceMemberType(ITypeDefinitionMember member)
        {
            return $"{(IsDefaultImplementationMethod(member) ? "Default interface" : "Interface")} member";
        }

        private bool CanIgnoreAddedInterfaceMember(ITypeDefinitionMember member)
        {
            if (!RuleSettings.AllowDefaultInterfaceMethods)
            {
                return false;
            }

            return IsDefaultImplementationMethod(member);
        }

        private bool IsDefaultImplementationMethod(ITypeDefinitionMember member)
        {
            // Default Implementation Method (DIM) scenario.
            // On DIM, static fields or methods that are not abstract,
            // have conditional implementation and should not be considered a break
            if (member is IFieldDefinition field)
            {
                return field.IsStatic;
            }

            if (member is IMethodDefinition method)
            {
                return !method.IsAbstract;
            }

            // If Getter or Setter exist, verify it is not Abstract
            if (member is IPropertyDefinition prop)
            {
                if ((prop.Getter != null && ((IMethodDefinition)prop.Getter).IsAbstract) || (prop.Setter != null && ((IMethodDefinition)prop.Setter).IsAbstract))
                {
                    return false;
                }

                return true;
            }

            // If Adder or Remover exist, verify it is not Abstract
            if (member is IEventDefinition evt)
            {
                if ((evt.Adder != null && ((IMethodDefinition)evt.Adder).IsAbstract) || (evt.Remover != null && ((IMethodDefinition)evt.Remover).IsAbstract))
                {
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
