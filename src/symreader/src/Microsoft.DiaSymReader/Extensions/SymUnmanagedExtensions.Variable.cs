// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;

namespace Microsoft.DiaSymReader
{
    using static InteropUtilities;

    partial class SymUnmanagedExtensions
    {
        public static int GetSlot(this ISymUnmanagedVariable local)
        {
            if (local == null)
            {
                throw new ArgumentNullException(nameof(local));
            }

            int result;
            ThrowExceptionForHR(local.GetAddressField1(out result));
            return result;
        }

        public static int GetAttributes(this ISymUnmanagedVariable local)
        {
            if (local == null)
            {
                throw new ArgumentNullException(nameof(local));
            }

            int result;
            ThrowExceptionForHR(local.GetAttributes(out result));
            return result;
        }

        public static string GetName(this ISymUnmanagedVariable local)
        {
            if (local == null)
            {
                throw new ArgumentNullException(nameof(local));
            }

            return BufferToString(GetItems(local,
                (ISymUnmanagedVariable a, int b, out int c, char[] d) => a.GetName(b, out c, d)));
        }
    }
}
