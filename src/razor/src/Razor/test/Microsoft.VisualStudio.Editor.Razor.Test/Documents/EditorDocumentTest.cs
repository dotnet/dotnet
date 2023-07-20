// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Test;
using Microsoft.VisualStudio.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Editor.Razor.Documents;

public class EditorDocumentTest : ProjectSnapshotManagerDispatcherTestBase
{
    private readonly EditorDocumentManager _documentManager;
    private readonly string _projectFilePath;
    private readonly ProjectKey _projectKey;
    private readonly string _documentFilePath;
    private readonly TextLoader _textLoader;
    private readonly FileChangeTracker _fileChangeTracker;
    private readonly TestTextBuffer _textBuffer;

    public EditorDocumentTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _documentManager = new Mock<EditorDocumentManager>(MockBehavior.Strict).Object;
        Mock.Get(_documentManager).Setup(m => m.RemoveDocument(It.IsAny<EditorDocument>())).Verifiable();
        _projectFilePath = TestProjectData.SomeProject.FilePath;
        _projectKey = TestProjectData.SomeProject.Key;
        _documentFilePath = TestProjectData.SomeProjectFile1.FilePath;
        _textLoader = TextLoader.From(TextAndVersion.Create(SourceText.From("FILE"), VersionStamp.Default));
        _fileChangeTracker = new DefaultFileChangeTracker(_documentFilePath);

        _textBuffer = new TestTextBuffer(new StringTextSnapshot("Hello"));
    }

    [Fact]
    public void EditorDocument_CreatedWhileOpened()
    {
        // Arrange & Act
        using (var document = new EditorDocument(
            _documentManager,
            Dispatcher,
            JoinableTaskFactory.Context,
            _projectFilePath,
            _documentFilePath,
            _projectKey,
            _textLoader,
            _fileChangeTracker,
            _textBuffer,
            changedOnDisk: null,
            changedInEditor: null,
            opened: null,
            closed: null))
        {
            // Assert
            Assert.True(document.IsOpenInEditor);
            Assert.Same(_textBuffer, document.EditorTextBuffer);
            Assert.NotNull(document.EditorTextContainer);
        }
    }

    [Fact]
    public void EditorDocument_CreatedWhileClosed()
    {
        // Arrange & Act
        using (var document = new EditorDocument(
            _documentManager,
            Dispatcher,
            JoinableTaskFactory.Context,
            _projectFilePath,
            _documentFilePath,
            _projectKey,
            _textLoader,
            _fileChangeTracker,
            null,
            changedOnDisk: null,
            changedInEditor: null,
            opened: null,
            closed: null))
        {
            // Assert
            Assert.False(document.IsOpenInEditor);
            Assert.Null(document.EditorTextBuffer);
            Assert.Null(document.EditorTextContainer);
        }
    }
}
