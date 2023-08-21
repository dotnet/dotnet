// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal interface IProjectSnapshotChangeTrigger
{
    void Initialize(ProjectSnapshotManagerBase projectManager);
}

internal interface IPriorityProjectSnapshotChangeTrigger : IProjectSnapshotChangeTrigger
{
}
