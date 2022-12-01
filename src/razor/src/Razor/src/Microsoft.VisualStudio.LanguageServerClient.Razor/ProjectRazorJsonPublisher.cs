// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Razor.Workspaces.Serialization;
using Newtonsoft.Json;
using Shared = System.Composition.SharedAttribute;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

/// <summary>
/// Publishes project.razor.json files.
/// </summary>
[Shared]
[Export(typeof(ProjectSnapshotChangeTrigger))]
internal class ProjectRazorJsonPublisher : ProjectSnapshotChangeTrigger
{
    internal readonly Dictionary<string, Task> DeferredPublishTasks;

    // Internal for testing
    internal bool _active;

    private const string TempFileExt = ".temp";
    private readonly RazorLogger _logger;
    private readonly LSPEditorFeatureDetector _lspEditorFeatureDetector;
    private readonly ProjectConfigurationFilePathStore _projectConfigurationFilePathStore;
    private readonly Dictionary<string, ProjectSnapshot> _pendingProjectPublishes;
    private readonly object _pendingProjectPublishesLock;
    private readonly object _publishLock;

    private readonly JsonSerializer _serializer = new();
    private ProjectSnapshotManagerBase? _projectSnapshotManager;
    private bool _documentsProcessed = false;

    private ProjectSnapshotManagerBase ProjectSnapshotManager
    {
        get
        {
            return _projectSnapshotManager ?? throw new InvalidOperationException($"{nameof(ProjectSnapshotManager)} called before {nameof(Initialize)}.");
        }
        set
        {
            _projectSnapshotManager = value;
        }
    }

    [ImportingConstructor]
    public ProjectRazorJsonPublisher(
        LSPEditorFeatureDetector lSPEditorFeatureDetector,
        ProjectConfigurationFilePathStore projectConfigurationFilePathStore,
        RazorLogger logger)
    {
        if (lSPEditorFeatureDetector is null)
        {
            throw new ArgumentNullException(nameof(lSPEditorFeatureDetector));
        }

        if (projectConfigurationFilePathStore is null)
        {
            throw new ArgumentNullException(nameof(projectConfigurationFilePathStore));
        }

        if (logger is null)
        {
            throw new ArgumentNullException(nameof(logger));
        }

        DeferredPublishTasks = new Dictionary<string, Task>(FilePathComparer.Instance);
        _pendingProjectPublishes = new Dictionary<string, ProjectSnapshot>(FilePathComparer.Instance);
        _pendingProjectPublishesLock = new();
        _publishLock = new object();

        _lspEditorFeatureDetector = lSPEditorFeatureDetector;
        _projectConfigurationFilePathStore = projectConfigurationFilePathStore;
        _logger = logger;

        _serializer.Converters.Add(TagHelperDescriptorJsonConverter.Instance);
        _serializer.Converters.Add(RazorConfigurationJsonConverter.Instance);
        _serializer.Converters.Add(ProjectRazorJsonJsonConverter.Instance);
    }

    // Internal settable for testing
    // 3000ms between publishes to prevent bursts of changes yet still be responsive to changes.
    internal int EnqueueDelay { get; set; } = 3000;

    public override void Initialize(ProjectSnapshotManagerBase projectManager)
    {
        ProjectSnapshotManager = projectManager;
        ProjectSnapshotManager.Changed += ProjectSnapshotManager_Changed;
    }

    // Internal for testing
    internal void EnqueuePublish(ProjectSnapshot projectSnapshot)
    {
        lock (_pendingProjectPublishesLock)
        {
            _pendingProjectPublishes[projectSnapshot.FilePath] = projectSnapshot;
        }

        if (!DeferredPublishTasks.TryGetValue(projectSnapshot.FilePath, out var update) || update.IsCompleted)
        {
            DeferredPublishTasks[projectSnapshot.FilePath] = PublishAfterDelayAsync(projectSnapshot.FilePath);
        }
    }

