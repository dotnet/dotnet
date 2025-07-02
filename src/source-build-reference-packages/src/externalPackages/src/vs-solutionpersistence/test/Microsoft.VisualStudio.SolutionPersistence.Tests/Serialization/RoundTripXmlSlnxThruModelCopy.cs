// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml;
using Xunit.Sdk;

namespace Serialization;

/// <summary>
/// These tests validate SLNX files can be round-tripped through the serializer and model.
/// These remove any user comments and whitespace from the original model.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "All tests in this class have the same purpose.")]
public class RoundTripXmlSlnxThruModelCopy
{
    [Fact]
    public Task CommentsAsync()
    {
        if (IsMono)
        {
            // Mono is not supported.
            return Task.CompletedTask;
        }

        return Assert.ThrowsAsync<FailException>(() => TestRoundTripSerializerAsync(SlnAssets.XmlSlnxComments));
    }

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
    public Task MissingConfigurationsAsync() => TestRoundTripSerializerAsync(SlnAssets.XmlSlnxMissingConfigurations);

    [Fact]
    public Task TraditionalAsync() => TestRoundTripSerializerAsync(SlnAssets.XmlSlnxTraditional);

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

        Assert.True(model.TryGetSettings(out SlnxSerializerSettings originalSettings));

        // Make a copy of the model.
        model = new SolutionModel(model)
        {
            // Strip off any comments or whitespace from the original model, but keep the indentation the same.
            SerializerExtension = SolutionSerializers.SlnXml.CreateModelExtension(
                new SlnxSerializerSettings(originalSettings)
                {
                    PreserveWhitespace = false,
                }),
        };

        // Save the Model back to stream.
        FileContents reserializedSolution = await model.ToLinesAsync(SolutionSerializers.SlnXml);

        AssertSolutionsAreEqual(slnStream.ToLines(), reserializedSolution);
    }
}
