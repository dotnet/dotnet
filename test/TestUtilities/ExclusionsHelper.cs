// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileSystemGlobbing;

namespace TestUtilities;

public class ExclusionsHelper
{
    private const string NullSuffix = "NULL_SUFFIX";

    private readonly string _exclusionsFileName;

    private readonly string _baselineSubDir;

    private readonly string _logsDirectory;

    // Use this to narrow down the scope of exclusions to a specific category.
    // For instance, setting this to "vstest" will consider 
    // "src/vstest/exclusions.txt" but not "src/arcade/exclusions.txt".
    private readonly Regex? _exclusionRegex;

    private readonly ExclusionFileEntry[] _exclusions;

    private readonly Dictionary<string, HashSet<string>> _suffixToUnusedExclusions;

    public ExclusionsHelper(string exclusionsFileName, string logsDirectory, string baselineSubDir = "", string? exclusionRegexString = null)
    {
        if (exclusionsFileName is null)
        {
            throw new ArgumentNullException(nameof(exclusionsFileName));
        }

        _exclusionsFileName = exclusionsFileName;
        _logsDirectory = logsDirectory;
        _baselineSubDir = baselineSubDir;
        _exclusionRegex = string.IsNullOrWhiteSpace(exclusionRegexString) ? null : new Regex(exclusionRegexString);
        string exclusionsFilePath = BaselineHelper.GetBaselineFilePath(_exclusionsFileName, _baselineSubDir);
        _exclusions = ParseExclusions(File.ReadAllText(exclusionsFilePath))
            .Where(entry => _exclusionRegex is null || _exclusionRegex.IsMatch(entry.Pattern))
            .ToArray();
        _suffixToUnusedExclusions = new Dictionary<string, HashSet<string>>(
            _exclusions
                .SelectMany(entry => entry.Suffixes.Count == 0
                    ? new[] { new { entry.Pattern, Suffix = NullSuffix } }
                    : entry.Suffixes.Select(suffix => new { entry.Pattern, Suffix = suffix }))
                .GroupBy(entry => entry.Suffix, entry => entry.Pattern)
                .ToDictionary(
                    group => group.Key,
                    group => new HashSet<string>(group)));
    }

    public bool IsFileExcluded(string filePath, string suffix = NullSuffix)
    {
        if (suffix is null)
        {
            throw new ArgumentNullException(nameof(suffix));
        }

        ExclusionFileMatch? match = FindMatchingExclusion(
            _exclusions,
            filePath,
            suffix == NullSuffix ? null : suffix);
        if (match is null)
        {
            return false;
        }

        RemoveUsedExclusion(
            match.Entry.Pattern,
            match.Suffix ?? NullSuffix);
        return true;
    }

    /// <summary>
    /// Generates a new baseline file with the exclusions that were used during the test run.
    /// <param name="updatedFileTag">Optional tag to append to the updated file name.</param>
    /// <param name="additionalLines">Optional additional lines to append to the updated file.</param>
    /// </summary>
    public void GenerateNewBaselineFile(string? updatedFileTag = null, List<string>? additionalLines = null)
    {
        string exclusionsFilePath = BaselineHelper.GetBaselineFilePath(_exclusionsFileName, _baselineSubDir);

        string[] lines = File.ReadAllLines(exclusionsFilePath);

        var newLines = lines
            .Select(line => UpdateExclusionsLine(line))
            .Where(line => line is not null);

        if (additionalLines is not null)
        {
            newLines = newLines.Concat(additionalLines);
        }

        string updatedFileName = updatedFileTag is null
            ? $"Updated{_exclusionsFileName}"
            : $"Updated{Path.GetFileNameWithoutExtension(_exclusionsFileName)}.{updatedFileTag}{Path.GetExtension(_exclusionsFileName)}";
        string actualFilePath = Path.Combine(_logsDirectory, updatedFileName);
        File.WriteAllLines(actualFilePath, newLines!);
    }

