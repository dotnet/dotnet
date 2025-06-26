// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Test;

public class FilePathServiceTest
{
    [Theory]
    [InlineData(@"C:\path\to\file.razor.t3Gf1FBjln6S9T95.ide.g.cs")]
    [InlineData(@"C:\path\to\file.razor__virtual.html")]
    [InlineData(@"C:\path\to\file.razor")]
    public void GetRazorFilePath_ReturnsExpectedPath(string inputFilePath)
    {
        // Arrange
        var filePathService = new TestFilePathService(new TestLanguageServerFeatureOptions(includeProjectKeyInGeneratedFilePath: true));

        // Act
        var result = filePathService.GetTestAccessor().GetRazorFilePath(inputFilePath);

        // Assert
        Assert.Equal(@"C:\path\to\file.razor", result);
    }

    [Theory]
    [InlineData(true, @"C:\path\to\file.razor.21z2YGQgr-neX-Hd.ide.g.cs")]
    [InlineData(false, @"C:\path\to\file.razor.ide.g.cs")]
    public void GetRazorCSharpFilePath_ReturnsExpectedPath(bool includeProjectKey, string expected)
    {
        // Arrange
        var projectKey = new ProjectKey(@"C:\path\to\obj");
        var filePathService = new TestFilePathService(new TestLanguageServerFeatureOptions(includeProjectKeyInGeneratedFilePath: includeProjectKey));

        // Act
        var result = filePathService.GetRazorCSharpFilePath(projectKey, @"C:\path\to\file.razor");

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(true, @"C:\path\to\file.razor.p.ide.g.cs")]
    [InlineData(false, @"C:\path\to\file.razor.ide.g.cs")]
    public void GetRazorCSharpFilePath_NoProjectInfo_ReturnsExpectedPath(bool includeProjectKey, string expected)
    {
        // Arrange
        var projectKey = default(ProjectKey);
        var filePathService = new TestFilePathService(new TestLanguageServerFeatureOptions(includeProjectKeyInGeneratedFilePath: includeProjectKey));

        // Act
        var result = filePathService.GetRazorCSharpFilePath(projectKey, @"C:\path\to\file.razor");

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(true, @"C:\path\to\file.razor.t3Gf1FBjln6S9T95.ide.g.cs")]
    [InlineData(true, @"C:\path\to\file.razor.p.ide.g.cs")]
    [InlineData(false, @"C:\path\to\file.razor.ide.g.cs")]
    public void GetRazorDocumentUri_CSharpFile_ReturnsExpectedUri(bool includeProjectKey, string input)
    {
        // Arrange
        var filePathService = new TestFilePathService(new TestLanguageServerFeatureOptions(includeProjectKeyInGeneratedFilePath: includeProjectKey));

        // Act
        var result = filePathService.GetRazorDocumentUri(new Uri(input));

        // Assert
        Assert.Equal(@"C:/path/to/file.razor", result.GetAbsoluteOrUNCPath());
    }

    [Fact]
    public void GetRazorDocumentUri_HtmlFile_ReturnsExpectedUri()
    {
        // Arrange
        var filePathService = new TestFilePathService(new TestLanguageServerFeatureOptions(includeProjectKeyInGeneratedFilePath: true));
        // Act
        var result = filePathService.GetRazorDocumentUri(new Uri(@"C:\path\to\file.razor__virtual.html"));

        // Assert
        Assert.Equal(@"C:/path/to/file.razor", result.GetAbsoluteOrUNCPath());
    }

    [Fact]
    public void GetRazorDocumentUri_RazorFile_ReturnsExpectedUri()
    {
        // Arrange
        var filePathService = new TestFilePathService(new TestLanguageServerFeatureOptions(includeProjectKeyInGeneratedFilePath: true));
        // Act
        var result = filePathService.GetRazorDocumentUri(new Uri(@"C:\path\to\file.razor"));

        // Assert
        Assert.Equal(@"C:/path/to/file.razor", result.GetAbsoluteOrUNCPath());
    }

    private class TestFilePathService(TestLanguageServerFeatureOptions options) : AbstractFilePathService(options)
    {
    }
}
