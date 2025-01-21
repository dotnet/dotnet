// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Razor;

internal interface IProjectWorkspaceStateGenerator
{
    void EnqueueUpdate(Project? workspaceProject, ProjectSnapshot projectSnapshot);

    void CancelUpdates();
}
