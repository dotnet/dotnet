// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace Microsoft.DotNet.Arcade.Sdk.Tests;

public class LocateDotNetTests
{
    [Theory]
    [InlineData("""{ "tools": { "dotnet": "1.0.100" } }""", "1.0.100")]
    [InlineData("""{ "sdk": { "version": "2.0.100" }, "tools": { "dotnet": "1.0.100" } }""", "1.0.100")]
    [InlineData("""{ "sdk": { "version": "2.0.100" }, "tools": { "vs": { } } }""", "2.0.100")]
    [InlineData("""
        {
          // comments and trailing commas are tolerated by the parser
          "sdk": { "errorMessage": "Install SDK {2.0.100}.", "version": "2.0.100-preview.1", },
          "msbuild-sdks": { "Microsoft.DotNet.Arcade.Sdk": "1.0.0" },
        }
        """, "2.0.100-preview.1")]
    public void TryGetDotNetSdkVersionReturnsVersion(string content, string expectedVersion)
    {
        Assert.True(LocateDotNet.TryGetDotNetSdkVersion(content, "global.json", out string version, out string error));
        Assert.Equal(expectedVersion, version);
        Assert.Null(error);
    }

    [Theory]
    [InlineData("""{ "sdk": { "rollForward": "latestFeature" }, "tools": { "vs": { } } }""", "Unable to determine dotnet version")]
    [InlineData("""{ "tools": """, "Unable to parse file")]
    public void TryGetDotNetSdkVersionReportsError(string content, string expectedError)
    {
        Assert.False(LocateDotNet.TryGetDotNetSdkVersion(content, "global.json", out string version, out string error));
        Assert.Null(version);
        Assert.Contains(expectedError, error);
    }
}
