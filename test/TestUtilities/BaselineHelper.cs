// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit.Abstractions;

namespace TestUtilities;

public static class BaselineHelper
{
    public static string GetAssetsDirectory() => Path.Combine(Directory.GetCurrentDirectory(), "assets");

    public static string GetBaselineFilePath(string baselineFileName, string baselineSubDir = "") =>
        Path.Combine(GetAssetsDirectory(), baselineSubDir, baselineFileName);

    /// <summary>
    /// Compares actual entries against a baseline file. Returns a message describing the differences, or null if they match.
    /// </summary>
    public static string? CompareEntries(string baselineFileName, IOrderedEnumerable<string> actualEntries)
    {
        IEnumerable<string> baseline = File.ReadAllLines(GetBaselineFilePath(baselineFileName));
        string[] missingEntries = actualEntries.Except(baseline).ToArray();
        string[] extraEntries = baseline.Except(actualEntries).ToArray();

        string? message = null;
        if (missingEntries.Length > 0)
        {
            message = $"Missing entries in '{baselineFileName}' baseline: {Environment.NewLine}{string.Join(Environment.NewLine, missingEntries)}{Environment.NewLine}{Environment.NewLine}";
        }

        if (extraEntries.Length > 0)
        {
            message += $"Extra entries in '{baselineFileName}' baseline: {Environment.NewLine}{string.Join(Environment.NewLine, extraEntries)}{Environment.NewLine}{Environment.NewLine}";
        }

        return message;
    }

    /// <summary>
    /// Writes actual contents to the logs directory and compares against the baseline file.
    /// Returns a message describing the differences, or null if they match.
    /// </summary>
    public static string? CompareBaselineContents(string baselineFileName, string actualContents, ITestOutputHelper outputHelper, string logsDirectory, string baselineSubDir = "")
    {
        string actualFilePath = Path.Combine(logsDirectory, $"Updated{baselineFileName}");
        File.WriteAllText(actualFilePath, actualContents);

        return CompareFiles(GetBaselineFilePath(baselineFileName, baselineSubDir), actualFilePath, outputHelper);
    }

    /// <summary>
    /// Compares two files. Returns a message describing the differences, or null if they match.
    /// </summary>
    public static string? CompareFiles(string expectedFilePath, string actualFilePath, ITestOutputHelper outputHelper)
    {
        string baselineFileText = File.ReadAllText(expectedFilePath).Trim();
        string actualFileText = File.ReadAllText(actualFilePath).Trim();

        if (baselineFileText != actualFileText)
        {
            // Retrieve a diff in order to provide a UX which calls out the diffs.
            string diff = DiffFiles(expectedFilePath, actualFilePath, outputHelper);
            return $"{Environment.NewLine}Expected file '{expectedFilePath}' does not match actual file '{actualFilePath}`.  {Environment.NewLine}"
                + $"{diff}{Environment.NewLine}";
        }

        return null;
    }

    public static string DiffFiles(string file1Path, string file2Path, ITestOutputHelper outputHelper)
    {
        (System.Diagnostics.Process Process, string StdOut, string StdErr) diffResult =
            ExecuteHelper.ExecuteProcess("git", $"diff --no-index {file1Path} {file2Path}", outputHelper);

        return diffResult.StdOut;
    }
}
