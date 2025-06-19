// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;

namespace Serialization;

/// <summary>
/// Tests related to how cross-platform paths are handled in serialization.
/// </summary>
public sealed class PathSlashes
{
    /// <summary>
    /// Validates that the model correctly converts paths to the current environment's directory separator
    /// and that model references work correctly.
    /// </summary>
    [Fact]
    public async Task VerifyModelSlahesAsync()
    {
        if (IsMono)
        {
            // Mono is not supported.
            return;
        }

        ResourceStream slashesResource = SlnAssets.LoadResource("Invalid/PathSlashes.slnx");

        SolutionModel solution = await SolutionSerializers.SlnXml.OpenAsync(slashesResource.Stream, CancellationToken.None);

        // Check the files in the folder.
        SolutionFolderModel folder = Assert.Single(solution.SolutionFolders);
        Assert.Equal(
            [
                Path.Join("..", "forwardslash.txt"),
                Path.Join("..", "backslash.txt"),
            ],
            folder.Files);

        // Check the project paths.
        HashSet<string> expectedProjectPaths =
            [
                Path.Join("folder", "forwardslash.csproj"),
                Path.Join("folder", "backslash.csproj"),
                Path.Join("folder", "folder", "mixed.csproj"),
                Path.Join("referenceForward.csproj"),
                Path.Join("referenceBack.csproj"),
            ];
        HashSet<string> projectPaths = [.. solution.SolutionProjects.ToArray(p => p.FilePath)];
        Assert.Equal(expectedProjectPaths, projectPaths);

        // Check the project references.
        SolutionProjectModel? forwardReference = solution.FindProject("referenceForward.csproj");
        Assert.NotNull(forwardReference);
        Assert.Equal(3, forwardReference.Dependencies?.Count);

        SolutionProjectModel? backReference = solution.FindProject("referenceBack.csproj");
        Assert.NotNull(backReference);
        Assert.Equal(3, backReference.Dependencies?.Count);
    }

    /// <summary>
    /// Verify project reference are normalized on save.
    /// </summary>
    [Fact]
    public async Task ProjectReferencesFixedUpAsync()
    {
        if (IsMono)
        {
            // Mono is not supported.
            return;
        }

        FileContents fixedSlashesResource = SlnAssets.LoadResource("PathSlashes.slnx").ToLines();

        // Save the Model back to stream.
        ResourceStream slashesResource = SlnAssets.LoadResource("Invalid/PathSlashes.slnx");
        SolutionModel solution = await SolutionSerializers.SlnXml.OpenAsync(slashesResource.Stream, CancellationToken.None);
        FileContents reserializedSolution = await solution.ToLinesAsync(SolutionSerializers.SlnXml);

        SlnTestHelper.AssertSolutionsAreEqual(fixedSlashesResource, reserializedSolution);
    }
}
