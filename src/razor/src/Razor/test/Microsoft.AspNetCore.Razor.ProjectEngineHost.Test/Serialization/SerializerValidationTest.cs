// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Serialization.Json;
using Microsoft.AspNetCore.Razor.Serialization.MessagePack.Resolvers;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Logging;
using Xunit;
using Xunit.Abstractions;
using MessagePackSerializationFormat = Microsoft.AspNetCore.Razor.Serialization.MessagePack.SerializationFormat;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Test.Serialization;

public class SerializerValidationTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Theory]
    [InlineData("Kendo.Mvc.Examples.project.razor.json")]
    [InlineData("project.razor.json")]
    public void VerifyMessagePack_RazorProjectInfo(string resourceName)
    {
        // Arrange
        var resourceBytes = RazorTestResources.GetResourceBytes(resourceName, "Benchmarking");

        // Read tag helpers from JSON
        var originalProjectInfo = DeserializeProjectInfoFromJsonBytes(resourceBytes);

        var options = MessagePackSerializerOptions.Standard
            .WithResolver(CompositeResolver.Create(
                RazorProjectInfoResolver.Instance,
                StandardResolver.Instance));

        // Act
        var bytes = MessagePackConvert.Serialize(originalProjectInfo, options);
        var actualProjectInfo = MessagePackConvert.Deserialize<RazorProjectInfo>(bytes, options);

        // Assert
        Assert.Equal(originalProjectInfo, actualProjectInfo);
    }

    [Theory]
    [InlineData("Kendo.Mvc.Examples.taghelpers.json")]
    [InlineData("taghelpers.json")]
    public void VerifyMessagePack_TagHelpers(string resourceName)
    {
        // Arrange
        var resourceBytes = RazorTestResources.GetResourceBytes(resourceName, "Benchmarking");

        // Read tag helpers from JSON
        var originalTagHelpers = ReadTagHelpersFromJsonBytes(resourceBytes);

        var options = MessagePackSerializerOptions.Standard.WithResolver(
            CompositeResolver.Create(
                FetchTagHelpersResultResolver.Instance,
                StandardResolver.Instance));

        // Act
        var bytes = MessagePackConvert.Serialize(originalTagHelpers, options);
        var actualTagHelpers = MessagePackConvert.Deserialize<ImmutableArray<TagHelperDescriptor>>(bytes, options);

        // Assert
        Assert.Equal<TagHelperDescriptor>(originalTagHelpers, actualTagHelpers);
    }

    [Theory]
    [InlineData("Kendo.Mvc.Examples.project.razor.json")]
    [InlineData("project.razor.json")]
    public void VerifyJson_RazorProjectInfo(string resourceName)
    {
        // Arrange
        var resourceBytes = RazorTestResources.GetResourceBytes(resourceName, "Benchmarking");

        // Read tag helpers from JSON
        var originalProjectInfo = DeserializeProjectInfoFromJsonBytes(resourceBytes);

        // Act

        // Serialize to JSON
        var jsonText = JsonDataConvert.SerializeObject(originalProjectInfo, ObjectWriters.WriteProperties);

        // Deserialize from message pack
        var actualProjectInfo = JsonDataConvert.DeserializeObject(jsonText, ObjectReaders.ReadProjectInfoFromProperties);
        Assert.NotNull(actualProjectInfo);

        // Assert
        Assert.Equal(originalProjectInfo.ProjectKey, actualProjectInfo.ProjectKey);
        Assert.Equal(originalProjectInfo.FilePath, actualProjectInfo.FilePath);
        Assert.Equal(originalProjectInfo.Configuration, actualProjectInfo.Configuration);
        Assert.Equal(originalProjectInfo.RootNamespace, actualProjectInfo.RootNamespace);
        Assert.Equal(originalProjectInfo.ProjectWorkspaceState, actualProjectInfo.ProjectWorkspaceState);
        Assert.Equal<DocumentSnapshotHandle>(originalProjectInfo.Documents, actualProjectInfo.Documents);
    }

    [Theory]
    [InlineData("Kendo.Mvc.Examples.taghelpers.json", "Benchmarking")]
    [InlineData("taghelpers.json", "Benchmarking")]
    [InlineData("BlazorServerApp.TagHelpers.json")]
    public void VerifyJson_TagHelpers(string resourceName, string? folderName = null)
    {
        // Arrange
        var resourceBytes = RazorTestResources.GetResourceBytes(resourceName, folderName);

        // Read tag helpers from JSON
        var originalTagHelpers = ReadTagHelpersFromJsonBytes(resourceBytes);

        // Act

        // Serialize to JSON
        var jsonText = JsonDataConvert.SerializeData(
            dataWriter => dataWriter.WriteArray(originalTagHelpers, ObjectWriters.Write));

        // Deserialize from JSON
        var actualTagHelpers = JsonDataConvert.DeserializeData(jsonText,
            r => r.ReadImmutableArray(
                static r => ObjectReaders.ReadTagHelper(r, useCache: false)));

        // Assert
        Assert.Equal<TagHelperDescriptor>(originalTagHelpers, actualTagHelpers);
    }

    /// <summary>
    /// Verifies that a previous generated file is able to be serialized. If this
    /// fails make sure to do ONE of the following:
    ///
    /// 1. Update the deserializer to be lenient on the previous version
    /// 2. Bump the Microsoft.AspNetCore.Razor.Serialization.MessagePack.SerializationFormat.Version
    ///     a. Generate a new file to replace project.razor.bin with the new format. This can be achieved by calling GenerateProjectRazorBin below.
    ///     b. This will require a dual insertion with Roslyn do use the correct version number
    /// </summary>
    [Theory]
    [InlineData("project.razor.bin")]
    public void VerifyMessagePack_DeserializeMessagePack(string resourceName)
    {
        // Arrange
        var resourceBytes = RazorTestResources.GetResourceBytes(resourceName, "Benchmarking");
        var options = MessagePackSerializerOptions.Standard
            .WithResolver(CompositeResolver.Create(
                RazorProjectInfoResolver.Instance,
                StandardResolver.Instance));

        // Act
        RazorProjectInfo actualProjectInfo;
        try
        {
            actualProjectInfo = MessagePackConvert.Deserialize<RazorProjectInfo>(resourceBytes, options);
        }
        catch (MessagePackSerializationException ex) when (ex.InnerException is RazorProjectInfoSerializationException)
        {
            Assert.Fail($"{typeof(MessagePackSerializationFormat).FullName}.{nameof(MessagePackSerializationFormat.Version)} has changed. Re-generate the project.razor.bin for this test.");
            return;
        }

        // Assert
        Assert.NotNull(actualProjectInfo);
        Assert.NotNull(actualProjectInfo.DisplayName);
    }

