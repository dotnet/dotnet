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
        var sourceManifest = await GetSourceManifestFromBranch("HEAD", repoRoot);

        LogInfo("Reading submodule paths and converting to glob patterns...");

        var submoduleGlobPatterns = sourceManifest.Submodules.Select(submodule => submodule.Path)
            .Select(pattern => ConvertSubmodulePathToVmrGlob(pattern))
            .ToList();

        LogInfo(string.Join(Environment.NewLine, submoduleGlobPatterns));

        var matcher = new Matcher();
        foreach (var pattern in submoduleGlobPatterns)
        {
            matcher.AddInclude(pattern);
        }

        var directory = new InMemoryDirectoryInfo("/", prInfo.ChangedFiles);

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

        return !modifiedSubmoduleFiles.Any();
    }

    private async Task<SourceManifest> GetSourceManifestFromBranch(string branchName, string repoRoot)
    {
        var sourceManifestContent = await _processManager.ExecuteGit(repoRoot, ["show", $"{branchName}:{VmrInfo.DefaultRelativeSourceManifestPath}"]);
        var sourceManifest = SourceManifest.FromJson(sourceManifestContent.StandardOutput);
        return sourceManifest;
    }
}
