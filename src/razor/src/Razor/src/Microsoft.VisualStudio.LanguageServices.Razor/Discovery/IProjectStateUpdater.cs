// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis;

namespace Microsoft.VisualStudio.Razor.Discovery;

internal interface IProjectStateUpdater
{
    void EnqueueUpdate(ProjectKey key, ProjectId? id);

    void CancelUpdates();
}
