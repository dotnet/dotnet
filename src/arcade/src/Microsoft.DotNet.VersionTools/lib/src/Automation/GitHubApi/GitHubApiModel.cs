// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json;

namespace Microsoft.DotNet.VersionTools.Automation.GitHubApi
{
    /// <summary>
    /// The interesting parts of a GitHub pull request, as returned by the pull request api.
    /// </summary>
    public class GitHubPullRequest
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public GitHubHead Head { get; set; }
        public GitHubUser User { get; set; }
        public GitHubLabel[] Labels { get; set; }
    }

    public class GitHubHead
    {
        public string Label { get; set; }
        public string Ref { get; set; }
        public string Sha { get; set; }
        public GitHubUser User { get; set; }
    }

    public class GitHubUser
    {
        public string Login { get; set; }
    }

    public class GitHubLabel
    {
        public string Name { get; set; }
    }

    public class GitHubIssueQueryResponse
    {
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
        public GitHubIssue[] Items { get; set; }
    }

    public class GitHubIssue
    {
        public int Id { get; set; }
        public int Number { get; set; }
    }

    public class GitHubContents
    {
        public string Sha { get; set; }
        public string Content { get; set; }
    }

    public class GitHubCombinedStatus
    {
        public string State { get; set; }
        public GitHubStatus[] Statuses { get; set; }
    }

    public class GitHubStatus
    {
        public string State { get; set; }
        [JsonProperty("target_url")]
        public string TargetUrl { get; set; }
        public string Description { get; set; }
        public string Context { get; set; }
    }

    public class GitCommit
    {
        public string Sha { get; set; }
        public GitCommitUser Author { get; set; }
        public GitCommitUser Committer { get; set; }
        public string Message { get; set; }
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
    }

    public class GitCommitUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class GitReference
    {
        public string Ref { get; set; }
        public GitReferenceObject Object { get; set; }
    }

    public class GitReferenceObject
    {
        public string Sha { get; set; }
    }

    public class GitTree
    {
        public string Sha { get; set; }
    }

    public class GitObject
    {
        public const string TypeBlob = "blob";

        public const string ModeFile = "100644";

        public string Path { get; set; }
        public string Mode { get; set; }
        public string Type { get; set; }
        public string Sha { get; set; }
        public string Content { get; set; }
    }
}
