// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Composition;
using System.Linq;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Extensions.CSharp;
using Microsoft.Cci.Mappings;

namespace Microsoft.Cci.Differs.Rules
{
    [ExportDifferenceRule]
    internal class MembersMustExist : CompatDifferenceRule
    {
        [Import]
        public IEqualityComparer<ITypeReference> _typeComparer { get; set; } = null;

        public override DifferenceType Diff(IDifferences differences, MemberMapping mapping)
        {
            ITypeDefinitionMember implMember = mapping[0];
            ITypeDefinitionMember contractMember = mapping[1];

            if (!(implMember == null && contractMember != null))
                return DifferenceType.Unknown;

            // Nested types are handled separately.
            // @TODO: Events and Properties - should we consider these too (or rely on the fact that dropping one of these will also drop their accessors.)
            if (!(contractMember is IMethodDefinition || contractMember is IFieldDefinition))
                return DifferenceType.Unknown;

            string incompatibleDifferenceMessage = contractMember.GetMemberViolationMessage("Member", $"does not exist in the {Implementation}", $"it does exist in the {Contract}");

            ITypeDefinition contractType = mapping.ContainingType[0];
            if (contractType != null)
            {
                if (contractMember is IMethodDefinition contractMethod)
                {
                    // If the contract is a Explicit Interface method, we don't need to check if the method is in implementation since that will be caught by different rule.
                    if (contractMethod.IsExplicitInterfaceMethod())
                        return DifferenceType.Unknown;

                    // It is valid to promote a member from a base type up so check to see if it member exits on a base type.
                    if (FindMatchingMethodOnBase(contractType, contractMethod))
                    {
                        return DifferenceType.Unknown;
                    }
                }
            }

            differences.AddIncompatibleDifference(this, incompatibleDifferenceMessage);
            return DifferenceType.Added;
        }

        private bool FindMatchingMethodOnBase(ITypeDefinition type, IMethodDefinition method)
        {
            if (type == null || method.IsConstructor)
            {
                return false;
            }

            foreach (var baseType in type.GetAllBaseTypes())
            {
                if (FindMethodInCollection(method, baseType.Methods))
                {
                    return true;
                }
            }

            return false;
        }

        private bool FindMethodInCollection(IMethodDefinition targetMethod, IEnumerable<IMethodDefinition> collectionOfMethods)
        {
            string targetMethodName = targetMethod.Name.Value;

            foreach (IMethodDefinition potentialMatch in collectionOfMethods)
            {
                if (targetMethodName == potentialMatch.Name.Value)
                {
                    if (ParameterTypesAreEqual(potentialMatch, targetMethod))
                    {
                        if (!ReturnTypesMatch(potentialMatch, targetMethod))
                        {
                            return false;
                        }

                        if (!targetMethod.IsGeneric && !potentialMatch.IsGeneric)
                        {
                            return true;
                        }

                        if (targetMethod.GenericParameterCount == potentialMatch.GenericParameterCount)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool ParameterTypesAreEqual(IMethodDefinition implMethod, IMethodDefinition contractMethod)
        {
            IParameterDefinition[] params1 = implMethod.Parameters.ToArray();
            IParameterDefinition[] params2 = contractMethod.Parameters.ToArray();

            if (params1.Length != params2.Length)
                return false;

            for (int i = 0; i < params1.Length; i++)
            {
                IParameterDefinition param1 = params1[i];
                IParameterDefinition param2 = params2[i];

                if (!_typeComparer.Equals(param1.Type, param2.Type))
                    return false;
            }

            return true;
        }

        public bool ReturnTypesMatch(IMethodDefinition implMethod, IMethodDefinition contractMethod)
        {
            ITypeReference implType = implMethod.GetReturnType();
            ITypeReference contractType = contractMethod.GetReturnType();

            if (implType == null || contractType == null)
                return true;

            if (!_typeComparer.Equals(implType, contractType))
            {
                return false;
            }

            return true;
        }
    }
}
