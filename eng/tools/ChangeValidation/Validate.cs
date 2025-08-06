// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.DotNet.DarcLib;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.DotNet.DarcLib.VirtualMonoRepo;
using Microsoft.DotNet.DarcLib.Models.VirtualMonoRepo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace ValidateVmrChanges;

internal static class Validate
{
    internal static void LogInfo(string message) => Console.WriteLine(message);
    internal static void LogWarning(string message) => Console.WriteLine($"##vso[task.logissue type=warning] {message}");
    internal static void LogError(string message) => Console.WriteLine($"##vso[task.logissue type=error] {message}");

    internal static async Task<int> Main(string[] args)
    {
        using var host = RegisterServices(args);
        List<IValidationStep> validationSteps = CreateValidationSteps(host.Services);
        PrInfo prInfo = SetupPrInfo();

        int stepCounter = 0;
        int failedSteps = 0;

        foreach (var validationStep in validationSteps)
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
                LogError($"{validationStep.DisplayName} was interrupted with an unexpected error.");
            }
            if (validationSuccess)
            {
                LogInfo($"{validationStep.DisplayName} succeeded.");
            }
            else
            {
                failedSteps++;
                Console.WriteLine($"{validationStep.DisplayName} failed.");
            }
        }
        if (failedSteps > 0)
        {
            Console.WriteLine();
            Console.WriteLine();
            LogError($"{failedSteps} out of {validationSteps.Count} validation steps have failed.");
            return 1;
        }
        else
        {
            LogInfo("All validation steps succeeded!");
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
            .ToImmutableList();

        return new PrInfo(targetBranch, changedFiles);
    }

    private static IHost RegisterServices(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddTransient<IProcessManager>(sp => new ProcessManager(NullLogger<ProcessManager>.Instance, "git"));
            services.AddSingleton<IVmrInfo>(sp =>
            {
                var pm = sp.GetRequiredService<IProcessManager>();
                var repoRoot = pm.FindGitRoot(AppContext.BaseDirectory);
                return new VmrInfo(repoRoot, "tmp");
            });
            services.AddScoped<ISourceManifest>(sp => new SourceManifest([], []));
            services.AddScoped<IVmrDependencyTracker, VmrDependencyTracker>();
            services.AddTransient<ISourceMappingParser, SourceMappingParser>();
            services.AddSingleton<IFileSystem, FileSystem>();

        })
        .Build();
    }

    private static List<IValidationStep> CreateValidationSteps(IServiceProvider serviceProvider)
    {
        return [
            ActivatorUtilities.CreateInstance<ToolingFilesValidation>(serviceProvider),
            ActivatorUtilities.CreateInstance<SubmoduleValidation>(serviceProvider),
            ActivatorUtilities.CreateInstance<ExclusionFileValidation>(serviceProvider)
            ];
    }
}
