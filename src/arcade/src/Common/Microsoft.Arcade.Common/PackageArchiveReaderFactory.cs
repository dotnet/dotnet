// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using NuGet.Packaging;

namespace Microsoft.Arcade.Common
{
    public class PackageArchiveReaderFactory : IPackageArchiveReaderFactory
    {
        public PackageArchiveReader CreatePackageArchiveReader(string path)
        {
            return new PackageArchiveReader(path);
        }
    }
}
