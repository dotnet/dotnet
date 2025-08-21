// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FluentAssertions;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class ReadmeUriTemplateResourceTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ReadmeUriTemplateResource_GetReadmeUrl_BlankTemplate_ReturnsEmptyString(string uriTemplate)
        {
            string expectedResult = string.Empty;
            var resource = new ReadmeUriTemplateResource(uriTemplate);

            var actual = resource.GetReadmeUrl("TestPackage", NuGetVersion.Parse("1.0.0"));

            Assert.Equal(expectedResult, actual);
        }

        [Fact]
        public void ReadmeUriTemplateResource_GetReadmeUrl_ReturnsFormedUrl()
        {
            const string uriTemplate = "https://test.nuget.org/{lower_id}/{lower_version}/readme";
            const string expectedResult = "https://test.nuget.org/testpackage/1.0.0/readme";
            var resource = new ReadmeUriTemplateResource(uriTemplate);

            var actual = resource.GetReadmeUrl("TestPackage", NuGetVersion.Parse("1.0.0"));

            Assert.Equal(expectedResult, actual.ToString());
        }

        [Fact]
        public void GetReadmeUrl_InvalidPackageId_Throws()
        {
            const string uriTemplate = "https://test.nuget.org/{lower_id}/{lower_version}/readme";
            const string id = "../contoso";
            var resource = new ReadmeUriTemplateResource(uriTemplate);

            var exception = Assert.Throws<Packaging.InvalidPackageIdException>(() => resource.GetReadmeUrl(id, NuGetVersion.Parse("1.0.0")));
            exception.Message.Should().Contain(string.Format(Strings.Error_Invalid_package_id, id));
        }
    }
}
