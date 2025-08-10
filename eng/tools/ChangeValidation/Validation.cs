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
        string? baseBranch = Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_SOURCEBRANCH");
        string? targetBranch = Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_TARGETBRANCH");

        Console.WriteLine($"Base branch is {baseBranch}");
        Console.WriteLine($"Target branch is {targetBranch}");

        if (string.IsNullOrEmpty(targetBranch))
        {
            throw new ArgumentException("Cannot determine PR target branch.");
        }

        if (string.IsNullOrEmpty(baseBranch))
        {
            throw new ArgumentException("Cannot determine PR source branch.");
        }

        await pm.ExecuteGit(repoPath, ["fetch", $"origin {targetBranch}"]);
        await pm.ExecuteGit(repoPath, ["fetch", $"origin {baseBranch}"]);
        var mergeBase = (await pm.ExecuteGit(repoPath, ["merge-base", "origin/" + targetBranch, "origin/" + baseBranch]));

        Console.WriteLine($"Merge base commit is {mergeBase.StandardOutput}");
        Console.WriteLine($"Merge base error? {mergeBase.StandardError}");

        var mergeBaseLocal = (await pm.ExecuteGit(repoPath, ["merge-base", targetBranch, baseBranch]));

        Console.WriteLine($"Merge base commit is {mergeBaseLocal.StandardOutput}");
        Console.WriteLine($"Merge base error? {mergeBaseLocal.StandardError}");

        baseBranch = "origin/" + baseBranch;
        targetBranch = "origin/" + targetBranch;


        var diffOutput = (await pm.ExecuteGit(repoPath, ["diff", "--name-only", mergeBase.StandardOutput, baseBranch]));

        Console.WriteLine($"Diff output error? {diffOutput.StandardError}");
        Console.WriteLine($"Diff output: {diffOutput.StandardOutput}");

        var changedFiles = diffOutput.StandardOutput
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(f => f.Replace('\\', '/').Trim())
            .ToImmutableList();

        Console.WriteLine($"Changed files: {string.Join("\n", changedFiles)}");

        return new PrInfo("origin/" + baseBranch, "origin/" + targetBranch, changedFiles);
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