    internal void ProjectSnapshotManager_Changed(object sender, ProjectChangeEventArgs args)
    {
        // Don't do any work if the solution is closing
        if (args.SolutionIsClosing)
        {
            return;
        }

        if (!_lspEditorFeatureDetector.IsLSPEditorAvailable())
        {
            return;
        }

        // Prior to doing any sort of project state serialization/publishing we avoid any excess work as long as there aren't any "open" Razor files.
        // However, once a Razor file is opened we turn the firehose on and start doing work.
        // By taking this lazy approach we ensure that we don't do any excess Razor work (serialization is expensive) in non-Razor scenarios.

        if (!_active)
        {
            // Not currently active, we need to decide if we should become active or if we should no-op.

            if (ProjectSnapshotManager.OpenDocuments.Count > 0)
            {
                // A Razor document was just opened, we should become "active" which means we'll constantly be monitoring project state.
                _active = true;

                if (ProjectWorkspacePublishable(args))
                {
                    // Typically document open events don't result in us re-processing project state; however, given this is the first time a user opened a Razor document we should.
                    // Don't enqueue, just publish to get the most immediate result.
                    ImmediatePublish(args.Newer!);
                    return;
                }
            }
            else
            {
                // No open documents and not active. No-op.
                return;
            }
        }

        // All the below Publish's (except ProjectRemoved) wait until our project has been initialized (ProjectWorkspaceState != null)
        // so that we don't publish half-finished projects, which can cause things like Semantic coloring to "flash"
        // when they update repeatedly as they load.
        switch (args.Kind)
        {
            case ProjectChangeKind.ProjectChanged:
                if (!ProjectWorkspacePublishable(args))
                {
                    break;
                }

                if (!ReferenceEquals(args.Newer!.ProjectWorkspaceState, args.Older!.ProjectWorkspaceState))
                {
                    // If our workspace state has changed since our last snapshot then this means pieces influencing
                    // TagHelper resolution have also changed. Fast path the TagHelper publish.
                    ImmediatePublish(args.Newer);
                }
                else
                {
                    // There was a project change that doesn't seem to be related to TagHelpers, we can be
                    // less aggressive and do a delayed publish.
                    EnqueuePublish(args.Newer);
                }

                break;
            case ProjectChangeKind.DocumentRemoved:
            case ProjectChangeKind.DocumentAdded:

                if (ProjectWorkspacePublishable(args))
                {
                    // These changes can come in bursts so we don't want to overload the publishing system. Therefore,
                    // we enqueue publishes and then publish the latest project after a delay.
                    EnqueuePublish(args.Newer!);
                }

                break;

            case ProjectChangeKind.ProjectAdded:

                if (ProjectWorkspacePublishable(args))
                {
                    ImmediatePublish(args.Newer!);
                }

                break;

            case ProjectChangeKind.ProjectRemoved:
                RemovePublishingData(args.Older!);
                break;
        }

        static bool ProjectWorkspacePublishable(ProjectChangeEventArgs args)
        {
            return args.Newer?.ProjectWorkspaceState != null;
        }
    }

