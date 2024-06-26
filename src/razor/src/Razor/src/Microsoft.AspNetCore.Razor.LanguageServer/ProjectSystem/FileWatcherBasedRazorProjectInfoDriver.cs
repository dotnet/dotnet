// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal partial class FileWatcherBasedRazorProjectInfoDriver : AbstractRazorProjectInfoDriver
{
    private enum ChangeKind { AddOrUpdate, Remove }

    private static readonly ImmutableArray<string> s_ignoredDirectories =
    [
        "node_modules",
        "bin",
        ".vs",
    ];

    private readonly IWorkspaceRootPathProvider _workspaceRootPathProvider;
    private readonly LanguageServerFeatureOptions _options;
    private readonly ILogger _logger;

    private readonly AsyncBatchingWorkQueue<(string FilePath, ChangeKind Kind)> _workQueue;

    private FileSystemWatcher? _fileSystemWatcher;

    public FileWatcherBasedRazorProjectInfoDriver(
        IWorkspaceRootPathProvider workspaceRootPathProvider,
        LanguageServerFeatureOptions options,
        ILoggerFactory loggerFactory,
        TimeSpan? delay = null)
        : base(loggerFactory, delay)
    {
        _workspaceRootPathProvider = workspaceRootPathProvider;
        _options = options;
        _logger = loggerFactory.GetOrCreateLogger<FileWatcherBasedRazorProjectInfoDriver>();

        _workQueue = new(delay ?? DefaultDelay, ProcessBatchAsync, DisposalToken);

        // Dispose our FileSystemWatcher when the driver is disposed.
        DisposalToken.Register(() =>
        {
            _fileSystemWatcher?.Dispose();
            _fileSystemWatcher = null;
        });

        StartInitialization();
    }

    protected override async Task InitializeAsync(CancellationToken cancellationToken)
    {
        var workspaceDirectoryPath = await _workspaceRootPathProvider.GetRootPathAsync(cancellationToken).ConfigureAwait(false);
        workspaceDirectoryPath = FilePathNormalizer.Normalize(workspaceDirectoryPath);

        var existingConfigurationFiles = DirectoryHelper.GetFilteredFiles(
            workspaceDirectoryPath,
            _options.ProjectConfigurationFileName,
            s_ignoredDirectories,
            logger: _logger).ToImmutableArray();

        foreach (var filePath in existingConfigurationFiles)
        {
            var razorProjectInfo = await TryDeserializeAsync(filePath, cancellationToken).ConfigureAwait(false);

            if (razorProjectInfo is not null)
            {
                EnqueueUpdate(razorProjectInfo);
            }
        }

        if (!Directory.Exists(workspaceDirectoryPath))
        {
            _logger.LogInformation($"Creating workspace directory: '{workspaceDirectoryPath}'");
            Directory.CreateDirectory(workspaceDirectoryPath);
        }

        _logger.LogInformation($"Starting {nameof(FileWatcherBasedRazorProjectInfoDriver)}: '{workspaceDirectoryPath}'");

        _fileSystemWatcher = new RazorFileSystemWatcher(workspaceDirectoryPath, _options.ProjectConfigurationFileName)
        {
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime,
            IncludeSubdirectories = true,
        };

        _fileSystemWatcher.Created += (_, args) => EnqueueAddOrChange(args.FullPath);
        _fileSystemWatcher.Changed += (_, args) => EnqueueAddOrChange(args.FullPath);
        _fileSystemWatcher.Deleted += (_, args) => EnqueueRemove(args.FullPath);
        _fileSystemWatcher.Renamed += (_, args) => EnqueueRename(args.OldFullPath, args.FullPath);

        _fileSystemWatcher.EnableRaisingEvents = true;

        void EnqueueAddOrChange(string filePath)
        {
            _workQueue.AddWork((filePath, ChangeKind.AddOrUpdate));
        }

        void EnqueueRemove(string filePath)
        {
            _workQueue.AddWork((filePath, ChangeKind.Remove));
        }

        void EnqueueRename(string oldFilePath, string newFilePath)
        {
            EnqueueRemove(oldFilePath);
            EnqueueAddOrChange(newFilePath);
        }
    }

    private async ValueTask ProcessBatchAsync(ImmutableArray<(string FilePath, ChangeKind Kind)> items, CancellationToken token)
    {
        foreach (var (filePath, changeKind) in items.GetMostRecentUniqueItems(Comparer.Instance))
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            switch (changeKind)
            {
                case ChangeKind.AddOrUpdate:
                    var razorProjectInfo = await TryDeserializeAsync(filePath, token).ConfigureAwait(false);

                    if (razorProjectInfo is not null)
                    {
                        EnqueueUpdate(razorProjectInfo);
                    }

                    break;

                case ChangeKind.Remove:
                    // The file path is located in the directory used as the ProjectKey.
                    var normalizedDirectory = FilePathNormalizer.GetNormalizedDirectoryName(filePath);
                    var projectKey = new ProjectKey(normalizedDirectory);

                    EnqueueRemove(projectKey);

                    break;

                default:
                    Assumed.Unreachable();
                    break;
            }
        }
    }

    private async ValueTask<RazorProjectInfo?> TryDeserializeAsync(string filePath, CancellationToken cancellationToken)
    {
        try
        {
            using var stream = File.OpenRead(filePath);

            return await RazorProjectInfo
                .DeserializeFromAsync(stream, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error occurred while reading and deserializing '{filePath}'");
        }

        return null;
    }
}
