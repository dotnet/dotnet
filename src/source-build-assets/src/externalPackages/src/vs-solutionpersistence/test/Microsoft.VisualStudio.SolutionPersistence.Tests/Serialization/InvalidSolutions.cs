// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;

namespace Serialization;

/// <summary>
/// These tests validate that errors are reported when trying to open invalid or malformed solution files.
/// </summary>
public sealed class InvalidSolutions
{
    /// <summary>
    /// Checks for a file that isn't XML.
    /// </summary>
    [Fact]
    public Task InvalidXmlSlnxAsync()
    {
        return Assert.ThrowsAsync<XmlException>(async () =>
        {
            using MemoryStream memoryStream = new(Encoding.UTF8.GetBytes("Invalid slnx file"));
            _ = await SolutionSerializers.SlnXml.OpenAsync(memoryStream, CancellationToken.None);
        });
    }

    /// <summary>
    /// Checks for an XML file that isn't an SLNX file.
    /// </summary>
    [Fact]
    public async Task InvalidSlnxAsync()
    {
        ResourceStream wrongRoot = SlnAssets.LoadResource("Invalid/WrongRoot.slnx");
        string wrongRootFile = wrongRoot.SaveResourceToTempFile();

        SolutionException ex = await Assert.ThrowsAsync<SolutionException>(
            async () => _ = await SolutionSerializers.SlnXml.OpenAsync(wrongRootFile, CancellationToken.None));

        Assert.Equal(Errors.NotSolution, ex.Message);
        Assert.Equal(wrongRootFile, ex.File);

        // This exception should not have line or column information since there is no XmlElement associated with it.
        Assert.Null(ex.Line);
        Assert.Null(ex.Column);
    }

    /// <summary>
    /// Check for file that isn't an .sln file.
    /// </summary>
    [Fact]
    public async Task InvalidSlnAsync()
    {
        ResourceStream invalidSln = SlnAssets.LoadResource("Invalid/Invalid.sln");
        string invalidSlnFile = invalidSln.SaveResourceToTempFile();

        SolutionException ex = await Assert.ThrowsAsync<SolutionException>(
            async () => _ = await SolutionSerializers.SlnFileV12.OpenAsync(invalidSlnFile, CancellationToken.None));

        Assert.Equal(Errors.NotSolution, ex.Message);
        Assert.Equal(invalidSlnFile, ex.File);
        Assert.Equal(2, ex.Line);
        Assert.Null(ex.Column);
    }

    /// <summary>
    /// Check for loop in the configuration.
    /// </summary>
    [Fact]
    public async Task InvalidConfigurationLoopAsync()
    {
        ResourceStream basedOnLoop = SlnAssets.LoadResource("Invalid/BasedOnLoop.slnx");

        SolutionException ex = await Assert.ThrowsAsync<SolutionException>(
            async () => _ = await SolutionSerializers.SlnXml.OpenAsync(basedOnLoop.Stream, CancellationToken.None));

        Assert.Equal(2, ex.Line);
        Assert.Equal(6, ex.Column);
    }

    /// <summary>
    /// Check for a .sln missing the end tags.
    /// </summary>
    [Fact]
    public async Task MissingEndAsync()
    {
        ResourceStream missingEnd = SlnAssets.LoadResource("Invalid/MissingEnd.sln");
        SolutionModel solution = await SolutionSerializers.SlnFileV12.OpenAsync(missingEnd.Stream, CancellationToken.None);
        Assert.NotNull(solution.SerializerExtension);
        Assert.True(solution.SerializerExtension.Tarnished);
    }

    /// <summary>
    /// Check that extra lines are ignored.
    /// </summary>
    [Fact]
    public async Task ExtraLinesAsync()
    {
        ResourceStream extraLines = SlnAssets.LoadResource("Invalid/ExtraLines.sln");
        SolutionModel solution = await SolutionSerializers.SlnFileV12.OpenAsync(extraLines.Stream, CancellationToken.None);
        Assert.NotNull(solution.SerializerExtension);
        Assert.False(solution.SerializerExtension.Tarnished);

        // Save the Model back to stream.
        FileContents reserializedSolution = await solution.ToLinesAsync(SolutionSerializers.SlnFileV12);

        AssertSolutionsAreEqual(SlnAssets.ClassicSlnMin.ToLines(), reserializedSolution);
    }

