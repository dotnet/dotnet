// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Octokit;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Task = System.Threading.Tasks.Task;
using FileMode = Octokit.FileMode;

namespace Microsoft.DotNet.Baselines.Tasks;

internal class PRCreator
{
    private readonly TaskLoggingHelper _logger;
    private readonly string _gitHubOrg;
    private readonly string _gitHubRepoName;
    private readonly GitHubClient _client;
    private const string BuildLink = "https://dev.azure.com/dnceng/internal/_build/results?buildId=";
    private const string TreeMode = "040000";
    private const int MaxRetries = 10;

    public PRCreator(TaskLoggingHelper logger, string gitHubOrg, string gitHubRepoName, string gitHubToken)
    {
        _logger = logger;

        // Create a new GitHub client
        _client = new GitHubClient(new ProductHeaderValue(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name));
        var authToken = new Credentials(gitHubToken);
        _client.Credentials = authToken;
        _gitHubOrg = gitHubOrg;
        _gitHubRepoName = gitHubRepoName;
    }

    public async Task<bool> ExecuteAsync(
        string targetDirectory,
        List<string> updatedFiles,
        int buildId,
        string title,
        string targetBranch,
        string defaultBaselineContent,
        bool unionExclusionsBaselines)
    {
        DateTime startTime = DateTime.Now.ToUniversalTime();

        _logger.LogMessage(MessageImportance.High, $"Starting PR creation at {startTime} UTC.");

        // Fetch the files within the desired path from the original tree. must be a relative path
        TreeResponse originalTreeResponse = await ApiRequestWithRetries(() => _client.Git.Tree.Get(_gitHubOrg, _gitHubRepoName, targetBranch));
        List<NewTreeItem> originalTreeItems = await FetchOriginalTreeItemsAsync(originalTreeResponse, targetBranch, targetDirectory);

        // Update the test results tree
        Dictionary<string, HashSet<string>> parsedUpdatedFiles = ParseAndGroupUpdatedFiles(updatedFiles);
        originalTreeItems = await UpdateAllFilesAsync(parsedUpdatedFiles, originalTreeItems, unionExclusionsBaselines, defaultBaselineContent);
        var testResultsTreeResponse = await CreateTreeFromItemsAsync(originalTreeItems);
        var parentTreeResponse = await CreateParentTreeAsync(testResultsTreeResponse, originalTreeResponse, targetDirectory);

        await CreateOrUpdatePullRequestAsync(parentTreeResponse, buildId, title, targetBranch);

        return !_logger.HasLoggedErrors;
    }

    private async Task<List<NewTreeItem>> FetchOriginalTreeItemsAsync(
        TreeResponse? treeResponse,
        string targetBranch,
        string targetDirectory)
    {
        if (treeResponse == null)
        {
            _logger.LogError($"Failed to fetch the original tree for branch '{targetBranch}' in repository '{_gitHubOrg}/{_gitHubRepoName}'.");
            throw new InvalidOperationException("Original tree response is invalid.");
        }

        ConcurrentBag<NewTreeItem> treeItems = new();
        await FetchOriginalTreeItemsAsync(treeResponse, treeItems, targetBranch, targetDirectory);

        List<NewTreeItem> items = treeItems.ToList();
        if (!items.Any())
        {
            _logger.LogError($"No files found in the original tree for branch '{targetBranch}' in repository '{_gitHubOrg}/{_gitHubRepoName}'.");
            throw new InvalidOperationException("No files found in the original tree.");
        }

        return items;
    }

    private async Task FetchOriginalTreeItemsAsync(
        TreeResponse? treeResponse,
        ConcurrentBag<NewTreeItem> treeItems,
        string targetBranch,
        string targetDirectory,
        string relativePath = "")
    {
        if (treeResponse == null)
        {
            return;
        }

        await Parallel.ForEachAsync(treeResponse.Tree, async (item, cancellationToken) =>
        {
            string path = Path.Combine(relativePath, item.Path);
            if (!path.StartsWith(targetDirectory) && !targetDirectory.StartsWith(path))
            {
                return;
            }

            if (item.Type == TreeType.Tree)
            {
                TreeResponse subTree = await ApiRequestWithRetries(() => _client.Git.Tree.Get(_gitHubOrg, _gitHubRepoName, item.Sha));
                await FetchOriginalTreeItemsAsync(subTree, treeItems, targetBranch, targetDirectory, path);
            }
            else
            {
                var newItem = new NewTreeItem
                {
                    Path = Path.GetRelativePath(targetDirectory, path),
                    Mode = item.Mode,
                    Type = item.Type.Value,
                    Sha = item.Sha
                };

                treeItems.Add(newItem);
            }
        });
    }

