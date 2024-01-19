// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Moq;
using NuGet.CommandLine.XPlat;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests
{
    public class PackageSearchResultJsonPrinterTests
    {
        [Fact]
        public void Add_NoPackage_RendersCorrectJson()
        {
            // Arrange
            var mockLoggerWithColor = new Mock<ILoggerWithColor>();
            var printer = new PackageSearchResultJsonRenderer(mockLoggerWithColor.Object, PackageSearchVerbosity.Normal);
            var mockSource = new Mock<PackageSource>("http://mocksource", "MockSource");
            var completedSearch = new List<IPackageSearchMetadata>();
            var mockMetadata = new Mock<IPackageSearchMetadata>();
            var packageIdentity = new PackageIdentity("NuGet.Versioning", new NuGetVersion("4.3.0"));
            printer.Start();

            var expectedZeroJson = $@"{{
  ""version"": 1,
  ""problems"": [],
  ""searchResult"": [
    {{
      ""sourceName"": ""MockSource"",
      ""problems"": null,
      ""packages"": []
    }}
  ]
}}";

            // Act
            printer.Add(mockSource.Object, completedSearch);
            printer.Finish();

            // Assert
            mockLoggerWithColor.Verify(x => x.LogMinimal(expectedZeroJson), Times.Exactly(1));
        }

        [Fact]
        public void Add_SinglePackage_RendersCorrectJson()
        {
            // Arrange
            var mockLoggerWithColor = new Mock<ILoggerWithColor>();
            var printer = new PackageSearchResultJsonRenderer(mockLoggerWithColor.Object, PackageSearchVerbosity.Normal);
            var mockSource = new Mock<PackageSource>("http://mocksource", "MockSource");
            var completedSearch = new List<IPackageSearchMetadata>();
            var mockMetadata = new Mock<IPackageSearchMetadata>();
            var packageIdentity = new PackageIdentity("NuGet.Versioning", new NuGetVersion("4.3.0"));

            mockMetadata.Setup(m => m.Identity).Returns(packageIdentity);
            mockMetadata.Setup(m => m.Authors).Returns("Microsoft");
            mockMetadata.Setup(m => m.DownloadCount).Returns(123456);

            completedSearch.Add(mockMetadata.Object);

            printer.Start();

            var expectedOneJson = $@"{{
  ""version"": 1,
  ""problems"": [],
  ""searchResult"": [
    {{
      ""sourceName"": ""MockSource"",
      ""problems"": null,
      ""packages"": [
        {{
          ""total downloads"": 123456,
          ""owners"": null,
          ""id"": ""NuGet.Versioning"",
          ""latestVersion"": ""4.3.0""
        }}
      ]
    }}
  ]
}}";

            // Act
            printer.Add(mockSource.Object, completedSearch);
            printer.Finish();

            // Assert
            mockLoggerWithColor.Verify(x => x.LogMinimal(expectedOneJson), Times.Exactly(1));
        }

        [Fact]
        public void Add_MultiplePackages_RendersCorrectJson()
        {
            // Arrange
            var mockLoggerWithColor = new Mock<ILoggerWithColor>();
            var printer = new PackageSearchResultJsonRenderer(mockLoggerWithColor.Object, PackageSearchVerbosity.Normal);
            var mockSource = new Mock<PackageSource>("http://mocksource", "MockSource");
            var completedSearch = new List<IPackageSearchMetadata>();
            var mockMetadata = new Mock<IPackageSearchMetadata>();
            var packageIdentity = new PackageIdentity("NuGet.Versioning", new NuGetVersion("4.3.0"));

            mockMetadata.Setup(m => m.Identity).Returns(packageIdentity);
            mockMetadata.Setup(m => m.Authors).Returns("Microsoft");
            mockMetadata.Setup(m => m.DownloadCount).Returns(123456);

            completedSearch.Add(mockMetadata.Object);
            completedSearch.Add(mockMetadata.Object);

            printer.Start();

            var expectedTwoJson = $@"{{
  ""version"": 1,
  ""problems"": [],
  ""searchResult"": [
    {{
      ""sourceName"": ""MockSource"",
      ""problems"": null,
      ""packages"": [
        {{
          ""total downloads"": 123456,
          ""owners"": null,
          ""id"": ""NuGet.Versioning"",
          ""latestVersion"": ""4.3.0""
        }},
        {{
          ""total downloads"": 123456,
          ""owners"": null,
          ""id"": ""NuGet.Versioning"",
          ""latestVersion"": ""4.3.0""
        }}
      ]
    }}
  ]
}}";

            // Act
            printer.Add(mockSource.Object, completedSearch);
            printer.Finish();

            // Assert
            mockLoggerWithColor.Verify(x => x.LogMinimal(expectedTwoJson), Times.Exactly(1));
        }

        [Fact]
        public void Add_Error_ShouldAddError()
        {
            // Arrange
            var mockLoggerWithColor = new Mock<ILoggerWithColor>();
            var printer = new PackageSearchResultJsonRenderer(mockLoggerWithColor.Object, PackageSearchVerbosity.Minimal);
            Mock<PackageSource> mockSource = new Mock<PackageSource>("http://errorsource", "ErrorTestSource");
            string errorMessage = "An error occurred";
            var expectedJson = $@"{{
  ""version"": 1,
  ""problems"": [],
  ""searchResult"": [
    {{
      ""sourceName"": ""ErrorTestSource"",
      ""problems"": [
        {{
          ""text"": ""An error occurred"",
          ""problemType"": ""Warning""
        }}
      ],
      ""packages"": []
    }}
  ]
}}";

            // Act
            printer.Start();
            printer.Add(mockSource.Object, new PackageSearchProblem(PackageSearchProblemType.Warning, errorMessage));
            printer.Finish();

            // Assert
            mockLoggerWithColor.Verify(x => x.LogMinimal(expectedJson), Times.Once);
        }
    }
}
