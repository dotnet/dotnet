// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

public class FilePathNormalizerTest : TestBase
{
    public FilePathNormalizerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [OSSkipConditionFact(new[] { "OSX", "Linux" })]
    public void Normalize_Windows_StripsPrecedingSlash()
    {
        // Arrange
        var path = "/c:/path/to/something";

        // Act
        path = FilePathNormalizer.Normalize(path);

        // Assert
        Assert.Equal("c:/path/to/something", path);
    }

    [Fact]
    public void Normalize_IgnoresUNCPaths()
    {
        // Arrange
        var path = "//ComputerName/path/to/something";

        // Act
        path = FilePathNormalizer.Normalize(path);

        // Assert
        Assert.Equal("//ComputerName/path/to/something", path);
    }

    [Fact]
    public void NormalizeDirectory_EndsWithSlash()
    {
        // Arrange
        var directory = "C:\\path\\to\\directory\\";

        // Act
        var normalized = FilePathNormalizer.NormalizeDirectory(directory);

        // Assert
        Assert.Equal("C:/path/to/directory/", normalized);
    }

    [Fact]
    public void NormalizeDirectory_EndsWithoutSlash()
    {
        // Arrange
        var directory = "C:\\path\\to\\directory";

        // Act
        var normalized = FilePathNormalizer.NormalizeDirectory(directory);

        // Assert
        Assert.Equal("C:/path/to/directory/", normalized);
    }

    [Fact]
    public void FilePathsEquivalent_NotEqualPaths_ReturnsFalse()
    {
        // Arrange
        var filePath1 = "path/to/document.cshtml";
        var filePath2 = "path\\to\\different\\document.cshtml";

        // Act
        var result = FilePathNormalizer.FilePathsEquivalent(filePath1, filePath2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void FilePathsEquivalent_NormalizesPathsBeforeComparison_ReturnsTrue()
    {
        // Arrange
        var filePath1 = "path/to/document.cshtml";
        var filePath2 = "path\\to\\document.cshtml";

        // Act
        var result = FilePathNormalizer.FilePathsEquivalent(filePath1, filePath2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetDirectory_IncludesTrailingSlash()
    {
        // Arrange
        var filePath = "C:/path/to/document.cshtml";

        // Act
        var normalized = FilePathNormalizer.GetDirectory(filePath);

        // Assert
        Assert.Equal("C:/path/to/", normalized);
    }

    [Fact]
    public void GetDirectory_NoDirectory_ReturnsRoot()
    {
        // Arrange
        var filePath = "C:/document.cshtml";

        // Act
        var normalized = FilePathNormalizer.GetDirectory(filePath);

        // Assert
        Assert.Equal("C:/", normalized);
    }

    [Fact]
    public void Normalize_NullFilePath_ReturnsForwardSlash()
    {
        // Act
        var normalized = FilePathNormalizer.Normalize((string)null);

        // Assert
        Assert.Equal("/", normalized);
    }

    [Fact]
    public void Normalize_EmptyFilePath_ReturnsEmptyString()
    {
        // Act
        var normalized = FilePathNormalizer.Normalize(string.Empty);

        // Assert
        Assert.Equal("/", normalized);
    }

    [OSSkipConditionFact(new[] { "Windows" })]
    public void Normalize_NonWindows_AddsLeadingForwardSlash()
    {
        // Arrange
        var filePath = "path/to/document.cshtml";

        // Act
        var normalized = FilePathNormalizer.Normalize(filePath);

        // Assert
        Assert.Equal("/path/to/document.cshtml", normalized);
    }

    [Fact]
    public void Normalize_UrlDecodesFilePath()
    {
        // Arrange
        var filePath = "C:/path%20to/document.cshtml";

        // Act
        var normalized = FilePathNormalizer.Normalize(filePath);

        // Assert
        Assert.Equal("C:/path to/document.cshtml", normalized);
    }

    [Fact]
    public void Normalize_UrlDecodesOnlyOnce()
    {
        // Arrange
        var filePath = "C:/path%2Bto/document.cshtml";

        // Act
        var normalized = FilePathNormalizer.Normalize(filePath);
        normalized = FilePathNormalizer.Normalize(normalized);

        // Assert
        Assert.Equal("C:/path+to/document.cshtml", normalized);
    }

    [Fact]
    public void Normalize_ReplacesBackSlashesWithForwardSlashes()
    {
        // Arrange
        var filePath = "C:\\path\\to\\document.cshtml";

        // Act
        var normalized = FilePathNormalizer.Normalize(filePath);

        // Assert
        Assert.Equal("C:/path/to/document.cshtml", normalized);
    }
}
