// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.LanguageServer.DirectoryHelper;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test;

public class DirectoryHelperTest(ITestOutputHelper testOutput) : TagHelperServiceTestBase(testOutput)
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
        var fileResults = new Dictionary<string, IEnumerable<string>>() {
            { "HigherDirectory", new []{ firstProjectRazorJson } },
            { "RealDirectory", new []{ secondProjectRazorJson } },
            { "LongDirectory", new[]{ "LONGPATH", "LONGPATH\\project.razor.bin" } },
            { "node_modules", null },
        };
        var directoryResults = new Dictionary<string, IEnumerable<string>>() {
            { "LowerDirectory", new[]{ "HigherDirectory" } },
            { "HigherDirectory", new[]{ "node_modules", "RealDirectory", "FakeDirectory", "LongDirectory" } },
            { "node_modules", null },
        };

#pragma warning disable CS0612 // Type or member is obsolete
        var fileSystem = new TestFileSystem(fileResults, directoryResults);
#pragma warning restore CS0612 // Type or member is obsolete

        // Act
        var files = DirectoryHelper.GetFilteredFiles(workspaceDirectory, searchPattern, ignoredDirectories, fileSystem);

        // Assert
        Assert.Collection(files,
            result => result.Equals(firstProjectRazorJson),
            result => result.Equals(secondProjectRazorJson)
        );
    }

    [Obsolete]
    private class TestFileSystem : IFileSystem
    {
        private readonly IDictionary<string, IEnumerable<string>> _fileResults;
        private readonly IDictionary<string, IEnumerable<string>> _directoryResults;

        public TestFileSystem(
            IDictionary<string, IEnumerable<string>> fileResults,
            IDictionary<string, IEnumerable<string>> directoryResults)
        {
            _fileResults = fileResults;
            _directoryResults = directoryResults;
        }

        public IEnumerable<string> GetDirectories(string workspaceDirectory)
        {
            var success = _directoryResults.TryGetValue(workspaceDirectory, out var results);
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
            var success = _fileResults.TryGetValue(workspaceDirectory, out var results);
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
    }
}
