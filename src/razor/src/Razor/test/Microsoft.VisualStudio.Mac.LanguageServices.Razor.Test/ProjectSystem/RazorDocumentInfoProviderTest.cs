// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Editor.Razor;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class RazorDocumentInfoProviderTest : WorkspaceTestBase
{
    private readonly VisualStudioMacDocumentInfoFactory _factory;
    private readonly DefaultRazorDynamicFileInfoProvider _innerDynamicDocumentInfoProvider;
    private readonly TestProjectSnapshotManager _projectSnapshotManager;
    private readonly IProjectSnapshot _projectSnapshot;
    private readonly ProjectId _projectId;
    private readonly IDocumentSnapshot _documentSnapshot;

    public RazorDocumentInfoProviderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _projectSnapshotManager = new TestProjectSnapshotManager(Workspace);

        var serviceProviderFactory = new DefaultRazorDocumentServiceProviderFactory();
        var lspEditorEnabledFeatureDetector = Mock.Of<LSPEditorFeatureDetector>(detector => detector.IsLSPEditorAvailable() == true, MockBehavior.Strict);
        var projectSnapshotManagerAccessor = Mock.Of<ProjectSnapshotManagerAccessor>(a => a.Instance == _projectSnapshotManager, MockBehavior.Strict);

        var filePathService = new FilePathService(TestLanguageServerFeatureOptions.Instance);
        _innerDynamicDocumentInfoProvider = new DefaultRazorDynamicFileInfoProvider(serviceProviderFactory, lspEditorEnabledFeatureDetector, filePathService, projectSnapshotManagerAccessor);

        var hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, "RootNamespace");
        _projectSnapshotManager.ProjectAdded(hostProject);

        var hostDocument = new HostDocument("C:/path/to/document.cshtml", "/C:/path/to/document.cshtml");
        var sourceText = SourceText.From("Hello World");
        var textAndVersion = TextAndVersion.Create(sourceText, VersionStamp.Default, hostDocument.FilePath);
        _projectSnapshotManager.DocumentAdded(hostProject.Key, hostDocument, TextLoader.From(textAndVersion));

        _projectSnapshot = _projectSnapshotManager.GetProjects()[0];
        _documentSnapshot = _projectSnapshot.GetDocument(hostDocument.FilePath);

        var factory = new Mock<VisualStudioMacDocumentInfoFactory>(MockBehavior.Strict);
        factory.Setup(f => f.CreateEmpty(It.IsAny<string>(), It.IsAny<ProjectId>(), It.IsAny<ProjectKey>()))
            .Returns<string, ProjectId, ProjectKey>((razorFilePath, projectId, projectKey) =>
            {
                var documentId = DocumentId.CreateNewId(projectId);
                var documentInfo = DocumentInfo.Create(documentId, "testDoc", filePath: razorFilePath);
                return documentInfo;
            });

        _factory = factory.Object;

        _projectId = ProjectId.CreateNewId();
        Workspace.TryApplyChanges(Workspace.CurrentSolution.AddProject(ProjectInfo.Create(
           _projectId,
           new VersionStamp(),
            "Project",
            "Assembly",
            LanguageNames.CSharp,
            filePath: _projectSnapshot.FilePath).WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath(Path.Combine(_projectSnapshot.IntermediateOutputPath, "project.dll")))));
    }

    [Fact]
    public void DelegatedUpdateFileInfo_UnknownDocument_Noops()
    {
        // Arrange
        var provider = new RazorDynamicDocumentInfoProvider(_factory, _innerDynamicDocumentInfoProvider);
        provider.Initialize(_projectSnapshotManager);
        provider.Updated += (_) => throw new XunitException("This should not have been called.");
        var documentContainer = new DefaultDynamicDocumentContainer(_documentSnapshot);

        // Act & Assert
        _innerDynamicDocumentInfoProvider.UpdateFileInfo(_projectSnapshot.Key, documentContainer);
    }

    [Fact]
    public void DelegatedUpdateFileInfo_KnownDocument_TriggersUpdate()
    {
        // Arrange
        var provider = new RazorDynamicDocumentInfoProvider(_factory, _innerDynamicDocumentInfoProvider);
        provider.Initialize(_projectSnapshotManager);
        DocumentInfo documentInfo = null;
        provider.Updated += (info) => documentInfo = info;

        // Populate the providers understanding of our project/document
        provider.GetDynamicDocumentInfo(_projectId, _projectSnapshot.FilePath, _documentSnapshot.FilePath);
        var documentContainer = new DefaultDynamicDocumentContainer(_documentSnapshot);

        // Act
        _innerDynamicDocumentInfoProvider.UpdateFileInfo(_projectSnapshot.Key, documentContainer);

        // Assert
        Assert.NotNull(documentInfo);
        Assert.Equal(_documentSnapshot.FilePath, documentInfo.FilePath);
    }

    [Fact]
    public void DelegatedSuppressDocument_UnknownDocument_Noops()
    {
        // Arrange
        var provider = new RazorDynamicDocumentInfoProvider(_factory, _innerDynamicDocumentInfoProvider);
        provider.Initialize(_projectSnapshotManager);
        provider.Updated += (_) => throw new XunitException("This should not have been called.");
        var documentContainer = new DefaultDynamicDocumentContainer(_documentSnapshot);

        // Act & Assert
        _innerDynamicDocumentInfoProvider.SuppressDocument(_projectSnapshot.Key, _documentSnapshot.FilePath);
    }

    [Fact]
    public void DelegatedSuppressDocument_KnownDocument_NotUpdated_Noops()
    {
        // Arrange
        var provider = new RazorDynamicDocumentInfoProvider(_factory, _innerDynamicDocumentInfoProvider);
        provider.Initialize(_projectSnapshotManager);
        provider.Updated += (_) => throw new XunitException("This should not have been called.");

        // Populate the providers understanding of our project/document
        provider.GetDynamicDocumentInfo(_projectId, _projectSnapshot.FilePath, _documentSnapshot.FilePath);
        var documentContainer = new DefaultDynamicDocumentContainer(_documentSnapshot);

        // Act & Assert
        _innerDynamicDocumentInfoProvider.SuppressDocument(_projectSnapshot.Key, _documentSnapshot.FilePath);
    }

    [Fact]
    public void DelegatedSuppressDocument_KnownAndUpdatedDocument_TriggersUpdate()
    {
        // Arrange
        var provider = new RazorDynamicDocumentInfoProvider(_factory, _innerDynamicDocumentInfoProvider);
        provider.Initialize(_projectSnapshotManager);
        DocumentInfo documentInfo = null;
        provider.Updated += (info) => documentInfo = info;

        // Populate the providers understanding of our project/document
        provider.GetDynamicDocumentInfo(_projectId, _projectSnapshot.FilePath, _documentSnapshot.FilePath);
        var documentContainer = new DefaultDynamicDocumentContainer(_documentSnapshot);

        // Update the document with content
        _innerDynamicDocumentInfoProvider.UpdateFileInfo(_projectSnapshot.Key, documentContainer);

        // Act
        _innerDynamicDocumentInfoProvider.SuppressDocument(_projectSnapshot.Key, _documentSnapshot.FilePath);

        // Assert
        Assert.NotNull(documentInfo);
        Assert.Equal(_documentSnapshot.FilePath, documentInfo.FilePath);
    }

    [Fact]
    public void DelegatedRemoveDynamicDocumentInfo_UntracksDocument()
    {
        // Arrange
        var provider = new RazorDynamicDocumentInfoProvider(_factory, _innerDynamicDocumentInfoProvider);
        provider.Initialize(_projectSnapshotManager);

        // Populate the providers understanding of our project/document
        provider.GetDynamicDocumentInfo(_projectId, _projectSnapshot.FilePath, _documentSnapshot.FilePath);
        var documentContainer = new DefaultDynamicDocumentContainer(_documentSnapshot);

        // Update the document with content
        _innerDynamicDocumentInfoProvider.UpdateFileInfo(_projectSnapshot.Key, documentContainer);

        // Now explode if any further updates happen
        provider.Updated += (_) => throw new XunitException("This should not have been called.");

        // Act
        provider.RemoveDynamicDocumentInfo(_projectId, _projectSnapshot.FilePath, _documentSnapshot.FilePath);

        // Assert this should not update
        _innerDynamicDocumentInfoProvider.UpdateFileInfo(_projectSnapshot.Key, documentContainer);
    }
}
