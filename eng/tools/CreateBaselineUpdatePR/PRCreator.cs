// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CreateBaselineUpdatePR;

using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.DotNet.DarcLib.Models;

public partial class PRCreator
{
    private readonly GitClient _gitClient;
    private readonly Pipelines _pipeline;
    private readonly string _identifier;
    private const string BuildLink = "https://dev.azure.com/dnceng/internal/_build/results?buildId=";
    private const string DefaultLicenseBaselineContent = "{\n  \"files\": []\n}";
    private const string UpdatedFilePrefix = "updated";

    // GitHub's git data API is eventually consistent: a branch ref read immediately after it is
    // written can still report "not found". Poll a handful of times before treating a missing
    // branch as a genuine failure.
    private const int BranchCreationVerificationAttempts = 5;
    private static readonly TimeSpan BranchCreationVerificationDelay = TimeSpan.FromSeconds(2);
    public PRCreator(GitClient gitClient, Pipelines pipeline, string? identifier = null)
    {
        _gitClient = gitClient;
        _pipeline = pipeline;
        _identifier = string.IsNullOrWhiteSpace(identifier) ? string.Empty : identifier.Trim();
    }

    [GeneratedRegex("-+")]
    private static partial Regex ConsecutiveDashesRegex { get; }

    public async Task ExecuteAsync(
        string originalFilesDirectory,
        string updatedFilesDirectory,
        int buildId,
        string title,
        string targetBranch)
    {
        DateTime startTime = DateTime.Now.ToUniversalTime();

        Log.LogInformation($"Starting PR creation at {startTime} UTC for pipeline {_pipeline}.");

        var updatedTestsFiles = GetUpdatedFiles(updatedFilesDirectory);

        // Fetch the files within the desired path from the original tree
        List<TreeFile> originalTreeItems = await FetchOriginalTreeItemsAsync(targetBranch, originalFilesDirectory);

        // Update the test results tree based on the pipeline
        List<TreeFile> modifiedTreeItems = UpdateAllFiles(updatedTestsFiles, [.. originalTreeItems], _pipeline);

        await CreateOrUpdatePullRequestAsync(originalTreeItems, modifiedTreeItems, originalFilesDirectory, buildId, title, targetBranch);
    }

    private async Task<List<TreeFile>> FetchOriginalTreeItemsAsync(
        string targetBranch,
        string desiredPath)
    {
        string? commitSha = await _gitClient.Repo.GetLastCommitShaAsync(_gitClient.RepoUri, targetBranch)
            ?? throw new InvalidOperationException(
                $"Could not resolve target branch '{targetBranch}' in {_gitClient.RepoUri}.");

        List<GitFile> files;
        try
        {
            files = await _gitClient.Repo.GetFilesAtCommitAsync(_gitClient.RepoUri, commitSha, desiredPath);
        }
        catch (DependencyFileNotFoundException)
        {
            // The desired path may not exist yet (no baseline directory). Treat as empty tree.
            return [];
        }

        string prefix = desiredPath.Replace('\\', '/').TrimEnd('/') + "/";
        return [.. files.Select(f =>
        {
            string relativePath = f.FilePath.StartsWith(prefix, StringComparison.Ordinal) ? f.FilePath[prefix.Length..] : f.FilePath;
            // Decode base64 blobs (DarcLib's GitHub client returns text files with
            // ContentEncoding.Base64) so the in-memory tree is uniformly plain UTF-8 text.
            string content = f.ContentEncoding == ContentEncoding.Base64
                ? Encoding.UTF8.GetString(Convert.FromBase64String(f.Content))
                : f.Content;
            return new TreeFile(relativePath, content);
        })];
    }

    // Return a dictionary using the filename without the 
    // "updated" prefix (case-insensitive) and anything after the first '.' as the key
    private Dictionary<string, HashSet<string>> GetUpdatedFiles(string updatedFilesDirectory) =>
        Directory
            .GetFiles(updatedFilesDirectory, "*", SearchOption.AllDirectories)
            .Where(file => Path.GetFileName(file).StartsWith(UpdatedFilePrefix, StringComparison.OrdinalIgnoreCase))
            .GroupBy(updatedTestsFile => ParseUpdatedFileName(updatedTestsFile).Split('.')[0])
            .ToDictionary(
                group => group.Key,
                group => new HashSet<string>(group)
            );

