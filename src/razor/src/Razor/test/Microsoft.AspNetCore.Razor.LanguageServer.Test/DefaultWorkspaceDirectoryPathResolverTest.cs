// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;
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

        var capabilitiesManager = new CapabilitiesManager(StrictMock.Of<ILspServices>());
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
            RootUri = VsLspFactory.CreateFilePathUri(initialWorkspaceDirectory),
        };
#pragma warning restore CS0618 // Type or member is obsolete

        var capabilitiesManager = new CapabilitiesManager(StrictMock.Of<ILspServices>());
        capabilitiesManager.SetInitializeParams(initializeParams);

        // Act
        var workspaceDirectoryPath = await capabilitiesManager.GetRootPathAsync(DisposalToken);

        // Assert
        var expectedWorkspaceDirectory = "C:/testpath";
        Assert.Equal(expectedWorkspaceDirectory, workspaceDirectoryPath);
    }
}
