// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using NuGet.Client;
using NuGet.ContentModel;
using NuGet.Packaging;
using NuGet.ProjectModel;
using Xunit;

namespace NuGet.Commands.Test;

public sealed class ContentFileUtilsTests
{
    [Theory]
    [InlineData("contentFiles/cs/net45/config/config.xml", "config/config.xml")]
    [InlineData("contentFiles/any/any/config/config.xml", "config/config.xml")]
    [InlineData("", "")]
    [InlineData("hello", "hello")]
    [InlineData("a/b/", "a/b/")]
    [InlineData("a/b/c/", "")]
    [InlineData("a/b/c", "a/b/c")]
    [InlineData("///config/config.xml", "config/config.xml")]
    [InlineData("/a/b/config/config.xml", "config/config.xml")]
    public void GetContentFileFolderRelativeToFramework(string input, string expected)
    {
        using var _ = new SuppressAsserts();

        string actual = ContentFileUtils.GetContentFileFolderRelativeToFramework(input.AsSpan());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetContentFileGroup_MultiSegmentPaths_MatchedByGlobstarPattern()
    {
        // Verifies that FilePatternMatch.Path round-trips correctly through
        // InMemoryDirectoryInfo for multi-segment paths with ** glob patterns.
        var nuspec = CreateNuspecWithContentFiles(("**/*", null, "EmbeddedResource"));

        var group = new ContentItemGroup();
        group.Properties[ManagedCodeConventions.PropertyNames.CodeLanguage] = "cs";
        group.Items.Add(new ContentItem { Path = "contentFiles/cs/net45/sub/deep/file.txt" });

        List<LockFileContentFile> result = ContentFileUtils.GetContentFileGroup(
            nuspec,
            new List<ContentItemGroup> { group });

        LockFileContentFile item = Assert.Single(result);
        Assert.Equal("contentFiles/cs/net45/sub/deep/file.txt", item.Path);
        // EmbeddedResource proves the glob matched; default would be Compile.
        Assert.Equal(BuildAction.Parse("EmbeddedResource"), item.BuildAction);
    }

    [Fact]
    public void GetContentFileGroup_MultipleNuspecEntries_AllMatchCorrectly()
    {
        // Verifies that the shared InMemoryDirectoryInfo is not mutated between
        // Matcher.Execute calls — the second matcher must still produce correct results.
        var nuspec = CreateNuspecWithContentFiles(
            ("cs/**/*", null, "EmbeddedResource"),
            ("vb/**/*", null, "None"));

        var csGroup = new ContentItemGroup();
        csGroup.Properties[ManagedCodeConventions.PropertyNames.CodeLanguage] = "cs";
        csGroup.Items.Add(new ContentItem { Path = "contentFiles/cs/net45/file.cs" });

        var vbGroup = new ContentItemGroup();
        vbGroup.Properties[ManagedCodeConventions.PropertyNames.CodeLanguage] = "vb";
        vbGroup.Items.Add(new ContentItem { Path = "contentFiles/vb/net45/file.vb" });

        List<LockFileContentFile> result = ContentFileUtils.GetContentFileGroup(
            nuspec,
            new List<ContentItemGroup> { csGroup, vbGroup });

        Assert.Equal(2, result.Count);
        LockFileContentFile csFile = result.Single(r => r.Path == "contentFiles/cs/net45/file.cs");
        LockFileContentFile vbFile = result.Single(r => r.Path == "contentFiles/vb/net45/file.vb");
        // EmbeddedResource proves cs/** matched the cs file.
        Assert.Equal(BuildAction.Parse("EmbeddedResource"), csFile.BuildAction);
        // None proves vb/** matched the vb file (default would be Compile).
        Assert.Equal(BuildAction.Parse("None"), vbFile.BuildAction);
    }

    private static NuspecReader CreateNuspecWithContentFiles(
        params (string include, string? exclude, string buildAction)[] entries)
    {
        var contentFilesElement = new XElement("contentFiles");

        foreach ((var include, var exclude, var buildAction) in entries)
        {
            var filesElement = new XElement("files", new XAttribute("include", include));

            if (exclude != null)
            {
                filesElement.Add(new XAttribute("exclude", exclude));
            }

            if (buildAction != null)
            {
                filesElement.Add(new XAttribute("buildAction", buildAction));
            }

            contentFilesElement.Add(filesElement);
        }

        var doc = new XDocument(
            new XElement("package",
                new XElement("metadata",
                    new XElement("id", "TestPackage"),
                    new XElement("version", "1.0.0"),
                    new XElement("description", "Test"),
                    new XElement("authors", "Test"),
                    contentFilesElement)));

        return new NuspecReader(doc);
    }

    /// <summary>
    /// Suppresses the default trace listeners for a specific duration. Useful to ensure that
    /// debug asserts don't block unit tests indefinitely by showing modal dialog messages
    /// about assertion failures.
    /// </summary>
    private sealed class SuppressAsserts : IDisposable
    {
        private readonly TraceListener[] _suppressedListeners;

        public SuppressAsserts()
        {
            _suppressedListeners = Trace.Listeners.Cast<TraceListener>().ToArray();

            Trace.Listeners.Clear();
        }

        public void Dispose()
        {
            Trace.Listeners.AddRange(_suppressedListeners);
        }
    }
}
