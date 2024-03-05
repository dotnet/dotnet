// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test;

internal class TestProjectSnapshotManagerAccessor(ProjectSnapshotManagerBase instance) : IProjectSnapshotManagerAccessor
{
    private readonly ProjectSnapshotManagerBase _instance = instance;

    public ProjectSnapshotManagerBase Instance => _instance;

    public bool TryGetInstance([NotNullWhen(true)] out ProjectSnapshotManagerBase? instance)
    {
        instance = _instance;
        return instance is not null;
    }
}
