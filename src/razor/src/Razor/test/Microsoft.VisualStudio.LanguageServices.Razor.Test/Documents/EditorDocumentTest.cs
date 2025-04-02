// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.Documents;

public class EditorDocumentTest : VisualStudioTestBase
{
    private readonly IEditorDocumentManager _documentManager;
    private readonly string _projectFilePath;
    private readonly ProjectKey _projectKey;
    private readonly string _documentFilePath;
    private readonly TextLoader _textLoader;
    private readonly IFileChangeTracker _fileChangeTracker;
    private readonly TestTextBuffer _textBuffer;

    public EditorDocumentTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _documentManager = StrictMock.Of<IEditorDocumentManager>();
        Mock.Get(_documentManager)
            .Setup(m => m.RemoveDocument(It.IsAny<EditorDocument>()))
            .Verifiable();

        _projectFilePath = TestProjectData.SomeProject.FilePath;
        _projectKey = TestProjectData.SomeProject.Key;
        _documentFilePath = TestProjectData.SomeProjectFile1.FilePath;
        _textLoader = TextLoader.From(TextAndVersion.Create(SourceText.From("FILE"), VersionStamp.Default));

        var mock = new StrictMock<IFileChangeTracker>();
        mock.SetupGet(x => x.FilePath)
            .Returns(_documentFilePath);
        mock.Setup(x => x.StartListening());
        mock.Setup(x => x.StopListening());

        _fileChangeTracker = mock.Object;

        _textBuffer = new TestTextBuffer(new StringTextSnapshot("Hello"));
    }

    [Fact]
    public void EditorDocument_CreatedWhileOpened()
    {
        // Arrange & Act
        using var document = new EditorDocument(
            _documentManager,
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
            closed: null);

        // Assert
        Assert.True(document.IsOpenInEditor);
        Assert.Same(_textBuffer, document.EditorTextBuffer);
        Assert.NotNull(document.EditorTextContainer);
    }

    [Fact]
    public void EditorDocument_CreatedWhileClosed()
    {
        // Arrange & Act
        using var document = new EditorDocument(
            _documentManager,
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
            closed: null);

        // Assert
        Assert.False(document.IsOpenInEditor);
        Assert.Null(document.EditorTextBuffer);
        Assert.Null(document.EditorTextContainer);
    }
}
