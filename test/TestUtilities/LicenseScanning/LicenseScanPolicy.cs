// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using TestUtilities;

namespace Microsoft.DotNet.SourceBuild.LicenseScanning;

/// <summary>
/// Defines the license-specific rules used to filter ScanCode results.
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
    /// Parses generic exclusion entries into license-specific records.
    /// </summary>
    public static Dictionary<string, LicenseExclusion> ParseExclusions(string content)
    {
        Dictionary<string, LicenseExclusion> exclusions = new(StringComparer.Ordinal);
        foreach (ExclusionFileEntry entry in ExclusionsHelper.ParseExclusions(content))
        {
            LicenseExclusion exclusion = new(
                NormalizePath(entry.Pattern),
                entry.Suffixes);
            exclusions[exclusion.Key] = exclusion;
        }

        return exclusions;
    }

    /// <summary>
    /// Applies the same file-globbing semantics used by the source-build tests.
    /// </summary>
    public static bool PathMatches(string pattern, string path) =>
        ExclusionsHelper.PathMatches(
            NormalizePath(pattern),
            NormalizePath(path));

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