    /// <summary>
    /// Tests for invalid solution folder name paths.
    /// </summary>
    [Fact]
    public async Task SolutionFolderAsync()
    {
        ResourceStream noEndSlash = SlnAssets.LoadResource("Invalid/SolutionFolder-NoEndSlash.slnx");
        SolutionException exNoEndSlash = await Assert.ThrowsAsync<SolutionException>(
            async () => _ = await SolutionSerializers.SlnXml.OpenAsync(noEndSlash.Stream, CancellationToken.None));
        Assert.StartsWith(string.Format(Errors.InvalidFolderPath_Args1, "/No/End/Slash"), exNoEndSlash.Message);
        Assert.Equal(2, exNoEndSlash.Line);
        Assert.Equal(6, exNoEndSlash.Column);

        ResourceStream noStartSlash = SlnAssets.LoadResource("Invalid/SolutionFolder-NoStartSlash.slnx");
        SolutionException exNoStartSlash = await Assert.ThrowsAsync<SolutionException>(
            async () => _ = await SolutionSerializers.SlnXml.OpenAsync(noStartSlash.Stream, CancellationToken.None));
        Assert.StartsWith(string.Format(Errors.InvalidFolderPath_Args1, "No/Start/Slash/"), exNoStartSlash.Message);
        Assert.Equal(2, exNoStartSlash.Line);
        Assert.Equal(6, exNoStartSlash.Column);

        ResourceStream wrongSlash = SlnAssets.LoadResource("Invalid/SolutionFolder-WrongSlash.slnx");
        SolutionException exWrongSlash = await Assert.ThrowsAsync<SolutionException>(
            async () => _ = await SolutionSerializers.SlnXml.OpenAsync(wrongSlash.Stream, CancellationToken.None));
        Assert.StartsWith(string.Format(Errors.InvalidName, @"/Wrong\Slash/"), exWrongSlash.Message);
        Assert.Equal(2, exWrongSlash.Line);
        Assert.Equal(6, exWrongSlash.Column);
    }

    /// <summary>
    /// Ensures solution fails to load if duplicate projects paths are found.
    /// </summary>
    [Fact]
    public async Task DuplicateProjectsAsync()
    {
        ResourceStream duplicateProjects = SlnAssets.LoadResource("Invalid/DuplicateProjects.slnx");
        SolutionException ex = await Assert.ThrowsAsync<SolutionException>(
            async () => _ = await SolutionSerializers.SlnXml.OpenAsync(duplicateProjects.Stream, CancellationToken.None));
        Assert.StartsWith(string.Format(Errors.DuplicateItemRef_Args2, "foo.vbproj", "Project"), ex.Message);
        Assert.Equal(3, ex.Line);
        Assert.Equal(10, ex.Column);
    }

    /// <summary>
    /// Ensure solution fails to load if duplicate folders are found.
    /// </summary>
    [Fact]
    public async Task DuplicateFoldersAsync()
    {
        ResourceStream duplicateFolders = SlnAssets.LoadResource("Invalid/DuplicateFolders.slnx");
        SolutionException ex = await Assert.ThrowsAsync<SolutionException>(
            async () => _ = await SolutionSerializers.SlnXml.OpenAsync(duplicateFolders.Stream, CancellationToken.None));
        Assert.StartsWith(string.Format(Errors.DuplicateItemRef_Args2, "/Solution Items/", "Folder"), ex.Message);
    }

    /// <summary>
    /// Ensure slnx solution fails to load if an unsupported future version is found.
    /// </summary>
    [Fact]
    public async Task FutureSlnxVersionAsync()
    {
        ResourceStream version = SlnAssets.LoadResource("Invalid/VersionFuture.slnx");
        SolutionException ex = await Assert.ThrowsAsync<SolutionException>(
            async () => _ = await SolutionSerializers.SlnXml.OpenAsync(version.Stream, CancellationToken.None));
        Assert.StartsWith(string.Format(Errors.UnsupportedVersion_Args1, "2.0"), ex.Message);
    }

