// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class PackageSearchMetadataV2FeedTests
    {
        [Fact]
        public async Task PackageSearchMetadata_Basic()
        {
            // Arrange
            var testPackage = CreateTestPackageInfo(new List<string>() { "James Newkirk", "Brad Wilson" },
                                                    null,
                                                    "https://raw.githubusercontent.com/xunit/media/master/logo-512-transparent.png",
                                                    "https://github.com/xunit/xunit",
                                                    "",
                                                    "invalidUri",
                                                    "anotherInvalidUri");

            // Act
            var metaData = new PackageSearchMetadataV2Feed(testPackage);

            // Assert
            Assert.Equal(metaData.Authors, "James Newkirk, Brad Wilson");
            Assert.Equal(metaData.Owners, "");
            Assert.Equal("https://raw.githubusercontent.com/xunit/media/master/logo-512-transparent.png", metaData.IconUrl.AbsoluteUri);
            Assert.Equal("https://github.com/xunit/xunit", metaData.LicenseUrl.AbsoluteUri);
            Assert.Null(metaData.ProjectUrl);
            Assert.Null(metaData.ReportAbuseUrl);
            Assert.Null(metaData.PackageDetailsUrl);
            Assert.Null(metaData.DeprecationMetadata);
            Assert.Null(await metaData.GetDeprecationMetadataAsync());
        }

        [Fact]
        public void PackageSearchMetadata_ValidReportAbuseUrl()
        {
            // Arrange
            var url = "https://www.nuget.org/packages/xunit/2.4.1/ReportAbuse";
            var testPackage = CreateTestPackageInfo(new List<string>() { "James Newkirk", "Brad Wilson" },
                                                    null,
                                                    "https://raw.githubusercontent.com/xunit/media/master/logo-512-transparent.png",
                                                    "https://github.com/xunit/xunit",
                                                    "",
                                                    url,
                                                    "anotherInvalidUri");

            // Act
            var metaData = new PackageSearchMetadataV2Feed(testPackage);

            // Assert
            Assert.Equal(new Uri(url), metaData.ReportAbuseUrl);
        }

        [Fact]
        public void PackageSearchMetadata_ValidPackageDetailsUrl()
        {
            // Arrange
            var url = "https://www.nuget.org/packages/xunit/2.4.1";
            var testPackage = CreateTestPackageInfo(new List<string>() { "James Newkirk", "Brad Wilson" },
                                                    null,
                                                    "https://raw.githubusercontent.com/xunit/media/master/logo-512-transparent.png",
                                                    "https://github.com/xunit/xunit",
                                                    "",
                                                    "invalidUri",
                                                    url);

            // Act
            var metaData = new PackageSearchMetadataV2Feed(testPackage);

            // Assert
            Assert.Equal(new Uri(url), metaData.PackageDetailsUrl);
        }

        private V2FeedPackageInfo CreateTestPackageInfo(IEnumerable<string> authors, IEnumerable<string> owners,
            string iconUrl, string licenseUrl, string projectUrl, string reportAbuseUrl, string galleryDetailsUrl)
        {
            return new V2FeedPackageInfo(new PackageIdentity("test", NuGetVersion.Parse("1.0.0")),
                                         "title", "summary", "description", authors, owners,
                                         iconUrl, licenseUrl, projectUrl, reportAbuseUrl, galleryDetailsUrl, "tags", null, null, null, "dependencies",
                                         false, "downloadUrl", "0", "packageHash", "packageHashAlgorithm", new NuGetVersion("3.0"));
        }
    }
}
