// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.DotNet.Helix.JobMonitor
{
    /// <summary>
    /// Computes the Helix <c>Source</c> string for a given Azure DevOps build context.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The Helix job submitter (<c>Microsoft.DotNet.Helix.JobSender.JobDefinition</c>) does not
    /// set <c>Source</c> directly. Instead it sets <c>SourcePrefix</c>, <c>TeamProject</c>,
    /// <c>Repository</c> and <c>Branch</c> on the job creation request and the Helix service
    /// combines them server-side into the final <c>Source</c> value as
    /// <c>{prefix}/{teamProject}/{repository}/{branch}</c>.
    /// </para>
    /// <para>
    /// <c>SourcePrefix</c> is derived from the Azure DevOps build context:
    /// <list type="bullet">
    /// <item><description><c>BUILD_REASON == "PullRequest"</c> → <c>pr</c></description></item>
    /// <item><description><c>SYSTEM_TEAMPROJECT == "internal"</c> (case-insensitive) → <c>official</c></description></item>
    /// <item><description>otherwise (public scheduled / manual / IndividualCI / BatchedCI / etc.) → <c>ci</c></description></item>
    /// </list>
    /// </para>
    /// <para>
    /// This means a public manually-queued or scheduled build produces a source like
    /// <c>ci/public/dotnet/arcade/refs/heads/main</c>, NOT <c>official/public/dotnet/arcade</c>.
    /// The monitor must reproduce the same logic so its <c>Job.ListAsync(source: ...)</c>
    /// query returns the actual Helix jobs that were submitted by the build.
    /// </para>
    /// </remarks>
    public static class HelixJobSource
    {
        /// <summary>
        /// Computes the Helix <c>Source</c> string that <see cref="Microsoft.DotNet.Helix.JobMonitor"/>
        /// can pass to <c>Job.ListAsync</c> so that it discovers exactly the Helix jobs submitted
        /// by Azure DevOps builds with the given context.
        /// </summary>
        /// <param name="buildReason">Value of <c>Build.Reason</c> (<c>BUILD_REASON</c>). Examples:
        ///   <c>PullRequest</c>, <c>Manual</c>, <c>Schedule</c>, <c>IndividualCI</c>, <c>BatchedCI</c>,
        ///   <c>BuildCompletion</c>, <c>ResourceTrigger</c>.</param>
        /// <param name="teamProject">Value of <c>System.TeamProject</c> (<c>SYSTEM_TEAMPROJECT</c>).
        ///   Typically <c>public</c> or <c>internal</c>.</param>
        /// <param name="repository">Value of <c>Build.Repository.Name</c> (<c>BUILD_REPOSITORY_NAME</c>).
        ///   For GitHub-backed AzDO repos this is <c>owner/repo</c>; for AzDO Git repos it is just
        ///   the repository name.</param>
        /// <param name="sourceBranch">Value of <c>Build.SourceBranch</c> (<c>BUILD_SOURCEBRANCH</c>).
        ///   For PR builds this is <c>refs/pull/{N}/merge</c>; for CI builds it is the actual ref
        ///   (e.g. <c>refs/heads/main</c>).</param>
        /// <returns>The Helix source string. Never null when <paramref name="teamProject"/> and
        ///   <paramref name="repository"/> are non-empty.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="teamProject"/> or
        ///   <paramref name="repository"/> or <paramref name="sourceBranch"/> is null or empty.</exception>
        public static string Compute(
            string buildReason,
            string teamProject,
            string repository,
            string sourceBranch)
        {
            if (string.IsNullOrWhiteSpace(teamProject))
            {
                throw new ArgumentException("Team project must be provided.", nameof(teamProject));
            }

            if (string.IsNullOrWhiteSpace(repository))
            {
                throw new ArgumentException("Repository must be provided.", nameof(repository));
            }

            if (string.IsNullOrWhiteSpace(sourceBranch))
            {
                throw new ArgumentException("Source branch must be provided.", nameof(sourceBranch));
            }

            string prefix = GetSourcePrefix(buildReason, teamProject);
            return $"{prefix}/{teamProject}/{repository}/{sourceBranch}";
        }

        /// <summary>
        /// Returns the <c>SourcePrefix</c> portion of the Helix source for the given build
        /// context, mirroring <c>Microsoft.DotNet.Helix.JobSender.JobDefinition.GetSourcePrefix</c>.
        /// Comparison is case-insensitive to tolerate variants of the variable values.
        /// </summary>
        public static string GetSourcePrefix(string buildReason, string teamProject)
        {
            if (string.Equals(buildReason, "PullRequest", StringComparison.OrdinalIgnoreCase))
            {
                return "pr";
            }

            if (string.Equals(teamProject, "internal", StringComparison.OrdinalIgnoreCase))
            {
                return "official";
            }

            return "ci";
        }
    }
}
