// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test;

public class DocumentContextFactoryTest : LanguageServerTestBase
{
    private static readonly string s_baseDirectory = PathUtilities.CreateRootedPath("path", "to");

    private readonly TestProjectSnapshotManager _projectManager;
    private readonly IDocumentVersionCache _documentVersionCache;

    public DocumentContextFactoryTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _projectManager = CreateProjectSnapshotManager();
        _documentVersionCache = new DocumentVersionCache(_projectManager);
    }

    [Fact]
    public async Task TryCreateAsync_CanNotResolveDocument_ReturnsNull()
    {
        // Arrange
        var filePath = FilePathNormalizer.Normalize(Path.Combine(s_baseDirectory, "file.cshtml"));
        var uri = new Uri(filePath);

        var factory = new DocumentContextFactory(_projectManager, new TestDocumentResolver(), _documentVersionCache, LoggerFactory);

        // Act
        var documentContext = await factory.TryCreateAsync(uri, DisposalToken);

        // Assert
        Assert.Null(documentContext);
    }

    [Fact]
    public async Task TryCreateForOpenDocumentAsync_CanNotResolveDocument_ReturnsNull()
    {
        // Arrange
        var filePath = FilePathNormalizer.Normalize(Path.Combine(s_baseDirectory, "file.cshtml"));
        var uri = new Uri(filePath);

        var factory = new DocumentContextFactory(_projectManager, new TestDocumentResolver(), _documentVersionCache, LoggerFactory);

        // Act
        var documentContext = await factory.TryCreateForOpenDocumentAsync(uri, DisposalToken);

        // Assert
        Assert.Null(documentContext);
    }

    [Fact]
    public async Task TryCreateForOpenDocumentAsync_CanNotResolveVersion_ReturnsNull()
    {
        // Arrange
        var filePath = FilePathNormalizer.Normalize(Path.Combine(s_baseDirectory, "file.cshtml"));
        var uri = new Uri(filePath);

        var documentSnapshot = TestDocumentSnapshot.Create(filePath);
        var documentResolver = new TestDocumentResolver(documentSnapshot);
        var factory = new DocumentContextFactory(_projectManager, documentResolver, _documentVersionCache, LoggerFactory);

        // Act
        var documentContext = await factory.TryCreateForOpenDocumentAsync(uri, DisposalToken);

        // Assert
        Assert.Null(documentContext);
    }

    [Fact]
    public async Task TryCreateAsync_ResolvesContent()
    {
        // Arrange
        var filePath = FilePathNormalizer.Normalize(Path.Combine(s_baseDirectory, "file.cshtml"));
        var uri = new Uri(filePath);

        var documentSnapshot = TestDocumentSnapshot.Create(filePath);
        var codeDocument = RazorCodeDocument.Create(RazorSourceDocument.Create(string.Empty, documentSnapshot.FilePath));
        documentSnapshot.With(codeDocument);
        var documentResolver = new TestDocumentResolver(documentSnapshot);
        var factory = new DocumentContextFactory(_projectManager, documentResolver, _documentVersionCache, LoggerFactory);

        // Act
        var documentContext = await factory.TryCreateAsync(uri, DisposalToken);

        // Assert
        Assert.NotNull(documentContext);
        Assert.Equal(uri, documentContext.Uri);
        Assert.Same(documentSnapshot, documentContext.Snapshot);
    }

    [Fact]
    public async Task TryCreateAsync_WithProjectContext_Resolves()
    {
        // Arrange
        var filePath = FilePathNormalizer.Normalize(Path.Combine(s_baseDirectory, "file.cshtml"));
        var intermediateOutputPath = Path.Combine(s_baseDirectory, "obj");
        var projectFilePath = Path.Combine(s_baseDirectory, "project.csproj");
        var uri = new Uri(filePath);

        var documentSnapshot = TestDocumentSnapshot.Create(filePath);
        var codeDocument = RazorCodeDocument.Create(RazorSourceDocument.Create(string.Empty, documentSnapshot.FilePath));
        documentSnapshot.With(codeDocument);
        var documentResolver = new TestDocumentResolver(documentSnapshot);
        var factory = new DocumentContextFactory(_projectManager, documentResolver, _documentVersionCache, LoggerFactory);

        var hostProject = new HostProject(projectFilePath, intermediateOutputPath, RazorConfiguration.Default, rootNamespace: null);
        var hostDocument = new HostDocument(filePath, "file.cshtml");

        await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
            updater.DocumentAdded(hostProject.Key, hostDocument, new EmptyTextLoader(filePath));
        });

        // Act
        var documentContext = await factory.TryCreateAsync(uri, new VisualStudio.LanguageServer.Protocol.VSProjectContext { Id = hostProject.Key.Id }, DisposalToken);

        // Assert
        Assert.NotNull(documentContext);
        Assert.Equal(uri, documentContext.Uri);
    }

    [Fact]
    public async Task TryCreateAsync_WithProjectContext_DoesntUseSnapshotResolver()
    {
        // Arrange
        var filePath = FilePathNormalizer.Normalize(Path.Combine(s_baseDirectory, "file.cshtml"));
        var intermediateOutputPath = Path.Combine(s_baseDirectory, "obj");
        var projectFilePath = Path.Combine(s_baseDirectory, "project.csproj");
        var uri = new Uri(filePath);

        var documentSnapshot = TestDocumentSnapshot.Create(filePath);
        var codeDocument = RazorCodeDocument.Create(RazorSourceDocument.Create(string.Empty, documentSnapshot.FilePath));
        documentSnapshot.With(codeDocument);
        var documentResolverMock = new Mock<ISnapshotResolver>(MockBehavior.Strict);
        var factory = new DocumentContextFactory(_projectManager, documentResolverMock.Object, _documentVersionCache, LoggerFactory);

        var hostProject = new HostProject(projectFilePath, intermediateOutputPath, RazorConfiguration.Default, rootNamespace: null);
        var hostDocument = new HostDocument(filePath, "file.cshtml");

        await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
            updater.DocumentAdded(hostProject.Key, hostDocument, new EmptyTextLoader(filePath));
        });

        // Act
        var documentContext = await factory.TryCreateAsync(uri, new VisualStudio.LanguageServer.Protocol.VSProjectContext { Id = hostProject.Key.Id }, DisposalToken);

        // Assert
        Assert.NotNull(documentContext);
        Assert.Equal(uri, documentContext.Uri);
        documentResolverMock.Verify();
    }

    [Fact]
    public async Task TryCreateForOpenDocumentAsync_ResolvesContent()
    {
        // Arrange
        var filePath = FilePathNormalizer.Normalize(Path.Combine(s_baseDirectory, "file.cshtml"));
        var uri = new Uri(filePath);

        var documentSnapshot = TestDocumentSnapshot.Create(filePath);
        var codeDocument = RazorCodeDocument.Create(RazorSourceDocument.Create(string.Empty, documentSnapshot.FilePath));
        documentSnapshot.With(codeDocument);
        var documentResolver = new TestDocumentResolver(documentSnapshot);
        await Dispatcher.RunAsync(() => _documentVersionCache.TrackDocumentVersion(documentSnapshot, version: 1337), DisposalToken);
        var factory = new DocumentContextFactory(_projectManager, documentResolver, _documentVersionCache, LoggerFactory);

        // Act
        var documentContext = await factory.TryCreateForOpenDocumentAsync(uri, DisposalToken);

        // Assert
        Assert.NotNull(documentContext);
        Assert.Equal(1337, documentContext.Version);
        Assert.Equal(uri, documentContext.Uri);
        Assert.Same(documentSnapshot, documentContext.Snapshot);
    }

    private class TestDocumentResolver(IDocumentSnapshot? documentSnapshot = null) : ISnapshotResolver
    {
        private readonly IDocumentSnapshot? _documentSnapshot = documentSnapshot;

        public ImmutableArray<IProjectSnapshot> FindPotentialProjects(string documentFilePath)
            => throw new NotImplementedException();

        public Task<IProjectSnapshot> GetMiscellaneousProjectAsync(CancellationToken cancellationToken)
            => throw new NotImplementedException();

        public Task<IDocumentSnapshot?> ResolveDocumentInAnyProjectAsync(string documentFilePath, CancellationToken cancellationToken)
            => documentFilePath == _documentSnapshot?.FilePath
                ? Task.FromResult<IDocumentSnapshot?>(_documentSnapshot)
                : Task.FromResult<IDocumentSnapshot?>(null);
    }
}
