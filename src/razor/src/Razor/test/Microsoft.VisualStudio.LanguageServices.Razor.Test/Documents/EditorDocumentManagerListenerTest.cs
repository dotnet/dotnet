// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Threading;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.Documents;

public class EditorDocumentManagerListenerTest(ITestOutputHelper testOutput) : VisualStudioTestBase(testOutput)
{
    private static readonly HostProject s_hostProject = new(
        filePath: "/path/to/project.csproj",
        intermediateOutputPath: "/path/to/obj",
        RazorConfiguration.Default,
        rootNamespace: null);

    private static readonly HostDocument s_hostDocument = new(
        filePath: "/path/to/file1.razor",
        targetPath: "/path/to/file1.razor");

    [UIFact]
    public async Task ProjectManager_Changed_DocumentRemoved_RemovesDocument()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(s_hostProject);
            updater.DocumentAdded(s_hostProject.Key, s_hostDocument, StrictMock.Of<TextLoader>());
        });

        var editorDocumentMangerMock = new StrictMock<IEditorDocumentManager>();
        var expectedDocument = GetEditorDocument(documentManager: editorDocumentMangerMock.Object);
        editorDocumentMangerMock
            .Setup(e => e.TryGetDocument(It.IsAny<DocumentKey>(), out expectedDocument))
            .Returns(true);
        editorDocumentMangerMock
            .Setup(e => e.RemoveDocument(It.IsAny<EditorDocument>()))
            .Callback<EditorDocument>(document => Assert.Same(expectedDocument, document))
            .Verifiable();

        var listener = new EditorDocumentManagerListener(
            editorDocumentMangerMock.Object, projectManager, JoinableTaskContext, NoOpTelemetryReporter.Instance);

        var listenerAccessor = listener.GetTestAccessor();

        // Act
        await projectManager.UpdateAsync(updater =>
        {
            updater.DocumentRemoved(s_hostProject.Key, s_hostDocument);
        });

        await listenerAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        editorDocumentMangerMock.VerifyAll();
    }

    [UIFact]
    public async Task ProjectManager_Changed_ProjectRemoved_RemovesAllDocuments()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(s_hostProject);
            updater.DocumentAdded(s_hostProject.Key, s_hostDocument, StrictMock.Of<TextLoader>());
        });

        var editorDocumentMangerMock = new StrictMock<IEditorDocumentManager>();
        var expectedDocument = GetEditorDocument(documentManager: editorDocumentMangerMock.Object);
        editorDocumentMangerMock
            .Setup(e => e.TryGetDocument(It.IsAny<DocumentKey>(), out expectedDocument))
            .Returns(true);
        editorDocumentMangerMock
            .Setup(e => e.RemoveDocument(It.IsAny<EditorDocument>()))
            .Callback<EditorDocument>(document => Assert.Same(expectedDocument, document))
            .Verifiable();

        var listener = new EditorDocumentManagerListener(
            editorDocumentMangerMock.Object, projectManager, JoinableTaskContext, NoOpTelemetryReporter.Instance);

        var listenerAccessor = listener.GetTestAccessor();

        // Act
        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectRemoved(s_hostProject.Key);
        });

        await listenerAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        editorDocumentMangerMock.VerifyAll();
    }

    [UIFact]
    public async Task ProjectManager_Changed_DocumentAdded_InvokesGetOrCreateDocument()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(s_hostProject);
        });

        var editorDocumentMangerMock = new StrictMock<IEditorDocumentManager>();
        editorDocumentMangerMock
            .Setup(e => e.GetOrCreateDocument(It.IsAny<DocumentKey>(), It.IsAny<string>(), It.IsAny<ProjectKey>(), It.IsAny<EventHandler>(), It.IsAny<EventHandler>(), It.IsAny<EventHandler>(), It.IsAny<EventHandler>()))
            .Returns(GetEditorDocument())
            .Callback<DocumentKey, string, ProjectKey, EventHandler, EventHandler, EventHandler, EventHandler>((key, filePath, projectKey, _, _, _, _) =>
            {
                Assert.Equal(s_hostDocument.FilePath, key.DocumentFilePath);
                Assert.Equal(s_hostProject.FilePath, filePath);
                Assert.Equal(s_hostProject.Key, projectKey);
            })
            .Verifiable();

        var listener = new EditorDocumentManagerListener(
            editorDocumentMangerMock.Object, projectManager, JoinableTaskContext, NoOpTelemetryReporter.Instance);

        var listenerAccessor = listener.GetTestAccessor();

        // Act
        await projectManager.UpdateAsync(updater =>
        {
            updater.DocumentAdded(s_hostProject.Key, s_hostDocument, StrictMock.Of<TextLoader>());
        });

        await listenerAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        editorDocumentMangerMock.VerifyAll();
    }

    [UIFact]
    public async Task ProjectManager_Changed_OpenDocumentAdded_InvokesOnOpened()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(s_hostProject);
        });

        var editorDocumentMangerMock = new StrictMock<IEditorDocumentManager>();
        editorDocumentMangerMock
            .Setup(e => e.GetOrCreateDocument(It.IsAny<DocumentKey>(), It.IsAny<string>(), It.IsAny<ProjectKey>(), It.IsAny<EventHandler>(), It.IsAny<EventHandler>(), It.IsAny<EventHandler>(), It.IsAny<EventHandler>()))
            .Returns(GetEditorDocument(isOpen: true));

        var listener = new EditorDocumentManagerListener(
            editorDocumentMangerMock.Object, projectManager, JoinableTaskContext, NoOpTelemetryReporter.Instance);

        var listenerAccessor = listener.GetTestAccessor();

        var called = false;
        listenerAccessor.OnOpened += delegate { called = true; };

        var projectFilePath = "/Path/to/project.csproj";
        var project = StrictMock.Of<IProjectSnapshot>(p =>
            p.Key == TestProjectKey.Create("/Path/to/obj") &&
            p.FilePath == projectFilePath);

        // Act
        await projectManager.UpdateAsync(updater =>
        {
            updater.DocumentAdded(s_hostProject.Key, s_hostDocument, StrictMock.Of<TextLoader>());
        });

        await listenerAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        Assert.True(called);
    }

    private EditorDocument GetEditorDocument(bool isOpen = false, IEditorDocumentManager? documentManager = null)
    {
        var fileChangeTrackerMock = new StrictMock<IFileChangeTracker>();
        fileChangeTrackerMock
            .SetupGet(x => x.FilePath)
            .Returns(s_hostDocument.FilePath);
        fileChangeTrackerMock
            .Setup(x => x.StartListening());
        fileChangeTrackerMock
            .Setup(x => x.StopListening());

        var textBuffer = isOpen
            ? new TestTextBuffer(new StringTextSnapshot("Hello"))
            : null;

        return new EditorDocument(
            documentManager ?? StrictMock.Of<IEditorDocumentManager>(),
            JoinableTaskContext,
            s_hostProject.FilePath,
            s_hostDocument.FilePath,
            s_hostProject.Key,
            StrictMock.Of<TextLoader>(),
            fileChangeTrackerMock.Object,
            textBuffer,
            changedOnDisk: null,
            changedInEditor: null,
            opened: null,
            closed: null);
    }
}
