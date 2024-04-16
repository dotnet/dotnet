// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Razor.LanguageClient.ProjectSystem;

/// <summary>
/// Publishes project data (including TagHelper info) discovered OOB to the server via LSP notification
/// instead of old method of writing a project configuration bin file
/// </summary>
[Export(typeof(RazorProjectInfoEndpointPublisher))]
internal partial class RazorProjectInfoEndpointPublisher : IDisposable
{
    private readonly LSPRequestInvoker _requestInvoker;
    private readonly IProjectSnapshotManager _projectManager;

    private readonly AsyncBatchingWorkQueue<(IProjectSnapshot Project, bool Removal)> _workQueue;
    private readonly CancellationTokenSource _disposeTokenSource;

    // Delay between publishes to prevent bursts of changes yet still be responsive to changes.
    private static readonly TimeSpan s_enqueueDelay = TimeSpan.FromMilliseconds(250);

    [ImportingConstructor]
    public RazorProjectInfoEndpointPublisher(
        LSPRequestInvoker requestInvoker,
        IProjectSnapshotManager projectManager)
        : this(requestInvoker, projectManager, s_enqueueDelay)
    {
    }

    // Provided for tests to specify enqueue delay
    public RazorProjectInfoEndpointPublisher(
        LSPRequestInvoker requestInvoker,
        IProjectSnapshotManager projectManager,
        TimeSpan enqueueDelay)
    {
        _requestInvoker = requestInvoker;
        _projectManager = projectManager;

        _disposeTokenSource = new();
        _workQueue = new(
            enqueueDelay,
            ProcessBatchAsync,
            _disposeTokenSource.Token);
    }

    public void Dispose()
    {
        _disposeTokenSource.Cancel();
        _disposeTokenSource.Dispose();
    }

    public void StartSending()
    {
        _projectManager.Changed += ProjectManager_Changed;

        var projects = _projectManager.GetProjects();
        foreach (var project in projects)
        {
            // Don't enqueue project addition as those are unlikely to come through in large batches.
            // Also ensures that we won't get project removal go through the queue without project addition.
            ImmediatePublish(project, _disposeTokenSource.Token);
        }
    }

    private void ProjectManager_Changed(object sender, ProjectChangeEventArgs args)
    {
        // Don't do any work if the solution is closing
        if (args.SolutionIsClosing)
        {
            return;
        }

        switch (args.Kind)
        {
            case ProjectChangeKind.ProjectChanged:
            case ProjectChangeKind.DocumentRemoved:
            case ProjectChangeKind.DocumentAdded:
                if (args.Newer is null)
                {
                    break;
                }

                EnqueuePublish(args.Newer.AssumeNotNull());

                break;

            case ProjectChangeKind.ProjectAdded:

                if (args.Newer is not null)
                {
                    // Don't enqueue project addition as those are unlikely to come through in large batches.
                    // Also ensures that we won't get project removal go through the queue without project addition.
                    ImmediatePublish(args.Newer.AssumeNotNull(), _disposeTokenSource.Token);
                }

                break;

            case ProjectChangeKind.ProjectRemoved:
                // Enqueue removal so it will replace any other project changes in the work queue as they unnecessary now.
                EnqueueRemoval(args.Older.AssumeNotNull());
                break;

            default:
                Debug.Fail("A new ProjectChangeKind has been added that the RazorProjectInfoEndpointPublisher doesn't know how to deal with");
                break;
        }
    }

    private void EnqueuePublish(IProjectSnapshot projectSnapshot)
    {
        _workQueue.AddWork((Project: projectSnapshot, Removal: false));
    }

    private void EnqueueRemoval(IProjectSnapshot projectSnapshot)
    {
        _workQueue.AddWork((Project: projectSnapshot, Removal: true));
    }

    private ValueTask ProcessBatchAsync(ImmutableArray<(IProjectSnapshot Project, bool Removal)> workItems, CancellationToken cancellationToken)
    {
        foreach (var workItem in workItems.GetMostRecentUniqueItems(Comparer.Instance))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return default;
            }

            if (workItem.Removal)
            {
                RemovePublishingData(workItem.Project, cancellationToken);
            }
            else
            {
                ImmediatePublish(workItem.Project, cancellationToken);
            }
        }

        return default;
    }

    private void RemovePublishingData(IProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
    {
        // This should never get called if we are inactive, so don't check _active flag
        ImmediatePublish(projectSnapshot.Key, encodedProjectInfo: null, cancellationToken);
    }

    private void ImmediatePublish(IProjectSnapshot projectSnapshot, CancellationToken cancellationToken)
    {
        using var stream = new MemoryStream();

        var projectInfo = projectSnapshot.ToRazorProjectInfo(projectSnapshot.IntermediateOutputPath);
        projectInfo.SerializeTo(stream);
        var base64ProjectInfo = Convert.ToBase64String(stream.ToArray());

        ImmediatePublish(projectSnapshot.Key, base64ProjectInfo, cancellationToken);
    }

    private void ImmediatePublish(ProjectKey projectKey, string? encodedProjectInfo, CancellationToken cancellationToken)
    {
        // This might be getting called after getting dequeued from work queue,
        // check if the work got cancelled before doing anything.
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        var parameter = new ProjectInfoParams()
        {
            ProjectKeyId = projectKey.Id,
            ProjectInfo = encodedProjectInfo
        };

        _requestInvoker.ReinvokeRequestOnServerAsync<ProjectInfoParams, object>(
                LanguageServerConstants.RazorProjectInfoEndpoint,
                RazorLSPConstants.RazorLanguageServerName,
                parameter,
                cancellationToken).Forget();
    }
}
