// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class DefaultWorkspaceDirectoryPathResolverTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public async Task Resolve_RootUriUnavailable_UsesRootPath()
    {
        // Arrange
        var expectedWorkspaceDirectory = "/testpath";
#pragma warning disable CS0618 // Type or member is obsolete
        var initializeParams = new InitializeParams()
        {
            RootPath = expectedWorkspaceDirectory
        };
#pragma warning restore CS0618 // Type or member is obsolete

        var capabilitiesManager = new CapabilitiesManager(LspServices.Empty);
        capabilitiesManager.SetInitializeParams(initializeParams);

        // Act
        var workspaceDirectoryPath = await capabilitiesManager.GetRootPathAsync(DisposalToken);

        // Assert
        Assert.Equal(expectedWorkspaceDirectory, workspaceDirectoryPath);
    }

    [Fact]
    public async Task Resolve_RootUriPrefered()
    {
        // Arrange
        var initialWorkspaceDirectory = "C:\\testpath";

#pragma warning disable CS0618 // Type or member is obsolete
        var initializeParams = new InitializeParams()
        {
            RootPath = "/somethingelse",
            RootUri = LspFactory.CreateFilePathUri(initialWorkspaceDirectory),
        };
#pragma warning restore CS0618 // Type or member is obsolete

        var capabilitiesManager = new CapabilitiesManager(LspServices.Empty);
        capabilitiesManager.SetInitializeParams(initializeParams);

        // Act
        var workspaceDirectoryPath = await capabilitiesManager.GetRootPathAsync(DisposalToken);

        // Assert
        var expectedWorkspaceDirectory = "C:/testpath";
        Assert.Equal(expectedWorkspaceDirectory, workspaceDirectoryPath);
    }
}
