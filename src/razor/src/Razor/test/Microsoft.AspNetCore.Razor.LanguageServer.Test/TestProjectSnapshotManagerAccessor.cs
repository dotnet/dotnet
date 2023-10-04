// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test;

internal class TestProjectSnapshotManagerAccessor : ProjectSnapshotManagerAccessor
{
    public TestProjectSnapshotManagerAccessor(ProjectSnapshotManagerBase instance)
    {
        Instance = instance;
    }

    public override ProjectSnapshotManagerBase Instance { get; }
}
