// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NuGet.LibraryModel;
using NuGet.ProjectManagement;
using Test.Utility;
using Xunit;

namespace NuGet.Commands.Test.RestoreCommandTests.Utility;

public class PackageSpecFactoryTests
{
    // The initial version of PackageSpecFactory is intended to maintain existing behavior with shared/common code, not
    // fix discrepencies. Before PackageSpecFactory, CPS and Legacy projects in VS would read the ManagePackageVersionsCentrally
    // property, while MSBuild and static restore reads the _CentalPackageVersionsEnabled property. The MSBuild targets sets
    // _CentralPackageVersionsEnabled based on the value of ManagePackageVersionsCentrally but also if a Directory.Packages.props
    // file exists/was imported. So, there are theoretical scenarios where the two properties could be different.
    [Theory]
    // BuildingInsideVisualStudio=true: reads ManagePackageVersionsCentrally
    [InlineData("true", "true", null, true)]
    [InlineData("true", "false", null, false)]
    [InlineData("true", null, null, false)]
    // BuildingInsideVisualStudio=false/absent: reads _CentralPackageVersionsEnabled
    [InlineData("false", null, "true", true)]
    [InlineData("false", null, "false", false)]
    [InlineData("false", null, null, false)]
    [InlineData(null, null, "true", true)]
    [InlineData(null, null, "false", false)]
    [InlineData(null, null, null, false)]
    public void GetPackageSpec_CpmEnabledByCorrectPropertyForBuildContext(
        string? buildingInsideVisualStudio,
        string? managePackageVersionsCentrally,
        string? centralPackageVersionsEnabled,
        bool expectedCpmEnabled)
    {
        // Arrange
        var factory = new TestPackageSpecFactory(outerBuild =>
        {
            outerBuild.WithProperty("TargetFramework", "net8.0");
            if (buildingInsideVisualStudio is not null)
            {
                outerBuild.WithProperty("BuildingInsideVisualStudio", buildingInsideVisualStudio);
            }
            if (managePackageVersionsCentrally is not null)
            {
                outerBuild.WithProperty(ProjectBuildProperties.ManagePackageVersionsCentrally, managePackageVersionsCentrally);
            }
            if (centralPackageVersionsEnabled is not null)
            {
                outerBuild.WithProperty("_CentralPackageVersionsEnabled", centralPackageVersionsEnabled);
            }
            outerBuild.WithItem("PackageReference", "SomePackage", null);
        });

        // Act
        var packageSpec = factory.Build();

        // Assert
        packageSpec.RestoreMetadata.CentralPackageVersionsEnabled.Should().Be(expectedCpmEnabled);
    }

    [Fact]
    public void GetPackageSpec_WithAnalyzerAssetsEnabled_PopulatesRestoreMetadata()
    {
        // Arrange
        var factory = new TestPackageSpecFactory(outerBuild =>
        {
            outerBuild.WithProperty("TargetFramework", "net8.0");
            outerBuild.WithProperty("RestoreEnableAnalyzerAssets", "true");
            outerBuild.WithItem("PackageReference", "SomePackage", null);
        });

        // Act
        var packageSpec = factory.Build();

        // Assert
        packageSpec.RestoreMetadata.RestoreEnableAnalyzerAssets.Should().BeTrue();
    }

    [Fact]
    public void GetPackageSpec_WithAnalyzerAssetsEnabledInAnyInnerBuild_IsEnabled()
    {
        // Arrange
        var factory = new TestPackageSpecFactory(outerBuild =>
        {
            outerBuild.WithProperty("TargetFrameworks", "net8.0;net9.0");
            outerBuild.WithProperty("RestoreEnableAnalyzerAssets", "false");
            outerBuild.WithItem("PackageReference", "SomePackage", null);
        })
        .WithInnerBuild(innerBuild =>
        {
            innerBuild.WithProperty("TargetFramework", "net8.0");
            innerBuild.WithProperty("RestoreEnableAnalyzerAssets", "false");
        })
        .WithInnerBuild(innerBuild =>
        {
            innerBuild.WithProperty("TargetFramework", "net9.0");
            innerBuild.WithProperty("RestoreEnableAnalyzerAssets", "true");
        });

        // Act
        var packageSpec = factory.Build();

        // Assert
        packageSpec.RestoreMetadata.RestoreEnableAnalyzerAssets.Should().BeTrue();
    }

