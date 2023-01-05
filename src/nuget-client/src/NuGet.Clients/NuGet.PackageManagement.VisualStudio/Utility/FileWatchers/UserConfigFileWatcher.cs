// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.IO;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.PackageManagement.VisualStudio.Utility.FileWatchers
{
    /// <summary>Watches a user settings directory to detect changes to nuget.config and config/*.config.</summary>
    internal class UserConfigFileWatcher : IFileWatcher
    {
        private bool _disposed;
        private readonly FileSystemWatcher _userSettingsWatcher;
        private readonly FileSystemWatcher _configDirectoryWatcher;
        private readonly FileStream _lockFile;

        public UserConfigFileWatcher()
            : this(NuGetEnvironment.GetFolderPath(NuGetFolderPath.UserSettingsDirectory))
        {
        }

        public UserConfigFileWatcher(string userSettingsDirectory)
        {
            _disposed = false;

            string configDirectory = Path.Combine(userSettingsDirectory, "config");
            string lockFile = Path.Combine(configDirectory, ".lock");

            // FileSystemWatcher doesn't handle directories being deleted then recreated. So, take advantage that
            // Windows doesn't let directories to be deleted when a file in the directory is being used.
            // Since the config directory is in the user settings directory, a single lock can prevent both from
            // being deleted.
            if (!Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
            }
            _lockFile = new FileStream(lockFile, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);

            _userSettingsWatcher = Create(userSettingsDirectory, Settings.DefaultSettingsFileName);
            _configDirectoryWatcher = Create(configDirectory, "*.config");

            FileSystemWatcher Create(string path, string filter)
            {
                var fileSystemWatcher = new FileSystemWatcher(path, filter);
                fileSystemWatcher.Created += OnFileSystemEvent;
                fileSystemWatcher.Changed += OnFileSystemEvent;
                fileSystemWatcher.Deleted += OnFileSystemEvent;
                fileSystemWatcher.Renamed += OnFileSystemEvent;
                fileSystemWatcher.EnableRaisingEvents = true;

                return fileSystemWatcher;
            }
        }

        public event EventHandler? FileChanged;

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            _configDirectoryWatcher.EnableRaisingEvents = false;
            _configDirectoryWatcher.Created -= OnFileSystemEvent;
            _configDirectoryWatcher.Changed -= OnFileSystemEvent;
            _configDirectoryWatcher.Deleted -= OnFileSystemEvent;
            _configDirectoryWatcher.Renamed -= OnFileSystemEvent;
            _configDirectoryWatcher.Dispose();

            _userSettingsWatcher.EnableRaisingEvents = false;
            _userSettingsWatcher.Created -= OnFileSystemEvent;
            _userSettingsWatcher.Changed -= OnFileSystemEvent;
            _userSettingsWatcher.Deleted -= OnFileSystemEvent;
            _userSettingsWatcher.Renamed -= OnFileSystemEvent;
            _userSettingsWatcher.Dispose();

            _lockFile.Dispose();
            try
            {
                File.Delete(_lockFile.Name);
            }
            catch (IOException)
            {
                // Another instance of VS is open.
            }

            GC.SuppressFinalize(this);
        }

        private void OnFileSystemEvent(object sender, FileSystemEventArgs e)
        {
            FileChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
