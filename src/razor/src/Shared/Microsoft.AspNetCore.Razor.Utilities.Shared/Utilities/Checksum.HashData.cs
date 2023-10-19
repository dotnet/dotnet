// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.InteropServices;

namespace Microsoft.AspNetCore.Razor.Utilities;

internal sealed partial record Checksum
{
    [StructLayout(LayoutKind.Explicit, Size = HashSize)]
    public readonly record struct HashData
    {
        [FieldOffset(0)]
        public readonly long Data1;

        [FieldOffset(8)]
        public readonly long Data2;

        [FieldOffset(16)]
        public readonly long Data3;

        [FieldOffset(24)]
        public readonly long Data4;

        public HashData(long data1, long data2, long data3, long data4)
        {
            Data1 = data1;
            Data2 = data2;
            Data3 = data3;
            Data4 = data4;
        }

        public override int GetHashCode()
        {
            // The checksum is already a hash. Just provide a 4-byte value as a well-distributed hash code.
            return (int)Data1;
        }
    }
}
