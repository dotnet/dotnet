// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization.Json;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Resolvers;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.Serialization;

public class SerializerValidationTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Theory]
    [InlineData("Kendo.Mvc.Examples.taghelpers.json", "Telerik")]
    [InlineData("taghelpers.json")]
    public void VerifyMessagePack_TagHelpers(string resourceName, string? folderName = null)
    {
        // Arrange
        var resourceBytes = RazorTestResources.GetResourceBytes(resourceName, folderName);

        // Read tag helpers from JSON
        var originalTagHelpers = JsonDataConvert.DeserializeTagHelperArray(resourceBytes);

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
    [InlineData("Kendo.Mvc.Examples.taghelpers.json", "Telerik")]
    [InlineData("taghelpers.json")]
    [InlineData("BlazorServerApp.TagHelpers.json")]
    public void VerifyJson_TagHelpers(string resourceName, string? folderName = null)
    {
        // Arrange
        var resourceBytes = RazorTestResources.GetResourceBytes(resourceName, folderName);

        // Read tag helpers from JSON
        var originalTagHelpers = JsonDataConvert.DeserializeTagHelperArray(resourceBytes);

        // Act

        // Serialize to JSON
        var jsonText = JsonDataConvert.Serialize(originalTagHelpers);

        // Deserialize from JSON
        var actualTagHelpers = JsonDataConvert.DeserializeTagHelperArray(jsonText);

        // Assert
        Assert.Equal<TagHelperDescriptor>(originalTagHelpers, actualTagHelpers);
    }
}
