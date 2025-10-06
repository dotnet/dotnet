// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test;

public class IFileSystemExtensionsTest(ITestOutputHelper testOutput) : TagHelperServiceTestBase(testOutput)
{
    [Fact]
    public void GetFilteredFiles_FindsFiles()
    {
        // Arrange
        var firstProjectRazorJson = @"HigherDirectory\project.razor.bin";
        var secondProjectRazorJson = @"HigherDirectory\RealDirectory\project.razor.bin";

        var workspaceDirectory = Path.Combine("LowerDirectory");
        var searchPattern = "project.razor.bin";
        var ignoredDirectories = new[] { "node_modules" };
        var fileResults = new Dictionary<string, string[]?>() {
            { "HigherDirectory", [firstProjectRazorJson] },
            { "RealDirectory", [secondProjectRazorJson] },
            { "LongDirectory", ["LONGPATH", "LONGPATH\\project.razor.bin"] },
            { "node_modules", null },
        };
        var directoryResults = new Dictionary<string, string[]?>() {
            { "LowerDirectory", ["HigherDirectory"] },
            { "HigherDirectory", ["node_modules", "RealDirectory", "FakeDirectory", "LongDirectory"] },
            { "node_modules", null },
        };

        var fileSystem = new TestFileSystem(fileResults, directoryResults);

        // Act
        var files = fileSystem.GetFilteredFiles(workspaceDirectory, searchPattern, ignoredDirectories, Logger);

        // Assert
        Assert.Collection(files,
            result => result.Equals(firstProjectRazorJson),
            result => result.Equals(secondProjectRazorJson)
        );
    }

    private class TestFileSystem(
        Dictionary<string, string[]?> fileResults,
        Dictionary<string, string[]?> directoryResults) : IFileSystem
    {
        public IEnumerable<string> GetDirectories(string workspaceDirectory)
        {
            var success = directoryResults.TryGetValue(workspaceDirectory, out var results);
            if (success)
            {
                if (results is null)
                {
                    Assert.Fail("Tried to walk a directory which should have been ignored");
                }

                if (results.Any(s => s.Equals("LONGPATH")))
                {
                    throw new PathTooLongException();
                }

                return results;
            }
            else
            {
                throw new DirectoryNotFoundException();
            }
        }

        public IEnumerable<string> GetFiles(string workspaceDirectory, string searchPattern, SearchOption searchOption)
        {
            var success = fileResults.TryGetValue(workspaceDirectory, out var results);
            if (success)
            {
                if (results is null)
                {
                    Assert.Fail("Tried to walk a directory which should have been ignored");
                }

                if (results.Any(s => s.Equals("LONGPATH")))
                {
                    throw new PathTooLongException();
                }

                return results;
            }
            else
            {
                throw new DirectoryNotFoundException();
            }
        }

        public bool FileExists(string filePath)
            => throw new NotImplementedException();

        public string ReadFile(string filePath)
            => throw new NotImplementedException();

        public Stream OpenReadStream(string filePath)
            => throw new NotImplementedException();
    }
}
