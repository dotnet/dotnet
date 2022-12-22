// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public abstract class SourceRepositoryCreator
    {
        protected PackageIdentity TestPackageIdentity = new PackageIdentity("FakePackage", new NuGetVersion("1.0.0"));
        protected readonly SourceRepository _source;
        protected readonly PackageMetadataResource _metadataResource;
        protected readonly TestLogger _logger = new TestLogger();

        public SourceRepositoryCreator()
        {
            // dependencies and data
            _metadataResource = Mock.Of<PackageMetadataResource>();
            _source = SetupSourceRepository(_metadataResource);
        }

        protected static SourceRepository SetupSourceRepository(PackageMetadataResource resource)
        {
            var provider = Mock.Of<INuGetResourceProvider>();
            Mock.Get(provider)
                .Setup(x => x.TryCreate(It.IsAny<SourceRepository>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(Tuple.Create(true, (INuGetResource)resource)));
            Mock.Get(provider)
                .Setup(x => x.ResourceType)
                .Returns(typeof(PackageMetadataResource));

            var packageSource = new Configuration.PackageSource("http://fake-source");
            return new SourceRepository(packageSource, new[] { provider });
        }

        protected NuGetProject SetupProject(PackageIdentity packageIdentity, string allowedVersions)
        {
            var installedPackages = new[]
            {
                new PackageReference(
                    packageIdentity,
                    NuGetFramework.Parse("net45"),
                    userInstalled: true,
                    developmentDependency: false,
                    requireReinstallation: false,
                    allowedVersions: allowedVersions != null ? VersionRange.Parse(allowedVersions) : null)
            };

            var project = Mock.Of<NuGetProject>();
            Mock.Get(project)
                .Setup(x => x.GetInstalledPackagesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<PackageReference>>(installedPackages));
            return project;
        }

        protected void SetupRemotePackageMetadata(string id, params string[] versions)
        {
            var metadata = versions
                .Select(v => PackageSearchMetadataBuilder
                    .FromIdentity(new PackageIdentity(id, new NuGetVersion(v)))
                    .Build());

            Mock.Get(_metadataResource)
                .Setup(x => x.GetMetadataAsync(id, true, false, It.IsAny<SourceCacheContext>(), It.IsAny<Common.ILogger>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(metadata));
        }
    }
}
