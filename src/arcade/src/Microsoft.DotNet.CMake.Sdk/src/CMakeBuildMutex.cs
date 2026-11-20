// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using System;
using System.IO;
using System.Threading;

namespace Microsoft.DotNet.CMake.Sdk;

internal sealed class CMakeBuildMutex : IDisposable
{
    private readonly Mutex _mutex;
    private bool _ownsMutex;

    private CMakeBuildMutex(Mutex mutex) => _mutex = mutex;

    public static CMakeBuildMutex Acquire(TaskEnvironment taskEnvironment, string path)
    {
        var result = new CMakeBuildMutex(CreateMutex(taskEnvironment, path));
        try
        {
            try
            {
                result._mutex.WaitOne();
            }
            catch (AbandonedMutexException)
            {
            }

            result._ownsMutex = true;
            return result;
        }
        catch
        {
            result.Dispose();
            throw;
        }
    }

    public void Dispose()
    {
        try
        {
            if (_ownsMutex)
            {
                _ownsMutex = false;
                _mutex.ReleaseMutex();
            }
        }
        finally
        {
            _mutex.Dispose();
        }
    }

    private static Mutex CreateMutex(TaskEnvironment taskEnvironment, string path)
    {
        string directory = Path.Combine(taskEnvironment.GetAbsolutePath(path).GetCanonicalForm().Value, ".cmake");
        Directory.CreateDirectory(taskEnvironment.GetAbsolutePath(directory));
        string identityDirectory = Path.Combine(directory, "Microsoft.DotNet.CMake.Sdk.mutex");
        AbsolutePath identityDirectoryPath = taskEnvironment.GetAbsolutePath(identityDirectory);
        AbsolutePath identityFile = taskEnvironment.GetAbsolutePath(Path.Combine(identityDirectory, "identity"));

        if (!Directory.Exists(identityDirectoryPath))
        {
            string identity = Guid.NewGuid().ToString("N");
            string temporaryDirectory = $"{identityDirectory}.{identity}.tmp";
            AbsolutePath temporaryDirectoryPath = taskEnvironment.GetAbsolutePath(temporaryDirectory);
            AbsolutePath temporaryFile = taskEnvironment.GetAbsolutePath(Path.Combine(temporaryDirectory, "identity"));
            Directory.CreateDirectory(temporaryDirectoryPath);
            try
            {
                File.WriteAllText(temporaryFile, identity);
                try
                {
                    // A nonempty directory cannot replace a competing identity on Unix.
                    Directory.Move(temporaryDirectoryPath, identityDirectoryPath);
                }
                catch (IOException) when (Directory.Exists(identityDirectoryPath))
                {
                }
            }
            finally
            {
                if (Directory.Exists(temporaryDirectoryPath))
                {
                    File.Delete(temporaryFile);
                    Directory.Delete(temporaryDirectoryPath);
                }
            }
        }

        string contents = File.ReadAllText(identityFile);
        if (!Guid.TryParseExact(contents, "N", out Guid mutexIdentity))
        {
            throw new InvalidDataException($"Invalid CMake build mutex identity in '{identityFile.Value}'.");
        }

#if NETFRAMEWORK
        return new Mutex(false, $"Local\\Microsoft.DotNet.CMake.Sdk.{mutexIdentity:N}");
#else
        return new Mutex(
            false,
            $"Microsoft.DotNet.CMake.Sdk.{mutexIdentity:N}",
            new NamedWaitHandleOptions { CurrentUserOnly = true, CurrentSessionOnly = false },
            out _);
#endif
    }
}
