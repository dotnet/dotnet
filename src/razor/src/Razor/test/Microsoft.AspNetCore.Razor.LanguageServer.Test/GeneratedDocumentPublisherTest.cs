// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class GeneratedDocumentPublisherTest : LanguageServerTestBase
{
    private readonly TestClient _serverClient;
    private readonly TestProjectSnapshotManager _projectManager;
    private readonly HostProject _hostProject;
    private readonly HostProject _hostProject2;
    private readonly HostDocument _hostDocument;

    public GeneratedDocumentPublisherTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _serverClient = new TestClient();
        _projectManager = TestProjectSnapshotManager.Create(ErrorReporter, Dispatcher);
        _projectManager.AllowNotifyListeners = true;
        _hostProject = new HostProject("/path/to/project.csproj", "/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
        _hostProject2 = new HostProject("/path/to/project2.csproj", "/path/to/obj2", RazorConfiguration.Default, "TestRootNamespace");
        _projectManager.ProjectAdded(_hostProject);
        _hostDocument = new HostDocument("/path/to/file.razor", "file.razor");
        _projectManager.DocumentAdded(_hostProject.Key, _hostDocument, new EmptyTextLoader(_hostDocument.FilePath));
    }

    [Fact]
    public async Task PublishCSharp_FirstTime_PublishesEntireSourceText()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var content = "// C# content";
        var sourceText = SourceText.From(content);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishCSharp(_hostProject.Key, "/path/to/file.razor", sourceText, 123));

        // Assert
        var updateRequest = Assert.Single(_serverClient.UpdateRequests);
        Assert.Equal("/path/to/file.razor", updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal(content, textChange.NewText);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task PublishHtml_FirstTime_PublishesEntireSourceText()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var content = "HTML content";
        var sourceText = SourceText.From(content);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishHtml(_hostProject.Key, "/path/to/file.razor", sourceText, 123));

        // Assert
        var updateRequest = Assert.Single(_serverClient.UpdateRequests);
        Assert.Equal("/path/to/file.razor", updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal(content, textChange.NewText);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task PublishCSharp_SecondTime_PublishesSourceTextDifferences()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var initialSourceText = SourceText.From("// Initial content\n");
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishCSharp(_hostProject.Key, "/path/to/file.razor", initialSourceText, 123));
        var change = new TextChange(
            new TextSpan(initialSourceText.Length, 0),
            "// Another line");
        var changedSourceText = initialSourceText.WithChanges(change);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishCSharp(_hostProject.Key, "/path/to/file.razor", changedSourceText, 124));

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal("/path/to/file.razor", updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal(change, textChange);
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task PublishHtml_SecondTime_PublishesSourceTextDifferences()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var initialSourceText = SourceText.From("HTML content\n");
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishHtml(_hostProject.Key, "/path/to/file.razor", initialSourceText, 123));
        var change = new TextChange(
            new TextSpan(initialSourceText.Length, 0),
            "More content!!");
        var changedSourceText = initialSourceText.WithChanges(change);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishHtml(_hostProject.Key, "/path/to/file.razor", changedSourceText, 124));

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal("/path/to/file.razor", updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal(change, textChange);
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task PublishCSharp_SecondTime_IdenticalContent_NoTextChanges()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var sourceTextContent = "// The content";
        var initialSourceText = SourceText.From(sourceTextContent);
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishCSharp(_hostProject.Key, "/path/to/file.razor", initialSourceText, 123));
        var identicalSourceText = SourceText.From(sourceTextContent);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishCSharp(_hostProject.Key, "/path/to/file.razor", identicalSourceText, 124));

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal("/path/to/file.razor", updateRequest.HostDocumentFilePath);
        Assert.Empty(updateRequest.Changes);
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task PublishHtml_SecondTime_IdenticalContent_NoTextChanges()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var sourceTextContent = "HTMl content";
        var initialSourceText = SourceText.From(sourceTextContent);
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishHtml(_hostProject.Key, "/path/to/file.razor", initialSourceText, 123));
        var identicalSourceText = SourceText.From(sourceTextContent);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishHtml(_hostProject.Key, "/path/to/file.razor", identicalSourceText, 124));

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal("/path/to/file.razor", updateRequest.HostDocumentFilePath);
        Assert.Empty(updateRequest.Changes);
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task PublishCSharp_DifferentFileSameContent_PublishesEverything()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var sourceTextContent = "// The content";
        var initialSourceText = SourceText.From(sourceTextContent);
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishCSharp(_hostProject.Key, "/path/to/file1.razor", initialSourceText, 123));
        var identicalSourceText = SourceText.From(sourceTextContent);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishCSharp(_hostProject.Key, "/path/to/file2.razor", identicalSourceText, 123));

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal("/path/to/file2.razor", updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal(sourceTextContent, textChange.NewText);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task PublishHtml_DifferentFileSameContent_PublishesEverything()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var sourceTextContent = "HTML content";
        var initialSourceText = SourceText.From(sourceTextContent);
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishHtml(_hostProject.Key, "/path/to/file1.razor", initialSourceText, 123));
        var identicalSourceText = SourceText.From(sourceTextContent);

        // Act
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishHtml(_hostProject.Key, "/path/to/file2.razor", identicalSourceText, 123));

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal("/path/to/file2.razor", updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal(sourceTextContent, textChange.NewText);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task ProjectSnapshotManager_DocumentChanged_OpenDocument_PublishesEmptyTextChanges_CSharp()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        publisher.Initialize(_projectManager);
        var sourceTextContent = "// The content";
        var initialSourceText = SourceText.From(sourceTextContent);
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishCSharp(_hostProject.Key, _hostDocument.FilePath, initialSourceText, 123));

        // Act
        _projectManager.DocumentOpened(_hostProject.Key, _hostDocument.FilePath, initialSourceText);
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishCSharp(_hostProject.Key, _hostDocument.FilePath, initialSourceText, 124));

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal(_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        Assert.Empty(updateRequest.Changes);
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task ProjectSnapshotManager_DocumentChanged_OpenDocument_VersionEquivalent_Noops_CSharp()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        publisher.Initialize(_projectManager);
        var sourceTextContent = "// The content";
        var initialSourceText = SourceText.From(sourceTextContent);
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishCSharp(_hostProject.Key, _hostDocument.FilePath, initialSourceText, 123));

        // Act
        _projectManager.DocumentOpened(_hostProject.Key, _hostDocument.FilePath, initialSourceText);
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishCSharp(_hostProject.Key, _hostDocument.FilePath, initialSourceText, 123));

        // Assert
        var updateRequest = Assert.Single(_serverClient.UpdateRequests);
        Assert.Equal(_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task ProjectSnapshotManager_DocumentChanged_OpenDocument_PublishesEmptyTextChanges_Html()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        publisher.Initialize(_projectManager);
        var sourceTextContent = "<!-- The content -->";
        var initialSourceText = SourceText.From(sourceTextContent);
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishHtml(_hostProject.Key, _hostDocument.FilePath, initialSourceText, 123));

        // Act
        _projectManager.DocumentOpened(_hostProject.Key, _hostDocument.FilePath, initialSourceText);
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishHtml(_hostProject.Key, _hostDocument.FilePath, initialSourceText, 124));

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal(_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        Assert.Empty(updateRequest.Changes);
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task ProjectSnapshotManager_DocumentChanged_OpenDocument_VersionEquivalent_Noops_Html()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        publisher.Initialize(_projectManager);
        var sourceTextContent = "<!-- The content -->";
        var initialSourceText = SourceText.From(sourceTextContent);
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishHtml(_hostProject.Key, _hostDocument.FilePath, initialSourceText, 123));

        // Act
        _projectManager.DocumentOpened(_hostProject.Key, _hostDocument.FilePath, initialSourceText);
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishHtml(_hostProject.Key, _hostDocument.FilePath, initialSourceText, 123));

        // Assert
        var updateRequest = Assert.Single(_serverClient.UpdateRequests);
        Assert.Equal(_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task ProjectSnapshotManager_DocumentChanged_ClosedDocument_RepublishesTextChanges()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        publisher.Initialize(_projectManager);
        var sourceTextContent = "// The content";
        var initialSourceText = SourceText.From(sourceTextContent);
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishCSharp(_hostProject.Key, _hostDocument.FilePath, initialSourceText, 123));
        _projectManager.DocumentOpened(_hostProject.Key, _hostDocument.FilePath, initialSourceText);

        // Act
        _projectManager.DocumentClosed(_hostProject.Key, _hostDocument.FilePath, new EmptyTextLoader(_hostDocument.FilePath));
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishCSharp(_hostProject.Key, _hostDocument.FilePath, initialSourceText, 123));

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal(_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal(sourceTextContent, textChange.NewText);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task ProjectSnapshotManager_DocumentMoved_DoesntRepublishWholeDocument()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        publisher.Initialize(_projectManager);
        var sourceTextContent = """
            public void Method()
            {
            }
            """;
        var initialSourceText = SourceText.From(sourceTextContent);
        var changedTextContent = """
            public void Method()
            {
                // some new code here
            }
            """;
        var changedSourceText = SourceText.From(changedTextContent);
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishCSharp(_hostProject.Key, _hostDocument.FilePath, initialSourceText, 123));
        _projectManager.DocumentOpened(_hostProject.Key, _hostDocument.FilePath, initialSourceText);

        // Act
        _projectManager.ProjectAdded(_hostProject2);
        _projectManager.DocumentAdded(_hostProject2.Key, _hostDocument, new EmptyTextLoader(_hostDocument.FilePath));
        await RunOnDispatcherThreadAsync(() =>
            publisher.PublishCSharp(_hostProject2.Key, _hostDocument.FilePath, changedSourceText, 124));

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal(_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal("// some new code here", textChange.NewText!.Trim());
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }
}