    [Fact]
    public void GetPackageSpec_WithAnalyzerAssetsEnabledInOuterBuild_IsEnabledRegardlessOfInnerBuilds()
    {
        // Arrange
        var factory = new TestPackageSpecFactory(outerBuild =>
        {
            outerBuild.WithProperty("TargetFrameworks", "net8.0;net9.0");
            outerBuild.WithProperty("RestoreEnableAnalyzerAssets", "true");
            outerBuild.WithItem("PackageReference", "SomePackage", null);
        })
        .WithInnerBuild(innerBuild =>
        {
            innerBuild.WithProperty("TargetFramework", "net8.0");
            innerBuild.WithProperty("RestoreEnableAnalyzerAssets", "false");
        })
        .WithInnerBuild(innerBuild =>
        {
            innerBuild.WithProperty("TargetFramework", "net9.0");
            innerBuild.WithProperty("RestoreEnableAnalyzerAssets", "false");
        });

        // Act
        var packageSpec = factory.Build();

        // Assert
        packageSpec.RestoreMetadata.RestoreEnableAnalyzerAssets.Should().BeTrue();
    }

    [Fact]
    public void GetPackageSpec_WithPackageReferenceAnalyzerAssetMetadata_PopulatesDependencyAssetFlags()
    {
        // Arrange
        var factory = new TestPackageSpecFactory(outerBuild =>
        {
            outerBuild.WithProperty("TargetFramework", "net8.0");
            outerBuild.WithItem(
                "PackageReference",
                "SomePackage",
                [
                    new KeyValuePair<string, string>("IncludeAssets", "runtime;analyzers"),
                    new KeyValuePair<string, string>("ExcludeAssets", "runtime"),
                    new KeyValuePair<string, string>("PrivateAssets", "analyzers"),
                ]);
        });

        // Act
        var packageSpec = factory.Build();
        var dependency = packageSpec.TargetFrameworks.Single().Dependencies.Single();

        // Assert
        dependency.IncludeType.Should().Be(LibraryIncludeFlags.Analyzers);
        dependency.SuppressParent.Should().Be(LibraryIncludeFlags.Analyzers);
    }

    [Fact]
    public void GetPackageSpec_WithProjectReferenceAnalyzerAssetMetadata_PopulatesProjectReferenceAssetFlags()
    {
        // Arrange
        var factory = new TestPackageSpecFactory(outerBuild =>
        {
            outerBuild.WithProperty("TargetFramework", "net8.0");
            outerBuild.WithItem(
                ProjectItems.ProjectReference,
                "..\\Referenced\\Referenced.csproj",
                [
                    new KeyValuePair<string, string>("IncludeAssets", "runtime;analyzers"),
                    new KeyValuePair<string, string>("ExcludeAssets", "runtime"),
                    new KeyValuePair<string, string>("PrivateAssets", "analyzers"),
                ]);
        });

        // Act
        var packageSpec = factory.Build();
        var projectReference = packageSpec.RestoreMetadata.TargetFrameworks.Single().ProjectReferences.Single();

        // Assert
        projectReference.IncludeAssets.Should().Be(LibraryIncludeFlags.Runtime | LibraryIncludeFlags.Analyzers);
        projectReference.ExcludeAssets.Should().Be(LibraryIncludeFlags.Runtime);
        projectReference.PrivateAssets.Should().Be(LibraryIncludeFlags.Analyzers);
    }
}
