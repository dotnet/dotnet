// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text;
internal static class StringBuilderPolyfill
{
#if !NET7_0_OR_GREATER
    [Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Added to match new API.")]
    public static StringBuilder Append(this StringBuilder builder, IFormatProvider? provider, string? value)
        => builder.Append(value);
#endif
}
