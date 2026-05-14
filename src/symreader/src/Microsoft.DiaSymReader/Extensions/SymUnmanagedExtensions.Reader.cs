// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.DiaSymReader
{
    using static InteropUtilities;

    partial class SymUnmanagedExtensions
    {
        // The name of the attribute containing the byte array of custom debug info.
        // MSCUSTOMDEBUGINFO in Dev10.
        private const string CdiAttributeName = "MD2";

        public static void UpdateSymbolStore(this ISymUnmanagedReader reader, Stream stream, string fileName = null)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            ThrowExceptionForHR(reader.UpdateSymbolStore(fileName, SymUnmanagedStreamFactory.CreateStream(stream)));
        }

        public static void Initialize(this ISymUnmanagedReader3 reader, Stream stream, object metadataImporter, string fileName = null, string searchPath = null)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            ThrowExceptionForHR(reader.Initialize(metadataImporter, fileName, searchPath, SymUnmanagedStreamFactory.CreateStream(stream)));
        }

        /// <summary>
        /// Get the blob of binary custom debug info for a given method.
        /// </summary>
        public static byte[] GetCustomDebugInfo(this ISymUnmanagedReader3 reader, int methodToken, int methodVersion)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return GetItems(
                reader,
                methodToken,
                methodVersion,
                (ISymUnmanagedReader3 pReader, int pMethodToken, int pMethodVersion, int pBufferLength, out int pCount, byte[] pCustomDebugInfo) =>
                    // Note:  Here, we are assuming that the sym reader implementation we're using implements ISymUnmanagedReader3.  This is
                    // necessary so that we get custom debug info for the correct method version in EnC scenarios.  However, some sym reader
                    // implementations do not support this interface (for example, the mscordbi dynamic sym reader).  If we need to fall back
                    // and call ISymUnmanagedReader.GetSymAttribute in those cases (assuming EnC is not supported), then we'll need to ensure
                    // that incorrect or missing custom debug info will not cause problems for any callers of this method.
                    pReader.GetSymAttributeByVersion(pMethodToken, pMethodVersion, CdiAttributeName, pBufferLength, out pCount, pCustomDebugInfo));
        }

        public static int GetUserEntryPoint(this ISymUnmanagedReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            int entryPoint;
            int hr = reader.GetUserEntryPoint(out entryPoint);
            if (hr == HResult.E_FAIL)
            {
                // Not all assemblies have entry points
                // dlls for example...
                return 0;
            }

            ThrowExceptionForHR(hr);
            return entryPoint;
        }

        public static ISymUnmanagedDocument GetDocument(this ISymUnmanagedReader reader, string name)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            ISymUnmanagedDocument document;
            ThrowExceptionForHR(reader.GetDocument(name, language: default, languageVendor: default, documentType: default, out document));
            return document;
        }

        public static ISymUnmanagedDocument[] GetDocuments(this ISymUnmanagedReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return NullToEmpty(GetItems(reader, 
                (ISymUnmanagedReader a, int b, out int c, ISymUnmanagedDocument[] d) => a.GetDocuments(b, out c, d)));
        }

        public static ISymUnmanagedMethod[] GetMethodsInDocument(this ISymUnmanagedReader reader, ISymUnmanagedDocument symDocument)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return NullToEmpty(GetItems((ISymUnmanagedReader2)reader, symDocument,
                (ISymUnmanagedReader2 a, ISymUnmanagedDocument b, int c, out int d, ISymUnmanagedMethod[] e) => a.GetMethodsInDocument(b, c, out d, e)));
        }

        public static ISymUnmanagedMethod GetMethod(this ISymUnmanagedReader reader, int methodToken)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            int hr = reader.GetMethod(methodToken, out var method);
            ThrowExceptionForHR(hr);

            if (hr < 0)
            {
                // method has no symbol info
                return null;
            }

            if (method == null)
            {
                throw new InvalidOperationException();
            }

            return method;
        }

        public static ISymUnmanagedMethod GetMethodByVersion(this ISymUnmanagedReader reader, int methodToken, int methodVersion)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            ISymUnmanagedMethod method;
            int hr = reader.GetMethodByVersion(methodToken, methodVersion, out method);
            ThrowExceptionForHR(hr);

            if (hr < 0)
            {
                // method has no symbol info
                return null;
            }

            if (method == null)
            {
                throw new InvalidOperationException();
            }

            return method;
        }

        public static int GetMethodVersion(this ISymUnmanagedReader reader, ISymUnmanagedMethod method)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            ThrowExceptionForHR(reader.GetMethodVersion(method, out var version));
            return version;
        }

        /// <summary>
        /// Returns compiler version number and name.
        /// </summary>
        public static bool TryGetCompilerInfo(this ISymUnmanagedCompilerInfoReader reader, out Version version, out string name)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            ThrowExceptionForHR(reader.GetCompilerInfo(out var _, out var _, out var _, out var _, bufferLength: 0, out var bufferLength, name: null));
            if (bufferLength == 0)
            {
                version = null;
                name = null;
                return false;
            }

            var nameBuffer = new char[bufferLength];
            ThrowExceptionForHR(reader.GetCompilerInfo(out var major, out var minor, out var build, out var revision, bufferLength, out var actualLength, nameBuffer));
            ValidateItems(actualLength, nameBuffer.Length);

            name = BufferToString(nameBuffer);
            version = new Version(major, minor, build, revision);
            return true;
        }
    }
}
