// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Extensions;
using Microsoft.Cci.Extensions.CSharp;
using System;
using System.Diagnostics;

namespace Microsoft.Cci.Differs.Rules
{
    [ExportDifferenceRule]
    internal class EnumValuesMustMatch : CompatDifferenceRule
    {
        public override DifferenceType Diff(IDifferences differences, ITypeDefinitionMember impl, ITypeDefinitionMember contract)
        {
            if (impl == null || contract == null)
                return DifferenceType.Unknown;

            if (!impl.ContainingTypeDefinition.IsEnum || !contract.ContainingTypeDefinition.IsEnum)
                return DifferenceType.Unknown;

            IFieldDefinition implField = impl as IFieldDefinition;
            IFieldDefinition contractField = contract as IFieldDefinition;

            Debug.Assert(implField != null || contractField != null);

            string implValue = Convert.ToString(implField.Constant.Value);
            string contractValue = Convert.ToString(contractField.Constant.Value);

            // Calling the toString method to compare in since we might have the case where one Enum is type a and the other is type b, but they might still have same value.
            if (implValue != contractValue)
            {
                ITypeReference implValType = impl.ContainingTypeDefinition.GetEnumType();
                ITypeReference contractValType = contract.ContainingTypeDefinition.GetEnumType();

                differences.AddIncompatibleDifference(this,
                    $"Enum value '{implField.FullName()}' is ({implValType.FullName()}){implField.Constant.Value} in the {Implementation} but ({contractValType.FullName()}){contractField.Constant.Value} in the {Contract}.");
                return DifferenceType.Changed;
            }

            return DifferenceType.Unknown;
        }
    }
}
