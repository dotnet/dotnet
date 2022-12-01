// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.CodeAnalysis.Razor;
using Xunit.Abstractions;

namespace Xunit;

public abstract class ProjectSnapshotManagerDispatcherTestBase : ParserTestBase
{
    internal ProjectSnapshotManagerDispatcher Dispatcher { get; }

    protected ProjectSnapshotManagerDispatcherTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        Dispatcher = new TestProjectSnapshotManagerDispatcher();
    }
}
