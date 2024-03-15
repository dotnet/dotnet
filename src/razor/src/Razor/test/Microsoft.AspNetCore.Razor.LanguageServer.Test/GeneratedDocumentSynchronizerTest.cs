// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class GeneratedDocumentSynchronizerTest : LanguageServerTestBase
{
    private readonly DocumentVersionCache _cache;
    private readonly GeneratedDocumentSynchronizer _synchronizer;
    private readonly TestGeneratedDocumentPublisher _publisher;
    private readonly IDocumentSnapshot _document;
    private readonly RazorCodeDocument _codeDocument;

    public GeneratedDocumentSynchronizerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var projectManager = StrictMock.Of<IProjectSnapshotManager>();
        _cache = new DocumentVersionCache(projectManager);
        _publisher = new TestGeneratedDocumentPublisher();
        _synchronizer = new GeneratedDocumentSynchronizer(_publisher, _cache, Dispatcher);
        _document = TestDocumentSnapshot.Create("C:/path/to/file.razor");
        _codeDocument = CreateCodeDocument("<p>Hello World</p>");
    }

    [Fact]
    public async Task DocumentProcessed_UnknownVersion_Noops()
    {
        // Arrange

        // Act
        await Dispatcher.RunAsync(
            () => _synchronizer.DocumentProcessed(_codeDocument, _document), DisposalToken);

        // Assert
        Assert.False(_publisher.PublishedCSharp);
        Assert.False(_publisher.PublishedHtml);
    }

    [Fact]
    public async Task DocumentProcessed_KnownVersion_Publishes()
    {
        // Arrange
        await Dispatcher.RunAsync(() =>
        {
            _cache.TrackDocumentVersion(_document, version: 1337);

            // Act
            _synchronizer.DocumentProcessed(_codeDocument, _document);
        }, DisposalToken);

        // Assert
        Assert.True(_publisher.PublishedCSharp);
        Assert.True(_publisher.PublishedHtml);
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
