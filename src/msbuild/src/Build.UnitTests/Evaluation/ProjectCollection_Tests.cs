// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Evaluation;
using Shouldly;
using Xunit;

#nullable disable

namespace Microsoft.Build.UnitTests.Evaluation
{
    public class ProjectCollection_Tests
    {
        [Fact]
        public void ProjectRootElementCache_IsDeterminedByEnvironmentVariable()
        {
            using var collectionWithDefaultCache = new ProjectCollection();
            collectionWithDefaultCache.ProjectRootElementCache.ShouldBeOfType<ProjectRootElementCache>();

            const string envKey = "MsBuildUseSimpleProjectRootElementCacheConcurrency";

            using (TestEnvironment env = TestEnvironment.Create())
            {
                env.SetEnvironmentVariable(envKey, "true");
                using var collectionWithSimpleCache = new ProjectCollection();
                collectionWithSimpleCache.ProjectRootElementCache.ShouldBeOfType<SimpleProjectRootElementCache>();
            }
        }

        [Theory]
        // The informational version's SHA is truncated to 9 characters. In VMR builds this SHA is the VMR
        // (dotnet/dotnet) commit, which is intentionally kept as the primary SHA shown to users.
        [InlineData("18.8.0-dev-26316-01+vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv", "18.8.0-dev-26316-01+vvvvvvvvv")]
        // A version with no '+' SHA suffix is returned unchanged.
        [InlineData("18.8.0-dev-26316-01", "18.8.0-dev-26316-01")]
        public void GetDisplayVersion_TruncatesShaToNineCharacters(string fullInformationalVersion, string expected)
        {
            ProjectCollection.GetDisplayVersion(fullInformationalVersion).ShouldBe(expected);
        }
    }
}
