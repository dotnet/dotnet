// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Serialization;

/// <summary>
/// Tests related to manipulating solution folders in the model.
/// </summary>
public sealed class Folders
{
    /// <summary>
    /// Remove a folder from the solution model.
    /// </summary>
    [Fact]
    public void RemoveFolder()
    {
        // Create a solution with a deep folder structure and projects.
        SolutionModel solution = new SolutionModel();
        SolutionFolderModel folderThis = solution.AddFolder("/This/");
        SolutionFolderModel folderIs = solution.AddFolder("/This/Is/");
        SolutionFolderModel folderA = solution.AddFolder("/This/Is/A/");
        SolutionFolderModel folderNested = solution.AddFolder("/This/Is/A/Nested/");
        SolutionFolderModel folderFolder = solution.AddFolder("/This/Is/A/Nested/Folder/");

        Assert.NotNull(folderThis);
        Assert.NotNull(folderIs);
        Assert.NotNull(folderA);
        Assert.NotNull(folderNested);
        Assert.NotNull(folderFolder);

        SolutionProjectModel projectInIs = solution.AddProject("ProjectInThis.csproj", folder: folderIs);
        Assert.NotNull(projectInIs);
        SolutionProjectModel projectInA = solution.AddProject("ProjectInA.csproj", folder: folderA);
        Assert.NotNull(projectInA);
        SolutionProjectModel projectInFolder = solution.AddProject("ProjectInFolder.csproj", folder: folderFolder);
        Assert.NotNull(projectInFolder);

        // Remove the middle 'A' folder.
        Assert.True(solution.RemoveFolder(folderA));

        // Make sure child folders were removed.
        Assert.Equal("/This/", folderThis.ItemRef);
        Assert.Equal("/This/Is/", folderIs.ItemRef);
        Assert.Null(solution.FindFolder(folderNested.ItemRef));
        Assert.Null(solution.FindFolder(folderFolder.ItemRef));

        // Make sure child projects were removed.
        Assert.Null(solution.FindProject(projectInA.ItemRef));
        Assert.Null(solution.FindProject(projectInFolder.ItemRef));

        // Make sure project in 'Is' folder was not removed.
        Assert.NotNull(projectInIs.Parent);
        Assert.NotNull(solution.FindProject(projectInIs.ItemRef));
        Assert.NotNull(projectInIs.Parent);
        Assert.Equal("/This/Is/", projectInIs.Parent.ItemRef);

        // Remove all folders in reverse.
        Assert.False(solution.RemoveFolder(folderFolder));
        Assert.False(solution.RemoveFolder(folderNested));
        Assert.True(solution.RemoveFolder(folderIs));
        Assert.True(solution.RemoveFolder(folderThis));

        Assert.Empty(solution.SolutionItems);
        Assert.Empty(solution.SolutionProjects);
        Assert.Empty(solution.SolutionFolders);
    }

    /// <summary>
    /// Rename a folder in the solution model.
    /// </summary>
    [Fact]
    public void RenameFolder()
    {
        // Create a solution with a deep folder structure and projects.
        SolutionModel solution = new SolutionModel();
        SolutionFolderModel folderThis = solution.AddFolder("/This/");
        SolutionFolderModel folderIs = solution.AddFolder("/This/Is/");
        SolutionFolderModel folderA = solution.AddFolder("/This/Is/A/");
        SolutionFolderModel folderNested = solution.AddFolder("/This/Is/A/Nested/");
        SolutionFolderModel folderFolder = solution.AddFolder("/This/Is/A/Nested/Folder/");

        Assert.NotNull(folderThis);
        Assert.NotNull(folderIs);
        Assert.NotNull(folderA);
        Assert.NotNull(folderNested);
        Assert.NotNull(folderFolder);

        SolutionProjectModel projectInA = solution.AddProject("ProjectInA.csproj", folder: folderA);
        Assert.NotNull(projectInA);
        SolutionProjectModel projectInFolder = solution.AddProject("ProjectInFolder.csproj", folder: folderFolder);
        Assert.NotNull(projectInFolder);

        folderA.Name = "The";

        // Make sure remaining folders have updated references.
        Assert.Equal("/This/", folderThis.ItemRef);
        Assert.Equal("/This/Is/", folderIs.ItemRef);
        Assert.Equal("/This/Is/The/", folderA.ItemRef);
        Assert.Equal("/This/Is/The/Nested/", folderNested.ItemRef);
        Assert.Equal("/This/Is/The/Nested/Folder/", folderFolder.ItemRef);

        // Make sure projects have updated references.
        Assert.NotNull(projectInA.Parent);
        Assert.Equal("/This/Is/The/", projectInA.Parent.ItemRef);

        Assert.NotNull(projectInFolder.Parent);
        Assert.Equal("/This/Is/The/Nested/Folder/", projectInFolder.Parent.ItemRef);
    }

