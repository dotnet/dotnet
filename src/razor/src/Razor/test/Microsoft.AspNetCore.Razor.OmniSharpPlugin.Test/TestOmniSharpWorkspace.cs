// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Linq;
using Microsoft.Extensions.Logging;
using OmniSharp.FileWatching;
using OmniSharp.Services;

namespace OmniSharp;

public static class TestOmniSharpWorkspace
{
    private static readonly object s_workspaceLock = new();

    public static OmniSharpWorkspace Create(ILoggerFactory loggerFactory)
    {
        lock (s_workspaceLock)
        {
            var hostServicesAggregator = new HostServicesAggregator(Enumerable.Empty<IHostServicesProvider>(), loggerFactory);
            var workspace = new OmniSharpWorkspace(hostServicesAggregator, loggerFactory, TestFileSystemWatcher.Instance);

            return workspace;
        }
    }

    private class TestFileSystemWatcher : IFileSystemWatcher
    {
        public static readonly TestFileSystemWatcher Instance = new();

        private TestFileSystemWatcher()
        {
        }

        public void Watch(string pathOrExtension, FileSystemNotificationCallback callback)
        {
        }

        public void WatchDirectories(FileSystemNotificationCallback callback)
        {
        }
    }
}
