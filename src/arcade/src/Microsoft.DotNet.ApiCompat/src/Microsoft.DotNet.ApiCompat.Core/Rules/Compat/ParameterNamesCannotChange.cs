// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Linq;
using Microsoft.Cci.Extensions;

namespace Microsoft.Cci.Differs.Rules
{
    [ExportDifferenceRule(OptionalRule = true)]
    internal class ParameterNamesCannotChange : CompatDifferenceRule
    {
        public override DifferenceType Diff(IDifferences differences, ITypeDefinitionMember impl, ITypeDefinitionMember contract)
        {
            if (impl == null || contract == null)
                return DifferenceType.Unknown;

            IMethodDefinition implMethod = impl as IMethodDefinition;
            IMethodDefinition contractMethod = contract as IMethodDefinition;

            if (implMethod == null || contractMethod == null)
                return DifferenceType.Unknown;

            if (!ParamNamesMatch(differences, implMethod, contractMethod))
                return DifferenceType.Changed;

            return DifferenceType.Unknown;
        }

        private bool ParamNamesMatch(IDifferences differences, IMethodDefinition implMethod, IMethodDefinition contractMethod)
        {
            int paramCount = implMethod.ParameterCount;

            Debug.Assert(paramCount == contractMethod.ParameterCount);

            IParameterDefinition[] implParams = implMethod.Parameters.ToArray();
            IParameterDefinition[] contractParams = contractMethod.Parameters.ToArray();

            bool match = true;
            for (int i = 0; i < paramCount; i++)
            {
                IParameterDefinition implParam = implParams[i];
                IParameterDefinition contractParam = contractParams[i];

                if (!implParam.Name.Value.Equals(contractParam.Name.Value))
                {
                    differences.AddIncompatibleDifference(this,
                        $"Parameter name on member '{implMethod.FullName()}' is '{implParam.Name.Value}' in the {Implementation} but '{contractParam.Name.Value}' in the {Contract}.");
                    match = false;
                }
            }
            return match;
        }
    }
}
