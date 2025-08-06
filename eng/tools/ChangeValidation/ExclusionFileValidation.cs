// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.VirtualMonoRepo;
using Microsoft.DotNet.DarcLib.Models.VirtualMonoRepo;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using static ValidateVmrChanges.Validate;
using Microsoft.DotNet.DarcLib.Helpers;

namespace ValidateVmrChanges;

internal class ExclusionFileValidation : IValidationStep
{
    private static readonly int maxDisplayedFiles = 20;
    private readonly IVmrDependencyTracker _dependencyTracker;
    private readonly IProcessManager _processManager;

    private readonly string _repoRoot;

    internal ExclusionFileValidation(
        IVmrDependencyTracker dependencyTracker,
        IProcessManager processManager)
    {
        _dependencyTracker = dependencyTracker;
        _processManager = processManager;
        _repoRoot = _processManager.FindGitRoot(AppContext.BaseDirectory);
    }

    public string DisplayName => "Exclusion File Validation";

    public async Task<bool> Execute(PrInfo prInfo)
    {
        var originalExclusionRules = await GetExclusionPatternsFromBranch("origin/" + prInfo.TargetBranch);
        var newExclusionRules = await GetExclusionPatternsFromBranch("HEAD");


        var originalExcludedFiles = FindMatchingFiles(originalExclusionRules);
        var newExcludedFiles = FindMatchingFiles(newExclusionRules);

        var excludedFilesInPr = prInfo.ChangedFiles
            .Where(file => newExcludedFiles.Contains(NormalizePath(file)))
            .ToList();

        var filesMatchingNewExclusionRules = newExcludedFiles
            .Where(file => !originalExcludedFiles.Contains(NormalizePath(file)) && !prInfo.ChangedFiles.Contains(NormalizePath(file)))
            .ToList();

        foreach (var file in excludedFilesInPr.Take(maxDisplayedFiles).ToList())
        {
            LogError($"This PR modifies the file `{file}`, which is part of the excluded files defined in {VmrInfo.DefaultRelativeSourceMappingsPath}. If these changes are necessary, please contact the repository organizers.");
        }

        if (excludedFilesInPr.Count > maxDisplayedFiles)
        {
            LogError($"... {excludedFilesInPr.Count - maxDisplayedFiles} more excluded files detected in the PR. Only showing the first {maxDisplayedFiles} files.");
        }

        foreach (var file in filesMatchingNewExclusionRules.Take(maxDisplayedFiles).ToList())
        {
            LogError($"The new exclusion rules defined in {VmrInfo.DefaultRelativeSourceMappingsPath} include the VMR file `{file}`. If this file is not needed in the VMR, consider deleting it as part of the PR.");
        }

        if (filesMatchingNewExclusionRules.Count > maxDisplayedFiles)
        {
            LogError($"... {filesMatchingNewExclusionRules.Count - maxDisplayedFiles} more VMR files match the new exclusion rules. Only showing the first {maxDisplayedFiles} files.");
        }

        return excludedFilesInPr.Any() || filesMatchingNewExclusionRules.Any();
    }

    private async Task<List<string>> GetExclusionPatternsFromBranch(string branchName)
    {
        string originalBranch = (await _processManager.ExecuteGit(_repoRoot, "rev-parse", "abbrev-ref HEAD")).StandardOutput;
        try
        {
            await _processManager.ExecuteGit(_repoRoot, "checkout", branchName);
            await _dependencyTracker.RefreshMetadata(_repoRoot);
            var sourceMappings = _dependencyTracker.Mappings;

            return sourceMappings.Select(mapping => mapping.Exclude)
                .SelectMany(exclude => exclude)
                .Distinct()
                .ToList();
        }
        finally
        {
            await _processManager.ExecuteGit(_repoRoot, $"checkout {originalBranch}");
        }
    }
    
    private HashSet<string> FindMatchingFiles(List<string> globPatterns)
    {
        var repoRoot = _processManager.FindGitRoot(AppContext.BaseDirectory);

        var matcher = new Matcher();
        foreach (var pattern in globPatterns)
        {
            matcher.AddInclude(pattern);
        }

        var directoryInfo = new DirectoryInfo(repoRoot);
        var directoryWrapper = new DirectoryInfoWrapper(directoryInfo);
        var result = matcher.Execute(directoryWrapper);

        return result.Files
            .Select(file => file.Path)
            .ToHashSet();
    }
    
    internal static string NormalizePath(string path)
    {
        return path.Replace('\\', '/');
    }
}
