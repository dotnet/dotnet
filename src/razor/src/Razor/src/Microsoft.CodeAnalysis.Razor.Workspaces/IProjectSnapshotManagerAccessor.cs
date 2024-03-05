// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal interface IProjectSnapshotManagerAccessor
{
    ProjectSnapshotManagerBase Instance { get; }

    /// <summary>
    ///  Retrieves the <see cref="ProjectSnapshotManagerBase"/> instance. Returns <see langword="true"/>
    ///  if the instance has been created; otherwise, <see langword="false"/>.
    /// </summary>
    bool TryGetInstance([NotNullWhen(true)] out ProjectSnapshotManagerBase? instance);
}
