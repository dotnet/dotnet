// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace Microsoft.Build.NuGetSdkResolver.Test
{
    /// <summary>
    /// Unit tests for the <see cref="GlobalJsonReader" /> class.
    /// </summary>
    public class GlobalJsonReaderTests
    {
        /// <summary>
        /// Verifies that <see cref="GlobalJsonReader.GetMSBuildSdkVersions(Framework.SdkResolverContext)" /> ignores duplicates in the msbuild-sdks section and uses the last specified version.
        /// </summary>
        [Fact]
        public void GetMSBuildSdkVersions_IgnoresDuplicates_WhenGlobalJsonContainsDuplicates()
        {
            var expectedVersions = new Dictionary<string, string>
            {
                {"Sdk1", "3.0.0"},
                {"Sdk2", "2.0.0"},
            };

            using (var testDirectory = TestDirectory.Create())
            {
                string expectedGlobalJsonPath = Path.Combine(testDirectory, GlobalJsonReader.GlobalJsonFileName);

                File.WriteAllText(
                    expectedGlobalJsonPath,
                    @"{
  // This is a comment
  ""msbuild-sdks"": {
    ""Sdk1"": ""1.0.0"",
    ""Sdk2"": ""2.0.0"",
    ""Sdk1"": ""3.0.0"",
  }
}
");

                var context = new MockSdkResolverContext(testDirectory);

                var globalJsonReader = GlobalJsonReader.Instance;

                string actualGlobalJsonPath = null;

                globalJsonReader.FileRead += (_, globalJsonPath) =>
                {
                    actualGlobalJsonPath = globalJsonPath;
                };

                globalJsonReader.GetMSBuildSdkVersions(context).Should().Equal(expectedVersions);

                actualGlobalJsonPath.Should().Be(expectedGlobalJsonPath);
            }
        }

        /// <summary>
        /// Verifies that <see cref="GlobalJsonReader.GetMSBuildSdkVersions(Framework.SdkResolverContext)" /> successfully parses the specified global.json and its msbuild-sdks section but ignores entries that are invalid.
        /// </summary>
        [Theory]
        [InlineData("1")] // A number value
        [InlineData("true")] // A boolean value
        [InlineData("[ ]")] // An empty array
        [InlineData("[ { \"Item1\": \"Value1\" }, { \"Item2\": \"Value2\"} ]")] // An array with items
        [InlineData("null")] // A null value
        [InlineData("{  } ")] // Empty object
        public void GetMSBuildSdkVersions_IgnoresInvalidVersions_WhenMSBuildSdksSectionContainsInvalidValues(string objectValue)
        {
            var expectedVersions = new Dictionary<string, string>
            {
                {"Sdk1", "1.0.0"},
            };

            using (var testDirectory = TestDirectory.Create())
            {
                var context = new MockSdkResolverContext(testDirectory);
                string expectedGlobalJsonPath = Path.Combine(testDirectory, GlobalJsonReader.GlobalJsonFileName);

                File.WriteAllText(expectedGlobalJsonPath, $@"{{
  ""sdk"" : {{
    ""version"": ""1.2.300""
  }},
  ""msbuild-sdks"": {{
    ""Sdk1"": ""1.0.0"",
    ""Sdk2"": {objectValue}
}}
}}");

                var globalJsonReader = GlobalJsonReader.Instance;

                string actualGlobalJsonPath = null;

                globalJsonReader.FileRead += (_, globalJsonPath) =>
                {
                    actualGlobalJsonPath = globalJsonPath;
                };

                globalJsonReader.GetMSBuildSdkVersions(context).Should().BeEquivalentTo(expectedVersions);

                actualGlobalJsonPath.Should().Be(expectedGlobalJsonPath);
            }
        }

        /// <summary>
        /// Verifies that <see cref="GlobalJsonReader.GetMSBuildSdkVersions(Framework.SdkResolverContext)" /> logs a message when the specified global.json contains invalid JSON.
        /// </summary>
        [Fact]
        public void GetMSBuildSdkVersions_LogsMessage_WhenGlobalJsonContainsInvalidJson()
        {
            var expectedVersions = new Dictionary<string, string>
            {
                {"Sdk1", "1.0.0"},
                {"Sdk2", "2.0.0"}
            };

            using (var testDirectory = TestDirectory.Create())
            {
                var expectedGlobalJsonPath = WriteGlobalJson(testDirectory, expectedVersions, additionalContent: ", invalid JSON!");

                var context = new MockSdkResolverContext(testDirectory);

                var globalJsonReader = GlobalJsonReader.Instance;

                string actualGlobalJsonPath = null;

                globalJsonReader.FileRead += (_, globalJsonPath) =>
                {
                    actualGlobalJsonPath = globalJsonPath;
                };

                globalJsonReader.GetMSBuildSdkVersions(context).Should().BeNull();

                context.MockSdkLogger.LoggedMessages.Count.Should().Be(1);
                context.MockSdkLogger.LoggedMessages.First().Message.Should().Be(
                    $"Failed to parse \"{expectedGlobalJsonPath}\". Invalid character after parsing property name. Expected ':' but got: J. Path 'msbuild-sdks.Sdk2', line 5, position 10.");

                actualGlobalJsonPath.Should().Be(expectedGlobalJsonPath);
            }
        }

        /// <summary>
        /// Verifies that <see cref="GlobalJsonReader.GetMSBuildSdkVersions(Framework.SdkResolverContext)" /> successfully parses the specified global.json and its msbuild-sdks section.
        /// </summary>
        [Fact]
        public void GetMSBuildSdkVersions_ParsesSdkVersions_WhenGlobalJsonIsValid()
        {
            var expectedVersions = new Dictionary<string, string>
            {
                {"Sdk1", "1.0.0"},
                {"Sdk2", "2.0.0"},
            };

            using (var testDirectory = TestDirectory.Create())
            {
                string expectedGlobalJsonPath = WriteGlobalJson(testDirectory, expectedVersions);

                var context = new MockSdkResolverContext(testDirectory);

                var globalJsonReader = GlobalJsonReader.Instance;

                string actualGlobalJsonPath = null;

                globalJsonReader.FileRead += (_, globalJsonPath) =>
                {
                    actualGlobalJsonPath = globalJsonPath;
                };

                globalJsonReader.GetMSBuildSdkVersions(context).Should().Equal(expectedVersions);

                actualGlobalJsonPath.Should().Be(expectedGlobalJsonPath);
            }
        }

        /// <summary>
        /// Verifies that <see cref="GlobalJsonReader.GetMSBuildSdkVersions(Framework.SdkResolverContext)" /> loads global.json again if it changes.
        /// </summary>
        [Fact]
        public void GetMSBuildSdkVersions_ReloadsGlobalJson_WhenGlobalJsonChanges()
        {
            var expectedVersions = new Dictionary<string, string>
            {
                {"Sdk1", "1.0.0"},
                {"Sdk2", "2.0.0"},
            };

            using (var testDirectory = TestDirectory.Create())
            {
                var expectedGlobalJsonReaderPath = WriteGlobalJson(testDirectory, expectedVersions);

                var context = new MockSdkResolverContext(testDirectory);

                var globalJsonReader = GlobalJsonReader.Instance;

                Dictionary<string, int> globalJsonReadCountByPath = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                globalJsonReader.FileRead += (_, globalJsonPath) =>
                {
                    if (globalJsonReadCountByPath.ContainsKey(globalJsonPath))
                    {
                        globalJsonReadCountByPath[globalJsonPath]++;
                    }
                    else
                    {
                        globalJsonReadCountByPath[globalJsonPath] = 1;
                    }
                };

                globalJsonReader.GetMSBuildSdkVersions(context).Should().Equal(expectedVersions);

                globalJsonReadCountByPath.ContainsKey(expectedGlobalJsonReaderPath).Should().BeTrue();

                globalJsonReadCountByPath[expectedGlobalJsonReaderPath].Should().Be(1);

                Parallel.For(0, Environment.ProcessorCount * 2, _ =>
                {
                    globalJsonReader.GetMSBuildSdkVersions(context).Should().Equal(expectedVersions);
                });

                globalJsonReadCountByPath.ContainsKey(expectedGlobalJsonReaderPath).Should().BeTrue();

                globalJsonReadCountByPath[expectedGlobalJsonReaderPath].Should().Be(1);

                expectedVersions["Sdk1"] = "2.0.0;";

                string path = WriteGlobalJson(testDirectory, expectedVersions);

                File.SetLastWriteTime(path, DateTime.Now.AddMinutes(1));

                Parallel.For(0, Environment.ProcessorCount * 2, _ =>
                {
                    globalJsonReader.GetMSBuildSdkVersions(context).Should().Equal(expectedVersions);
                });

                globalJsonReadCountByPath.ContainsKey(expectedGlobalJsonReaderPath).Should().BeTrue();

                globalJsonReadCountByPath[expectedGlobalJsonReaderPath].Should().Be(2);
            }
        }

        /// <summary>
        /// Verifies that <see cref="GlobalJsonReader.GetMSBuildSdkVersions(Framework.SdkResolverContext)" /> returns <see langword="null" /> when a file is not found in a parent directory.
        /// </summary>
        [Fact]
        public void GetMSBuildSdkVersions_ReturnsNull_WhenGlobalJsonDoesNotExist()
        {
            // In some cases, a global.json exists because tests are run in the repo so a different file name must be passed in
            const string globalJsonFileName = "global.test.json";

            using (var testDirectory = TestDirectory.Create())
            {
                var context = new MockSdkResolverContext(testDirectory);

                var globalJsonReader = GlobalJsonReader.Instance;

                bool wasGlobalJsonRead = false;

                globalJsonReader.FileRead += (sender, args) =>
                {
                    wasGlobalJsonRead = true;
                };

                globalJsonReader.GetMSBuildSdkVersions(context, globalJsonFileName).Should().BeNull();

                wasGlobalJsonRead.Should().BeFalse();
            }
        }

        /// <summary>
        /// Verifies that <see cref="GlobalJsonReader.GetMSBuildSdkVersions(Framework.SdkResolverContext)" /> returns <see langword="null" /> when the specified global.json is empty or does not contain an msbuild-sdks section.
        /// </summary>
        [Theory]
        [InlineData("{ }")]
        [InlineData("  { }  ")]
        [InlineData("// No actual content, only a comment")]
        [InlineData("")]
        public void GetMSBuildSdkVersions_ReturnsNull_WhenGlobalJsonIsEmpty(string contents)
        {
            using (var testDirectory = TestDirectory.Create())
            {
                File.WriteAllText(Path.Combine(testDirectory.Path, GlobalJsonReader.GlobalJsonFileName), contents);

                var context = new MockSdkResolverContext(testDirectory);

                var globalJsonReader = GlobalJsonReader.Instance;

                bool wasGlobalJsonRead = false;

                globalJsonReader.FileRead += (sender, args) =>
                {
                    wasGlobalJsonRead = true;
                };

                globalJsonReader.GetMSBuildSdkVersions(context).Should().BeNull();

                wasGlobalJsonRead.Should().BeTrue();
            }
        }

        /// <summary>
        /// Verifies that <see cref="GlobalJsonReader.GetMSBuildSdkVersions(Framework.SdkResolverContext)" /> returns <see langword="null" /> when the specified global.json contains valid JSON but the msbuild-sdks section isn't correctly declared.
        /// </summary>
        [Theory]
        [InlineData("1")] // A number value
        [InlineData("\"Value\"")] // A string value
        [InlineData("true")] // A boolean value
        [InlineData("[ ]")] // An empty array
        [InlineData("[ { \"Item1\": \"Value1\" }, { \"Item2\": \"Value2\"} ]")] // An array with items
        [InlineData("null")] // A null value
        [InlineData("{  } ")] // Empty object
        public void GetMSBuildSdkVersions_ReturnsNull_WhenMSBuildSdksSectionIsNotDeclaredCorrectly(string msbuildSdksSection)
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var context = new MockSdkResolverContext(testDirectory);

                File.WriteAllText(Path.Combine(testDirectory, GlobalJsonReader.GlobalJsonFileName), $@"{{
  ""msbuild-sdks"": {msbuildSdksSection}
}}");

                var globalJsonReader = GlobalJsonReader.Instance;

                bool wasGlobalJsonRead = false;

                globalJsonReader.FileRead += (sender, args) =>
                {
                    wasGlobalJsonRead = true;
                };

                globalJsonReader.GetMSBuildSdkVersions(context).Should().BeNull();

                wasGlobalJsonRead.Should().BeTrue();
            }
        }

        /// <summary>
        /// Verifies that <see cref="GlobalJsonReader.GetMSBuildSdkVersions(Framework.SdkResolverContext)" /> returns <see langword="null" /> when the <see cref="Framework.SdkResolverContext.SolutionFilePath" /> and <see cref="Framework.SdkResolverContext.ProjectFilePath" /> is null.
        /// </summary>
        [Fact]
        public void GetMSBuildSdkVersions_ReturnsNull_WhenSolutionFilePathAndProjectFilePathIsNull()
        {
            var context = new MockSdkResolverContext(projectPath: null, solutionPath: null);

            var globalJsonReader = GlobalJsonReader.Instance;

            bool wasGlobalJsonRead = false;

            globalJsonReader.FileRead += (sender, args) =>
            {
                wasGlobalJsonRead = true;
            };

            globalJsonReader.GetMSBuildSdkVersions(context).Should().BeNull();

            wasGlobalJsonRead.Should().BeFalse();
        }

        /// <summary>
        /// Verifies that <see cref="GlobalJsonReader.GetMSBuildSdkVersions(Framework.SdkResolverContext)" /> successfully parses the specified global.json if it contains comments.
        /// </summary>
        [Fact]
        public void GetMSBuildSdkVersions_Succeeds_WhenGlobalJsonContainsComments()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                File.WriteAllText(
                    Path.Combine(testDirectory, GlobalJsonReader.GlobalJsonFileName),
                    @"// Comment before content
// Comment before content
{ // Comment on same line as token
// Comment after start token
// Comment after start token
  ""unrelated-section-before"" : {
    // Comment in unrelated section
    // Comment in unrelated section
    ""property1"": ""value1""
  }, // comment after token, whitespace below is intentional

  ""msbuild-sdks"": {  // Comment after token
    /* This is another comment */
    // Comment before value
    ""Sdk1"": ""1.0.0"", // Comment after value
    // Comment between value
    // Comment between value
    ""Sdk2"": ""2.0.0"" // Comment after value
    // Comment after value
    // Comment after value
  }, // Comment after end token
  ""unrelated-section-after"" : {
    // Comment in unrelated section
    // Comment in unrelated section
    ""property1"": ""value1""
  }, // comment after token
} // Comment after end token
// Comment at end of file
// Comment at end of file");

                var context = new MockSdkResolverContext(testDirectory);

                var globalJsonReader = GlobalJsonReader.Instance;

                bool wasGlobalJsonRead = false;

                globalJsonReader.FileRead += (sender, args) =>
                {
                    wasGlobalJsonRead = true;
                };

                globalJsonReader.GetMSBuildSdkVersions(context).Should().BeEquivalentTo(new Dictionary<string, string>
                {
                    ["Sdk1"] = "1.0.0",
                    ["Sdk2"] = "2.0.0"
                });

                wasGlobalJsonRead.Should().BeTrue();
            }
        }

        /// <summary>
        /// Verifies that the <see cref="GlobalJsonReader.GetStartingPath(Framework.SdkResolverContext)" /> method returns the project path if the solution path is null or whitespace.
        /// </summary>
        /// <param name="solutionPath"></param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("     ")]
        public void GetStartingPath_ReturnsProjectPath_WhenSolutionPathIsNullOrWhitespace(string solutionPath)
        {
            const string projectPath = "PROJECT_PATH";

            var context = new MockSdkResolverContext(projectPath, solutionPath);

            GlobalJsonReader.GetStartingPath(context).Should().Be(projectPath);
        }

        /// <summary>
        /// Verifies that the <see cref="GlobalJsonReader.GetStartingPath(Framework.SdkResolverContext)" /> method returns the solution path if it is not null or whitespace.
        /// </summary>
        /// <param name="solutionPath"></param>
        [Fact]
        public void GetStartingPath_ReturnsSolutionPath_WhenSolutionPathIsNotNullOrWhitespace()
        {
            const string solutionPath = "SOLUTION_PATH";

            var context = new MockSdkResolverContext(projectPath: null, solutionPath);

            GlobalJsonReader.GetStartingPath(context).Should().Be(solutionPath);
        }

        /// <summary>
        /// Verifies that the <see cref="GlobalJsonReader.GetStartingPath(Framework.SdkResolverContext)" /> method returns the project path if the solution path is null or whitespace.
        /// </summary>
        /// <param name="solutionPath"></param>
        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData("", "")]
        [InlineData(null, "")]
        [InlineData("     ", "     ")]
        public void GetStartingPath_ReturnsNull_WhenSolutionPathAndProjectPathIsNullOrWhitespace(string solutionPath, string projectPath)
        {
            var context = new MockSdkResolverContext(projectPath, solutionPath);

            GlobalJsonReader.GetStartingPath(context).Should().BeNull();
        }

        /// <summary>
        /// Verifies that <see cref="GlobalJsonReader.TryGetPathOfFileAbove(string, DirectoryInfo, out FileInfo)" /> return <see langword="false" /> when a file could not be found.
        /// </summary>
        [Fact]
        public void TryGetPathOfFileAbove_ReturnsFalse_WhenFileIsNotFound()
        {
            const string filename = "test.txt";

            using (var testDirectory = TestDirectory.Create())
            {
                DirectoryInfo startingDirectory = Directory.CreateDirectory(Path.Combine(testDirectory, "a", "b", "c", "d"));

                var actualFilePath = new FileInfo(Path.Combine(testDirectory, filename));

                bool result = GlobalJsonReader.TryGetPathOfFileAbove(file: filename, startingDirectory, out FileInfo fullPath);

                result.Should().BeFalse();
                fullPath.Should().BeNull();
            }
        }

        /// <summary>
        /// Verifies that <see cref="GlobalJsonReader.TryGetPathOfFileAbove(string, DirectoryInfo, out FileInfo)" /> return <see langword="false" /> when specifying <see langword="null" /> for the file parameter.
        /// </summary>
        [Fact]
        public void TryGetPathOfFileAbove_ReturnsFalse_WhenFileIsNull()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                bool result = GlobalJsonReader.TryGetPathOfFileAbove(file: null, new DirectoryInfo(testDirectory), out FileInfo fullPath);

                result.Should().BeFalse();
                fullPath.Should().BeNull();
            }
        }

        /// <summary>
        /// Verifies that <see cref="GlobalJsonReader.TryGetPathOfFileAbove(string, DirectoryInfo, out FileInfo)" /> return <see langword="false" /> when specifying a starting directory that does not exist.
        /// </summary>
        [Fact]
        public void TryGetPathOfFileAbove_ReturnsFalse_WhenStartingDirectoryDoesNotExist()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                File.WriteAllText(Path.Combine(testDirectory, "test.txt"), string.Empty);

                bool result = GlobalJsonReader.TryGetPathOfFileAbove(file: "test.txt", new DirectoryInfo(Path.Combine(testDirectory, "DoesNotExist")), out FileInfo fullPath);

                result.Should().BeFalse();
                fullPath.Should().BeNull();
            }
        }

        /// <summary>
        /// Verifies that <see cref="GlobalJsonReader.TryGetPathOfFileAbove(string, DirectoryInfo, out FileInfo)" /> return <see langword="false" /> when specifying <see langword="null" /> for the startingDirectory parameter.
        /// </summary>
        [Fact]
        public void TryGetPathOfFileAbove_ReturnsFalse_WhenStartingDirectoryIsNull()
        {
            bool result = GlobalJsonReader.TryGetPathOfFileAbove(file: "Test.txt", startingDirectory: null, out FileInfo fullPath);

            result.Should().BeFalse();
            fullPath.Should().BeNull();
        }

        /// <summary>
        /// Verifies that <see cref="GlobalJsonReader.TryGetPathOfFileAbove(string, DirectoryInfo, out FileInfo)" /> return <see langword="true" /> and the path to the file when one is found.
        /// </summary>
        [Fact]
        public void TryGetPathOfFileAbove_ReturnsTrue_WhenFileIsFound()
        {
            const string filename = "test.txt";

            using (var testDirectory = TestDirectory.Create())
            {
                DirectoryInfo startingDirectory = Directory.CreateDirectory(Path.Combine(testDirectory, "a", "b", "c", "d"));

                var actualFilePath = new FileInfo(Path.Combine(testDirectory, filename));

                File.WriteAllText(actualFilePath.FullName, string.Empty);

                bool result = GlobalJsonReader.TryGetPathOfFileAbove(file: filename, startingDirectory, out FileInfo fullPath);

                result.Should().BeTrue();
                fullPath.Should().NotBeNull();
                fullPath.FullName.Should().Be(actualFilePath.FullName);
            }
        }

        /// <summary>
        /// Writes a global.json file with the specified versions in the msbuild-sdks section.
        /// </summary>
        /// <param name="directory">The directory to create the global.json file in.</param>
        /// <param name="sdkVersions">A <see cref="Dictionary{TKey, TValue}" /> containing MSBuild project SDK versions.</param>
        /// <param name="additionalContent">An optional string to include in the msbuild-sdks section.</param>
        /// <returns></returns>
        private static string WriteGlobalJson(string directory, Dictionary<string, string> sdkVersions, string additionalContent = "")
        {
            string path = Path.Combine(directory, GlobalJsonReader.GlobalJsonFileName);

            using (var writer = File.CreateText(path))
            {
                writer.WriteLine("{");
                if (sdkVersions != null)
                {
                    writer.WriteLine("    \"msbuild-sdks\": {");
                    writer.WriteLine(string.Join($",{Environment.NewLine}        ", sdkVersions.Select(i => $"\"{i.Key}\": \"{i.Value}\"")));
                    if (!string.IsNullOrWhiteSpace(additionalContent))
                    {
                        writer.Write(additionalContent);
                    }
                    writer.WriteLine("    }");
                }

                writer.WriteLine("}");
            }

            return path;
        }
    }
}