#pragma warning disable IDE0051 // Remove unused private members
    private void GenerateProjectRazorBin()
    {
        var resourceBytes = RazorTestResources.GetResourceBytes("project.razor.json", "Benchmarking");
        var projectInfo = DeserializeProjectInfoFromJsonBytes(resourceBytes);

        var options = MessagePackSerializerOptions.Standard
            .WithResolver(CompositeResolver.Create(
                RazorProjectInfoResolver.Instance,
                StandardResolver.Instance));

        var filePath = Assembly.GetExecutingAssembly().Location;
        var binFilePath = Path.Combine(Path.GetDirectoryName(filePath).AssumeNotNull(), "project.razor.bin");

        using (var stream = File.OpenWrite(binFilePath))
        {
            MessagePackSerializer.Serialize(stream, projectInfo, options);
        }

        Logger.LogInformation($"New project.razor.bin written to '{binFilePath}'");
    }
#pragma warning restore IDE0051 // Remove unused private members

    private static RazorProjectInfo DeserializeProjectInfoFromJsonBytes(byte[] resourceBytes)
    {
        using var stream = new MemoryStream(resourceBytes);
        using var streamReader = new StreamReader(stream);

        var originalProjectInfo = JsonDataConvert.DeserializeObject(streamReader, ObjectReaders.ReadProjectInfoFromProperties);
        Assert.NotNull(originalProjectInfo);

        return originalProjectInfo;
    }

    private static ImmutableArray<TagHelperDescriptor> ReadTagHelpersFromJsonBytes(byte[] resourceBytes)
    {
        using var stream = new MemoryStream(resourceBytes);
        using var streamReader = new StreamReader(stream);

        return JsonDataConvert.DeserializeData(streamReader,
            static r => r.ReadImmutableArray(
                static r => ObjectReaders.ReadTagHelper(r, useCache: false)));
    }
}
