// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class DefaultGeneratedDocumentPublisherTest : LanguageServerTestBase
{
    private readonly TestClient _serverClient;
    private readonly TestProjectSnapshotManager _projectManager;
    private readonly HostProject _hostProject;
    private readonly HostDocument _hostDocument;

    public DefaultGeneratedDocumentPublisherTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _serverClient = new TestClient();
        _projectManager = TestProjectSnapshotManager.Create(LegacyDispatcher);
        _projectManager.AllowNotifyListeners = true;
        _hostProject = new HostProject("/path/to/project.csproj", RazorConfiguration.Default, "TestRootNamespace");
        _projectManager.ProjectAdded(_hostProject);
        _hostDocument = new HostDocument("/path/to/file.razor", "file.razor");
        _projectManager.DocumentAdded(_hostProject, _hostDocument, new EmptyTextLoader(_hostDocument.FilePath));
    }

    [Fact]
    public void PublishCSharp_FirstTime_PublishesEntireSourceText()
    {
        // Arrange
        var generatedDocumentPublisher = new DefaultGeneratedDocumentPublisher(LegacyDispatcher, _serverClient, LoggerFactory);
        var content = "// C# content";
        var sourceText = SourceText.From(content);

        // Act
        generatedDocumentPublisher.PublishCSharp("/path/to/file.razor", sourceText, 123);

        // Assert
        var updateRequest = Assert.Single(_serverClient.UpdateRequests);
        Assert.Equal("/path/to/file.razor", updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal(content, textChange.NewText);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public void PublishHtml_FirstTime_PublishesEntireSourceText()
    {
        // Arrange
        var generatedDocumentPublisher = new DefaultGeneratedDocumentPublisher(LegacyDispatcher, _serverClient, LoggerFactory);
        var content = "HTML content";
        var sourceText = SourceText.From(content);

        // Act
        generatedDocumentPublisher.PublishHtml("/path/to/file.razor", sourceText, 123);

        // Assert
        var updateRequest = Assert.Single(_serverClient.UpdateRequests);
        Assert.Equal("/path/to/file.razor", updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal(content, textChange.NewText);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public void PublishCSharp_SecondTime_PublishesSourceTextDifferences()
    {
        // Arrange
        var generatedDocumentPublisher = new DefaultGeneratedDocumentPublisher(LegacyDispatcher, _serverClient, LoggerFactory);
        var initialSourceText = SourceText.From("// Initial content\n");
        generatedDocumentPublisher.PublishCSharp("/path/to/file.razor", initialSourceText, 123);
        var change = new TextChange(
            new TextSpan(initialSourceText.Length, 0),
            "// Another line");
        var changedSourceText = initialSourceText.WithChanges(change);

        // Act
        generatedDocumentPublisher.PublishCSharp("/path/to/file.razor", changedSourceText, 124);

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal("/path/to/file.razor", updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal(change, textChange);
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public void PublishHtml_SecondTime_PublishesSourceTextDifferences()
    {
        // Arrange
        var generatedDocumentPublisher = new DefaultGeneratedDocumentPublisher(LegacyDispatcher, _serverClient, LoggerFactory);
        var initialSourceText = SourceText.From("HTML content\n");
        generatedDocumentPublisher.PublishHtml("/path/to/file.razor", initialSourceText, 123);
        var change = new TextChange(
            new TextSpan(initialSourceText.Length, 0),
            "More content!!");
        var changedSourceText = initialSourceText.WithChanges(change);

        // Act
        generatedDocumentPublisher.PublishHtml("/path/to/file.razor", changedSourceText, 124);

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal("/path/to/file.razor", updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal(change, textChange);
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public void PublishCSharp_SecondTime_IdenticalContent_NoTextChanges()
    {
        // Arrange
        var generatedDocumentPublisher = new DefaultGeneratedDocumentPublisher(LegacyDispatcher, _serverClient, LoggerFactory);
        var sourceTextContent = "// The content";
        var initialSourceText = SourceText.From(sourceTextContent);
        generatedDocumentPublisher.PublishCSharp("/path/to/file.razor", initialSourceText, 123);
        var identicalSourceText = SourceText.From(sourceTextContent);

        // Act
        generatedDocumentPublisher.PublishCSharp("/path/to/file.razor", identicalSourceText, 124);

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal("/path/to/file.razor", updateRequest.HostDocumentFilePath);
        Assert.Empty(updateRequest.Changes);
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public void PublishHtml_SecondTime_IdenticalContent_NoTextChanges()
    {
        // Arrange
        var generatedDocumentPublisher = new DefaultGeneratedDocumentPublisher(LegacyDispatcher, _serverClient, LoggerFactory);
        var sourceTextContent = "HTMl content";
        var initialSourceText = SourceText.From(sourceTextContent);
        generatedDocumentPublisher.PublishHtml("/path/to/file.razor", initialSourceText, 123);
        var identicalSourceText = SourceText.From(sourceTextContent);

        // Act
        generatedDocumentPublisher.PublishHtml("/path/to/file.razor", identicalSourceText, 124);

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal("/path/to/file.razor", updateRequest.HostDocumentFilePath);
        Assert.Empty(updateRequest.Changes);
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public void PublishCSharp_DifferentFileSameContent_PublishesEverything()
    {
        // Arrange
        var generatedDocumentPublisher = new DefaultGeneratedDocumentPublisher(LegacyDispatcher, _serverClient, LoggerFactory);
        var sourceTextContent = "// The content";
        var initialSourceText = SourceText.From(sourceTextContent);
        generatedDocumentPublisher.PublishCSharp("/path/to/file1.razor", initialSourceText, 123);
        var identicalSourceText = SourceText.From(sourceTextContent);

        // Act
        generatedDocumentPublisher.PublishCSharp("/path/to/file2.razor", identicalSourceText, 123);

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal("/path/to/file2.razor", updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal(sourceTextContent, textChange.NewText);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public void PublishHtml_DifferentFileSameContent_PublishesEverything()
    {
        // Arrange
        var generatedDocumentPublisher = new DefaultGeneratedDocumentPublisher(LegacyDispatcher, _serverClient, LoggerFactory);
        var sourceTextContent = "HTML content";
        var initialSourceText = SourceText.From(sourceTextContent);
        generatedDocumentPublisher.PublishHtml("/path/to/file1.razor", initialSourceText, 123);
        var identicalSourceText = SourceText.From(sourceTextContent);

        // Act
        generatedDocumentPublisher.PublishHtml("/path/to/file2.razor", identicalSourceText, 123);

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal("/path/to/file2.razor", updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal(sourceTextContent, textChange.NewText);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public void ProjectSnapshotManager_DocumentChanged_OpenDocument_PublishesEmptyTextChanges_CSharp()
    {
        // Arrange
        var generatedDocumentPublisher = new DefaultGeneratedDocumentPublisher(LegacyDispatcher, _serverClient, LoggerFactory);
        generatedDocumentPublisher.Initialize(_projectManager);
        var sourceTextContent = "// The content";
        var initialSourceText = SourceText.From(sourceTextContent);
        generatedDocumentPublisher.PublishCSharp(_hostDocument.FilePath, initialSourceText, 123);

        // Act
        _projectManager.DocumentOpened(_hostProject.FilePath, _hostDocument.FilePath, initialSourceText);
        generatedDocumentPublisher.PublishCSharp(_hostDocument.FilePath, initialSourceText, 124);

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal(_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        Assert.Empty(updateRequest.Changes);
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public void ProjectSnapshotManager_DocumentChanged_OpenDocument_VersionEquivalent_Noops_CSharp()
    {
        // Arrange
        var generatedDocumentPublisher = new DefaultGeneratedDocumentPublisher(LegacyDispatcher, _serverClient, LoggerFactory);
        generatedDocumentPublisher.Initialize(_projectManager);
        var sourceTextContent = "// The content";
        var initialSourceText = SourceText.From(sourceTextContent);
        generatedDocumentPublisher.PublishCSharp(_hostDocument.FilePath, initialSourceText, 123);

        // Act
        _projectManager.DocumentOpened(_hostProject.FilePath, _hostDocument.FilePath, initialSourceText);
        generatedDocumentPublisher.PublishCSharp(_hostDocument.FilePath, initialSourceText, 123);

        // Assert
        var updateRequest = Assert.Single(_serverClient.UpdateRequests);
        Assert.Equal(_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public void ProjectSnapshotManager_DocumentChanged_OpenDocument_PublishesEmptyTextChanges_Html()
    {
        // Arrange
        var generatedDocumentPublisher = new DefaultGeneratedDocumentPublisher(LegacyDispatcher, _serverClient, LoggerFactory);
        generatedDocumentPublisher.Initialize(_projectManager);
        var sourceTextContent = "<!-- The content -->";
        var initialSourceText = SourceText.From(sourceTextContent);
        generatedDocumentPublisher.PublishHtml(_hostDocument.FilePath, initialSourceText, 123);

        // Act
        _projectManager.DocumentOpened(_hostProject.FilePath, _hostDocument.FilePath, initialSourceText);
        generatedDocumentPublisher.PublishHtml(_hostDocument.FilePath, initialSourceText, 124);

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal(_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        Assert.Empty(updateRequest.Changes);
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public void ProjectSnapshotManager_DocumentChanged_OpenDocument_VersionEquivalent_Noops_Html()
    {
        // Arrange
        var generatedDocumentPublisher = new DefaultGeneratedDocumentPublisher(LegacyDispatcher, _serverClient, LoggerFactory);
        generatedDocumentPublisher.Initialize(_projectManager);
        var sourceTextContent = "<!-- The content -->";
        var initialSourceText = SourceText.From(sourceTextContent);
        generatedDocumentPublisher.PublishHtml(_hostDocument.FilePath, initialSourceText, 123);

        // Act
        _projectManager.DocumentOpened(_hostProject.FilePath, _hostDocument.FilePath, initialSourceText);
        generatedDocumentPublisher.PublishHtml(_hostDocument.FilePath, initialSourceText, 123);

        // Assert
        var updateRequest = Assert.Single(_serverClient.UpdateRequests);
        Assert.Equal(_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public void ProjectSnapshotManager_DocumentChanged_ClosedDocument_RepublishesTextChanges()
    {
        // Arrange
        var generatedDocumentPublisher = new DefaultGeneratedDocumentPublisher(LegacyDispatcher, _serverClient, LoggerFactory);
        generatedDocumentPublisher.Initialize(_projectManager);
        var sourceTextContent = "// The content";
        var initialSourceText = SourceText.From(sourceTextContent);
        generatedDocumentPublisher.PublishCSharp(_hostDocument.FilePath, initialSourceText, 123);
        _projectManager.DocumentOpened(_hostProject.FilePath, _hostDocument.FilePath, initialSourceText);

        // Act
        _projectManager.DocumentClosed(_hostProject.FilePath, _hostDocument.FilePath, new EmptyTextLoader(_hostDocument.FilePath));
        generatedDocumentPublisher.PublishCSharp(_hostDocument.FilePath, initialSourceText, 123);

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal(_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal(sourceTextContent, textChange.NewText);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }
}
