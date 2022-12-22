// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.DiaSymReader
{
    using static InteropUtilities;

    partial class SymUnmanagedExtensions
    {
        public static ISymUnmanagedDocument[] GetDocumentsForMethod(this ISymUnmanagedMethod method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return NullToEmpty(GetItems((ISymEncUnmanagedMethod)method,
                (ISymEncUnmanagedMethod a, out int b) => a.GetDocumentsForMethodCount(out b),
                (ISymEncUnmanagedMethod a, int b, out int c, ISymUnmanagedDocument[] d) => a.GetDocumentsForMethod(b, out c, d)));
        }

        public static void GetSourceExtentInDocument(this ISymEncUnmanagedMethod method, ISymUnmanagedDocument document, out int startLine, out int endLine)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            ThrowExceptionForHR(method.GetSourceExtentInDocument(document, out startLine, out endLine));
        }

        public static int GetToken(this ISymUnmanagedMethod method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            ThrowExceptionForHR(method.GetToken(out int token));
            return token;
        }

        public static int GetLocalSignatureToken(this ISymUnmanagedMethod2 method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            ThrowExceptionForHR(method.GetLocalSignatureToken(out int token));
            return token;
        }

        public static ISymUnmanagedScope GetRootScope(this ISymUnmanagedMethod method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            ThrowExceptionForHR(method.GetRootScope(out var scope));
            return scope;
        }

        public static IEnumerable<SymUnmanagedSequencePoint> GetSequencePoints(this ISymUnmanagedMethod method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            // NB: method.GetSequencePoints(0, out numAvailable, ...) always returns 0.
            int numAvailable;
            ThrowExceptionForHR(method.GetSequencePointCount(out numAvailable));
            if (numAvailable == 0)
            {
                yield break;
            }

            int[] offsets = new int[numAvailable];
            ISymUnmanagedDocument[] documents = new ISymUnmanagedDocument[numAvailable];
            int[] startLines = new int[numAvailable];
            int[] startColumns = new int[numAvailable];
            int[] endLines = new int[numAvailable];
            int[] endColumns = new int[numAvailable];

            int numRead;
            ThrowExceptionForHR(method.GetSequencePoints(numAvailable, out numRead, offsets, documents, startLines, startColumns, endLines, endColumns));
            ValidateItems(numRead, offsets.Length);

            for (int i = 0; i < numRead; i++)
            {
                yield return new SymUnmanagedSequencePoint(
                    offsets[i],
                    documents[i],
                    startLines[i],
                    startColumns[i],
                    endLines[i],
                    endColumns[i]);
            }
        }

        public static ISymUnmanagedAsyncMethod AsAsyncMethod(this ISymUnmanagedMethod method)
        {
            var asyncMethod = method as ISymUnmanagedAsyncMethod;
            if (asyncMethod == null)
            {
                return null;
            }

            bool isAsyncMethod;
            ThrowExceptionForHR(asyncMethod.IsAsyncMethod(out isAsyncMethod));
            if (!isAsyncMethod)
            {
                return null;
            }

            return asyncMethod;
        }

        public static int GetCatchHandlerILOffset(this ISymUnmanagedAsyncMethod method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            bool hasCatchHandler;
            ThrowExceptionForHR(method.HasCatchHandlerILOffset(out hasCatchHandler));
            if (!hasCatchHandler)
            {
                return -1;
            }

            int result;
            ThrowExceptionForHR(method.GetCatchHandlerILOffset(out result));
            return result;
        }

        public static int GetKickoffMethod(this ISymUnmanagedAsyncMethod method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            int result;
            ThrowExceptionForHR(method.GetKickoffMethod(out result));
            return result;
        }

        public static IEnumerable<SymUnmanagedAsyncStepInfo> GetAsyncStepInfos(this ISymUnmanagedAsyncMethod method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            int count;
            ThrowExceptionForHR(method.GetAsyncStepInfoCount(out count));
            if (count == 0)
            {
                yield break;
            }

            var yieldOffsets = new int[count];
            var breakpointOffsets = new int[count];
            var breakpointMethods = new int[count];
            ThrowExceptionForHR(method.GetAsyncStepInfo(count, out count, yieldOffsets, breakpointOffsets, breakpointMethods));
            ValidateItems(count, yieldOffsets.Length);
            ValidateItems(count, breakpointOffsets.Length);
            ValidateItems(count, breakpointMethods.Length);

            for (int i = 0; i < count; i++)
            {
                yield return new SymUnmanagedAsyncStepInfo(yieldOffsets[i], breakpointOffsets[i], breakpointMethods[i]);
            }
        }
    }
}
