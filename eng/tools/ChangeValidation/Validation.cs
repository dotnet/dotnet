// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Maestro.Common;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.DotNet.DarcLib.VirtualMonoRepo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;


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
        LogInfo($"Repository root: {repoRoot}");

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
            catch (Exception ex)
            {
                LogError($"{validationStep.DisplayName} was interrupted with an unexpected error.");
                LogError($"Exception: {ex}");
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
        string? rawTargetBranch = Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_TARGETBRANCH");
        string? rawSourceBranch = Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_SOURCEBRANCH");
        string? prNumber = Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTNUMBER");

        LogInfo($"Raw environment variables:");
        LogInfo($"  SYSTEM_PULLREQUEST_TARGETBRANCH: {rawTargetBranch}");
        LogInfo($"  SYSTEM_PULLREQUEST_SOURCEBRANCH: {rawSourceBranch}");
        LogInfo($"  SYSTEM_PULLREQUEST_PULLREQUESTNUMBER: {prNumber}");

        string? targetBranch = $"origin/{rawTargetBranch}";

        if (string.IsNullOrEmpty(rawTargetBranch))
        {
            throw new ArgumentException("Cannot determine PR target branch.");
        }

        LogInfo($"Resolved target branch ref: {targetBranch}");

        // Create a git reference to the original branching-off point of the PR branch from the target branch
        LogInfo($"Fetching PR merge ref: refs/pull/{prNumber}/merge");
        await pm.ExecuteGit(repoPath, "fetch", "origin", $"refs/pull/{prNumber}/merge:pr-merge");

        LogInfo($"Computing diff between {targetBranch} and pr-merge...");
        var diffOutput = (await pm.ExecuteGit(repoPath, ["diff", "--name-only", targetBranch, "pr-merge"])).StandardOutput.Trim();

        var changedFiles = diffOutput
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(f => f.Replace('\\', '/').Trim())
            .ToImmutableList();

        LogInfo($"Found modifications to {changedFiles.Count} file(s) in PR head branch");

        return new PrInfo(targetBranch, changedFiles);
    }

    private static ServiceProvider RegisterServices(string repoRoot)
    {
        return new ServiceCollection()
            .AddCodeflow("tmp", vmrPath: repoRoot)
            .AddSingleton<IRemoteTokenProvider, NullRemoteTokenProvider>()
            .BuildServiceProvider();
    }

    private static List<IValidationStep> CreateValidationSteps(IServiceProvider serviceProvider)
    {
        return [
            ActivatorUtilities.CreateInstance<ToolingFilesValidation>(serviceProvider),
            ActivatorUtilities.CreateInstance<SubmoduleValidation>(serviceProvider),
            ActivatorUtilities.CreateInstance<ExclusionFileValidation>(serviceProvider)
            ];
    }

    private class NullRemoteTokenProvider : IRemoteTokenProvider
    {
        public string? GetTokenForRepository(string repoUri) => null;
        public Task<string?> GetTokenForRepositoryAsync(string repoUri) => Task.FromResult<string?>(null);
    }
}
