// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Maestro.Common.AzureDevOpsTokens;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.Extensions.Logging.Abstractions;

namespace CreateBaselineUpdatePR;

/// <summary>
/// <see cref="GitClient"/> implementation targeting Azure DevOps.
/// </summary>
public sealed class AzureDevOpsGitClient : GitClient
{
    public AzureDevOpsGitClient(string repoUri, string token)
        : base(CreateUnderlyingClient(repoUri, token), repoUri)
    {
    }

    public static bool Matches(string repoUri) =>
        repoUri.StartsWith("https://dev.azure.com/", StringComparison.OrdinalIgnoreCase)
        || repoUri.Contains(".visualstudio.com/", StringComparison.OrdinalIgnoreCase);

    public override string BuildPullRequestApiUrl(int pullRequestId)
    {
        (string account, string project, string repo) = AzureDevOpsClient.ParseRepoUri(RepoUri);
        return $"https://dev.azure.com/{account}/{project}/_apis/git/repositories/{repo}/pullRequests/{pullRequestId}";
    }

    public override string BuildPullRequestHtmlUrl(int pullRequestId)
    {
        (string account, string project, string repo) = AzureDevOpsClient.ParseRepoUri(RepoUri);
        return $"https://dev.azure.com/{account}/{project}/_git/{repo}/pullrequest/{pullRequestId}";
    }

    private static AzureDevOpsClient CreateUnderlyingClient(string repoUri, string token)
    {
        (string account, _, _) = AzureDevOpsClient.ParseRepoUri(repoUri);
        var tokenOptions = new AzureDevOpsTokenProviderOptions
        {
            [account] = new AzureDevOpsCredentialResolverOptions { Token = token }
        };
        return new AzureDevOpsClient(
            AzureDevOpsTokenProvider.FromStaticOptions(tokenOptions),
            new ProcessManager(NullLogger<IProcessManager>.Instance, "git"),
            NullLogger<AzureDevOpsClient>.Instance);
    }
}
