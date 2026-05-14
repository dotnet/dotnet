// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Cci.Comparers;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Writers.Syntax;

namespace Microsoft.Cci.Writers.CSharp
{
    public partial class CSDeclarationWriter
    {
        private void WriteEnumValue(IMetadataConstant constant, ITypeReference constantType = null)
        {
            ITypeReference enumType = (constantType == null ? constant.Type : constantType);
            var resolvedType = enumType.ResolvedType;

            if (resolvedType != null)
            {
                // First look for exact match
                foreach (var enumField in resolvedType.Fields)
                {
                    var enumFieldValue = enumField?.Constant?.Value;
                    if (enumFieldValue != null && enumFieldValue.Equals(constant.Value))
                    {
                        WriteTypeName(enumType, noSpace: true);
                        WriteSymbol(".");
                        WriteIdentifier(enumField.Name);
                        return;
                    }
                }

                // if flags and we didn't find an exact match, find a combination of flags that match
                if (resolvedType.Attributes.Any(a => a.Type.GetTypeName() == "System.FlagsAttribute"))
                {
                    ulong value = ToULongUnchecked(constant.Value);
                    ulong satisfiedValue = 0;

                    // keep track of candidate flags
                    List<IFieldDefinition> candidateFlagFields = new List<IFieldDefinition>();

                    // ensure stable sort
                    IEnumerable<IFieldDefinition> sortedFields = resolvedType.Fields.OrderBy(f => f.Name.Value, StringComparer.OrdinalIgnoreCase);

                    foreach (var candidateFlagField in sortedFields)
                    {
                        object candidateFlagObj = candidateFlagField?.Constant?.Value;
                        if (candidateFlagObj == null)
                        {
                            continue;
                        }

                        ulong candidateFlag = ToULongUnchecked(candidateFlagObj);

                        if ((value & candidateFlag) == candidateFlag)
                        {
                            // reduce: find out if the current flag is better or worse
                            // than any of those we've already seen
                            bool shouldAdd = true;
                            for (int i = 0; i < candidateFlagFields.Count; i++)
                            {
                                ulong otherFlagValue = ToULongUnchecked(candidateFlagFields[i].Constant.Value);

                                ulong intersectingFlagValue = candidateFlag & otherFlagValue;

                                if (intersectingFlagValue == otherFlagValue)
                                {
                                    // other flag is completely redundant
                                    // remove it, but continue looking as other
                                    // flags may also be redundant
                                    candidateFlagFields.RemoveAt(i--);
                                }
                                else if (intersectingFlagValue == candidateFlag)
                                {
                                    // this flag is redundant, don't add it and stop
                                    // comparing
                                    shouldAdd = false;
                                    break;
                                }
                            }

                            if (shouldAdd)
                            {
                                candidateFlagFields.Add(candidateFlagField);
                                satisfiedValue |= candidateFlag;

                                if (value == satisfiedValue)
                                {
                                    break;
                                }
                            }
                        }
                    }

                    // we found a valid combination of flags
                    if (value == satisfiedValue && candidateFlagFields.Count > 0)
                    {
                        for (int i = 0; i < candidateFlagFields.Count; i++)
                        {
                            if (i != 0)
                            {
                                WriteSymbol(" | ");
                            }
                            WriteTypeName(enumType, noSpace: true);
                            WriteSymbol(".");
                            WriteIdentifier(candidateFlagFields[i].Name);
                        }

                        return;
                    }
                }
            }

            if (constant.Value == null || ToULongUnchecked(constant.Value) == 0) // default(T) on an enum is 0
            {
                if (enumType.IsValueType)
                {
                    // Write default(T) for value types
                    WriteDefaultOf(enumType);
                }
                else
                {
                    WriteKeyword("null", noSpace: true);
                }
            }
            else
            {
                // couldn't find a symbol for enum, just cast it
                WriteSymbol("(");
                WriteTypeName(enumType, noSpace: true);
                WriteSymbol(")");
                WriteSymbol("("); // Wrap value in parens to avoid issues with negative values
                Write(constant.Value.ToString());
                WriteSymbol(")");
            }
        }

        private static ulong ToULongUnchecked(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value is byte)
            {
                return (ulong)(byte)value;
            }
            if (value is sbyte)
            {
                return unchecked((ulong)(sbyte)value & 0xFF);
            }
            if (value is char)
            {
                return (ulong)(char)value;
            }
            if (value is ushort)
            {
                return (ulong)(ushort)value;
            }
            if (value is short)
            {
                return unchecked((ulong)(short)value & 0xFFFF);
            }
            if (value is uint)
            {
                return (ulong)(uint)value;
            }
            if (value is int)
            {
                return unchecked((ulong)(int)value & 0xFFFFFFFF);
            }
            if (value is ulong)
            {
                return (ulong)value;
            }
            if (value is long)
            {
                return unchecked((ulong)(long)value);
            }

            throw new ArgumentException($"Unsupported type {value.GetType()}", nameof(value));
        }
    }
}
