// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Editor;

/// <summary>
/// Infrastructure methods to find project information from an <see cref="ITextBuffer"/>.
/// </summary>
[System.Composition.Shared]
[Export(typeof(TextBufferProjectService))]
internal class DefaultTextBufferProjectService : TextBufferProjectService
{
    private const string DotNetCoreCapability = "(CSharp|VB)&CPS";

    private readonly RunningDocumentTable _documentTable;
    private readonly ITextDocumentFactoryService _documentFactory;
    private readonly AggregateProjectCapabilityResolver _projectCapabilityResolver;

    [ImportingConstructor]
    public DefaultTextBufferProjectService(
        [Import(typeof(SVsServiceProvider))] IServiceProvider services,
        ITextDocumentFactoryService documentFactory,
        AggregateProjectCapabilityResolver projectCapabilityResolver)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

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
        _documentTable = new RunningDocumentTable(services);
    }

    public override object? GetHostProject(ITextBuffer textBuffer)
    {
        if (textBuffer is null)
        {
            throw new ArgumentNullException(nameof(textBuffer));
        }

        // If there's no document we can't find the FileName, or look for a matching hierarchy.
        if (!_documentFactory.TryGetTextDocument(textBuffer, out var textDocument))
        {
            return null;
        }

        var hostProject = GetHostProject(textDocument.FilePath);
        return hostProject;
    }

    public override object? GetHostProject(string documentFilePath)
    {
        _documentTable.FindDocument(documentFilePath, out var hierarchy, out _, out _);

        // We don't currently try to look a Roslyn ProjectId at this point, we just want to know some
        // basic things.
        // See https://github.com/dotnet/roslyn/blob/4e3db2b7a0732d45a720e9ed00c00cd22ab67a14/src/VisualStudio/Core/SolutionExplorerShim/HierarchyItemToProjectIdMap.cs#L47
        // for a more complete implementation.
        return hierarchy;
    }

    public override string GetProjectPath(object project)
    {
        if (project is null)
        {
            throw new ArgumentNullException(nameof(project));
        }

        var hierarchy = project as IVsHierarchy;
        Assumes.NotNull(hierarchy);

        ErrorHandler.ThrowOnFailure(((IVsProject)hierarchy).GetMkDocument((uint)VSConstants.VSITEMID.Root, out var path), VSConstants.E_NOTIMPL);
        return path;
    }

    public override bool IsSupportedProject(object project)
    {
        if (project is null)
        {
            throw new ArgumentNullException(nameof(project));
        }

        var capabilitySupported = _projectCapabilityResolver.HasCapability(project, DotNetCoreCapability);
        return capabilitySupported;
    }

    public override string? GetProjectName(object project)
    {
        if (project is null)
        {
            throw new ArgumentNullException(nameof(project));
        }

        var hierarchy = (IVsHierarchy)project;

        if (ErrorHandler.Failed(hierarchy.GetProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_Name, out var name)))
        {
            return null;
        }

        return (string)name;
    }
}
