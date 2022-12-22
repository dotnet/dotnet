// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.UI
{
    public class DetailedPackageMetadataTests
    {
        [Theory]
        [InlineData("https://www.nuget.org/", "nuget.org")]
        [InlineData("https://wwwnuget.example/", "wwwnuget.example")]
        [InlineData("https://foo.www.nuget.org/", "foo.www.nuget.org")]
        [InlineData("https://WWW.NUGET.ORG/", "nuget.org")]
        [InlineData("https://wWw.NUGET.ORG/", "nuget.org")]
        [InlineData("https://www.nüget.org/", "nüget.org")]
        [InlineData("https://nuget.org/", "nuget.org")]
        [InlineData("https://www.nugettest.org/", "nugettest.org")]
        [InlineData("https://dev.nugettest.org/", "dev.nugettest.org")]
        [InlineData("https://www.example/", "example")]
        [InlineData("https://www/", "www")]
        [InlineData("https://www./", "www.")]
        [InlineData("https://www.a/", "a")]
        [InlineData("https://example/", "example")]
        [InlineData("https://dotnet.myget.example/", "dotnet.myget.example")]
        [InlineData("https://www.myget.example/", "myget.example")]
        [InlineData("http://www.nuget.org/", "nuget.org")]
        [InlineData("ftp://www.nuget.org/", "nuget.org")]
        public void RemovesWwwSubdomainFromPackageDetailsText(string url, string expected)
        {
            var packageSearchMetadata = new PackageSearchMetadataBuilder.ClonedPackageSearchMetadata()
            {
                Identity = new PackageIdentity("NuGet.Versioning", NuGetVersion.Parse("4.3.0")),
                PackageDetailsUrl = new Uri(url)
            };

            var packageSearchMetadataContextInfo = PackageSearchMetadataContextInfo.Create(packageSearchMetadata);

            var target = new DetailedPackageMetadata(packageSearchMetadataContextInfo, deprecationMetadata: null, downloadCount: null);

            Assert.Equal(expected, target.PackageDetailsText);
        }
    }
}
