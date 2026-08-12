// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.DotNet.DarcLib.VirtualMonoRepo;
using Microsoft.DotNet.DarcLib.Models.VirtualMonoRepo;
using static ChangeValidation.Validation;

namespace ChangeValidation;

internal class SubmoduleValidation : IValidationStep
{
    private static readonly int maxDisplayedFiles = 20;
    private readonly IVmrDependencyTracker _dependencyTracker;
    private readonly IProcessManager _processManager;

    public string DisplayName => "Submodule Validation";

    private static string ConvertSubmodulePathToVmrGlob(string submodulePath) =>
        "src/" + submodulePath.Replace('\\', '/').TrimStart('/').TrimEnd('/') + "/**";

    public SubmoduleValidation(
        IVmrDependencyTracker vmrDependencyTracker,
        IProcessManager processManager)
    {
        _dependencyTracker = vmrDependencyTracker;  
        _processManager = processManager;
    }

    public async Task<bool> Validate(PrInfo prInfo)
    {
        var repoRoot = _processManager.FindGitRoot(AppContext.BaseDirectory);

        // Read the source manifest at both HEAD and the merge base so we can tell which
        // submodule entries actually changed as part of this PR.
        var headManifest = await GetSourceManifestFromBranch("HEAD", repoRoot);
        var baseManifest = await GetSourceManifestFromBranch(prInfo.MergeBaseCommit, repoRoot);

        var baseByPath = baseManifest.Submodules.ToDictionary(s => s.Path, s => s, StringComparer.Ordinal);
        var headByPath = headManifest.Submodules.ToDictionary(s => s.Path, s => s, StringComparer.Ordinal);

        // Adding or removing submodules is not allowed in the VMR. If the set of submodule paths
        // differs between the merge base and HEAD, a submodule was added or removed, so fail.
        var addedSubmodules = headByPath.Keys.Except(baseByPath.Keys, StringComparer.Ordinal).ToList();
        var removedSubmodules = baseByPath.Keys.Except(headByPath.Keys, StringComparer.Ordinal).ToList();
        if (addedSubmodules.Count > 0 || removedSubmodules.Count > 0)
        {
            foreach (var path in addedSubmodules)
            {
                LogError($"This PR adds a new submodule `{path}` to `{VmrInfo.DefaultRelativeSourceManifestPath}`. Adding submodules to the VMR is not allowed.");
            }

            foreach (var path in removedSubmodules)
            {
                LogError($"This PR removes the submodule `{path}` from `{VmrInfo.DefaultRelativeSourceManifestPath}`. Removing submodules from the VMR is not allowed.");
            }

            return false;
        }

        // Both manifests now contain the same set of submodules. A submodule entry counts as a
        // valid (reset) change when its CommitSha / RemoteUri differ between the merge base and HEAD.
        var changedEntryPaths = new HashSet<string>(StringComparer.Ordinal);
        foreach (var (path, headEntry) in headByPath)
        {
            var baseEntry = baseByPath[path];
            if (!string.Equals(baseEntry.CommitSha, headEntry.CommitSha, StringComparison.Ordinal) ||
                !string.Equals(baseEntry.RemoteUri, headEntry.RemoteUri, StringComparison.Ordinal))
            {
                changedEntryPaths.Add(path);
            }
        }

        // Determine, per submodule, which of the PR's changed files belong to that submodule.
        var changedFilesDirectory = new InMemoryDirectoryInfo("/", prInfo.ChangedFiles);
        var submodulesWithFileChanges = new Dictionary<string, List<string>>(StringComparer.Ordinal);
        foreach (var path in headByPath.Keys)
        {
            var matcher = new Matcher();
            matcher.AddInclude(ConvertSubmodulePathToVmrGlob(path));

            var matchedFiles = matcher.Execute(changedFilesDirectory).Files.Select(f => f.Path).ToList();
            if (matchedFiles.Count > 0)
            {
                submodulesWithFileChanges[path] = matchedFiles;
            }
        }

        var manifestFileChanged = prInfo.ChangedFiles.Any(f =>
            string.Equals(f, VmrInfo.DefaultRelativeSourceManifestPath, StringComparison.Ordinal));

        if (submodulesWithFileChanges.Count == 0)
        {
            if (manifestFileChanged)
            {
                LogError($"The file `{VmrInfo.DefaultRelativeSourceManifestPath}` is a protected file that may only be modified as part of a submodule reset (i.e. alongside the corresponding submodule change).");
                return false;
            }

            return true;
        }

        var violatingSubmodules = submodulesWithFileChanges.Keys
            .Where(path => !changedEntryPaths.Contains(path))
            .ToList();

        if (violatingSubmodules.Count == 0)
        {
            LogInfo($"Submodule changes are allowed because each modified submodule has a corresponding updated entry in `{VmrInfo.DefaultRelativeSourceManifestPath}` (submodule reset).");
            LogInfo($"Validated submodule paths: {string.Join(", ", submodulesWithFileChanges.Keys.OrderBy(p => p, StringComparer.Ordinal))}");
            return true;
        }

        var violatingFiles = violatingSubmodules
            .SelectMany(path => submodulesWithFileChanges[path])
            .ToList();

        foreach (var file in violatingFiles.Take(maxDisplayedFiles))
        {
            LogError($"This PR modifies the submodule file `{file}`, but the corresponding submodule entry in `{VmrInfo.DefaultRelativeSourceManifestPath}` was not updated. Submodule changes are only permitted as part of a submodule reset that updates that submodule's entry (CommitSha) in the source manifest.");
        }

        if (violatingFiles.Count > maxDisplayedFiles)
        {
            LogError($"... {violatingFiles.Count - maxDisplayedFiles} more submodule files detected in the PR. Only showing the first {maxDisplayedFiles} files.");
        }

        return false;
    }

    private async Task<SourceManifest> GetSourceManifestFromBranch(string branchName, string repoRoot)
    {
        var sourceManifestContent = await _processManager.ExecuteGit(repoRoot, ["show", $"{branchName}:{VmrInfo.DefaultRelativeSourceManifestPath}"]);
        var sourceManifest = SourceManifest.FromJson(sourceManifestContent.StandardOutput);
        return sourceManifest;
    }
}
