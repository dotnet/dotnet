// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Maestro.Common;
using Microsoft.DotNet.DarcLib.Helpers;
using Microsoft.DotNet.DarcLib.VirtualMonoRepo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CloakedFileScan;

internal static class Program
{
    private static void LogInfo(string message) => Console.WriteLine(message);
    private static void LogError(string message) => Console.WriteLine($"##vso[task.logissue type=error] {message}");

    internal static async Task<int> Main()
    {
        var pm = new ProcessManager(NullLogger<ProcessManager>.Instance, "git");
        var vmrPath = pm.FindGitRoot(AppContext.BaseDirectory);

        LogInfo($"VMR root: {vmrPath}");

        var serviceProvider = RegisterServices(vmrPath);
        await using var scope = serviceProvider.CreateAsyncScope();

        var scanner = scope.ServiceProvider.GetRequiredService<VmrCloakedFileScanner>();

        LogInfo("Scanning VMR for cloaked files...");
        var files = await scanner.ScanVmr(baselineFilePath: null, CancellationToken.None);

        foreach (var file in files)
        {
            Console.WriteLine(file);
        }

        if (files.Count > 0)
        {
            LogError($"Found {files.Count} cloaked file(s) in the VMR.");
            return 1;
        }

        LogInfo("No cloaked files found.");
        return 0;
    }

    private static ServiceProvider RegisterServices(string vmrPath)
    {
        return new ServiceCollection()
            .AddCodeflow("tmp", vmrPath: vmrPath)
            .AddSingleton<IRemoteTokenProvider, NullRemoteTokenProvider>()
            .AddLogging(b => b.AddConsole())
            .BuildServiceProvider();
    }

    private sealed class NullRemoteTokenProvider : IRemoteTokenProvider
    {
        public string? GetTokenForRepository(string repoUri) => null;
        public Task<string?> GetTokenForRepositoryAsync(string repoUri) => Task.FromResult<string?>(null);
    }
}
