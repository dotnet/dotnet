// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.Build.Framework;
using Xunit;

namespace NuGet.Build.Tasks.Test
{
    /// <summary>
    /// Verifies functionality in the <see cref="GetRestorePackageReferencesTask" /> class.
    /// </summary>
    public class GetRestorePackageReferencesTaskTests
    {
        /// <summary>
        /// Verifies that when a package reference is specified more than one time, the first version specified is returned by <see cref="GetRestorePackageReferencesTask "/> and the duplicates are ignored.
        /// </summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Execute_EmptyTargetFrameworks_ReturnsValueWithNoTargetFrameworks(string targetFrameworks)
        {
            // Arrange
            var task = new GetRestorePackageReferencesTask()
            {
                BuildEngine = new TestBuildEngine(),
                ProjectUniqueName = "MyProj",
                TargetFrameworks = targetFrameworks,
                PackageReferences = new ITaskItem[]
                {
                    new MockTaskItem("X")
                    {
                        ["Version"] = "[1.0.0]"
                    },
                }
            };

            // Act
            var result = task.Execute();

            // Assert
            Assert.True(result, "Task failed");
            Assert.Equal(1, task.RestoreGraphItems.Length);
            Assert.Equal("X", task.RestoreGraphItems[0].GetMetadata("Id"));
            Assert.Equal("[1.0.0]", task.RestoreGraphItems[0].GetMetadata("VersionRange"));
            Assert.False(task.RestoreGraphItems[0].MetadataNames.Cast<string>().Any(i => i.Equals("TargetFrameworks")), "TargetFrameworks property should not exist");
        }

        /// <summary>
        /// Verifies that when a package reference is specified more than one time, the first version specified is returned by <see cref="GetRestorePackageReferencesTask "/> and the duplicates are ignored.
        /// </summary>
        [Fact]
        public void Execute_WithDuplicates_FirstVersionWins()
        {
            // Arrange
            var task = new GetRestorePackageReferencesTask()
            {
                BuildEngine = new TestBuildEngine(),
                ProjectUniqueName = "MyProj",
                TargetFrameworks = "netstandard2.0",
                PackageReferences = new ITaskItem[]
                {
                    new MockTaskItem("X")
                    {
                        ["Version"] = "[1.0.0]"
                    },
                    new MockTaskItem("X")
                    {
                        ["Version"] = "[2.0.0]"
                    }
                }
            };

            // Act
            var result = task.Execute();

            // Assert
            Assert.True(result, "Task failed");
            Assert.Equal(1, task.RestoreGraphItems.Length);
            Assert.Equal("X", task.RestoreGraphItems[0].GetMetadata("Id"));
            Assert.Equal("[1.0.0]", task.RestoreGraphItems[0].GetMetadata("VersionRange"));
        }

        /// <summary>
        /// Verifies that additional metadata like IncludeAssets and GeneratePath property are correctly returned by <see cref="GetRestorePackageReferencesTask "/> when they are specified.
        /// </summary>
        [Theory]
        [InlineData("IncludeAssets", "All")]
        [InlineData("ExcludeAssets", "All")]
        [InlineData("PrivateAssets", "All")]
        [InlineData("NoWarn", "Something")]
        [InlineData("IsImplicitlyDefined", "true")]
        [InlineData("GeneratePathProperty", "true")]
        [InlineData("Aliases", "Something")]
        [InlineData("VersionOverride", "2.0.0")]
        public void Execute_WithExtraMetadata_ValuesExistIfSpecified(string propertyName, string expectedValue)
        {
            // Arrange
            var task = new GetRestorePackageReferencesTask()
            {
                BuildEngine = new TestBuildEngine(),
                ProjectUniqueName = "MyProj",
                TargetFrameworks = "netstandard2.0",
                PackageReferences = new ITaskItem[]
                {
                    new MockTaskItem("X")
                    {
                        ["Version"] = "1.0.0",
                        [propertyName] = expectedValue
                    }
                }
            };

            // Act
            var result = task.Execute();

            // Assert
            Assert.True(result, "Task failed");
            Assert.Equal(1, task.RestoreGraphItems.Length);
            Assert.Equal("X", task.RestoreGraphItems[0].GetMetadata("Id"));
            Assert.Equal("1.0.0", task.RestoreGraphItems[0].GetMetadata("VersionRange"));

            foreach (var item in new[] { "IncludeAssets", "ExcludeAssets", "PrivateAssets", "NoWarn", "IsImplicitlyDefined", "GeneratePathProperty", "Aliases", "VersionOverride" })
            {
                string actualValue = task.RestoreGraphItems[0].GetMetadata(item);

                Assert.Equal(item == propertyName ? expectedValue : string.Empty, actualValue);
            }
        }

        /// <summary>
        /// Verifies that when a package reference does not have an ID specified it is ignored and not returned by <see cref="GetRestorePackageReferencesTask "/>.
        /// </summary>
        [Fact]
        public void Execute_WithPackageMissingId_ResultDoesNotContainItem()
        {
            // Arrange
            var task = new GetRestorePackageReferencesTask()
            {
                BuildEngine = new TestBuildEngine(),
                ProjectUniqueName = "MyProj",
                TargetFrameworks = "netstandard2.0",
                PackageReferences = new ITaskItem[]
                {
                    new MockTaskItem("X")
                    {
                        ["Version"] = "[1.0.0]"
                    },
                    new MockTaskItem(string.Empty)
                    {
                        ["Version"] = "[1.0.0]"
                    }
                }
            };

            // Act
            var result = task.Execute();

            // Assert
            Assert.True(result, "Task failed");
            Assert.Equal(1, task.RestoreGraphItems.Length);
        }

        /// <summary>
        /// Verifies that standard metadata like Id and version are correctly returned by <see cref="GetRestorePackageReferencesTask "/>.
        /// </summary>
        [Fact]
        public void Execute_WithStandardMetada_ResultContainsCorrectValues()
        {
            // Arrange
            var task = new GetRestorePackageReferencesTask()
            {
                BuildEngine = new TestBuildEngine(),
                ProjectUniqueName = "MyProj",
                TargetFrameworks = "netstandard2.0",
                PackageReferences = new ITaskItem[]
                {
                    new MockTaskItem("X")
                    {
                        ["Version"] = "[1.0.0]"
                    }
                }
            };

            // Act
            var result = task.Execute();

            // Assert
            Assert.True(result, "Task failed");
            Assert.Equal(1, task.RestoreGraphItems.Length);
            Assert.Equal("X", task.RestoreGraphItems[0].GetMetadata("Id"));
            Assert.Equal(task.ProjectUniqueName, task.RestoreGraphItems[0].GetMetadata("ProjectUniqueName"));
            Assert.Equal("Dependency", task.RestoreGraphItems[0].GetMetadata("Type"));
            Assert.Equal("[1.0.0]", task.RestoreGraphItems[0].GetMetadata("VersionRange"));
            Assert.Equal("netstandard2.0", task.RestoreGraphItems[0].GetMetadata("TargetFrameworks"));
        }
    }
}
