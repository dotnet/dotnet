// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader
{
    internal static class EmptyArray<T>
    {
        public static readonly T[] Instance = new T[0];
    }

    internal class InteropUtilities
    {
        private static readonly IntPtr s_ignoreIErrorInfo = new IntPtr(-1);

        internal static T[] NullToEmpty<T>(T[] items) => (items == null) ? EmptyArray<T>.Instance : items;

        internal static void ThrowExceptionForHR(int hr)
        {
            // E_FAIL indicates "no info".
            // E_NOTIMPL indicates a lack of ISymUnmanagedReader support (in a particular implementation).
            if (hr < 0 && hr != HResult.E_FAIL && hr != HResult.E_NOTIMPL)
            {
                Marshal.ThrowExceptionForHR(hr, s_ignoreIErrorInfo);
            }
        }

        internal unsafe static void CopyQualifiedTypeName(char* qualifiedName, int qualifiedNameBufferLength, int* qualifiedNameLength, string namespaceStr, string nameStr)
        {
            Debug.Assert(nameStr != null);

            if (namespaceStr == null)
            {
                namespaceStr = string.Empty;
            }

            if (qualifiedNameLength != null)
            {
                int fullLength = (namespaceStr.Length > 0 ? namespaceStr.Length + 1 : 0) + nameStr.Length;
                if (qualifiedName != null)
                {
                    // If the buffer is given return the length of the possibly truncated name not including NUL.
                    // If we returned length greater than the buffer length the SymWriter would replace the name with CRC32 hash.
                    *qualifiedNameLength = Math.Min(fullLength, Math.Max(0, qualifiedNameBufferLength - 1));
                }
                else
                {
                    // If the buffer is not given then return the full length.
                    *qualifiedNameLength = fullLength;
                }
            }

            if (qualifiedName != null && qualifiedNameBufferLength > 0)
            {
                char* dst = qualifiedName;
                char* dstEndPtr = dst + qualifiedNameBufferLength - 1;

                if (namespaceStr.Length > 0)
                {
                    for (int i = 0; i < namespaceStr.Length && dst < dstEndPtr; i++)
                    {
                        *dst = namespaceStr[i];
                        dst++;
                    }

                    if (dst < dstEndPtr)
                    {
                        *dst = '.';
                        dst++;
                    }
                }

                for (int i = 0; i < nameStr.Length && dst < dstEndPtr; i++)
                {
                    *dst = nameStr[i];
                    dst++;
                }

                Debug.Assert(dst <= dstEndPtr);
                *dst = '\0';
            }
        }

        // PERF: The purpose of all this code duplication is to avoid allocating any display class instances.
        // Effectively, we will use the stack frames themselves as display classes.
        internal delegate int CountGetter<TEntity>(TEntity entity, out int count);
        internal delegate int ItemsGetter<TEntity, TItem>(TEntity entity, int bufferLength, out int count, TItem[] buffer);
        internal delegate int ItemsGetter<TEntity, TArg1, TItem>(TEntity entity, TArg1 arg1, int bufferLength, out int count, TItem[] buffer);
        internal delegate int ItemsGetter<TEntity, TArg1, TArg2, TItem>(TEntity entity, TArg1 arg1, TArg2 arg2, int bufferLength, out int count, TItem[] buffer);

        internal static string BufferToString(char[] buffer)
        {
            Debug.Assert(buffer[buffer.Length - 1] == 0);
            return new string(buffer, 0, buffer.Length - 1);
        }

        internal static void ValidateItems(int actualCount, int bufferLength)
        {
            if (actualCount != bufferLength)
            {
                // TODO: localize
                throw new InvalidOperationException(string.Format("Read only {0} of {1} items.", actualCount, bufferLength));
            }
        }

        internal static TItem[] GetItems<TEntity, TItem>(TEntity entity, CountGetter<TEntity> countGetter, ItemsGetter<TEntity, TItem> itemsGetter)
        {
            int count;
            ThrowExceptionForHR(countGetter(entity, out count));
            if (count == 0)
            {
                return null;
            }

            var result = new TItem[count];
            ThrowExceptionForHR(itemsGetter(entity, count, out count, result));
            ValidateItems(count, result.Length);
            return result;
        }

        internal static TItem[] GetItems<TEntity, TItem>(TEntity entity, ItemsGetter<TEntity, TItem> getter)
        {
            int count;
            ThrowExceptionForHR(getter(entity, 0, out count, null));
            if (count == 0)
            {
                return null;
            }

            var result = new TItem[count];
            ThrowExceptionForHR(getter(entity, count, out count, result));
            ValidateItems(count, result.Length);
            return result;
        }

        internal static TItem[] GetItems<TEntity, TArg1, TItem>(TEntity entity, TArg1 arg1, ItemsGetter<TEntity, TArg1, TItem> getter)
        {
            int count;
            ThrowExceptionForHR(getter(entity, arg1, 0, out count, null));
            if (count == 0)
            {
                return null;
            }

            var result = new TItem[count];
            ThrowExceptionForHR(getter(entity, arg1, count, out count, result));
            ValidateItems(count, result.Length);
            return result;
        }

        internal static TItem[] GetItems<TEntity, TArg1, TArg2, TItem>(TEntity entity, TArg1 arg1, TArg2 arg2, ItemsGetter<TEntity, TArg1, TArg2, TItem> getter)
        {
            int count;
            ThrowExceptionForHR(getter(entity, arg1, arg2, 0, out count, null));
            if (count == 0)
            {
                return null;
            }

            var result = new TItem[count];
            ThrowExceptionForHR(getter(entity, arg1, arg2, count, out count, result));
            ValidateItems(count, result.Length);
            return result;
        }
    }
}
