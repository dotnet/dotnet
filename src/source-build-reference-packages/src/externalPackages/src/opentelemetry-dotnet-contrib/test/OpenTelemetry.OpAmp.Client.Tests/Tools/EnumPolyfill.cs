// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK

namespace OpenTelemetry.OpAmp.Client.Tests.Tools;

internal static class EnumPolyfill
{
    public static TEnum[] GetValues<TEnum>()
        where TEnum : struct, Enum
    {
        return (TEnum[])Enum.GetValues(typeof(TEnum));
    }
}

#endif
