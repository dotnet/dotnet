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
    private const string Utf16Marker = "UTF-16";
    private const int ChunkSize = 4096;
    private static readonly Regex GitCleanRegex = new Regex(@"Would (remove|skip)( repository)? (.*)");

    public static async Task<IList<string>> ExecuteAsync(
        TaskLoggingHelper log,
        string targetDirectory,
        string outputReportDirectory,
        string? allowedBinariesFile)
    {
        log.LogMessage(MessageImportance.High, $"Detecting binaries in '{targetDirectory}' not listed in '{allowedBinariesFile}'...");

        IEnumerable<string> patterns = ParseAllowedBinariesFile(log, allowedBinariesFile);
        var usedPatterns = new ConcurrentBag<string>();
        var newBinaries = new ConcurrentBag<string>();

        IEnumerable<(string pattern, Matcher matcher)> patternMatchers = patterns.Select(p =>
        {
            var m = new Matcher(StringComparison.Ordinal);
            m.AddInclude(p);
            return (pattern: p, matcher: m);
        });

        await Parallel.ForEachAsync(Directory.EnumerateFiles(targetDirectory, "*", SearchOption.AllDirectories), async (file, _) =>
        {
            bool matched = false;

            foreach (var (pattern, matcher) in patternMatchers)
            {
                if (matcher.Match(targetDirectory, file).HasMatches)
                {
                    usedPatterns.Add(pattern);
                    matched = true;
                    break;
                }
            }

            if (!matched)
            {
                if (await IsBinaryAsync(log, file))
                {
                    newBinaries.Add(file.Substring(targetDirectory.Length + 1));
                }
            }
        });

        var unusedPatterns = new HashSet<string>(patterns.Except(usedPatterns));
        UpdateAllowedBinariesFile(log, allowedBinariesFile, outputReportDirectory, unusedPatterns);

        log.LogMessage(MessageImportance.High, $"Finished binary detection.");

        return newBinaries.ToList();
    }

    private static async Task<bool> IsBinaryAsync(TaskLoggingHelper log, string filePath)
    {
        // Using the GNU diff heuristic to determine if a file is binary or not.
        // For more details, refer to the GNU diff manual: 
        // https://www.gnu.org/software/diffutils/manual/html_node/Binary.html

        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        using (BinaryReader br = new BinaryReader(fs))
        {
            byte[] buffer = new byte[ChunkSize];
            int bytesRead = br.Read(buffer, 0, ChunkSize);
            for (int i = 0; i < bytesRead; i++)
            {
                if (buffer[i] == 0)
                {
                    // Need to check that the file is not UTF-16 encoded
                    // because heuristic can return false positives
                    return await IsNotUTF16Async(log, filePath);
                }
            }
        }
        return false;
    }

    private static async Task<bool> IsNotUTF16Async(TaskLoggingHelper log, string file)
    {
        if (Environment.OSVersion.Platform == PlatformID.Unix)
        {
            string output = await ExecuteProcessAsync(log, "file", $"\"{file}\"");
            output = output.Split(":")[1].Trim();

            if (output.Contains(Utf16Marker))
            {
                return false;
            }
        }
        return true;
    }

    private static async Task<string> ExecuteProcessAsync(TaskLoggingHelper log, string executable, string arguments)
    {
        ProcessStartInfo psi = new()
        {
            FileName = executable,
            Arguments = arguments,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        var proc = Process.Start(psi)!;

        string output = await proc.StandardOutput.ReadToEndAsync();
        string error = await proc.StandardError.ReadToEndAsync();

        await proc.WaitForExitAsync();

        if (!string.IsNullOrEmpty(error))
        {
            log.LogError(error);
        }

        return output;
    }

    private static IEnumerable<string> ParseAllowedBinariesFile(TaskLoggingHelper log, string? file, List<string>? knownFiles = null)
    {
        knownFiles ??= new List<string>();
        if (!File.Exists(file))
        {
            throw new ArgumentException($"AllowedBinariesFile '{file}' does not exist.");
        }

        if (knownFiles.Contains(file))
        {
            throw new InvalidOperationException($"Duplicate import of allowed binaries file: '{file}'.");
        }

        knownFiles.Add(file);

        foreach (string line in File.ReadLines(file))
        {
            string trimmedLine = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith("#"))
            {
                continue;
            }

            trimmedLine = RemoveCommentsAndWhitespace(trimmedLine);

            if (TryGetImportFile(trimmedLine, file, out string importFile))
            {
                foreach (string importedLine in ParseAllowedBinariesFile(log, importFile, knownFiles))
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

    private static void UpdateAllowedBinariesFile(TaskLoggingHelper log, string? file, string outputReportDirectory, HashSet<string> unusedPatterns)
    {
        if (File.Exists(file) && unusedPatterns.Any())
        {
            List<string> newLines = new List<string>();
            foreach (string line in File.ReadLines(file))
            {
                string trimmedLine = RemoveCommentsAndWhitespace(line);
                if (unusedPatterns.Contains(trimmedLine))
                {
                    continue;
                }

                if (TryGetImportFile(trimmedLine, file, out string importFile))
                {
                    UpdateAllowedBinariesFile(log, importFile, outputReportDirectory, unusedPatterns);
                }
                newLines.Add(line);
            }

            string updatedFile = Path.Combine(outputReportDirectory, "Updated" + Path.GetFileName(file));

            File.WriteAllLines(updatedFile, newLines);

            log.LogMessage(MessageImportance.High, $"    Updated allowed binaries file '{Path.GetFileName(file)}' written to '{updatedFile}'");
        }
    }

    private static bool TryGetImportFile(string line, string currentFile, out string importFile)
    {
        importFile = string.Empty;
        if (line.StartsWith("import:"))
        {
            importFile = line.Substring("import:".Length).Trim();
            if (!Path.IsPathFullyQualified(importFile))
            {
                var currentDirectory = Path.GetDirectoryName(currentFile) ?? Directory.GetCurrentDirectory();
                importFile = Path.Combine(currentDirectory, importFile);
            }
            return true;
        }
        return false;
    }

    private static string RemoveCommentsAndWhitespace(string line)
        => line.Split('#')[0].Trim();
}
