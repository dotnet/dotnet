// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Razor.DynamicFiles;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using static Microsoft.VisualStudio.Razor.DynamicFiles.RazorDynamicFileInfoProvider;

namespace Microsoft.VisualStudio.LanguageServices.Razor.ProjectSystem;

public class RazorDynamicFileInfoProviderTest(ITestOutputHelper testOutput) : VisualStudioWorkspaceTestBase(testOutput)
{
    private readonly ProjectId _projectId = ProjectId.CreateNewId();

    // These fields are initialized by InitializeAsync().
#nullable disable
    private RazorDynamicFileInfoProvider _provider;
    private TestAccessor _testAccessor;
    private TestProjectSnapshotManager _projectManager;
    private IProjectSnapshot _project;
    private IDocumentSnapshot _document1;
    private IDocumentSnapshot _document2;
    private IDynamicDocumentContainer _lspDocumentContainer;
#nullable enable

    protected override async Task InitializeAsync()
    {
        var documentServiceFactory = new RazorDocumentServiceProviderFactory();
        var editorFeatureDetector = StrictMock.Of<LSPEditorFeatureDetector>();

        _projectManager = CreateProjectSnapshotManager();

        var hostProject = new HostProject(@"C:\project.csproj", @"C:\obj", RazorConfiguration.Default, rootNamespace: "TestNamespace");
        var hostDocument1 = new HostDocument(@"C:\document1.razor", "document1.razor", FileKinds.Component);
        var hostDocument2 = new HostDocument(@"C:\document2.razor", "document2.razor", FileKinds.Component);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
            updater.DocumentAdded(hostProject.Key, hostDocument1, new EmptyTextLoader(hostDocument1.FilePath));
            updater.DocumentAdded(hostProject.Key, hostDocument2, new EmptyTextLoader(hostDocument2.FilePath));
        });

        var projectKey = _projectManager.GetAllProjectKeys(hostProject.FilePath).Single();
        _project = _projectManager.GetLoadedProject(projectKey);
        _document1 = _project.GetDocument(hostDocument1.FilePath).AssumeNotNull();
        _document2 = _project.GetDocument(hostDocument2.FilePath).AssumeNotNull();

        var languageServerFeatureOptions = new TestLanguageServerFeatureOptions(includeProjectKeyInGeneratedFilePath: true);
        var filePathService = new VisualStudioFilePathService(languageServerFeatureOptions);

        var serviceProvider = VsMocks.CreateServiceProvider(static b =>
            b.AddComponentModel(static b =>
            {
                var startupInitializer = new RazorStartupInitializer([]);
                b.AddExport(startupInitializer);
            }));

        var fallbackProjectManager = new FallbackProjectManager(
            serviceProvider,
            StrictMock.Of<ProjectConfigurationFilePathStore>(),
            languageServerFeatureOptions,
            _projectManager,
            WorkspaceProvider,
            NoOpTelemetryReporter.Instance);

        _provider = new RazorDynamicFileInfoProvider(
            documentServiceFactory, editorFeatureDetector, filePathService, WorkspaceProvider, _projectManager, fallbackProjectManager);
        _testAccessor = _provider.GetTestAccessor();

        var lspDocumentContainerMock = new StrictMock<IDynamicDocumentContainer>();
        lspDocumentContainerMock
            .Setup(c => c.SetSupportsDiagnostics(true))
            .Verifiable();
        lspDocumentContainerMock
            .Setup(container => container.GetTextLoader(It.IsAny<string>()))
            .Returns(new EmptyTextLoader(string.Empty));
        _lspDocumentContainer = lspDocumentContainerMock.Object;

        var projectInfo = ProjectInfo.Create(
            _projectId, version: default, "Project", "Assembly", LanguageNames.CSharp, filePath: _project.FilePath)
            .WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath(Path.Combine(_project.IntermediateOutputPath, "project.dll")));
        var newSolution = Workspace.CurrentSolution.AddProject(projectInfo);
        Workspace.TryApplyChanges(newSolution);
    }

    [Fact]
    public void UpdateLSPFileInfo_UnknownFile_Noops()
    {
        // Arrange
        _provider.Updated += (sender, args) => throw new XunitException("Should not have been called.");

        // Act & Assert
        var documentContainer = new Mock<IDynamicDocumentContainer>(MockBehavior.Strict);
        documentContainer.Setup(c => c.SetSupportsDiagnostics(true)).Verifiable();
        _provider.UpdateLSPFileInfo(new Uri("C:/this/does/not/exist.razor"), documentContainer.Object);
    }

    [Fact]
    public async Task GetDynamicFileInfoAsync_IncludesProjectToken()
    {
        // Arrange
        var info = await _testAccessor.GetDynamicFileInfoAsync(_projectId, _document1.FilePath.AssumeNotNull(), DisposalToken);
        Assert.NotNull(info);

        Assert.Equal(@"C:\document1.razor.fJcYlbdqjCXiWYY1.ide.g.cs", info.FilePath);
    }

    [Fact]
    public async Task UpdateLSPFileInfo_Updates()
    {
        // Arrange
        await _testAccessor.GetDynamicFileInfoAsync(_projectId, _document1.FilePath.AssumeNotNull(), DisposalToken);
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
        await _testAccessor.GetDynamicFileInfoAsync(_projectId, _document1.FilePath.AssumeNotNull(), DisposalToken);
        var called = false;
        _provider.Updated += (sender, args) => called = true;

        await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectRemoved(_project.Key);
        });

        // Act
        _provider.UpdateLSPFileInfo(new Uri(_document1.FilePath), _lspDocumentContainer);

        // Assert
        Assert.False(called);
    }

    [Fact]
    public async Task UpdateLSPFileInfo_SolutionClosing_ClearsAllDocuments()
    {
        // Arrange
        await _testAccessor.GetDynamicFileInfoAsync(_projectId, _document1.FilePath.AssumeNotNull(), DisposalToken);
        await _testAccessor.GetDynamicFileInfoAsync(_projectId, _document2.FilePath.AssumeNotNull(), DisposalToken);
        _provider.Updated += (sender, documentFilePath) => throw new InvalidOperationException("Should not have been called!");

        await _projectManager.UpdateAsync(updater =>
        {
            updater.SolutionClosed();
            updater.DocumentClosed(_project.Key, _document1.FilePath, new EmptyTextLoader(string.Empty));
        });

        // Act & Assert
        _provider.UpdateLSPFileInfo(new Uri(_document2.FilePath), _lspDocumentContainer);
        _provider.UpdateLSPFileInfo(new Uri(_document1.FilePath), _lspDocumentContainer);
    }
}
