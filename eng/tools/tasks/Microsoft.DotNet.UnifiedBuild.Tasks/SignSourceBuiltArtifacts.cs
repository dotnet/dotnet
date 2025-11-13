// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.DotNet.UnifiedBuild.Tasks.Services;
using BuildTask = Microsoft.Build.Utilities.Task;

namespace Microsoft.DotNet.UnifiedBuild.Tasks;

public class SignSourceBuiltArtifacts : BuildTask
{
    /// <summary>
    /// List of source-built assets to sign
    /// </summary>
    [Required]
    public required ITaskItem[] SignableSourceBuiltAssets { get; init; }

    /// <summary>
    /// Path to the eng directory
    /// </summary>
    [Required]
    public required string DotNetEngDirectory { get; init; }

    /// <summary>
    /// Path to the logs directory
    /// </summary>
    [Required]
    public required string LogsDirectory { get; init; }

    /// <summary>
    /// Sign-specific properties to pass to the signing process
    /// </summary>
    public string SignProperties { get; init; } = string.Empty;

    /// <summary>
    /// The official build ID
    /// </summary>
    public string OfficialBuildId { get; init; } = string.Empty;

    private string SigningPropsTargetPath => Path.Combine(DotNetEngDirectory, Config.SigningPropsFileName);
    private const string SignBinlogFileName = "sign-source-built-artifacts.binlog";
    private const int SigningTimeout = 60 * 60 * 1000 * 2; // 2 hours

    public override bool Execute() => ExecuteAsync().GetAwaiter().GetResult();

    private async Task<bool> ExecuteAsync()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Log.LogError("Windows is not supported for signing.");
            return false;
        }

        try
        {
            PrepareForSigning();
            await RunSigningAsync();
            Log.LogMessage(MessageImportance.High, "Signing complete.");
        }
        catch (Exception ex)
        {
            Log.LogError($"Signing failed: {ex.Message}");
        }
        finally
        {
            CleanupSigning();
        }

        return !Log.HasLoggedErrors;
    }

    private void PrepareForSigning()
    {
        if (!Directory.Exists(LogsDirectory))
        {
            Directory.CreateDirectory(LogsDirectory);
        }

        // Place the Signing.props file into the repo's eng directory for
        // Arcade's signing infra to find
        File.Copy(Config.SigningPropsPath, SigningPropsTargetPath, overwrite: true);
    }

    private void CleanupSigning()
    {
        if (File.Exists(SigningPropsTargetPath))
        {
            File.Delete(SigningPropsTargetPath);
        }
    }

    /// <summary>
    /// Runs the signing process
    /// </summary>
    private async Task RunSigningAsync()
    {
        (string command, string arguments) = GetSigningCommandAndArguments();
        Log.LogMessage(MessageImportance.High, $"Running signing command: {command} {arguments}");

        var processService = new ProcessService(Log, timeout: SigningTimeout);
        ProcessResult result = await processService.RunProcessAsync(
            command,
            arguments,
            environmentVariables: new List<ProcessEnvironmentVariable> { new ProcessEnvironmentVariable("OPENSSL_ENABLE_SHA1_SIGNATURES", "1") },
            printOutput: true);

        if (result.ExitCode != 0)
        {
            Log.LogError($"Signing failed with exit code {result.ExitCode}");
        }
    }

    /// <summary>
    /// Gets the command and arguments to run signing
    /// </summary>
    private (string command, string arguments) GetSigningCommandAndArguments()
    {
        string buildScript = Path.Combine(DotNetEngDirectory, "common", "build.sh");
        string binlog = Path.Combine(LogsDirectory, SignBinlogFileName);
        string signableAssets = string.Join(';', SignableSourceBuiltAssets.Select(a => a.ItemSpec));

        string arguments =
            $"--ci " +
            $"--configuration Release " +
            $"--restore " +
            $"--projects {Config.EmptyProjectPath} " +
            $"--sign " +
            $"--excludecibinarylog " +
            $"/bl:{binlog} " +
            $"/p:OfficialBuildId={OfficialBuildId} " +
            $"/p:SignableSourceBuiltAssets=\\\"{signableAssets}\\\" " +
            $"{SignProperties}";

        return (buildScript, arguments);
    }
}
