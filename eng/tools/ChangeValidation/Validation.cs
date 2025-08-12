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


namespace ChangeValidation;

internal static class Validation
{
    internal static void LogInfo(string message) => Console.WriteLine(message);
    internal static void LogWarning(string message) => Console.WriteLine($"##vso[task.logissue type=warning] {message}");
    internal static void LogError(string message) => Console.WriteLine($"##vso[task.logissue type=error] {message}");

    internal static async Task<int> Main(string[] args)
    {
        var pm = new ProcessManager(NullLogger<ProcessManager>.Instance, "git");

        var repoRoot = pm.FindGitRoot(AppContext.BaseDirectory);

        var serviceProvider = RegisterServices(repoRoot);

        List<IValidationStep> validationSteps = CreateValidationSteps(serviceProvider);

        PrInfo prInfo = await SetupPrInfo(pm, repoRoot);

        return await RunValidationSteps(validationSteps, prInfo) ? 0 : 1;
    }

    private static async Task<bool> RunValidationSteps(List<IValidationStep> validationSteps, PrInfo prInfo)
    {
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
                validationSuccess = await validationStep.Validate(prInfo);
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
            return false;
        }
        else
        {
            LogInfo("All validation steps succeeded!");
            return true;
        }
    }

    private static async Task<PrInfo> SetupPrInfo(IProcessManager pm, string repoPath)
    {
        string? targetBranch = $"origin/{Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_TARGETBRANCH")}";
        string? prNumber = Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTNUMBER");

        if (string.IsNullOrEmpty(targetBranch))
        {
            throw new ArgumentException("Cannot determine PR target branch.");
        }

        // Create a git reference to the PR head commit
        await pm.ExecuteGit(repoPath, ["fetch", "origin", $"refs/pull/{prNumber}/head:pr-head"]);

        string mergeBaseCommit = (await pm.ExecuteGit(repoPath, ["merge-base", "pr-head", targetBranch])).StandardOutput.Trim();

        var diffOutput = (await pm.ExecuteGit(repoPath, ["diff", "--name-only", mergeBaseCommit, "pr-head"])).StandardOutput.Trim();

        var changedFiles = diffOutput
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(f => f.Replace('\\', '/').Trim())
            .ToImmutableList();

        Console.WriteLine($"Merge base commit is {mergeBaseCommit}");
        Console.WriteLine($"Found modifications to {changedFiles.Count} file(s) in PR head branch");

        return new PrInfo(targetBranch, changedFiles);
    }

    private static IServiceProvider RegisterServices(string repoRoot)
    {
        var services = new ServiceCollection();

        VmrRegistrations.AddSingleVmrSupport(
            services,
            "git",
            repoRoot,
            "tmp",
            null,
            null);

        var serviceProvider = services.BuildServiceProvider();

        return serviceProvider;
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
