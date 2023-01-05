// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using NuGet.Configuration;

namespace NuGet.PackageManagement.VisualStudio.Utility.FileWatchers
{
    internal class SolutionConfigFileWatcher : IFileWatcher
    {
        private bool _disposed;
        private readonly List<FileSystemWatcher> _watchers;

        public SolutionConfigFileWatcher(string solutionDirectory)
        {
            string? current = solutionDirectory.EndsWith("\\", StringComparison.Ordinal)
                ? Path.GetDirectoryName(solutionDirectory)
                : solutionDirectory;

            _watchers = new List<FileSystemWatcher>();
            while (current != null)
            {
                if (Directory.Exists(current))
                {
                    var watcher = Create(current);
                    _watchers.Add(watcher);
                }

                current = Path.GetDirectoryName(current);
            }

            FileSystemWatcher Create(string path)
            {
                var watcher = new FileSystemWatcher(path, Settings.DefaultSettingsFileName);
                watcher.Created += OnFileSystemEvent;
                watcher.Changed += OnFileSystemEvent;
                watcher.Deleted += OnFileSystemEvent;
                watcher.Renamed += OnFileSystemEvent;
                watcher.EnableRaisingEvents = true;

                return watcher;
            }
        }

        public event EventHandler? FileChanged;

        public void Dispose()
        {
            if (_disposed) { return; }

            _disposed = true;

            foreach (var watcher in _watchers)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Created -= OnFileSystemEvent;
                watcher.Changed -= OnFileSystemEvent;
                watcher.Deleted -= OnFileSystemEvent;
                watcher.Renamed -= OnFileSystemEvent;
                watcher.Dispose();
            }

            _watchers.Clear();

            GC.SuppressFinalize(this);
        }

        private void OnFileSystemEvent(object sender, FileSystemEventArgs e)
        {
            FileChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