    public static ExclusionFileEntry[] ParseExclusions(string content)
    {
        List<ExclusionFileEntry> exclusions = [];
        foreach (string rawLine in content.Split(["\r\n", "\n"], StringSplitOptions.None))
        {
            string line = rawLine.Trim();
            if (line.Length == 0 || line.StartsWith('#'))
            {
                continue;
            }

            int separator = line.IndexOf('|');
            if (separator == 0 ||
                (separator >= 0 && line.IndexOf('|', separator + 1) >= 0))
            {
                throw new FormatException(
                    $"Invalid exclusion: '{line}'. Expected a nonempty pattern " +
                    "followed by at most one pipe.");
            }

            string[] suffixes = separator < 0
                ? []
                : line[(separator + 1)..]
                    .Split(',', StringSplitOptions.TrimEntries);
            if (suffixes.Any(string.IsNullOrEmpty))
            {
                throw new FormatException(
                    $"Invalid exclusion: '{line}'. Suffixes must be nonempty.");
            }

            exclusions.Add(new ExclusionFileEntry(
                separator < 0 ? line : line[..separator].Trim(),
                suffixes));
        }

        return exclusions.ToArray();
    }

    public static bool PathMatches(string pattern, string path)
    {
        Matcher matcher = new();
        matcher.AddInclude(pattern);
        return matcher.Match(path).HasMatches;
    }

    public static ExclusionFileMatch? FindMatchingExclusion(
        IEnumerable<ExclusionFileEntry> exclusions,
        string path,
        string? suffix)
    {
        ExclusionFileEntry? wholeFileMatch = null;
        foreach (ExclusionFileEntry exclusion in exclusions)
        {
            if (!PathMatches(exclusion.Pattern, path))
            {
                continue;
            }

            string? matchingSuffix = exclusion.Suffixes.FirstOrDefault(candidate =>
                string.Equals(candidate, suffix, StringComparison.Ordinal));
            if (matchingSuffix is not null)
            {
                return new ExclusionFileMatch(exclusion, matchingSuffix);
            }

            if (exclusion.Suffixes.Count == 0)
            {
                wholeFileMatch ??= exclusion;
            }
        }

        return wholeFileMatch is null
            ? null
            : new ExclusionFileMatch(wholeFileMatch, Suffix: null);
    }

    private void RemoveUsedExclusion(string exclusion, string suffix)
    {
        if (_suffixToUnusedExclusions.TryGetValue(suffix, out HashSet<string>? exclusions))
        {
            exclusions.Remove(exclusion);
        }
    }

    private string? UpdateExclusionsLine(string line)
    {
        string[] parts = line.Split('|');
        string exclusion = parts[0];
        var unusedSuffixes = _suffixToUnusedExclusions.Where(pair => pair.Value.Contains(exclusion)).Select(pair => pair.Key).ToList();

        if (!unusedSuffixes.Any())
        {
            // Exclusion is used in all suffixes, so we can keep it as is
            return line;
        }

        if (parts.Length == 1)
        {
            if (unusedSuffixes.Contains(NullSuffix))
            {
                // Exclusion is unused in the default suffix, so we can remove it entirely
                return null;
            }
            // Line is duplicated for other suffixes, but null suffix is used so we can keep it as is
            return line;
        }

        string suffixString = parts[1];
        var originalSuffixes = suffixString.Split(',').Select(suffix => suffix.Trim()).ToList();
        var newSuffixes = originalSuffixes.Except(unusedSuffixes).ToList();

        if (newSuffixes.Count == 0)
        {
            // All suffixes were unused, so we can remove the line entirely
            return null;
        }

        return line.Replace(suffixString, string.Join(",", newSuffixes));
    }
}

public sealed record ExclusionFileEntry(
    string Pattern,
    IReadOnlyList<string> Suffixes);

public sealed record ExclusionFileMatch(
    ExclusionFileEntry Entry,
    string? Suffix);
