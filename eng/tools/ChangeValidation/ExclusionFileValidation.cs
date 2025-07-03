// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using static ValidateVmrChanges.Validate;

namespace ValidateVmrChanges;

internal class ExclusionFileValidation
{
    private static readonly string SourceMappingsPath = "src/source-mappings.json";

    internal static List<ProcessingMessage> VerifyFileExclusions(List<string> diffFiles, string targetBranch)
    {
        List<ProcessingMessage> messages = [];

        var originalFileExclusionRules = GetExclusionPatternsFromBranch(targetBranch);
        var newFileExclusionRules = GetExclusionPatternsFromBranch("HEAD");

        if (originalFileExclusionRules == null || !originalFileExclusionRules.Any())
        {
            AddProcessingMessage(messages, Error($"No exclusion rules found in `{SourceMappingsPath}` on the target branch, or the file does not exist."));
            return messages;
        }

        if (newFileExclusionRules == null || !newFileExclusionRules.Any())
        {
            AddProcessingMessage(messages, Error($"No exclusion rules found in `{SourceMappingsPath}` on the PR branch, or the file does not exist."));
            return messages;
        }

        var originalExcludedFiles = FilesMatchingGlobs(originalFileExclusionRules);
        var newExcludedFiles = FilesMatchingGlobs(newFileExclusionRules);

        var excludedFilesInPr = diffFiles
            .Where(file => newExcludedFiles.Contains(NormalizePath(file)))
            .ToList();

        var filesMatchingNewExclusionRules = newExcludedFiles
            .Where(file => !originalExcludedFiles.Contains(NormalizePath(file)) && !diffFiles.Contains(NormalizePath(file)))
            .ToList();

        if (excludedFilesInPr.Any())
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"This PR adds or modifies {excludedFilesInPr.Count} file(s) that are in the exclusion list:");
            sb.AppendLine("These files will not be synced from/to the VMR. Consider deleting them if they are not needed.");
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
            sb.AppendLine($"Warning: This PR modifies exclusion rules in {SourceMappingsPath} and adds {filesMatchingNewExclusionRules.Count} file(s) that match new exclusion rules:");
            sb.AppendLine("These files will not be synced from/to the VMR. Consider deleting them if they are not needed.");
            foreach (var file in filesMatchingNewExclusionRules)
            {
                sb.AppendLine($" - {file}");
            }
            string warningMessage = sb.ToString();
            AddProcessingMessage(messages, Warn(warningMessage));
        }
        return messages;
    }

    private static List<string> GetExclusionPatternsFromBranch(string branchName)
    {
        var fileContents = RunGitCommand($"show {branchName}:src/source-mappings.json");

        if (string.IsNullOrEmpty(fileContents))
        {
            return new List<string>();
        }
        var jsonObject = JObject.Parse(fileContents);
        var mappings = jsonObject["mappings"]?.ToObject<List<string>>() ?? new List<string>();

        List<string> allExcludes = new List<string>();

        foreach (string exclusion in mappings)
        {
            JToken excludeToken = mapping["exclude"];
            if (excludeToken != null && excludeToken.Type == JTokenType.Array)
            {
                allExcludes.AddRange(excludeToken.ToObject<List<string>>());
            }
        }

        foreach (string exclude in allExcludes)
        {
            Console.WriteLine($"Found exclusion rule: {exclude}")
        }

        return globPatterns;
    }
    
    private static HashSet<string> FilesMatchingGlobs(List<string> globPatterns)
    {
        var repoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", ".."));

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
