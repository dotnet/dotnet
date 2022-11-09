// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class PackageMetadataResourceV3Tests
    {
        [Fact]
        public async Task PackageMetadataResourceV3_GetMetadataAsync()
        {
            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add("http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/deepequal/index.json", JsonData.DeepEqualRegistationIndex);

            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", Repository.Provider.GetCoreV3(), responses);

            var resource = await repo.GetResourceAsync<PackageMetadataResource>();

            var package = new PackageIdentity("deepequal", NuGetVersion.Parse("0.9.0"));

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var result = (PackageSearchMetadataRegistration)await resource.GetMetadataAsync(package, sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None);

                // Assert
                Assert.Equal("deepequal", result.Identity.Id, StringComparer.OrdinalIgnoreCase);
                Assert.Equal("0.9.0", result.Identity.Version.ToNormalizedString());
                Assert.True(result.Authors.Contains("James Foster"));
                Assert.Equal("An extensible deep comparison library for .NET", result.Description);
                Assert.Null(result.IconUrl);
                Assert.Null(result.LicenseUrl);
                Assert.Equal("http://github.com/jamesfoster/DeepEqual", result.ProjectUrl.ToString());
                Assert.Equal("https://api.nuget.org/v3/catalog0/data/2015.02.03.18.34.51/deepequal.0.9.0.json",
                    result.CatalogUri.ToString());
                Assert.Equal(DateTimeOffset.Parse("2013-08-28T09:19:10.013Z"), result.Published);
                Assert.False(result.RequireLicenseAcceptance);
                Assert.Equal(result.Description, result.Summary);
                Assert.Equal(string.Join(", ", "deepequal", "deep", "equal"), result.Tags);
                Assert.Equal("DeepEqual", result.Title);
                Assert.True(result.IsListed);
            }
        }

        [Fact]
        public async Task PackageMetadataResourceV3_GetMetadataAsync_Unlisted()
        {
            var responses = new Dictionary<string, string>();
            responses.Add("http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/unlistedpackagea/index.json", JsonData.UnlistedPackageARegistration);

            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", Repository.Provider.GetCoreV3(), responses);

            var resource = await repo.GetResourceAsync<PackageMetadataResource>();

            var package = new PackageIdentity("unlistedpackagea", NuGetVersion.Parse("1.0.0"));

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var result = (PackageSearchMetadataRegistration)await resource.GetMetadataAsync(package, sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None);

                // Assert
                Assert.False(result.IsListed);
            }
        }

        [Fact]
        public async Task PackageMetadataResourceV3_UsesReferenceCache()
        {
            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add("http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/afine/index.json", JsonData.DuplicatePackageBesidesVersionRegistrationIndex);

            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", Repository.Provider.GetCoreV3(), responses);

            var resource = await repo.GetResourceAsync<PackageMetadataResource>();

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var result = (IEnumerable<PackageSearchMetadataRegistration>)await resource.GetMetadataAsync("afine", true, true, sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None);

                var first = result.ElementAt(0);
                var second = result.ElementAt(1);

                // Assert
                MetadataReferenceCacheTestUtility.AssertPackagesHaveSameReferences(first, second);
            }
        }

        [Fact]
        public async Task PackageMetadataResourceV3_GetMetadataAsync_NotFound()
        {
            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add("http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/deepequal/index.json", JsonData.DeepEqualRegistationIndex);

            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", Repository.Provider.GetCoreV3(), responses);

            var resource = await repo.GetResourceAsync<PackageMetadataResource>();

            var package = new PackageIdentity("deepequal", NuGetVersion.Parse("0.0.0"));

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var result = await resource.GetMetadataAsync(package, sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None);

                // Assert
                Assert.Null(result);
            }
        }

        [Theory]
        [InlineData("MIT OR Apache-2.0", "1.0")]
        [InlineData("Apache-2.0", null)]
        [InlineData("MIT OR Apache-2.0", "bad version")]
        [InlineData("MIT", "0.0")]
        [InlineData("( MIT )", "0.0")]
        [InlineData("         MIT           ", "0.0")]
        public async Task PackageMetadataResourceV3_GetMetadataAsync_ParsesLicenseExpression(string expression, string version)
        {

            var licenseData = $@"""{JsonProperties.LicenseExpression}"": ""{expression}""," +
                              (version != null ? $@"""{JsonProperties.LicenseExpressionVersion}"": ""{version}""," : "");

            var sourceName = $"http://{Guid.NewGuid().ToString()}.com/v3/index.json";

            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add(sourceName, JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/packagea/index.json", string.Format(JsonData.PackageARegistration, licenseData));

            var repo = StaticHttpHandler.CreateSource(sourceName, Repository.Provider.GetCoreV3(), responses);

            var resource = await repo.GetResourceAsync<PackageMetadataResource>();

            var package = new PackageIdentity("PackageA", NuGetVersion.Parse("1.0.0"));

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var result = await resource.GetMetadataAsync(package, sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None);

                // Assert
                Assert.NotNull(result);

                Assert.NotNull(result.LicenseMetadata);
                Assert.Equal(LicenseType.Expression, result.LicenseMetadata.Type);
                Assert.Null(result.LicenseMetadata.WarningsAndErrors);
                Assert.NotNull(result.LicenseMetadata.LicenseExpression);
                Assert.Equal(expression.Trim(), result.LicenseMetadata.License);

                Version.TryParse(version, out var parsedVersion);
                if (parsedVersion != null)
                {
                    Assert.Equal(parsedVersion, result.LicenseMetadata.Version);
                }
                else
                {
                    Assert.Equal(LicenseMetadata.EmptyVersion, result.LicenseMetadata.Version);
                }
            }
        }

        [Theory]
        [InlineData("MIT OR Apache-2.0 BLA", null, 1, "Invalid element 'BLA'.")]
        [InlineData("MIT OR Apache-2.0", "15.0", 1, "The license version string '15.0' is higher than the one supported by this toolset")]
        [InlineData("CoolLicense OR CoolerLicense", null, 1, "The license identifier(s) CoolLicense, CoolerLicense is(are) not recognized by the current toolset.")]
        public async Task PackageMetadataResourceV3_GetMetadataAsync_ParsesLicenseExpressionWithWarnings(string expression, string version, int errorCount, string errorMessage)
        {

            var licenseData = $@"""{JsonProperties.LicenseExpression}"": ""{expression}""," +
                              (version != null ? $@"""{JsonProperties.LicenseExpressionVersion}"": ""{version}""," : "");

            var sourceName = $"http://{Guid.NewGuid().ToString()}.com/v3/index.json";

            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add(sourceName, JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/packagea/index.json", string.Format(JsonData.PackageARegistration, licenseData));

            var repo = StaticHttpHandler.CreateSource(sourceName, Repository.Provider.GetCoreV3(), responses);

            var resource = await repo.GetResourceAsync<PackageMetadataResource>();

            var package = new PackageIdentity("PackageA", NuGetVersion.Parse("1.0.0"));

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var result = await resource.GetMetadataAsync(package, sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None);

                // Assert
                Assert.NotNull(result);

                Assert.NotNull(result.LicenseMetadata);
                Assert.Equal(LicenseType.Expression, result.LicenseMetadata.Type);
                Assert.Equal(expression.Trim(), result.LicenseMetadata.License);
                Version.TryParse(version, out var parsedVersion);
                if (parsedVersion != null)
                {
                    Assert.Equal(parsedVersion, result.LicenseMetadata.Version);
                }
                else
                {
                    Assert.Equal(LicenseMetadata.EmptyVersion, result.LicenseMetadata.Version);
                }

                Assert.Equal(errorCount, result.LicenseMetadata.WarningsAndErrors.Count);
                Assert.Contains(errorMessage, result.LicenseMetadata.WarningsAndErrors[0]);
            }
        }

        [Fact]
        public async Task PackageMetadataResourceV3_GetMetadataAsync_NotFoundHandleNullStream()
        {
            // Arrange
            var notExistPackage = "NotExistPackage";
            var package = new PackageIdentity(notExistPackage, NuGetVersion.Parse("1.0.0"));
            var source = "http://testsource.com/v3/index.json";

            var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
            {
                {
                    source,
                    _ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new TestContent(JsonData.IndexWithoutFlatContainer)
                    })
                },
                {
                    $"https://api.nuget.org/v3/registration0/{notExistPackage.ToLowerInvariant()}/index.json",
                    _ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound))
                }
            };

            var repo = StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses);
            var resource = await repo.GetResourceAsync<PackageMetadataResource>();

            //Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var metadata = await resource.GetMetadataAsync(package, sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None);

                //Assert
                Assert.Null(metadata);
            }
        }

        [Fact]
        public async Task PackageMetadataResourceV3_GetMetadataAsync_NoContentHandleNullStream()
        {
            // Arrange
            var noContentPackage = "NoContentPackage";
            var package = new PackageIdentity(noContentPackage, NuGetVersion.Parse("1.0.0"));
            var source = "http://testsource.com/v3/index.json";

            var responses = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>
            {
                {
                    source,
                    _ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new TestContent(JsonData.IndexWithoutFlatContainer)
                    })
                },
                {
                    $"https://api.nuget.org/v3/registration0/{noContentPackage.ToLowerInvariant()}/index.json",
                    _ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.NoContent))
                }
            };

            var repo = StaticHttpHandler.CreateSource(source, Repository.Provider.GetCoreV3(), responses);
            var resource = await repo.GetResourceAsync<PackageMetadataResource>();

            //Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var metadata = await resource.GetMetadataAsync(package, sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None);

                //Assert
                Assert.Null(metadata);
            }
        }
    }
}
