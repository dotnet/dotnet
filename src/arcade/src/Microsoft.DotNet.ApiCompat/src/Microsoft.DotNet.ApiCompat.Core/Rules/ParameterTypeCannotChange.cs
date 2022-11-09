// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Extensions;
using Microsoft.Cci.Mappings;

namespace Microsoft.Cci.Differs.Rules
{
    // This rule only helps try to match up methods where a return type changed on one or the other.
    //[ExportDifferenceRule(NonAPIConformanceRule=true)]
    internal class ParameterTypeCannotChange : DifferenceRule
    {
        // TODO: Add support for property parameter lists
        public override DifferenceType Diff(IDifferences differences, MemberMapping mapping)
        {
            IMethodDefinition method1 = mapping[0] as IMethodDefinition;
            IMethodDefinition method2 = mapping[1] as IMethodDefinition;

            if (method1 == null && method2 == null)
                return DifferenceType.Unknown;

            if (method1 != null && method2 != null)
                return DifferenceType.Unknown;

            if (method1 != null)
            {
                IMethodDefinition match = FindBestMatch(method1, mapping.ContainingType, 1, 0);

                if (match != null)
                {
                    differences.AddIncompatibleDifference(this,
                        "Cannot change parameter types for method {0} and {1}", method1.GetMethodSignature(), match.GetMethodSignature());
                    return DifferenceType.Changed;
                }
            }

            if (method2 != null)
            {
                IMethodDefinition match = FindBestMatch(method2, mapping.ContainingType, 0, 1);

                if (match != null)
                {
                    differences.AddIncompatibleDifference(this,
                        "Cannot change parameter types for method {0} and {1}", method2.GetMethodSignature(), match.GetMethodSignature());
                    return DifferenceType.Changed;
                }
            }

            return DifferenceType.Unknown;
        }

        private IMethodDefinition FindBestMatch(IMethodDefinition matchMethod, TypeMapping mapping, int typeIndex, int memberIndex)
        {
            // No matches if we don't have a matching type.
            if (mapping[typeIndex] == null)
                return null;

            foreach (IMethodDefinition method in mapping[typeIndex].Methods)
            {
                if (method.Name.Value != matchMethod.Name.Value) continue;

                if (method.ParameterCount != matchMethod.ParameterCount) continue;

                if (method.IsGeneric && matchMethod.IsGeneric &&
                    method.GenericParameterCount != matchMethod.GenericParameterCount)
                    continue;

                MemberMapping member = mapping.FindMember(method);

                // It is possible to find a match that was filtered at the mapping layer
                if (member == null) continue;

                // If the other member also doesn't have a match then this is our best match
                if (member[memberIndex] == null)
                    return method;
            }

            return null;
        }
    }
}
