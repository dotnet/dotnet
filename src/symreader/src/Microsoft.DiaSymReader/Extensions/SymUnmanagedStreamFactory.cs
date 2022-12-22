// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace Microsoft.DiaSymReader
{
    public static class SymUnmanagedStreamFactory
    {
        public static IStream CreateStream(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanSeek)
            {
                // TODO: localize
                throw new ArgumentException("Stream must support seek operation.", nameof(stream));
            }

            return new ComStreamWrapper(stream);
        }
    }
}
