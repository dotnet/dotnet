// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Threading;
using NuGet.Common;

namespace NuGet.CommandLine.Test
{
    /// <summary>
    /// Helps ensuring only one unit-test can backup/restore the global nuget.config at a time.
    /// </summary>
    public sealed class DefaultConfigurationFilePreserver : IDisposable
    {
        private const string MutexName = "DefaultConfigurationFilePreserver";
        private readonly Mutex _mutex;
        private bool _disposed = false;

        public DefaultConfigurationFilePreserver()
        {
            bool mutexWasCreated;
            // Request initial ownership of the named mutex by passing true for the first parameter.
            //Only one system object named "DefaultConfigurationFilePreserver" can exist
            _mutex = new Mutex(initiallyOwned: true, MutexName, out mutexWasCreated);
            if (!mutexWasCreated)
            {
                bool owner = _mutex.WaitOne(TimeSpan.FromMinutes(2));
                if (!owner)
                    throw new TimeoutException(string.Format(CultureInfo.CurrentCulture, "Timedout while waiting for mutex {0}", MutexName));
            }

            BackupAndDeleteDefaultConfigurationFile();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    RestoreDefaultConfigurationFile();
                    _mutex.ReleaseMutex();
                    _mutex.Dispose();
                }

                _disposed = true;
            }
        }

        ~DefaultConfigurationFilePreserver()
        {
            Dispose(false);
        }

        private static void BackupAndDeleteDefaultConfigurationFile()
        {
            string defaultConfigurationFile = Path.Combine(NuGetEnvironment.GetFolderPath(NuGetFolderPath.UserSettingsDirectory), "NuGet.Config");
            string backupFileName = defaultConfigurationFile + ".backup";

            if (File.Exists(defaultConfigurationFile))
            {
                File.Copy(defaultConfigurationFile, backupFileName, true);
                File.Delete(defaultConfigurationFile);
            }
        }

        private static void RestoreDefaultConfigurationFile()
        {
            string defaultConfigurationFile = Path.Combine(NuGetEnvironment.GetFolderPath(NuGetFolderPath.UserSettingsDirectory), "NuGet.Config");
            string backupFileName = defaultConfigurationFile + ".backup";

            if (File.Exists(backupFileName))
            {
                File.Copy(backupFileName, defaultConfigurationFile, true);
                File.Delete(backupFileName);
            }
        }
    }
}
