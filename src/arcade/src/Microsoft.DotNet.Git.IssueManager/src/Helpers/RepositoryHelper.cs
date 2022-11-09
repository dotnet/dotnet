// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Git.IssueManager.Clients;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Git.IssueManager.Helpers
{
    static class RepositoryHelper
    {
        public static async Task<string> GetCommitAuthorAsync(
            string repositoryUrl,
            string commit,
            string gitHubPersonalAccessToken,
            string azureDevOpsPersonalAccessToken)
        {
            if (Uri.TryCreate(repositoryUrl, UriKind.Absolute, out Uri parsedUri))
            {
                if (parsedUri.Host == "github.com")
                {
                    if (string.IsNullOrEmpty(gitHubPersonalAccessToken))
                    {
                        throw new ArgumentException("A GitHub personal access token is needed for this operation.");
                    }

                    return await GitHubClient.GetCommitAuthorAsync(repositoryUrl, commit, gitHubPersonalAccessToken);
                }

                if (string.IsNullOrEmpty(azureDevOpsPersonalAccessToken))
                {
                    throw new ArgumentException("An Azure DevOps personal access token is needed for this operation.");
                }

                return await AzureDevOpsClient.GetCommitAuthorAsync(repositoryUrl, commit, azureDevOpsPersonalAccessToken);
            }

            throw new InvalidCastException($"'{parsedUri}' is not a valid URI");
        }

        public static async Task<int> CreateNewIssueAsync(
            string repositoryUrl,
            string issueTitle,
            string issueDescription,
            string gitHubPersonalAccessToken,
            int? milestone = null,
            IEnumerable<string> labels = null,
            IEnumerable<string> assignees = null)
        {
            if (Uri.TryCreate(repositoryUrl, UriKind.Absolute, out Uri parsedUri))
            {
                if (parsedUri.Host == "github.com")
                {
                    if (string.IsNullOrEmpty(gitHubPersonalAccessToken))
                    {
                        throw new ArgumentException("A GitHub personal access token is needed for this operation.");
                    }

                    return await GitHubClient.CreateNewIssueAsync(
                        repositoryUrl,
                        issueTitle,
                        issueDescription,
                        gitHubPersonalAccessToken,
                        milestone,
                        labels,
                        assignees);
                }

                throw new NotImplementedException("Creating issues is not currently supported for an Azure DevOps repo.");
            }

            throw new InvalidCastException($"'{parsedUri}' is not a valid URI");
        }
    }
}
