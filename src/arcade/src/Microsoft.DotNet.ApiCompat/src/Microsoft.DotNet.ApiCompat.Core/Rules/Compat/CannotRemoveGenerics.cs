// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Cci.Extensions;
using System.Linq;
using System.Collections.Generic;

namespace Microsoft.Cci.Differs.Rules
{
    // @todo: More thinking needed to see whether this is really breaking.
    //[ExportDifferenceRule]
    internal class CannotRemoveGenerics : CompatDifferenceRule
    {
        public override DifferenceType Diff(IDifferences differences, ITypeDefinition impl, ITypeDefinition contract)
        {
            if (impl == null || contract == null)
                return DifferenceType.Unknown;

            return DiffConstraints(differences, impl, impl.GenericParameters, contract.GenericParameters);
        }

        public override DifferenceType Diff(IDifferences differences, ITypeDefinitionMember impl, ITypeDefinitionMember contract)
        {
            return Diff(differences, impl as IMethodDefinition, contract as IMethodDefinition);
        }

        private DifferenceType Diff(IDifferences differences, IMethodDefinition implMethod, IMethodDefinition contractMethod)
        {
            if (implMethod == null || contractMethod == null)
                return DifferenceType.Unknown;

            return DiffConstraints(differences, implMethod, implMethod.GenericParameters, contractMethod.GenericParameters);
        }

        private DifferenceType DiffConstraints(IDifferences differences, IReference target, IEnumerable<IGenericParameter> implGenericParams, IEnumerable<IGenericParameter> contractGenericParams)
        {
            int beforeCount = differences.Count();
            IGenericParameter[] implParams = implGenericParams.ToArray();
            IGenericParameter[] contractParams = contractGenericParams.ToArray();

            // We shouldn't hit this because the types/members shouldn't be matched up if they have different generic argument lists
            if (implParams.Length != contractParams.Length)
                return DifferenceType.Changed;

            for (int i = 0; i < implParams.Length; i++)
            {
                IGenericParameter implParam = implParams[i];
                IGenericParameter contractParam = contractParams[i];

                if (contractParam.Variance != TypeParameterVariance.NonVariant &&
                    contractParam.Variance != implParam.Variance)
                {
                    differences.AddIncompatibleDifference("CannotChangeVariance",
                        $"Variance on generic parameter '{implParam.FullName()}' for '{target.FullName()}' is '{implParam.Variance}' in the {Implementation} but '{contractParam.Variance}' in the {Contract}.");
                }

                string implConstraints = string.Join(",", GetConstraints(implParam).OrderBy(s => s, StringComparer.OrdinalIgnoreCase));
                string contractConstraints = string.Join(",", GetConstraints(contractParam).OrderBy(s => s, StringComparer.OrdinalIgnoreCase));

                if (!string.Equals(implConstraints, contractConstraints))
                {
                    differences.AddIncompatibleDifference("CannotChangeGenericConstraints",
                        $"Constraints for generic parameter '{implParam.FullName()}' for '{target.FullName()}' is '{implConstraints}' in the {Implementation} but '{contractConstraints}' in the {Contract}.");
                }
            }

            if (differences.Count() != beforeCount)
                return DifferenceType.Changed;

            return DifferenceType.Unknown;
        }

        private IEnumerable<string> GetConstraints(IGenericParameter parameter)
        {
            if (parameter.MustBeValueType)
                yield return "struct";
            else
            {
                if (parameter.MustBeReferenceType)
                    yield return "class";

                if (parameter.MustHaveDefaultConstructor)
                    yield return "new()";
            }

            foreach (var constraint in parameter.Constraints)
            {
                // Skip valuetype because we should get it above.
                if (TypeHelper.TypesAreEquivalent(constraint, constraint.PlatformType.SystemValueType) && parameter.MustBeValueType)
                    continue;

                yield return constraint.FullName();
            }
        }
    }
}