    private List<TreeFile> UpdateAllFiles(Dictionary<string, HashSet<string>> updatedFiles, List<TreeFile> tree, Pipelines pipeline)
    {
        bool unionExclusions = pipeline switch
        {
            Pipelines.Sdk => true,
            Pipelines.Reproducibility => true,
            Pipelines.License => false,
            _ => throw new ArgumentOutOfRangeException(nameof(pipeline))
        };
        string? defaultContent = pipeline == Pipelines.License ? DefaultLicenseBaselineContent : null;
        foreach (var updatedFile in updatedFiles)
        {
            if (updatedFile.Key.Contains("Exclusions"))
            {
                tree = UpdateExclusionFile(updatedFile.Key, updatedFile.Value, tree, union: unionExclusions);
            }
            else
            {
                tree = UpdateRegularFiles(updatedFile.Value, tree, defaultContent);
            }
        }
        return tree;
    }

    private static List<TreeFile> UpdateExclusionFile(string fileNameKey, HashSet<string> updatedFiles, List<TreeFile> tree, bool union = false)
    {
        string? content = null;
        IEnumerable<string> parsedFile = Enumerable.Empty<string>();

        // Combine the lines of the updated files
        foreach (var filePath in updatedFiles)
        {
            var updatedFileLines = File.ReadAllLines(filePath);
            if (!parsedFile.Any())
            {
                parsedFile = updatedFileLines;
            }
            else if (union == true)
            {
                parsedFile = parsedFile.Union(updatedFileLines);
            }
            else
            {
                parsedFile = parsedFile.Where(parsedLine => updatedFileLines.Contains(parsedLine));
            }
        }

        if (union == true)
        {
            // Need to compare to the original file and remove any lines that are not in the parsed updated file

            // Find the key in the tree
            var originalTreeItem = tree
                .Where(item => item.RelativePath.Contains(fileNameKey, StringComparison.Ordinal))
                .FirstOrDefault();

            if (originalTreeItem != null)
            {
                content = originalTreeItem.Content;
                var originalContent = content.Split("\n");

                foreach (var line in originalContent)
                {
                    if (!parsedFile.Contains(line))
                    {
                        // If the newline character is not present, the line is at the end of the file
                        content = content.Contains(line + "\n") ? content.Replace(line + "\n", "") : content.Replace(line, "");
                    }
                }
            }
        }

        else
        {
            if (parsedFile.Any())
            {
                // No need to compare to the original file, just log the parsed lines
                content = string.Join("\n", parsedFile) + "\n";
            }
        }

        string updatedFilePath = fileNameKey + ".txt";
        return UpdateFile(tree, content, fileNameKey, updatedFilePath);
    }

    private static List<TreeFile> UpdateRegularFiles(HashSet<string> updatedFiles, List<TreeFile> tree, string? compareContent = null)
    {
        foreach (var filePath in updatedFiles)
        {
            var content = File.ReadAllText(filePath);
            if (compareContent != null && content == compareContent)
            {
                content = null;
            }
            string originalFileName = Path.GetFileName(ParseUpdatedFileName(filePath));
            tree = UpdateFile(tree, content, originalFileName, originalFileName);
        }
        return tree;
    }

    private static List<TreeFile> UpdateFile(List<TreeFile> tree, string? content, string searchFileName, string updatedPath)
    {
        var originalTreeItem = tree
            .Where(item => item.RelativePath.Contains(searchFileName, StringComparison.Ordinal))
            .FirstOrDefault();

        if (content == null)
        {
            // Content is null, delete the file if it exists
            if (originalTreeItem != null)
            {
                tree.Remove(originalTreeItem);
            }
        }
        else if (originalTreeItem == null)
        {
            // Path not in the tree, add a new tree item
            tree.Add(new TreeFile(updatedPath, content));
        }
        else
        {
            // Path in the tree, update the content (TreeFile is immutable so replace the entry)
            tree.Remove(originalTreeItem);
            tree.Add(originalTreeItem with { Content = content });
        }
        return tree;
    }

