// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization.Json;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.Serialization;

public class JsonSerializationTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    private readonly RazorConfiguration _configuration = new(RazorLanguageVersion.Experimental, ConfigurationName: "Custom", [new("TestExtension")]);

    [Fact]
    public void RazorConfiguration_CanRoundTrip()
    {
        // Arrange
        var jsonText = JsonDataConvert.Serialize(_configuration);
        Assert.NotNull(jsonText);

        // Act
        var deserializedConfiguration = JsonDataConvert.DeserializeConfiguration(jsonText);

        // Assert
        Assert.Equal(_configuration, deserializedConfiguration);
    }
}
