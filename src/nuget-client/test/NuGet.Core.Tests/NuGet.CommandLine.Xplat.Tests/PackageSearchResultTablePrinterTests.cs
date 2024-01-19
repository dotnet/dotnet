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
    public class PackageSearchResultTablePrinterTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(100)]
        public void Add_MultiplePackageSearchMetadata_RendersCorrectNumberOfPackages(int numberOfPackages)
        {
            // Arrange
            var searchTerm = "TestPackage";
            Mock<ILoggerWithColor> mockLoggerWithColor = new Mock<ILoggerWithColor>();
            PackageSearchResultTableRenderer renderer = new PackageSearchResultTableRenderer(searchTerm, mockLoggerWithColor.Object, PackageSearchVerbosity.Normal, false);
            Mock<PackageSource> mockSource = new Mock<PackageSource>("http://mysource", "TestSource");
            var packageIdentity = new PackageIdentity("NuGet.Versioning", new NuGetVersion("4.3.0"));
            var completedSearch = new List<IPackageSearchMetadata>();
            Mock<IPackageSearchMetadata> packageSearchMetadata = new Mock<IPackageSearchMetadata>();
            packageSearchMetadata.Setup(m => m.Identity).Returns(packageIdentity);
            packageSearchMetadata.Setup(m => m.Authors).Returns("Microsoft");
            packageSearchMetadata.Setup(m => m.DownloadCount).Returns(123456);

            for (int i = 0; i < numberOfPackages; i++)
            {
                completedSearch.Add(packageSearchMetadata.Object);
            }

            // Act
            renderer.Add(mockSource.Object, completedSearch);

            // Assert
            mockLoggerWithColor.Verify(x => x.LogMinimal("****************************************"), Times.Once);
            mockLoggerWithColor.Verify(x => x.LogMinimal($"Source: TestSource (http://mysource/)"), Times.Once);
            if (numberOfPackages == 0)
            {
                mockLoggerWithColor.Verify(x => x.LogMinimal("No results found."), Times.Once);
                return;
            }
            else
            {
                mockLoggerWithColor.Verify(x => x.LogMinimal("| Package ID       ", System.Console.ForegroundColor), Times.Once);
                mockLoggerWithColor.Verify(x => x.LogMinimal("| Latest Version ", System.Console.ForegroundColor), Times.Once);
                mockLoggerWithColor.Verify(x => x.LogMinimal("| Owners ", System.Console.ForegroundColor), Times.Once);
                mockLoggerWithColor.Verify(x => x.LogMinimal("| Total Downloads ", System.Console.ForegroundColor), Times.Once);
                mockLoggerWithColor.Verify(x => x.LogMinimal("| NuGet.Versioning ", System.Console.ForegroundColor), Times.Exactly(numberOfPackages));
                mockLoggerWithColor.Verify(x => x.LogMinimal("| 4.3.0          ", System.Console.ForegroundColor), Times.Exactly(numberOfPackages));
                mockLoggerWithColor.Verify(x => x.LogMinimal("|        ", System.Console.ForegroundColor), Times.Exactly(numberOfPackages));
                mockLoggerWithColor.Verify(x => x.LogMinimal("| 123,456         ", System.Console.ForegroundColor), Times.Exactly(numberOfPackages));
            }
        }


        [Fact]
        public void Add_ErrorEncountered_LogsErrorMessage()
        {
            // Arrange
            var searchTerm = "ErrorPackage";
            Mock<ILoggerWithColor> mockLoggerWithColor = new Mock<ILoggerWithColor>();
            PackageSearchResultTableRenderer renderer = new PackageSearchResultTableRenderer(searchTerm, mockLoggerWithColor.Object, PackageSearchVerbosity.Normal, false);
            Mock<PackageSource> mockSource = new Mock<PackageSource>("http://errorsource", "ErrorTestSource");
            string errorMessage = "Error retrieving data";

            // Act
            renderer.Add(mockSource.Object, new PackageSearchProblem(PackageSearchProblemType.Warning, errorMessage));

            // Assert
            mockLoggerWithColor.Verify(x => x.LogMinimal("****************************************"), Times.Once);
            mockLoggerWithColor.Verify(x => x.LogMinimal($"Source: ErrorTestSource (http://errorsource/)"), Times.Once);
            mockLoggerWithColor.Verify(x => x.LogWarning(errorMessage), Times.Once);
        }

    }
}
