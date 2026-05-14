// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace Microsoft.VisualStudio.SolutionPersistence.Utilities;

internal static class ParseUtilities
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static StringSpan SliceToLast(this StringSpan span, char delimiter)
    {
        int pos = span.LastIndexOf(delimiter);
        return pos < 0 ? StringSpan.Empty : span.Slice(pos);
    }

    internal static bool IsWhiteSpace(this char c) => c == '\0' || char.IsWhiteSpace(c);
}
