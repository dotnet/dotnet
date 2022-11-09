// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Cci.Comparers;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Extensions.CSharp;

namespace Microsoft.Cci.Differs.Rules
{
    // Look for differences in a parameter's marshaling attributes like in, out, & ref, as well as
    // potentially custom modifiers like const & volatile.
    [ExportDifferenceRule]
    internal class ParameterModifiersCannotChange : CompatDifferenceRule
    {
        public override DifferenceType Diff(IDifferences differences, ITypeDefinitionMember impl, ITypeDefinitionMember contract)
        {
            if (impl == null || contract == null)
                return DifferenceType.Unknown;

            IMethodDefinition method1 = impl as IMethodDefinition;
            IMethodDefinition method2 = contract as IMethodDefinition;

            if (method1 == null || method2 == null)
                return DifferenceType.Unknown;

            if (!CheckModifiersOnParametersAndReturnValue(differences, method1, method2))
                return DifferenceType.Changed;

            return DifferenceType.Unknown;
        }

        private bool CheckModifiersOnParametersAndReturnValue(IDifferences differences, IMethodDefinition implMethod, IMethodDefinition contractMethod)
        {
            int paramCount = implMethod.ParameterCount;

            Debug.Assert(paramCount == contractMethod.ParameterCount);

            if (paramCount == 0)
                return true;

            IParameterDefinition[] implParams = implMethod.Parameters.ToArray();
            IParameterDefinition[] contractParams = contractMethod.Parameters.ToArray();

            bool match = true;
            for (int i = 0; i < paramCount; i++)
            {
                IParameterDefinition implParam = implParams[i];
                IParameterDefinition contractParam = contractParams[i];

                //TODO: Do we care about the compatibility with marshalling attributes Out\In? They don't seem to be set consistently.

                if (GetModifier(implParam) != GetModifier(contractParam))
                {
                    differences.AddIncompatibleDifference(this,
                        $"Modifiers on parameter '{implParam.Name.Value}' on method '{implMethod.FullName()}' are '{GetModifier(implParam)}' in the {Implementation} but '{GetModifier(contractParam)}' in the {Contract}.");
                    match = false;
                }

                // Now check custom modifiers, primarily focused on const & volatile
                if (implParam.IsModified || contractParam.IsModified)
                {
                    var union = implParam.CustomModifiers
                        .Union(contractParam.CustomModifiers, ModifierComparer.Default);
                    if (implParam.CustomModifiers.Count() != union.Count())
                    {
                        differences.AddIncompatibleDifference(this,
                            $"Custom modifiers on parameter '{implParam.Name.Value}' on method '{implMethod.FullName()}' are '{PrintCustomModifiers(implParam.CustomModifiers)}' in the {Implementation} but '{PrintCustomModifiers(contractParam.CustomModifiers)}' in the {Contract}.");
                        match = false;
                    }
                }
            }

            string implReturnModifier = GetReturnValueModifier(implMethod);
            string contractReturnModifier = GetReturnValueModifier(contractMethod);

            if (implReturnModifier != contractReturnModifier)
            {
                differences.AddIncompatibleDifference(this,
                    $"Modifiers on return type of method '{implMethod.FullName()}' are '{implReturnModifier}' in the {Implementation} but '{contractReturnModifier}' in the {Contract}.");
                match = false;
            }

            return match;
        }

        private string GetModifier(IParameterDefinition parameter)
        {
            if (parameter.IsOut && !parameter.IsIn && parameter.IsByReference)
            {
                return "out";
            }
            else if (parameter.IsByReference)
            {
                if (parameter.Attributes.HasIsReadOnlyAttribute())
                {
                    return "in";
                }

                return "ref";
            }

            return "<none>";
        }

        private String PrintCustomModifiers(IEnumerable<ICustomModifier> modifiers)
        {
            String s = String.Join(", ", modifiers.Select(m => m.Modifier.FullName()));
            if (String.IsNullOrEmpty(s))
                return "<no custom modifiers>";
            return s;
        }

        private string GetReturnValueModifier(IMethodDefinition method)
        {
            string modifier = "<none>";
            if (method.ReturnValueIsByRef)
            {
                modifier = "ref";

                if (method.ReturnValueAttributes.HasIsReadOnlyAttribute())
                    modifier += " readonly";
            }
            return modifier;
        }

        private class ModifierComparer : IEqualityComparer<ICustomModifier>
        {
            private readonly static IEqualityComparer<ITypeReference> TypeComparer =
                CciComparers.Default.GetEqualityComparer<ITypeReference>();

            private ModifierComparer() { }

            public static ModifierComparer Default = new ModifierComparer();

            public bool Equals(ICustomModifier x, ICustomModifier y)
            {
                if (x == null || y == null)
                {
                    return x == null && y == null;
                }

                return x.IsOptional == y.IsOptional &&
                    TypeComparer.Equals(x.Modifier, y.Modifier);
            }

            public int GetHashCode(ICustomModifier obj)
            {
                if (obj == null)
                {
                    return 0;
                }

                return (obj.IsOptional ? 1 : 0) | TypeComparer.GetHashCode(obj.Modifier) << 1;
            }
        }
    }
}
