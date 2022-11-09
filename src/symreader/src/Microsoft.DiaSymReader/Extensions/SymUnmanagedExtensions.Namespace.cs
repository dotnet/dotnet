// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;

namespace Microsoft.DiaSymReader
{
    using static InteropUtilities;

    partial class SymUnmanagedExtensions
    {
        public static string GetName(this ISymUnmanagedNamespace @namespace)
        {
            if (@namespace == null)
            {
                throw new ArgumentNullException(nameof(@namespace));
            }

            return BufferToString(GetItems(@namespace,
                (ISymUnmanagedNamespace a, int b, out int c, char[] d) => a.GetName(b, out c, d)));
        }
    }
}