    // Return a dictionary using the filename without the
    // "Updated" prefix (if present) and anything before the first '.' as the key
    private Dictionary<string, HashSet<string>> ParseAndGroupUpdatedFiles(List<string> updatedFiles) =>
        updatedFiles
            .Select(updatedFile => {
                if (!File.Exists(updatedFile))
                {
                    throw new ArgumentException($"Updated file path '{updatedFile}' is not a valid file.");
                }
                return updatedFile;
            })
            .GroupBy(updatedFile => ParseUpdatedFileName(updatedFile).Split('.')[0])
            .ToDictionary(
                group => group.Key,
                group => new HashSet<string>(group)
            );

    private async Task<List<NewTreeItem>> UpdateAllFilesAsync(
        Dictionary<string, HashSet<string>> updatedFiles,
        List<NewTreeItem> tree,
        bool unionExclusionsBaselines,
        string defaultBaselineContent)
    {
        foreach (var updatedFile in updatedFiles)
        {
            if (updatedFile.Key.Contains("Exclusions"))
            {
                tree = await UpdateExclusionFileAsync(updatedFile.Key, updatedFile.Value, tree, unionExclusionsBaselines);
            }
            else
            {
                tree = await UpdateRegularFilesAsync(updatedFile.Value, tree, defaultBaselineContent);
            }
        }
        return tree;
    }

    private async Task<List<NewTreeItem>> UpdateExclusionFileAsync(
        string fileNameKey,
        HashSet<string> updatedFiles,
        List<NewTreeItem> tree,
        bool union = false)
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

            // Find the key in the tree, download the blob, and convert it to utf8
            var originalTreeItem = tree
                .Where(item => item.Path.Contains(fileNameKey))
                .FirstOrDefault();

            if (originalTreeItem != null)
            {
                var originalBlob = await ApiRequestWithRetries(() => _client.Git.Blob.Get(_gitHubOrg, _gitHubRepoName, originalTreeItem.Sha));
                content = Encoding.UTF8.GetString(Convert.FromBase64String(originalBlob.Content));
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
        return await UpdateFileAsync(tree, content, fileNameKey, updatedFilePath);
    }

    private async Task<List<NewTreeItem>> UpdateRegularFilesAsync(
        HashSet<string> updatedFiles,
        List<NewTreeItem> tree,
        string defaultBaselineContent)
    {
        foreach (string filePath in updatedFiles)
        {
            string? content = File.ReadAllText(filePath);
            if (!string.IsNullOrEmpty(defaultBaselineContent) && content == defaultBaselineContent)
            {
                content = null;
            }
            string originalFileName = Path.GetFileName(ParseUpdatedFileName(filePath));
            tree = await UpdateFileAsync(tree, content, originalFileName, originalFileName);
        }
        return tree;
    }

