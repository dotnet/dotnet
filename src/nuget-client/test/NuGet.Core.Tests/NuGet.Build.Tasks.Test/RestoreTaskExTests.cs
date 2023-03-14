// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                    var arguments = task.GetCommandLineArguments().ToList();
                    arguments.Should().BeEquivalentTo(
#if IS_CORECLR
                    Path.ChangeExtension(typeof(RestoreTaskEx).Assembly.Location, ".Console.dll"),
#endif
                    "CleanupAssetsForUnsupportedProjects=True;DisableParallel=True;Force=True;ForceEvaluate=True;HideWarningsAndErrors=True;IgnoreFailedSources=True;Interactive=True;NoCache=True;Recursive=True;RestorePackagesConfig=True",
#if IS_CORECLR
                    Path.Combine(msbuildBinPath, "MSBuild.dll"),
#else
                    Path.Combine(msbuildBinPath, "MSBuild.exe"),
#endif
                    projectPath,
                        $"Property1=Value1;Property2=  Value2  ;ExcludeRestorePackageImports=True;OriginalMSBuildStartupDirectory={testDirectory}");
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
