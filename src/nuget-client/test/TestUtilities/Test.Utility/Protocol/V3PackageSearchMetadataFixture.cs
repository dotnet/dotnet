// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Test.Utility
{
    public class V3PackageSearchMetadataFixture : IDisposable
    {
        private bool _disposedValue = false; // To detect redundant calls
        public IPackageSearchMetadata TestData { get; private set; }

        public V3PackageSearchMetadataFixture()
        {
            TestData = new MockPackageSearchMetadata()
            {
                Vulnerabilities = new List<PackageVulnerabilityMetadata>()
                {
                    new PackageVulnerabilityMetadata()
                    {
                        AdvisoryUrl = new Uri("https://example/advisory/ABCD-1234-5678-9012"),
                        Severity = 2
                    },
                    new PackageVulnerabilityMetadata()
                    {
                        AdvisoryUrl = new Uri("https://example/advisory/ABCD-1234-5678-3535"),
                        Severity = 3
                    }
                }
            };
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
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

        public class MockPackageSearchMetadata : IPackageSearchMetadata
        {
            public MockPackageSearchMetadata()
            {
                Identity = new PackageIdentity("nuget.psm.test", new NuGetVersion(0, 0, 1));
            }

            public string Authors => string.Empty;

            public IEnumerable<PackageDependencyGroup> DependencySets => null;

            public string Description => string.Empty;

            public long? DownloadCount => 100L;

            public Uri IconUrl => null;

            public PackageIdentity Identity { get; set; }

            public Uri ReadmeUrl => null;

            public string ReadmeFileUrl => null;

            public Uri LicenseUrl => null;

            public Uri ProjectUrl => null;

            public Uri ReportAbuseUrl => null;

            public Uri PackageDetailsUrl => null;

            public string PackagePath => null;

            public DateTimeOffset? Published => DateTimeOffset.Now;

            public IReadOnlyList<string> OwnersList => null;

            public string Owners => string.Empty;

            public bool RequireLicenseAcceptance => false;

            public string Summary => string.Empty;

            public string Tags => null;

            public string Title => "title";

            public bool IsListed => true;

            public bool PrefixReserved => false;

            public LicenseMetadata LicenseMetadata => null;

            public Task<PackageDeprecationMetadata> GetDeprecationMetadataAsync() => Task.FromResult<PackageDeprecationMetadata>(null);

            public Task<IEnumerable<VersionInfo>> GetVersionsAsync()
            {
                throw new NotImplementedException();
            }

            public IEnumerable<PackageVulnerabilityMetadata> Vulnerabilities { get; set; }
        }
    }
}
