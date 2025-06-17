// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Package.Update;
using NuGet.Configuration;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

using Pkg = NuGet.CommandLine.XPlat.Commands.Package.Update.Package;

namespace NuGet.CommandLine.Xplat.Tests.Commands.Package.Update
{
    public class PackageUpdateCommandRunner_GetPackageToUpdateTests
    {
        private readonly ITestOutputHelper _output;

        public PackageUpdateCommandRunner_GetPackageToUpdateTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task RequestSinglePackage_GetsRequestedVersion()
        {
            // Arrange
            Pkg package = new()
            {
                Id = "Contoso.Utils",
                VersionRange = new VersionRange(new NuGetVersion("1.2.3"))
            };

            PackageSpec packageSpec = new TestPackageSpecFactory(builder =>
                {
                    builder.WithProperty("TargetFramework", "net9.0")
                           .WithItem("PackageReference", "Contoso.Utils", [new("Version", "1.0.0")]);
                })
                .Build();

            var versionChooser = new Mock<IVersionChooser>(MockBehavior.Strict);
            var logger = new Mock<ILoggerWithColor>();

            // Act
            var (packageToUpdate, _) = await PackageUpdateCommandRunner.GetPackageToUpdateAsync(
                [package],
                packageSpec,
                versionChooser.Object,
                NullSettings.Instance,
                logger.Object,
                CancellationToken.None);

            // Assert
            packageToUpdate.Should().NotBeNull();
            packageToUpdate.Id.Should().Be("Contoso.Utils");
            packageToUpdate.VersionRange.ToString().Should().Be("[1.2.3, )");
            logger.Invocations.Count.Should().Be(0);
        }

        [Fact]
        public async Task RequestPackageWithoutVersion_GetsLatestVersion()
        {
            // Arrange
            Pkg package = new()
            {
                Id = "Contoso.Utils",
                VersionRange = null
            };

            PackageSpec packageSpec = new TestPackageSpecFactory(builder =>
            {
                builder.WithProperty("TargetFramework", "net9.0")
                       .WithItem("PackageReference", "Contoso.Utils", [new("Version", "1.0.0")]);
            })
                .Build();

            var versionChooser = new Mock<IVersionChooser>(MockBehavior.Strict);
            versionChooser
                .Setup(v => v.GetLatestVersionAsync("Contoso.Utils", It.IsAny<ILoggerWithColor>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NuGetVersion("3.4.5"));

            var sourceCacheContext = new SourceCacheContext();
            var logger = new Mock<ILoggerWithColor>();

            // Act
            var (packageToUpdate, _) = await PackageUpdateCommandRunner.GetPackageToUpdateAsync(
                [package],
                packageSpec,
                versionChooser.Object,
                NullSettings.Instance,
                logger.Object,
                CancellationToken.None);

            // Assert
            packageToUpdate.Should().NotBeNull();
            packageToUpdate.Id.Should().Be("Contoso.Utils");
            packageToUpdate.VersionRange.ToString().Should().Be("[3.4.5, )");
            logger.Invocations.Count.Should().Be(0);
        }

        [Fact]
        public async Task RequestSinglePackageWithRangeSyntax_GetsRequestedVersion()
        {
            // Arrange
            Pkg package = new()
            {
                Id = "Contoso.Utils",
                VersionRange = VersionRange.Parse("[1.2.3,2.0.0)")
            };

            PackageSpec packageSpec = new TestPackageSpecFactory(builder =>
            {
                builder.WithProperty("TargetFramework", "net9.0")
                       .WithItem("PackageReference", "Contoso.Utils", [new("Version", "1.0.0")]);
            })
                .Build();

            var versionChooser = new Mock<IVersionChooser>(MockBehavior.Strict);
            var logger = new Mock<ILoggerWithColor>();

            // Act
            var (packageToUpdate, _) = await PackageUpdateCommandRunner.GetPackageToUpdateAsync(
                [package],
                packageSpec,
                versionChooser.Object,
                NullSettings.Instance,
                logger.Object,
                CancellationToken.None);

            // Assert
            packageToUpdate.Should().NotBeNull();
            packageToUpdate.Id.Should().Be("Contoso.Utils");
            packageToUpdate.VersionRange.ToString().Should().Be("[1.2.3, 2.0.0)");
            logger.Invocations.Count.Should().Be(0);
        }

        [Fact]
        public async Task RequestPackageNotOnSource_LogsError()
        {
            // Arrange
            Pkg package = new()
            {
                Id = "Contoso.Utils",
                VersionRange = null
            };

            PackageSpec packageSpec = new TestPackageSpecFactory(builder =>
            {
                builder.WithProperty("TargetFramework", "net9.0")
                       .WithItem("PackageReference", "Contoso.Utils", [new("Version", "1.0.0")]);
            })
                .Build();

            var versionChooser = new Mock<IVersionChooser>(MockBehavior.Strict);
            versionChooser
                .Setup(v => v.GetLatestVersionAsync("Contoso.Utils", It.IsAny<ILoggerWithColor>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((NuGetVersion?)null);
            var logger = new Mock<ILoggerWithColor>();

            // Act
            var (packageToUpdate, _) = await PackageUpdateCommandRunner.GetPackageToUpdateAsync(
                [package],
                packageSpec,
                versionChooser.Object,
                NullSettings.Instance,
                logger.Object,
                CancellationToken.None);

            // Assert
            packageToUpdate.Should().BeNull();
            logger.Invocations.Count.Should().BeGreaterThan(0);
        }
    }
}
