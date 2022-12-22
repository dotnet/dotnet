// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using static Microsoft.CodeAnalysis.Razor.Workspaces.DefaultRazorDynamicFileInfoProvider;

namespace Microsoft.VisualStudio.LanguageServices.Razor.ProjectSystem;

public class DefaultRazorDynamicFileInfoProviderTest : WorkspaceTestBase
{
    private readonly DefaultRazorDynamicFileInfoProvider _provider;
    private readonly TestAccessor _testAccessor;
    private readonly RazorDocumentServiceProviderFactory _documentServiceFactory;
    private readonly LSPEditorFeatureDetector _editorFeatureDetector;
    private readonly TestProjectSnapshotManager _projectSnapshotManager;
    private readonly DefaultProjectSnapshot _project;
    private readonly DefaultDocumentSnapshot _document1;
    private readonly DefaultDocumentSnapshot _document2;
    private readonly DynamicDocumentContainer _lspDocumentContainer;

    public DefaultRazorDynamicFileInfoProviderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _documentServiceFactory = new DefaultRazorDocumentServiceProviderFactory();
        _editorFeatureDetector = Mock.Of<LSPEditorFeatureDetector>(MockBehavior.Strict);
        _projectSnapshotManager = new TestProjectSnapshotManager(Workspace)
        {
            AllowNotifyListeners = true
        };
        var hostProject = new HostProject("C:\\project.csproj", RazorConfiguration.Default, rootNamespace: "TestNamespace");
        _projectSnapshotManager.ProjectAdded(hostProject);
        var hostDocument1 = new HostDocument("C:\\document1.razor", "document1.razor", FileKinds.Component);
        _projectSnapshotManager.DocumentAdded(hostProject, hostDocument1, new EmptyTextLoader(hostDocument1.FilePath));
        var hostDocument2 = new HostDocument("C:\\document2.razor", "document2.razor", FileKinds.Component);
        _projectSnapshotManager.DocumentAdded(hostProject, hostDocument2, new EmptyTextLoader(hostDocument2.FilePath));
        _project = _projectSnapshotManager.GetSnapshot(hostProject);
        _document1 = (DefaultDocumentSnapshot)_project.GetDocument(hostDocument1.FilePath);
        _document2 = (DefaultDocumentSnapshot)_project.GetDocument(hostDocument2.FilePath);

        _provider = new DefaultRazorDynamicFileInfoProvider(_documentServiceFactory, _editorFeatureDetector, TestLanguageServerFeatureOptions.Instance);
        _testAccessor = _provider.GetTestAccessor();
        _provider.Initialize(_projectSnapshotManager);

        var lspDocumentContainer = new Mock<DynamicDocumentContainer>(MockBehavior.Strict);
        lspDocumentContainer.SetupSet(c => c.SupportsDiagnostics = true).Verifiable();
        lspDocumentContainer.Setup(container => container.GetTextLoader(It.IsAny<string>())).Returns(new EmptyTextLoader(string.Empty));
        _lspDocumentContainer = lspDocumentContainer.Object;
    }

    [Fact]
    public void UpdateLSPFileInfo_UnknownFile_Noops()
    {
        // Arrange
        _provider.Updated += (sender, args) => throw new XunitException("Should not have been called.");

        // Act & Assert
        var documentContainer = new Mock<DynamicDocumentContainer>(MockBehavior.Strict);
        documentContainer.SetupSet(c => c.SupportsDiagnostics = true).Verifiable();
        _provider.UpdateLSPFileInfo(new Uri("C:/this/does/not/exist.razor"), documentContainer.Object);
    }

    [Fact]
    public async Task UpdateLSPFileInfo_Updates()
    {
        // Arrange
        await _testAccessor.GetDynamicFileInfoAsync(_project.FilePath, _document1.FilePath, DisposalToken);
        var called = false;
        _provider.Updated += (sender, args) => called = true;

        // Act
        _provider.UpdateLSPFileInfo(new Uri(_document1.FilePath), _lspDocumentContainer);

        // Assert
        Assert.True(called);
    }

    [Fact]
    public async Task UpdateLSPFileInfo_ProjectRemoved_Noops()
    {
        // Arrange
        await _testAccessor.GetDynamicFileInfoAsync(_project.FilePath, _document1.FilePath, DisposalToken);
        var called = false;
        _provider.Updated += (sender, args) => called = true;
        _projectSnapshotManager.ProjectRemoved(_project.HostProject);

        // Act
        _provider.UpdateLSPFileInfo(new Uri(_document1.FilePath), _lspDocumentContainer);

        // Assert
        Assert.False(called);
    }

    [Fact]
    public async Task UpdateLSPFileInfo_SolutionClosing_ClearsAllDocuments()
    {
        // Arrange
        await _testAccessor.GetDynamicFileInfoAsync(_project.FilePath, _document1.FilePath, DisposalToken);
        await _testAccessor.GetDynamicFileInfoAsync(_project.FilePath, _document2.FilePath, DisposalToken);
        _provider.Updated += (sender, documentFilePath) => throw new InvalidOperationException("Should not have been called!");

        _projectSnapshotManager.SolutionClosed();
        _projectSnapshotManager.DocumentClosed(_project.FilePath, _document1.FilePath, new EmptyTextLoader(string.Empty));

        // Act & Assert
        _provider.UpdateLSPFileInfo(new Uri(_document2.FilePath), _lspDocumentContainer);
        _provider.UpdateLSPFileInfo(new Uri(_document1.FilePath), _lspDocumentContainer);
    }
}
