// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Serialization;

/// <summary>
/// These tests validate SLN files can be round-tripped through the serializer and model.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "All tests in this class have the same purpose.")]
public class RoundTripClassicSln
{
    public static TheoryData<ResourceName> ClassicSlnFiles =>
        new TheoryData<ResourceName>(SlnAssets.ClassicSlnFiles);

    [Fact]
    public Task BlankAsync() => TestRoundTripSerializerAsync(SlnAssets.ClassicSlnBlank);

    [Fact]
    public Task CpsAsync() => TestRoundTripSerializerAsync(SlnAssets.ClassicSlnCps);

    [Fact]
    public Task EverythingAsync() => TestRoundTripSerializerAsync(SlnAssets.ClassicSlnEverything);

    [Fact]
    public Task ManyAsync() => TestRoundTripSerializerAsync(SlnAssets.ClassicSlnMany);

    [Fact]
    public Task OrchardCoreAsync() => TestRoundTripSerializerAsync(SlnAssets.ClassicSlnOrchardCore);

    [Fact]
    public Task SingleNativeProjectAsync() => TestRoundTripSerializerAsync(SlnAssets.ClassicSlnSingleNativeProject);

    [Fact]
    public Task GiantAsync() => TestRoundTripSerializerAsync(SlnAssets.ClassicSlnGiant);

    [Fact]
    public Task TraditionalAsync() => TestRoundTripSerializerAsync(SlnAssets.ClassicSlnTraditional);

    [Fact]
    public Task MissingConfigurationsAsync() => TestRoundTripSerializerAsync(SlnAssets.ClassicSlnMissingConfigurations);

    [Fact]
    public Task FolderIdAsync() => TestRoundTripSerializerAsync(SlnAssets.LoadResource("FolderId.sln"));

    [Theory]
    [MemberData(nameof(ClassicSlnFiles))]
    public Task AllClassicSolutionAsync(ResourceName sampleFile)
    {
        return TestRoundTripSerializerAsync(sampleFile.Load());
    }

    /// <summary>
    /// Round trip a .SLN file through the serializer and model.
    /// </summary>
    /// <param name="slnStream">The .SLN to test.</param>
    /// <returns>Task to track the asynchronous call status.</returns>
    private static async Task TestRoundTripSerializerAsync(ResourceStream slnStream)
    {
        if (IsMono)
        {
            // Mono is not supported.
            return;
        }

        FileContents originalSolution = slnStream.ToLines();

        // Open the Model from stream.
        SolutionModel model = await SolutionSerializers.SlnFileV12.OpenAsync(slnStream.Stream, CancellationToken.None);
        AssertNotTarnished(model);

        // Save the Model back to stream.
        FileContents reserializedSolution = await model.ToLinesAsync(SolutionSerializers.SlnFileV12);

        AssertSolutionsAreEqual(originalSolution, reserializedSolution);
    }
}
