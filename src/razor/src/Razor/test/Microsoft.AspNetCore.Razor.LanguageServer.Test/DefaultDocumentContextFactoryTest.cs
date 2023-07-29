// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test;

public class DefaultDocumentContextFactoryTest : LanguageServerTestBase
{
    private readonly DocumentVersionCache _documentVersionCache;

    public DefaultDocumentContextFactoryTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _documentVersionCache = new DefaultDocumentVersionCache();
    }

    [Fact]
    public void TryCreateAsync_CanNotResolveDocument_ReturnsNull()
    {
        // Arrange
        var uri = new Uri("C:/path/to/file.cshtml");
        var factory = new DefaultDocumentContextFactory(new TestDocumentResolver(), _documentVersionCache, LoggerFactory);

        // Act
        var documentContext = factory.TryCreate(uri);

        // Assert
        Assert.Null(documentContext);
    }

    [Fact]
    public void TryCreateForOpenDocumentAsync_CanNotResolveDocument_ReturnsNull()
    {
        // Arrange
        var uri = new Uri("C:/path/to/file.cshtml");
        var factory = new DefaultDocumentContextFactory(new TestDocumentResolver(), _documentVersionCache, LoggerFactory);

        // Act
        var documentContext = factory.TryCreateForOpenDocument(uri);

        // Assert
        Assert.Null(documentContext);
    }

    [Fact]
    public void TryCreateForOpenDocumentAsync_CanNotResolveVersion_ReturnsNull()
    {
        // Arrange
        var uri = new Uri("C:/path/to/file.cshtml");
        var documentSnapshot = TestDocumentSnapshot.Create(uri.GetAbsoluteOrUNCPath());
        var documentResolver = new TestDocumentResolver(documentSnapshot);
        var factory = new DefaultDocumentContextFactory(documentResolver, _documentVersionCache, LoggerFactory);

        // Act
        var documentContext = factory.TryCreateForOpenDocument(uri);

        // Assert
        Assert.Null(documentContext);
    }

    [Fact]
    public void TryCreateAsync_ResolvesContent()
    {
        // Arrange
        var uri = new Uri("C:/path/to/file.cshtml");
        var documentSnapshot = TestDocumentSnapshot.Create(uri.GetAbsoluteOrUNCPath());
        var codeDocument = RazorCodeDocument.Create(RazorSourceDocument.Create(string.Empty, documentSnapshot.FilePath));
        documentSnapshot.With(codeDocument);
        var documentResolver = new TestDocumentResolver(documentSnapshot);
        var factory = new DefaultDocumentContextFactory(documentResolver, _documentVersionCache, LoggerFactory);

        // Act
        var documentContext = factory.TryCreate(uri);

        // Assert
        Assert.NotNull(documentContext);
        Assert.Equal(uri, documentContext.Uri);
        Assert.Same(documentSnapshot, documentContext.Snapshot);
    }

    [Fact]
    public async Task TryCreateForOpenDocumentAsync_ResolvesContent()
    {
        // Arrange
        var uri = new Uri("C:/path/to/file.cshtml");
        var documentSnapshot = TestDocumentSnapshot.Create(uri.GetAbsoluteOrUNCPath());
        var codeDocument = RazorCodeDocument.Create(RazorSourceDocument.Create(string.Empty, documentSnapshot.FilePath));
        documentSnapshot.With(codeDocument);
        var documentResolver = new TestDocumentResolver(documentSnapshot);
        await Dispatcher.RunOnDispatcherThreadAsync(() => _documentVersionCache.TrackDocumentVersion(documentSnapshot, version: 1337), DisposalToken);
        var factory = new DefaultDocumentContextFactory(documentResolver, _documentVersionCache, LoggerFactory);

        // Act
        var documentContext = factory.TryCreateForOpenDocument(uri);

        // Assert
        Assert.NotNull(documentContext);
        Assert.Equal(1337, documentContext.Version);
        Assert.Equal(uri, documentContext.Uri);
        Assert.Same(documentSnapshot, documentContext.Snapshot);
    }

    private class TestDocumentResolver : ISnapshotResolver
    {
        private readonly IDocumentSnapshot _documentSnapshot;

        public TestDocumentResolver()
        {
        }

        public TestDocumentResolver(IDocumentSnapshot documentSnapshot)
        {
            _documentSnapshot = documentSnapshot;
        }

        public IEnumerable<IProjectSnapshot> FindPotentialProjects(string documentFilePath)
        {
            throw new NotImplementedException();
        }

        public IProjectSnapshot GetMiscellaneousProject()
        {
            throw new NotImplementedException();
        }

        public bool TryResolveDocumentInAnyProject(string documentFilePath, [NotNullWhen(true)] out IDocumentSnapshot documentSnapshot)
        {
            if (documentFilePath == _documentSnapshot?.FilePath)
            {
                documentSnapshot = _documentSnapshot;
                return true;
            }

            documentSnapshot = null;
            return false;
        }
    }
}
