// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Linq;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Extensions.CSharp;

namespace Microsoft.Cci.Differs.Rules
{
    [ExportDifferenceRule]
    internal class DelegatesMustMatch : CompatDifferenceRule
    {
        [Import]
        public IEqualityComparer<ITypeReference> _typeComparer { get; set; } = null;

        public override DifferenceType Diff(IDifferences differences, ITypeDefinition impl, ITypeDefinition contract)
        {
            if (impl == null || contract == null)
                return DifferenceType.Unknown;

            if (!impl.IsDelegate || !contract.IsDelegate)
                return DifferenceType.Unknown;

            IMethodDefinition implMethod = impl.GetInvokeMethod();
            IMethodDefinition contractMethod = contract.GetInvokeMethod();

            Debug.Assert(implMethod != null && contractMethod != null);

            if (!ReturnTypesMatch(differences, implMethod, contractMethod) ||
                !ParamNamesAndTypesMatch(differences, implMethod, contractMethod))
                return DifferenceType.Changed;

            return DifferenceType.Unknown;
        }

        private bool ReturnTypesMatch(IDifferences differences, IMethodDefinition implMethod, IMethodDefinition contractMethod)
        {
            ITypeReference implReturnType = implMethod.GetReturnType();
            ITypeReference contractReturnType = contractMethod.GetReturnType();

            if (implReturnType == null || contractReturnType == null)
                return true;

            if (!_typeComparer.Equals(implReturnType, contractReturnType))
            {
                differences.AddTypeMismatchDifference("DelegateReturnTypesMustMatch", implReturnType, contractReturnType,
                    $"Return type on delegate '{implMethod.ContainingType.FullName()}' is '{implReturnType.FullName()}' in the {Implementation} but '{contractReturnType.FullName()}' in the {Contract}.");
                return false;
            }

            return true;
        }

        private bool ParamNamesAndTypesMatch(IDifferences differences, IMethodDefinition implMethod, IMethodDefinition contractMethod)
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
                    differences.AddIncompatibleDifference("DelegateParamNameMustMatch",
                        $"Parameter name on delegate '{implMethod.ContainingType.FullName()}' is '{implParam.Name.Value}' in the {Implementation} but '{contractParam.Name.Value}' in the {Contract}.");
                    match = false;
                }

                if (!_typeComparer.Equals(implParam.Type, contractParam.Type))
                {
                    differences.AddTypeMismatchDifference("DelegateParamTypeMustMatch", implParam.Type, contractParam.Type,
                        $"Type for parameter '{implParam.Name.Value}' on delegate '{implMethod.ContainingType.FullName()}' is '{implParam.Type.FullName()}' in the {Implementation} but '{contractParam.Type.FullName()}' in the {Contract}.");
                    match = false;
                }
            }
            return match;
        }
    }
}
