// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Serialization;

/// <summary>
/// Tests related to configuration rules in the model.
/// </summary>
public sealed class Configurations
{
    /// <summary>
    /// Tests the <see cref="SolutionProjectModel.GetProjectConfiguration(string, string)"/> API.
    /// </summary>
    [Fact]
    public void ProjectConfigurations()
    {
        const string platform = "TestPlatform";
        const string buildType = "Debug";
        const string projectPlatform = "ProjectTestPlatform";

        // Create a simple solution with a single project.
        SolutionModel solutionModel = new SolutionModel();

        solutionModel.AddPlatform(platform);
        solutionModel.AddPlatform("Any CPU");

        solutionModel.AddBuildType(buildType);
        solutionModel.AddBuildType("Release");

        SolutionProjectModel project = solutionModel.AddProject(Path.Join("Foo", "Foo.csproj"));

        // Add some project configurations for a specific solution configuration.
        project.AddProjectConfigurationRule(new ConfigurationRule(BuildDimension.Build, buildType, platform, bool.TrueString));
        project.AddProjectConfigurationRule(new ConfigurationRule(BuildDimension.Deploy, buildType, platform, bool.TrueString));
        project.AddProjectConfigurationRule(new ConfigurationRule(BuildDimension.BuildType, buildType, platform, buildType));
        project.AddProjectConfigurationRule(new ConfigurationRule(BuildDimension.Platform, buildType, platform, projectPlatform));

        Assert.NotNull(project.ProjectConfigurationRules);
        Assert.Equal(4, project.ProjectConfigurationRules.Count);

        // Remove implied configurations.
        solutionModel.DistillProjectConfigurations();

        Assert.Equal(2, project.ProjectConfigurationRules.Count);

        // Verify set configurations are still there.
        (string? BuildType, string? Platform, bool Build, bool Deploy) projectConfig = project.GetProjectConfiguration(buildType, platform);
        Assert.True(projectConfig.Build);
        Assert.True(projectConfig.Deploy);
        Assert.Equal(buildType, projectConfig.BuildType);
        Assert.Equal(projectPlatform, projectConfig.Platform);
    }

    /// <summary>
    /// A malformed .sln file can be missing configurations. This test validates that the model can handle this.
    /// </summary>
    [Fact]
    public async Task MissingSlnConfigurations()
    {
        // Solution with missing configurations.
        ResourceStream missing = SlnAssets.ClassicSlnMissingConfigurations;

        SolutionModel missingModel = await SolutionSerializers.SlnFileV12.OpenAsync(missing.Stream, CancellationToken.None);

        SolutionProjectModel? goodProject = missingModel.FindProject(Path.Join("Good", "Good.vcxproj"));
        Assert.NotNull(goodProject);

        SolutionProjectModel? missingProject = missingModel.FindProject(Path.Join("Missing", "Missing.vcxproj"));
        Assert.NotNull(missingProject);

        SolutionProjectModel? partialProject = missingModel.FindProject(Path.Join("Partial", "Partial.vcxproj"));
        Assert.NotNull(partialProject);

        // Debug|x86
        {
            string buildType = "Debug";
            string platform = "x86";

            // The good project has configurations for every solution configuration.
            (string? BuildType, string? Platform, bool Build, bool Deploy) good1 = goodProject.GetProjectConfiguration(buildType, platform);
            Assert.Equal("Debug", good1.BuildType);
            Assert.Equal("Win32", good1.Platform);
            Assert.True(good1.Build);
            Assert.False(good1.Deploy);

            // The missing project has no configurations.
            (string? BuildType, string? Platform, bool Build, bool Deploy) missing1 = missingProject.GetProjectConfiguration(buildType, platform);
            Assert.Null(missing1.BuildType);
            Assert.Null(missing1.Platform);
            Assert.False(missing1.Build);
            Assert.False(missing1.Deploy);

            // The good project has configurations for Release|x64, but just a build line for Debug|x86.
            (string? BuildType, string? Platform, bool Build, bool Deploy) partial1 = partialProject.GetProjectConfiguration(buildType, platform);
            Assert.Null(partial1.BuildType);
            Assert.Null(partial1.Platform);
            Assert.True(partial1.Build);
            Assert.False(partial1.Deploy);
        }

        // Release|x64
        {
            string buildType = "Release";
            string platform = "x64";
            (string? BuildType, string? Platform, bool Build, bool Deploy) good1 = goodProject.GetProjectConfiguration(buildType, platform);
            Assert.Equal("Release", good1.BuildType);
            Assert.Equal("x64", good1.Platform);
            Assert.True(good1.Build);
            Assert.False(good1.Deploy);

            (string? BuildType, string? Platform, bool Build, bool Deploy) missing1 = missingProject.GetProjectConfiguration(buildType, platform);
            Assert.Null(missing1.BuildType);
            Assert.Null(missing1.Platform);
            Assert.False(missing1.Build);
            Assert.False(missing1.Deploy);

            (string? BuildType, string? Platform, bool Build, bool Deploy) partial1 = partialProject.GetProjectConfiguration(buildType, platform);
            Assert.Equal("Release", partial1.BuildType);
            Assert.Equal("x64", partial1.Platform);
            Assert.True(partial1.Build);
            Assert.False(partial1.Deploy);
        }
    }

    /// <summary>
    /// Validate that the model can handle configurations on a project without a project type.
    /// </summary>
    [Fact]
    public async Task UnknownProjectTypeWithConfigAsync()
    {
        ResourceStream projectTypeConfigResource = SlnAssets.LoadResource("ProjectTypeConfig.sln");

        SolutionModel solution = await SolutionSerializers.SlnFileV12.OpenAsync(projectTypeConfigResource.Stream, CancellationToken.None);

        SolutionProjectModel? project6 = solution.FindProject("Project6.wapproj");
        Assert.NotNull(project6);

        (string? BuildType, string? Platform, bool Build, bool Deploy) configs = project6.GetProjectConfiguration("Debug", "Win32");
        Assert.Equal("Debug", configs.BuildType);
        Assert.Equal("x86", configs.Platform);
        Assert.True(configs.Build);
    }
}
