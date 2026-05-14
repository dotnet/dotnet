// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Packaging.Signing
{
    internal sealed class UnsignedPackageArchiveMetadata
    {
        internal long EndOfLocalFileHeadersPosition { get; }
        internal long EndOfCentralDirectoryHeadersPosition { get; }

        internal UnsignedPackageArchiveMetadata(
            long endOfLocalFileHeadersPosition,
            long endOfCentralDirectoryHeadersPosition)
        {
            EndOfLocalFileHeadersPosition = endOfLocalFileHeadersPosition;
            EndOfCentralDirectoryHeadersPosition = endOfCentralDirectoryHeadersPosition;
        }
    }
}
