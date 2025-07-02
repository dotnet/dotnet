// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Serialization;

/// <summary>
/// Tests for validating the solution model.
/// </summary>
public class Validation
{
    /// <summary>
    /// Validates that the model throws when adding projects with the same project path.
    /// </summary>
    [Fact]
    public void DuplicateProjects()
    {
        // Create a solution with duplicate projects.
        SolutionModel solution = new SolutionModel();
        _ = solution.AddProject("Project.csproj");

        _ = Assert.ThrowsAny<ArgumentException>(() => solution.AddProject("Project.csproj"));
    }

    /// <summary>
    /// Validates that the model throws when adding projects with the same display name in the root of the solution.
    /// </summary>
    [Fact]
    public void DuplicateProjectNamesInRoot()
    {
        // Create a solution with duplicate project names, but in different folders.
        SolutionModel solution = new SolutionModel();
        _ = solution.AddProject(Path.Join("Folder1", "Project.csproj"));

        _ = Assert.ThrowsAny<ArgumentException>(() => solution.AddProject(Path.Join("Folder2", "Project.csproj")));
    }

    /// <summary>
    /// Validates that the model throws when adding projects with the same display name in the same solution folder.
    /// </summary>
    [Fact]
    public void DuplicateProjectNamesInSameFolder()
    {
        // Create a solution with duplicate project names, but in different folders.
        SolutionModel solution = new SolutionModel();
        SolutionFolderModel folder = solution.AddFolder("/Folder/");
        _ = solution.AddProject(Path.Join("Folder1", "Project.csproj"), folder: folder);

        _ = Assert.ThrowsAny<ArgumentException>(() => solution.AddProject(Path.Join("Folder2", "Project.csproj"), folder: folder));
    }

    /// <summary>
    /// Validates that the model *allows* adding projects with the same display name in different solution folders.
    /// </summary>
    [Fact]
    public void DuplicateProjectNamesInDifferentFolder()
    {
        // Create a solution with duplicate project names, but in different folders.
        SolutionModel solution = new SolutionModel();

        SolutionFolderModel folder1 = solution.AddFolder("/Folder1/");
        Assert.NotNull(solution.AddProject(Path.Join("Folder1", "Project.csproj"), folder: folder1));

        SolutionFolderModel folder2 = solution.AddFolder("/Folder2/");
        Assert.NotNull(solution.AddProject(Path.Join("Folder2", "Project.csproj"), folder: folder2));
    }

    /// <summary>
    /// Validates the naming check is consistent with existing naming rules.
    /// </summary>
    /// <remarks>
    /// This code is shared between projects, folders and configurations. Although this just validates configurations.
    /// </remarks>
    [Fact]
    public void ConfigurationName()
    {
        SolutionModel solution = new SolutionModel();

        string[] invalidNames = [
            null!,
            string.Empty,
            "\0",
            " ",
            "\t",
            "/",
            "\\",
            "\"",
            "<",
            ">",
            "?",
            "*",
            ":",
            "|",
            "con",
            "com1",
            "lpt9",
            ".",
            "..",
        ];

        string[] validNames = [
            new string('+', 260),
            "com0",
            "com200",
            "...",
            "$",
            "€",
            "=_+-[]{}'`.^",
            "𓃠",
            "🐈",
            "cat",
            "gato",
            "pisică",
            "kočka",
            "котка",
            "кошка",
            "قط",
            "แมว",
            "बिल्ली",
            "חתול",
            "猫",
            "貓",
        ];

        foreach (string invalidName in invalidNames)
        {
            // Platforms
            {
                ArgumentException ex = Assert.ThrowsAny<ArgumentException>(() => solution.AddPlatform(invalidName));
                if (!string.IsNullOrWhiteSpace(invalidName))
                {
                    Assert.StartsWith(Errors.InvalidName, ex.Message);
                }
            }

            // Build Types
            {
                ArgumentException ex = Assert.ThrowsAny<ArgumentException>(() => solution.AddBuildType(invalidName));
                if (!string.IsNullOrWhiteSpace(invalidName))
                {
                    Assert.StartsWith(Errors.InvalidName, ex.Message);
                }
            }
        }

        foreach (string validName in validNames)
        {
            solution.AddPlatform(validName);
            Assert.Contains(validName, solution.Platforms);

            solution.AddBuildType(validName);
            Assert.Contains(validName, solution.BuildTypes);
        }

        Assert.Equal(validNames.Length, solution.Platforms.Count);
        Assert.Equal(validNames.Length, solution.BuildTypes.Count);
    }

    /// <summary>
    /// Validates the model correctly validates when adding invalid solution folders.
    /// </summary>
    [Fact]
    public void SolutionFolders()
    {
        // Create a solution with duplicate solution folders.
        SolutionModel solution = new SolutionModel();

        string invalidNameError = Errors.InvalidName;

        // Don't allow invalid characters
        Assert.StartsWith(invalidNameError, Assert.ThrowsAny<ArgumentException>(() => solution.AddFolder("/Foo</")).Message);

        // Don't allow reserved names
        Assert.StartsWith(invalidNameError, Assert.ThrowsAny<ArgumentException>(() => solution.AddFolder("/LPT4/")).Message);
        Assert.StartsWith(invalidNameError, Assert.ThrowsAny<ArgumentException>(() => solution.AddFolder("/Aux/")).Message);
        Assert.StartsWith(invalidNameError, Assert.ThrowsAny<ArgumentException>(() => solution.AddFolder("/../")).Message);

        // Verify the maximum length of a folder name
        string longName = "/" + new string('v', 260) + "/";
        _ = solution.AddFolder(longName);
        _ = solution.AddFolder(longName + "after/");
        _ = solution.AddFolder("/before" + longName);

        // Don't allow long names
        string tooLongName = "/" + new string('v', 261) + "/";
        _ = Assert.ThrowsAny<ArgumentOutOfRangeException>(() => solution.AddFolder(tooLongName));
        _ = Assert.ThrowsAny<ArgumentOutOfRangeException>(() => solution.AddFolder(tooLongName + "after/"));
        _ = Assert.ThrowsAny<ArgumentOutOfRangeException>(() => solution.AddFolder("/before" + tooLongName));

        // Don't allow backslash
        _ = Assert.ThrowsAny<ArgumentException>(() => solution.AddFolder("/Foo\\Bar/"));

        // Don't allow double slashed
        _ = Assert.ThrowsAny<ArgumentNullException>(() => solution.AddFolder("/Foo//Bar/")).Message;
        _ = Assert.ThrowsAny<ArgumentNullException>(() => solution.AddFolder("//Foo/Bar/"));
        _ = Assert.ThrowsAny<ArgumentNullException>(() => solution.AddFolder("/Foo/Bar//"));

        // Don't allow path without slashes
        string slashError = string.Format(Errors.InvalidFolderPath_Args1, "Foo");
        Assert.StartsWith(slashError, Assert.ThrowsAny<ArgumentException>(() => solution.AddFolder("Foo")).Message);

        // Folders added as ItemId's try to find existing folders.
        SolutionFolderModel subFolder1 = solution.AddFolder("/Folder/Subfolder/");
        SolutionFolderModel subFolder2 = solution.AddFolder("/Folder/Subfolder/");
        Assert.Equal(subFolder1, subFolder2);

        // Validate internal CreateFolder API for reading .SLN files.
        // This supports loading from SLN files where the folder name is the only thing known.
        SolutionFolderModel folder1 = solution.CreateFolder("UniqueFolder");
        SolutionFolderModel folder2 = solution.CreateFolder("UniqueFolder");
        Assert.NotEqual(folder1, folder2);
    }
}
