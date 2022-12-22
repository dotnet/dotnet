// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.Build.Framework;
using Xunit;

namespace NuGet.Build.Tasks.Test
{
    public class GetCentralPackageVersionsTaskTests
    {
        [Fact]
        public void Execute_CheckMetadata()
        {
            // Arrange
            var task = new GetCentralPackageVersionsTask()
            {
                BuildEngine = new TestBuildEngine(),
                ProjectUniqueName = "MyProj",
                TargetFrameworks = "netstandard2.0",
                CentralPackageVersions = new ITaskItem[]
                {
                    new MockTaskItem("x")
                    {
                        ["Version"] = "[1.0.0]"
                    },
                    new MockTaskItem("y")
                    {
                        ["Version"] = "2.0.0",
                        ["Dummy"] = "someDummyValue"
                    }
                }
            };

            // Act
            var result = task.Execute();

            // Assert
            Assert.True(result, "Task failed");
            Assert.Equal(2, task.RestoreGraphItems.Length);
            var graphItems = task.RestoreGraphItems.OrderBy(item => item.GetMetadata("Id")).ToList();

            Assert.Equal("x", graphItems[0].GetMetadata("Id"));
            Assert.Equal(task.ProjectUniqueName, task.RestoreGraphItems[0].GetMetadata("ProjectUniqueName"));
            Assert.Equal("CentralPackageVersion", graphItems[0].GetMetadata("Type"));
            Assert.Equal("[1.0.0]", graphItems[0].GetMetadata("VersionRange"));
            Assert.Equal("netstandard2.0", graphItems[0].GetMetadata("TargetFrameworks"));

            Assert.Equal("y", graphItems[1].GetMetadata("Id"));
            Assert.Equal("CentralPackageVersion", graphItems[1].GetMetadata("Type"));
            Assert.Equal("2.0.0", graphItems[1].GetMetadata("VersionRange"));
            Assert.Equal("netstandard2.0", graphItems[1].GetMetadata("TargetFrameworks"));
            Assert.Equal(task.ProjectUniqueName, task.RestoreGraphItems[1].GetMetadata("ProjectUniqueName"));
            Assert.Equal(0, graphItems[1].MetadataNames.Cast<string>().Where(n => string.Equals(n, "Dummy")).Count());
        }

        [Fact]
        public void Execute_RemoveDuplicates()
        {
            // Arrange
            var task = new GetCentralPackageVersionsTask()
            {
                BuildEngine = new TestBuildEngine(),
                ProjectUniqueName = "MyProj",
                TargetFrameworks = "netstandard2.0",
                CentralPackageVersions = new ITaskItem[]
                {
                    new MockTaskItem("x")
                    {
                        ["Version"] = "[1.0.0]"
                    },
                    new MockTaskItem("X")
                    {
                        ["Version"] = "2.0.0",
                    }
                }
            };

            // Act
            var result = task.Execute();

            // Assert
            Assert.True(result, "Task failed");
            Assert.Equal(1, task.RestoreGraphItems.Length);
            var graphItems = task.RestoreGraphItems.OrderBy(item => item.GetMetadata("Id")).ToList();

            Assert.Equal("x", graphItems[0].GetMetadata("Id"));
            Assert.Equal(task.ProjectUniqueName, task.RestoreGraphItems[0].GetMetadata("ProjectUniqueName"));
            Assert.Equal("CentralPackageVersion", graphItems[0].GetMetadata("Type"));
            Assert.Equal("[1.0.0]", graphItems[0].GetMetadata("VersionRange"));
            Assert.Equal("netstandard2.0", graphItems[0].GetMetadata("TargetFrameworks"));
        }
    }
}
