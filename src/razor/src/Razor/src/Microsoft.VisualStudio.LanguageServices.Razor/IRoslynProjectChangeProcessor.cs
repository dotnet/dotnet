// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Razor;

internal interface IRoslynProjectChangeProcessor
{
    void EnqueueUpdate(Project? workspaceProject, IProjectSnapshot projectSnapshot);

    void CancelUpdates();
}