    /// <summary>
    /// Ensure sln solution fails to load if an unsupported future version is found.
    /// </summary>
    [Fact]
    public async Task FutureSlnVersionAsync()
    {
        ResourceStream version = SlnAssets.LoadResource("Invalid/VersionFuture.sln");
        SolutionException ex = await Assert.ThrowsAsync<SolutionException>(
            async () => _ = await SolutionSerializers.SlnFileV12.OpenAsync(version.Stream, CancellationToken.None));
        Assert.StartsWith(string.Format(Errors.UnsupportedVersion_Args1, "13"), ex.Message);
    }

    /// <summary>
    /// Ensure slnx solution fails to load if an invalid version is found.
    /// </summary>
    [Fact]
    public async Task InvalidSlnxVersionAsync()
    {
        ResourceStream version = SlnAssets.LoadResource("Invalid/VersionInvalid.slnx");
        SolutionException ex = await Assert.ThrowsAsync<SolutionException>(
            async () => _ = await SolutionSerializers.SlnXml.OpenAsync(version.Stream, CancellationToken.None));
        Assert.StartsWith(string.Format(Errors.InvalidVersion_Args1, "v1.0"), ex.Message);
        Assert.Equal(1, ex.Line);
        Assert.Equal(2, ex.Column);
    }

    /// <summary>
    /// The legacy sln solution parser would ignore duplicate folder guids.
    /// Ensure this behavior is maintained.
    /// </summary>
    [Fact]
    public async Task DuplicateFolderIdSlnAsync()
    {
        ResourceStream duplicateId = SlnAssets.LoadResource("Invalid/DuplicateFolderId.sln");
        SolutionModel solution = await SolutionSerializers.SlnFileV12.OpenAsync(duplicateId.Stream, CancellationToken.None);

        Assert.Equal(4, solution.SolutionFolders.Count);

        Assert.NotNull(solution.SerializerExtension);
        Assert.True(solution.SerializerExtension.Tarnished);
    }

    /// <summary>
    /// Ensure the slnx parser does fail on duplicate folder guids.
    /// </summary>
    [Fact]
    public async Task DuplicateFolderIdSlnxAsync()
    {
        ResourceStream duplicateId = SlnAssets.LoadResource("Invalid/DuplicateFolderId.slnx");
        SolutionException ex = await Assert.ThrowsAsync<SolutionException>(
            async () => _ = await SolutionSerializers.SlnXml.OpenAsync(duplicateId.Stream, CancellationToken.None));

        Assert.StartsWith(string.Format(Errors.DuplicateItemRef_Args2, "11111111-1111-1111-1111-111111111111", nameof(SolutionFolderModel)), ex.Message);
        Assert.Equal(3, ex.Line);
        Assert.Equal(4, ex.Column);
    }

    /// <summary>
    /// Ensure the slnx parser does fail if project id collides with folder id.
    /// Expected behavior is the folder id is changed to a new value.
    /// </summary>
    [Fact]
    public async Task DuplicateItemIdSlnAsync()
    {
        ResourceStream duplicateId = SlnAssets.LoadResource("Invalid/DuplicateItemId.sln");
        SolutionModel solution = await SolutionSerializers.SlnFileV12.OpenAsync(duplicateId.Stream, CancellationToken.None);
        Assert.NotNull(solution.SerializerExtension);
        Assert.True(solution.SerializerExtension.Tarnished);

        SolutionProjectModel? projectModel = solution.FindProject("DuplicateProjectId.csproj");
        Assert.NotNull(projectModel);
        Assert.Equal(projectModel.Id, new Guid("11111111-1111-1111-1111-111111111111"));

        SolutionFolderModel? folderModel = solution.FindFolder("/DuplicateFolderId/");
        Assert.NotNull(folderModel);
        Assert.NotEqual(folderModel.Id, new Guid("11111111-1111-1111-1111-111111111111"));
    }

