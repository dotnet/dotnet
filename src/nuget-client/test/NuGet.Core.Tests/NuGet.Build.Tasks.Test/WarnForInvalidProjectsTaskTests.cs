// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Build.Framework;
using Moq;
using Xunit;

namespace NuGet.Build.Tasks.Test
{
    public class WarnForInvalidProjectsTaskTests
    {
        [Fact]
        public void WarnForInvalidProjectsTask_MissingAllProjectsAreIgnored()
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var all = new List<ITaskItem>();
            var valid = new List<ITaskItem>();

            var project1 = new Mock<ITaskItem>();
            project1.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            valid.Add(project1.Object);

            var task = new WarnForInvalidProjectsTask
            {
                BuildEngine = buildEngine,
                AllProjects = all.ToArray(),
                ValidProjects = valid.ToArray(),
            };

            var result = task.Execute();
            result.Should().BeTrue();

            testLogger.Warnings.Should().Be(0);
            testLogger.Errors.Should().Be(0);
        }

        [Fact]
        public void WarnForInvalidProjectsTask_SingleInvalidItemsVerifySingleWarning()
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var all = new List<ITaskItem>();
            var valid = new List<ITaskItem>();

            var project1 = new Mock<ITaskItem>();
            project1.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            var project2 = new Mock<ITaskItem>();
            project2.SetupGet(e => e.ItemSpec).Returns("b.csproj");

            all.Add(project1.Object);
            all.Add(project2.Object);
            valid.Add(project1.Object);

            var task = new WarnForInvalidProjectsTask
            {
                BuildEngine = buildEngine,
                AllProjects = all.ToArray(),
                ValidProjects = valid.ToArray(),
            };

            var result = task.Execute();
            result.Should().BeTrue();

            testLogger.Warnings.Should().Be(1);
            testLogger.Errors.Should().Be(0);
            testLogger.Messages.Where(e => e.Contains("Skipping restore for project 'b.csproj'. The project file may be invalid or missing targets required for restore.")).Count().Should().Be(1);
        }

        [Fact]
        public void WarnForInvalidProjectsTask_NoValidItemsVerifyWarning()
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var all = new List<ITaskItem>();
            var valid = new List<ITaskItem>();

            var project1 = new Mock<ITaskItem>();
            project1.SetupGet(e => e.ItemSpec).Returns("a.csproj");

            all.Add(project1.Object);
            // not added to valid

            var task = new WarnForInvalidProjectsTask
            {
                BuildEngine = buildEngine,
                AllProjects = all.ToArray(),
                ValidProjects = valid.ToArray(),
            };

            var result = task.Execute();
            result.Should().BeTrue();

            testLogger.Warnings.Should().Be(1);
            testLogger.Errors.Should().Be(0);
        }

        [Fact]
        public void WarnForInvalidProjectsTask_AllValidItemsVerifyNoWarnings()
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var all = new List<ITaskItem>();
            var valid = new List<ITaskItem>();

            var project1 = new Mock<ITaskItem>();
            project1.SetupGet(e => e.ItemSpec).Returns("a.csproj");

            all.Add(project1.Object);
            valid.Add(project1.Object);

            var task = new WarnForInvalidProjectsTask
            {
                BuildEngine = buildEngine,
                AllProjects = all.ToArray(),
                ValidProjects = valid.ToArray(),
            };

            var result = task.Execute();
            result.Should().BeTrue();

            testLogger.Warnings.Should().Be(0);
            testLogger.Errors.Should().Be(0);
        }

        [Fact]
        public void WarnForInvalidProjectsTask_NoItemsVerifyNoErrors()
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;
            var task = new WarnForInvalidProjectsTask
            {
                BuildEngine = buildEngine,
                AllProjects = new ITaskItem[0],
                ValidProjects = new ITaskItem[0],
            };

            var result = task.Execute();
            result.Should().BeTrue();

            testLogger.Warnings.Should().Be(0);
            testLogger.Errors.Should().Be(0);
        }

        [Fact]
        public void WarnForInvalidProjectsTask_NullItemsVerifyNoErrors()
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;
            var task = new WarnForInvalidProjectsTask
            {
                BuildEngine = buildEngine
            };

            var result = task.Execute();
            result.Should().BeTrue();

            testLogger.Warnings.Should().Be(0);
            testLogger.Errors.Should().Be(0);
        }
    }
}
