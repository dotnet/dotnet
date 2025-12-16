// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
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
            var searchTerm = "nuget";
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
                // Asserts for "| Package ID       "
                mockLoggerWithColor.Verify(x => x.LogInline("| ", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("P", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("a", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("c", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("k", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("a", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("g", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("e", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline(" ", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("I", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("D", System.Console.ForegroundColor));

                // Asserts for "| Latest Version "
                mockLoggerWithColor.Verify(x => x.LogInline("| ", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("L", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("a", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("t", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("e", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("s", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("t", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline(" ", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("V", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("e", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("r", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("s", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("i", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("o", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("n", System.Console.ForegroundColor));

                // Asserts for "| Owners "
                mockLoggerWithColor.Verify(x => x.LogInline("| ", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("O", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("w", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("n", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("e", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("r", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("s", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline(" ", System.Console.ForegroundColor));

                // Asserts for "| Total Downloads "
                mockLoggerWithColor.Verify(x => x.LogInline("| ", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("T", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("o", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("t", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("a", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("l", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline(" ", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("D", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("o", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("w", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("n", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("l", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("o", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("a", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("d", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("s", System.Console.ForegroundColor));

                // Assert for "NuGet.Versioning"
                mockLoggerWithColor.Verify(x => x.LogInline("N", ConsoleColor.Red));
                mockLoggerWithColor.Verify(x => x.LogInline("u", ConsoleColor.Red));
                mockLoggerWithColor.Verify(x => x.LogInline("G", ConsoleColor.Red));
                mockLoggerWithColor.Verify(x => x.LogInline("e", ConsoleColor.Red));
                mockLoggerWithColor.Verify(x => x.LogInline("t", ConsoleColor.Red));
                mockLoggerWithColor.Verify(x => x.LogInline(".", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("V", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("e", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("r", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("s", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("i", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("o", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("n", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("i", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("n", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("g", System.Console.ForegroundColor));

                // Assert for "4.3.0"
                mockLoggerWithColor.Verify(x => x.LogInline("4", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline(".", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("3", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline(".", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("0", System.Console.ForegroundColor));

                // Assert for "123,456"
                mockLoggerWithColor.Verify(x => x.LogInline("1", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("2", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("3", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline(",", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("4", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("5", System.Console.ForegroundColor));
                mockLoggerWithColor.Verify(x => x.LogInline("6", System.Console.ForegroundColor));
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
