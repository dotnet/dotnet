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
                    ProjectFullPath = projectPath,
                    Recursive = true,
                    RestorePackagesConfig = true,
                    MSBuildStartupDirectory = testDirectory,
                })
                {
                    using var stream = new MemoryStream();
                    using var streamWriter = new StreamWriter(stream, Encoding.UTF8);

                    task.WriteArguments(streamWriter);

                    string actualArguments = task.GetCommandLineArguments(msbuildBinPath);

                    string[] expectedArguments = new[]
                    {
#if IS_CORECLR
                        Path.ChangeExtension(typeof(RestoreTaskEx).Assembly.Location, ".Console.dll"),
                        Path.Combine(msbuildBinPath, "MSBuild.dll"),
#else
                        Path.Combine(msbuildBinPath, "MSBuild.exe"),
#endif
                        projectPath
                    };

                    actualArguments.Should().Be($"\"{string.Join("\" \"", expectedArguments)}\"");

                    stream.Position = 0;

                    StaticGraphRestoreArguments arguments = StaticGraphRestoreArguments.Read(stream, streamWriter.Encoding);

                    arguments.Options.Should().BeEquivalentTo(new Dictionary<string, string>()
                    {
                        [nameof(RestoreTaskEx.CleanupAssetsForUnsupportedProjects)] = task.CleanupAssetsForUnsupportedProjects.ToString(),
                        [nameof(RestoreTaskEx.DisableParallel)] = task.DisableParallel.ToString(),
                        [nameof(RestoreTaskEx.Force)] = task.Force.ToString(),
                        [nameof(RestoreTaskEx.ForceEvaluate)] = task.ForceEvaluate.ToString(),
                        [nameof(RestoreTaskEx.HideWarningsAndErrors)] = task.HideWarningsAndErrors.ToString(),
                        [nameof(RestoreTaskEx.IgnoreFailedSources)] = task.IgnoreFailedSources.ToString(),
                        [nameof(RestoreTaskEx.Interactive)] = task.Interactive.ToString(),
                        [nameof(RestoreTaskEx.NoCache)] = task.NoCache.ToString(),
                        [nameof(RestoreTaskEx.Recursive)] = task.Recursive.ToString(),
                        [nameof(RestoreTaskEx.RestorePackagesConfig)] = task.RestorePackagesConfig.ToString(),
                    });

                    arguments.GlobalProperties.Should().Contain(globalProperties);
                }
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
    }
}
