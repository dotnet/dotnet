// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class DefaultWorkspaceSemanticTokensRefreshTriggerTest : LanguageServerTestBase
{
    private readonly TestProjectSnapshotManager _projectManager;
    private readonly HostProject _hostProject;
    private readonly HostDocument _hostDocument;

    public DefaultWorkspaceSemanticTokensRefreshTriggerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _projectManager = TestProjectSnapshotManager.Create(LegacyDispatcher);
        _projectManager.AllowNotifyListeners = true;
        _hostProject = new HostProject("/path/to/project.csproj", RazorConfiguration.Default, "TestRootNamespace");
        _projectManager.ProjectAdded(_hostProject);
        _hostDocument = new HostDocument("/path/to/file.razor", "file.razor");
        _projectManager.DocumentAdded(_hostProject, _hostDocument, new EmptyTextLoader(_hostDocument.FilePath));
    }

    [Fact]
    public void PublishesOnWorkspaceUpdate()
    {
        // Arrange
        var workspaceChangedPublisher = new Mock<WorkspaceSemanticTokensRefreshPublisher>(MockBehavior.Strict);
        workspaceChangedPublisher.Setup(w => w.EnqueueWorkspaceSemanticTokensRefresh());
        var defaultWorkspaceChangedRefresh = new TestDefaultWorkspaceSemanticTokensRefreshTrigger(workspaceChangedPublisher.Object);
        defaultWorkspaceChangedRefresh.Initialize(_projectManager);

        // Act
        var newDocument = new HostDocument("/path/to/newFile.razor", "newFile.razor");
        _projectManager.DocumentAdded(_hostProject, newDocument, new EmptyTextLoader(newDocument.FilePath));

        // Assert
        workspaceChangedPublisher.VerifyAll();
    }

    private class TestDefaultWorkspaceSemanticTokensRefreshTrigger : DefaultWorkspaceSemanticTokensRefreshTrigger
    {
        internal TestDefaultWorkspaceSemanticTokensRefreshTrigger(WorkspaceSemanticTokensRefreshPublisher workspaceSemanticTokensRefreshPublisher)
            : base(workspaceSemanticTokensRefreshPublisher)
        {
        }
    }

    private class TestErrorReporter : ErrorReporter
    {
        public override void ReportError(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override void ReportError(Exception exception, ProjectSnapshot? project)
        {
            throw new NotImplementedException();
        }

        public override void ReportError(Exception exception, Project workspaceProject)
        {
            throw new NotImplementedException();
        }
    }
}
