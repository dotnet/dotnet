// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.Test.Common.Editor;

public abstract class ProjectSnapshotManagerDispatcherWorkspaceTestBase(ITestOutputHelper testOutput) : WorkspaceTestBase(testOutput)
{
    private protected override ProjectSnapshotManagerDispatcher CreateDispatcher()
        => new TestProjectSnapshotManagerDispatcher();
}
