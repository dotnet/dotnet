// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Linq;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Test.Common;

internal class TestProjectSnapshotManager : DefaultProjectSnapshotManager
{
    private TestProjectSnapshotManager(ProjectSnapshotManagerDispatcher dispatcher, IErrorReporter errorReporter, Workspace workspace)
        : base(dispatcher, errorReporter, Array.Empty<ProjectSnapshotChangeTrigger>(), workspace)
    {
    }

    public static TestProjectSnapshotManager Create(ProjectSnapshotManagerDispatcher dispatcher, IErrorReporter errorReporter)
    {
        if (dispatcher is null)
        {
            throw new ArgumentNullException(nameof(dispatcher));
        }

        var services = TestServices.Create(
            workspaceServices: new[]
            {
                new DefaultProjectSnapshotProjectEngineFactory(new FallbackProjectEngineFactory(), MefProjectEngineFactories.Factories)
            },
            razorLanguageServices: Enumerable.Empty<ILanguageService>());
        var workspace = TestWorkspace.Create(services);
        var testProjectManager = new TestProjectSnapshotManager(dispatcher, errorReporter, workspace);

        return testProjectManager;
    }

    public bool AllowNotifyListeners { get; set; }

    protected override void NotifyListeners(ProjectChangeEventArgs e)
    {
        if (AllowNotifyListeners)
        {
            base.NotifyListeners(e);
        }
    }
}
