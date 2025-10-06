// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Build.Tasks.Test
{
    public class RestoreTaskExTests
    {
        [Fact]
        public void Cancel_WhenCanceled_CancellationTokenSourceIsCancellationRequestedIsTrue()
        {
            using (var task = new RestoreTaskEx())
            {
                task.Cancel();

                task._cancellationTokenSource.IsCancellationRequested.Should().BeTrue();
            }
        }

        [Theory]
        [InlineData(true, "", null)]
        [InlineData(true, null, null)]
        [InlineData(true, "User;Value", "User%3BValue")]
        [InlineData(false, "", null)]
        [InlineData(false, null, null)]
        [InlineData(false, "UserValue", null)]
        [InlineData(null, "", null)]
        [InlineData(null, null, null)]
        [InlineData(null, "User;Value", "User%3BValue")]
        public void GetCommandLineArguments_WhenBinaryLoggerParametersSpecified_CorrectValuesReturned(bool? enabled, string parameters, string expected)
        {
            using (var testDirectory = TestDirectory.Create())
            {
                string msbuildBinPath = Path.Combine(testDirectory, "MSBuild", "Current", "Bin");
                string projectPath = Path.Combine(testDirectory, "src", "project1", "project1.csproj");

                var globalProperties = new Dictionary<string, string>();

                var buildEngine = new TestBuildEngine(globalProperties);

                using (var task = new RestoreTaskEx
                {
                    BuildEngine = buildEngine,
                    BinaryLoggerParameters = parameters,
                    MSBuildBinPath = msbuildBinPath,
                    ProjectFullPath = projectPath,
                    MSBuildStartupDirectory = testDirectory,
                })
                {
                    if (enabled.HasValue)
                    {
                        task.EnableBinaryLogger = enabled.HasValue ? enabled.Value.ToString() : null;
                    }

                    string arguments = task.GetCommandLineArguments(globalProperties);

                    arguments.Should().Be(StaticGraphRestoreTaskBase.CreateArgumentString(GetExpectedArguments(msbuildBinPath, projectPath, enabled, parameters, expected)));
                }
            }

            static IEnumerable<string> GetExpectedArguments(string msbuildBinPath, string projectPath, bool? enabled, string parameters, string expected)
            {
#if IS_CORECLR
                yield return Path.ChangeExtension(typeof(RestoreTaskEx).Assembly.Location, ".Console.dll");
#endif
                if (enabled == false)
                {
                    yield return "Recursive=False;CleanupAssetsForUnsupportedProjects=True;DisableParallel=False;Force=False;ForceEvaluate=False;HideWarningsAndErrors=False;IgnoreFailedSources=False;Interactive=False;NoCache=False;NoHttpCache=False;RestorePackagesConfig=False";
                }

                if (enabled == true)
                {
                    if (!string.IsNullOrEmpty(parameters))
                    {
                        yield return $"Recursive=False;EnableBinaryLogger=True;BinaryLoggerParameters={expected};CleanupAssetsForUnsupportedProjects=True;DisableParallel=False;Force=False;ForceEvaluate=False;HideWarningsAndErrors=False;IgnoreFailedSources=False;Interactive=False;NoCache=False;NoHttpCache=False;RestorePackagesConfig=False";
                    }
                    else
                    {
                        yield return "Recursive=False;EnableBinaryLogger=True;CleanupAssetsForUnsupportedProjects=True;DisableParallel=False;Force=False;ForceEvaluate=False;HideWarningsAndErrors=False;IgnoreFailedSources=False;Interactive=False;NoCache=False;NoHttpCache=False;RestorePackagesConfig=False";
                    }
                }

                else if (enabled == null)
                {
                    if (!string.IsNullOrEmpty(parameters))
                    {
                        yield return $"Recursive=False;EnableBinaryLogger=True;BinaryLoggerParameters={expected};CleanupAssetsForUnsupportedProjects=True;DisableParallel=False;Force=False;ForceEvaluate=False;HideWarningsAndErrors=False;IgnoreFailedSources=False;Interactive=False;NoCache=False;NoHttpCache=False;RestorePackagesConfig=False";
                    }
                    else
                    {
                        yield return "Recursive=False;CleanupAssetsForUnsupportedProjects=True;DisableParallel=False;Force=False;ForceEvaluate=False;HideWarningsAndErrors=False;IgnoreFailedSources=False;Interactive=False;NoCache=False;NoHttpCache=False;RestorePackagesConfig=False";
                    }
                }
#if IS_CORECLR
                yield return Path.Combine(msbuildBinPath, "MSBuild.dll");
#else
                yield return Path.Combine(msbuildBinPath, "MSBuild.exe");
#endif
                yield return projectPath;

                yield return $"";
            }
        }

        [Theory]
        [InlineData("something", true)]
        [InlineData("  ", false)]
        [InlineData(null, false)]
        public void GetCommandLineArguments_WhenEmbedFilesInBinlogSpecified_CorrectValuesReturned(string embedFilesInBinlogValue, bool expectedToBeSet)
        {
            using (var testDirectory = TestDirectory.Create())
            {
                string msbuildBinPath = Path.Combine(testDirectory, "MSBuild", "Current", "Bin");
                string projectPath = Path.Combine(testDirectory, "src", "project1", "project1.csproj");

                var globalProperties = new Dictionary<string, string>();

                var buildEngine = new TestBuildEngine(globalProperties);

                using (var task = new RestoreTaskEx
                {
                    BuildEngine = buildEngine,
                    DisableParallel = true,
                    Force = true,
                    ForceEvaluate = true,
                    HideWarningsAndErrors = true,
                    IgnoreFailedSources = true,
                    Interactive = true,
                    MSBuildBinPath = msbuildBinPath,
                    NoCache = true,
                    NoHttpCache = true,
                    ProjectFullPath = projectPath,
                    Recursive = true,
                    RestorePackagesConfig = true,
                    MSBuildStartupDirectory = testDirectory,
                    EmbedFilesInBinlog = embedFilesInBinlogValue
                })
                {
                    string arguments = task.GetCommandLineArguments(globalProperties);

                    arguments.Should().Be(StaticGraphRestoreTaskBase.CreateArgumentString(GetExpectedArguments(msbuildBinPath, projectPath)));
                }
            }

            IEnumerable<string> GetExpectedArguments(string msbuildBinPath, string projectPath)
            {
#if IS_CORECLR
                yield return Path.ChangeExtension(typeof(RestoreTaskEx).Assembly.Location, ".Console.dll");
#endif
                if (expectedToBeSet)
                {
                    yield return "Recursive=True;CleanupAssetsForUnsupportedProjects=True;DisableParallel=True;Force=True;ForceEvaluate=True;HideWarningsAndErrors=True;IgnoreFailedSources=True;Interactive=True;NoCache=True;NoHttpCache=True;RestorePackagesConfig=True;EmbedFilesInBinlog=" + embedFilesInBinlogValue.ToString();
                }
                else
                {
                    yield return "Recursive=True;CleanupAssetsForUnsupportedProjects=True;DisableParallel=True;Force=True;ForceEvaluate=True;HideWarningsAndErrors=True;IgnoreFailedSources=True;Interactive=True;NoCache=True;NoHttpCache=True;RestorePackagesConfig=True";
                }
#if IS_CORECLR
                yield return Path.Combine(msbuildBinPath, "MSBuild.dll");
#else
                yield return Path.Combine(msbuildBinPath, "MSBuild.exe");
#endif
                yield return projectPath;

                yield return $"";
            }
        }

        [Fact]
        public void GetCommandLineArguments_WhenOptionsSpecified_CorrectValuesReturned()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                string msbuildBinPath = Path.Combine(testDirectory, "MSBuild", "Current", "Bin");
                string projectPath = Path.Combine(testDirectory, "src", "project1", "project1.csproj");

                var globalProperties = new Dictionary<string, string>
                {
                    ["Property1"] = "Value1",
                    ["Property2"] = "  Value2  "
                };

                var buildEngine = new TestBuildEngine(globalProperties);

                using (var task = new RestoreTaskEx
                {
                    BuildEngine = buildEngine,
                    DisableParallel = true,
                    Force = true,
                    ForceEvaluate = true,
                    HideWarningsAndErrors = true,
                    IgnoreFailedSources = true,
                    Interactive = true,
                    MSBuildBinPath = msbuildBinPath,
                    NoCache = true,
                    NoHttpCache = true,
                    ProjectFullPath = projectPath,
                    Recursive = true,
                    RestorePackagesConfig = true,
                    MSBuildStartupDirectory = testDirectory,
                })
                {
                    string arguments = task.GetCommandLineArguments(globalProperties);

                    arguments.Should().Be(StaticGraphRestoreTaskBase.CreateArgumentString(GetExpectedArguments(msbuildBinPath, projectPath)));
                }
            }

            IEnumerable<string> GetExpectedArguments(string msbuildBinPath, string projectPath)
            {
#if IS_CORECLR
                yield return Path.ChangeExtension(typeof(RestoreTaskEx).Assembly.Location, ".Console.dll");
#endif
                yield return "Recursive=True;CleanupAssetsForUnsupportedProjects=True;DisableParallel=True;Force=True;ForceEvaluate=True;HideWarningsAndErrors=True;IgnoreFailedSources=True;Interactive=True;NoCache=True;NoHttpCache=True;RestorePackagesConfig=True";
#if IS_CORECLR
                yield return Path.Combine(msbuildBinPath, "MSBuild.dll");
#else
                yield return Path.Combine(msbuildBinPath, "MSBuild.exe");
#endif
                yield return projectPath;

                yield return $"Property1=Value1;Property2=  Value2  ";
            }
        }

        /// <summary>
        /// Verifies that the <see cref="RestoreTaskEx.GetGlobalProperties" /> returns the global properties plus any extra ones set by NuGet.
        /// </summary>
        [Fact]
        public void GetGlobalProperties_ExtraGlobalProperties_AreSetCorrectly()
        {
            const string MSBuildStartupDirectory = @"C:\something";

            Dictionary<string, string> globalProperties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Property1"] = "Value1",
                ["Property2"] = "Value2"
            };

            using (var task = new RestoreTaskEx()
            {
                BuildEngine = new TestBuildEngine(globalProperties),
                MSBuildStartupDirectory = MSBuildStartupDirectory
            })
            {
                var actual = task.GetGlobalProperties().ToDictionary(i => i.Key, i => i.Value, StringComparer.OrdinalIgnoreCase);

                actual.TryGetValue("Property1", out string value1).Should().BeTrue();
                value1.Should().Be("Value1");

                actual.TryGetValue("Property2", out string value2).Should().BeTrue();
                value2.Should().Be("Value2");

                actual.TryGetValue("ExcludeRestorePackageImports", out string excludeRestorePackageImports).Should().BeTrue();
                excludeRestorePackageImports.Should().Be(bool.TrueString);

                actual.TryGetValue("OriginalMSBuildStartupDirectory", out string originalMSBuildStartupDirectory).Should().BeTrue();
                originalMSBuildStartupDirectory.Should().Be(MSBuildStartupDirectory);
            }
        }

        [Fact]
        public void GetProcessFileName_WhenCalled_ReturnsCorrectValue()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                string msbuildBinPath = Path.Combine(testDirectory, "MSBuild", "Current", "Bin");

                using (var task = new RestoreTaskEx
                {
                    MSBuildBinPath = msbuildBinPath
                })
                {
#if IS_CORECLR
                    task.GetProcessFileName(null).Should().Be(Path.Combine(testDirectory, "MSBuild", "dotnet"));
#else
                    task.GetProcessFileName(null).Should().Be(Path.ChangeExtension(typeof(RestoreTaskEx).Assembly.Location, ".Console.exe"));
#endif
                }
            }
        }

        [Fact]
        public void GetProcessFileName_WithExePathParameter_ReturnsCorrectValue()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                string msbuildBinPath = Path.Combine(testDirectory, "MSBuild", "Current", "Bin");
                string exePath = Path.Combine(testDirectory, "override.exe");

                using (var task = new RestoreTaskEx
                {
                    MSBuildBinPath = msbuildBinPath
                })
                {
                    task.GetProcessFileName(exePath).Should().Be(exePath);
                }
            }
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("*Undefined*", false)]
        [InlineData(@"C:\foo\bar.sln", true)]
        public void IsSolutionPathDefined_WhenDifferentValuesSpecified_CorrectValueReturned(string value, bool expected)
        {
            using (var task = new RestoreTaskEx
            {
                SolutionPath = value
            })
            {
                if (expected)
                {
                    task.IsSolutionPathDefined.Should().BeTrue();
                }
                else
                {
                    task.IsSolutionPathDefined.Should().BeFalse();
                }
            }
        }

        /// <summary>
        /// Verifies that the <see cref="StaticGraphRestoreTaskBase.WriteGlobalProperties(Stream, Dictionary{string, string})" /> method serializes the global properties correctly.
        /// </summary>
        /// <param name="count">The size of the dictionary to test with.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        public void WriteGlobalProperties_WhenGivenDictionary_Succeeds(int count)
        {
            var globalProperties = new Dictionary<string, string>(count);

            for (int i = 0; i < count; i++)
            {
                globalProperties.Add($"Property{i}", $"Value{i}");
            }

            using var stream = new MemoryStream();

            using var writer = new BinaryWriter(stream);

            StaticGraphRestoreTaskBase.WriteGlobalProperties(writer, globalProperties);

            stream.Position = 0;

            using var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);

            reader.ReadInt32().Should().Be(count);

            for (int i = 0; i < count; i++)
            {
                reader.ReadString().Should().Be($"Property{i}");
                reader.ReadString().Should().Be($"Value{i}");
            }
        }
    }
}
