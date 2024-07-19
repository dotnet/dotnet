// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Buffers;
using System.Text;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Collection of text encoding-related helper methods.
    /// </summary>
    internal static class EncodingUtils
    {
        /// <summary>
        /// Obtains bytes from a string using the specified encoding and then performs an action on the resulting byte array.
        /// </summary>
        /// <typeparam name="T">The return type of the operation.</typeparam>
        /// <param name="input">The string to process.</param>
        /// <param name="encoding">The encoding used to obtain bytes from the string.</param>
        /// <param name="action">The operation to invoke with the result, which is a byte array and the length of useful data in the array (with offset as 0).</param>
        /// <returns>An instance of {T}.</returns>
        /// <remarks>
        /// The encoding operation uses a shared memory pool to avoid allocations.
        /// The length of the rented array of bytes may be larger than the actual encoded bytes; therefore, the action needs to know the actual length to use.
        /// The result of <see cref="Encoding.GetBytes(string, int, int, byte[], int)"/> is passed to the action.
        /// </remarks>
        internal static T PerformEncodingDependentOperation<T>(
            string input,
            Encoding encoding,
            Func<byte[], int, T> action)
        {
            return PerformEncodingDependentOperation<T>(input, 0, input.Length, encoding, action);
        }

        /// <summary>
        /// Obtains bytes from a string using the specified encoding and then performs an action on the resulting byte array.
        /// </summary>
        /// <typeparam name="T">The return type of the operation.</typeparam>
        /// <param name="input">The string to process.</param>
        /// <param name="offset">The index to start from in <paramref name="input"/>.</param>
        /// <param name="length">The length of characters to operate on in <paramref name="input"/> from <paramref name="offset"/>.</param>
        /// <param name="encoding">The encoding used to obtain bytes from the string.</param>
        /// <param name="action">The operation to invoke with the result, which is a byte array and the length of useful data in the array (with offset as 0).</param>
        /// <returns>An instance of {T}.</returns>
        /// <remarks>
        /// The encoding operation uses a shared memory pool to avoid allocations.
        /// The length of the rented array of bytes may be larger than the actual encoded bytes; therefore, the action needs to know the actual length to use.
        /// The result of <see cref="Encoding.GetBytes(string, int, int, byte[], int)"/> is passed to the action.
        /// </remarks>
        internal static T PerformEncodingDependentOperation<T>(
            string input,
            int offset,
            int length,
            Encoding encoding,
            Func<byte[], int, T> action)
        {
            int size = encoding.GetMaxByteCount(length);
            byte[] bytes = ArrayPool<byte>.Shared.Rent(size);
            try
            {
                size = encoding.GetBytes(input, offset, length, bytes, 0);
                return action(bytes, size);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(bytes);
            }
        }

        /// <summary>
        /// Obtains bytes from a string using the specified encoding and then performs an action on the resulting byte array.
        /// </summary>
        /// <typeparam name="T">The return type of the operation.</typeparam>
        /// <typeparam name="TX">The type of the first input parameter to the action.</typeparam>
        /// <typeparam name="TY">The type of the second input parameter to the action.</typeparam>
        /// <typeparam name="TZ">The type of the third input parameter to the action.</typeparam>
        /// <param name="input">The string to process.</param>
        /// <param name="offset">The index to start from in <paramref name="input"/>.</param>
        /// <param name="length">The length of characters to operate on in <paramref name="input"/> from <paramref name="offset"/>.</param>
        /// <param name="encoding">The encoding used to obtain bytes from the string.</param>
        /// <param name="argx">The first input parameter to the action.</param>
        /// <param name="argy">The second input parameter to the action.</param>
        /// <param name="argz">The third input parameter to the action.</param>
        /// <param name="action">The action to perform with the resulting byte array and the length of useful data in the array.</param>
        /// <returns>An instance of {T}.</returns>
        /// <remarks>
        /// The encoding operation uses a shared memory pool to avoid allocations.
        /// The length of the rented array of bytes may be larger than the actual encoded bytes; therefore, the action needs to know the actual length to use.
        /// The result of <see cref="Encoding.GetBytes(string, int, int, byte[], int)"/> is passed to the action.
        /// </remarks>
        internal static T PerformEncodingDependentOperation<T, TX, TY, TZ>(
            string input,
            int offset,
            int length,
            Encoding encoding,
            TX argx,
            TY argy,
            TZ argz,
            Func<byte[], int, TX, TY, TZ, T> action)
        {
            int size = encoding.GetMaxByteCount(length);
            byte[] bytes = ArrayPool<byte>.Shared.Rent(size);
            try
            {
                size = encoding.GetBytes(input, offset, length, bytes, 0);
                return action(bytes, size, argx, argy, argz);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(bytes);
            }
        }

        /// <summary>
        /// Encodes the string using the given encoding and invokes the operation with the resulting byte array.
        /// </summary>
        /// <typeparam name="T">The return type of the operation.</typeparam>
        /// <typeparam name="TX">The type of the input parameter to the operation.</typeparam>
        /// <param name="input">The string to process.</param>
        /// <param name="encoding">The encoding used to obtain bytes from the string.</param>
        /// <param name="parameter">The additional parameter for the operation.</param>
        /// <param name="action">The operation to invoke with the resulting byte array and the length of useful data in the array.</param>
        /// <returns>The result of the operation.</returns>
        /// <remarks>
        /// The encoding operation uses a shared memory pool to avoid allocations.
        /// The length of the rented array of bytes may be larger than the actual encoded bytes; therefore, the action needs to know the actual length to use.
        /// The result of <see cref="Encoding.GetBytes(string, int, int, byte[], int)"/> is passed to the action.
        /// </remarks>
        internal static T PerformEncodingDependentOperation<T, TX>(
            string input,
            Encoding encoding,
            TX parameter,
            Func<byte[], int, TX, T> action)
        {
            return PerformEncodingDependentOperation(input, 0, input.Length, encoding, parameter, action);
        }

        /// <summary>
        /// Obtains bytes from a string using the specified encoding and then performs an action on the resulting byte array.
        /// </summary>
        /// <typeparam name="T">The return type of the operation.</typeparam>
        /// <typeparam name="TX">The type of the input parameter to the operation.</typeparam>
        /// <param name="input">The string to process.</param>
        /// <param name="offset">The index to start from in <paramref name="input"/>.</param>
        /// <param name="length">The length of characters to operate on in <paramref name="input"/> from <paramref name="offset"/>.</param>
        /// <param name="encoding">The encoding used to obtain bytes from the string.</param>
        /// <param name="parameter">The additional parameter for the operation.</param>
        /// <param name="action">The operation to invoke with the resulting byte array and the length of useful data in the array.</param>
        /// <returns>An instance of {T}.</returns>
        /// <remarks>
        /// The encoding operation uses a shared memory pool to avoid allocations.
        /// The length of the rented array of bytes may be larger than the actual encoded bytes; therefore, the action needs to know the actual length to use.
        /// The result of <see cref="Encoding.GetBytes(string, int, int, byte[], int)"/> is passed to the action.
        /// </remarks>
        internal static T PerformEncodingDependentOperation<T, TX>(
            string input,
            int offset,
            int length,
            Encoding encoding,
            TX parameter,
            Func<byte[], int, TX, T> action)
        {
            
            int size = encoding.GetMaxByteCount(length);
            byte[] bytes = ArrayPool<byte>.Shared.Rent(size);
            try
            {
                size = encoding.GetBytes(input, offset, length, bytes, 0);
                return action(bytes, size, parameter);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(bytes);
            }
        }
    }
}
