// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Net.BuildServerUtils;

internal static class BuildServerUtility
{
    public const string WindowsPipePrefix = """\\.\pipe\""";
    public const string DotNetHostServerPath = "DOTNET_HOST_SERVER_PATH";

    public static void ListenForShutdown(string label, Action<string> onStart, Action onShutdown, Action<Exception> onError, CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            try
            {
                await WaitForShutdownAsync(label, onStart, cancellationToken).ConfigureAwait(false);
                onShutdown();
            }
            catch (OperationCanceledException e) when (e.CancellationToken == cancellationToken)
            {
                // Not an error.
            }
            catch (Exception ex)
            {
                onError(ex);
            }
        },
        cancellationToken);
    }

    public static async Task WaitForShutdownAsync(string label, Action<string> onStart, CancellationToken cancellationToken)
    {
        var pipePath = GetPipePath(label);
        onStart(pipePath);

        // Delete the pipe if it exists (can happen if a previous build server did not shut down gracefully and its PID is recycled).
        File.Delete(pipePath);

        // Wait for any input which means shutdown is requested.
        using var server = new NamedPipeServerStream(NormalizePipeNameForStream(pipePath));
        await server.WaitForConnectionAsync(cancellationToken).ConfigureAwait(false);
        await server.ReadAsync(new byte[1], 0, 1, cancellationToken).ConfigureAwait(false);

        // Close and delete the pipe.
        server.Dispose();
        File.Delete(pipePath);
    }

    private static string GetPipePath(string label)
    {
        var folder = Environment.GetEnvironmentVariable(DotNetHostServerPath);
        var pipeFolder = GetPipeFolder(folder) ?? throw new InvalidOperationException($"Environment variable '{DotNetHostServerPath}' is not set.");
        var pid = GetCurrentProcessId();
        return Path.Combine(pipeFolder, $"{pid}.{label}.pipe");
    }

#if NET
    public static bool TryParsePipePath(string path, out int pid, out ReadOnlySpan<char> label)
    {
        var fileName = Path.GetFileNameWithoutExtension(path);
        var dotIndex = fileName.IndexOf('.');
        if (dotIndex >= 0 && int.TryParse(fileName.AsSpan(0, dotIndex), out pid))
        {
            label = fileName.AsSpan(dotIndex + 1);
            return true;
        }

        pid = 0;
        label = default;
        return false;
    }
#endif

    private static int GetCurrentProcessId()
    {
#if NET
        return Environment.ProcessId;
#else
        return Process.GetCurrentProcess().Id;
#endif
    }

    public static string? GetPipeFolder(string? hostServerPath)
    {
        if (string.IsNullOrWhiteSpace(hostServerPath))
        {
            return null;
        }

        hostServerPath = Path.GetFullPath(hostServerPath);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            string normalized = hostServerPath.Replace('/', '\\').ToLowerInvariant();

            if (!normalized.EndsWith("\\", StringComparison.Ordinal))
            {
                normalized += '\\';
            }

            return $"{WindowsPipePrefix}{normalized}";
        }

        return hostServerPath;
    }

    /// <summary>
    /// Removes <c>\.\\pipe\</c> prefix on Windows which must not be passed
    /// to <see cref="PipeStream"/> constructors (they would duplicate it).
    /// </summary>
    public static string NormalizePipeNameForStream(string pipeName)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) &&
            pipeName.StartsWith(WindowsPipePrefix, StringComparison.OrdinalIgnoreCase))
        {
            return pipeName[WindowsPipePrefix.Length..];
        }

        return pipeName;
    }
}
