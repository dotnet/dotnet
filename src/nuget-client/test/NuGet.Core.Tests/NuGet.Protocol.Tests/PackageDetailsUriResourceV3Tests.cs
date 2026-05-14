// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FluentAssertions;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class PackageDetailsUriResourceV3Tests
    {
        [Theory]
        [InlineData("https://ex/packages/{id}/{version}", "https://ex/packages/Test/1.0.0-ALPHA")]
        [InlineData("HTTPS://EX/packages/{id}/{version}", "https://ex/packages/Test/1.0.0-ALPHA")]
        [InlineData("https://ex/packages/{id}", "https://ex/packages/Test")]
        [InlineData("https://ex/packages/{version}", "https://ex/packages/1.0.0-ALPHA")]
        [InlineData("https://ex/packages", "https://ex/packages")]
        [InlineData("https://ex", "https://ex/")]
        public void GetUriReplacesIdAndVersionTokensInUriTemplateWhenAvailable(string template, string expected)
        {
            var resource = PackageDetailsUriResourceV3.CreateOrNull(template);

            var actual = resource.GetUri("Test", NuGetVersion.Parse("1.0.0.0-ALPHA+git"));

            Assert.Equal(expected, actual.ToString());
        }

        [Theory]
        [InlineData("https://ex/packages/{id}/{version}")]
        [InlineData("HTTPS://EX/packages/{id}/{version}")]
        [InlineData("https://ex/packages/{id}")]
        [InlineData("https://ex/packages/{version}")]
        [InlineData("https://ex/packages")]
        [InlineData("https://ex")]
        public void GetUri_InvalidPackageID_Throws(string template)
        {
            // Arrange
            string id = "../contoso";
            var resource = PackageDetailsUriResourceV3.CreateOrNull(template);

            // Act & Assert
            var exception = Assert.Throws<Packaging.InvalidPackageIdException>(() => resource.GetUri(id, NuGetVersion.Parse("1.0.0.0-ALPHA+git")));
            exception.Message.Should().Contain(string.Format(Strings.Error_Invalid_package_id, id));
        }

        [Theory]
        [InlineData("ex/packages/{id}/{version}")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  \t\n")]
        [InlineData("foo!")]
        [InlineData("https://")]
        [InlineData("http://ex/packages")]
        [InlineData("ftp://ex/packages")]
        [InlineData("//ex/packages")]
        [InlineData("../somepath")]
        [InlineData(@"C:\packages\{id}\{version}\index.html")]
        [InlineData("http://unit.test/packages/{id}/{version}")]
        [InlineData("file:///my/path")]
        public void CreateOrNullReturnsNullForInvalid(string template)
        {
            var resource = PackageDetailsUriResourceV3.CreateOrNull(template);

            Assert.Null(resource);
        }
    }
}
