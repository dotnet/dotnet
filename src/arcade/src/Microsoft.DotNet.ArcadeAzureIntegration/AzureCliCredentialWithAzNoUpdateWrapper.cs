// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !NET472_OR_GREATER

#nullable enable

using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;

namespace Microsoft.DotNet.ArcadeAzureIntegration;


// This class is an workaround for disable az cli auto update mechanism which cause timeout when waiting for
// console input in case of new version of az available
// - this wrppper will run "az config set auto-upgrade.enable=no" only once before first call to az for acquiring the token
public class AzureCliCredentialWithAzNoUpdateWrapper : TokenCredential
{
    private readonly AzureCliCredential _azureCliCredential;

    public AzureCliCredentialWithAzNoUpdateWrapper(AzureCliCredential azureCliCredential)
    {
        _azureCliCredential = azureCliCredential;
    }

    public static string? EnvProgramFilesX86 => GetNonEmptyStringOrNull(Environment.GetEnvironmentVariable("ProgramFiles(x86)"));
    public static string? EnvProgramFiles => GetNonEmptyStringOrNull(Environment.GetEnvironmentVariable("ProgramFiles"));
    public static string? EnvPath => GetNonEmptyStringOrNull(Environment.GetEnvironmentVariable("PATH"));

    private static readonly string DefaultPathWindows = $"{EnvProgramFilesX86}\\Microsoft SDKs\\Azure\\CLI2\\wbin;{EnvProgramFiles}\\Microsoft SDKs\\Azure\\CLI2\\wbin";
    private static readonly string DefaultWorkingDirWindows = Environment.GetFolderPath(Environment.SpecialFolder.System);
    private const string DefaultPathNonWindows = "/usr/bin:/usr/local/bin";
    private const string DefaultWorkingDirNonWindows = "/bin/";
    private static readonly string DefaultPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? DefaultPathWindows : DefaultPathNonWindows;
    private static readonly string DefaultWorkingDir = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? DefaultWorkingDirWindows : DefaultWorkingDirNonWindows;

    private static string? GetNonEmptyStringOrNull(string? str)
    {
        return !string.IsNullOrEmpty(str) ? str : null;
    }

    private static SemaphoreSlim _azCliInitSemaphore = new SemaphoreSlim(1, 1);
    private static bool _azCliInitialized = false;

    private async Task SetUpAzAsync()
    {
        await _azCliInitSemaphore.WaitAsync();
        try
        {
            if (_azCliInitialized) return;

            string fileName;
            string argument;
            string command = $"az config set auto-upgrade.enable=no";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe");
                argument = $"/d /c \"{command}\"";
            }
            else
            {
                fileName = "/bin/sh";
                argument = $"-c \"{command}\"";
            }

            string path = !string.IsNullOrEmpty(EnvPath) ? EnvPath : DefaultPath;
            var processInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = argument,
                UseShellExecute = false,
                ErrorDialog = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = DefaultWorkingDir,
                Environment = { { "PATH", path } }
            };

            using Process? process = Process.Start(processInfo);
            if (process == null)
            {
                throw new InvalidOperationException("Failed to start process to disable auto update of Azure CLI");
            }

            using var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(TimeSpan.FromSeconds(30));
            await process.WaitForExitAsync(tokenSource.Token);

            if (!process.HasExited)
            {
                // try clean up the process if it is still running after timeout
                try { process.Kill(); } catch { /* ignore this excpetion */}
                throw new InvalidOperationException("Could not finish az config command to disable auto update on time");
            }

            process.StandardInput.Close();
            process.Close();
        }
        catch (Exception e)
        {
            // silent catch with direct console output as this is not a critical error
            Console.WriteLine($"Warning - Disable auto update of Azure CLI failed: {e.Message}");
        }
        finally
        {
            _azCliInitialized = true;
            _azCliInitSemaphore.Release();
        }
    }

    public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken = default)
    {
        if (!_azCliInitialized)
        {
            SetUpAzAsync().Wait();
        }
        return _azureCliCredential.GetToken(requestContext, cancellationToken);
    }

    public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken = default)
    {
        if (!_azCliInitialized)
        {
            await SetUpAzAsync();
        }
        return await _azureCliCredential.GetTokenAsync(requestContext, cancellationToken);
    }
}

#endif
