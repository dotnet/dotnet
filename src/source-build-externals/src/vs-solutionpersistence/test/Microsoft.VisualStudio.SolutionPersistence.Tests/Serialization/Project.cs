// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Serialization;

/// <summary>
/// Tests related to manipulating projects in the model.
/// </summary>
public sealed class Project
{
    /// <summary>
    /// Ensure the model can be used to add projects.
    /// This also validates the API fails correctly when adding invalid projects.
    /// </summary>
    [Fact]
    public void AddProject()
    {
        string projectPath = Path.Join("..", "Folder", "Subfolder", "ProjectFileName.csproj");
        string anotherProjectPath = Path.Join("..", "Folder", "Subfolder", "AnotherProjectFileName.csproj");

        SolutionModel solution = new SolutionModel();
        SolutionFolderModel folder = solution.AddFolder("/Solution Items/");
        SolutionProjectModel project = solution.AddProject(projectPath, projectTypeName: null);
        SolutionProjectModel anotherProject = solution.AddProject(anotherProjectPath, projectTypeName: null);

        Assert.Equal("ProjectFileName", project.ActualDisplayName);
        Assert.Empty(project.Type);

        // Verify error if same project is added again.
        {
            Exception ex = Assert.Throws<SolutionArgumentException>(() => solution.AddProject(projectPath, projectTypeName: null));
            Assert.StartsWith(string.Format(Errors.DuplicateItemRef_Args2, projectPath, "Project"), ex.Message);
        }

        // Verify error if same project is added to folder.
        {
            Exception ex = Assert.Throws<SolutionArgumentException>(() => solution.AddProject(projectPath, projectTypeName: null, folder: folder));
            Assert.StartsWith(string.Format(Errors.DuplicateItemRef_Args2, projectPath, "Project"), ex.Message);
        }

        // Verify error if same project is added with different case..
        {
            string projectPathUpper = projectPath.ToUpperInvariant();
            Exception ex = Assert.Throws<SolutionArgumentException>(() => solution.AddProject(projectPathUpper, projectTypeName: null));
            Assert.StartsWith(string.Format(Errors.DuplicateItemRef_Args2, projectPathUpper, "Project"), ex.Message);
        }

        // Try chaging a path to an existing project.
        {
            Exception ex = Assert.Throws<SolutionArgumentException>(() => anotherProject.FilePath = project.FilePath);
            Assert.StartsWith(string.Format(Errors.DuplicateItemRef_Args2, projectPath, "Project"), ex.Message);
        }
    }

    /// <summary>
    /// Ensures the model can be used to remove projects.
    /// </summary>
    [Fact]
    public async Task RemoveProjectAsync()
    {
        if (IsMono)
        {
            // Mono is not supported.
            return;
        }

        SolutionModel solution = await SolutionSerializers.SlnXml.OpenAsync(SlnAssets.XmlSlnxEverything.Stream, CancellationToken.None);

        string toRemove = Path.Join("src", "CoreMauiApp", "CoreMauiApp.csproj");

        SolutionProjectModel? projectToRemove = solution.FindProject(toRemove);
        Assert.NotNull(projectToRemove);

        Assert.True(solution.RemoveProject(projectToRemove));

        (SolutionModel reserializedSolution, FileContents _) = await SaveAndReopenModelAsync(SolutionSerializers.SlnXml, solution);

        Assert.Null(reserializedSolution.FindProject(toRemove));
    }

    /// <summary>
    /// Ensures the model can be used to move projects to a soluiton folder.
    /// </summary>
    [Fact]
    public async Task MoveProjectAsync()
    {
        if (IsMono)
        {
            // Mono is not supported.
            return;
        }

        SolutionModel solution = await SolutionSerializers.SlnXml.OpenAsync(SlnAssets.XmlSlnxEverything.Stream, CancellationToken.None);

        string toMove = Path.Join("BlazorApp1", "BlazorApp1.csproj");

        SolutionFolderModel? solutionFolder = solution.FindFolder("/SolutionFolder/");

        SolutionProjectModel? projectToMove = solution.FindProject(toMove);
        Assert.NotNull(projectToMove);

        projectToMove.MoveToFolder(solutionFolder);

        (SolutionModel reserializedSolution, FileContents _) = await SaveAndReopenModelAsync(SolutionSerializers.SlnXml, solution);

        SolutionProjectModel? foundProject = reserializedSolution.FindProject(toMove);
        Assert.NotNull(foundProject);
        Assert.NotNull(foundProject.Parent);
        Assert.Equal("/SolutionFolder/", foundProject.Parent.Path);
    }
}
