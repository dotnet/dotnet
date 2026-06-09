// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class PackageMetadataResourceV3Tests
    {
        private static IEnumerable<Lazy<INuGetResourceProvider>> CreateProvidersWithEnvReader(string useStj)
        {
            var envReader = new Mock<IEnvironmentVariableReader>();
            envReader.Setup(e => e.GetEnvironmentVariable(NuGet.Shared.NuGetFeatureFlags.UseSystemTextJsonDeserializationEnvVar)).Returns(useStj);

            return Repository.Provider.GetCoreV3()
                .Where(p => p.Value is not PackageMetadataResourceV3Provider)
                .Append(new Lazy<INuGetResourceProvider>(() => new PackageMetadataResourceV3Provider(envReader.Object)));
        }

        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        public async Task PackageMetadataResourceV3_GetMetadataAsync(string useStj)
        {
            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add("http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/deepequal/index.json", JsonData.DeepEqualRegistationIndex);

            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", CreateProvidersWithEnvReader(useStj), responses);

            var resource = await repo.GetResourceAsync<PackageMetadataResource>(CancellationToken.None)
                ?? throw new Xunit.Sdk.XunitException("Expected PackageMetadataResource.");

            var package = new PackageIdentity("deepequal", NuGetVersion.Parse("0.9.0"));

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var result = (PackageSearchMetadataRegistration)(await resource.GetMetadataAsync(package, sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None)
                    ?? throw new Xunit.Sdk.XunitException("Expected package metadata."));

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

        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        public async Task PackageMetadataResourceV3_GetMetadataAsync_Unlisted(string useStj)
        {
            var responses = new Dictionary<string, string>();
            responses.Add("http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/unlistedpackagea/index.json", JsonData.UnlistedPackageARegistration);

            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", CreateProvidersWithEnvReader(useStj), responses);

            var resource = await repo.GetResourceAsync<PackageMetadataResource>(CancellationToken.None)
                ?? throw new Xunit.Sdk.XunitException("Expected PackageMetadataResource.");

            var package = new PackageIdentity("unlistedpackagea", NuGetVersion.Parse("1.0.0"));

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var result = (PackageSearchMetadataRegistration)(await resource.GetMetadataAsync(package, sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None)
                    ?? throw new Xunit.Sdk.XunitException("Expected package metadata."));

                // Assert
                Assert.False(result.IsListed);
            }
        }

        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        public async Task PackageMetadataResourceV3_UsesReferenceCache(string useStj)
        {
            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add("http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/afine/index.json", JsonData.DuplicatePackageBesidesVersionRegistrationIndex);

            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", CreateProvidersWithEnvReader(useStj), responses);

            var resource = await repo.GetResourceAsync<PackageMetadataResource>(CancellationToken.None)
                ?? throw new Xunit.Sdk.XunitException("Expected PackageMetadataResource.");

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var result = (IEnumerable<PackageSearchMetadataRegistration>)(await resource.GetMetadataAsync("afine", true, true, sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None)
                    ?? throw new Xunit.Sdk.XunitException("Expected package metadata list."));

                var first = result.ElementAt(0);
                var second = result.ElementAt(1);

                // Assert
                MetadataReferenceCacheTestUtility.AssertPackagesHaveSameReferences(first, second);
            }
        }

        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        public async Task PackageMetadataResourceV3_GetMetadataAsync_NotFound(string useStj)
        {
            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add("http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/deepequal/index.json", JsonData.DeepEqualRegistationIndex);

            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", CreateProvidersWithEnvReader(useStj), responses);

            var resource = await repo.GetResourceAsync<PackageMetadataResource>(CancellationToken.None)
                ?? throw new Xunit.Sdk.XunitException("Expected PackageMetadataResource.");

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
        [InlineData("MIT OR Apache-2.0", "1.0", "true")]
        [InlineData("MIT OR Apache-2.0", "1.0", "false")]
        [InlineData("Apache-2.0", null, "true")]
        [InlineData("Apache-2.0", null, "false")]
        [InlineData("MIT OR Apache-2.0", "bad version", "true")]
        [InlineData("MIT OR Apache-2.0", "bad version", "false")]
        [InlineData("MIT", "0.0", "true")]
        [InlineData("MIT", "0.0", "false")]
        [InlineData("( MIT )", "0.0", "true")]
        [InlineData("( MIT )", "0.0", "false")]
        [InlineData("         MIT           ", "0.0", "true")]
        [InlineData("         MIT           ", "0.0", "false")]
        public async Task PackageMetadataResourceV3_GetMetadataAsync_ParsesLicenseExpression(string expression, string version, string useStj)
        {

            var licenseData = $@"""{JsonProperties.LicenseExpression}"": ""{expression}""," +
                              (version != null ? $@"""{JsonProperties.LicenseExpressionVersion}"": ""{version}""," : "");

            var sourceName = $"http://{Guid.NewGuid().ToString()}.com/v3/index.json";

            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add(sourceName, JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/packagea/index.json", string.Format(JsonData.PackageARegistration, licenseData));

            var repo = StaticHttpHandler.CreateSource(sourceName, CreateProvidersWithEnvReader(useStj), responses);

            var resource = await repo.GetResourceAsync<PackageMetadataResource>(CancellationToken.None)
                ?? throw new Xunit.Sdk.XunitException("Expected PackageMetadataResource.");

            var package = new PackageIdentity("PackageA", NuGetVersion.Parse("1.0.0"));

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var result = await resource.GetMetadataAsync(package, sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None)
                    ?? throw new Xunit.Sdk.XunitException("Expected package metadata.");

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
        [InlineData("MIT OR Apache-2.0 BLA", null, 1, "Invalid element 'BLA'.", "true")]
        [InlineData("MIT OR Apache-2.0 BLA", null, 1, "Invalid element 'BLA'.", "false")]
        [InlineData("MIT OR Apache-2.0", "15.0", 1, "The license version string '15.0' is higher than the one supported by this toolset", "true")]
        [InlineData("MIT OR Apache-2.0", "15.0", 1, "The license version string '15.0' is higher than the one supported by this toolset", "false")]
        [InlineData("CoolLicense OR CoolerLicense", null, 1, "The license identifier(s) CoolLicense, CoolerLicense is(are) not recognized by the current toolset.", "true")]
        [InlineData("CoolLicense OR CoolerLicense", null, 1, "The license identifier(s) CoolLicense, CoolerLicense is(are) not recognized by the current toolset.", "false")]
        public async Task PackageMetadataResourceV3_GetMetadataAsync_ParsesLicenseExpressionWithWarnings(string expression, string version, int errorCount, string errorMessage, string useStj)
        {

            var licenseData = $@"""{JsonProperties.LicenseExpression}"": ""{expression}""," +
                              (version != null ? $@"""{JsonProperties.LicenseExpressionVersion}"": ""{version}""," : "");

            var sourceName = $"http://{Guid.NewGuid().ToString()}.com/v3/index.json";

            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add(sourceName, JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/packagea/index.json", string.Format(JsonData.PackageARegistration, licenseData));

            var repo = StaticHttpHandler.CreateSource(sourceName, CreateProvidersWithEnvReader(useStj), responses);

            var resource = await repo.GetResourceAsync<PackageMetadataResource>(CancellationToken.None)
                ?? throw new Xunit.Sdk.XunitException("Expected PackageMetadataResource.");

            var package = new PackageIdentity("PackageA", NuGetVersion.Parse("1.0.0"));

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var result = await resource.GetMetadataAsync(package, sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None)
                    ?? throw new Xunit.Sdk.XunitException("Expected package metadata.");

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

                Assert.Equal(errorCount, result.LicenseMetadata.WarningsAndErrors!.Count);
                Assert.Contains(errorMessage, result.LicenseMetadata.WarningsAndErrors[0]);
            }
        }

        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        public async Task PackageMetadataResourceV3_GetMetadataAsync_NotFoundHandleNullStream(string useStj)
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

            var repo = StaticHttpHandler.CreateSource(source, CreateProvidersWithEnvReader(useStj), responses);
            var resource = await repo.GetResourceAsync<PackageMetadataResource>(CancellationToken.None)
                ?? throw new Xunit.Sdk.XunitException("Expected PackageMetadataResource.");

            //Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var metadata = await resource.GetMetadataAsync(package, sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None);

                //Assert
                Assert.Null(metadata);
            }
        }

        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        public async Task PackageMetadataResourceV3_GetMetadataAsync_NoContentHandleNullStream(string useStj)
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

            var repo = StaticHttpHandler.CreateSource(source, CreateProvidersWithEnvReader(useStj), responses);
            var resource = await repo.GetResourceAsync<PackageMetadataResource>(CancellationToken.None)
                ?? throw new Xunit.Sdk.XunitException("Expected PackageMetadataResource.");

            //Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var metadata = await resource.GetMetadataAsync(package, sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None);

                //Assert
                Assert.Null(metadata);
            }
        }

        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        public async Task PackageMetadataResourceV3_GetMetadataAsync_DependencyRangeNull(string useStj)
        {
            // Arrange
            var responses = new Dictionary<string, string>();
            responses.Add("http://testsource.com/v3/index.json", JsonData.IndexWithoutFlatContainer);
            responses.Add("https://api.nuget.org/v3/registration0/dependencyedgecases/index.json", JsonData.PackageDependencyWithNullAndEmptyRange);

            var repo = StaticHttpHandler.CreateSource("http://testsource.com/v3/index.json", CreateProvidersWithEnvReader(useStj), responses);

            var resource = await repo.GetResourceAsync<PackageMetadataResource>(CancellationToken.None)
                ?? throw new Xunit.Sdk.XunitException("Expected PackageMetadataResource.");

            var package = new PackageIdentity("dependencyedgecases", NuGetVersion.Parse("0.1.0"));

            // Act
            using (var sourceCacheContext = new SourceCacheContext())
            {
                var result = await resource.GetMetadataAsync(package, sourceCacheContext, Common.NullLogger.Instance, CancellationToken.None)
                    ?? throw new Xunit.Sdk.XunitException("Expected package metadata.");

                // Assert
                result.Should().NotBeNull();
                result.DependencySets.Should().HaveCount(1);
                var dependencySets = result.DependencySets.ToList();
                dependencySets[0].Packages.Should().HaveCount(3);
                dependencySets[0].Packages.All(p => p.VersionRange == VersionRange.All).Should().BeTrue();
            }
        }
    }
}
