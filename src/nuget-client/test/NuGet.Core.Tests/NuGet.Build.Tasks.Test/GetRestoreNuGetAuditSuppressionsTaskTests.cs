// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Build.Framework;
using Xunit;

namespace NuGet.Build.Tasks.Test
{
    public class GetRestoreNuGetAuditSuppressionsTaskTests
    {
        [Fact]
        public void Execute_CheckAllMetadata()
        {
            // Arrange
            var task = new GetRestoreNuGetAuditSuppressionsTask()
            {
                BuildEngine = new TestBuildEngine(),
                ProjectUniqueName = "MyProj",
                TargetFrameworks = "netstandard2.0",
                NuGetAuditSuppressions = new ITaskItem[] { new MockTaskItem("https://cve.test/suppressed") }
            };

            // Act
            var result = task.Execute();

            // Assert
            Assert.True(result, "Task failed");
            Assert.Equal(1, task.RestoreGraphItems.Length);
            Assert.Equal("NuGetAuditSuppress", task.RestoreGraphItems[0].GetMetadata("Type"));
            Assert.Equal("https://cve.test/suppressed", task.RestoreGraphItems[0].GetMetadata("Id"));
            Assert.Equal(task.ProjectUniqueName, task.RestoreGraphItems[0].GetMetadata("ProjectUniqueName"));
            Assert.Equal("netstandard2.0", task.RestoreGraphItems[0].GetMetadata("TargetFrameworks"));
        }

        [Fact]
        public void Execute_RemovesDuplicates()
        {
            // Arrange
            var task = new GetRestoreNuGetAuditSuppressionsTask()
            {
                BuildEngine = new TestBuildEngine(),
                ProjectUniqueName = "MyProj",
                TargetFrameworks = "netstandard2.0",
                NuGetAuditSuppressions = new ITaskItem[] {
                    new MockTaskItem("https://cve.test/suppressed/1"),
                    new MockTaskItem("https://cve.test/suppressed/1"),
                    new MockTaskItem("https://cve.test/suppressed/2")
                }
            };

            // Act
            var result = task.Execute();

            // Assert
            Assert.True(result, "Task failed");
            Assert.Equal(2, task.RestoreGraphItems.Length);
            Assert.Equal("https://cve.test/suppressed/1", task.RestoreGraphItems[0].GetMetadata("Id"));
            Assert.Equal("https://cve.test/suppressed/2", task.RestoreGraphItems[1].GetMetadata("Id"));
        }

        [Fact]
        public void Execute_DoesNotDeduplicateWhenOnlyCaseDiffers()
        {
            // Arrange
            var task = new GetRestoreNuGetAuditSuppressionsTask()
            {
                BuildEngine = new TestBuildEngine(),
                ProjectUniqueName = "MyProj",
                TargetFrameworks = "netstandard2.0",
                NuGetAuditSuppressions = new ITaskItem[] {
                    new MockTaskItem("https://cve.test/suppressed"),
                    new MockTaskItem("https://cve.test/SUPPRESSED"),
                }
            };

            // Act
            var result = task.Execute();

            // Assert
            Assert.True(result, "Task failed");
            Assert.Equal(2, task.RestoreGraphItems.Length);
            Assert.Equal("https://cve.test/suppressed", task.RestoreGraphItems[0].GetMetadata("Id"));
            Assert.Equal("https://cve.test/SUPPRESSED", task.RestoreGraphItems[1].GetMetadata("Id"));
        }
    }
}
