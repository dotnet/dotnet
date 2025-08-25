// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using Microsoft.Build.Framework;

[assembly:UnsupportedOSPlatform("windows")]

namespace Microsoft.DotNet.SourceBuild.Tasks.Sign;

public class SignArtifacts : Microsoft.Build.Utilities.Task
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

    public override bool Execute()
    {
        try
        {
            PrepareForSigning();
            RunSigning();
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
    private void RunSigning()
    {
        using (var process = new Process())
        {
            (string command, List<string> arguments) = GetSigningCommandAndArguments();

            process.StartInfo = new ProcessStartInfo()
            {
                FileName = command,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                EnvironmentVariables = { ["OPENSSL_ENABLE_SHA1_SIGNATURES"] = "1" }
            };

            foreach (string arg in arguments)
            {
                process.StartInfo.ArgumentList.Add(arg);
            }

            process.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                    Log.LogMessage(MessageImportance.High, args.Data);
            };
            process.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                    Log.LogError(args.Data);
            };

            Log.LogMessage(MessageImportance.High, $"Running signing...");

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            bool hasExited = process.WaitForExit(SigningTimeout);
            if (!hasExited)
            {
                process.Kill();
                process.WaitForExit();
                throw new TimeoutException($"Signing timed out after {SigningTimeout / 1000} seconds.");
            }

            if (process.ExitCode != 0)
            {
                throw new Exception($"Signing failed with exit code {process.ExitCode}");
            }

            Log.LogMessage(MessageImportance.High, $"Signing completed.");
        }
    }

    /// <summary>
    /// Gets the command and arguments to run signing
    /// </summary>
    private (string command, List<string> arguments) GetSigningCommandAndArguments()
    {
        string buildScript = Path.Combine(DotNetEngDirectory, "common", "build.sh");
        string binlog = Path.Combine(LogsDirectory, SignBinlogFileName);
        string signableAssets = string.Join(';', SignableSourceBuiltAssets.Select(a => a.ItemSpec));

        var arguments = new List<string>
        {
            "--ci",
            "--configuration", "Release",
            "--restore",
            "--projects", Config.EmptyProjectPath,
            "--sign",
            "--excludecibinarylog",
            $"/bl:{binlog}",
            $"/p:OfficialBuildId={OfficialBuildId}",
            $"/p:AspNetCoreSnkPath={Config.AspNetCoreSnkPath}",
            $"/p:SignableSourceBuiltAssets=\"{signableAssets}\""
        };

        if (!string.IsNullOrWhiteSpace(SignProperties))
        {
            arguments.AddRange(SignProperties.Split(' '));
        }

        return (buildScript, arguments);
    }
}
