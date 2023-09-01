// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.VisualStudio.LiveShare.Razor;

internal sealed class ProjectSnapshotManagerProxyState(IReadOnlyList<ProjectSnapshotHandleProxy> projectHandles)
{
    public IReadOnlyList<ProjectSnapshotHandleProxy> ProjectHandles { get; } = projectHandles ?? throw new ArgumentNullException(nameof(projectHandles));
}
