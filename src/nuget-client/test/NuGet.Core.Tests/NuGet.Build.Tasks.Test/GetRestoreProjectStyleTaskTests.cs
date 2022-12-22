// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using NuGet.Configuration;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Build.Tasks.Test
{
    public class GetRestoreProjectStyleTaskTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("None")]
        [InlineData("SomethingRandom")]
        public void Execute_WhenNothingMatches_ReturnsUnknown(string restoreStyle)
        {
            var buildEngine = new TestBuildEngine();

            var task = new GetRestoreProjectStyleTask
            {
                BuildEngine = buildEngine,
                RestoreProjectStyle = restoreStyle,
                ProjectJsonPath = string.Empty,
                HasPackageReferenceItems = false,
                MSBuildProjectName = "ProjectA",
                MSBuildProjectDirectory = "SomeDirectory"
            };

            task.Execute().Should().BeTrue();

            task.ProjectStyle.Should().Be(ProjectStyle.Unknown);
            task.IsPackageReferenceCompatibleProjectStyle.Should().BeFalse();
        }

        [Fact]
        public void Execute_WhenProjectHasPackageReferenceItems_ReturnsPackageReference()
        {
            var buildEngine = new TestBuildEngine();

            var task = new GetRestoreProjectStyleTask
            {
                BuildEngine = buildEngine,
                HasPackageReferenceItems = true
            };

            task.Execute().Should().BeTrue();

            task.ProjectStyle.Should().Be(ProjectStyle.PackageReference);
            task.IsPackageReferenceCompatibleProjectStyle.Should().BeTrue();
        }

        [Theory]
        [InlineData("packages.config")]
        [InlineData("packages.ProjectA.config")]
        public void Execute_WhenProjectHasPackagesConfigFile_ReturnsPackagesConfig(string packagesConfigFileName)
        {
            using (var testDirectory = TestDirectory.Create())
            {
                File.WriteAllText(Path.Combine(testDirectory, packagesConfigFileName), string.Empty);

                var buildEngine = new TestBuildEngine();

                var task = new GetRestoreProjectStyleTask
                {
                    BuildEngine = buildEngine,
                    MSBuildProjectDirectory = testDirectory,
                    MSBuildProjectName = "ProjectA"
                };

                task.Execute().Should().BeTrue();

                task.ProjectStyle.Should().Be(ProjectStyle.PackagesConfig);
                task.IsPackageReferenceCompatibleProjectStyle.Should().BeFalse();
            }
        }

        [Fact]
        public void Execute_WhenProjectJsonPathSpecified_ReturnsProjectJson()
        {
            var buildEngine = new TestBuildEngine();

            var task = new GetRestoreProjectStyleTask
            {
                BuildEngine = buildEngine,
                ProjectJsonPath = "SomePath"
            };

            task.Execute().Should().BeTrue();

            task.ProjectStyle.Should().Be(ProjectStyle.ProjectJson);
            task.IsPackageReferenceCompatibleProjectStyle.Should().BeFalse();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Execute_WhenProjectStyleSupplied_ReturnsSuppliedProjectStyle(bool lowerCase)
        {
            foreach (var projectStyle in Enum.GetValues(typeof(ProjectStyle)).Cast<ProjectStyle>())
            {
                var buildEngine = new TestBuildEngine();

                var task = new GetRestoreProjectStyleTask
                {
                    BuildEngine = buildEngine,
                    RestoreProjectStyle = lowerCase ? projectStyle.ToString().ToLower() : projectStyle.ToString()
                };

                task.Execute().Should().BeTrue();

                task.ProjectStyle.Should().Be(projectStyle);
                if (projectStyle == ProjectStyle.PackageReference || projectStyle == ProjectStyle.DotnetToolReference)
                {
                    task.IsPackageReferenceCompatibleProjectStyle.Should().BeTrue();
                }
                else
                {
                    task.IsPackageReferenceCompatibleProjectStyle.Should().BeFalse();
                }
            }
        }

        [Fact]
        public void Execute_WhenUserSuppliedValueOverridesDefault_ReturnsUserSuppliedProjectStyle()
        {
            var expected = ProjectStyle.Standalone;

            using (var testDirectory = TestDirectory.Create())
            {
                File.WriteAllText(Path.Combine(testDirectory, NuGetConstants.PackageReferenceFile), string.Empty);

                var buildEngine = new TestBuildEngine();

                var task = new GetRestoreProjectStyleTask
                {
                    BuildEngine = buildEngine,
                    RestoreProjectStyle = expected.ToString(),
                    ProjectJsonPath = "Some value",
                    HasPackageReferenceItems = true,
                    MSBuildProjectName = "ProjectA",
                    MSBuildProjectDirectory = "SomeDirectory"
                };

                task.Execute().Should().BeTrue();

                task.ProjectStyle.Should().Be(expected);
                task.IsPackageReferenceCompatibleProjectStyle.Should().BeFalse();
            }
        }
    }
}
