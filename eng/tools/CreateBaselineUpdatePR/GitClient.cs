// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.DarcLib;

namespace CreateBaselineUpdatePR;

/// <summary>
/// Wraps a DarcLib <see cref="IRemoteGitRepo"/> together with helpers for
/// constructing provider-specific PR URLs that DarcLib's own APIs require.
/// </summary>
public abstract class GitClient
{
    public IRemoteGitRepo Repo { get; }

    public string RepoUri { get; }

    protected GitClient(IRemoteGitRepo repo, string repoUri)
    {
        Repo = repo;
        RepoUri = repoUri;
    }

    /// <summary>
    /// Returns an API-style URL for the given pull request, suitable for use with
    /// DarcLib's <c>UpdatePullRequestAsync</c> / <c>GetPullRequestAsync</c>.
    /// </summary>
    public abstract string BuildPullRequestApiUrl(int pullRequestId);

    /// <summary>
    /// Returns a human-friendly HTML URL for the given pull request.
    /// </summary>
    public abstract string BuildPullRequestHtmlUrl(int pullRequestId);

    /// <summary>
    /// Creates a <see cref="GitClient"/> appropriate for the given repository URL.
    /// </summary>
    public static GitClient Create(string repoUri, string token)
    {
        ArgumentException.ThrowIfNullOrEmpty(repoUri);
        ArgumentException.ThrowIfNullOrEmpty(token);

        if (GitHubGitClient.Matches(repoUri))
        {
            return new GitHubGitClient(repoUri, token);
        }

        if (AzureDevOpsGitClient.Matches(repoUri))
        {
            return new AzureDevOpsGitClient(repoUri, token);
        }

        throw new ArgumentException($"Unsupported repository URL '{repoUri}'.", nameof(repoUri));
    }
}
