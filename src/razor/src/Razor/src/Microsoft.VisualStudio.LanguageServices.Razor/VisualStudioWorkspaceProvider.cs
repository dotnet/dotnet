// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.VisualStudio.LanguageServices.Razor;

[Export(typeof(IWorkspaceProvider))]
[method: ImportingConstructor]
internal sealed class VisualStudioWorkspaceProvider(VisualStudioWorkspace workspace) : IWorkspaceProvider
{
    public Workspace GetWorkspace() => workspace;
}
