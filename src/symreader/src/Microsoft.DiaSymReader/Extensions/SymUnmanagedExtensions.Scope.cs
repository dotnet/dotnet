// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;

namespace Microsoft.DiaSymReader
{
    using static InteropUtilities;

    partial class SymUnmanagedExtensions
    {
        public static int GetStartOffset(this ISymUnmanagedScope scope)
        {
            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }

            int startOffset;
            ThrowExceptionForHR(scope.GetStartOffset(out startOffset));
            return startOffset;
        }

        public static int GetEndOffset(this ISymUnmanagedScope scope)
        {
            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }

            int endOffset;
            ThrowExceptionForHR(scope.GetEndOffset(out endOffset));
            return endOffset;
        }

        public static ISymUnmanagedNamespace[] GetNamespaces(this ISymUnmanagedScope scope)
        {
            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }

            return NullToEmpty(GetItems(scope,
                (ISymUnmanagedScope a, int b, out int c, ISymUnmanagedNamespace[] d) => a.GetNamespaces(b, out c, d)));
        }

        public static ISymUnmanagedScope[] GetChildren(this ISymUnmanagedScope scope)
        {
            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }

            return NullToEmpty(GetItems(scope,
                (ISymUnmanagedScope a, int b, out int c, ISymUnmanagedScope[] d) => a.GetChildren(b, out c, d)));
        }

        public static ISymUnmanagedVariable[] GetLocals(this ISymUnmanagedScope scope)
        {
            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }

            return NullToEmpty(GetItems(scope,
                (ISymUnmanagedScope a, out int b) => a.GetLocalCount(out b),
                (ISymUnmanagedScope a, int b, out int c, ISymUnmanagedVariable[] d) => a.GetLocals(b, out c, d)));
        }

        public static ISymUnmanagedConstant[] GetConstants(this ISymUnmanagedScope scope)
        {
            var scope2 = scope as ISymUnmanagedScope2;
            if (scope2 == null)
            {
                if (scope == null)
                {
                    throw new ArgumentNullException(nameof(scope));
                }

                return EmptyArray<ISymUnmanagedConstant>.Instance;
            }

            return scope2.GetConstants();
        }

        public static ISymUnmanagedConstant[] GetConstants(this ISymUnmanagedScope2 scope)
        {
            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }

            return NullToEmpty(GetItems(scope,
               (ISymUnmanagedScope2 a, int b, out int c, ISymUnmanagedConstant[] d) => a.GetConstants(b, out c, d)));
        }
    }
}
