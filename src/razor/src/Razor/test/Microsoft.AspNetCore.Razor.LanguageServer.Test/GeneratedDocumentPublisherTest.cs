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
using Microsoft.VisualStudio.LanguageServer.Protocol;
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
            updater.AddProject(s_hostProject);
            updater.AddDocument(s_hostProject.Key, s_hostDocument, EmptyTextLoader.Instance);
        });
    }

    [Fact]
    public void PublishCSharp_FirstTime_PublishesEntireSourceText()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(_projectManager, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var content = "// C# content";
        var sourceText = SourceText.From(content);

        // Act
        publisher.PublishCSharp(s_hostProject.Key, "/path/to/file.razor", sourceText, 123);

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
        var publisher = new GeneratedDocumentPublisher(_projectManager, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var content = "HTML content";
        var sourceText = SourceText.From(content);

        // Act
        publisher.PublishHtml(s_hostProject.Key, "/path/to/file.razor", sourceText, 123);

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
        var publisher = new GeneratedDocumentPublisher(_projectManager, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var initialSourceText = SourceText.From("// Initial content\n");
        publisher.PublishCSharp(s_hostProject.Key, "/path/to/file.razor", initialSourceText, 123);
        var change = new TextChange(
            new TextSpan(initialSourceText.Length, 0),
            "// Another line");
        var changedSourceText = initialSourceText.WithChanges(change);

        // Act
        publisher.PublishCSharp(s_hostProject.Key, "/path/to/file.razor", changedSourceText, 124);

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal("/path/to/file.razor", updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal(change.ToRazorTextChange(), textChange);
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public void PublishHtml_SecondTime_PublishesSourceTextDifferences()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(_projectManager, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var initialSourceText = SourceText.From("HTML content\n");
        publisher.PublishHtml(s_hostProject.Key, "/path/to/file.razor", initialSourceText, 123);
        var change = new TextChange(
            new TextSpan(initialSourceText.Length, 0),
            "More content!!");
        var changedSourceText = initialSourceText.WithChanges(change);

        // Act
        publisher.PublishHtml(s_hostProject.Key, "/path/to/file.razor", changedSourceText, 124);

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal("/path/to/file.razor", updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal(change.ToRazorTextChange(), textChange);
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public void PublishCSharp_SecondTime_IdenticalContent_NoTextChanges()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(_projectManager, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var sourceTextContent = "// The content";
        var initialSourceText = SourceText.From(sourceTextContent);
        publisher.PublishCSharp(s_hostProject.Key, "/path/to/file.razor", initialSourceText, 123);
        var identicalSourceText = SourceText.From(sourceTextContent);

        // Act
        publisher.PublishCSharp(s_hostProject.Key, "/path/to/file.razor", identicalSourceText, 124);

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
        var publisher = new GeneratedDocumentPublisher(_projectManager, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var sourceTextContent = "HTMl content";
        var initialSourceText = SourceText.From(sourceTextContent);
        publisher.PublishHtml(s_hostProject.Key, "/path/to/file.razor", initialSourceText, 123);
        var identicalSourceText = SourceText.From(sourceTextContent);

        // Act
        publisher.PublishHtml(s_hostProject.Key, "/path/to/file.razor", identicalSourceText, 124);

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
        var publisher = new GeneratedDocumentPublisher(_projectManager, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var sourceTextContent = "// The content";
        var initialSourceText = SourceText.From(sourceTextContent);
        publisher.PublishCSharp(s_hostProject.Key, "/path/to/file1.razor", initialSourceText, 123);
        var identicalSourceText = SourceText.From(sourceTextContent);

        // Act
        publisher.PublishCSharp(s_hostProject.Key, "/path/to/file2.razor", identicalSourceText, 123);

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
        var publisher = new GeneratedDocumentPublisher(_projectManager, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var sourceTextContent = "HTML content";
        var initialSourceText = SourceText.From(sourceTextContent);
        publisher.PublishHtml(s_hostProject.Key, "/path/to/file1.razor", initialSourceText, 123);
        var identicalSourceText = SourceText.From(sourceTextContent);

        // Act
        publisher.PublishHtml(s_hostProject.Key, "/path/to/file2.razor", identicalSourceText, 123);

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal("/path/to/file2.razor", updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal(sourceTextContent, textChange.NewText);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task PublishCSharp_OpenDocument_SameText_DifferentHostDocumentVersions_PublishesEmptyTextChanges()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(_projectManager, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var initialSourceText = SourceText.From("// The content");

        publisher.PublishCSharp(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.OpenDocument(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText);
        });

        publisher.PublishCSharp(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 124);

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal(s_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        Assert.Empty(updateRequest.Changes);
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task PublishCSharp_OpenDocument_SameText_SameHostDocumentVersion_Ignored()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(_projectManager, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var initialSourceText = SourceText.From("// The content");

        publisher.PublishCSharp(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.OpenDocument(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText);
        });

        publisher.PublishCSharp(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);

        // Assert
        var updateRequest = Assert.Single(_serverClient.UpdateRequests);
        Assert.Equal(s_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task PublishHtml_OpenDocument_SameText_DifferentHostDocumentVersions_PublishesEmptyTextChanges()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(_projectManager, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var initialSourceText = SourceText.From("<!-- The content -->");

        publisher.PublishHtml(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.OpenDocument(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText);
        });

        publisher.PublishHtml(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 124);

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal(s_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        Assert.Empty(updateRequest.Changes);
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task PublishHtml_OpenDocument_SameText_SameHostDocumentVersion_Ignored()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(_projectManager, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var initialSourceText = SourceText.From("<!-- The content -->");

        publisher.PublishHtml(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.OpenDocument(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText);
        });

        publisher.PublishHtml(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);

        // Assert
        var updateRequest = Assert.Single(_serverClient.UpdateRequests);
        Assert.Equal(s_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task PublishCSharp_CloseDocument_RepublishesTextChanges()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(_projectManager, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
        var sourceTextContent = "// The content";
        var initialSourceText = SourceText.From(sourceTextContent);

        publisher.PublishCSharp(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.OpenDocument(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText);
        });

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.CloseDocument(s_hostProject.Key, s_hostDocument.FilePath, EmptyTextLoader.Instance);
        });

        publisher.PublishCSharp(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal(s_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal(sourceTextContent, textChange.NewText);
        Assert.Equal(123, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task PublishCSharp_DocumentMoved_DoesntRepublishWholeDocument()
    {
        // Arrange
        var publisher = new GeneratedDocumentPublisher(_projectManager, _serverClient, TestLanguageServerFeatureOptions.Instance, LoggerFactory);
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

        publisher.PublishCSharp(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.OpenDocument(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText);
        });

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject2);
            updater.AddDocument(s_hostProject2.Key, s_hostDocument, EmptyTextLoader.Instance);
        });

        publisher.PublishCSharp(s_hostProject2.Key, s_hostDocument.FilePath, changedSourceText, 124);

        // Assert
        Assert.Equal(2, _serverClient.UpdateRequests.Count);
        var updateRequest = _serverClient.UpdateRequests.Last();
        Assert.Equal(s_hostDocument.FilePath, updateRequest.HostDocumentFilePath);
        var textChange = Assert.Single(updateRequest.Changes);
        Assert.Equal("// some new code here", textChange.NewText!.Trim());
        Assert.Equal(124, updateRequest.HostDocumentVersion);
    }

    [Fact]
    public async Task PublishCSharp_RemoveDocument_ClearsContent()
    {
        // Arrange
        var options = new TestLanguageServerFeatureOptions(includeProjectKeyInGeneratedFilePath: true);
        var publisher = new GeneratedDocumentPublisher(_projectManager, _serverClient, options, LoggerFactory);
        var initialSourceText = SourceText.From("// The content");

        publisher.PublishCSharp(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.OpenDocument(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText);
        });

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.RemoveDocument(s_hostProject.Key, s_hostDocument.FilePath);
        });

        Assert.Equal(0, publisher.GetTestAccessor().PublishedCSharpDataCount);
    }

    [Fact]
    public async Task PublishCSharp_RemoveProject_ClearsContent()
    {
        // Arrange
        var options = new TestLanguageServerFeatureOptions(includeProjectKeyInGeneratedFilePath: true);
        var publisher = new GeneratedDocumentPublisher(_projectManager, _serverClient, options, LoggerFactory);
        var initialSourceText = SourceText.From("// The content");

        publisher.PublishCSharp(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText, 123);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.OpenDocument(s_hostProject.Key, s_hostDocument.FilePath, initialSourceText);
        });

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.RemoveProject(s_hostProject.Key);
        });

        Assert.Equal(0, publisher.GetTestAccessor().PublishedCSharpDataCount);
    }
}
