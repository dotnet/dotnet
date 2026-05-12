// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

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
        _ => throw new NotSupportedException($"Unsupported AnyValue type: {value.Type}"),
    };
}
