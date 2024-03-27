// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Linq;
using Xunit;
using System.Threading.Tasks;

namespace Microsoft.Deployment.DotNet.Releases.Tests
{
    public class ProductReleaseTests : TestBase
    {
        [Theory]
        [InlineData("2.2", "2.2.8", "2.2.110", "2.2.207")]
        [InlineData("2.1", "2.1.20", "2.1.808", "2.1.613", "2.1.516")]
        [InlineData("2.0", "2.1.201", "2.1.201")]
        [InlineData("1.1", "1.1.13", "1.1.14")]
        public void ItContainsAllSdks(string productVersion, string releaseVersion, params string[] expectedSdkVersions)
        {
            var productReleaseVersion = new ReleaseVersion(releaseVersion);
            var release = ProductReleases[productVersion].Where(r => r.Version == productReleaseVersion).FirstOrDefault();
            var actualSdkVersions = release.Sdks.Select(s => s.Version.ToString());

            Assert.Equal(expectedSdkVersions.Length, release.Sdks.Count);
            Assert.All(expectedSdkVersions, sdkVersion => actualSdkVersions.Contains(sdkVersion));
        }

        [Theory]
        [InlineData("2.0", "2.1.201")]
        [InlineData("1.1", "1.1.13")]
        public void ItLinksBackToProduct(string productVersion, string releaseVersion)
        {
            var productReleaseVersion = new ReleaseVersion(releaseVersion);
            var release = ProductReleases[productVersion].Where(r => r.Version == productReleaseVersion).FirstOrDefault();

            Assert.Equal(release.Product.ProductVersion, productVersion);
        }

        [Fact]
        public void ItContainsAllFiles()
        {
            ProductRelease release = GetProductRelease("2.1", "2.1.8");

            Assert.Equal(33, release.Files.Count);
            Assert.Empty(release.Files.Where(f => f.FileName.Contains("-gs")));
        }

        [Fact]
        public void ItContainsAllCves()
        {
            ProductRelease release = GetProductRelease("6.0", "6.0.5");

            Assert.Equal(3, release.Cves.Count);
            Assert.Equal("CVE-2022-29117", release.Cves[0].Id);
            Assert.Equal("CVE-2022-23267", release.Cves[1].Id);
            Assert.Equal("CVE-2022-29145", release.Cves[2].Id);
        }

        [Fact]
        public async Task ItCanCreateASingleRelease()
        {
            var releases = await Product.GetReleasesAsync(Path.Combine("data", "5.0", "releases.json"));

            Assert.Equal(new ReleaseVersion("5.0.17"), releases[0].Version);
            Assert.Null(releases[0].Product);
        }

        [Fact]
        public async Task ItCanParseTheLatestPublishedJson()
        {
            string tempPath = Path.GetRandomFileName();
            var products = await ProductCollection.GetFromFileAsync(
                Path.Combine(tempPath, "releases-index.json"), downloadLatest: true);

            foreach (var product in products)
            {
                await product.GetReleasesAsync(Path.Combine(tempPath, product.ProductVersion, "releases.json"), downloadLatest: true);
            }

            // Nothing to assert, but we want to ensure there are no exceptions parsing all the latest data from the CDN

            Directory.Delete(tempPath, recursive: true);
        }
    }
}
