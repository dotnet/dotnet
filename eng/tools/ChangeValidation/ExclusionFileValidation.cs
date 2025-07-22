// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using static ValidateVmrChanges.Validate;

namespace ValidateVmrChanges;

internal class ExclusionFileValidation
{
    private static readonly string SourceMappingsPath = "src/source-mappings.json";

    internal static List<ProcessingMessage> VerifyFileExclusions(List<string> diffFiles, string targetBranch)
    {
        List<ProcessingMessage> messages = [];

        string remoteTargetBranch = "origin/" + targetBranch;
        var originalExclusionRules = GetExclusionPatternsFromBranch(remoteTargetBranch);

        if (originalExclusionRules == null || !originalExclusionRules.Any())
        {
            AddProcessingMessage(messages, Error($"No exclusion rules found in `{SourceMappingsPath}` on the target branch, or the file does not exist."));
            return messages;
        }

        var newExclusionRules = GetExclusionPatternsFromBranch("HEAD");

        if (newExclusionRules == null || !newExclusionRules.Any())
        {
            AddProcessingMessage(messages, Error($"No exclusion rules found in `{SourceMappingsPath}` on the PR branch, or the file does not exist."));
            return messages;
        }

        var originalExcludedFiles = VmrFileMatches(originalExclusionRules);
        var newExcludedFiles = VmrFileMatches(newExclusionRules);

        var excludedFilesInPr = diffFiles
            .Where(file => newExcludedFiles.Contains(NormalizePath(file)))
            .ToList();

        var filesMatchingNewExclusionRules = newExcludedFiles
            .Where(file => !originalExcludedFiles.Contains(NormalizePath(file)) && !diffFiles.Contains(NormalizePath(file)))
            .ToList();

        if (excludedFilesInPr.Any())
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"This PR modifies or creates {excludedFilesInPr.Count} file(s) that are in the exclusion list.");
            sb.AppendLine("If these files are not needed in the VMR, please delete them in this PR.");
            foreach (var file in excludedFilesInPr)
            {
                sb.AppendLine($" - {file}");
            }
            string warningMessage = sb.ToString();
            AddProcessingMessage(messages, Warn(warningMessage));
        }

        if (filesMatchingNewExclusionRules.Any())
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"This PR modifies exclusion rules in {SourceMappingsPath}, leading to {filesMatchingNewExclusionRules.Count} newly excluded file(s) in the VMR.");
            sb.AppendLine("If these files are not needed in the VMR, please delete them in this PR.");
            foreach (var file in filesMatchingNewExclusionRules)
            {
                sb.AppendLine($" - {file}");
            }
            string warningMessage = sb.ToString();
            AddProcessingMessage(messages, Warn(warningMessage));
        }

        if (!excludedFilesInPr.Any() && !filesMatchingNewExclusionRules.Any())
        {
            AddProcessingMessage(messages, Success($"Exclusion file validation succeeded."));
        } else
        {
            AddProcessingMessage(messages, Warn($"Exclusion file validation failed."));
        }
        return messages;
    }

    private static List<string> GetExclusionPatternsFromBranch(string branchName)
    {
        Console.WriteLine($"Retrieving {SourceMappingsPath} from branch `{branchName}`...");
        string? fileContents = null;
        try
        {
            fileContents = RunGitCommand($"show {branchName}:src/source-mappings.json");
        } catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving {SourceMappingsPath} from branch `{branchName}`: {ex.Message}");
            return [];
        }

        if (string.IsNullOrEmpty(fileContents))
        {
            Console.WriteLine($"Error: Could not retrieve {SourceMappingsPath} from branch `{branchName}`. The file may not exist or is empty.");
            return [];
        }

        JArray? mappings = null;
        try
        {
            var jsonRoot = JObject.Parse(fileContents);

            if (jsonRoot.TryGetValue("mappings", out JToken? mappingsToken) && mappingsToken?.Type == JTokenType.Array)
            {
                mappings = (JArray)mappingsToken;
            }
            else
            {
                Console.WriteLine($"Cannot find mappings in {SourceMappingsPath}");
                return [];
            }
        }
        catch (Exception)
        {
            Console.WriteLine($"Error parsing {SourceMappingsPath}");
            return [];
        }

        List<string> allExcludes = new List<string>();

        foreach (var mapping in mappings.OfType<JObject>())
        {
            var excludes = mapping["exclude"] as JArray;
            if (excludes != null)
            {
                allExcludes.AddRange(excludes.Values<string>().Where(x => x != null)!);
            }
        }

        return allExcludes;
    }
    
    private static HashSet<string> VmrFileMatches(List<string> globPatterns)
    {
        var repoRoot = FindGitRepoRoot(AppContext.BaseDirectory);

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

    private static string FindGitRepoRoot(string startDirectory)
    {
        var dir = new DirectoryInfo(startDirectory);
        while (dir != null)
        {
            var gitPath = Path.Combine(dir.FullName, ".git");
            if (Directory.Exists(gitPath) || File.Exists(gitPath))
            {
                return dir.FullName;
            }
            dir = dir.Parent;
        }
        throw new InvalidOperationException("Could not find the Git repository root.");
    }
}