    private static string ParseUpdatedFileName(string updatedFile)
    {
        string fileName = Path.GetFileName(updatedFile);
        if (fileName.StartsWith(UpdatedFilePrefix, StringComparison.OrdinalIgnoreCase))
        {
            return fileName.Substring(UpdatedFilePrefix.Length);
        }
        throw new ArgumentException($"File name '{fileName}' does not start with '{UpdatedFilePrefix}' prefix.", nameof(updatedFile));
    }

    private async Task CreateOrUpdatePullRequestAsync(
        List<TreeFile> originalTreeItems,
        List<TreeFile> modifiedTreeItems,
        string originalFilesDirectory,
        int buildId,
        string title,
        string targetBranch)
    {
        // Use a deterministic head branch name so re-runs reuse the same PR. DarcLib's
        // SearchPullRequestsAsync requires a head branch on both GitHub and AzDO, so
        // looking up the PR by branch is the cleanest cross-provider option.
        string newBranchName = BuildHeadBranchName(targetBranch);
        int? existingPullRequestId = await GetExistingPullRequestAsync(newBranchName, targetBranch);

        // Choose the "base" tree for the diff:
        //   - Create path: the target branch's tree. The commit lands on a fresh branch
        //     reset to target by EnsureHeadBranchAsync, so target-relative diff is correct.
        //   - Update path: the existing PR head's tree. DarcLib's CommitFilesWithNoCloningAsync
        //     bases new commits on the current branch HEAD, so the diff MUST be computed
        //     against PR head — otherwise stale modifications from prior runs (files no
        //     longer in modifiedTreeItems, or files that should revert to target's content)
        //     would silently persist on the PR branch.
        List<TreeFile> baseTreeItems = existingPullRequestId is null
            ? originalTreeItems
            : await FetchOriginalTreeItemsAsync(newBranchName, originalFilesDirectory);

        // Compute the actual set of file changes and bail early if nothing differs.
        // NOTE: We do not merge the target branch into an existing PR branch before pushing
        // (the original Octokit-based tool did, via Repository.Merging.Create). DarcLib's
        // IRemoteGitRepo abstraction doesn't expose a server-side merge, and adding per-provider
        // escapes for it isn't justified for this use case. Consequence: a long-lived baseline
        // PR may show "behind target" in the PR UI. In practice these PRs merge quickly.
        List<GitFile> commitFiles = ComputeChangeSet(baseTreeItems, modifiedTreeItems, originalFilesDirectory);
        if (!ShouldMakeUpdates(commitFiles))
        {
            return;
        }

        string pullRequestBody = $"This PR was created by the `CreateBaselineUpdatePR` tool for build {buildId}. \n\n" +
                             $"The updated test results can be found at {BuildLink}{buildId} (internal Microsoft link)";
        string commitMessage = $"Update baselines for build {BuildLink}{buildId} (internal Microsoft link)";

        if (existingPullRequestId != null)
        {
            await UpdatePullRequestAsync(newBranchName, commitFiles, commitMessage, title, pullRequestBody, existingPullRequestId.Value);
        }
        else
        {
            await CreatePullRequestAsync(newBranchName, commitFiles, commitMessage, targetBranch, title, pullRequestBody, originalFilesDirectory);
        }
    }

    private async Task<int?> GetExistingPullRequestAsync(string headBranch, string targetBranch)
    {
        // Do NOT catch exceptions here. A lookup failure must propagate, because the caller
        // would otherwise take the "no PR exists" path, where EnsureHeadBranchAsync would
        // delete the deterministic head branch that backs the active PR. Failing the
        // pipeline cleanly on transient errors is recoverable; clobbering an open PR's
        // branch is not.
        IEnumerable<int> ids = await _gitClient.Repo.SearchPullRequestsAsync(
            _gitClient.RepoUri, headBranch, PrStatus.Open);

        // SearchPullRequestsAsync filters by source branch. Verify the target branch also matches.
        foreach (int id in ids)
        {
            PullRequest pr = await _gitClient.Repo.GetPullRequestAsync(_gitClient.BuildPullRequestApiUrl(id));
            if (string.Equals(pr.BaseBranch, targetBranch, StringComparison.Ordinal))
            {
                return id;
            }
        }
        return null;
    }

    private static bool ShouldMakeUpdates(List<GitFile> changes)
    {
        if (changes.Count == 0)
        {
            Log.LogInformation("No changes to commit. Skipping PR creation/updates.");
            return false;
        }
        
        return true;
    }

