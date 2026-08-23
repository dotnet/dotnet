// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Extensions.FileSystemGlobbing;
using Task = System.Threading.Tasks.Task;

namespace BinaryToolKit;

public static class DetectBinaries
{
    private static readonly Regex GitCleanRegex = new Regex(@"Would (remove|skip)( repository)? (.*)");

    public static IList<string> Execute(
        TaskLoggingHelper log,
        TaskEnvironment taskEnvironment,
        string targetDirectory,
        string outputReportDirectory,
        string allowedBinariesFile)
    {
        log.LogMessage(MessageImportance.High, $"Detecting binaries in '{targetDirectory}' not listed in '{allowedBinariesFile}'...");

        // Resolve once and use for both enumeration and the relative-path math below, so that a
        // relative targetDirectory cannot produce a mismatched prefix length.
        string absoluteTargetDirectory = taskEnvironment.GetAbsolutePath(targetDirectory);

        IEnumerable<string> patterns = ParseAllowedBinariesFile(log, taskEnvironment, allowedBinariesFile);
        var usedPatterns = new ConcurrentBag<string>();
        var newBinaries = new ConcurrentBag<string>();

        List<(string pattern, Matcher matcher)> patternMatchers = patterns.Select(p =>
        {
            var m = new Matcher(StringComparison.Ordinal);
            m.AddInclude(p);
            return (pattern: p, matcher: m);
        }).ToList();

        foreach (var file in Directory.EnumerateFiles(absoluteTargetDirectory, "*", new EnumerationOptions() { AttributesToSkip = FileAttributes.ReparsePoint, RecurseSubdirectories = true }))
        {
            // This code is meant for finding binaries in source code repositories.
            // Most files will be non-binary. We want to avoid checking each of those against the patternMatchers [O(N*M)], so we first check if the file is binary.
            if (IsBinary(taskEnvironment, file))
            {
                bool matched = false;

                foreach (var (pattern, matcher) in patternMatchers)
                {
                    if (matcher.Match(absoluteTargetDirectory, file).HasMatches)
                    {
                        usedPatterns.Add(pattern);
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    newBinaries.Add(file.Substring(absoluteTargetDirectory.Length + 1));
                }
            }
        }

        var unusedPatterns = new HashSet<string>(patterns.Except(usedPatterns));
        UpdateAllowedBinariesFile(log, taskEnvironment, allowedBinariesFile, outputReportDirectory, unusedPatterns);

        log.LogMessage(MessageImportance.High, $"Finished binary detection.");

        return newBinaries.ToList();
    }

    private static bool IsBinary(TaskEnvironment taskEnvironment, string filePath)
    {
        // Using the GNU diff heuristic to determine if a file is binary or not.
        // For more details, refer to the GNU diff manual: 
        // https://www.gnu.org/software/diffutils/manual/html_node/Binary.html

        using (FileStream fs = new FileStream(taskEnvironment.GetAbsolutePath(filePath), FileMode.Open, FileAccess.Read))
        {
            Span<byte> buffer = stackalloc byte[4096];
            int bytesRead = fs.Read(buffer);
            buffer = buffer[..bytesRead];

            bool hasZeroByte = buffer.IndexOf((byte)0) != -1;
            bool hasUTF16ByteOrderMarker = buffer.StartsWith((ReadOnlySpan<byte>)[0xFE, 0xFF]) || buffer.StartsWith((ReadOnlySpan<byte>)[0xFF, 0xFE]);

            return hasZeroByte && !hasUTF16ByteOrderMarker;
        }
    }

    private static IEnumerable<string> ParseAllowedBinariesFile(TaskLoggingHelper log, TaskEnvironment taskEnvironment, string file, List<string>? knownFiles = null)
    {
        knownFiles ??= new List<string>();
        if (string.IsNullOrEmpty(file) || !File.Exists(taskEnvironment.GetAbsolutePath(file)))
        {
            throw new ArgumentException($"AllowedBinariesFile '{file}' does not exist.");
        }

        if (knownFiles.Contains(file))
        {
            throw new InvalidOperationException($"Duplicate import of allowed binaries file: '{file}'.");
        }

        knownFiles.Add(file);

        foreach (string line in File.ReadLines(taskEnvironment.GetAbsolutePath(file)))
        {
            string trimmedLine = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith("#"))
            {
                continue;
            }

            trimmedLine = RemoveCommentsAndWhitespace(trimmedLine);

            if (TryGetImportFile(taskEnvironment, trimmedLine, file, out string importFile))
            {
                foreach (string importedLine in ParseAllowedBinariesFile(log, taskEnvironment, importFile, knownFiles))
                {
                    yield return importedLine;
                }
            }
            else
            {
                yield return trimmedLine;
            }
        }
    }

    private static void UpdateAllowedBinariesFile(TaskLoggingHelper log, TaskEnvironment taskEnvironment, string file, string outputReportDirectory, HashSet<string> unusedPatterns)
    {
        if (!string.IsNullOrEmpty(file) && File.Exists(taskEnvironment.GetAbsolutePath(file)) && unusedPatterns.Any())
        {
            List<string> newLines = new List<string>();
            foreach (string line in File.ReadLines(taskEnvironment.GetAbsolutePath(file)))
            {
                string trimmedLine = RemoveCommentsAndWhitespace(line);
                if (unusedPatterns.Contains(trimmedLine))
                {
                    continue;
                }

                if (TryGetImportFile(taskEnvironment, trimmedLine, file, out string importFile))
                {
                    UpdateAllowedBinariesFile(log, taskEnvironment, importFile, outputReportDirectory, unusedPatterns);
                }
                newLines.Add(line);
            }

            string updatedFile = Path.Combine(outputReportDirectory, "Updated" + Path.GetFileName(file));

            File.WriteAllLines(taskEnvironment.GetAbsolutePath(updatedFile), newLines);

            log.LogMessage(MessageImportance.High, $"    Updated allowed binaries file '{Path.GetFileName(file)}' written to '{updatedFile}'");
        }
    }

    private static bool TryGetImportFile(TaskEnvironment taskEnvironment, string line, string currentFile, out string importFile)
    {
        importFile = string.Empty;
        if (line.StartsWith("import:"))
        {
            importFile = line.Substring("import:".Length).Trim();
            if (!Path.IsPathFullyQualified(importFile))
            {
                var currentDirectory = Path.GetDirectoryName(currentFile) ?? taskEnvironment.ProjectDirectory;
                importFile = Path.Combine(currentDirectory, importFile);
            }
            return true;
        }
        return false;
    }

    private static string RemoveCommentsAndWhitespace(string line)
        => line.Split('#')[0].Trim();
}
