// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text;

namespace Microsoft.AspNetCore.Razor;

internal static class StringBuilderExtensions
{
    public static void SetCapacityIfLarger(this StringBuilder builder, int newCapacity)
    {
        if (builder.Capacity < newCapacity)
        {
            builder.Capacity = newCapacity;
        }
    }
}
