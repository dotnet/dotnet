// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.CodeAnalysis.Razor;
using Xunit.Abstractions;

namespace Xunit;

public abstract class ProjectSnapshotManagerDispatcherWorkspaceTestBase : WorkspaceTestBase
{
    internal ProjectSnapshotManagerDispatcher Dispatcher { get; }

    protected ProjectSnapshotManagerDispatcherWorkspaceTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        Dispatcher = new TestProjectSnapshotManagerDispatcher();
    }
}
