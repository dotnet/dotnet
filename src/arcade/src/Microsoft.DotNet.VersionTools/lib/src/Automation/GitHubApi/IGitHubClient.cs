// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;

namespace Microsoft.DotNet.VersionTools.Automation.GitHubApi
{
    public interface IGitHubClient : IDisposable
    {
        GitHubAuth Auth { get; }

        Task<GitHubContents> GetGitHubFileAsync(
            string path,
            GitHubProject project,
            string @ref);

        Task<string> GetGitHubFileContentsAsync(
            string path,
            GitHubBranch branch);

        Task<string> GetGitHubFileContentsAsync(
            string path,
            GitHubProject project,
            string @ref);

        Task PutGitHubFileAsync(
            string fileUrl,
            string commitMessage,
            string newFileContents);

        Task PostGitHubPullRequestAsync(
            string title,
            string description,
            GitHubBranch headBranch,
            GitHubBranch baseBranch,
            bool maintainersCanModify);

        Task UpdateGitHubPullRequestAsync(
            GitHubProject project,
            int number,
            string title = null,
            string body = null,
            string state = null,
            bool? maintainersCanModify = null);

        Task<GitHubPullRequest> SearchPullRequestsAsync(
            GitHubProject project,
            string headPrefix,
            string author,
            string sortType = "created");

        Task<GitHubCombinedStatus> GetStatusAsync(
            GitHubProject project,
            string @ref);

        Task PostCommentAsync(
            GitHubProject project,
            int issueNumber,
            string message);

        Task<GitCommit> GetCommitAsync(
            GitHubProject project,
            string sha);

        Task<GitReference> GetReferenceAsync(
            GitHubProject project,
            string @ref);

        Task<GitTree> PostTreeAsync(
            GitHubProject project,
            string baseTree,
            GitObject[] tree);

        Task<GitCommit> PostCommitAsync(
            GitHubProject project,
            string message,
            string tree,
            string[] parents);

        Task<GitReference> PostReferenceAsync(
            GitHubProject project,
            string @ref,
            string sha);

        Task<GitReference> PatchReferenceAsync(
            GitHubProject project,
            string @ref,
            string sha,
            bool force);

        /// <summary>
        /// Get author ID in a form that can be used to search for pull requests. For GitHub, this
        /// is simply the auth username. For VSTS, this is a GUID fetched from an API.
        /// </summary>
        Task<string> GetMyAuthorIdAsync();

        string CreateGitRemoteUrl(
            GitHubProject project);

        void AdjustOptionsToCapability(
            PullRequestOptions options);
    }
}
