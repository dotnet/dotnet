// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace NuGet.Build.Tasks.Test
{
    /// <summary>
    /// Verifies the build-scoped guard that performs the start-of-build process-state reset at most once per node per
    /// build. The guard registers a build-lifetime sentinel exactly when it resets, so the number of
    /// <see cref="TestBuildEngine.RegisterTaskObjectCount" /> registrations equals the number of resets performed.
    /// Counting per engine keeps these tests isolated from other test classes that also trigger the reset.
    /// </summary>
    public class BuildTasksUtilityResetProcessStateTests
    {
        [Fact]
        public void ResetProcessStateOncePerBuild_WithSameBuildEngine_ResetsOnlyOnce()
        {
            var buildEngine = new TestBuildEngine();

            for (int i = 0; i < 5; i++)
            {
                BuildTasksUtility.ResetProcessStateOncePerBuild(buildEngine);
            }

            buildEngine.RegisterTaskObjectCount.Should().Be(1, "the build-scoped guard resets once per node per build");
        }

        [Fact]
        public void ResetProcessStateOncePerBuild_WithNewBuildEngine_ResetsAgain()
        {
            // A fresh TestBuildEngine models a reused node whose build-lifetime registry was cleared between builds.
            var firstBuild = new TestBuildEngine();
            var secondBuild = new TestBuildEngine();

            BuildTasksUtility.ResetProcessStateOncePerBuild(firstBuild);
            BuildTasksUtility.ResetProcessStateOncePerBuild(secondBuild);

            firstBuild.RegisterTaskObjectCount.Should().Be(1);
            secondBuild.RegisterTaskObjectCount.Should().Be(1, "a reused node re-reads the environment on each subsequent build");
        }

        [Fact]
        public void ResetProcessStateOncePerBuild_WithParallelInvocations_ResetsOnce()
        {
            var buildEngine = new TestBuildEngine();

            Parallel.For(0, 256, _ => BuildTasksUtility.ResetProcessStateOncePerBuild(buildEngine));

            buildEngine.RegisterTaskObjectCount.Should().Be(1, "parallel per-project invocations on the same node must not each reset");
        }
    }
}
