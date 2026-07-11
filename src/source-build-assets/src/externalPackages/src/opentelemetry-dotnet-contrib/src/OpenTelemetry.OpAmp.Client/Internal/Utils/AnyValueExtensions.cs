// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpAmp.Proto.V1;
using OpenTelemetry.OpAmp.Client.Internal.Settings;
using OpenTelemetry.OpAmp.Client.Settings;

namespace OpenTelemetry.OpAmp.Client.Internal.Utils;

internal static class AnyValueExtensions
{
    public static AnyValue ToAnyValue(this AnyValueUnion value) => value.Type switch
    {
        AnyValueType.String => new AnyValue() { StringValue = value.StringValue },
        AnyValueType.Integer => new AnyValue() { IntValue = value.IntValue!.Value },
        AnyValueType.Double => new AnyValue() { DoubleValue = value.DoubleValue!.Value },
        AnyValueType.Boolean => new AnyValue() { BoolValue = value.BoolValue!.Value },
        AnyValueType.Array => ConvertToArray(value),
        _ => throw new NotSupportedException($"Unsupported AnyValue type: {value.Type}"),
    };

    private static AnyValue ConvertToArray(AnyValueUnion value)
    {
        var anyValue = new AnyValue
        {
            ArrayValue = new ArrayValue(),
        };

        Debug.Assert(value.ArrayValue != null, "ArrayValue should not be null in case of AnyValueType.Array");

#pragma warning disable IDE0370 // Remove unnecessary suppression
        foreach (var item in value.ArrayValue!)
#pragma warning restore IDE0370 // Remove unnecessary suppression
        {
            anyValue.ArrayValue.Values.Add(item.ToAnyValue());
        }

        return anyValue;
    }
}
