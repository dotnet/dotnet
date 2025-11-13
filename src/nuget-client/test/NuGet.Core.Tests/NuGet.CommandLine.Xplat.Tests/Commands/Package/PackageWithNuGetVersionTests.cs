// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.CommandLine;
using FluentAssertions;
using NuGet.Versioning;
using Xunit;

using Pkg = NuGet.CommandLine.XPlat.Commands.Package.PackageWithNuGetVersion;

namespace NuGet.CommandLine.Xplat.Tests.Commands.Package
{
    public class PackageWithNuGetVersionTests
    {
        private RootCommand _exactVersionCommand;
        private Argument<IReadOnlyList<Pkg>> _exactVersionPackagesArgument;

        public PackageWithNuGetVersionTests()
        {
            _exactVersionCommand = new RootCommand();
            _exactVersionPackagesArgument = new Argument<IReadOnlyList<Pkg>>("packages")
            {
                Arity = ArgumentArity.ZeroOrMore,
                CustomParser = Pkg.Parse
            };
            _exactVersionCommand.Arguments.Add(_exactVersionPackagesArgument);
        }

        [Fact]
        public void Parse_PackageWithExactVersion_ReturnsPackageWithExactVersion()
        {
            // Arrange
            var result = _exactVersionCommand.Parse("packageId@1.2.3");

            // Act
            var packages = result.GetValue(_exactVersionPackagesArgument);

            // Assert
            IReadOnlyList<Pkg> expects = [new Pkg()
            {
                Id = "packageId",
                NuGetVersion = new NuGetVersion("1.2.3"),
            }];
            packages.Should().BeEquivalentTo(expects);
        }

        [Fact]
        public void Parse_TwoPackagesWithExactVersions_ReturnsListOfTwo()
        {
            // Arrange
            var result = _exactVersionCommand.Parse("packageId1@1.0.0 packageId2@2.3.4");

            // Act
            var packages = result.GetValue(_exactVersionPackagesArgument);

            // Assert
            IReadOnlyList<Pkg> expects = [
                new Pkg() { Id = "packageId1", NuGetVersion = new NuGetVersion("1.0.0") },
                new Pkg() { Id = "packageId2", NuGetVersion = new NuGetVersion("2.3.4") }
            ];
            packages.Should().BeEquivalentTo(expects);
        }

        [Fact]
        public void Parse_PackageWithoutVersion_ReturnsPackageWithNullVersion()
        {
            // Arrange & Act
            var result = _exactVersionCommand.Parse("packageId");

            // Act
            var packages = result.GetValue(_exactVersionPackagesArgument);

            // Assert
            IReadOnlyList<Pkg> expects = [new Pkg()
            {
                Id = "packageId",
                NuGetVersion = null,
            }];
            packages.Should().BeEquivalentTo(expects);
        }

        [Fact]
        public void Parse_VersionWithNoId_ReturnsError()
        {
            // Arrange & Act
            var result = _exactVersionCommand.Parse("@1.2.3");

            // Assert
            result.Errors.Should().ContainSingle();
        }

        [Fact]
        public void Parse_PackageWithRangeSyntax_ReturnsError()
        {
            // Arrange & Act
            var result = _exactVersionCommand.Parse("packageId@[1.2.3,2.0.0)");

            // Assert
            result.Errors.Should().ContainSingle();
        }

        [Fact]
        public void Parse_PackageWithInvalidVersion_ReturnsError()
        {
            // Arrange & Act
            var result = _exactVersionCommand.Parse("packageId@one");

            // Assert
            result.Errors.Should().ContainSingle();
        }

        [Fact]
        public void Equals_SameIdAndVersion_ReturnsTrue()
        {
            // Arrange
            var package1 = new Pkg()
            {
                Id = "packageId",
                NuGetVersion = new NuGetVersion("1.2.3"),
            };
            var package2 = new Pkg()
            {
                Id = "packageId",
                NuGetVersion = new NuGetVersion("1.2.3"),
            };
            // Act & Assert
            package1.Should().Be(package2);
        }

        [Fact]
        public void Equals_DifferentId_ReturnsFalse()
        {
            // Arrange
            var package1 = new Pkg()
            {
                Id = "packageId1",
                NuGetVersion = new NuGetVersion("1.2.3"),
            };
            var package2 = new Pkg()
            {
                Id = "packageId2",
                NuGetVersion = new NuGetVersion("1.2.3"),
            };
            // Act & Assert
            package1.Should().NotBe(package2);
        }

        [Fact]
        public void Equals_DifferentVersion_ReturnsFalse()
        {
            // Arrange
            var package1 = new Pkg()
            {
                Id = "packageId",
                NuGetVersion = new NuGetVersion("1.2.3"),
            };
            var package2 = new Pkg()
            {
                Id = "packageId",
                NuGetVersion = new NuGetVersion("2.0.0"),
            };
            // Act & Assert
            package1.Should().NotBe(package2);
        }

        [Fact]
        public void Equals_NullVersionAndNonNullVersion_ReturnsFalse()
        {
            // Arrange
            var package1 = new Pkg()
            {
                Id = "packageId",
                NuGetVersion = null,
            };
            var package2 = new Pkg()
            {
                Id = "packageId",
                NuGetVersion = new NuGetVersion("1.0.0"),
            };
            // Act & Assert
            package1.Should().NotBe(package2);
        }
    }
}
