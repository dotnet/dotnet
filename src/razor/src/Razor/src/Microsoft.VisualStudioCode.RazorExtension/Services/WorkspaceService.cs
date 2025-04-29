// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;
using Microsoft.Extensions.Logging;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

[ExportRazorStatelessLspService(typeof(RazorWorkspaceService)), Shared]
[method: ImportingConstructor]
internal sealed class WorkspaceService(ILoggerFactory loggerFactory) : RazorWorkspaceService
{
    private readonly ILoggerFactory _loggerFactory = loggerFactory;
    private readonly ILogger _logger = loggerFactory.CreateLogger<WorkspaceService>();
    private readonly Lock _initializeLock = new();
    private RazorWorkspaceListener? _razorWorkspaceListener;
    private HashSet<ProjectId>? _projectIdWithDynamicFiles = [];

    public override void Initialize(Workspace workspace, string pipeName)
    {
        HashSet<ProjectId> projectsToInitialize;
        lock (_initializeLock)
        {
            // Only initialize once
            if (_razorWorkspaceListener is not null)
            {
                return;
            }

            //_logger.LogTrace("Initializing the Razor workspace listener with pipe name {0}", pipeName);
            _razorWorkspaceListener = new RazorWorkspaceListener(_loggerFactory);
            _razorWorkspaceListener.EnsureInitialized(workspace, pipeName);

            // _projectIdWithDynamicFiles won't be used again after initialization is done
            projectsToInitialize = _projectIdWithDynamicFiles.AssumeNotNull();
            _projectIdWithDynamicFiles = null;
        }

        foreach (var projectId in projectsToInitialize)
        {
            _logger.LogTrace("{projectId} notifying a dynamic file for the first time", projectId);
            _razorWorkspaceListener.NotifyDynamicFile(projectId);
        }
    }

    public override void NotifyDynamicFile(ProjectId projectId)
    {
        if (_razorWorkspaceListener is null)
        {
            lock (_initializeLock)
            {
                if (_razorWorkspaceListener is not null)
                {
                    _razorWorkspaceListener.NotifyDynamicFile(projectId);
                    return;
                }

                _projectIdWithDynamicFiles
                    .AssumeNotNull()
                    .Add(projectId);
            }
        }
        else
        {
            _razorWorkspaceListener?.NotifyDynamicFile(projectId);
        }
    }
}
