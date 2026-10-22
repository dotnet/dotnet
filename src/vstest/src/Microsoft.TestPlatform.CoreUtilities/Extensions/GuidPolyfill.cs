// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System;
/// <summary>
/// A polyfill helper for Guid.
/// </summary>
internal static class GuidPolyfill
{
    public static Guid Parse(string s, IFormatProvider? provider)
        => Guid.Parse(s
#if NET7_0_OR_GREATER
            , System.Globalization.CultureInfo.InvariantCulture
#endif
            );
}
