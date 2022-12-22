// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using NuGet.Packaging;

namespace Microsoft.DotNet.VersionTools.Automation
{
    public interface IPackageArchiveReaderFactory
    {
        PackageArchiveReader CreatePackageArchiveReader(string path);
    }

    public class PackageArchiveReaderFactory : IPackageArchiveReaderFactory
    {
        public PackageArchiveReader CreatePackageArchiveReader(string path)
        {
            return new PackageArchiveReader(path);
        }
    }
}
