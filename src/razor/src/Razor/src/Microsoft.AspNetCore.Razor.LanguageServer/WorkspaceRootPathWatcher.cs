// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal partial class WorkspaceRootPathWatcher : IOnInitialized, IDisposable
{
    private static readonly TimeSpan s_delay = TimeSpan.FromSeconds(1);
    private static readonly ImmutableArray<string> s_filters = ["*.razor", "*.cshtml"];
    private static readonly string[] s_ignoredDirectories = ["node_modules"];

    private readonly IWorkspaceRootPathProvider _workspaceRootPathProvider;
    private readonly IRazorProjectService _projectService;
    private readonly LanguageServerFeatureOptions _options;

    private readonly CancellationTokenSource _disposeTokenSource;
    private readonly AsyncBatchingWorkQueue<(string, RazorFileChangeKind)> _workQueue;
    private readonly Dictionary<string, (RazorFileChangeKind kind, int index)> _filePathToChangeMap;
    private readonly HashSet<int> _indicesToSkip;
    private readonly List<FileSystemWatcher> _watchers;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _logger;

    public WorkspaceRootPathWatcher(
        IWorkspaceRootPathProvider workspaceRootPathProvider,
        IRazorProjectService projectService,
        LanguageServerFeatureOptions options,
        IFileSystem fileSystem,
        ILoggerFactory loggerFactory)
        : this(workspaceRootPathProvider, projectService, options, fileSystem, loggerFactory, s_delay)
    {
    }

    protected WorkspaceRootPathWatcher(
        IWorkspaceRootPathProvider workspaceRootPathProvider,
        IRazorProjectService projectService,
        LanguageServerFeatureOptions options,
        IFileSystem fileSystem,
        ILoggerFactory loggerFactory,
        TimeSpan delay)
    {
        _workspaceRootPathProvider = workspaceRootPathProvider;
        _projectService = projectService;
        _options = options;

        _disposeTokenSource = new();
        _workQueue = new AsyncBatchingWorkQueue<(string, RazorFileChangeKind)>(delay, ProcessBatchAsync, _disposeTokenSource.Token);
        _filePathToChangeMap = new(FilePathComparer.Instance);
        _indicesToSkip = [];
        _watchers = [];
        _fileSystem = fileSystem;
        _logger = loggerFactory.GetOrCreateLogger<WorkspaceRootPathWatcher>();
    }

    public void Dispose()
    {
        if (_disposeTokenSource.IsCancellationRequested)
        {
            return;
        }

        _disposeTokenSource.Cancel();
        StopFileWatchers();

        _disposeTokenSource.Dispose();
    }

    private async ValueTask ProcessBatchAsync(ImmutableArray<(string, RazorFileChangeKind)> items, CancellationToken token)
    {
        // Clear out our helper collections.
        _filePathToChangeMap.Clear();
        _indicesToSkip.Clear();

        // First, collect all of the file paths and note the indices add/remove change pairs for the same file path.
        using var potentialItems = new PooledArrayBuilder<string>(capacity: items.Length);

        var index = 0;

        foreach (var (filePath, kind) in items)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            if (_filePathToChangeMap.TryGetValue(filePath, out var value))
            {
                // We've already seen this file path, so we should skip it later.
                _indicesToSkip.Add(index);

                var (existingKind, existingIndex) = value;

                // We only ever get added or removed. So, if we've already received an add and are getting
                // a remove, there's no need to send the notification. Likewise, if we've received a remove
                // and are getting an add, we can just elide this notification altogether.
                if (kind != existingKind)
                {
                    _filePathToChangeMap.Remove(filePath);
                    _indicesToSkip.Add(existingIndex);
                }
                else
                {
                    Debug.Fail($"Unexpected {kind} event because our prior tracked state was the same.");
                }
            }
            else
            {
                _filePathToChangeMap.Add(filePath, (kind, index));
            }

            potentialItems.Add(filePath);
            index++;
        }

        // Now, loop through all of the file paths we collected and notify listeners of changes,
        // taking care of to skip any indices that we noted earlier.
        for (var i = 0; i < potentialItems.Count; i++)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            if (_indicesToSkip.Contains(i))
            {
                continue;
            }

            var filePath = potentialItems[i];

            if (!_filePathToChangeMap.TryGetValue(filePath, out var value))
            {
                continue;
            }

            // We only send notifications for the changes that we kept.
            if (value.kind == RazorFileChangeKind.Added)
            {
                await _projectService.AddDocumentToMiscProjectAsync(filePath, token).ConfigureAwait(false);
            }
            else if (value.kind == RazorFileChangeKind.Removed)
            {
                await _projectService.RemoveDocumentAsync(filePath, token).ConfigureAwait(false);
            }
        }
    }

    public async Task OnInitializedAsync(ILspServices services, CancellationToken cancellationToken)
    {
        // Initialized request, this occurs once the server and client have agreed on what sort of features they both support. It only happens once.

        var workspaceDirectoryPath = await _workspaceRootPathProvider.GetRootPathAsync(cancellationToken).ConfigureAwait(false);

        await StartAsync(workspaceDirectoryPath, cancellationToken).ConfigureAwait(false);

        if (_disposeTokenSource.IsCancellationRequested)
        {
            // Got disposed while starting our file change detectors. We need to re-stop our change detectors.
            StopFileWatchers();
        }
    }

    // Protected virtual for testing
    protected virtual async Task StartAsync(string workspaceDirectory, CancellationToken cancellationToken)
    {
        // Dive through existing Razor files and fabricate "added" events so listeners can accurately listen to state changes for them.

        workspaceDirectory = FilePathNormalizer.Normalize(workspaceDirectory);

        // There's a double negative below because we want to initialize the misc project unless the option is set to *not* initialize it.
        // This is slightly awkward but is more convenient for command-line configuration.
        //
        // https://github.com/dotnet/razor/issues/11594 tracks removing this option and the code to support it.

        if (!_options.DoNotInitializeMiscFilesProjectFromWorkspace)
        {
            var existingRazorFiles = GetExistingRazorFiles(workspaceDirectory);

            await _projectService.AddDocumentsToMiscProjectAsync(existingRazorFiles, cancellationToken).ConfigureAwait(false);
        }

        if (cancellationToken.IsCancellationRequested || !InitializeFileWatchers)
        {
            return;
        }

        // Start listening for project file changes (added/removed/renamed).
        foreach (var filter in s_filters)
        {
            var watcher = new RazorFileSystemWatcher(workspaceDirectory, filter)
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime,
                IncludeSubdirectories = true,
            };

            watcher.Created += (sender, args) => _workQueue.AddWork((args.FullPath, RazorFileChangeKind.Added));
            watcher.Deleted += (sender, args) => _workQueue.AddWork((args.FullPath, RazorFileChangeKind.Removed));
            watcher.Renamed += (sender, args) =>
            {
                // Translate file renames into remove->add

                // All of the filters are of the form, *.extension. So, we can just slice to the extension.
                Debug.Assert(filter[0] == '*');
                var extension = filter.AsSpan()[1..];

                if (PathUtilities.GetExtension(args.OldFullPath.AsSpan()).Equals(extension, FilePathComparison.Instance))
                {
                    // Renaming from Razor file to something else.
                    _workQueue.AddWork((args.OldFullPath, RazorFileChangeKind.Removed));
                }

                if (PathUtilities.GetExtension(args.FullPath.AsSpan()).Equals(extension, FilePathComparison.Instance))
                {
                    // Renaming to a Razor file.
                    _workQueue.AddWork((args.FullPath, RazorFileChangeKind.Added));
                }
            };

            watcher.EnableRaisingEvents = true;

            _watchers.Add(watcher);
        }
    }

    private void StopFileWatchers()
    {
        foreach (var watcher in _watchers)
        {
            watcher.Dispose();
        }

        _watchers.Clear();
    }

    // Protected virtual for testing
    protected virtual bool InitializeFileWatchers => true;

    // Protected virtual for testing
    protected virtual ImmutableArray<string> GetExistingRazorFiles(string workspaceDirectory)
    {
        using var result = new PooledArrayBuilder<string>();

        foreach (var filter in s_filters)
        {
            var existingFiles = _fileSystem.GetFilteredFiles(workspaceDirectory, filter, s_ignoredDirectories, _logger);
            result.AddRange(existingFiles);
        }

        return result.DrainToImmutable();
    }
}
