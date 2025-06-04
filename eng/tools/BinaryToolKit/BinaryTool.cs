// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Task = System.Threading.Tasks.Task;

namespace BinaryToolKit;

public static class BinaryTool
{
    public static async Task ExecuteAsync(
        TaskLoggingHelper log,
        string targetDirectory,
        string outputReportDirectory,
        string allowedBinariesFile,
        Modes mode)
    {
        DateTime startTime = DateTime.Now;

        log.LogMessage(MessageImportance.High, $"Starting binary tool at {startTime} in {mode} mode");

        // Run the tooling
        var detectedBinaries = await DetectBinaries.ExecuteAsync(log, targetDirectory, outputReportDirectory, allowedBinariesFile);

        if (mode == Modes.Validate)
        {
            ValidateBinaries(log, detectedBinaries, outputReportDirectory);
        }

        else if (mode == Modes.Clean)
        {
            RemoveBinaries(log, detectedBinaries, targetDirectory);
        }

        log.LogMessage(MessageImportance.High, "Finished all binary tasks. Took " + (DateTime.Now - startTime).TotalSeconds + " seconds.");
    }

    private static void ValidateBinaries(TaskLoggingHelper log, IEnumerable<string> newBinaries, string outputReportDirectory)
    {
        string newBinariesFile = Path.Combine(outputReportDirectory, "NewBinaries.txt");
        File.WriteAllLines(newBinariesFile, newBinaries);

        if (newBinaries.Any())
        {
            log.LogMessage(MessageImportance.Low, "New binaries:");

            foreach (var binary in newBinaries)
            {
                log.LogMessage(MessageImportance.Low, $"    {binary}");
            }

            log.LogMessage(MessageImportance.High, $"{newBinaries.Count()} new binaries. Check '{newBinariesFile}' for details.");
        }
    }

    private static void RemoveBinaries(TaskLoggingHelper log, IEnumerable<string> binariesToRemove, string targetDirectory)
    {
        log.LogMessage(MessageImportance.High, $"Removing binaries from '{targetDirectory}'...");
        
        foreach (var binary in binariesToRemove)
        {
            File.Delete(Path.Combine(targetDirectory, binary));
            log.LogMessage(MessageImportance.Low, $"    {binary}");
        }

        log.LogMessage(MessageImportance.High, $"Finished binary removal. Removed {binariesToRemove.Count()} binaries.");
    }
}
