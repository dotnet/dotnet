// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.FileSystemGlobbing;

namespace Microsoft.DotNet.SourceBuild.LicenseScanning;

/// <summary>
/// Defines the parsing and matching rules used to filter ScanCode results.
/// </summary>
public static class LicenseScanPolicy
{
    /// <summary>
    /// Extracts the individual identifiers from a ScanCode license expression.
    /// </summary>
    /// <remarks>
    /// License scanning treats AND and OR identically because filtering is performed
    /// per identifier. This intentionally does not implement general SPDX expression
    /// semantics.
    /// </remarks>
    public static string[] SplitExpression(string? expression) =>
        expression is null
            ? []
            : expression
                .Replace("(", "", StringComparison.Ordinal)
                .Replace(")", "", StringComparison.Ordinal)
                .Replace(" AND ", ",", StringComparison.Ordinal)
                .Replace(" OR ", ",", StringComparison.Ordinal)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    /// <summary>
    /// Parses license exclusions into a stable, path-and-license keyed collection.
    /// </summary>
    /// <remarks>
    /// A line containing only a path excludes every license for that path. A pipe
    /// followed by comma-separated identifiers limits the exclusion to those
    /// identifiers. Blank lines and lines beginning with <c>#</c> are ignored.
    /// </remarks>
    public static Dictionary<string, LicenseExclusion> ParseExclusions(string content)
    {
        Dictionary<string, LicenseExclusion> exclusions = new(StringComparer.Ordinal);
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
                    $"Invalid license exclusion: '{line}'. Expected a nonempty path " +
                    "followed by at most one pipe.");
            }

            string[] licenses = separator < 0
                ? []
                : line[(separator + 1)..]
                    .Split(',', StringSplitOptions.TrimEntries);
            if (licenses.Any(string.IsNullOrEmpty))
            {
                throw new FormatException(
                    $"Invalid license exclusion: '{line}'. License identifiers must " +
                    "be nonempty.");
            }

            LicenseExclusion exclusion = separator < 0
                ? new LicenseExclusion(NormalizePath(line), [])
                : new LicenseExclusion(
                    NormalizePath(line[..separator].Trim()),
                    licenses);
            exclusions[exclusion.Key] = exclusion;
        }

        return exclusions;
    }

    /// <summary>
    /// Applies the same file-globbing semantics used by the source-build tests.
    /// </summary>
    public static bool PathMatches(string pattern, string path)
    {
        Matcher matcher = new();
        matcher.AddInclude(NormalizePath(pattern));
        return matcher.Match(NormalizePath(path)).HasMatches;
    }

    /// <summary>
    /// Converts paths to the forward-slash form used by ScanCode and baseline files.
    /// </summary>
    public static string NormalizePath(string value)
    {
        string normalized = value.Replace('\\', '/');
        while (normalized.StartsWith("./", StringComparison.Ordinal))
        {
            normalized = normalized[2..];
        }

        return normalized;
    }
}

/// <summary>
/// Represents either a whole-file exclusion or a set of license-specific exclusions
/// for one literal or globbed VMR path.
/// </summary>
public sealed record LicenseExclusion(string Path, IReadOnlyList<string> Licenses)
{
    // The null separator cannot occur in a path or license identifier, so it keeps
    // path-only and path-plus-license entries distinct without ambiguous punctuation.
    public string Key => Path + '\0' + string.Join(',', Licenses);
}
