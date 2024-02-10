// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Moq;

namespace Microsoft.CodeAnalysis.Remote.Razor;

public partial class OutOfProcTagHelperResolverTest
{
    private static readonly Lazy<ProjectSnapshotManagerDispatcher> s_projectSnapshotManagerDispatcher = new(() =>
    {
        var dispatcher = new Mock<ProjectSnapshotManagerDispatcher>(MockBehavior.Strict);
        dispatcher.SetupGet(d => d.IsDispatcherThread).Returns(true);
        return dispatcher.Object;
    });

    private class TestProjectSnapshotManager(IProjectEngineFactoryProvider projectEngineFactoryProvider) : DefaultProjectSnapshotManager(
        triggers: [],
        projectEngineFactoryProvider,
        s_projectSnapshotManagerDispatcher.Value,
        Mock.Of<IErrorReporter>(MockBehavior.Strict))
    {
    }
}
