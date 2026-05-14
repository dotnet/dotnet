// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Extensions.CSharp;
using Microsoft.Cci.Writers.Syntax;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Cci.Writers.CSharp
{
    public partial class CSDeclarationWriter
    {
        private void WriteGenericParameters(IEnumerable<IGenericParameter> genericParameters)
        {
            if (!genericParameters.Any())
                return;

            WriteSymbol("<");
            _writer.WriteList(genericParameters, p => WriteGenericParameter(p));
            WriteSymbol(">");
        }

        private void WriteGenericParameter(IGenericParameter param)
        {
            switch (param.Variance)
            {
                case TypeParameterVariance.Contravariant:
                    WriteKeyword("in"); break;
                case TypeParameterVariance.Covariant:
                    WriteKeyword("out"); break;
            }
            WriteTypeName(param, noSpace: true);
        }

        private void WriteGenericContraints(IEnumerable<IGenericParameter> genericParams, byte? methodNullableContextValue = null)
        {
            if (!genericParams.Any())
                return;

            foreach (IGenericParameter param in genericParams)
            {
                Action[] constraints = GetConstraints(param, methodNullableContextValue).ToArray();

                if (constraints.Length <= 0)
                    continue;

                WriteSpace();
                WriteKeyword("where");
                WriteTypeName(param);
                WriteSymbol(":", true);

                _writer.WriteList(constraints, c => c());
            }
        }

        private IEnumerable<Action> GetConstraints(IGenericParameter parameter, byte? methodNullableContextValue)
        {
            parameter.Attributes.TryGetAttributeOfType(CSharpCciExtensions.NullableAttributeFullName, out ICustomAttribute nullableAttribute);
            object nullableAttributeValue = nullableAttribute.GetAttributeArgumentValue<byte>() ?? methodNullableContextValue ?? TypeNullableContextValue ?? ModuleNullableContextValue;

            if (parameter.MustBeValueType)
                yield return () => WriteKeyword("struct", noSpace: true);
            else
            {
                if (parameter.MustBeReferenceType)
                    yield return () =>
                    {
                        WriteKeyword("class", noSpace: true);

                        if (nullableAttribute != null)
                        {
                            WriteNullableSymbolForReferenceType(nullableAttributeValue, arrayIndex: 0);
                        }
                    };
            }

            // If there are no struct or class constraints and contains a nullableAttributeValue then it might have a notnull constraint
            if (!parameter.MustBeValueType && !parameter.MustBeReferenceType && nullableAttributeValue != null)
            {
                if (((byte)nullableAttributeValue & 1) != 0)
                {
                    yield return () => WriteKeyword("notnull", noSpace: true);
                }
            }

            var assemblyLocation = parameter.Locations.FirstOrDefault()?.Document?.Location;

            int constraintIndex = 0;
            foreach (var constraint in parameter.Constraints)
            {
                // Skip valuetype because we should get it above.
                if (!TypeHelper.TypesAreEquivalent(constraint, constraint.PlatformType.SystemValueType) && !parameter.MustBeValueType)
                {
                    if (assemblyLocation != null)
                    {
                        nullableAttributeValue = parameter.GetGenericParameterConstraintConstructorArgument(constraintIndex, assemblyLocation, _metadataReaderCache, CSharpCciExtensions.NullableConstructorArgumentParser) ?? nullableAttributeValue;
                    }

                    constraintIndex++;
                    yield return () => WriteTypeName(constraint, noSpace: true, nullableAttributeArgument: nullableAttributeValue ?? methodNullableContextValue ?? TypeNullableContextValue ?? ModuleNullableContextValue);
                }
            }

            // new constraint cannot be put on structs and needs to be the last constraint
            if (!parameter.MustBeValueType && parameter.MustHaveDefaultConstructor)
                yield return () => { WriteKeyword("new", noSpace: true); WriteSymbol("("); WriteSymbol(")"); };
        }
    }
}
