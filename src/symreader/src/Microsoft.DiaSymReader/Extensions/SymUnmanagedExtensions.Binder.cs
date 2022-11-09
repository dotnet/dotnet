// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.ComponentModel;
using System.IO;

namespace Microsoft.DiaSymReader
{
    using static InteropUtilities;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class SymUnmanagedExtensions
    {
        public static ISymUnmanagedReader GetReaderFromStream(this ISymUnmanagedBinder binder, Stream stream, object metadataImporter)
        {
            if (binder == null)
            {
                throw new ArgumentNullException(nameof(binder));
            }

            ISymUnmanagedReader symReader;
            ThrowExceptionForHR(binder.GetReaderFromStream(metadataImporter, SymUnmanagedStreamFactory.CreateStream(stream), out symReader));
            return symReader;
        }

        public static ISymUnmanagedReader GetReaderFromPdbStream(this ISymUnmanagedBinder4 binder, Stream stream, IMetadataImportProvider metadataImportProvider)
        {
            if (binder == null)
            {
                throw new ArgumentNullException(nameof(binder));
            }

            ISymUnmanagedReader symReader;
            ThrowExceptionForHR(binder.GetReaderFromPdbStream(metadataImportProvider, SymUnmanagedStreamFactory.CreateStream(stream), out symReader));
            return symReader;
        }
    }
}
