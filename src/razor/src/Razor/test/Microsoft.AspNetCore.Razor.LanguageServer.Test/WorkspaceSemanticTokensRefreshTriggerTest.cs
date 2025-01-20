// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class WorkspaceSemanticTokensRefreshTriggerTest : LanguageServerTestBase
{
    private static readonly HostProject s_hostProject = new("/path/to/project.csproj", "/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
    private static readonly HostDocument s_hostDocument = new("/path/to/file.razor", "file.razor");

    private readonly TestProjectSnapshotManager _projectManager;

    public WorkspaceSemanticTokensRefreshTriggerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _projectManager = CreateProjectSnapshotManager();
    }

    protected override Task InitializeAsync()
    {
        return _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
            updater.AddDocument(s_hostProject.Key, s_hostDocument, EmptyTextLoader.Instance);
        });
    }

    [Fact]
    public async Task NotifiesOnWorkspaceUpdate()
    {
        // Arrange
        var publisher = new StrictMock<IWorkspaceSemanticTokensRefreshNotifier>();
        publisher
            .Setup(w => w.NotifyWorkspaceSemanticTokensRefresh())
            .Verifiable();

        var refreshTrigger = new TestWorkspaceSemanticTokensRefreshTrigger(publisher.Object, _projectManager);

        // Act
        var newDocument = new HostDocument("/path/to/newFile.razor", "newFile.razor");

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddDocument(s_hostProject.Key, newDocument, EmptyTextLoader.Instance);
        });

        // Assert
        publisher.VerifyAll();
    }

    private class TestWorkspaceSemanticTokensRefreshTrigger(
        IWorkspaceSemanticTokensRefreshNotifier publisher,
        ProjectSnapshotManager projectManager)
        : WorkspaceSemanticTokensRefreshTrigger(publisher, projectManager);
}