    /// <summary>
    /// Ensures projects update correctly when moving between folders.
    /// </summary>
    [Fact]
    public void MoveProjectToFolder()
    {
        // Create a solution with a deep folder structure and projects.
        SolutionModel solution = new SolutionModel();
        SolutionFolderModel folderThis = solution.AddFolder("/This/");
        SolutionFolderModel folderIs = solution.AddFolder("/This/Is/");
        SolutionFolderModel folderA = solution.AddFolder("/This/Is/A/");
        SolutionFolderModel folderNested = solution.AddFolder("/This/Is/A/Nested/");
        SolutionFolderModel folderFolder = solution.AddFolder("/This/Is/A/Nested/Folder/");

        Assert.NotNull(folderThis);
        Assert.NotNull(folderIs);
        Assert.NotNull(folderA);
        Assert.NotNull(folderNested);
        Assert.NotNull(folderFolder);

        SolutionProjectModel existingProject = solution.AddProject(Path.Join("Different", "Project.csproj"), folder: folderA);

        SolutionProjectModel wanderingProject = solution.AddProject("Project.csproj");

        // Move project to folder
        wanderingProject.MoveToFolder(folderThis);
        Assert.NotNull(wanderingProject.Parent);
        Assert.Equal("/This/", wanderingProject.Parent.ItemRef);

        // Move project to another folder
        wanderingProject.MoveToFolder(folderFolder);
        Assert.NotNull(wanderingProject.Parent);
        Assert.Equal("/This/Is/A/Nested/Folder/", wanderingProject.Parent.ItemRef);

        // Try moving project to folder with existing project
        ArgumentException ex = Assert.Throws<SolutionArgumentException>(() => wanderingProject.MoveToFolder(folderA));
        Assert.Equal(string.Format(Errors.DuplicateProjectName_Arg1, wanderingProject.ActualDisplayName), ex.Message);
        Assert.Equal("/This/Is/A/Nested/Folder/", wanderingProject.Parent.ItemRef);

        // Move project to root
        wanderingProject.MoveToFolder(null);
        Assert.Null(wanderingProject.Parent);
    }

    /// <summary>
    /// Ensures that folders and subitems update correctly when moved to a new parent folder.
    /// </summary>
    [Fact]
    public void MoveFolder()
    {
        // Create a solution with a deep folder structure and projects.
        SolutionModel solution = new SolutionModel();
        SolutionFolderModel folderThis = solution.AddFolder("/This/");
        SolutionFolderModel folderIs = solution.AddFolder("/This/Is/");
        SolutionFolderModel folderA = solution.AddFolder("/This/Is/A/");
        SolutionFolderModel folderNested = solution.AddFolder("/This/Is/A/Nested/");
        SolutionFolderModel folderFolder = solution.AddFolder("/This/Is/A/Nested/Folder/");

        Assert.NotNull(folderThis);
        Assert.NotNull(folderIs);
        Assert.NotNull(folderA);
        Assert.NotNull(folderNested);
        Assert.NotNull(folderFolder);

        // Move folder to another folder
        folderFolder.MoveToFolder(folderA);
        Assert.NotNull(folderFolder.Parent);
        Assert.Equal("/This/Is/A/", folderFolder.Parent.ItemRef);

        // Try to move folder under itself.
        ArgumentException ex = Assert.Throws<SolutionArgumentException>(() => folderThis.MoveToFolder(folderNested));
        Assert.StartsWith(Errors.CannotMoveFolderToChildFolder, ex.Message);
    }

    /// <summary>
    /// Validate changing a folder name.
    /// Make sure it detects duplicates.
    /// </summary>
    [Fact]
    public void ChangeFolderName()
    {
        SolutionModel solution = new SolutionModel();

        SolutionFolderModel folderA = solution.AddFolder("/A/");
        SolutionFolderModel folderB = solution.AddFolder("/B/");
        SolutionFolderModel folderNestedA = solution.AddFolder("/A/Nested/Deep/");
        SolutionFolderModel folderNestedB = solution.AddFolder("/B/Nested/Deep/");

        // Try case exact
        {
            ArgumentException ex = Assert.Throws<SolutionArgumentException>(() => folderB.Name = "A");
            Assert.StartsWith(string.Format(Errors.DuplicateItemRef_Args2, "/A/", "Folder"), ex.Message);

            Assert.Equal("/B/Nested/Deep/", folderNestedB.Path);
        }

        // Try case insensitive
        {
            ArgumentException ex = Assert.Throws<SolutionArgumentException>(() => folderB.Name = "a");
            Assert.StartsWith(string.Format(Errors.DuplicateItemRef_Args2, "/a/", "Folder"), ex.Message);

            Assert.Equal("/B/Nested/Deep/", folderNestedB.Path);
        }

        Assert.Equal("/A/Nested/Deep/", folderNestedA.Path);
        Assert.Equal("/B/Nested/Deep/", folderNestedB.Path);

        // Try a successful rename.
        folderB.Name = "C";

        Assert.Equal("C", folderB.Name);
        Assert.Equal("/C/", folderB.Path);
        Assert.Equal("/A/Nested/Deep/", folderNestedA.Path);
        Assert.Equal("/C/Nested/Deep/", folderNestedB.Path);
    }
}
