// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class DefaultRazorConfigurationServiceTest : LanguageServerTestBase
{
    public DefaultRazorConfigurationServiceTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task GetLatestOptionsAsync_ReturnsExpectedOptions()
    {
        // Arrange
        var expectedOptions = new RazorLSPOptions(
            Trace.Messages, enableFormatting: false, autoClosingTags: false, insertSpaces: true, tabSize: 8);
        var razorJsonString = @"
{
  ""trace"": ""Messages"",
  ""format"": {
    ""enable"": ""false""
  }
}
".Trim();
        var htmlJsonString = @"
{
  ""format"": ""true"",
  ""autoClosingTags"": ""false""
}
".Trim();
        var vsEditorJsonString = @"
{
  ""IndentSize"": 8,
  ""IndentWithTabs"": ""false""
}
".Trim();

        var result = new JObject[] { JObject.Parse(razorJsonString), JObject.Parse(htmlJsonString), JObject.Parse(vsEditorJsonString) };
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
        var languageServer = GetLanguageServer<JObject[]>(result: null);
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
        var languageServer = GetLanguageServer<JObject[]>(result: null, shouldThrow: true);
        var configurationService = new DefaultRazorConfigurationService(languageServer, LoggerFactory);

        // Act
        var options = await configurationService.GetLatestOptionsAsync(DisposalToken);

        // Assert
        Assert.Null(options);
    }

    [Fact]
    public void BuildOptions_ReturnsExpectedOptions()
    {
        // Arrange - purposely choosing options opposite of default
        var expectedOptions = new RazorLSPOptions(
            Trace.Verbose, enableFormatting: false, autoClosingTags: false, insertSpaces: false, tabSize: 8);
        var razorJsonString = @"
{
  ""trace"": ""Verbose"",
  ""format"": {
    ""enable"": ""false""
  }
}
".Trim();
        var htmlJsonString = @"
{
  ""format"": ""true"",
  ""autoClosingTags"": ""false""
}
".Trim();
        var vsEditorJsonString = @"
{
  ""IndentSize"": 8,
  ""IndentWithTabs"": ""true""
}
".Trim();

        // Act
        var result = new JObject[] { JObject.Parse(razorJsonString), JObject.Parse(htmlJsonString), JObject.Parse(vsEditorJsonString) };
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
        var defaultOptions = RazorLSPOptions.Default;
        var expectedOptions = new RazorLSPOptions(
            defaultOptions.Trace, defaultOptions.EnableFormatting, defaultOptions.AutoClosingTags,
            insertSpaces: false, defaultOptions.TabSize);
        var razorJsonString = @"
{
  ""trace"": 0,
  ""format"": {
    ""enable"": ""fals""
  }
}
".Trim();
        var htmlJsonString = @"
{
  ""format"": """",
}
".Trim();
        var vsEditorJsonString = @"
{
  ""IndentSize"": ""supposedToBeAnInt"",
  ""IndentWithTabs"": 4
}
".Trim();

        // Act
        var result = new JObject[] { JObject.Parse(razorJsonString), JObject.Parse(htmlJsonString), JObject.Parse(vsEditorJsonString) };
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
        var result = new JObject[] { null, null, null };
        var languageServer = GetLanguageServer(result);
        var configurationService = new DefaultRazorConfigurationService(languageServer, LoggerFactory);
        var options = configurationService.BuildOptions(result);

        // Assert
        Assert.Equal(expectedOptions, options);
    }

    private static ClientNotifierServiceBase GetLanguageServer<IResult>(IResult result, bool shouldThrow = false)
    {
        var languageServer = new Mock<ClientNotifierServiceBase>(MockBehavior.Strict);

        if (shouldThrow)
        {
        }
        else
        {
            languageServer
                .Setup(l => l.SendRequestAsync<ConfigurationParams, IResult>("workspace/configuration", It.IsAny<ConfigurationParams>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);
        }

        return languageServer.Object;
    }
}
