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
    public class GetRestoreProjectReferencesTaskTests
    {
#if !NETFRAMEWORK
        [Fact]
        public void Execute_RelativeProjectReference_ResolvesAgainstProjectDirectoryNotCurrentDirectory()
        {
            using TestDirectory projectDirectory = TestDirectory.Create();
            var task = new GetRestoreProjectReferencesTask
            {
                BuildEngine = new TestBuildEngine(),
                TaskEnvironment = TaskEnvironment.CreateWithProjectDirectoryAndEnvironment(projectDirectory.Path),
                ProjectUniqueName = "MyProject",
                ParentProjectPath = Path.Combine("solution", "MyProject.csproj"),                               // relative
                ProjectReferences = new ITaskItem[] { new MockTaskItem(Path.Combine("ProjectA", "ProjectA.csproj")) }, // relative
            };

            task.Execute().Should().BeTrue();

            string expectedRootedAtProjectDirectory = Path.GetFullPath(Path.Combine(projectDirectory.Path, "solution", "ProjectA", "ProjectA.csproj"));
            string ifResolvedAgainstCurrentDirectory = Path.GetFullPath(Path.Combine("solution", "ProjectA", "ProjectA.csproj"));
            string actual = task.RestoreGraphItems.Should().ContainSingle().Which.GetMetadata("ProjectPath");

            actual.Should().Be(expectedRootedAtProjectDirectory);
            actual.Should().StartWith(projectDirectory.Path);
            actual.Should().NotBe(ifResolvedAgainstCurrentDirectory);
        }
#endif
    }
}
