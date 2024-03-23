// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class GeneratedDocumentPublisherTest : LanguageServerTestBase
{
    private static readonly HostProject s_hostProject = new("/path/to/project.csproj", "/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
    private static readonly HostProject s_hostProject2 = new("/path/to/project2.csproj", "/path/to/obj2", RazorConfiguration.Default, "TestRootNamespace");
    private static readonly HostDocument s_hostDocument = new("/path/to/file.razor", "file.razor");

    private readonly TestClient _serverClient = new();
    private readonly TestProjectSnapshotManager _projectManager;

    public GeneratedDocumentPublisherTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _projectManager = CreateProjectSnapshotManager();
    }

    protected override async Task InitializeAsync()
    {
        await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(s_hostProject);
            updater.DocumentAdded(s_hostProject.Key, s_hostDocument, new EmptyTextLoader(s_hostDocument.FilePath));
        });
    }

    [Fact]
    public async Task PublishCSharp_FirstTime_PublishesEntireSourceText()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(_projectManager, Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var content = "// C# content";
        var sourceText = SourceText.From(content);

        // Act
        await RunOnDispatcherAsync(() =>
        {
            publisher.PublishCSharp(s_hostProject.Key, "/path/to/file.razor", sourceText, 123);
        });

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
        var publisher = new GeneratedDocumentPublisher(_projectManager, Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var content = "HTML content";
        var sourceText = SourceText.From(content);

        // Act
        await RunOnDispatcherAsync(() =>
            publisher.PublishHtml(s_hostProject.Key, "/path/to/file.razor", sourceText, 123));

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
        var publisher = new GeneratedDocumentPublisher(_projectManager, Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var initialSourceText = SourceText.From("// Initial content\n");
        await RunOnDispatcherAsync(() =>
            publisher.PublishCSharp(s_hostProject.Key, "/path/to/file.razor", initialSourceText, 123));
        var change = new TextChange(
            new TextSpan(initialSourceText.Length, 0),
            "// Another line");
        var changedSourceText = initialSourceText.WithChanges(change);

        // Act
        await RunOnDispatcherAsync(() =>
            publisher.PublishCSharp(s_hostProject.Key, "/path/to/file.razor", changedSourceText, 124));

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
        var publisher = new GeneratedDocumentPublisher(_projectManager, Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var initialSourceText = SourceText.From("HTML content\n");
        await RunOnDispatcherAsync(() =>
            publisher.PublishHtml(s_hostProject.Key, "/path/to/file.razor", initialSourceText, 123));
        var change = new TextChange(
            new TextSpan(initialSourceText.Length, 0),
            "More content!!");
        var changedSourceText = initialSourceText.WithChanges(change);

        // Act
        await RunOnDispatcherAsync(() =>
            publisher.PublishHtml(s_hostProject.Key, "/path/to/file.razor", changedSourceText, 124));

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
        var publisher = new GeneratedDocumentPublisher(_projectManager, Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var sourceTextContent = "// The content";
        var initialSourceText = SourceText.From(sourceTextContent);
        await RunOnDispatcherAsync(() =>
            publisher.PublishCSharp(s_hostProject.Key, "/path/to/file.razor", initialSourceText, 123));
        var identicalSourceText = SourceText.From(sourceTextContent);

        // Act
        await RunOnDispatcherAsync(() =>
            publisher.PublishCSharp(s_hostProject.Key, "/path/to/file.razor", identicalSourceText, 124));

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
        var publisher = new GeneratedDocumentPublisher(_projectManager, Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var sourceTextContent = "HTMl content";
        var initialSourceText = SourceText.From(sourceTextContent);
        await RunOnDispatcherAsync(() =>
            publisher.PublishHtml(s_hostProject.Key, "/path/to/file.razor", initialSourceText, 123));
        var identicalSourceText = SourceText.From(sourceTextContent);

        // Act
        await RunOnDispatcherAsync(() =>
            publisher.PublishHtml(s_hostProject.Key, "/path/to/file.razor", identicalSourceText, 124));

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
        var publisher = new GeneratedDocumentPublisher(_projectManager, Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var sourceTextContent = "// The content";
        var initialSourceText = SourceText.From(sourceTextContent);
        await RunOnDispatcherAsync(() =>
            publisher.PublishCSharp(s_hostProject.Key, "/path/to/file1.razor", initialSourceText, 123));
        var identicalSourceText = SourceText.From(sourceTextContent);

        // Act
        await RunOnDispatcherAsync(() =>
            publisher.PublishCSharp(s_hostProject.Key, "/path/to/file2.razor", identicalSourceText, 123));

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
        var publisher = new GeneratedDocumentPublisher(_projectManager, Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var sourceTextContent = "HTML content";
        var initialSourceText = SourceText.From(sourceTextContent);
        await RunOnDispatcherAsync(() =>
            publisher.PublishHtml(s_hostProject.Key, "/path/to/file1.razor", initialSourceText, 123));
        var identicalSourceText = SourceText.From(sourceTextContent);

        // Act
        await RunOnDispatcherAsync(() =>
            publisher.PublishHtml(s_hostProject.Key, "/path/to/file2.razor", identicalSourceText, 123));

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
        var publisher = new GeneratedDocumentPublisher(_projectManager, Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var sourceTextContent = "// The content";
        var initialSourceText = SourceText.From(sourceTextContent);

        await RunOnDispatcherAsync(() =>
        {
            publisher.PublishCSharp(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);
        });

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.DocumentOpened(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText);
        });

        await RunOnDispatcherAsync(() =>
        {
            publisher.PublishCSharp(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 124);
        });

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal(s_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        Assert.Empty(updateRequest.Changes);
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task ProjectSnapshotManager_DocumentChanged_OpenDocument_VersionEquivalent_Noops_CSharp()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(_projectManager, Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var sourceTextContent = "// The content";
        var initialSourceText = SourceText.From(sourceTextContent);

        await RunOnDispatcherAsync(() =>
        {
            publisher.PublishCSharp(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);
        });

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.DocumentOpened(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText);
        });

        await RunOnDispatcherAsync(() =>
        {
            publisher.PublishCSharp(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);
        });

        // Assert
        var updateRequest = Assert.Single(_serverClient.UpdateRequests);
        Assert.Equal(s_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task ProjectSnapshotManager_DocumentChanged_OpenDocument_PublishesEmptyTextChanges_Html()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(_projectManager, Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var sourceTextContent = "<!-- The content -->";
        var initialSourceText = SourceText.From(sourceTextContent);

        await RunOnDispatcherAsync(() =>
        {
            publisher.PublishHtml(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);
        });

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.DocumentOpened(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText);
        });

        await RunOnDispatcherAsync(() =>
        {
            publisher.PublishHtml(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 124);
        });

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal(s_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        Assert.Empty(updateRequest.Changes);
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task ProjectSnapshotManager_DocumentChanged_OpenDocument_VersionEquivalent_Noops_Html()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(_projectManager, Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var sourceTextContent = "<!-- The content -->";
        var initialSourceText = SourceText.From(sourceTextContent);

        await RunOnDispatcherAsync(() =>
        {
            publisher.PublishHtml(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);
        });

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.DocumentOpened(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText);
        });

        await RunOnDispatcherAsync(() =>
        {
            publisher.PublishHtml(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);
        });

        // Assert
        var updateRequest = Assert.Single(_serverClient.UpdateRequests);
        Assert.Equal(s_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task ProjectSnapshotManager_DocumentChanged_ClosedDocument_RepublishesTextChanges()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(_projectManager, Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var sourceTextContent = "// The content";
        var initialSourceText = SourceText.From(sourceTextContent);

        await RunOnDispatcherAsync(() =>
        {
            publisher.PublishCSharp(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);
        });

        await _projectManager.UpdateAsync(updater =>
        {
            updater.DocumentOpened(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText);
        });

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.DocumentClosed(s_hostProject.Key, s_hostDocument.FilePath, new EmptyTextLoader(s_hostDocument.FilePath));
        });

        await RunOnDispatcherAsync(() =>
        {
            publisher.PublishCSharp(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);
        });

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal(s_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal(sourceTextContent, textChange.NewText);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task ProjectSnapshotManager_DocumentMoved_DoesntRepublishWholeDocument()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(_projectManager, Dispatcher, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
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

        await RunOnDispatcherAsync(() =>
        {
            publisher.PublishCSharp(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);
        });

        await _projectManager.UpdateAsync(updater =>
        {
            updater.DocumentOpened(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText);
        });

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(s_hostProject2);
            updater.DocumentAdded(s_hostProject2.Key, s_hostDocument, new EmptyTextLoader(s_hostDocument.FilePath));
        });

        await RunOnDispatcherAsync(() =>
        {
            publisher.PublishCSharp(s_hostProject2.Key, s_hostDocument.FilePath, changedSourceText, 124);
        });

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal(s_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal("// some new code here", textChange.NewText!.Trim());
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task ProjectSnapshotManager_ProjectRemoved_ClearsContent()
    {
        // Arrange
        var options = new TestLanguageServerFeatureOptions(includeProjectKeyInGeneratedFilePath: true);
        var publisher = new GeneratedDocumentPublisher(_projectManager, Dispatcher, _serverClient, options, LoggerFactory);
        var sourceTextContent = "// The content";
        var initialSourceText = SourceText.From(sourceTextContent);

        await RunOnDispatcherAsync(() =>
        {
            publisher.PublishCSharp(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);
        });

        await _projectManager.UpdateAsync(updater =>
        {
            updater.DocumentOpened(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText);
        });

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectRemoved(s_hostProject.Key);
        });

        Assert.Equal(0, publisher.GetTestAccessor().PublishedCSharpDataCount);
    }
}
