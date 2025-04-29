// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class DefaultRazorConfigurationServiceTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task GetLatestOptionsAsync_ReturnsExpectedOptions()
    {
        // Arrange
        var expectedOptions = new RazorLSPOptions(
            FormattingFlags.Disabled, AutoClosingTags: false, InsertSpaces: true, TabSize: 4, AutoShowCompletion: true, AutoListParams: true, AutoInsertAttributeQuotes: true, ColorBackground: false, CodeBlockBraceOnNextLine: true, CommitElementsWithSpace: false, TaskListDescriptors: []);
        var razorJsonString =
            """

            {
              "format": {
                "enable": false,
                "codeBlockBraceOnNextLine": true
              }
            }

            """;

        var htmlJsonString = """

            {
              "format": true,
              "autoClosingTags": false
            }

            """;

        var vsEditorJsonString = """
            {
            }

            """;

        var result = new JsonObject[] { JsonNode.Parse(razorJsonString).AsObject(), JsonNode.Parse(htmlJsonString).AsObject(), JsonNode.Parse(vsEditorJsonString).AsObject() };
        var languageServer = GetLanguageServer(result);
        var configurationService = new DefaultRazorConfigurationService(languageServer, LoggerFactory);

        // Act
        var options = await configurationService.GetLatestOptionsAsync(DisposalToken);

        // Assert
        Assert.Equal(expectedOptions, options);
    }

    [Fact]
    public async Task GetLatestOptionsAsync_EmptyResponse_ReturnsNull()
    {
        // Arrange
        var languageServer = GetLanguageServer<JsonObject[]>(result: null);
        var configurationService = new DefaultRazorConfigurationService(languageServer, LoggerFactory);

        // Act
        var options = await configurationService.GetLatestOptionsAsync(DisposalToken);

        // Assert
        Assert.Null(options);
    }

    [Fact]
    public async Task GetLatestOptionsAsync_ClientRequestThrows_ReturnsNull()
    {
        // Arrange
        var languageServer = GetLanguageServer<JsonObject[]>(result: null, shouldThrow: true);
        var configurationService = new DefaultRazorConfigurationService(languageServer, LoggerFactory);

        // Act
        var options = await configurationService.GetLatestOptionsAsync(DisposalToken);

        // Assert
        Assert.Null(options);
    }

    [Fact]
    public void BuildOptions_VSCodeOptionsOnly_ReturnsExpected()
    {
        // Arrange - purposely choosing options opposite of default
        var expectedOptions = new RazorLSPOptions(
            FormattingFlags.Disabled, AutoClosingTags: false, InsertSpaces: true, TabSize: 4, AutoShowCompletion: true, AutoListParams: true, AutoInsertAttributeQuotes: true, ColorBackground: false, CodeBlockBraceOnNextLine: true, CommitElementsWithSpace: false, TaskListDescriptors: []);
        var razorJsonString = """
            {
              "format": {
                "enable": false,
                "codeBlockBraceOnNextLine": true
              }
            }

            """;
        var htmlJsonString = """
            {
              "format": true,
              "autoClosingTags": false
            }

            """;
        var vsEditorJsonString = """
            {
            }
            """;

        // Act
        var result = new JsonObject[] { JsonNode.Parse(razorJsonString).AsObject(), JsonNode.Parse(htmlJsonString).AsObject(), JsonNode.Parse(vsEditorJsonString).AsObject() };
        var languageServer = GetLanguageServer(result);
        var configurationService = new DefaultRazorConfigurationService(languageServer, LoggerFactory);
        var options = configurationService.BuildOptions(result);

        // Assert
        Assert.Equal(expectedOptions, options);
    }

    [Fact]
    public void BuildOptions_VSOptionsOnly_ReturnsExpected()
    {
        // Arrange - purposely choosing options opposite of default
        var expectedOptions = new RazorLSPOptions(
            FormattingFlags.Enabled, AutoClosingTags: false, InsertSpaces: false, TabSize: 8, AutoShowCompletion: true, AutoListParams: true, AutoInsertAttributeQuotes: false, ColorBackground: false, CodeBlockBraceOnNextLine: false, CommitElementsWithSpace: false, TaskListDescriptors: []);
        var razorJsonString = """
            {
            }

            """;
        var htmlJsonString = """
            {
            }

            """;
        var vsEditorJsonString = """
            {
                "ClientSpaceSettings": {
                    "IndentSize": 8,
                    "IndentWithTabs": true
                },
                "AdvancedSettings": {
                    "FormatOnType": false,
                    "AutoClosingTags": false,
                    "AutoInsertAttributeQuotes": false,
                    "CommitElementsWithSpace": false
                }
            }
            """;

        // Act
        var result = new JsonObject[] { JsonNode.Parse(razorJsonString).AsObject(), JsonNode.Parse(htmlJsonString).AsObject(), JsonNode.Parse(vsEditorJsonString).AsObject() };
        var languageServer = GetLanguageServer(result);
        var configurationService = new DefaultRazorConfigurationService(languageServer, LoggerFactory);
        var options = configurationService.BuildOptions(result);

        // Assert
        Assert.Equal(expectedOptions, options);
    }

    [Fact]
    public void BuildOptions_MalformedOptions()
    {
        // This test is purely to ensure we don't crash if the user provides malformed options.

        // Arrange
        // The Json blob is seen as a VS Code options set, so we have to use its default
        var expectedOptions = RazorLSPOptions.Default with { CommitElementsWithSpace = false };
        var razorJsonString = @"
{
  ""format"": {
    ""enable"": ""fals""
  }
}
".Trim();
        var htmlJsonString = @"
{
  ""format"": """"
}
".Trim();
        var vsEditorJsonString = @"
{
    ""ClientSpaceSettings"": {
          ""IndentSize"": ""supposedToBeAnInt"",
          ""IndentWithTabs"": 4
    }
}
".Trim();

        // Act
        var result = new JsonObject[] { JsonNode.Parse(razorJsonString).AsObject(), JsonNode.Parse(htmlJsonString).AsObject(), JsonNode.Parse(vsEditorJsonString).AsObject() };
        var languageServer = GetLanguageServer(result);
        var configurationService = new DefaultRazorConfigurationService(languageServer, LoggerFactory);
        var options = configurationService.BuildOptions(result);

        // Assert
        Assert.Equal(expectedOptions, options);
    }

    [Fact]
    public void BuildOptions_NullOptions()
    {
        // Arrange
        var expectedOptions = RazorLSPOptions.Default;

        // Act
        var result = new JsonObject[] { null, null, null };
        var languageServer = GetLanguageServer(result);
        var configurationService = new DefaultRazorConfigurationService(languageServer, LoggerFactory);
        var options = configurationService.BuildOptions(result);

        // Assert
        Assert.Equal(expectedOptions, options);
    }

    private static IClientConnection GetLanguageServer<IResult>(IResult result, bool shouldThrow = false)
    {
        var clientConnection = new Mock<IClientConnection>(MockBehavior.Strict);

        if (shouldThrow)
        {
        }
        else
        {
            clientConnection
                .Setup(l => l.SendRequestAsync<ConfigurationParams, IResult>("workspace/configuration", It.IsAny<ConfigurationParams>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);
        }

        return clientConnection.Object;
    }
}
