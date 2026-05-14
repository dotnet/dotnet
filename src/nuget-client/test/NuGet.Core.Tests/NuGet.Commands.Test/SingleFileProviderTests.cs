// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.FileProviders;
using Xunit;

namespace NuGet.Commands.Test;

public sealed class SingleFileProviderTests
{
    [Theory]
    [InlineData("a/b/file.txt", "ROOT", "a", true)]
    [InlineData("a/b/file.txt", "ROOT/a", "a/b", true)]
    [InlineData("a/b/file.txt", "ROOT/a/b", "a/b/file.txt", false)]
    [InlineData("a/b/file.txt", "a", "a/b", true)]
    [InlineData("a/b/file.txt", "a/b", "a/b/file.txt", false)]
    [InlineData("a/b/file.txt", "ROOT/A", "a/b", true)]
    [InlineData("a/b/file.txt", "ROOT/A/B", "a/b/file.txt", false)]
    [InlineData("a/b/file.txt", "A", "a/b", true)]
    [InlineData("a/b/file.txt", "A/B", "a/b/file.txt", false)]
    public void GetDirectoryContents(string source, string search, string expected, bool isDirectory)
    {
        IDirectoryContents contents = new SingleFileProvider(source).GetDirectoryContents(search);

        IFileInfo content = Assert.Single(contents);

        Assert.Equal(expected, content.PhysicalPath);
        Assert.Equal(isDirectory, content.IsDirectory);
    }

    [Theory]
    [InlineData("a/b/file.txt", "")]
    [InlineData("a/b/file.txt", "ROOT/")]
    [InlineData("a/b/file.txt", "ROOT/a/")]
    [InlineData("a/b/file.txt", "ROOT/z")]
    [InlineData("a/b/file.txt", "ROOT/aa")]
    [InlineData("a/b/file.txt", "ROOT/z/a")]
    [InlineData("a/b/file.txt", "ROOT/a/z")]
    [InlineData("a/b/file.txt", "ROOT/a/b/file")]
    [InlineData("a/b/file.txt", "ROOT/a/b/file.txt")]
    [InlineData("a/b/file.txt", "/a/b/file.txt")]
    [InlineData("a/b/file.txt", "/a/")]
    [InlineData("a/b/file.txt", "/a")]
    [InlineData("a/b/file.txt", "a/")]
    [InlineData("a/b/file.txt", "a//b")]
    public void GetDirectoryContents_NoMatch(string source, string search)
    {
        IDirectoryContents contents = new SingleFileProvider(source).GetDirectoryContents(search);

        Assert.Empty(contents);
    }
}
