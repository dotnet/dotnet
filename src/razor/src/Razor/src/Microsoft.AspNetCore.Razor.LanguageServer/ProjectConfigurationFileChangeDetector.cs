// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class ProjectConfigurationFileChangeDetector(
    IEnumerable<IProjectConfigurationFileChangeListener> listeners,
    LanguageServerFeatureOptions options,
    ILoggerFactory loggerFactory) : IFileChangeDetector
{
    private static readonly ImmutableArray<string> s_ignoredDirectories =
    [
        "node_modules",
        "bin",
        ".vs",
    ];

    private readonly ImmutableArray<IProjectConfigurationFileChangeListener> _listeners = listeners.ToImmutableArray();
    private readonly LanguageServerFeatureOptions _options = options;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<ProjectConfigurationFileChangeDetector>();

    private FileSystemWatcher? _watcher;

    public Task StartAsync(string workspaceDirectory, CancellationToken cancellationToken)
    {
        // Dive through existing project configuration files and fabricate "added" events so listeners can accurately listen to state changes for them.

        workspaceDirectory = FilePathNormalizer.Normalize(workspaceDirectory);
        var existingConfigurationFiles = GetExistingConfigurationFiles(workspaceDirectory);

        _logger.LogDebug($"Triggering events for existing project configuration files");

        foreach (var configurationFilePath in existingConfigurationFiles)
        {
            NotifyListeners(new(configurationFilePath, RazorFileChangeKind.Added));
        }

        // This is an entry point for testing
        if (!InitializeFileWatchers)
        {
            return Task.CompletedTask;
        }

        try
        {
            // FileSystemWatcher will throw if the folder we want to watch doesn't exist yet.
            if (!Directory.Exists(workspaceDirectory))
            {
                _logger.LogInformation($"Workspace directory '{workspaceDirectory}' does not exist yet, so Razor is going to create it.");
                Directory.CreateDirectory(workspaceDirectory);
            }
        }
        catch (Exception ex)
        {
            // Directory.Exists will throw on things like long paths
            _logger.LogError(ex, $"Failed validating that file watcher would be successful for '{workspaceDirectory}'");

            // No point continuing because the FileSystemWatcher constructor would just throw too.
            return Task.FromException(ex);
        }

        if (cancellationToken.IsCancellationRequested)
        {
            // Client cancelled connection, no need to setup any file watchers. Server is about to tear down.
            return Task.FromCanceled(cancellationToken);
        }

        _logger.LogInformation($"Starting configuration file change detector for '{workspaceDirectory}'");
        _watcher = new RazorFileSystemWatcher(workspaceDirectory, _options.ProjectConfigurationFileName)
        {
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime,
            IncludeSubdirectories = true,
        };

        _watcher.Created += (sender, args) => NotifyListeners(args.FullPath, RazorFileChangeKind.Added);
        _watcher.Deleted += (sender, args) => NotifyListeners(args.FullPath, RazorFileChangeKind.Removed);
        _watcher.Changed += (sender, args) => NotifyListeners(args.FullPath, RazorFileChangeKind.Changed);
        _watcher.Renamed += (sender, args) =>
        {
            // Translate file renames into remove / add

            if (args.OldFullPath.EndsWith(_options.ProjectConfigurationFileName, FilePathComparison.Instance))
            {
                // Renaming from project configuration file to something else. Just remove the configuration file.
                NotifyListeners(args.OldFullPath, RazorFileChangeKind.Removed);
            }
            else if (args.FullPath.EndsWith(_options.ProjectConfigurationFileName, FilePathComparison.Instance))
            {
                // Renaming from a non-project configuration file file to a real one. Just add the configuration file.
                NotifyListeners(args.FullPath, RazorFileChangeKind.Added);
            }
        };

        _watcher.EnableRaisingEvents = true;

        return Task.CompletedTask;
    }

    public void Stop()
    {
        // We're relying on callers to synchronize start/stops so we don't need to ensure one happens before the other.

        _watcher?.Dispose();
        _watcher = null;
    }

    // Protected virtual for testing
    protected virtual bool InitializeFileWatchers => true;

    // Protected virtual for testing
    protected virtual ImmutableArray<string> GetExistingConfigurationFiles(string workspaceDirectory)
    {
        return DirectoryHelper.GetFilteredFiles(
            workspaceDirectory,
            _options.ProjectConfigurationFileName,
            s_ignoredDirectories,
            logger: _logger).ToImmutableArray();
    }

    private void NotifyListeners(string physicalFilePath, RazorFileChangeKind kind)
    {
        NotifyListeners(new(physicalFilePath, kind));
    }

    private void NotifyListeners(ProjectConfigurationFileChangeEventArgs args)
    {
        foreach (var listener in _listeners)
        {
            _logger.LogDebug($"Notifying listener '{listener}' of config file path '{args.ConfigurationFilePath}' change with kind '{args.Kind}'");
            listener.ProjectConfigurationFileChanged(args);
        }
    }
}
