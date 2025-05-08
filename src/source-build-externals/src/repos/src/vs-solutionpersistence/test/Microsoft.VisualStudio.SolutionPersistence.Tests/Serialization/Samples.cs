// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Serialization;

/// <summary>
/// Tests exercising the sample code to make sure it works as expected.
/// </summary>
public class Samples
{
    /// <summary>
    /// Tests the wiki sample code to convert an SLN file to an SLNX file.
    /// </summary>
    [Fact]
    public async Task ConvertToSlnx()
    {
        if (IsMono)
        {
            // Mono is not supported.
            return;
        }

        string filePath = SlnAssets.ClassicSlnEverything.SaveResourceToTempFile();
        string slnxFilePath = Path.ChangeExtension(filePath, SolutionSerializers.SlnXml.DefaultFileExtension);

        try
        {
            await ConvertToSlnxAsync(filePath, slnxFilePath, CancellationToken.None);
        }
        finally
        {
            TryDeleteFile(filePath);
            TryDeleteFile(slnxFilePath);
        }
    }

    /// <summary>
    /// Tests the wiki sample code to read configurations from each project.
    /// </summary>
    [Fact]
    public async Task ReadEachProjectAsync()
    {
        SolutionModel solution = await SolutionSerializers.SlnFileV12.OpenAsync(SlnAssets.ClassicSlnEverything.Stream, CancellationToken.None);
        ReadEachProject(solution);
    }

    /// <summary>
    /// Tests the wiki sample code to add a project.
    /// </summary>
    [Fact]
    public async Task AddProjectAsync()
    {
        SolutionModel solution = await SolutionSerializers.SlnFileV12.OpenAsync(SlnAssets.ClassicSlnEverything.Stream, CancellationToken.None);

        string solutionFolder = "/Folder/Subfolder/";
        string projectFilePath = "Project/Project.csproj";

        AddProject(solution, solutionFolder, projectFilePath);
    }

    private static async Task ConvertToSlnxAsync(string filePath, string slnxFilePath, CancellationToken cancellationToken)
    {
        // See if the file is a known solution file.
        ISolutionSerializer? serializer = SolutionSerializers.GetSerializerByMoniker(filePath);
        if (serializer is null)
        {
            return;
        }

        try
        {
            SolutionModel solution = await serializer.OpenAsync(filePath, cancellationToken);
            await SolutionSerializers.SlnXml.SaveAsync(slnxFilePath, solution, cancellationToken);
        }
        catch (SolutionException)
        {
            // There was an unrecoverable syntax error reading the solution file.
            return;
        }
    }

    private static void ReadEachProject(SolutionModel solution)
    {
        // Pick the first configuration in the solution.
        string buildConfiguration = solution.BuildTypes[0];
        string buildPlatform = solution.Platforms[0];

        foreach (SolutionProjectModel project in solution.SolutionProjects)
        {
            string projectFilePath = project.FilePath;

            // Find the project configuration for the solution build configuration and platform.
            (string? buildType, string? platform, bool build, bool deploy) = project.GetProjectConfiguration(buildConfiguration, buildPlatform);
            if (build)
            {
                Console.WriteLine("Project {0} is set to build as {1}|{2}.", projectFilePath, buildType, platform);
            }

            if (deploy)
            {
                Console.WriteLine("Project {0} is set to deploy as {1}|{2}.", projectFilePath, buildType, platform);
            }
        }
    }

    private static void AddProject(SolutionModel solution, string? solutionFolder, string projectFilePath)
    {
        SolutionFolderModel? parentSolutionFolder = null;
        if (solutionFolder is not null)
        {
            parentSolutionFolder = solution.AddFolder(solutionFolder);
        }

        // If the project type can be determined from the file extension, the project type name is optional.
        // If it cannot, a built-in name can be used, such as "Website" for a website project.
        // If it is not a built-in name, a project type can be added to the solution.
        // Finally the project type id guid can be used.
        string? projectTypeName = null;

        SolutionProjectModel newProject = solution.AddProject(projectFilePath, projectTypeName, parentSolutionFolder);
    }
}
