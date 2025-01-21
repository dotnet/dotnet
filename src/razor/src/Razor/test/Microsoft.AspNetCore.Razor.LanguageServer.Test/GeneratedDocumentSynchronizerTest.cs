// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class GeneratedDocumentSynchronizerTest : LanguageServerTestBase
{
    private static readonly HostProject s_hostProject = new("/path/to/project.csproj", "/path/to/obj", RazorConfiguration.Default, "TestRootNamespace");
    private static readonly HostDocument s_hostDocument = new("/path/to/file.razor", "file.razor");

    private readonly GeneratedDocumentSynchronizer _synchronizer;
    private readonly TestGeneratedDocumentPublisher _publisher;
    private readonly TestProjectSnapshotManager _projectManager;

    public GeneratedDocumentSynchronizerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _publisher = new TestGeneratedDocumentPublisher();
        _projectManager = CreateProjectSnapshotManager();
        _synchronizer = new GeneratedDocumentSynchronizer(_publisher, TestLanguageServerFeatureOptions.Instance, _projectManager);
    }

    protected override async Task InitializeAsync()
    {
        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
            updater.AddDocument(s_hostProject.Key, s_hostDocument, TestMocks.CreateTextLoader("<p>Hello World</p>"));
        });
    }

    [Fact]
    public async Task DocumentProcessed_OpenDocument_Publishes()
    {
        // Arrange
        await _projectManager.UpdateAsync(updater =>
        {
            updater.OpenDocument(s_hostProject.Key, s_hostDocument.FilePath, SourceText.From("<p>Hello World</p>"));
        });

        var document = _projectManager.GetRequiredDocument(s_hostProject.Key, s_hostDocument.FilePath);
        var codeDocument = await document.GetGeneratedOutputAsync(DisposalToken);

        // Act
        _synchronizer.DocumentProcessed(codeDocument, document);

        // Assert
        Assert.True(_publisher.PublishedCSharp);
        Assert.True(_publisher.PublishedHtml);
    }

    [Fact]
    public async Task DocumentProcessed_CloseDocument_WithOption_Publishes()
    {
        var options = new TestLanguageServerFeatureOptions(updateBuffersForClosedDocuments: true);
        var synchronizer = new GeneratedDocumentSynchronizer(_publisher, options, _projectManager);

        var document = _projectManager.GetRequiredDocument(s_hostProject.Key, s_hostDocument.FilePath);
        var codeDocument = await document.GetGeneratedOutputAsync(DisposalToken);

        // Act
        synchronizer.DocumentProcessed(codeDocument, document);

        // Assert
        Assert.True(_publisher.PublishedCSharp);
        Assert.True(_publisher.PublishedHtml);
    }

    [Fact]
    public async Task DocumentProcessed_CloseDocument_DoesntPublish()
    {
        var document = _projectManager.GetRequiredDocument(s_hostProject.Key, s_hostDocument.FilePath);
        var codeDocument = await document.GetGeneratedOutputAsync(DisposalToken);

        // Act
        _synchronizer.DocumentProcessed(codeDocument, document);

        // Assert
        Assert.False(_publisher.PublishedCSharp);
        Assert.False(_publisher.PublishedHtml);
    }

    [Fact]
    public async Task DocumentProcessed_RemovedDocument_DoesntPublish()
    {
        var document = await _projectManager.UpdateAsync(updater =>
        {
            var removedDocument = updater.GetRequiredDocument(s_hostProject.Key, s_hostDocument.FilePath);
            updater.RemoveDocument(s_hostProject.Key, s_hostDocument.FilePath);

            return removedDocument;
        });

        var codeDocument = await document.GetGeneratedOutputAsync(DisposalToken);

        // Act
        _synchronizer.DocumentProcessed(codeDocument, document);

        // Assert
        Assert.False(_publisher.PublishedCSharp);
        Assert.False(_publisher.PublishedHtml);
    }

    private class TestGeneratedDocumentPublisher : IGeneratedDocumentPublisher
    {
        public bool PublishedCSharp { get; private set; }

        public bool PublishedHtml { get; private set; }

        public void PublishCSharp(ProjectKey projectKey, string filePath, SourceText sourceText, int hostDocumentVersion)
        {
            PublishedCSharp = true;
        }

        public void PublishHtml(ProjectKey projectKey, string filePath, SourceText sourceText, int hostDocumentVersion)
        {
            PublishedHtml = true;
        }
    }
}
