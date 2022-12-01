// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Test;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Editor.Razor.Documents;

public class EditorDocumentManagerListenerTest : ProjectSnapshotManagerDispatcherTestBase
{
    private readonly string _projectFilePath;
    private readonly string _documentFilePath;
    private readonly TextLoader _textLoader;
    private readonly FileChangeTracker _fileChangeTracker;
    private readonly TestTextBuffer _textBuffer;

    public EditorDocumentManagerListenerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _projectFilePath = TestProjectData.SomeProject.FilePath;
        _documentFilePath = TestProjectData.SomeProjectFile1.FilePath;
        _textLoader = TextLoader.From(TextAndVersion.Create(SourceText.From("FILE"), VersionStamp.Default));
        _fileChangeTracker = new DefaultFileChangeTracker(_documentFilePath);

        _textBuffer = new TestTextBuffer(new StringTextSnapshot("Hello"));
    }

    [Fact]
    public void ProjectManager_Changed_DocumentAdded_InvokesGetOrCreateDocument()
    {
        // Arrange
        var changedOnDisk = new EventHandler((o, args) => { });
        var changedInEditor = new EventHandler((o, args) => { });
        var opened = new EventHandler((o, args) => { });
        var closed = new EventHandler((o, args) => { });

        var editorDocumentManger = new Mock<EditorDocumentManager>(MockBehavior.Strict);
        editorDocumentManger
            .Setup(e => e.GetOrCreateDocument(It.IsAny<DocumentKey>(), It.IsAny<EventHandler>(), It.IsAny<EventHandler>(), It.IsAny<EventHandler>(), It.IsAny<EventHandler>()))
            .Returns(GetEditorDocument())
            .Callback<DocumentKey, EventHandler, EventHandler, EventHandler, EventHandler>((key, onChangedOnDisk, onChangedInEditor, onOpened, onClosed) =>
            {
                Assert.Same(changedOnDisk, onChangedOnDisk);
                Assert.Same(changedInEditor, onChangedInEditor);
                Assert.Same(opened, onOpened);
                Assert.Same(closed, onClosed);
            });

        var listener = new EditorDocumentManagerListener(
            Dispatcher, JoinableTaskFactory.Context, editorDocumentManger.Object, changedOnDisk, changedInEditor, opened, closed);

        var project = Mock.Of<ProjectSnapshot>(p => p.FilePath == "/Path/to/project.csproj", MockBehavior.Strict);

        // Act & Assert
        listener.ProjectManager_Changed(null, new ProjectChangeEventArgs(project, project, ProjectChangeKind.DocumentAdded));
    }

    [Fact]
    public void ProjectManager_Changed_OpenDocumentAdded_InvokesOnOpened()
    {
        // Arrange
        var called = false;
        var opened = new EventHandler((o, args) => called = true);

        var editorDocumentManger = new Mock<EditorDocumentManager>(MockBehavior.Strict);
        editorDocumentManger
            .Setup(e => e.GetOrCreateDocument(It.IsAny<DocumentKey>(), It.IsAny<EventHandler>(), It.IsAny<EventHandler>(), It.IsAny<EventHandler>(), It.IsAny<EventHandler>()))
            .Returns(GetEditorDocument(isOpen: true));

        var listener = new EditorDocumentManagerListener(
            Dispatcher, JoinableTaskFactory.Context, editorDocumentManger.Object, onChangedOnDisk: null, onChangedInEditor: null, onOpened: opened, onClosed: null);

        var project = Mock.Of<ProjectSnapshot>(p => p.FilePath == "/Path/to/project.csproj", MockBehavior.Strict);

        // Act
        listener.ProjectManager_Changed(null, new ProjectChangeEventArgs(project, project, ProjectChangeKind.DocumentAdded));

        // Assert
        Assert.True(called);
    }

    private EditorDocument GetEditorDocument(bool isOpen = false)
    {
        var document = new EditorDocument(
            Mock.Of<EditorDocumentManager>(MockBehavior.Strict),
            Dispatcher,
            JoinableTaskFactory.Context,
            _projectFilePath,
            _documentFilePath,
            _textLoader,
            _fileChangeTracker,
            isOpen ? _textBuffer : null,
            changedOnDisk: null,
            changedInEditor: null,
            opened: null,
            closed: null);

        return document;
    }
}
