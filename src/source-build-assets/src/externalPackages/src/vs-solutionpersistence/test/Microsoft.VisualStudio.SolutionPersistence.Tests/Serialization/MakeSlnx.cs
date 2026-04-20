// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Serialization;

/// <summary>
/// This test generates files from the .sln tests assets included in the project.
/// It outputs the reset to the temp directory <see cref="OutputDirectory"/>.
/// These can be used to syncronize converted test assets for any changes.
/// </summary>
/// <param name="fixture">Fixture to ensure a temp directory is created for all the tests.</param>
public sealed partial class MakeSlnx(MakeSlnx.MakeSlnxFixture fixture) : IClassFixture<MakeSlnx.MakeSlnxFixture>
{
    /// <summary>
    /// Gets the classic SLN files to convert to SLNX.
    /// </summary>
    public static TheoryData<ResourceName> ClassicSlnFiles =>
        new TheoryData<ResourceName>(SlnAssets.ClassicSlnFiles);

    /// <summary>
    /// Gets the SLNX files to convert to SLN.
    /// </summary>
    public static TheoryData<ResourceName> XmlSlnxFiles =>
        new TheoryData<ResourceName>(SlnAssets.XmlSlnxFiles);

    /// <summary>
    /// Converts all the sample SLN files into SLNX and puts them in the temp <see cref="OutputDirectory"/> directory.
    /// </summary>
    /// <param name="slnFileName">The file to convert.</param>
    [Theory]
    [MemberData(nameof(ClassicSlnFiles))]
    [Trait("TestCategory", "FailsInCloudTest")]
    public async Task ConvertSlnToSlnxAsync(ResourceName slnFileName)
    {
        if (IsMono)
        {
            // Mono is not supported.
            return;
        }

        ResourceStream slnFile = slnFileName.Load();
        int sampleFileSize = checked((int)slnFile.Stream.Length);
        string slnToSlnxFile = Path.ChangeExtension(Path.Join(fixture.SlnToSlnxDirectory, slnFile.Name), SolutionSerializers.SlnXml.DefaultFileExtension);
        string slnViaSlnxFile = Path.Join(fixture.SlnViaSlnxDirectory, slnFile.Name);

        SolutionModel model = await SolutionSerializers.SlnFileV12.OpenAsync(slnFile.Stream, CancellationToken.None);

        // Original sln converted back to sln via slnx file
        (SolutionModel slnxModel, _) = await SlnTestHelper.ThruSlnxStreamAsync(model, sampleFileSize * 10);
        await SolutionSerializers.SlnFileV12.SaveAsync(slnViaSlnxFile, slnxModel, CancellationToken.None);
        RenameSolutionFile(slnViaSlnxFile);

        // Original sln converted to slnx
        model.TrimVisualStudioProperties();
        await SolutionSerializers.SlnXml.SaveAsync(slnToSlnxFile, model, CancellationToken.None);
        RenameSolutionFile(slnToSlnxFile);
    }

    /// <summary>
    /// Converts all the sample SLNX files into SLN and puts them in the temp <see cref="OutputDirectory"/> directory.
    /// </summary>
    /// <param name="slnxFileName">The slnx file to conver.</param>
    [Theory]
    [MemberData(nameof(XmlSlnxFiles))]
    [Trait("TestCategory", "FailsInCloudTest")]
    public async Task ConvertSlnxToSlnAsync(ResourceName slnxFileName)
    {
        if (IsMono)
        {
            // Mono is not supported.
            return;
        }

        ResourceStream slnxFile = slnxFileName.Load();

        string slnxToSlnFile = Path.ChangeExtension(Path.Join(fixture.SlnxToSlnDirectory, slnxFile.Name), SolutionSerializers.SlnFileV12.DefaultFileExtension);

        SolutionModel model = await SolutionSerializers.SlnXml.OpenAsync(slnxFile.Stream, CancellationToken.None);

        // Convert slnx to sln
        await SolutionSerializers.SlnFileV12.SaveAsync(slnxToSlnFile, model, CancellationToken.None);
        RenameSolutionFile(slnxToSlnFile);
    }

    // Make it easy to update open and update samples.
    private static void RenameSolutionFile(string path)
    {
        StringSpan fileName = Path.GetFileNameWithoutExtension(path.AsSpan());
        StringSpan extension = Path.GetExtension(path.AsSpan());

        // If the file name contains a '.' then we need to create a directory structure.
        string newPath = fileName.Contains('.') ?
            Path.Join(Path.GetDirectoryName(path), fileName.ToString().Replace('.', Path.DirectorySeparatorChar) + extension.ToString()) :
            path;

        // Add common extensions to the file name to avoid opening test assets as solutions.
        switch (extension)
        {
            case ".sln":
                newPath += ".txt";
                break;
            case ".slnx":
                newPath += ".xml";
                break;
        }

        // Ensure the new directory exists.
        string? directory = Path.GetDirectoryName(newPath);
        if (!directory.IsNullOrEmpty() && !Directory.Exists(directory))
        {
            _ = Directory.CreateDirectory(directory);
        }

        File.Move(path, newPath);
    }
}
