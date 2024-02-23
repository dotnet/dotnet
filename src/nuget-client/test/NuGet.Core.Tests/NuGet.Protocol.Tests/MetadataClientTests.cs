// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class MetadataClientTests
    {
        [Fact]
        public async Task MetadataClient_ResolvePackageWithEmptyDependencyGroups()
        {
            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add("http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/deepequal/index.json", JsonData.DeepEqualRegistationIndex);

            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", Repository.Provider.GetCoreV3(), responses);

            var resource = await repo.GetResourceAsync<DependencyInfoResource>(CancellationToken.None);

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var results = await resource.ResolvePackages("deepequal", NuGetFramework.Parse("net45"), sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None);

                var target = results.Where(p => p.Version == NuGetVersion.Parse("1.4.0")).Single();

                // Assert
                Assert.Equal(19, results.Count());

                Assert.Equal(0, target.Dependencies.Count());
            }
        }

        [Fact]
        public async Task MetadataClient_GatherExactPackage()
        {
            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add("http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/deepequal/index.json", JsonData.DeepEqualRegistationIndex);

            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", Repository.Provider.GetCoreV3(), responses);

            var resource = await repo.GetResourceAsync<DependencyInfoResource>(CancellationToken.None);

            var package = new PackageIdentity("deepequal", NuGetVersion.Parse("0.9.0"));

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var result = await resource.ResolvePackage(package, NuGetFramework.Parse("net45"), sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None);

                // Assert
                Assert.Equal(result.Version, package.Version);
            }
        }

        [Fact]
        public async Task MetadataClient_GatherAllPackages()
        {
            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add("http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/deepequal/index.json", JsonData.DeepEqualRegistationIndex);

            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", Repository.Provider.GetCoreV3(), responses);

            var resource = await repo.GetResourceAsync<DependencyInfoResource>(CancellationToken.None);

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var results = await resource.ResolvePackages("deepequal", NuGetFramework.Parse("net45"), sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None);

                // Assert
                Assert.Equal(19, results.Count());
                Assert.Equal(1, results.Count(package => package.Version.IsPrerelease));
            }
        }

        [Fact]
        public async Task MetadataClient_ResolvePackageWhereDependencyIsNotFoundOnServer()
        {
            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add("http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/microsoft.owin/index.json", JsonData.MicrosoftOwinRegistration);
            responses.Add("https://api.nuget.org/v3/registration0/owin/index.json", null);
            // Owin is not added
            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", Repository.Provider.GetCoreV3(), responses);

            var resource = await repo.GetResourceAsync<DependencyInfoResource>(CancellationToken.None);

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var results = await resource.ResolvePackages("microsoft.owin", NuGetFramework.Parse("net45"), sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None);

                // Assert
                Assert.Equal(14, results.Count());
                Assert.True(results.All(p => p.Id.Equals("microsoft.owin", StringComparison.OrdinalIgnoreCase)));
            }
        }

        [Fact]
        public async Task MetadataClient_ResolvePackageNotFoundOnServer()
        {
            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add("http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/microsoft.owin/index.json", JsonData.MicrosoftOwinRegistration);
            responses.Add("https://api.nuget.org/v3/registration0/owin/index.json", "");
            // Owin is not added
            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", Repository.Provider.GetCoreV3(), responses);

            var resource = await repo.GetResourceAsync<DependencyInfoResource>(CancellationToken.None);

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var results = await resource.ResolvePackages("owin", NuGetFramework.Parse("net45"), sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None);

                // Assert
                Assert.Equal(0, results.Count());
            }
        }

        [Fact]
        public async Task MetadataClient_ResolvePackageNotFoundOnServer_Exact()
        {
            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add("http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/microsoft.owin/index.json", JsonData.MicrosoftOwinRegistration);
            responses.Add("https://api.nuget.org/v3/registration0/owin/index.json", "");
            // Owin is not added
            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", Repository.Provider.GetCoreV3(), responses);

            var resource = await repo.GetResourceAsync<DependencyInfoResource>(CancellationToken.None);

            var package = new PackageIdentity("owin", NuGetVersion.Parse("1.0.0"));

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var result = await resource.ResolvePackage(package, NuGetFramework.Parse("net45"), sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task MetadataClient_ResolvePackageUnlisted()
        {
            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add("http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/unlistedpackagea/index.json", JsonData.UnlistedPackageARegistration);
            responses.Add("https://api.nuget.org/v3/registration0/unlistedpackageb/index.json", JsonData.UnlistedPackageBRegistration);

            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", Repository.Provider.GetCoreV3(), responses);

            var resource = await repo.GetResourceAsync<DependencyInfoResource>(CancellationToken.None);

            var package = new PackageIdentity("unlistedpackagea", NuGetVersion.Parse("1.0.0"));

            var projectFramework = NuGetFramework.Parse("net45");

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var result = await resource.ResolvePackage(package, projectFramework, sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None);

                // Assert
                Assert.False(result.Listed);
            }
        }

        [Fact]
        public async Task MetadataClient_ResolvePackageListed()
        {
            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add("http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/unlistedpackagea/index.json", JsonData.UnlistedPackageARegistration);
            responses.Add("https://api.nuget.org/v3/registration0/unlistedpackageb/index.json", JsonData.UnlistedPackageBRegistration);
            responses.Add("https://api.nuget.org/v3/registration0/unlistedpackagec/index.json", JsonData.UnlistedPackageCRegistration);

            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", Repository.Provider.GetCoreV3(), responses);

            var resource = await repo.GetResourceAsync<DependencyInfoResource>(CancellationToken.None);

            var package = new PackageIdentity("unlistedpackagec", NuGetVersion.Parse("1.0.0"));

            var projectFramework = NuGetFramework.Parse("net45");

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var result = await resource.ResolvePackage(package, projectFramework, sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None);

                // Assert
                Assert.True(result.Listed);
            }
        }
    }
}
