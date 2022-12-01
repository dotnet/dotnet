// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Projection;

namespace Microsoft.VisualStudio.LanguageServices.Razor;

[System.Composition.Shared]
[Export(typeof(VisualStudioWorkspaceAccessor))]
internal class DefaultVisualStudioWorkspaceAccessor : VisualStudioWorkspaceAccessor
{
    private readonly IBufferGraphFactoryService _bufferGraphService;
    private readonly TextBufferProjectService _projectService;
    private readonly Workspace _defaultWorkspace;

    [ImportingConstructor]
    public DefaultVisualStudioWorkspaceAccessor(
        IBufferGraphFactoryService bufferGraphService,
        TextBufferProjectService projectService,
        [Import(typeof(VisualStudioWorkspace))] Workspace defaultWorkspace)
    {
        if (bufferGraphService is null)
        {
            throw new ArgumentNullException(nameof(bufferGraphService));
        }

        if (projectService is null)
        {
            throw new ArgumentNullException(nameof(projectService));
        }

        if (defaultWorkspace is null)
        {
            throw new ArgumentNullException(nameof(defaultWorkspace));
        }

        _bufferGraphService = bufferGraphService;
        _projectService = projectService;
        _defaultWorkspace = defaultWorkspace;
    }

    public override bool TryGetWorkspace(ITextBuffer textBuffer, [NotNullWhen(returnValue: true)] out Workspace? workspace)
    {
        if (textBuffer is null)
        {
            throw new ArgumentNullException(nameof(textBuffer));
        }

        // We do a best effort approach in this method to get the workspace that belongs to the TextBuffer.
        // The approaches we take to find the workspace are:
        //
        // 1. Look for a C# projection buffer associated with the Razor buffer. If we can find one we let
        //    Roslyn take control of finding the Workspace (projectionBuffer.GetWorkspace()). If not,
        //    fall back to determining if we can use the default workspace.
        // 2. Look to see if this ITextBuffer is associated with a host project. If we find that our Razor
        //    buffer has a host project, we make the assumption that we should use the default VisualStudioWorkspace.

        if (TryGetWorkspaceFromProjectionBuffer(textBuffer, out workspace))
        {
            return true;
        }

        if (TryGetWorkspaceFromHostProject(textBuffer, out workspace))
        {
            return true;
        }

        workspace = null;
        return false;
    }

    // Internal virtual for testing
    internal virtual bool TryGetWorkspaceFromProjectionBuffer(ITextBuffer textBuffer, [NotNullWhen(returnValue: true)] out Workspace? workspace)
    {
        var graph = _bufferGraphService.CreateBufferGraph(textBuffer);
        var projectedCSharpBuffer = graph.GetTextBuffers(buffer => buffer.ContentType.IsOfType("CSharp")).FirstOrDefault();

        if (projectedCSharpBuffer is null)
        {
            workspace = null;
            return false;
        }

        workspace = projectedCSharpBuffer.GetWorkspace();
        if (workspace is null)
        {
            // Couldn't resolve a workspace for the projected csharp buffer.
            return false;
        }

        return true;
    }

    // Internal virtual for testing
    internal virtual bool TryGetWorkspaceFromHostProject(ITextBuffer textBuffer, [NotNullWhen(returnValue: true)] out Workspace? workspace)
    {
        var project = _projectService.GetHostProject(textBuffer);

        if (project is null)
        {
            // Could not locate a project for the given text buffer.
            workspace = null;
            return false;
        }

        // We have a host project, assume default workspace.
        workspace = _defaultWorkspace;
        return true;
    }
}
