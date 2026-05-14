// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;

namespace Microsoft.DiaSymReader
{
    using static InteropUtilities;

    partial class SymUnmanagedExtensions
    {
        public static string GetName(this ISymUnmanagedConstant constant)
        {
            if (constant == null)
            {
                throw new ArgumentNullException(nameof(constant));
            }

            return BufferToString(GetItems(constant,
                (ISymUnmanagedConstant a, int b, out int c, char[] d) => a.GetName(b, out c, d)));
        }

        public static object GetValue(this ISymUnmanagedConstant constant)
        {
            if (constant == null)
            {
                throw new ArgumentNullException(nameof(constant));
            }

            object value;
            ThrowExceptionForHR(constant.GetValue(out value));
            return value;
        }

        public static byte[] GetSignature(this ISymUnmanagedConstant constant)
        {
            if (constant == null)
            {
                throw new ArgumentNullException(nameof(constant));
            }

            return NullToEmpty(GetItems(constant,
                (ISymUnmanagedConstant a, int b, out int c, byte[] d) => a.GetSignature(b, out c, d)));
        }
    }
}
