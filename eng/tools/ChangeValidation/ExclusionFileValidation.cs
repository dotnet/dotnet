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
using Microsoft.DotNet.DarcLib.Helpers;
using static ChangeValidation.Validation;

namespace ChangeValidation;

internal class ExclusionFileValidation : IValidationStep
{
    private static readonly int maxDisplayedFiles = 20;
    private readonly IVmrDependencyTracker _dependencyTracker;
    private readonly IProcessManager _processManager;
    private readonly IVmrInfo _vmrInfo;
    private readonly ILocalGitRepoFactory _localGitRepoFactory;

    public ExclusionFileValidation(
        IVmrDependencyTracker dependencyTracker,
        IProcessManager processManager,
        ILocalGitRepoFactory localGitRepoFactory,
        IVmrInfo vmrInfo)
    {
        _dependencyTracker = dependencyTracker;
        _processManager = processManager;
        _localGitRepoFactory = localGitRepoFactory;
        _vmrInfo = vmrInfo;
    }

    public string DisplayName => "Exclusion File Validation";

    public async Task<bool> Validate(PrInfo prInfo)
    {
        ILocalGitRepo vmr = _localGitRepoFactory.Create(_vmrInfo.VmrPath);

        var newExclusionRules = await GetExclusionPatterns(); // We are already checked to the PR Head commit, just parse exclusions from file
        var originalExclusionRules = await GetExclusionPatternsFromBranch(vmr, prInfo.TargetBranch);

        var newExcludedFiles = FindMatchingFiles(newExclusionRules);
        var originalExcludedFiles = FindMatchingFiles(originalExclusionRules);

        var excludedFilesInPr = prInfo.ChangedFiles
            .Where(file => newExcludedFiles.Contains(NormalizePath(file)))
            .ToList();

        var filesMatchingNewExclusionRules = newExcludedFiles
            .Where(file => !originalExcludedFiles.Contains(NormalizePath(file)) && !prInfo.ChangedFiles.Contains(NormalizePath(file)))
            .ToList();

        foreach (var file in excludedFilesInPr.Take(maxDisplayedFiles).ToList())
        {
            LogError($"This PR modifies the file `{file}`, which is part of the excluded files" +
                $" defined in {VmrInfo.DefaultRelativeSourceMappingsPath}. If these changes are" +
                $" necessary, please contact @dotnet/product-construction.");
        }

        if (excludedFilesInPr.Count > maxDisplayedFiles)
        {
            LogError($"... {excludedFilesInPr.Count - maxDisplayedFiles} more excluded files detected" +
                $" in the PR (only showing the first {maxDisplayedFiles} files).");
        }

        foreach (var file in filesMatchingNewExclusionRules.Take(maxDisplayedFiles).ToList())
        {
            LogError($"The new exclusion rules defined in {VmrInfo.DefaultRelativeSourceMappingsPath} " +
                $"include the VMR file `{file}`. If this file is not needed in the VMR, consider " +
                $"deleting it as part of the PR.");
        }

        if (filesMatchingNewExclusionRules.Count > maxDisplayedFiles)
        {
            LogError($"... {filesMatchingNewExclusionRules.Count - maxDisplayedFiles} more VMR files " +
                $"match the new exclusion rules (only showing the first {maxDisplayedFiles} files). " +
                "Please review the modifications made to exclusion rules");
        }

        return !excludedFilesInPr.Any() && !filesMatchingNewExclusionRules.Any();
    }

    private async Task<List<string>> GetExclusionPatternsFromBranch(ILocalGitRepo vmr, string branchName)
    {
        string originalRef = await vmr.GetCheckedOutBranchAsync();
        try
        {
            LogInfo($"Checking out branch {branchName} to get exclusion patterns.");
            await vmr.CheckoutAsync(branchName);
            return await GetExclusionPatterns();
        }
        finally
        {
            await vmr.CheckoutAsync(originalRef);
        }
    }

    private async Task<List<string>> GetExclusionPatterns()
    {
        await _dependencyTracker.RefreshMetadata();

        var sourceMappings = _dependencyTracker.Mappings;

        var exclusionPatterns = sourceMappings.SelectMany(mapping => GetVmrGlobFromSourceMapping(mapping))
            .Distinct()
            .ToList();

        LogInfo("Successfully parsed exclusion patterns...");
        LogInfo(string.Join(Environment.NewLine, exclusionPatterns));

        return exclusionPatterns;
    }
    
    private List<string> GetVmrGlobFromSourceMapping(SourceMapping mapping)
    {
        return mapping.Exclude.Select(exclusion => "src/" + mapping.Name + "/" + NormalizePath(exclusion)).ToList();
    }

    private HashSet<string> FindMatchingFiles(List<string> globPatterns)
    {
        var matcher = new Matcher();

        foreach (var pattern in globPatterns)
        {
            matcher.AddInclude(pattern);
        }

        var directoryInfo = new DirectoryInfo(_vmrInfo.VmrPath);
        var directoryWrapper = new DirectoryInfoWrapper(directoryInfo);
        var result = matcher.Execute(directoryWrapper);

        var res = result.Files
            .Select(file => file.Path)
            .ToHashSet();

        return res;
    }
    
    internal static string NormalizePath(string path)
    {
        return path.Replace('\\', '/');
    }
}
