// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Common.Test
{
    public class PathResolverTests : IClassFixture<TestDirectoryFixture>
    {
        private readonly TestDirectoryFixture _fixture;

        public PathResolverTests(TestDirectoryFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("*", true)]
        [InlineData("a.*", true)]
        [InlineData("*.b", true)]
        [InlineData("*.*", true)]
        [InlineData("a/b", false)]
        [InlineData("a/b.*", true)]
        [InlineData(@"a\b", false)]
        [InlineData(@"a\b.*", true)]
        [InlineData("**", true)]
        [InlineData("**/a", true)]
        [InlineData("**/*.a", true)]
        [InlineData(@"**\a", true)]
        [InlineData(@"**\*.a", true)]
        [InlineData("?", false)]
        [InlineData("[", false)]
        public void IsWildcardSearch_WithValidFilter_ReturnsExpectedResult(string filter, bool expectedResult)
        {
            Assert.Equal(expectedResult, PathResolver.IsWildcardSearch(filter));
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("/", false)]
        [InlineData(@"\", false)]
        [InlineData("a", false)]
        [InlineData("a/", true)]
        [InlineData("/a", false)]
        [InlineData("/a/", true)]
        public void IsDirectoryPath_WithSpecificPath_ReturnsExpectedResult(string path, bool expectedResult)
        {
            Assert.Equal(expectedResult, PathResolver.IsDirectoryPath(path));
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData(@"a\", true)]
        [InlineData(@"\\a", false)]
        [InlineData(@"\\a\", true)]
        [InlineData(@"\\a\b", false)]
        [InlineData(@"\\a\c\", true)]
        [InlineData("C:", false)]
        [InlineData(@"C:\a", false)]
        [InlineData(@"C:\a\", true)]
        [InlineData(@".\", true)]
        [InlineData(@"..\", true)]
        public void IsDirectoryPath_WithSpecificPath_OnWindows_ReturnsExpectedResult(string path, bool expectedResult)
        {
            Assert.Equal(expectedResult, PathResolver.IsDirectoryPath(path));
        }

        [PlatformTheory(Platform.Darwin)]
        [InlineData(@"a\", false)]
        [InlineData(@"\\a", false)]
        [InlineData(@"\\a\", false)]
        [InlineData(@"\\a\b", false)]
        [InlineData(@"\\a\c\", false)]
        [InlineData("C:", false)]
        [InlineData(@"C:\a", false)]
        [InlineData(@"C:\a\", false)]
        [InlineData(@".\", false)]
        [InlineData(@"..\", false)]
        public void IsDirectoryPath_WithSpecificPath_OnMacOs_ReturnsExpectedResult(string path, bool expectedResult)
        {
            Assert.Equal(expectedResult, PathResolver.IsDirectoryPath(path));
        }

        [PlatformTheory(Platform.Linux)]
        [InlineData(@"a\", false)]
        [InlineData(@"\\a", false)]
        [InlineData(@"\\a\", false)]
        [InlineData(@"\\a\b", false)]
        [InlineData(@"\\a\c\", false)]
        [InlineData("C:", false)]
        [InlineData(@"C:\a", false)]
        [InlineData(@"C:\a\", false)]
        [InlineData(@".\", false)]
        [InlineData(@"..\", false)]
        public void IsDirectoryPath_WithSpecificPath_OnLinux_ReturnsExpectedResult(string path, bool expectedResult)
        {
            Assert.Equal(expectedResult, PathResolver.IsDirectoryPath(path));
        }

        [Theory]
        [InlineData("**")]
        [InlineData("**/")]
        [InlineData(@"**\")]
        [InlineData("**/a/*.b")]
        [InlineData(@"**\a\*.b")]
        public void NormalizeWildcardForExcludedFiles_WithLeadingGlobstar_ReturnsWildcardAsIs(string wildcard)
        {
            string actualResult = PathResolver.NormalizeWildcardForExcludedFiles(_fixture.Path, wildcard);

            Assert.Equal(wildcard, actualResult);
        }

        [Fact]
        public void NormalizeWildcardForExcludedFiles_WithLeadingParentPathReference_ReturnsNormalizedWildcard()
        {
            string basePath = Path.Combine(_fixture.Path, "dir1", "dir2");
            string wildcard = string.Format("..{0}..{0}*", Path.DirectorySeparatorChar);
            string expectedResult = string.Format("{0}{1}*", new DirectoryInfo(_fixture.Path).FullName, Path.DirectorySeparatorChar);
            string actualResult = PathResolver.NormalizeWildcardForExcludedFiles(basePath, wildcard);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetMatches_WithDotGlobstar_ReturnsMatches()
        {
            IEnumerable<string> sources = GetPlatformSpecificPaths(new[] { ".{0}a{0}.c", ".{0}a{0}c", ".{0}.c", ".{0}c", "bc", "b.c", "c", ".c" });
            string[] wildcards = new[] { ".**" };
            IEnumerable<string> actualResults = PathResolver.GetMatches(sources, source => source, wildcards);
            IEnumerable<string> expectedResults = GetPlatformSpecificPaths(new[] { ".c" });

            IEnumerable<string> orderedActualResults = actualResults.OrderBy(path => path);
            IEnumerable<string> orderedExpectedResults = expectedResults.OrderBy(path => path);

            Assert.Equal(orderedExpectedResults, orderedActualResults);
        }

        [Fact]
        public void GetMatches_WithGlobstarSlash_ReturnsMatches()
        {
            IEnumerable<string> sources = GetPlatformSpecificPaths(new[] { "a{0}d", "a{0}de", "a{0}b{0}d", "a{0}b{0}de", "a{0}b{0}c{0}d", "a{0}b{0}c{0}de" });
            IEnumerable<string> wildcards = GetPlatformSpecificPaths(new[] { "a{0}**{0}d" });
            IEnumerable<string> actualResults = PathResolver.GetMatches(sources, source => source, wildcards);
            IEnumerable<string> expectedResults = GetPlatformSpecificPaths(new[] { "a{0}d", "a{0}b{0}d", "a{0}b{0}c{0}d" });

            IEnumerable<string> orderedActualResults = actualResults.OrderBy(path => path);
            IEnumerable<string> orderedExpectedResults = expectedResults.OrderBy(path => path);

            Assert.Equal(orderedExpectedResults, orderedActualResults);
        }

        [Fact]
        public void GetMatches_WithGlobstarDotFileName_ReturnsFileNameMatches()
        {
            IEnumerable<string> sources = GetPlatformSpecificPaths(new[] { ".{0}.c", "a{0}.c", "a{0}{0}.c", "a{0}bc", "bc", "b.c", "a{0}b{0}bc.c", "a{0}b{0}.c", "a{0}b{0}c" });
            IEnumerable<string> wildcards = GetPlatformSpecificPaths(new[] { "**.c" });
            IEnumerable<string> actualResults = PathResolver.GetMatches(sources, source => source, wildcards);
            IEnumerable<string> expectedResults = GetPlatformSpecificPaths(new[] { ".{0}.c", "a{0}.c", "a{0}{0}.c", "b.c", "a{0}b{0}bc.c", "a{0}b{0}.c" });

            IEnumerable<string> orderedActualResults = actualResults.OrderBy(path => path);
            IEnumerable<string> orderedExpectedResults = expectedResults.OrderBy(path => path);

            Assert.Equal(orderedExpectedResults, orderedActualResults);
        }

        [Fact]
        public void GetMatches_WithGlobstarSlashDotFileName_ReturnsFileNameMatches()
        {
            IEnumerable<string> sources = GetPlatformSpecificPaths(new[] { ".{0}.c", "a{0}.c", "a{0}{0}.c", "a{0}bc", "bc", "b.c", "a{0}b{0}bc.c", "a{0}b{0}.c", "a{0}b{0}c" });
            IEnumerable<string> wildcards = GetPlatformSpecificPaths(new[] { "**.c" });
            IEnumerable<string> actualResults = PathResolver.GetMatches(sources, source => source, wildcards);
            IEnumerable<string> expectedResults = GetPlatformSpecificPaths(new[] { ".{0}.c", "a{0}.c", "a{0}{0}.c", "b.c", "a{0}b{0}bc.c", "a{0}b{0}.c" });

            IEnumerable<string> orderedActualResults = actualResults.OrderBy(path => path);
            IEnumerable<string> orderedExpectedResults = expectedResults.OrderBy(path => path);

            Assert.Equal(orderedExpectedResults, orderedActualResults);
        }

        [Fact]
        public void GetMatches_WithGlobstarSlashFileName_ReturnsFileNameMatches()
        {
            IEnumerable<string> sources = GetPlatformSpecificPaths(new[] { ".{0}c", "a{0}c", "a{0}bc", "bc" });
            IEnumerable<string> wildcards = GetPlatformSpecificPaths(new[] { "**{0}c" });
            IEnumerable<string> actualResults = PathResolver.GetMatches(sources, source => source, wildcards);
            IEnumerable<string> expectedResults = GetPlatformSpecificPaths(new[] { ".{0}c", "a{0}c" });

            IEnumerable<string> orderedActualResults = actualResults.OrderBy(path => path);
            IEnumerable<string> orderedExpectedResults = expectedResults.OrderBy(path => path);

            Assert.Equal(orderedExpectedResults, orderedActualResults);
        }

        [Fact]
        public void GetMatches_WithGlobstar_ReturnsMatches()
        {
            IEnumerable<string> sources = GetPlatformSpecificPaths(new[] { "a{0}d", "a{0}de", "a{0}b{0}d", "a{0}b{0}de", "a{0}b{0}c{0}d", "a{0}b{0}c{0}de" });
            IEnumerable<string> wildcards = GetPlatformSpecificPaths(new[] { "a{0}**d" });
            IEnumerable<string> actualResults = PathResolver.GetMatches(sources, source => source, wildcards);
            IEnumerable<string> expectedResults = GetPlatformSpecificPaths(new[] { "a{0}d", "a{0}b{0}d", "a{0}b{0}c{0}d" });

            IEnumerable<string> orderedActualResults = actualResults.OrderBy(path => path);
            IEnumerable<string> orderedExpectedResults = expectedResults.OrderBy(path => path);

            Assert.Equal(orderedExpectedResults, orderedActualResults);
        }

        [Fact]
        public void GetMatches_WithStar_ReturnsMatches()
        {
            IEnumerable<string> sources = GetPlatformSpecificPaths(new[] { "a{0}d", "a{0}de", "a{0}b{0}d", "a{0}b{0}de", "a{0}b{0}c{0}d", "a{0}b{0}c{0}de" });
            IEnumerable<string> wildcards = GetPlatformSpecificPaths(new[] { "a{0}*{0}*{0}d" });
            IEnumerable<string> actualResults = PathResolver.GetMatches(sources, source => source, wildcards);
            IEnumerable<string> expectedResults = GetPlatformSpecificPaths(new[] { "a{0}b{0}c{0}d" });

            IEnumerable<string> orderedActualResults = actualResults.OrderBy(path => path);
            IEnumerable<string> orderedExpectedResults = expectedResults.OrderBy(path => path);

            Assert.Equal(orderedExpectedResults, orderedActualResults);
        }

        [Fact]
        public void GetMatches_WithQuestionMark_ReturnsMatches()
        {
            string[] sources = new[] { "a", "ab", "abc", "ac", "adc" };
            string[] wildcards = new[] { "a?c" };
            IEnumerable<string> actualResults = PathResolver.GetMatches(sources, source => source, wildcards);
            string[] expectedResults = new[] { "abc", "adc" };

            IEnumerable<string> orderedActualResults = actualResults.OrderBy(path => path);
            IEnumerable<string> orderedExpectedResults = expectedResults.OrderBy(path => path);

            Assert.Equal(orderedExpectedResults, orderedActualResults);
        }

        [Fact]
        public void FilterPackageFiles_WithWildcard_RemovesWildcardMatches()
        {
            var sources = new List<string>(GetPlatformSpecificPaths(new[] { "c", "a{0}c", "a{0}b{0}c", "a{0}d" }));
            IEnumerable<string> wildcards = GetPlatformSpecificPaths(new[] { "**{0}c" });

            PathResolver.FilterPackageFiles(sources, path => path, wildcards);

            IEnumerable<string> expectedResults = GetPlatformSpecificPaths(new[] { "a{0}d" });
            Assert.Equal(expectedResults, sources);
        }

        [Theory]
        [InlineData("dir1/dir2")]
        [InlineData(@"dir1\dir2")]
        public void PerformWildcardSearch_WithDirectory_FindsNoMatchingFiles(string searchPath)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(new string[] { }, actualFullPaths);
        }

        [Fact]
        public void PerformWildcardSearch_WithDirectory_FindsMatchingEmptyDirectory()
        {
            string normalizedBasePath;
            IEnumerable<PathResolver.SearchPathResult> actualResults = PathResolver.PerformWildcardSearch(
                _fixture.Path, "dir*",
                includeEmptyDirectories: true,
                normalizedBasePath: out normalizedBasePath);

            Assert.Collection(actualResults, result =>
            {
                Assert.False(result.IsFile);

                var expectedRelativePath = string.Format("{0}dir5", Path.DirectorySeparatorChar);
                var actualRelativePath = result.Path.Substring(_fixture.Path.Length);

                Assert.Equal(expectedRelativePath, actualRelativePath);
            });
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("dir1/dir2/")]
        [InlineData(@"dir1\dir2\")]
        public void PerformWildcardSearch_WithDirectoryAndTrailingSlashRecursively_OnWindows_FindsAllMatchingFiles(string searchPath)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(
                new[]
                {
                    @"\dir1\dir2\file1.txt",
                    @"\dir1\dir2\file2.txt",
                    @"\dir1\dir2\dir3\file1.txt",
                    @"\dir1\dir2\dir3\file2.txt"
                },
                actualFullPaths);
        }

        [PlatformTheory(Platform.Darwin)]
        [InlineData("dir1/dir2/", new[]
            {
                "/dir1/dir2/file1.txt",
                "/dir1/dir2/file2.txt",
                "/dir1/dir2/dir3/file1.txt",
                "/dir1/dir2/dir3/file2.txt"
            })]
        [InlineData(@"dir1\dir2\", new string[] { })]
        public void PerformWildcardSearch_WithDirectoryAndTrailingSlashRecursively_OnMacOs_FindsAllMatchingFiles(string searchPath, string[] expectedResults)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(expectedResults, actualFullPaths);
        }

        [PlatformTheory(Platform.Linux)]
        [InlineData("dir1/dir2/", new[]
            {
                "/dir1/dir2/file1.txt",
                "/dir1/dir2/file2.txt",
                "/dir1/dir2/dir3/file1.txt",
                "/dir1/dir2/dir3/file2.txt"
            })]
        [InlineData(@"dir1\dir2\", new string[] { })]
        public void PerformWildcardSearch_WithDirectoryAndTrailingSlashRecursively_OnLinux_FindsAllMatchingFiles(string searchPath, string[] expectedResults)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(expectedResults, actualFullPaths);
        }

        [Fact]
        public void PerformWildcardSearch_WithFileName_FindsNoMatchingFile()
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, "file3.txt");

            Verify(new string[] { }, actualFullPaths);
        }

        [Fact]
        public void PerformWildcardSearch_WithFileName_FindsMatchingFile()
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, "file1.txt");

            Verify(
                new[]
                {
                    $"{Path.DirectorySeparatorChar}file1.txt"
                },
                actualFullPaths);
        }

        [Fact]
        public void PerformWildcardSearch_WithFileNamePattern_FindsMatchingFiles()
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, "*.txt");

            Verify(
                new[]
                {
                    $"{Path.DirectorySeparatorChar}file1.txt",
                    $"{Path.DirectorySeparatorChar}file2.txt"
                },
                actualFullPaths);
        }

        [Fact]
        public void PerformWildcardSearch_WithFileNamePattern_FindsNoMatchingFile()
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, "*.dll");

            Verify(new string[] { }, actualFullPaths);
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("dir1/file2.txt")]
        [InlineData(@"dir1\file2.txt")]
        public void PerformWildcardSearch_WithDirectoryAndFileName_OnWindows_FindsMatchingFile(string searchPath)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(new[] { @"\dir1\file2.txt" }, actualFullPaths);
        }

        [PlatformTheory(Platform.Darwin)]
        [InlineData("dir1/file2.txt", new[] { "/dir1/file2.txt" })]
        [InlineData(@"dir1\file2.txt", new string[] { })]
        public void PerformWildcardSearch_WithDirectoryAndFileName_OnMacOs_FindsMatchingFile(string searchPath, string[] expectedResults)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(expectedResults, actualFullPaths);
        }

        [PlatformTheory(Platform.Linux)]
        [InlineData("dir1/file2.txt", new[] { "/dir1/file2.txt" })]
        [InlineData(@"dir1\file2.txt", new string[] { })]
        public void PerformWildcardSearch_WithDirectoryAndFileName_OnLinux_FindsMatchingFile(string searchPath, string[] expectedResults)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(expectedResults, actualFullPaths);
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("dir1/../file1.txt")]
        [InlineData(@"dir1\..\file1.txt")]
        public void PerformWildcardSearch_WithDirectoryRelativePathAndFileName_OnWindows_FindsMatchingFile(string searchPath)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(new[] { @"\dir1\..\file1.txt" }, actualFullPaths);
        }

        [PlatformTheory(Platform.Darwin)]
        [InlineData("dir1/../file1.txt", new[] { "/dir1/../file1.txt" })]
        [InlineData(@"dir1\..\file1.txt", new string[] { })]
        public void PerformWildcardSearch_WithDirectoryRelativePathAndFileName_OnMacOs_FindsMatchingFile(string searchPath, string[] expectedResults)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(expectedResults, actualFullPaths);
        }

        [PlatformTheory(Platform.Linux)]
        [InlineData("dir1/../file1.txt", new[] { "/dir1/../file1.txt" })]
        [InlineData(@"dir1\..\file1.txt", new string[] { })]
        public void PerformWildcardSearch_WithDirectoryRelativePathAndFileName_OnLinux_FindsMatchingFile(string searchPath, string[] expectedResults)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(expectedResults, actualFullPaths);
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("**/file1.txt")]
        [InlineData(@"**\file1.txt")]
        public void PerformWildcardSearch_WithGlobstarAndFileName_OnWindows_RecursivelyFindsAllMatchingFiles(string searchPath)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(
                new[]
                {
                    @"\file1.txt",
                    @"\dir1\file1.txt",
                    @"\dir1\dir2\file1.txt",
                    @"\dir1\dir2\dir3\file1.txt",
                    @"\dir1\dir4\file1.txt"
                },
                actualFullPaths);
        }

        [PlatformTheory(Platform.Darwin)]
        [InlineData("**/file1.txt", new[]
            {
                "/file1.txt",
                "/dir1/file1.txt",
                "/dir1/dir2/file1.txt",
                "/dir1/dir2/dir3/file1.txt",
                "/dir1/dir4/file1.txt"
            })]
        [InlineData(@"**\file1.txt", new string[] { })]
        public void PerformWildcardSearch_WithGlobstarAndFileName_OnMacOs_RecursivelyFindsAllMatchingFiles(string searchPath, string[] expectedResults)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(expectedResults, actualFullPaths);
        }

        [PlatformTheory(Platform.Linux)]
        [InlineData("**/file1.txt", new[]
            {
                "/file1.txt",
                "/dir1/file1.txt",
                "/dir1/dir2/file1.txt",
                "/dir1/dir2/dir3/file1.txt",
                "/dir1/dir4/file1.txt"
            })]
        [InlineData(@"**\file1.txt", new string[] { })]
        public void PerformWildcardSearch_WithGlobstarAndFileName_OnLinux_RecursivelyFindsAllMatchingFiles(string searchPath, string[] expectedResults)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(expectedResults, actualFullPaths);
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("**/*.txt")]
        [InlineData(@"**\*.txt")]
        public void PerformWildcardSearch_WithGlobstarAndFileNamePattern_OnWindows_RecursivelyFindsAllMatchingFiles(string searchPath)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(
                new[]
                {
                    @"\file1.txt",
                    @"\file2.txt",
                    @"\dir1\file1.txt",
                    @"\dir1\file2.txt",
                    @"\dir1\dir2\file1.txt",
                    @"\dir1\dir2\file2.txt",
                    @"\dir1\dir2\dir3\file1.txt",
                    @"\dir1\dir2\dir3\file2.txt",
                    @"\dir1\dir4\file1.txt",
                    @"\dir1\dir4\file2.txt"
                },
                actualFullPaths);
        }

        [PlatformTheory(Platform.Darwin)]
        [InlineData("**/*.txt", new[]
            {
                "/file1.txt",
                "/file2.txt",
                "/dir1/file1.txt",
                "/dir1/file2.txt",
                "/dir1/dir2/file1.txt",
                "/dir1/dir2/file2.txt",
                "/dir1/dir2/dir3/file1.txt",
                "/dir1/dir2/dir3/file2.txt",
                "/dir1/dir4/file1.txt",
                "/dir1/dir4/file2.txt"
            })]
        [InlineData(@"**\*.txt", new string[] { })]
        public void PerformWildcardSearch_WithGlobstarAndFileNamePattern_OnMacOs_RecursivelyFindsAllMatchingFiles(string searchPath, string[] expectedResults)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(expectedResults, actualFullPaths);
        }

        [PlatformTheory(Platform.Linux)]
        [InlineData("**/*.txt", new[]
            {
                "/file1.txt",
                "/file2.txt",
                "/dir1/file1.txt",
                "/dir1/file2.txt",
                "/dir1/dir2/file1.txt",
                "/dir1/dir2/file2.txt",
                "/dir1/dir2/dir3/file1.txt",
                "/dir1/dir2/dir3/file2.txt",
                "/dir1/dir4/file1.txt",
                "/dir1/dir4/file2.txt"
            })]
        [InlineData(@"**\*.txt", new string[] { })]
        public void PerformWildcardSearch_WithGlobstarAndFileNamePattern_OnLinux_RecursivelyFindsAllMatchingFiles(string searchPath, string[] expectedResults)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(expectedResults, actualFullPaths);
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("dir1/**/file2.txt")]
        [InlineData(@"dir1\**\file2.txt")]
        public void PerformWildcardSearch_WithDirectoryGlobstarAndFileNameAtNonRootDirectory_OnWindows_RecursivelyFindsAllMatchingFiles(string searchPath)
        {
            var actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(
                new[]
                {
                    @"\dir1\file2.txt",
                    @"\dir1\dir2\file2.txt",
                    @"\dir1\dir2\dir3\file2.txt",
                    @"\dir1\dir4\file2.txt"
                },
                actualFullPaths);
        }

        [PlatformTheory(Platform.Darwin)]
        [InlineData("dir1/**/file2.txt", new[]
            {
                "/dir1/file2.txt",
                "/dir1/dir2/file2.txt",
                "/dir1/dir2/dir3/file2.txt",
                "/dir1/dir4/file2.txt"
            })]
        [InlineData(@"dir1\**\file2.txt", new string[] { })]
        public void PerformWildcardSearch_WithDirectoryGlobstarAndFileNameAtNonRootDirectory_OnMacOs_RecursivelyFindsAllMatchingFiles(string searchPath, string[] expectedResults)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(expectedResults, actualFullPaths);
        }

        [PlatformTheory(Platform.Linux)]
        [InlineData("dir1/**/file2.txt", new[]
            {
                "/dir1/file2.txt",
                "/dir1/dir2/file2.txt",
                "/dir1/dir2/dir3/file2.txt",
                "/dir1/dir4/file2.txt"
            })]
        [InlineData(@"dir1\**\file2.txt", new string[] { })]
        public void PerformWildcardSearch_WithDirectoryGlobstarAndFileNameAtNonRootDirectory_OnLinux_RecursivelyFindsAllMatchingFiles(string searchPath, string[] expectedResults)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(expectedResults, actualFullPaths);
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("dir1/dir*/*.txt")]
        [InlineData(@"dir1\dir*\*.txt")]
        public void PerformWildcardSearch_WithDirectoryPatternAndFileNamePattern_OnWindows_RecursivelyFindsAllMatchingFiles(string searchPath)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(
                new[]
                {
                    @"\dir1\dir2\file1.txt",
                    @"\dir1\dir2\file2.txt",
                    @"\dir1\dir4\file1.txt",
                    @"\dir1\dir4\file2.txt"
                },
                actualFullPaths);
        }

        [PlatformTheory(Platform.Darwin)]
        [InlineData("dir1/dir*/*.txt", new[]
            {
                "/dir1/dir2/file1.txt",
                "/dir1/dir2/file2.txt",
                "/dir1/dir4/file1.txt",
                "/dir1/dir4/file2.txt"
            })]
        [InlineData(@"dir1\dir*\*.txt", new string[] { })]
        public void PerformWildcardSearch_WithDirectoryPatternAndFileNamePattern_OnMacOs_RecursivelyFindsAllMatchingFiles(string searchPath, string[] expectedResults)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(expectedResults, actualFullPaths);
        }

        [PlatformTheory(Platform.Linux)]
        [InlineData("dir1/dir*/*.txt", new[]
            {
                "/dir1/dir2/file1.txt",
                "/dir1/dir2/file2.txt",
                "/dir1/dir4/file1.txt",
                "/dir1/dir4/file2.txt"
            })]
        [InlineData(@"dir1\dir*\*.txt", new string[] { })]
        public void PerformWildcardSearch_WithDirectoryPatternAndFileNamePattern_OnLinux_RecursivelyFindsAllMatchingFiles(string searchPath, string[] expectedResults)
        {
            IEnumerable<string> actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(expectedResults, actualFullPaths);
        }

        [PlatformTheory(Platform.Linux)]
        [InlineData("dir6/*.EMPTY", new[]
            {
                "/dir6/FILE3.EMPTY",
            })]
        public void PathResolver_PerformWildcardSearch_UppercaseFilename_OnLinux(string searchPath, string[] expectedResults)
        {
            var actualFullPaths = PathResolver.PerformWildcardSearch(_fixture.Path, searchPath);

            Verify(expectedResults, actualFullPaths);
        }

        private void Verify(IEnumerable<string> expectedRelativePaths, IEnumerable<string> actualFullPaths)
        {
            IEnumerable<string> actualRelativePaths = actualFullPaths.Select(fullPath => fullPath.Substring(_fixture.Path.Length));
            IOrderedEnumerable<string> expectedResults = expectedRelativePaths.OrderBy(path => path);
            IOrderedEnumerable<string> actualResults = actualRelativePaths.OrderBy(path => path);

            Assert.Equal(expectedResults, actualResults);
        }

        private static IEnumerable<string> GetPlatformSpecificPaths(IEnumerable<string> platformUnspecificPaths)
        {
            return platformUnspecificPaths.Select(path => string.Format(path, Path.DirectorySeparatorChar));
        }
    }

    /*
    Test directory contents:

        dir1
            dir2
                dir3
                    file1.txt
                    file2.txt
                file1.txt
                file2.txt
            dir4
                file1.txt
                file2.txt
            file1.txt
            file2.txt
        dir5
        dir6
            FILE3.EMPTY
        file1.txt
        file2.txt
    */
    public sealed class TestDirectoryFixture : IDisposable
    {
        private readonly TestDirectory _rootDirectory;

        public string Path => _rootDirectory.Path;

        public TestDirectoryFixture()
        {
            _rootDirectory = TestDirectory.Create();

            PopulateTestDirectory();
        }

        public void Dispose()
        {
            _rootDirectory.Dispose();
        }

        private void PopulateTestDirectory()
        {
            var rootDirectory = new DirectoryInfo(_rootDirectory.Path);
            DirectoryInfo directory1 = Directory.CreateDirectory(System.IO.Path.Combine(rootDirectory.FullName, "dir1"));
            DirectoryInfo directory2 = Directory.CreateDirectory(System.IO.Path.Combine(directory1.FullName, "dir2"));
            DirectoryInfo directory3 = Directory.CreateDirectory(System.IO.Path.Combine(directory2.FullName, "dir3"));
            DirectoryInfo directory4 = Directory.CreateDirectory(System.IO.Path.Combine(directory1.FullName, "dir4"));
            Directory.CreateDirectory(System.IO.Path.Combine(rootDirectory.FullName, "dir5"));
            DirectoryInfo directory6 = Directory.CreateDirectory(System.IO.Path.Combine(rootDirectory.FullName, "dir6"));

            CreateTestFiles(rootDirectory);
            CreateTestFiles(directory1);
            CreateTestFiles(directory2);
            CreateTestFiles(directory3);
            CreateTestFiles(directory4);

            File.WriteAllText(System.IO.Path.Combine(directory6.FullName, "FILE3.EMPTY"), string.Empty);
        }

        private static void CreateTestFiles(DirectoryInfo directory)
        {
            File.WriteAllText(System.IO.Path.Combine(directory.FullName, "file1.txt"), string.Empty);
            File.WriteAllText(System.IO.Path.Combine(directory.FullName, "file2.txt"), string.Empty);
        }
    }
}
