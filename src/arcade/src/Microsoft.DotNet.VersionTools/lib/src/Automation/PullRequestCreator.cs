// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.Automation.GitHubApi;
using Microsoft.DotNet.VersionTools.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.DotNet.VersionTools.Automation
{
    public class PullRequestCreator
    {
        private const string DiscardedCommitElementName = "auto-pr-discard-list";
        private const string MaestroStopUpdatesLabel = "Maestro-Stop-Updating";

        private GitHubAuth _auth;

        public string GitAuthorName { get; }

        public PullRequestCreator(
            GitHubAuth auth,
            string gitAuthorName = null)
        {
            if (auth == null)
            {
                throw new ArgumentNullException(
                    nameof(auth),
                    "Authentication is required: pull requests cannot be created anonymously.");
            }
            _auth = auth;
            GitAuthorName = gitAuthorName ?? auth.User;
        }

        public async Task CreateOrUpdateAsync(
            string commitMessage,
            string title,
            string description,
            GitHubBranch baseBranch,
            GitHubProject origin,
            PullRequestOptions options)
        {
            using (var client = new GitHubClient(_auth))
            {
                await CreateOrUpdateAsync(
                    commitMessage,
                    title,
                    description,
                    baseBranch,
                    origin,
                    options,
                    client);
            }
        }

        public async Task CreateOrUpdateAsync(
            string commitMessage,
            string title,
            string description,
            GitHubBranch baseBranch,
            GitHubProject origin,
            PullRequestOptions options,
            IGitHubClient client)
        {
            options = options ?? new PullRequestOptions();
            client.AdjustOptionsToCapability(options);

            var upstream = baseBranch.Project;

            GitHubBranch originBranch = null;
            GitHubPullRequest pullRequestToUpdate = null;

            IUpdateBranchNamingStrategy namingStrategy = options.BranchNamingStrategy
                ?? new SingleBranchNamingStrategy("UpdateDependencies");

            string upgradeBranchPrefix = namingStrategy.Prefix(baseBranch.Name);

            if (!options.ForceCreate)
            {
                string myAuthorId = await client.GetMyAuthorIdAsync();

                pullRequestToUpdate = await client.SearchPullRequestsAsync(
                    upstream,
                    upgradeBranchPrefix,
                    myAuthorId);

                if (pullRequestToUpdate == null)
                {
                    Trace.TraceInformation($"No existing pull request found.");
                }
                else
                {
                    Trace.TraceInformation(
                        $"Pull request already exists for {upgradeBranchPrefix} in {upstream.Segments}. " +
                        $"#{pullRequestToUpdate.Number}, '{pullRequestToUpdate.Title}'");

                    GitCommit headCommit = await client.GetCommitAsync(
                        origin,
                        pullRequestToUpdate.Head.Sha);

                    string blockedReason = GetUpdateBlockedReason(
                        pullRequestToUpdate,
                        headCommit,
                        upgradeBranchPrefix,
                        origin,
                        options);

                    if (blockedReason == null)
                    {
                        if (options.TrackDiscardedCommits)
                        {
                            await PostDiscardedCommitCommentAsync(
                                baseBranch.Project,
                                pullRequestToUpdate,
                                headCommit,
                                client);
                        }

                        originBranch = new GitHubBranch(
                            pullRequestToUpdate.Head.Ref,
                            origin);
                    }
                    else
                    {
                        string comment =
                            $"Couldn't update this pull request: {blockedReason}\n" +
                            $"Would have applied '{commitMessage}'";

                        Trace.TraceInformation($"Sending comment to PR: {comment}");

                        await client.PostCommentAsync(upstream, pullRequestToUpdate.Number, comment);
                        return;
                    }
                }

                // No existing branch to update: push to a new one.
                if (originBranch == null)
                {
                    string newBranchName =
                        namingStrategy.Prefix(baseBranch.Name) +
                        namingStrategy.CreateFreshBranchNameSuffix(baseBranch.Name);

                    originBranch = new GitHubBranch(newBranchName, origin);
                }

                PushNewCommit(originBranch, commitMessage, client);

                if (pullRequestToUpdate != null)
                {
                    await client.UpdateGitHubPullRequestAsync(
                        upstream,
                        pullRequestToUpdate.Number,
                        title,
                        description,
                        maintainersCanModify: options.MaintainersCanModify);
                }
                else
                {
                    await client.PostGitHubPullRequestAsync(
                        title,
                        description,
                        originBranch,
                        baseBranch,
                        options.MaintainersCanModify);
                }
            }
        }

        private async Task PostDiscardedCommitCommentAsync(
            GitHubProject baseProject,
            GitHubPullRequest pullRequestToUpdate,
            GitCommit oldCommit,
            IGitHubClient client)
        {
            GitHubCombinedStatus combinedStatus = await client.GetStatusAsync(
                baseProject,
                oldCommit.Sha);

            CiStatusLine[] statuses = combinedStatus
                .Statuses
                .OrderBy(s => s.State)
                .ThenBy(s => s.Context)
                .Select(CiStatusLine.Create)
                .ToArray();

            string statusLines = statuses
                .Aggregate(string.Empty, (acc, line) => acc + line.MarkdownLine + "\r\n");

            string ciSummary = string.Join(
                " ",
                statuses
                    .GroupBy(s => s.Emoticon)
                    .Select(g => $"{g.Count()}{g.Key}")
                    .ToArray());

            string commentBody =
                $"Discarded [`{oldCommit.Sha.Substring(0, 7)}`]({oldCommit.HtmlUrl}): " +
                $"`{oldCommit.Message}`";

            if (statuses.Any())
            {
                commentBody += "\r\n\r\n" +
                    "<details>" +
                    "<summary>" +
                    $"CI Status: {ciSummary} (click to expand)\r\n" +
                    "</summary>" +
                    $"\r\n\r\n{statusLines}\r\n" +
                    "</details>";
            }

            await client.PostCommentAsync(
                baseProject,
                pullRequestToUpdate.Number,
                commentBody);
        }

        public static string NotificationString(IEnumerable<string> usernames)
        {
            return $"/cc @{string.Join(" @", usernames)}";
        }

        private string GetUpdateBlockedReason(
            GitHubPullRequest pr,
            GitCommit headCommit,
            string upgradeBranchPrefix,
            GitHubProject origin,
            PullRequestOptions options)
        {
            if (pr.Head.User.Login != origin.Owner && !options.AllowBranchOnAnyRepoOwner)
            {
                return $"Owner of head repo '{pr.Head.User.Login}' is not '{origin.Owner}'";
            }
            if (!pr.Head.Ref.StartsWith(upgradeBranchPrefix))
            {
                return $"Ref name '{pr.Head.Ref}' does not start with '{upgradeBranchPrefix}'";
            }
            if (headCommit.Author.Name != GitAuthorName)
            {
                return $"Head commit author '{headCommit.Author.Name}' is not '{GitAuthorName}'";
            }
            if (headCommit.Committer.Name != GitAuthorName)
            {
                return $"Head commit committer '{headCommit.Committer.Name}' is not '{GitAuthorName}'";
            }
            if (pr.Labels?.Any(IsMaestroStopUpdatesLabel) ?? false)
            {
                return $"Label `{MaestroStopUpdatesLabel}` is attached";
            }
            return null;
        }

        private void PushNewCommit(GitHubBranch branch, string commitMessage, IGitHubClient client)
        {
            GitCommand.Commit(commitMessage, GitAuthorName, _auth.Email, all: true);

            string remoteUrl = client.CreateGitRemoteUrl(branch.Project);
            string refSpec = $"HEAD:refs/heads/{branch.Name}";

            GitCommand.Push(
                $"https://{_auth.User}:{_auth.AuthToken}@{remoteUrl}",
                $"https://{remoteUrl}",
                refSpec,
                force: true);
        }

        private static bool IsMaestroStopUpdatesLabel(GitHubLabel label) => string.Equals(
            label?.Name,
            MaestroStopUpdatesLabel,
            StringComparison.OrdinalIgnoreCase);

        private class CiStatusLine
        {
            public static CiStatusLine Create(GitHubStatus status)
            {
                string emoticon = ":grey_question:";
                if (string.Equals(status.State, "success", StringComparison.OrdinalIgnoreCase))
                {
                    emoticon = ":heavy_check_mark:";
                }
                else if (string.Equals(status.State, "pending", StringComparison.OrdinalIgnoreCase))
                {
                    emoticon = ":hourglass:";
                }
                else if (string.Equals(status.State, "error", StringComparison.OrdinalIgnoreCase) ||
                         string.Equals(status.State, "failure", StringComparison.OrdinalIgnoreCase))
                {
                    emoticon = ":x:";
                }

                string line = $" * {emoticon} **{status.Context}** {status.Description}";
                if (!string.IsNullOrEmpty(status.TargetUrl))
                {
                    line += $" [Details]({status.TargetUrl})";
                }

                return new CiStatusLine
                {
                    Emoticon = emoticon,
                    MarkdownLine = line
                };
            }

            public string Emoticon { get; private set; }
            public string MarkdownLine { get; private set; }
        }
    }
}
