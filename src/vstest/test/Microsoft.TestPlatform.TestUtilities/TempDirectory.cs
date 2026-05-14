// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

using IO = System.IO;

namespace Microsoft.TestPlatform.TestUtilities;

public class TempDirectory : IDisposable
{
    private bool _isDisposed;

    /// <summary>
    /// Creates a unique temporary directory.
    /// </summary>
    public TempDirectory()
    {
        Path = CreateUniqueDirectory();
    }

    public string Path { get; }

    public static string? NuGetConfigPath { get; set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
            return;

        if (disposing)
        {
            TryRemoveDirectory(Path);
        }

        _isDisposed = true;
    }

    public DirectoryInfo CreateDirectory(string dir)
        => Directory.CreateDirectory(IO.Path.Combine(Path, dir));
    public void CopyDirectory(string sourceDirectory, string targetDirectory)
    {
        CopyDirectory(new DirectoryInfo(sourceDirectory), new DirectoryInfo(targetDirectory));
    }

#pragma warning disable CA1822 // Mark members as static
    public void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
#pragma warning restore CA1822 // Mark members as static
    {
        DirectoryUtils.CopyDirectory(source, target);
    }
    /// <summary>
    /// Copy given files into the TempDirectory and return the updated paths that are pointing to TempDirectory.
    /// </summary>
    /// <param name="filePaths"></param>
    /// <returns></returns>
    public string[] CopyFile(params string[] filePaths)
    {
        var paths = new List<string>(filePaths.Length);
        foreach (var filePath in filePaths)
        {
            var destination = IO.Path.Combine(Path, IO.Path.GetFileName(filePath));
            File.Copy(filePath, destination);
            paths.Add(destination);
        }

        return paths.ToArray();
    }

    /// <summary>
    /// Copy given file into TempDirectory and return the updated path.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public string CopyFile(string filePath)
    {
        var destination = IO.Path.Combine(Path, IO.Path.GetFileName(filePath));
        File.Copy(filePath, destination);
        return destination;
    }

    /// <summary>
    /// Creates an unique temporary directory.
    /// </summary>
    /// <returns>
    /// Path of the created directory.
    /// </returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    internal static string CreateUniqueDirectory()
    {
        var temp = GetTempPath();
        var directoryPath = IO.Path.Combine(temp, "vstest", RandomId.Next());
        Directory.CreateDirectory(directoryPath);

        if (NuGetConfigPath == null)
        {
            throw new InvalidOperationException("NuGetConfigPath on TempDirectory class must be set.");
        }

        var tempNugetConfigPath = IO.Path.Combine(directoryPath, IO.Path.GetFileName(NuGetConfigPath));

        if (!File.Exists(tempNugetConfigPath))
        {
            File.Copy(NuGetConfigPath, tempNugetConfigPath);
        }

        return directoryPath;
    }

    private static string GetTempPath()
    {
        // AGENT_TEMPDIRECTORY is AzureDevops variable, which is set to path
        // that is cleaned up after every job. This is preferable to use over
        // just the normal TEMP, because that is not cleaned up for every run.
        //
        // System.IO.Path.GetTempPath is banned from the rest of the code. This is the only
        // place where we are allowed to use it. All other methods should use our GetTempPath (this method).
#pragma warning disable RS0030 // Do not used banned APIs
        return Environment.GetEnvironmentVariable("AGENT_TEMPDIRECTORY")
            ?? IO.Path.GetTempPath();
#pragma warning restore RS0030 // Do not used banned APIs
    }

    public static void TryRemoveDirectory(string directory)
    {
        if (Directory.Exists(directory))
        {
            try
            {
                Directory.Delete(directory, recursive: true);
            }
            catch { }
        }
    }
}
