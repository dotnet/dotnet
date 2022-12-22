// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Xunit;

namespace Microsoft.Deployment.DotNet.Releases.Tests
{
    public class ProductsTests : TestBase
    {
        private static readonly string ProductWithEolSupportPhaseJson = @"{""channel-version"": ""2.2"", ""latest-release"": ""2.2.8"", ""latest-release-date"": ""2019-11-19"",
""security"": true, ""latest-runtime"": ""2.2.8"", ""latest-sdk"": ""2.2.207"", ""product"": "".NET Core"", ""support-phase"": ""eol"",
""eol-date"": ""2019-12-23"", ""releases.json"": ""https://dotnetcli.blob.core.windows.net/dotnet/release-metadata/2.2/releases.json""}";

        private static readonly string ProductWithEolDateJson = @"{""channel-version"": ""2.2"", ""latest-release"": ""2.2.8"", ""latest-release-date"": ""2019-11-19"",
""security"": true, ""latest-runtime"": ""2.2.8"", ""latest-sdk"": ""2.2.207"", ""product"": "".NET Core"", ""support-phase"": ""current"",
""eol-date"": ""2019-12-23"", ""releases.json"": ""https://dotnetcli.blob.core.windows.net/dotnet/release-metadata/2.2/releases.json""}";

        private static readonly string ProductWithNullEolDateJson = @"{""channel-version"": ""2.2"", ""latest-release"": ""2.2.8"", ""latest-release-date"": ""2019-11-19"",
""security"": true, ""latest-runtime"": ""2.2.8"", ""latest-sdk"": ""2.2.207"", ""product"": "".NET Core"", ""support-phase"": ""current"",
""eol-date"": null, ""releases.json"": ""https://dotnetcli.blob.core.windows.net/dotnet/release-metadata/2.2/releases.json""}";

        [Fact]
        public void IsOutOfSupportChecksEolDateIfSupportPhaseIsNotEol()
        {
            var product = CreateProduct(ProductWithEolDateJson);

            Assert.True(product.IsOutOfSupport());
        }

        [Fact]
        public void IsOutOfSupportChecksSupportPhaseFirst()
        {
            var product = CreateProduct(ProductWithEolSupportPhaseJson);

            Assert.True(product.IsOutOfSupport());
        }

        [Fact]
        public void IsOutOfSupportReturnsFalseIfEolDateIsNull()
        {
            var product = CreateProduct(ProductWithNullEolDateJson);

            Assert.False(product.IsOutOfSupport());
        }

        [Fact]
        public void Properties()
        {
            var product = Products.Where(p => p.ProductVersion == "3.1").FirstOrDefault();

            Assert.Equal(".NET Core", product.ProductName);
            Assert.Equal("2021-02-09", product.LatestReleaseDate.ToString("yyyy-MM-dd"));
            Assert.True(product.LatestReleaseIncludesSecurityUpdate);
        }
    }
}
