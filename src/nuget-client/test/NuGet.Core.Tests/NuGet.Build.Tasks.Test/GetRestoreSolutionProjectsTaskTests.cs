// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if !NETFRAMEWORK
using System.IO;
using FluentAssertions;
using Microsoft.Build.Framework;
using NuGet.Test.Utility;
using Xunit;
#endif

namespace NuGet.Build.Tasks.Test
{
    public class GetRestoreSolutionProjectsTaskTests
    {
#if !NETFRAMEWORK
        [Fact]
        public void Execute_RelativePaths_ResolvesAgainstProjectDirectoryNotCurrentDirectory()
        {
            using TestDirectory projectDirectory = TestDirectory.Create();

            var task = new GetRestoreSolutionProjectsTask
            {
                BuildEngine = new TestBuildEngine(),
                TaskEnvironment = TaskEnvironment.CreateWithProjectDirectoryAndEnvironment(projectDirectory.Path),
                SolutionFilePath = Path.Combine("solution", "MySolution.sln"),                                       // relative
                ProjectReferences = new ITaskItem[] { new MockTaskItem(Path.Combine("ProjectA", "ProjectA.csproj")) }, // relative
            };

            task.Execute().Should().BeTrue();

            string expectedRootedAtProjectDirectory = Path.GetFullPath(Path.Combine(projectDirectory.Path, "solution", "ProjectA", "ProjectA.csproj"));
            string ifResolvedAgainstCurrentDirectory = Path.GetFullPath(Path.Combine("solution", "ProjectA", "ProjectA.csproj"));
            string actual = task.OutputProjectReferences.Should().ContainSingle().Which.ItemSpec;

            actual.Should().Be(expectedRootedAtProjectDirectory);
            actual.Should().StartWith(projectDirectory.Path);
            actual.Should().NotBe(ifResolvedAgainstCurrentDirectory);
        }
#endif
    }
}