    // Internal for testing
    internal void Publish(ProjectSnapshot projectSnapshot)
    {
        if (projectSnapshot is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshot));
        }

        lock (_publishLock)
        {
            string? configurationFilePath = null;
            try
            {
                if (!_projectConfigurationFilePathStore.TryGet(projectSnapshot.FilePath, out configurationFilePath))
                {
                    return;
                }

                // We don't want to serialize the project until it's ready to avoid flashing as the project loads different parts.
                // Since the project configuration from last session likely still exists the experience is unlikely to be degraded by this delay.
                // An exception is made for when there's no existing project configuration file because some flashing is preferable to having no TagHelper knowledge.
                if (ShouldSerialize(projectSnapshot, configurationFilePath))
                {
                    SerializeToFile(projectSnapshot, configurationFilePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($@"Could not update Razor project configuration file '{configurationFilePath}':
{ex}");
            }
        }
    }

    // Internal for testing
    internal void RemovePublishingData(ProjectSnapshot projectSnapshot)
    {
        lock (_publishLock)
        {
            var oldProjectFilePath = projectSnapshot.FilePath;
            if (!_projectConfigurationFilePathStore.TryGet(oldProjectFilePath, out var configurationFilePath))
            {
                // If we don't track the value in PublishFilePathMappings that means it's already been removed, do nothing.
                return;
            }

            lock (_pendingProjectPublishesLock)
            {
                if (_pendingProjectPublishes.TryGetValue(oldProjectFilePath, out _))
                {
                    // Project was removed while a delayed publish was in flight. Clear the in-flight publish so it noops.
                    _pendingProjectPublishes.Remove(oldProjectFilePath);
                }
            }
        }
    }

    protected virtual void SerializeToFile(ProjectSnapshot projectSnapshot, string publishFilePath)
    {
        // We need to avoid having an incomplete file at any point, but our
        // project configuration file is large enough that it will be written as multiple operations.
        var tempFilePath = string.Concat(publishFilePath, TempFileExt);
        var tempFileInfo = new FileInfo(tempFilePath);

        if (tempFileInfo.Exists)
        {
            // This could be caused by failures during serialization or early process termination.
            tempFileInfo.Delete();
        }

        // This needs to be in explicit brackets because the operation needs to be completed
        // by the time we move the tempfile into its place
        using (var writer = tempFileInfo.CreateText())
        {
            var projectRazorJson = new ProjectRazorJson(publishFilePath, projectSnapshot);
            _serializer.Serialize(writer, projectRazorJson);
        }

        var fileInfo = new FileInfo(publishFilePath);
        if (fileInfo.Exists)
        {
            fileInfo.Delete();
        }

        File.Move(tempFilePath, publishFilePath);
    }

    protected virtual bool FileExists(string file)
    {
        return File.Exists(file);
    }

    protected virtual bool ShouldSerialize(ProjectSnapshot projectSnapshot, string configurationFilePath)
    {
        if (!FileExists(configurationFilePath))
        {
            return true;
        }

        // Don't serialize our understanding until we're "ready"
        if (!_documentsProcessed)
        {
            if (projectSnapshot.DocumentFilePaths.Any(d => AspNetCore.Razor.Language.FileKinds.GetFileKindFromFilePath(d)
                .Equals(AspNetCore.Razor.Language.FileKinds.Component, StringComparison.OrdinalIgnoreCase)))
            {
                foreach (var document in projectSnapshot.DocumentFilePaths)
                {
                    // We want to wait until at least one document has been processed (meaning it became a TagHelper.
                    // Because we don't have a way to tell which TagHelpers were created from the local project just from their descriptors we have to improvise
                    // We assume that a document has been processed if at least one Component matches the name of one of our files.
                    var fileName = Path.GetFileNameWithoutExtension(document);

                    var documentSnapshot = projectSnapshot.GetDocument(document);

                    if (documentSnapshot.FileKind.Equals(AspNetCore.Razor.Language.FileKinds.Component, StringComparison.OrdinalIgnoreCase) &&
                        projectSnapshot.TagHelpers.Any(t => t.Name.EndsWith("." + fileName, StringComparison.OrdinalIgnoreCase)))
                    {
                        // Documents have been processed, lets publish
                        _documentsProcessed = true;
                        break;
                    }
                }
            }
            else
            {
                // This project has no Components and thus cannot suffer from the lagging compilation problem.
                _documentsProcessed = true;
            }
        }

        return _documentsProcessed;
    }

    private void ImmediatePublish(ProjectSnapshot projectSnapshot)
    {
        lock (_pendingProjectPublishesLock)
        {
            // Clear any pending publish
            _pendingProjectPublishes.Remove(projectSnapshot.FilePath);
        }

        Publish(projectSnapshot);
    }

    private async Task PublishAfterDelayAsync(string projectFilePath)
    {
        await Task.Delay(EnqueueDelay).ConfigureAwait(false);

        ProjectSnapshot projectSnapshot;
        lock (_pendingProjectPublishesLock)
        {
            if (!_pendingProjectPublishes.TryGetValue(projectFilePath, out projectSnapshot))
            {
                // Project was removed while waiting for the publish delay.
                return;
            }

            _pendingProjectPublishes.Remove(projectFilePath);
        }

        Publish(projectSnapshot);
    }
}