    private async Task<List<NewTreeItem>> UpdateFileAsync(
        List<NewTreeItem> tree,
        string? content,
        string searchFileName,
        string updatedPath)
    {
        var originalTreeItem = tree
            .Where(item => item.Path.Contains(searchFileName))
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
            var blob = await CreateBlobAsync(content);
            tree.Add(new NewTreeItem
            {
                Type = TreeType.Blob,
                Mode = FileMode.File,
                Path = updatedPath,
                Sha = blob.Sha
            });
        }
        else
        {
            // Path in the tree, update the sha and the content
            var blob = await CreateBlobAsync(content);
            originalTreeItem.Sha = blob.Sha;
        }
        return tree;
    }

    private async Task<BlobReference> CreateBlobAsync(string content)
    {
        var blob = new NewBlob
        {
            Content = content,
            Encoding = EncodingType.Utf8
        };
        return await ApiRequestWithRetries(() => _client.Git.Blob.Create(_gitHubOrg, _gitHubRepoName, blob));
    }

    private string ParseUpdatedFileName(string updatedFile)
    {
        try
        {
            return updatedFile.Split("Updated")[1];
        }
        catch
        {
            return updatedFile;
        }
    }

    private async Task<TreeResponse> CreateTreeFromItemsAsync(List<NewTreeItem> items, string path = "")
    {
        List<NewTreeItem> newTreeItems = [];

        var groups = items.GroupBy(item => Path.GetDirectoryName(item.Path));
        foreach (var group in groups)
        {
            if (string.IsNullOrEmpty(group.Key) || group.Key == path)
            {
                // These items are in the current directory, so add them to the new tree items
                foreach (var item in group)
                {
                    if (item.Type != TreeType.Tree)
                    {
                        newTreeItems.Add(new NewTreeItem
                        {
                            Path = path == string.Empty ? item.Path : Path.GetRelativePath(path, item.Path),
                            Mode = item.Mode,
                            Type = item.Type,
                            Sha = item.Sha
                        });
                    }
                }
            }
            else
            {
                // These items are in a subdirectory, so recursively create a tree for them
                var subtreeResponse = await CreateTreeFromItemsAsync(group.ToList(), group.Key);
                newTreeItems.Add(new NewTreeItem
                {
                    Path = group.Key,
                    Mode = TreeMode,
                    Type = TreeType.Tree,
                    Sha = subtreeResponse.Sha
                });
            }
        }

        var newTree = new NewTree();
        foreach (var item in newTreeItems)
        {
            newTree.Tree.Add(item);
        }
        return await ApiRequestWithRetries(() => _client.Git.Tree.Create(_gitHubOrg, _gitHubRepoName, newTree));
    }

    private async Task<TreeResponse> CreateParentTreeAsync(
        TreeResponse testResultsTreeResponse,
        TreeResponse originalTreeResponse,
        string targetDirectory)
    {
        // Create a new tree for the parent directory
        NewTree parentTree = new NewTree { BaseTree = originalTreeResponse.Sha };

        //  Connect the updated test results tree
        parentTree.Tree.Add(new NewTreeItem
        {
            Path = targetDirectory,
            Mode = TreeMode,
            Type = TreeType.Tree,
            Sha = testResultsTreeResponse.Sha
        });

        return await ApiRequestWithRetries(() => _client.Git.Tree.Create(_gitHubOrg, _gitHubRepoName, parentTree));
    }

    private async Task CreateOrUpdatePullRequestAsync(TreeResponse parentTreeResponse, int buildId, string title, string targetBranch)
    {
        var existingPullRequest = await GetExistingPullRequestAsync(title, targetBranch);

        // Create the branch name and get the head reference
        string newBranchName = string.Empty;
        string headSha = await GetHeadShaAsync(targetBranch);
        if (existingPullRequest == null)
        {
            string utcTime = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            newBranchName = $"pr-baseline-{utcTime}";
        }
        else
        {
            newBranchName = existingPullRequest.Head.Ref;

            try
            {
                // Merge the target branch into the existing pull request
                var merge = new NewMerge(newBranchName, headSha);
                await ApiRequestWithRetries(() => _client.Repository.Merging.Create(_gitHubOrg, _gitHubRepoName, merge));
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Failed to merge the target branch into the existing pull request: {e.Message}");
                _logger.LogWarning("Continuing with updating the existing pull request. You may need to resolve conflicts manually in the PR.");
            }

            headSha = await GetHeadShaAsync(newBranchName);
        }

        string commitSha = await CreateCommitAsync(
            parentTreeResponse.Sha,
            headSha,
            $"Update baselines for build {BuildLink}{buildId} (internal Microsoft link)");

        if (await ShouldMakeUpdatesAsync(headSha, commitSha))
        {
            string pullRequestBody = $"This PR was created by `Microsoft.DotNet.Baselines.Tasks.CreateUpdatePR` for build {buildId}. \n\n" +
                                 $"The updated test results can be found at {BuildLink}{buildId} (internal Microsoft link)";
            if (existingPullRequest != null)
            {
                await UpdatePullRequestAsync(newBranchName, commitSha, pullRequestBody, existingPullRequest);
            }
            else
            {
                await CreatePullRequestAsync(newBranchName, commitSha, targetBranch, title, pullRequestBody);
            }
        }
    }

    private async Task<PullRequest?> GetExistingPullRequestAsync(string title, string targetBranch)
    {
        var request = new PullRequestRequest
        {
            Base = targetBranch
        };

        var existingPullRequests = await ApiRequestWithRetries(() =>
            _client.PullRequest.GetAllForRepository(_gitHubOrg, _gitHubRepoName, request));

        return existingPullRequests.FirstOrDefault(pr => pr.Title == title);
    }

    private async Task<string> CreateCommitAsync(string newSha, string headSha, string commitMessage)
    {
        var newCommit = new NewCommit(commitMessage, newSha, headSha);
        var commit = await ApiRequestWithRetries(() => _client.Git.Commit.Create(_gitHubOrg, _gitHubRepoName, newCommit));
        return commit.Sha;
    }

    private async Task<bool> ShouldMakeUpdatesAsync(string headSha, string commitSha)
    {
        var comparison = await ApiRequestWithRetries(() => _client.Repository.Commit.Compare(_gitHubOrg, _gitHubRepoName, headSha, commitSha));
        if (!comparison.Files.Any())
        {
            _logger.LogMessage(MessageImportance.High, "No changes to commit. Skipping PR creation/updates.");
            return false;
        }
        return true;
    }

    private async Task UpdatePullRequestAsync(string branchName, string commitSha, string body, PullRequest pullRequest)
    {
        await UpdateReferenceAsync(branchName, commitSha);

        var pullRequestUpdate = new PullRequestUpdate
        {
            Body = body
        };
        await ApiRequestWithRetries(() => _client.PullRequest.Update(_gitHubOrg, _gitHubRepoName, pullRequest.Number, pullRequestUpdate));

        _logger.LogMessage(MessageImportance.High, $"Updated existing pull request #{pullRequest.Number}. URL: {pullRequest.HtmlUrl}");
    }

    private async Task CreatePullRequestAsync(string newBranchName, string commitSha, string targetBranch, string title, string body)
    {
        await CreateReferenceAsync(newBranchName, commitSha);

        var newPullRequest = new NewPullRequest(title, newBranchName, targetBranch)
        {
            Body = body
        };
        var pullRequest = await ApiRequestWithRetries(() => _client.PullRequest.Create(_gitHubOrg, _gitHubRepoName, newPullRequest));

        _logger.LogMessage(MessageImportance.High, $"Created pull request #{pullRequest.Number}. URL: {pullRequest.HtmlUrl}");
    }

    private async Task<string> GetHeadShaAsync(string branchName)
    {
        var reference = await ApiRequestWithRetries(() => _client.Git.Reference.Get(_gitHubOrg, _gitHubRepoName, $"heads/{branchName}"));
        return reference.Object.Sha;
    }

    private async Task UpdateReferenceAsync(string branchName, string commitSha)
    {
        var referenceUpdate = new ReferenceUpdate(commitSha);
        await ApiRequestWithRetries(() => _client.Git.Reference.Update(_gitHubOrg, _gitHubRepoName, $"heads/{branchName}", referenceUpdate));
    }

    private async Task CreateReferenceAsync(string branchName, string commitSha)
    {
        var newReference = new NewReference($"refs/heads/{branchName}", commitSha);
        await ApiRequestWithRetries(() => _client.Git.Reference.Create(_gitHubOrg, _gitHubRepoName, newReference));
    }

    private async Task<T> ApiRequestWithRetries<T>(Func<Task<T>> action)
    {
        int attempt = 0;
        int delayMilliseconds = 1000;
        while (true)
        {
            try
            {
                return await action();
            }
            catch (RateLimitExceededException ex)
            {
                var resetTime = ex.Reset.UtcDateTime;
                var delay = resetTime - DateTime.UtcNow;
                _logger.LogWarning($"Rate limit exceeded. Retrying after {delay.TotalSeconds} seconds...");
                await Task.Delay(delay);
            }
            catch (Exception ex) when (
                attempt < MaxRetries
                && (ex is ApiException || ex is HttpRequestException)
                && (ex.InnerException is TaskCanceledException))
            {
                attempt++;
                _logger.LogWarning($"Attempt {attempt} failed: {ex.Message}. Retrying in {delayMilliseconds}ms...");
                await Task.Delay(delayMilliseconds * attempt); // Exponential backoff
            }
        }
    }
}
