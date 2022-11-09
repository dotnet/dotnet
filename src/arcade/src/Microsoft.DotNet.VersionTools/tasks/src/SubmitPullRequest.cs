// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.DotNet.VersionTools.Automation.GitHubApi;
using Microsoft.DotNet.VersionTools.Automation.VstsApi;
using System;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.DotNet.Build.Tasks.VersionTools
{
    public class SubmitPullRequest : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string PullRequestServiceType { get; set; }

        [Required]
        public string PullRequestAuthToken { get; set; }

        /// <summary>
        /// The name of the user creating this PR. Used as the default for PullRequestAuthor.
        ///
        /// For GitHub, this locates the dev (origin) fork.
        ///
        /// For VSTS, this is only used as a default for PullRequestAuthor. (PullRequestAuthToken is
        /// used to fetch the calling user's GUID from VSTS. The GUID is used to search for existing
        /// PRs, like GitHub username is used to find GitHub PRs. However, VSTS user GUID isn't a
        /// good commit author, so the caller must provide a friendly name.)
        /// </summary>
        [Required]
        public string PullRequestUser { get; set; }

        /// <summary>
        /// Sets the Git author for the update commit. Defaults to PullRequestUser's value.
        /// </summary>
        public string PullRequestAuthor { get; set; }

        /// <summary>
        /// Sets the Git author's email for the update commit.
        /// </summary>
        [Required]
        public string PullRequestEmail { get; set; }

        /// <summary>
        /// Required for VSTS PullRequestServiceType. Used to find the VSTS repository.
        /// </summary>
        public string VstsInstanceName { get; set; }

        public string VstsApiVersionOverride { get; set; }

        /// <summary>
        /// For GitHub, the upstream repository owner. Defaults to 'dotnet'.
        ///
        /// For VSTS, the project containing the repo.
        /// </summary>
        public string ProjectRepoOwner { get; set; }

        [Required]
        public string ProjectRepoName { get; set; }
        [Required]
        public string ProjectRepoBranch { get; set; }

        public string CommitMessage { get; set; }

        /// <summary>
        /// Title of the pull request. Defaults to the commit message.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Body/description of the pull request. Optional.
        /// 
        /// This will overwrite the current PR body if updating a PR.
        /// </summary>
        public string Body { get; set; }

        public ITaskItem[] NotifyGitHubUsers { get; set; }

        public bool AlwaysCreateNewPullRequest { get; set; }

        public bool MaintainersCanModifyPullRequest { get; set; }

        public bool TrackDiscardedCommits { get; set; }

        public override bool Execute()
        {
            Trace.Listeners.MsBuildListenedInvoke(Log, TraceListenedExecute);
            return !Log.HasLoggedErrors;
        }

        private void TraceListenedExecute()
        {
            // GitHub and VSTS have different dev flow conventions.
            GitHubProject origin;

            using (IGitHubClient client = CreateClient(out origin))
            {
                var upstreamBranch = new GitHubBranch(
                    ProjectRepoBranch,
                    new GitHubProject(ProjectRepoName, ProjectRepoOwner));

                string body = Body ?? string.Empty;

                if (NotifyGitHubUsers != null)
                {
                    body += PullRequestCreator.NotificationString(NotifyGitHubUsers.Select(item => item.ItemSpec));
                }

                var options = new PullRequestOptions
                {
                    ForceCreate = AlwaysCreateNewPullRequest,
                    MaintainersCanModify = MaintainersCanModifyPullRequest,
                    TrackDiscardedCommits = TrackDiscardedCommits
                };

                var prCreator = new PullRequestCreator(client.Auth, PullRequestAuthor);
                prCreator.CreateOrUpdateAsync(
                    CommitMessage,
                    CommitMessage + $" ({ProjectRepoBranch})",
                    body,
                    upstreamBranch,
                    origin,
                    options,
                    client).Wait();
            }
        }

        private IGitHubClient CreateClient(out GitHubProject origin)
        {
            PullRequestServiceType type;
            if (!Enum.TryParse(PullRequestServiceType, true, out type))
            {
                string options = string.Join(", ", Enum.GetNames(typeof(PullRequestServiceType)));
                throw new ArgumentException(
                    $"{nameof(PullRequestServiceType)} '{PullRequestServiceType}' is not valid. " +
                    $"Options are {options}");
            }

            var auth = new GitHubAuth(
                PullRequestAuthToken,
                PullRequestUser,
                PullRequestEmail);

            switch (type)
            {
                case VersionTools.PullRequestServiceType.GitHub:
                    origin = new GitHubProject(ProjectRepoName, PullRequestUser);
                    return new GitHubClient(auth);

                case VersionTools.PullRequestServiceType.Vsts:
                    if (string.IsNullOrEmpty(VstsInstanceName))
                    {
                        throw new ArgumentException($"{nameof(VstsInstanceName)} is required but not set.");
                    }

                    origin = new GitHubProject(ProjectRepoName, ProjectRepoOwner);
                    return new VstsAdapterClient(auth, VstsInstanceName, VstsApiVersionOverride);

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(PullRequestServiceType),
                        type,
                        "Enum value invalid.");
            }
        }
    }
}
