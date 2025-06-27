// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class RemoteRazorProjectFileSystemTest : ToolingTestBase
{
    public RemoteRazorProjectFileSystemTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void GetItem_RootlessFilePath()
    {
        // Arrange
        var fileSystem = new RemoteRazorProjectFileSystem("C:/path/to");
        var documentFilePath = "file.cshtml";

        // Act
        var item = fileSystem.GetItem(documentFilePath, fileKind: null);

        // Assert
        Assert.Equal(documentFilePath, item.FilePath);
        Assert.Equal("C:/path/to/file.cshtml", item.PhysicalPath);
    }

    [Fact]
    public void GetItem_RootedFilePath_BelongsToProject()
    {
        // Arrange
        var fileSystem = new RemoteRazorProjectFileSystem("C:/path/to");
        var documentFilePath = "C:/path/to/file.cshtml";

        // Act
        var item = fileSystem.GetItem(documentFilePath, fileKind: null);

        // Assert
        Assert.Equal("file.cshtml", item.FilePath);
        Assert.Equal(documentFilePath, item.PhysicalPath);
    }

    [Fact]
    public void GetItem_RootedFilePath_DoesNotBelongToProject()
    {
        // Arrange
        RemoteRazorProjectFileSystem fileSystem;
        string documentFilePath;

        if (PlatformInformation.IsWindows)
        {
            fileSystem = new RemoteRazorProjectFileSystem(@"C:\path\to");
            documentFilePath = @"C:\otherpath\to\file.cshtml";
        }
        else
        {
            fileSystem = new RemoteRazorProjectFileSystem("/path/to");
            documentFilePath = "/otherpath/to/file.cshtml";
        }

        // Act
        var item = fileSystem.GetItem(documentFilePath, fileKind: null);

        // Assert
        Assert.Equal(documentFilePath, item.FilePath);
        Assert.Equal(documentFilePath, item.PhysicalPath);
    }
}