    /// <summary>
    /// Recreate legacy bug behavior when duplicate project id guids are found in .sln files.
    /// Use corrupt configuration and change second project id to new value.
    /// </summary>
    [Fact]
    public async Task DuplicateProjectIdSlnAsync()
    {
        ResourceStream duplicateId = SlnAssets.LoadResource("Invalid/DuplicateProjectId.sln");
        SolutionModel solution = await SolutionSerializers.SlnFileV12.OpenAsync(duplicateId.Stream, CancellationToken.None);
        Assert.NotNull(solution.SerializerExtension);
        Assert.True(solution.SerializerExtension.Tarnished);

        SolutionProjectModel? project = solution.FindProject("DuplicateProjectId.csproj");
        Assert.NotNull(project);
        Assert.Equal(project.Id, new Guid("8BADBEEF-1111-2222-3333-444444444444"));

        SolutionProjectModel? project2 = solution.FindProject("DuplicateProjectIdOpposite.csproj");
        Assert.NotNull(project2);
        Assert.NotEqual(project2.Id, new Guid("8BADBEEF-2222-3333-4444-555555555555"));

        (string? buildType, string? platform, bool build, bool deploy) = project.GetProjectConfiguration(BuildTypeNames.Debug, PlatformNames.AnySpaceCPU);

        (string? buildType2, string? platform2, bool build2, bool deploy2) = project2.GetProjectConfiguration(BuildTypeNames.Debug, PlatformNames.AnySpaceCPU);

        // The configuration from the "opposite" project should be used by both as it appears last in the file.
        Assert.Equal(BuildTypeNames.Release, buildType);
        Assert.Equal(PlatformNames.AnySpaceCPU, platform);
        Assert.True(build);
        Assert.False(deploy);

        // The configuration from the "opposite" project should be used by both as it appears last in the file.
        Assert.Equal(BuildTypeNames.Release, buildType2);
        Assert.Equal(PlatformNames.AnySpaceCPU, platform2);
        Assert.True(build2);
        Assert.False(deploy2);

        // Compare the new model to the 'After' sln (Change the new Guid to FFFF to match).
        project2.Id = new Guid("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");
        ResourceStream duplicateIdAfter = SlnAssets.LoadResource("DuplicateProjectId-After.sln");
        AssertSolutionsAreEqual(duplicateIdAfter.ToLines(), await solution.ToLinesAsync(SolutionSerializers.SlnFileV12));
    }

    /// <summary>
    /// Ensure the slnx parser does fail on duplicate project guids.
    /// </summary>
    [Fact]
    public async Task DuplicateProjectIdSlnxAsync()
    {
        ResourceStream duplicateId = SlnAssets.LoadResource("Invalid/DuplicateProjectId.slnx");
        SolutionException ex = await Assert.ThrowsAsync<SolutionException>(
            async () => _ = await SolutionSerializers.SlnXml.OpenAsync(duplicateId.Stream, CancellationToken.None));

        Assert.StartsWith(string.Format(Errors.DuplicateItemRef_Args2, "8badbeef-1111-2222-3333-444444444444", nameof(SolutionProjectModel)), ex.Message);
        Assert.Equal(5, ex.Line);
        Assert.Equal(6, ex.Column);
    }

    /// <summary>
    /// Verifies that a malformed project type id is loaded as an empty guid without errors, and
    /// the solution is marked as tarnished.
    /// </summary>
    [Fact]
    public async Task InvalidProjectTypeSlnAsync()
    {
        ResourceStream invalidProjectType = SlnAssets.LoadResource("Invalid/InvalidProjectType.sln");
        SolutionModel solution = await SolutionSerializers.SlnFileV12.OpenAsync(invalidProjectType.Stream, CancellationToken.None);
        Assert.NotNull(solution.SerializerExtension);
        Assert.True(solution.SerializerExtension.Tarnished);

        SolutionProjectModel? project = solution.FindProject("InvalidProjectType.csproj");
        Assert.NotNull(project);
        Assert.Equal(Guid.Empty, project.TypeId);
    }
}
