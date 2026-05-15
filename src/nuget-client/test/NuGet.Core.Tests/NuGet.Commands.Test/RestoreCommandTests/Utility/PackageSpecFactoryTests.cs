// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using FluentAssertions;
using NuGet.Configuration;
using NuGet.ProjectManagement;
using NuGet.Test.Utility;
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

    [Theory]
    // No RestorePackagesPath set → falls back to globalPackagesFolder from settings
    [InlineData(null, null, "custom-gpf")]
    // RestorePackagesPath set as project property → uses project value
    [InlineData(null, "my-packages", "my-packages")]
    // RestorePackagesPath set as global property → global override wins
    [InlineData("override-packages", "my-packages", "override-packages")]
    public void ApplySettings_PackagesPath_ResolvedFromCorrectSource(
        string? globalOverride,
        string? projectPackagesPath,
        string expected)
    {
        // Arrange
        using var testDirectory = TestDirectory.Create();
        string projectPath = Path.Combine(testDirectory, "my.csproj");
        string globalPackagesFolder = Path.Combine(testDirectory, "custom-gpf");

        var settings = new MockSettings
        {
            Sections = new[]
            {
                new MockSettingSection(
                    ConfigurationConstants.Config,
                    new AddItem(ConfigurationConstants.GlobalPackagesFolder, globalPackagesFolder))
            }
        };

        var factory = new TestPackageSpecFactory(projectPath, outerBuild =>
        {
            outerBuild.WithProperty("TargetFramework", "net8.0");
            if (projectPackagesPath is not null)
            {
                outerBuild.WithProperty("RestorePackagesPath", projectPackagesPath);
            }
        });
        if (globalOverride is not null)
        {
            factory.WithGlobalProperty("RestorePackagesPath", globalOverride);
        }

        // Act
        var packageSpec = factory.Build(settings);

        // Assert
        packageSpec.RestoreMetadata.PackagesPath.Should().Be(Path.Combine(testDirectory, expected));
    }
}