    private async Task UpdatePullRequestAsync(string branchName, List<GitFile> commitFiles, string commitMessage, string title, string body, int pullRequestId)
    {
        await _gitClient.Repo.CommitFilesWithNoCloningAsync(commitFiles, _gitClient.RepoUri, branchName, commitMessage);

        string apiUrl = _gitClient.BuildPullRequestApiUrl(pullRequestId);
        // Explicitly include Title in the update payload. DarcLib's AzureDevOpsClient
        // forwards null Title into a GitPullRequest sent through GitHttpClient.UpdatePullRequestAsync,
        // which serializes nulls and may either clear the PR title or reject the request.
        var pullRequestUpdate = new PullRequest
        {
            Title = title,
            Description = body
        };
        await _gitClient.Repo.UpdatePullRequestAsync(apiUrl, pullRequestUpdate);

        Log.LogInformation($"Updated existing pull request #{pullRequestId}. URL: {_gitClient.BuildPullRequestHtmlUrl(pullRequestId)}");
    }

    private async Task CreatePullRequestAsync(string newBranchName, List<GitFile> commitFiles, string commitMessage, string targetBranch, string title, string body, string originalFilesDirectory)
    {
        await EnsureHeadBranchAsync(newBranchName, targetBranch);
        await _gitClient.Repo.CommitFilesWithNoCloningAsync(commitFiles, _gitClient.RepoUri, newBranchName, commitMessage);

        // Safety net: never open a no-diff PR. Even though ComputeChangeSet found differences,
        // the commit can still land an empty tree (byte-identical to target) — most commonly when
        // another baseline PR updated the target branch with the equivalent content between when
        // this run computed its change set and when it committed. Verify real Git state before
        // creating the PR and clean up the branch we created if there is nothing to merge.
        if (!await CommitIntroducesTargetChangesAsync(commitFiles, targetBranch, originalFilesDirectory))
        {
            Log.LogInformation("No effective changes after commit; skipping PR creation.");
            await _gitClient.Repo.DeleteBranchAsync(_gitClient.RepoUri, newBranchName);
            return;
        }

        var newPullRequest = new PullRequest
        {
            Title = title,
            Description = body,
            BaseBranch = targetBranch,
            HeadBranch = newBranchName
        };
        PullRequest pullRequest = await _gitClient.Repo.CreatePullRequestAsync(_gitClient.RepoUri, newPullRequest);

        // DarcLib returns the provider's API URL (e.g. https://api.github.com/repos/<o>/<r>/pulls/<id>),
        // which isn't user-friendly. Convert to the human-facing HTML URL via the same helper the
        // update path uses, so both code paths log a clickable browser link.
        int pullRequestId = ParsePullRequestIdFromUrl(pullRequest.Url);
        Log.LogInformation($"Created pull request. URL: {_gitClient.BuildPullRequestHtmlUrl(pullRequestId)}");
    }

