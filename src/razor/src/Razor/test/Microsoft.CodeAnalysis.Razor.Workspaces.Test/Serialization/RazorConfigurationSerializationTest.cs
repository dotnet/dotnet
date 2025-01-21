// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

public class RazorConfigurationSerializationTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public void RazorConfigurationJsonConverter_Serialization_CanRoundTrip()
    {
        // Arrange
        var configuration = new RazorConfiguration(
            RazorLanguageVersion.Version_1_1,
            "Test",
            [new("Test-Extension1"), new("Test-Extension2")],
            CodeAnalysis.CSharp.LanguageVersion.CSharp7,
            UseConsolidatedMvcViews: false,
            SuppressAddComponentParameter: true,
            UseRoslynTokenizer: true,
            PreprocessorSymbols: ["DEBUG", "TRACE", "DAVID"]);

        // Act
        var json = JsonDataConvert.Serialize(configuration);
        Assert.NotNull(json);

        var obj = JsonDataConvert.DeserializeConfiguration(json);
        Assert.NotNull(obj);

        // Assert
        Assert.Equal(configuration.ConfigurationName, obj.ConfigurationName);
        Assert.Collection(
            configuration.Extensions,
            e => Assert.Equal("Test-Extension1", e.ExtensionName),
            e => Assert.Equal("Test-Extension2", e.ExtensionName));
        Assert.Equal(configuration.LanguageVersion, obj.LanguageVersion);
        Assert.Equal(configuration.CSharpLanguageVersion, obj.CSharpLanguageVersion);
        Assert.Equal(configuration.UseConsolidatedMvcViews, obj.UseConsolidatedMvcViews);
        Assert.Equal(configuration.SuppressAddComponentParameter, obj.SuppressAddComponentParameter);
        Assert.Equal(configuration.UseRoslynTokenizer, obj.UseRoslynTokenizer);
        Assert.Collection(obj.PreprocessorSymbols,
            s => Assert.Equal("DEBUG", s),
            s => Assert.Equal("TRACE", s),
            s => Assert.Equal("DAVID", s));
    }

    [Fact]
    public void RazorConfigurationJsonConverter_Serialization_MVC3_CanRead()
    {
        // Arrange
        var configurationJson = """
            {
              "ConfigurationName": "MVC-3.0",
              "LanguageVersion": "3.0",
              "Extensions": ["MVC-3.0"]
            }
            """;

        // Act
        var obj = JsonDataConvert.DeserializeConfiguration(configurationJson);
        Assert.NotNull(obj);

        // Assert
        Assert.Equal("MVC-3.0", obj.ConfigurationName);
        var extension = Assert.Single(obj.Extensions);
        Assert.Equal("MVC-3.0", extension.ExtensionName);
        Assert.Equal(RazorLanguageVersion.Parse("3.0"), obj.LanguageVersion);
    }

    [Fact]
    public void RazorConfigurationJsonConverter_Serialization_MVC2_CanRead()
    {
        // Arrange
        var configurationJson = """
            {
              "ConfigurationName": "MVC-2.1",
              "LanguageVersion": "2.1",
              "Extensions": ["MVC-2.1"]
            }
            """;

        // Act
        var obj = JsonDataConvert.DeserializeConfiguration(configurationJson);
        Assert.NotNull(obj);

        // Assert
        Assert.Equal("MVC-2.1", obj.ConfigurationName);
        var extension = Assert.Single(obj.Extensions);
        Assert.Equal("MVC-2.1", extension.ExtensionName);
        Assert.Equal(RazorLanguageVersion.Parse("2.1"), obj.LanguageVersion);
    }

    [Fact]
    public void RazorConfigurationJsonConverter_Serialization_MVC1_CanRead()
    {
        // Arrange
        var configurationJson = """
            {
              "ConfigurationName": "MVC-1.1",
              "LanguageVersion": "1.1",
              "Extensions": ["MVC-1.1"]
            }
            """;

        // Act
        var obj = JsonDataConvert.DeserializeConfiguration(configurationJson);
        Assert.NotNull(obj);

        // Assert
        Assert.Equal("MVC-1.1", obj.ConfigurationName);
        var extension = Assert.Single(obj.Extensions);
        Assert.Equal("MVC-1.1", extension.ExtensionName);
        Assert.Equal(RazorLanguageVersion.Parse("1.1"), obj.LanguageVersion);
    }
}
