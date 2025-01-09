// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Serialization;

/// <summary>
/// These tests update elements in a slnx file. These tests start with the KitchenSink slnx file and modify it.
/// This test is intented to exercise the code that updates child items in a collection and maintains the correct whitespace.
/// </summary>
/// <remarks>
/// The results of these tests aren't always intuitive since the input slnx file is intentionally not ordered correctly.
/// The insertion logic is designed to work best when documents start ordered.
/// </remarks>
public class ManipulateXmlKitchenSink
{
    /// <summary>
    /// Adds configurations and dependencies to the solution and verifies they
    /// are added to the expected location.
    /// </summary>
    [Fact]
    public async Task AddConfigurations()
    {
        if (IsMono)
        {
            // Mono is not supported.
            return;
        }

        await ValidateModifiedSolutionAsync(
            CreateModifiedModel,
            SlnAssets.LoadResource("SlnxWhitespace/KitchenSink.slnx"),
            SlnAssets.LoadResource("SlnxWhitespace/KitchenSink-AddConfigurations.slnx"));

        static void CreateModifiedModel(SolutionModel solution)
        {
            solution.AddPlatform("Z80");
            solution.AddBuildType("Fast");

            ProjectType newType = new ProjectType(Guid.Empty, []) { Extension = ".emptyproj" };
            solution.ProjectTypes = [newType, .. solution.ProjectTypes];

            SolutionProjectModel? project = solution.FindProject(Path.Join("other", "Project4.nativeproj"));
            Assert.NotNull(project);
            project.AddProjectConfigurationRule(new ConfigurationRule(BuildDimension.Platform, "*", "Arm64", "Z80"));

            SolutionProjectModel? project3 = solution.FindProject("Project3.csproj");
            Assert.NotNull(project3);
            project.AddDependency(project3);
        }
    }

    /// <summary>
    /// Adds folders to the solution and verifies they are added to the expected location.
    /// </summary>
    [Fact]
    public async Task AddFolders()
    {
        await ValidateModifiedSolutionAsync(
            CreateModifiedModel,
            SlnAssets.LoadResource("SlnxWhitespace/KitchenSink.slnx"),
            SlnAssets.LoadResource("SlnxWhitespace/KitchenSink-AddFolders.slnx"));

        static void CreateModifiedModel(SolutionModel solution)
        {
            _ = solution.AddFolder("/AAAA/NewFolder/");
            _ = solution.AddFolder("/NewFolder/");
            _ = solution.AddFolder("/ZZZZ/NewFolder/");
        }
    }

    /// <summary>
    /// Adds projects to the solution and verifies they are added to the expected location.
    /// </summary>
    [Fact]
    public async Task AddProjects()
    {
        await ValidateModifiedSolutionAsync(
            CreateModifiedModel,
            SlnAssets.LoadResource("SlnxWhitespace/KitchenSink.slnx"),
            SlnAssets.LoadResource("SlnxWhitespace/KitchenSink-AddProjects.slnx"));

        static void CreateModifiedModel(SolutionModel solution)
        {
            SolutionFolderModel? folder = solution.FindFolder("/Other Projects/");
            Assert.NotNull(folder);

            _ = solution.AddProject("ProjectZ.csproj");
            _ = solution.AddProject("other/Project5555.csproj", folder: folder);
        }
    }

    /// <summary>
    /// Adds properties to the solution and verifies they are added to the expected location.
    /// </summary>
    [Fact]
    public async Task AddPropertySets()
    {
        await ValidateModifiedSolutionAsync(
            CreateModifiedModel,
            SlnAssets.LoadResource("SlnxWhitespace/KitchenSink.slnx"),
            SlnAssets.LoadResource("SlnxWhitespace/KitchenSink-AddPropertySets.slnx"));

        static void CreateModifiedModel(SolutionModel solution)
        {
            SolutionFolderModel? folder = solution.FindFolder("/Managed Projects/");
            Assert.NotNull(folder);

            SolutionPropertyBag props = folder.AddProperties("NEW Folder Properties");
            props.Add("Key", "Value");

            SolutionProjectModel? project = solution.FindProject("Project3.nativeproj");
            Assert.NotNull(project);

            SolutionPropertyBag projectProps = project.AddProperties("NEW Project Properties");
            projectProps.Add("Key", "Value");

            SolutionPropertyBag solutionProps = solution.AddProperties("NEW Solution Properties");
            solutionProps.Add("Key", "Value");
        }
    }

    /// <summary>
    /// Adds files to the solution and verifies they are added to the expected location.
    /// </summary>
    [Fact]
    public async Task AddSolutionFiles()
    {
        await ValidateModifiedSolutionAsync(
            CreateModifiedModel,
            SlnAssets.LoadResource("SlnxWhitespace/KitchenSink.slnx"),
            SlnAssets.LoadResource("SlnxWhitespace/KitchenSink-AddSolutionFiles.slnx"));

        static void CreateModifiedModel(SolutionModel solution)
        {
            SolutionFolderModel? folder = solution.FindFolder("/Other Projects/");
            Assert.NotNull(folder);

            folder.AddFile("File0.txt");
            folder.AddFile("FileZ.txt");
        }
    }
}
