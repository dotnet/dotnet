// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK || NETCOREAPP || NETSTANDARD2_0

using System.IO;

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

/// <inheritdoc/>
public class PlatformStream : IStream
{
    /// <inheritdoc/>
    public Stream CreateBufferedStream(Stream stream, int bufferSize)
    {
        return new BufferedStream(stream, bufferSize);
    }
}

#endif