    // Determines whether committing `commitFiles` actually changed the repository tree, by checking
    // each file against the current target branch content. CommitFilesWithNoCloningAsync writes
    // these files on top of the head branch, which EnsureHeadBranchAsync had just reset to
    // `targetBranch`, so the head tree equals the target tree with `commitFiles` applied. If none
    // of the files change the current target content, the commit is empty and a PR would show no
    // diff. We re-read `targetBranch` (which this tool never writes to) rather than the freshly
    // pushed head branch, so the check is immune to the GitHub read-after-write lag that affects
    // newly created/updated refs.
    private async Task<bool> CommitIntroducesTargetChangesAsync(List<GitFile> commitFiles, string targetBranch, string desiredPath)
    {
        List<TreeFile> currentTarget = await FetchOriginalTreeItemsAsync(targetBranch, desiredPath);
        var targetByPath = currentTarget.ToDictionary(f => f.RelativePath, f => f.Content, StringComparer.Ordinal);
        string prefix = desiredPath.Replace('\\', '/').TrimEnd('/') + "/";

        foreach (GitFile file in commitFiles)
        {
            string relativePath = file.FilePath.StartsWith(prefix, StringComparison.Ordinal)
                ? file.FilePath[prefix.Length..]
                : file.FilePath;
            bool existsInTarget = targetByPath.TryGetValue(relativePath, out string? targetContent);

            if (file.Operation == GitFileOperation.Delete)
            {
                // A delete is a real change only if the file still exists on target.
                if (existsInTarget)
                {
                    return true;
                }
            }
            // file.Content is the value GitFile's constructor stored — i.e. exactly the bytes that
            // will be committed (GitFile folds Environment.NewLine to '\n' and ensures a trailing
            // '\n'; note that on the Linux CI agent Environment.NewLine is "\n", so CRLF is left
            // intact). Comparing it against the current target blob therefore detects a real change
            // and, conversely, only reports "no change" when the commit would genuinely be empty.
            else if (!existsInTarget || !string.Equals(targetContent, file.Content, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    // Both GitHub (.../pulls/<id>) and AzDO (.../pullRequests/<id>) API URLs end with the PR id,
    // and our BuildPullRequestApiUrl overrides produce the same shape. Pull the trailing integer
    // segment off rather than parsing the full URI per provider.
    private static int ParsePullRequestIdFromUrl(string url)
    {
        string trimmed = url.TrimEnd('/');
        int lastSlash = trimmed.LastIndexOf('/');
        string idSegment = lastSlash >= 0 ? trimmed[(lastSlash + 1)..] : trimmed;
        if (!int.TryParse(idSegment, out int id))
        {
            throw new InvalidOperationException(
                $"Could not parse pull request id from URL '{url}'.");
        }
        return id;
    }

    // Diff (originalTreeItems, modifiedTreeItems) → list of DarcLib GitFiles to commit.
    // Paths in the input lists are relative to `originalFilesDirectory`; output paths are
    // repo-relative. This is the boundary at which the in-memory TreeFile representation
    // is converted to DarcLib's commit-side model.
    private static List<GitFile> ComputeChangeSet(List<TreeFile> originalTreeItems, List<TreeFile> modifiedTreeItems, string originalFilesDirectory)
    {
        var originalByPath = originalTreeItems.ToDictionary(f => f.RelativePath, StringComparer.Ordinal);
        var modifiedByPath = modifiedTreeItems.ToDictionary(f => f.RelativePath, StringComparer.Ordinal);
        string prefix = originalFilesDirectory.Replace('\\', '/').TrimEnd('/') + "/";

        var changes = new List<GitFile>();

        foreach (var (path, modifiedItem) in modifiedByPath)
        {
            // Build the file exactly as it will be committed. DarcLib's GitFile constructor folds
            // Environment.NewLine to '\n' and always appends a trailing '\n' (so empty content
            // becomes "\n"); the committed bytes can therefore differ from the raw content. Detect
            // changes by comparing the original against the modified content in that same committed
            // form, otherwise a 0-byte baseline (whose committed form is "\n") is perpetually
            // re-written to "\n", producing a spurious no-op diff on every run. Real whitespace/
            // line-ending differences are still captured because both sides are normalized identically.
            GitFile candidate = new(prefix + path, modifiedItem.Content);
            if (!originalByPath.TryGetValue(path, out var originalItem)
                || new GitFile(prefix + path, originalItem.Content).Content != candidate.Content)
            {
                changes.Add(candidate);
            }
        }

        foreach (var (path, _) in originalByPath)
        {
            if (!modifiedByPath.ContainsKey(path))
            {
                changes.Add(new GitFile(prefix + path, "", ContentEncoding.Utf8, operation: GitFileOperation.Delete));
            }
        }

        return changes;
    }

    private async Task EnsureHeadBranchAsync(string headBranch, string targetBranch)
    {
        if (await _gitClient.Repo.DoesBranchExistAsync(_gitClient.RepoUri, headBranch))
        {
            // Orphan branch from a previously-closed PR. Delete it so the upcoming
            // CreateBranchAsync starts fresh from targetBranch. If deletion fails we MUST
            // surface the error: silently swallowing it would let the no-op CreateBranchAsync
            // (both GitHub's POST git/refs and DarcLib's AzDO client treat an existing ref
            // as success) pass the post-create DoesBranchExistAsync probe, and the next
            // CommitFilesWithNoCloningAsync would commit on top of stale content.
            await _gitClient.Repo.DeleteBranchAsync(_gitClient.RepoUri, headBranch);
        }

        Log.LogInformation($"Creating branch '{headBranch}' from '{targetBranch}'.");
        await _gitClient.Repo.CreateBranchAsync(_gitClient.RepoUri, headBranch, targetBranch);

        // Verify creation actually took. DarcLib's AzDO CreateBranchAsync silently
        // returns success even when GetLastCommitShaAsync failed to resolve the base
        // branch (it sends a request with a null newObjectId, which the server accepts
        // as a no-op). If we don't fail here, the subsequent CommitFilesWithNoCloningAsync gives
        // a confusing "couldn't find remote ref" error.
        //
        // The verification must tolerate GitHub's read-after-write eventual consistency: a single
        // immediate DoesBranchExistAsync probe frequently returns false for a branch that was in
        // fact created, which previously failed this step spuriously. Poll a few times before
        // giving up. In the genuine AzDO no-op case the branch never appears, so this still throws;
        // in the GitHub read-after-write-lag case it appears within a retry or two.
        bool branchExists = false;
        for (int attempt = 1; attempt <= BranchCreationVerificationAttempts; attempt++)
        {
            if (await _gitClient.Repo.DoesBranchExistAsync(_gitClient.RepoUri, headBranch))
            {
                branchExists = true;
                break;
            }

            if (attempt < BranchCreationVerificationAttempts)
            {
                Log.LogDebug(
                    $"Branch '{headBranch}' not yet visible on remote " +
                    $"(attempt {attempt}/{BranchCreationVerificationAttempts}); " +
                    $"retrying in {BranchCreationVerificationDelay.TotalSeconds}s.");
                await Task.Delay(BranchCreationVerificationDelay);
            }
        }

        if (!branchExists)
        {
            throw new InvalidOperationException(
                $"Branch '{headBranch}' was not created on '{_gitClient.RepoUri}' from base '{targetBranch}'. " +
                "This usually indicates the base branch could not be resolved on the remote.");
        }
        Log.LogInformation($"Branch '{headBranch}' exists on remote.");
    }

    // Builds a stable, branch-safe head-branch name for the PR. Includes a short hash of the
    // unsanitized inputs to avoid lossy collisions when the target branch name contains
    // characters that get folded to '-'.
    private string BuildHeadBranchName(string targetBranch)
    {
        string sanitized = SanitizeForBranchName(targetBranch);

        // When no identifier is supplied, preserve the exact historical branch name and hash
        // input so open PRs from single-job pipelines (sdk, license) are undisturbed.
        if (_identifier.Length == 0)
        {
            string hashNoIdentifier = ShortHash($"{_pipeline}|{targetBranch}");
            return $"pr-baseline-{_pipeline.ToString().ToLowerInvariant()}-{sanitized}-{hashNoIdentifier}";
        }

        // With an identifier, fold it into both the readable branch name and the hash so that
        // multiple jobs of the same pipeline targeting the same branch (e.g. the reproducibility
        // pipeline's SourceBuilt vs Msft jobs) resolve to distinct branches and therefore distinct PRs.
        string sanitizedIdentifier = SanitizeForBranchName(_identifier);
        string hash = ShortHash($"{_pipeline}|{_identifier}|{targetBranch}");
        return $"pr-baseline-{_pipeline.ToString().ToLowerInvariant()}-{sanitizedIdentifier}-{sanitized}-{hash}";
    }

    private static string SanitizeForBranchName(string s)
    {
        var sb = new StringBuilder(s.Length);
        foreach (char c in s)
        {
            sb.Append(char.IsLetterOrDigit(c) ? char.ToLowerInvariant(c) : '-');
        }
        return ConsecutiveDashesRegex.Replace(sb.ToString(), "-").Trim('-');
    }

    private static string ShortHash(string s)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(s));
        return Convert.ToHexString(bytes, 0, 4).ToLowerInvariant();
    }

    // In-memory representation of a baseline file fetched from / staged for the target tree.
    // Kept distinct from DarcLib's GitFile because (a) GitFile.Content semantics depend on
    // ContentEncoding (base64 vs UTF-8), and (b) the GitFile constructor normalizes newlines
    // and appends trailing '\n', both of which would corrupt content used for in-memory
    // comparisons. DarcLib GitFiles are only constructed at the commit boundary
    // (ComputeChangeSet → CommitFilesWithNoCloningAsync).
    private sealed record TreeFile(string RelativePath, string Content);
}
