// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Xunit;

namespace Microsoft.Deployment.DotNet.Releases.Tests
{
    public class ProductsTests : TestBase
    {
        private const string ProductWithEolSupportPhaseJson = @"{""channel-version"": ""2.2"", ""latest-release"": ""2.2.8"", ""latest-release-date"": ""2019-11-19"",
""security"": true, ""latest-runtime"": ""2.2.8"", ""latest-sdk"": ""2.2.207"", ""product"": "".NET Core"", ""support-phase"": ""eol"",
""eol-date"": ""2019-12-23"", ""releases.json"": ""https://dotnetcli.blob.core.windows.net/dotnet/release-metadata/2.2/releases.json""}";

        private const string ProductWithEolDateJson = @"{""channel-version"": ""2.2"", ""latest-release"": ""2.2.8"", ""latest-release-date"": ""2019-11-19"",
""security"": true, ""latest-runtime"": ""2.2.8"", ""latest-sdk"": ""2.2.207"", ""product"": "".NET Core"", ""support-phase"": ""current"",
""eol-date"": ""2019-12-23"", ""releases.json"": ""https://dotnetcli.blob.core.windows.net/dotnet/release-metadata/2.2/releases.json""}";

        private const string ProductWithNullEolDateJson = @"{""channel-version"": ""2.2"", ""latest-release"": ""2.2.8"", ""latest-release-date"": ""2019-11-19"",
""security"": true, ""latest-runtime"": ""2.2.8"", ""latest-sdk"": ""2.2.207"", ""product"": "".NET Core"", ""support-phase"": ""current"",
""eol-date"": null, ""releases.json"": ""https://dotnetcli.blob.core.windows.net/dotnet/release-metadata/2.2/releases.json""}";

        [Fact]
        public void IsOutOfSupportChecksEolDateIfSupportPhaseIsNotEol()
        {
            Product product = CreateProduct(ProductWithEolDateJson);

            Assert.True(product.IsOutOfSupport());
        }

        [Fact]
        public void IsOutOfSupportChecksSupportPhaseFirst()
        {
            Product product = CreateProduct(ProductWithEolSupportPhaseJson);

            Assert.True(product.IsOutOfSupport());
        }

        [Fact]
        public void IsOutOfSupportReturnsFalseIfEolDateIsNull()
        {
            Product product = CreateProduct(ProductWithNullEolDateJson);

            Assert.False(product.IsOutOfSupport());
        }

        [Theory]
        [InlineData("7.0", ".NET", SupportPhase.GoLive, "2022-10-11", true, ReleaseType.STS)]
        [InlineData("5.0", ".NET", SupportPhase.EOL, "2022-05-10", true, ReleaseType.STS)]
        [InlineData("3.1", ".NET Core", SupportPhase.Maintenance, "2022-10-11", true, ReleaseType.LTS)]
        [InlineData("2.0", ".NET Core", SupportPhase.EOL, "2018-07-10", true, ReleaseType.STS)]
        public void Properties(string productVersion, string expectedProductName,
            SupportPhase expectedSupportPhase, string expectedLatestReleaseDate, bool expectedSecurityUpdate,
            ReleaseType expectedReleaseType)
        {
            Product product = Products.Where(p => p.ProductVersion == productVersion).FirstOrDefault();

            Assert.Equal(expectedProductName, product.ProductName);
            Assert.Equal(expectedSupportPhase, product.SupportPhase);
            Assert.Equal(expectedLatestReleaseDate, product.LatestReleaseDate.ToString("yyyy-MM-dd"));
            Assert.Equal(expectedSecurityUpdate, product.LatestReleaseIncludesSecurityUpdate);
            Assert.Equal(expectedReleaseType, product.ReleaseType);
        }
    }
}
