// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Git.IssueManager.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Git.IssueManager
{
    public class IssueManager
    {
        public string GitHubPersonalAccessToken { get; set; }

        public string AzureDevOpsPersonalAccessToken { get; set; }

        public IssueManager(
            string gitHubPersonalAccessToken = null,
            string azureDevOpsPersonalAccessToken = null)
        {
            GitHubPersonalAccessToken = gitHubPersonalAccessToken;
            AzureDevOpsPersonalAccessToken = azureDevOpsPersonalAccessToken;
        }

        /// <summary>
        /// Gets the author of a commit from a repo+commit
        /// </summary>
        /// <param name="repositoryUrl">The repository URL</param>
        /// <param name="commit">The commit SHA.</param>
        /// <returns>In GitHub returns the handle, in AzDO returns the full name.</returns>
        public async Task<string> GetCommitAuthorAsync(string repositoryUrl, string commit)
        {
            if (string.IsNullOrEmpty(repositoryUrl))
            {
                throw new ArgumentException(nameof(repositoryUrl));
            }

            if (string.IsNullOrEmpty(commit))
            {
                throw new ArgumentException(nameof(commit));
            }

            return await RepositoryHelper.GetCommitAuthorAsync(repositoryUrl, commit, GitHubPersonalAccessToken, AzureDevOpsPersonalAccessToken);

        }

        /// <summary>
        /// Creates a new GitHub issue.
        /// </summary>
        /// <param name="repositoryUrl">Repository URL where to create the issue.</param>
        /// <param name="issueTitle">Title of the issue.</param>
        /// <param name="issueDescription">Description of the issue.</param>
        /// <returns></returns>
        public async Task<int> CreateNewIssueAsync(
            string repositoryUrl,
            string issueTitle,
            string issueDescription,
            int? milestone = null,
            IEnumerable<string> labels = null,
            IEnumerable<string> assignees = null)
        {
            return await RepositoryHelper.CreateNewIssueAsync(
                repositoryUrl,
                issueTitle,
                issueDescription,
                GitHubPersonalAccessToken,
                milestone,
                labels,
                assignees);
        }
    }
}
