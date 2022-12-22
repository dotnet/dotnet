// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal abstract class ProjectSnapshotChangeTrigger
{
    public virtual int InitializePriority { get; }

    public abstract void Initialize(ProjectSnapshotManagerBase projectManager);
}
