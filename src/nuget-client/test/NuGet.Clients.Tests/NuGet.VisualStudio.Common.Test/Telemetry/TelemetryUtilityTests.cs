// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using FluentAssertions;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.VisualStudio.Common.Test.Telemetry
{
    public class TelemetryUtilityTests
    {
        [Fact]
        public void IsHttpV3_WhenSourceIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => TelemetryUtility.IsHttpV3(source: null));

            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public void IsHttpV3_WhenSourceIsLocal_ReturnsFalse()
        {
            var source = new PackageSource(@"C:\packages");
            var actualResult = TelemetryUtility.IsHttpV3(source);

            Assert.False(actualResult);
        }

        [Theory]
        [InlineData("https://nuget.test/index.json")]
        [InlineData("https://nuget.test/INDEX.JSON")]
        public void IsHttpV3_WhenSourceIsHttpAndEndsWithIndexJson_ReturnsTrue(string packageSourceUrl)
        {
            var source = new PackageSource(packageSourceUrl);
            var actualResult = TelemetryUtility.IsHttpV3(source);

            Assert.True(actualResult);
        }

        [Fact]
        public void IsHttpV3_WhenSourceIsHttpAndProtocolVersionIs2_ReturnsFalse()
        {
            var source = new PackageSource("https://nuget.test")
            {
                ProtocolVersion = 2
            };
            var actualResult = TelemetryUtility.IsHttpV3(source);

            Assert.False(actualResult);
        }

        [Fact]
        public void IsHttpV3_WhenSourceIsHttpAndProtocolVersionIs3_ReturnsTrue()
        {
            var source = new PackageSource("https://nuget.test")
            {
                ProtocolVersion = 3
            };
            var actualResult = TelemetryUtility.IsHttpV3(source);

            Assert.True(actualResult);
        }

        [Theory]
        [InlineData("http://nuget.org/api/v2", true)]
        [InlineData("http://NUGET.ORG/api/v2", true)]
        [InlineData("https://nuget.org/api/v2", true)]
        [InlineData("https://NUGET.ORG/api/v2", true)]
        [InlineData("http://www.nuget.org/api/v2", true)]
        [InlineData("http://WWW.NUGET.ORG/api/v2", true)]
        [InlineData("https://www.nuget.org/api/v2", true)]
        [InlineData("https://WWW.NUGET.ORG/api/v2", true)]
        [InlineData("http://api.nuget.org/v3/index.json", true)]
        [InlineData("http://API.NUGET.ORG/v3/index.json", true)]
        [InlineData("https://api.nuget.org/v3/index.json", true)]
        [InlineData("https://API.NUGET.ORG/v3/index.json", true)]
        [InlineData("http://notnuget.org/api/v2", false)]
        [InlineData("https://nuget.org.internal/v3/index.json", false)]
        public void IsNuGetOrg(string sourceUrl, bool expected)
        {
            // Arrange
            var source = new PackageSource(sourceUrl);

            // Act
            var actual = UriUtility.IsNuGetOrg(source.Source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("https://tenant.pkgs.visualstudio.com/_packaging/feedname/nuget/v3/index.json", true)]
        [InlineData("https://pkgs.dev.azure.com/tenant/_packaging/feedname/nuget/v3/index.json", true)]
        [InlineData("https://mywebsite.azurewebsites.net/nuget/", false)]
        [InlineData("https://dotnetfeed.blob.core.windows.net/dotnet-core/index.json", false)]
        public void IsAzureArtifacts(string sourceUrl, bool expected)
        {
            // Arrange
            var source = new PackageSource(sourceUrl);

            // Act
            var actual = TelemetryUtility.IsAzureArtifacts(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("https://nuget.pkg.github.com/dotnet/index.json", true)]
        [InlineData("https://nuget.pkg.github.com/nuget/index.json", true)]
        [InlineData("https://raw.githubusercontent.com/account/repo/branch/index.json", false)]
        public void IsGitHub(string sourceUrl, bool expected)
        {
            // Arrange
            var source = new PackageSource(sourceUrl);

            // Act
            var actual = TelemetryUtility.IsGitHub(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsVsOfflineFeed_WhenSourceIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => TelemetryUtility.IsVsOfflineFeed(source: null));

            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public void IsVsOfflineFeed_WhenSourceIsNotLocal_ReturnsFalse()
        {
            var source = new PackageSource("https://nuget.test");
            bool actualResult = TelemetryUtility.IsVsOfflineFeed(source);

            Assert.False(actualResult);
        }

        [Fact]
        public void IsVsOfflineFeed_WhenVsOfflinePackagesPathIsNull_ReturnsFalse()
        {
            var source = new PackageSource(@"C:\packages");
            string expectedVsOfflinePackagesPath = null;
            bool actualResult = TelemetryUtility.IsVsOfflineFeed(source, expectedVsOfflinePackagesPath);

            Assert.False(actualResult);
        }

        [Fact]
        public void IsVsOfflineFeed_WhenVsOfflinePackagesPathIsValidAndDoesNotMatchPackageSource_ReturnsFalse()
        {
            var source = new PackageSource(@"C:\packages");
            var expectedVsOfflinePackagesPath = @"C:\VSOfflinePackages";
            bool actualResult = TelemetryUtility.IsVsOfflineFeed(source, expectedVsOfflinePackagesPath);

            Assert.False(actualResult);
        }

        [Theory]
        [InlineData(@"C:\VSOfflinePackages", @"C:\VSOfflinePackages")]  // identical
        [InlineData(@"c:\vsofflinepackages", @"C:\VSOfflinePackages")]  // differ only in casing
        [InlineData(@"C:\VSOfflinePackages\", @"C:\VSOfflinePackages")] // differ only in trailing slash
        public void IsVsOfflineFeed_WhenVsOfflinePackagesPathIsValidAndMatchesPackageSource_ReturnsFalse(string packageSourcePath, string vsOfflinePackagesPath)
        {
            var source = new PackageSource(packageSourcePath);
            var expectedVsOfflinePackagesPath = vsOfflinePackagesPath;
            bool actualResult = TelemetryUtility.IsVsOfflineFeed(source, expectedVsOfflinePackagesPath);

            Assert.True(actualResult);
        }

        [Theory]
        [InlineData(@"C:\Program Files (x86)\Microsoft SDKs\NuGetPackages\")]
        [InlineData(@"C:\Program Files\Microsoft SDKs\NuGetPackages\")]
        public void IsVSOfflineFeed_WithValidOfflineFeed_ReturnsTrue(string expectedOfflineFeed)
        {
            bool actualResult = TelemetryUtility.IsVsOfflineFeed(new PackageSource(expectedOfflineFeed));

            Assert.True(actualResult);
        }

        [Fact]
        public void ToJsonArrayOfTimingsInSeconds_WithEmptyArray_ReturnsEmptyString()
        {
            TelemetryUtility.ToJsonArrayOfTimingsInSeconds(Enumerable.Empty<TimeSpan>()).Should().Be(string.Empty);
        }

        [Fact]
        public void ToJsonArrayOfTimingsInSeconds_WithNullArgument_ReturnsEmptyString()
        {
            TelemetryUtility.ToJsonArrayOfTimingsInSeconds(null).Should().Be(string.Empty);
        }

        [Fact]
        public void ToJsonArrayOfTimingsInSeconds_WithOneValue_ReturnsTimingsInSeconds()
        {
            TimeSpan[] values = new[] { new TimeSpan(hours: 0, minutes: 0, seconds: 5) };
            TelemetryUtility.ToJsonArrayOfTimingsInSeconds(values).Should().Be("[5]");
        }

        [Fact]
        public void ToJsonArrayOfTimingsInSeconds_WithMultipleValues_AppendsValuesWithComma()
        {
            TimeSpan[] values = new[] { new TimeSpan(hours: 0, minutes: 0, seconds: 5), new TimeSpan(days: 0, hours: 0, minutes: 1, seconds: 0, milliseconds: 500) };
            TelemetryUtility.ToJsonArrayOfTimingsInSeconds(values).Should().Be("[5,60.5]");
        }
    }
}
