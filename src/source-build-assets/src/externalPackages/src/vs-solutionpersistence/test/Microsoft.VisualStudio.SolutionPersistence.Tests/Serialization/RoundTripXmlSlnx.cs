// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Serialization;

/// <summary>
/// These tests validate SLNX files can be round-tripped through the serializer and model.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "All tests in this class have the same purpose.")]
public class RoundTripXmlSlnx
{
    public static TheoryData<ResourceName> XmlSlnxFiles =>
        new TheoryData<ResourceName>(SlnAssets.XmlSlnxFiles);

    [Fact]
    public Task CommentsAsync() => TestRoundTripSerializerAsync(SlnAssets.XmlSlnxComments);

    [Fact]
    public Task LegacyValuesAsync() => TestRoundTripSerializerAsync(SlnAssets.XmlSlnxLegacyValuesNoObsolete);

    [Fact]
    public Task BlankAsync() => TestRoundTripSerializerAsync(SlnAssets.XmlSlnxBlank);

    [Fact]
    public Task CpsAsync() => TestRoundTripSerializerAsync(SlnAssets.XmlSlnxCps);

    [Fact]
    public Task EverythingAsync() => TestRoundTripSerializerAsync(SlnAssets.XmlSlnxEverything);

    [Fact]
    public Task OrchardCoreAsync() => TestRoundTripSerializerAsync(SlnAssets.XmlSlnxOrchardCore);

    [Fact]
    public Task SingleNativeProjectAsync() => TestRoundTripSerializerAsync(SlnAssets.XmlSlnxSingleNativeProject);

    [Fact]
    public Task BuiltInProjectTypesAsync() => TestRoundTripSerializerAsync(SlnAssets.XmlBuiltInProjectTypes);

    [Fact]
    public Task GiantAsync() => TestRoundTripSerializerAsync(SlnAssets.XmlSlnxGiant);

    [Fact]
    public Task TraditionalAsync() => TestRoundTripSerializerAsync(SlnAssets.XmlSlnxTraditional);

    [Fact]
    public Task MissingConfigurationsAsync() => TestRoundTripSerializerAsync(SlnAssets.XmlSlnxMissingConfigurations);

    [Fact]
    public Task VersionAsync() => TestRoundTripSerializerAsync(SlnAssets.XmlSlnxVersion);

    [Fact]
    public Task VersionMinAsync() => TestRoundTripSerializerAsync(SlnAssets.XmlSlnxVersionMin);

    [Theory]
    [MemberData(nameof(XmlSlnxFiles))]
    public Task AllXmlSolutionAsync(ResourceName sampleFile)
    {
        return TestRoundTripSerializerAsync(sampleFile.Load());
    }

    private static async Task TestRoundTripSerializerAsync(ResourceStream slnStream)
    {
        if (IsMono)
        {
            // Mono is not supported.
            return;
        }

        // Open the Model from stream.
        SolutionModel model = await SolutionSerializers.SlnXml.OpenAsync(slnStream.Stream, CancellationToken.None);
        AssertNotTarnished(model);

        // Save the Model back to stream.
        FileContents reserializedSolution = await model.ToLinesAsync(SolutionSerializers.SlnXml);

        AssertSolutionsAreEqual(slnStream.ToLines(), reserializedSolution);
    }
}
