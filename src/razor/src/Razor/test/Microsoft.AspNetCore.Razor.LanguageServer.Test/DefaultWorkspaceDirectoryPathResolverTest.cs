// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class DefaultWorkspaceDirectoryPathResolverTest : TestBase
{
    public DefaultWorkspaceDirectoryPathResolverTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void Resolve_RootUriUnavailable_UsesRootPath()
    {
        // Arrange
        var expectedWorkspaceDirectory = "/testpath";
#pragma warning disable CS0618 // Type or member is obsolete
        var clientSettings = new InitializeParams()
        {
            RootPath = expectedWorkspaceDirectory
        };
#pragma warning restore CS0618 // Type or member is obsolete
        var server = new Mock<IInitializeManager<InitializeParams, InitializeResult>>(MockBehavior.Strict);
        server.Setup(m => m.GetInitializeParams()).Returns(clientSettings);
        var workspaceDirectoryPathResolver = new DefaultWorkspaceDirectoryPathResolver(server.Object);

        // Act
        var workspaceDirectoryPath = workspaceDirectoryPathResolver.Resolve();

        // Assert
        Assert.Equal(expectedWorkspaceDirectory, workspaceDirectoryPath);
    }

    [Fact]
    public void Resolve_RootUriPrefered()
    {
        // Arrange
        var initialWorkspaceDirectory = "C:\\testpath";
        var uriBuilder = new UriBuilder
        {
            Scheme = "file",
            Host = null,
            Path = initialWorkspaceDirectory,
        };
#pragma warning disable CS0618 // Type or member is obsolete
        var clientSettings = new InitializeParams()
        {
            RootPath = "/somethingelse",
            RootUri = uriBuilder.Uri,
        };
#pragma warning restore CS0618 // Type or member is obsolete
        var server = new Mock<IInitializeManager<InitializeParams, InitializeResult>>(MockBehavior.Strict);
        server.Setup(s => s.GetInitializeParams()).Returns(clientSettings);
        var workspaceDirectoryPathResolver = new DefaultWorkspaceDirectoryPathResolver(server.Object);

        // Act
        var workspaceDirectoryPath = workspaceDirectoryPathResolver.Resolve();

        // Assert
        var expectedWorkspaceDirectory = "C:/testpath";
        Assert.Equal(expectedWorkspaceDirectory, workspaceDirectoryPath);
    }
}
