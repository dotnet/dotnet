// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.ProjectSystem;

internal interface IUpdateProjectAction
{
}

internal record RemoveDocumentAction(HostDocument OriginalDocument) : IUpdateProjectAction;

internal record AddDocumentAction(HostDocument NewDocument, TextLoader TextLoader) : IUpdateProjectAction;

internal record UpdateDocumentAction(HostDocument OriginalDocument, HostDocument NewDocument, TextLoader TextLoader) : IUpdateProjectAction;

internal record MoveDocumentAction(IProjectSnapshot OriginalProject, IProjectSnapshot DestinationProject, HostDocument Document, TextLoader TextLoader) : IUpdateProjectAction;

internal record OpenDocumentAction(SourceText SourceText) : IUpdateProjectAction;

internal record CloseDocumentAction(TextLoader TextLoader) : IUpdateProjectAction;

internal record DocumentTextChangedAction(SourceText SourceText) : IUpdateProjectAction;

internal record DocumentTextLoaderChangedAction(TextLoader TextLoader) : IUpdateProjectAction;

internal record ProjectAddedAction(HostProject HostProject) : IUpdateProjectAction;

internal record ProjectRemovedAction(ProjectKey ProjectKey) : IUpdateProjectAction;

internal record HostProjectUpdatedAction(HostProject HostProject) : IUpdateProjectAction;

internal record ProjectWorkspaceStateChanged(ProjectWorkspaceState WorkspaceState) : IUpdateProjectAction;
