// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.
using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader
{
    using static InteropUtilities;

    partial class SymUnmanagedExtensions
    {
        public static string GetName(this ISymUnmanagedDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return BufferToString(GetItems(document,
                (ISymUnmanagedDocument a, int b, out int c, char[] d) => a.GetUrl(b, out c, d)));
        }

        public static byte[] GetChecksum(this ISymUnmanagedDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return NullToEmpty(GetItems(document,
                (ISymUnmanagedDocument a, int b, out int c, byte[] d) => a.GetChecksum(b, out c, d)));
        }

        public static Guid GetLanguage(this ISymUnmanagedDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            Guid result = default(Guid);
            ThrowExceptionForHR(document.GetLanguage(ref result));
            return result;
        }

        public static Guid GetLanguageVendor(this ISymUnmanagedDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            Guid result = default(Guid);
            ThrowExceptionForHR(document.GetLanguageVendor(ref result));
            return result;
        }

        public static Guid GetDocumentType(this ISymUnmanagedDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            Guid result = default(Guid);
            ThrowExceptionForHR(document.GetDocumentType(ref result));
            return result;
        }

        public static Guid GetHashAlgorithm(this ISymUnmanagedDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            Guid result = default(Guid);
            ThrowExceptionForHR(document.GetChecksumAlgorithmId(ref result));
            return result;
        }

        /// <summary>
        /// Returns the embedded source for specified document.
        /// </summary>
        /// <param name="document">The document to read source of.</param>
        /// <returns>
        /// default(<see cref="ArraySegment{T}"/>) if the document doesn't have embedded source, 
        /// otherwise byte array segment containing the source.
        /// </returns>
        public static ArraySegment<byte> GetEmbeddedSource(this ISymUnmanagedDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            Marshal.ThrowExceptionForHR(document.GetSourceLength(out int length));
            if (length == 0)
            {
                return default(ArraySegment<byte>);
            }

            if (length < sizeof(int))
            {
                throw new InvalidDataException();
            }

            var blob = new byte[length];
            Marshal.ThrowExceptionForHR(document.GetSourceRange(0, 0, int.MaxValue, int.MaxValue, length, out int bytesRead, blob));
            if (bytesRead < sizeof(int) || bytesRead > blob.Length)
            {
                throw new InvalidDataException();
            }

            int uncompressedLength = BitConverter.ToInt32(blob, 0);
            if (uncompressedLength == 0)
            {
                return new ArraySegment<byte>(blob, sizeof(int), bytesRead - sizeof(int));
            }

            var uncompressedBytes = new byte[uncompressedLength];

            var compressed = new MemoryStream(blob, sizeof(int), bytesRead - sizeof(int));
            using (var decompressor = new DeflateStream(compressed, CompressionMode.Decompress))
            {
                int position = 0;

                while (true)
                {
                    int bytesDecompressed = decompressor.Read(uncompressedBytes, position, uncompressedBytes.Length - position);
                    if (bytesDecompressed == 0)
                    {
                        break;
                    }

                    position += bytesDecompressed;
                }

                if (position != uncompressedBytes.Length  || decompressor.ReadByte() != -1)
                {
                    throw new InvalidDataException();
                }

                return new ArraySegment<byte>(uncompressedBytes);
            }
        }
    }
}
