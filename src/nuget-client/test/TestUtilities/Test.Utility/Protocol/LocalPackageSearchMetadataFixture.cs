// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Xml.Linq;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Versioning;

namespace NuGet.Test.Utility
{
    public class LocalPackageSearchMetadataFixture : IDisposable
    {
        private bool _disposedValue = false; // To detect redundant calls
        private readonly TestDirectory _testDirectory;
        public LocalPackageSearchMetadata TestData { get; private set; }

        public LocalPackageSearchMetadataFixture()
        {
            var pkgId = new PackageIdentity("nuget.psm.test", new NuGetVersion(0, 0, 1));
            var pkg = new SimpleTestPackageContext(pkgId.Id, pkgId.Version.ToNormalizedString());
            var nuspec = NuspecBuilder.Create()
                .WithPackageId(pkgId.Id)
                .WithPackageVersion(pkgId.Version.ToNormalizedString())
                .WithIcon("icon.png")
                .Build();
            pkg.Nuspec = XDocument.Parse(nuspec.ToString());

            _testDirectory = TestDirectory.Create();

            SimpleTestPackageUtility.CreatePackagesAsync(_testDirectory.Path, pkg).Wait();
            var pkgPath = Path.Combine(_testDirectory.Path, $"{pkgId.Id}.{pkgId.Version.ToNormalizedString()}.nupkg");
            var info = new LocalPackageInfo(
                identity: pkgId,
                path: pkgPath,
                lastWriteTimeUtc: DateTime.UtcNow,
                nuspec: new Lazy<Packaging.NuspecReader>(() =>
                {
                    var reader = new PackageArchiveReader(pkgPath);
                    return reader.NuspecReader;
                }),
                useFolder: false);
            TestData = new LocalPackageSearchMetadata(info);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _testDirectory.Dispose();
                }

                TestData = null;
                _disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
    }
}
