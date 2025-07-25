// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Linq;

namespace ValidateVmrChanges;

internal static class Validate
{
    internal static ProcessingMessage Success(string message) => new ProcessingMessage(message, WarningLevel.Success);
    internal static ProcessingMessage Info(string message) => new ProcessingMessage(message, WarningLevel.None);
    internal static ProcessingMessage Warn(string message) => new ProcessingMessage(message, WarningLevel.Warning);
    internal static ProcessingMessage Error(string message) => new ProcessingMessage(message, WarningLevel.Error);

    internal static int Main(string[] args)
    {
        string? targetBranch = Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_TARGETBRANCH");

        if (string.IsNullOrEmpty(targetBranch))
        {
            Console.WriteLine("Error: The target branch is not specified. Please set the SYSTEM_PULLREQUEST_TARGETBRANCH environment variable.");
            return 1;
        }

        List<ProcessingMessage> processingMessages = new List<ProcessingMessage>();

        try
        {
            RunGitCommand($"fetch origin {targetBranch}");
            string diffOutput = RunGitCommand($"diff --name-only origin/{targetBranch}...HEAD");

            var changedFiles = diffOutput
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(f => f.Replace('\\', '/').Trim())
                .ToList();

            // Run Tooling Files Validation
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("1. Starting Tooling Files Validation...");
            List<ProcessingMessage> toolingMessages;
            try
            {
                toolingMessages = ToolingFilesValidation.VerifyMaestroFileChanges(changedFiles);
            }
            catch (Exception)
            {
                toolingMessages = new List<ProcessingMessage>();
                AddProcessingMessage(processingMessages, Error("Error: Failed to perform validation on tooling files."));
            }
            processingMessages.AddRange(toolingMessages);

            // Run Submodule Validation
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("2. Starting Submodule Validation...");
            List<ProcessingMessage> submoduleMessages;
            try
            {
                submoduleMessages = SubmoduleValidation.VerifySubModuleFileChanges(changedFiles);
            }
            catch (Exception)
            {
                submoduleMessages = new List<ProcessingMessage>();
                AddProcessingMessage(processingMessages, Error("Error: Failed to perform validation on submodule files."));
            }
            processingMessages.AddRange(submoduleMessages);

            // Run Exclusion File Validation
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("3. Starting Exclusion File Validation...");
            List<ProcessingMessage> exclusionMessages;
            try
            {
                exclusionMessages = ExclusionFileValidation.VerifyFileExclusions(changedFiles, targetBranch);
            }
            catch (Exception)
            {
                exclusionMessages = new List<ProcessingMessage>();
                AddProcessingMessage(processingMessages, Error("Error: Failed to perform validation on exclusion files."));
            }
            processingMessages.AddRange(exclusionMessages);
        }
        catch (Exception)
        {
            Console.WriteLine($"An unexpected error occurred during the validation process.");
            return 1;
        }

        var warnings = processingMessages
            .Where(m => m.WarningLevel == WarningLevel.Warning)
            .ToList();

        var errors = processingMessages
            .Where(m => m.WarningLevel == WarningLevel.Error)
            .ToList();

        if (errors.Any())
        {
            Console.WriteLine($"VMR Change Validation failed with {errors.Count} error(s) and {warnings.Count} warning(s).");
            return 1;
        }
        else
        {
            Console.WriteLine($"VMR Change Validation completed successfully with {warnings.Count} warning(s).");
            return 0;
        }
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

    internal static void AddProcessingMessage(List<ProcessingMessage> processingMessages, ProcessingMessage processingMessage)
    {
        var lines = processingMessage.Message.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        if (processingMessage.WarningLevel == WarningLevel.Error)
        {
            foreach (var line in lines)
            {
                Console.WriteLine($"##vso[task.logissue type=error] {line}");
            }
        }
        else if (processingMessage.WarningLevel == WarningLevel.Warning)
        {
            foreach (var line in lines)
            {
                Console.WriteLine($"##vso[task.logissue type=warning] {line}");
            }
        }
        else if (processingMessage.WarningLevel == WarningLevel.Success)
        {

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(processingMessage.Message);
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine(processingMessage.Message);
        }
        processingMessages.Add(processingMessage);
    }
}


internal class ProcessingMessage
{
    public string Message { get; set; }
    public WarningLevel WarningLevel { get; set; }
    public ProcessingMessage(string message, WarningLevel warningLevel = WarningLevel.None)
    {
        Message = message;
        WarningLevel = warningLevel;
    }
}

internal enum WarningLevel
{
    None,
    Success,
    Warning,
    Error
}
