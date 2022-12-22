// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using NuGet.Packaging;
using NuGet.Packaging.Core;

namespace Microsoft.DotNet.VersionTools.Automation
{
    public interface INupkgInfoFactory
    {
        NupkgInfo CreateNupkgInfo(string path);
    }

    public class NupkgInfoFactory : INupkgInfoFactory
    {
        private readonly IPackageArchiveReaderFactory _packageArchiveReaderFactory;

        public NupkgInfoFactory(IPackageArchiveReaderFactory packageArchiveReaderFactory)
        {
            _packageArchiveReaderFactory = packageArchiveReaderFactory;
        }

        public NupkgInfo CreateNupkgInfo(string path)
        {
            using PackageArchiveReader archiveReader = _packageArchiveReaderFactory.CreatePackageArchiveReader(path);
            PackageIdentity identity = archiveReader.GetIdentity();

            return new NupkgInfo(identity);
        }
    }
}
