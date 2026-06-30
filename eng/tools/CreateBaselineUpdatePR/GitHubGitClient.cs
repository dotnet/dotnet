// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Maestro.Common;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.Extensions.Logging.Abstractions;

namespace CreateBaselineUpdatePR;

/// <summary>
/// <see cref="GitClient"/> implementation targeting GitHub.
/// </summary>
public sealed class GitHubGitClient : GitClient
{
    public GitHubGitClient(string repoUri, string token)
        : base(CreateUnderlyingClient(token), repoUri)
    {
    }

    public static bool Matches(string repoUri) =>
        repoUri.StartsWith("https://github.com/", StringComparison.OrdinalIgnoreCase);

    public override string BuildPullRequestApiUrl(int pullRequestId)
    {
        (string owner, string repo) = GitHubClient.ParseRepoUri(RepoUri);
        return $"https://api.github.com/repos/{owner}/{repo}/pulls/{pullRequestId}";
    }

    public override string BuildPullRequestHtmlUrl(int pullRequestId)
    {
        (string owner, string repo) = GitHubClient.ParseRepoUri(RepoUri);
        return $"https://github.com/{owner}/{repo}/pull/{pullRequestId}";
    }

    private static GitHubClient CreateUnderlyingClient(string token) =>
        new(
            new ResolvedTokenProvider(token),
            new ProcessManager(NullLogger<IProcessManager>.Instance, "git"),
            NullLogger<GitHubClient>.Instance,
            cache: null);
}
