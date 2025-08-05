// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.DotNet.DarcLib.Helpers;

namespace ValidateVmrChanges;

internal static class Validate
{
    internal static void LogInfo(string message) => Console.WriteLine($"##vso[task.logissue type=information] {message}");
    internal static void LogWarning(string message) => Console.WriteLine($"##vso[task.logissue type=warning] {message}");
    internal static void LogError(string message) => Console.WriteLine($"##vso[task.logissue type=error] {message}");

    private static List<IValidationStep> _validationSteps = new List<IValidationStep>
    {
        new ToolingFilesValidation(),
        new SubmoduleValidation(),
        new ExclusionFileValidation()
    };

    internal static async Task<int> Main(string[] args)
    {
        PrInfo prInfo = SetupPrInfo();
        int stepCounter = 0;
        int failedSteps = 0;

        foreach (var validationStep in _validationSteps)
        {
            stepCounter++;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"{stepCounter}. Starting {validationStep.DisplayName}");
            bool validationSuccess = false;
            try
            {
                validationSuccess = await validationStep.Execute(prInfo);
            }
            catch (Exception)
            {
                LogError($"An unexpected error occurred during the validation step: {validationStep.DisplayName}.");
            }
            if (!validationSuccess)
            {
                failedSteps++;
                Console.WriteLine($"Validation step {validationStep.DisplayName} failed.");
            }
        }
        if (failedSteps > 0)
        {
            LogError($"Validation failed. {failedSteps} out of {_validationSteps.Count} steps failed.");
            Console.WriteLine("Please review the errors above and fix the issues before proceeding.");
            return 1;
        }
        return 0;
    }

    internal static string RunGitCommand(string arguments)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        string err = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new Exception($"Git command failed: {arguments}\n{err}");
        }

        return output;
    }

    private static PrInfo SetupPrInfo()
    {
        string? targetBranch = Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_TARGETBRANCH");

        if (string.IsNullOrEmpty(targetBranch))
        {
            throw new ArgumentException("Cannot determine PR target branch.");
        }

        RunGitCommand($"fetch origin {targetBranch}");
        string diffOutput = RunGitCommand($"diff --name-only origin/{targetBranch}...HEAD");

        var changedFiles = diffOutput
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(f => f.Replace('\\', '/').Trim())
            .ToList();

        return new PrInfo(targetBranch, changedFiles);
    }
}
