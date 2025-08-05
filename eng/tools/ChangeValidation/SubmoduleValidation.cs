// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
using static ValidateVmrChanges.Validate;

namespace ValidateVmrChanges;

internal class SubmoduleValidation : IValidationStep
{
    private static readonly int maxDisplayedFiles = 20;
    private readonly IVmrDependencyTracker _dependencyTracker;
    private readonly IProcessManager _processManager;
    private readonly string _repoRoot;

    public string DisplayName => "Submodule Validation";

    internal SubmoduleValidation()
    {
        _processManager = new ProcessManager(NullLogger<ProcessManager>.Instance, "git");
        VmrInfo vmrInfo = new VmrInfo(_processManager.FindGitRoot(""), "");
        ILogger<VmrDependencyTracker> nullLogger = NullLogger<VmrDependencyTracker>.Instance;
        SourceMappingParser parser = new SourceMappingParser(vmrInfo, new FileSystem());
        SourceManifest sourceManifest = new SourceManifest([], []);
        _dependencyTracker = new VmrDependencyTracker(vmrInfo, new FileSystem(), parser, sourceManifest, nullLogger);
        _repoRoot = _processManager.FindGitRoot(AppContext.BaseDirectory);
    }

    public async Task<bool> Execute(PrInfo prInfo)
    {
        var sourceManifest = await GetSourceManifestFromBranch("HEAD");
        var submoduleGlobPatterns = sourceManifest.Submodules.Select(submodule => submodule.Path)
            .Select(pattern => pattern.TrimStart('/')) // Remove leading slashes for globbing
            .ToList();

        var matcher = new Matcher();

        foreach (var pattern in submoduleGlobPatterns)
        {
            matcher.AddInclude(pattern);
        }

        var directory = new InMemoryDirectoryInfo("", prInfo.ChangedFiles);

        var result = matcher.Execute(directory);

        var modifiedSubmoduleFiles = result.Files.Select(f => f.Path).ToList();

        foreach(var file in modifiedSubmoduleFiles.Take(maxDisplayedFiles).ToList())
        {
            LogError($"This PR modifies the submodule file `{file}`, which is not allowed. Submodule files are managed by the repository maintainers and should not be modified directly.");
        }

        if (modifiedSubmoduleFiles.Count > maxDisplayedFiles)
        {
            LogError($"... {modifiedSubmoduleFiles.Count - maxDisplayedFiles} more submodule files detected in the PR. Only showing the first {maxDisplayedFiles} files.");
        }

        if (modifiedSubmoduleFiles.Any())
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private async Task<SourceManifest> GetSourceManifestFromBranch(string branchName)
    {
        var sourceManifestContent = await _processManager.ExecuteGit(_repoRoot, "show", $"{branchName}:{VmrInfo.DefaultRelativeSourceManifestPath}");
        var sourceManifest = SourceManifest.FromJson(sourceManifestContent.StandardOutput);
        return sourceManifest;
    }
}
