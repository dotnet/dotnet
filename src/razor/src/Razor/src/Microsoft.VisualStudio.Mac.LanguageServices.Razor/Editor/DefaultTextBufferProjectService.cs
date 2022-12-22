// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Text;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace Microsoft.VisualStudio.Mac.LanguageServices.Razor.Editor;

/// <summary>
/// Infrastructure methods to find project information from an <see cref="ITextBuffer"/>.
/// </summary>
[System.Composition.Shared]
[Export(typeof(TextBufferProjectService))]
internal class DefaultTextBufferProjectService : TextBufferProjectService
{
    private const string DotNetCoreRazorCapability = "DotNetCoreRazor | AspNetCore";
    private readonly ITextDocumentFactoryService _documentFactory;
    private readonly AggregateProjectCapabilityResolver _projectCapabilityResolver;

    [ImportingConstructor]
    public DefaultTextBufferProjectService(
        ITextDocumentFactoryService documentFactory,
        AggregateProjectCapabilityResolver projectCapabilityResolver)
    {
        if (documentFactory is null)
        {
            throw new ArgumentNullException(nameof(documentFactory));
        }

        if (projectCapabilityResolver is null)
        {
            throw new ArgumentNullException(nameof(projectCapabilityResolver));
        }

        _documentFactory = documentFactory;
        _projectCapabilityResolver = projectCapabilityResolver;
    }

    public override object? GetHostProject(ITextBuffer textBuffer)
    {
        if (textBuffer is null)
        {
            throw new ArgumentNullException(nameof(textBuffer));
        }

        // If there's no document we can't find the FileName, or look for an associated project.
        if (!_documentFactory.TryGetTextDocument(textBuffer, out var textDocument))
        {
            return null;
        }

        var hostProject = GetHostProject(textDocument.FilePath);
        return hostProject;
    }

    public override object? GetHostProject(string documentFilePath)
    {
        var projectsContainingFilePath = IdeApp.Workspace.GetProjectsContainingFile(documentFilePath);
        foreach (var project in projectsContainingFilePath)
        {
            if (project is not DotNetProject)
            {
                continue;
            }

            var projectFile = project.GetProjectFile(documentFilePath);
            if (!projectFile.IsHidden)
            {
                return project;
            }
        }

        return null;
    }

    public override string GetProjectPath(object project)
    {
        if (project is null)
        {
            throw new ArgumentNullException(nameof(project));
        }

        var dotnetProject = (DotNetProject)project;
        return dotnetProject.FileName.FullPath;
    }

    // VisualStudio for Mac only supports ASP.NET Core Razor.
    public override bool IsSupportedProject(object project)
    {
        var capabilitySupported = _projectCapabilityResolver.HasCapability(project, DotNetCoreRazorCapability);
        return capabilitySupported;
    }

    public override string GetProjectName(object project)
    {
        if (project is null)
        {
            throw new ArgumentNullException(nameof(project));
        }

        var dotnetProject = (DotNetProject)project;

        return dotnetProject.Name;
    }
}
