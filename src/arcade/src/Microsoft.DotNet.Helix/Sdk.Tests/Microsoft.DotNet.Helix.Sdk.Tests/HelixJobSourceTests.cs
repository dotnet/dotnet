// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.DotNet.Helix.JobMonitor;
using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    /// <summary>
    /// Validates that <see cref="HelixJobSource.Compute"/> reproduces exactly the
    /// <c>Source</c> string that the Helix job submitter
    /// (<c>Microsoft.DotNet.Helix.JobSender.JobDefinition</c>) records for the same
    /// Azure DevOps build context. These cases were derived by reading
    /// <c>JobDefinition.InitializeSourceParameters</c> / <c>JobDefinition.GetSourcePrefix</c>
    /// and from observed Helix job records on public PR/CI/scheduled builds in
    /// dnceng-public and on internal official builds in dnceng.
    /// </summary>
    public class HelixJobSourceTests
    {
        [Theory]
        // Build.Reason / TeamProject / Repository (BUILD_REPOSITORY_NAME) / BUILD_SOURCEBRANCH / expected source

        // PR validation builds (dnceng-public). BUILD_SOURCEBRANCH for PR builds is the merge ref.
        [InlineData("PullRequest", "public", "dotnet/arcade", "refs/pull/16740/merge",
                    "pr/public/dotnet/arcade/refs/pull/16740/merge")]

        // Manually-queued public build (e.g. user clicks "Run pipeline"). Reason = Manual.
        // No PR is associated; BUILD_SOURCEBRANCH is the actual branch.
        [InlineData("Manual", "public", "dotnet/arcade", "refs/heads/main",
                    "ci/public/dotnet/arcade/refs/heads/main")]

        // Scheduled public build (cron-style trigger). Reason = Schedule.
        [InlineData("Schedule", "public", "dotnet/runtime", "refs/heads/release/9.0",
                    "ci/public/dotnet/runtime/refs/heads/release/9.0")]

        // Public CI build, single commit ('IndividualCI').
        [InlineData("IndividualCI", "public", "dotnet/sdk", "refs/heads/main",
                    "ci/public/dotnet/sdk/refs/heads/main")]

        // Public CI build, batched commits ('BatchedCI').
        [InlineData("BatchedCI", "public", "dotnet/sdk", "refs/heads/main",
                    "ci/public/dotnet/sdk/refs/heads/main")]

        // Internal team project, manually-queued. Prefix is 'official'.
        [InlineData("Manual", "internal", "dotnet-arcade", "refs/heads/main",
                    "official/internal/dotnet-arcade/refs/heads/main")]

        // Internal team project, scheduled. Still 'official'.
        [InlineData("Schedule", "internal", "dotnet-runtime", "refs/heads/release/9.0",
                    "official/internal/dotnet-runtime/refs/heads/release/9.0")]

        // Internal team project, regular CI run. Still 'official'.
        [InlineData("IndividualCI", "internal", "dotnet-arcade", "refs/heads/main",
                    "official/internal/dotnet-arcade/refs/heads/main")]

        // BuildCompletion / ResourceTrigger and any other reasons that aren't 'PullRequest'
        // fall through to 'ci' on public.
        [InlineData("BuildCompletion", "public", "dotnet/arcade", "refs/heads/main",
                    "ci/public/dotnet/arcade/refs/heads/main")]
        [InlineData("ResourceTrigger", "public", "dotnet/arcade", "refs/heads/main",
                    "ci/public/dotnet/arcade/refs/heads/main")]

        // Comparison is case-insensitive against the Build.Reason / TeamProject values.
        [InlineData("pullrequest", "PUBLIC", "dotnet/arcade", "refs/pull/1/merge",
                    "pr/PUBLIC/dotnet/arcade/refs/pull/1/merge")]
        [InlineData("manual", "INTERNAL", "dotnet-arcade", "refs/heads/main",
                    "official/INTERNAL/dotnet-arcade/refs/heads/main")]

        // Missing / empty BUILD_REASON falls back to 'ci' on public, 'official' on internal,
        // matching JobSender's behavior (it treats anything that isn't 'PullRequest' the same).
        [InlineData("", "public", "dotnet/arcade", "refs/heads/main",
                    "ci/public/dotnet/arcade/refs/heads/main")]
        [InlineData(null, "public", "dotnet/arcade", "refs/heads/main",
                    "ci/public/dotnet/arcade/refs/heads/main")]
        [InlineData(null, "internal", "dotnet-arcade", "refs/heads/main",
                    "official/internal/dotnet-arcade/refs/heads/main")]
        public void Compute_ReturnsExpectedSource(string buildReason, string teamProject, string repository, string sourceBranch, string expected)
        {
            Assert.Equal(expected, HelixJobSource.Compute(buildReason, teamProject, repository, sourceBranch));
        }

        [Theory]
        [InlineData(null, "dotnet/arcade", "refs/heads/main")]
        [InlineData("", "dotnet/arcade", "refs/heads/main")]
        [InlineData("public", null, "refs/heads/main")]
        [InlineData("public", "", "refs/heads/main")]
        [InlineData("public", "dotnet/arcade", null)]
        [InlineData("public", "dotnet/arcade", "")]
        public void Compute_ThrowsWhenRequiredComponentMissing(string teamProject, string repository, string sourceBranch)
        {
            Assert.Throws<ArgumentException>(() => HelixJobSource.Compute("PullRequest", teamProject, repository, sourceBranch));
        }

        [Theory]
        [InlineData("PullRequest", "public", "pr")]
        [InlineData("pullrequest", "public", "pr")] // case-insensitive
        [InlineData("PullRequest", "internal", "pr")] // PR wins over team project
        [InlineData("Manual", "internal", "official")]
        [InlineData("Manual", "INTERNAL", "official")] // case-insensitive team project
        [InlineData("Manual", "public", "ci")]
        [InlineData("Schedule", "public", "ci")]
        [InlineData("IndividualCI", "public", "ci")]
        [InlineData("BatchedCI", "public", "ci")]
        [InlineData(null, "public", "ci")]
        [InlineData("", "internal", "official")]
        public void GetSourcePrefix_MatchesJobSenderRules(string buildReason, string teamProject, string expectedPrefix)
        {
            Assert.Equal(expectedPrefix, HelixJobSource.GetSourcePrefix(buildReason, teamProject));
        }
    }
}
