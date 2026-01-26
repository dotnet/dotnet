// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.VisualStudio.Razor.LanguageClient.Cohost;
using Microsoft.VisualStudioCode.RazorExtension.Endpoints;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudioCode.RazorExtension.Test;

public class CohostConfigurationChangedServiceTest(ITestOutputHelper testOutput) : CohostEndpointTestBase(testOutput)
{
    [Fact]
    public void ReadSettings()
    {
        var settings = ClientSettingsManager.GetClientSettings().AdvancedSettings;

        // If the defaults for these settings change, the json parsing could break and we might not know
        Assert.False(settings.CodeBlockBraceOnNextLine);
        Assert.Equal(AttributeIndentStyle.AlignWithFirst, settings.AttributeIndentStyle);
        Assert.True(settings.CommitElementsWithSpace);

        var json = (JsonArray)JsonNode.Parse("""
            ["true", "indentByOne", "false"]
            """).AssumeNotNull();

        var updatedSettings = CohostConfigurationChangedService.TestAccessor.UpdateSettingsFromJson(settings, json);

        Assert.True(updatedSettings.CodeBlockBraceOnNextLine);
        Assert.Equal(AttributeIndentStyle.IndentByOne, updatedSettings.AttributeIndentStyle);
        Assert.False(updatedSettings.CommitElementsWithSpace);
    }
}
