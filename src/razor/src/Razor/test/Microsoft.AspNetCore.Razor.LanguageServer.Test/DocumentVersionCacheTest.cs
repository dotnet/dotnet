// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class DocumentVersionCacheTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public void MarkAsLatestVersion_UntrackedDocument_Noops()
    {
        // Arrange
        var cache = new DocumentVersionCache();
        var cacheAccessor = cache.GetTestAccessor();
        var document = TestDocumentSnapshot.Create("C:/file.cshtml");
        cache.TrackDocumentVersion(document, 123);
        var untrackedDocument = TestDocumentSnapshot.Create("C:/other.cshtml");

        // Act
        cacheAccessor.MarkAsLatestVersion(untrackedDocument);

        // Assert
        Assert.False(cache.TryGetDocumentVersion(untrackedDocument, out var version));
        Assert.Null(version);
    }

    [Fact]
    public void MarkAsLatestVersion_KnownDocument_TracksNewDocumentAsLatest()
    {
        // Arrange
        var cache = new DocumentVersionCache();
        var cacheAccessor = cache.GetTestAccessor();
        var documentInitial = TestDocumentSnapshot.Create("C:/file.cshtml");
        cache.TrackDocumentVersion(documentInitial, 123);
        var documentLatest = TestDocumentSnapshot.Create(documentInitial.FilePath);

        // Act
        cacheAccessor.MarkAsLatestVersion(documentLatest);

        // Assert
        Assert.True(cache.TryGetDocumentVersion(documentLatest, out var version));
        Assert.Equal(123, version);
    }

    [Fact]
    public async Task ProjectSnapshotManager_Changed_DocumentRemoved_DoesNotEvictDocument()
    {
        // Arrange
        var cache = new DocumentVersionCache();
        var projectSnapshotManager = CreateProjectSnapshotManager();
        cache.Initialize(projectSnapshotManager);

        var document = TestDocumentSnapshot.Create("C:/file.cshtml");
        Assert.True(document.TryGetText(out var text));
        Assert.True(document.TryGetTextVersion(out var textVersion));
        var textAndVersion = TextAndVersion.Create(text, textVersion);
        cache.TrackDocumentVersion(document, 1337);

        await RunOnDispatcherThreadAsync(() =>
        {
            projectSnapshotManager.ProjectAdded(document.ProjectInternal.HostProject);
            projectSnapshotManager.DocumentAdded(document.ProjectInternal.Key, document.State.HostDocument, TextLoader.From(textAndVersion));
        });

        // Act - 1
        var result = cache.TryGetDocumentVersion(document, out _);

        // Assert - 1
        Assert.True(result);

        // Act - 2
        await RunOnDispatcherThreadAsync(() =>
            projectSnapshotManager.DocumentRemoved(document.ProjectInternal.Key, document.State.HostDocument));
        result = cache.TryGetDocumentVersion(document, out _);

        // Assert - 2
        Assert.True(result);
    }

    [Fact]
    public async Task ProjectSnapshotManager_Changed_OpenDocumentRemoved_DoesNotEvictDocument()
    {
        // Arrange
        var cache = new DocumentVersionCache();
        var projectSnapshotManager = CreateProjectSnapshotManager();
        cache.Initialize(projectSnapshotManager);

        var document = TestDocumentSnapshot.Create("C:/file.cshtml");
        Assert.True(document.TryGetText(out var text));
        Assert.True(document.TryGetTextVersion(out var textVersion));
        var textAndVersion = TextAndVersion.Create(text, textVersion);
        cache.TrackDocumentVersion(document, 1337);

        await RunOnDispatcherThreadAsync(() =>
        {
            projectSnapshotManager.ProjectAdded(document.ProjectInternal.HostProject);
            projectSnapshotManager.DocumentAdded(document.ProjectInternal.Key, document.State.HostDocument, TextLoader.From(textAndVersion));
            projectSnapshotManager.DocumentOpened(document.ProjectInternal.Key, document.FilePath, textAndVersion.Text);
        });

        // Act - 1
        var result = cache.TryGetDocumentVersion(document, out _);

        // Assert - 1
        Assert.True(result);
        Assert.True(projectSnapshotManager.IsDocumentOpen(document.FilePath));

        // Act - 2
        await RunOnDispatcherThreadAsync(() =>
            projectSnapshotManager.DocumentRemoved(document.ProjectInternal.Key, document.State.HostDocument));
        result = cache.TryGetDocumentVersion(document, out _);

        // Assert - 2
        Assert.True(result);
    }

    [Fact]
    public async Task ProjectSnapshotManager_Changed_DocumentClosed_EvictsDocument()
    {
        // Arrange
        var cache = new DocumentVersionCache();
        var projectSnapshotManager = CreateProjectSnapshotManager();
        cache.Initialize(projectSnapshotManager);

        var document = TestDocumentSnapshot.Create("C:/file.cshtml");
        Assert.True(document.TryGetText(out var text));
        Assert.True(document.TryGetTextVersion(out var textVersion));
        var textAndVersion = TextAndVersion.Create(text, textVersion);
        cache.TrackDocumentVersion(document, 1337);
        var textLoader = TextLoader.From(textAndVersion);

        await RunOnDispatcherThreadAsync(() =>
        {
            projectSnapshotManager.ProjectAdded(document.ProjectInternal.HostProject);
            projectSnapshotManager.DocumentAdded(document.ProjectInternal.Key, document.State.HostDocument, textLoader);
        });

        // Act - 1
        var result = cache.TryGetDocumentVersion(document, out _);

        // Assert - 1
        Assert.True(result);

        // Act - 2
        await RunOnDispatcherThreadAsync(() =>
            projectSnapshotManager.DocumentClosed(document.ProjectInternal.HostProject.Key, document.State.HostDocument.FilePath, textLoader));
        result = cache.TryGetDocumentVersion(document, out var version);

        // Assert - 2
        Assert.False(result);
        Assert.Null(version);
    }

    [Fact]
    public void TrackDocumentVersion_AddsFirstEntry()
    {
        // Arrange
        var cache = new DocumentVersionCache();
        var cacheAccessor = cache.GetTestAccessor();
        var document = TestDocumentSnapshot.Create("C:/file.cshtml");

        // Act
        cache.TrackDocumentVersion(document, 1337);

        // Assert
        var entries = cacheAccessor.GetEntries();
        var (filePath, entry) = Assert.Single(entries);
        Assert.Equal(document.FilePath, filePath);
        var (actualDocument, actualVersion) = Assert.Single(entry);
        Assert.Same(document, actualDocument);
        Assert.Equal(1337, actualVersion);
    }

    [Fact]
    public void TrackDocumentVersion_EvictsOldEntries()
    {
        // Arrange
        var cache = new DocumentVersionCache();
        var cacheAccessor = cache.GetTestAccessor();
        var document = TestDocumentSnapshot.Create("C:/file.cshtml");

        for (var i = 0; i < DocumentVersionCache.MaxDocumentTrackingCount; i++)
        {
            cache.TrackDocumentVersion(document, i);
        }

        // Act
        cache.TrackDocumentVersion(document, 1337);

        // Assert
        var (_, entry) = Assert.Single(cacheAccessor.GetEntries());
        Assert.Equal(DocumentVersionCache.MaxDocumentTrackingCount, entry.Length);
        Assert.Equal(1337, entry[^1].Version);
    }

    [Fact]
    public void TryGetDocumentVersion_UntrackedDocumentPath_ReturnsFalse()
    {
        // Arrange
        var cache = new DocumentVersionCache();
        var document = TestDocumentSnapshot.Create("C:/file.cshtml");

        // Act
        var result = cache.TryGetDocumentVersion(document, out var version);

        // Assert
        Assert.False(result);
        Assert.Null(version);
    }

    [Fact]
    public void TryGetDocumentVersion_EvictedDocument_ReturnsFalse()
    {
        // Arrange
        var cache = new DocumentVersionCache();
        var document = TestDocumentSnapshot.Create("C:/file.cshtml");
        var evictedDocument = TestDocumentSnapshot.Create(document.FilePath);
        cache.TrackDocumentVersion(document, 1337);

        // Act
        var result = cache.TryGetDocumentVersion(evictedDocument, out var version);

        // Assert
        Assert.False(result);
        Assert.Null(version);
    }

    [Fact]
    public void TryGetDocumentVersion_KnownDocument_ReturnsTrue()
    {
        // Arrange
        var cache = new DocumentVersionCache();
        var document = TestDocumentSnapshot.Create("C:/file.cshtml");
        cache.TrackDocumentVersion(document, 1337);

        // Act
        var result = cache.TryGetDocumentVersion(document, out var version);

        // Assert
        Assert.True(result);
        Assert.Equal(1337, version);
    }

    [Fact]
    public async Task ProjectSnapshotManager_KnownDocumentAdded_TracksNewDocument()
    {
        // Arrange
        var cache = new DocumentVersionCache();
        var cacheAccessor = cache.GetTestAccessor();
        var projectSnapshotManager = CreateProjectSnapshotManager();
        cache.Initialize(projectSnapshotManager);

        var project1 = TestProjectSnapshot.Create(
            "C:/path/to/project1.csproj",
            intermediateOutputPath: "C:/path/to/obj1",
            documentFilePaths: [],
            RazorConfiguration.Default,
            projectWorkspaceState: null);

        var document1 = await RunOnDispatcherThreadAsync(() =>
        {
            projectSnapshotManager.ProjectAdded(project1.HostProject);
            return projectSnapshotManager.CreateAndAddDocument(project1, @"C:\path\to\file.razor");
        });

        // Act
        cache.TrackDocumentVersion(document1, 1337);

        // Assert
        var (filePath, entries) = Assert.Single(cacheAccessor.GetEntries());
        Assert.Equal(document1.FilePath, filePath);
        var (actualDocument, actualVersion) = Assert.Single(entries);
        Assert.Same(document1, actualDocument);
        Assert.Equal(1337, actualVersion);

        // Act II
        var project2 = TestProjectSnapshot.Create(
            "C:/path/to/project2.csproj",
            intermediateOutputPath: "C:/path/to/obj2",
            documentFilePaths: [],
            RazorConfiguration.Default,
            projectWorkspaceState: null);

        var document2 = await RunOnDispatcherThreadAsync(() =>
        {
            projectSnapshotManager.ProjectAdded(project2.HostProject);
            projectSnapshotManager.CreateAndAddDocument(project2, @"C:\path\to\file.razor");

            return projectSnapshotManager
                .GetLoadedProject(project2.Key)
                .AssumeNotNull()
                .GetDocument(document1.FilePath);
        });

        // Assert II
        (filePath, entries) = Assert.Single(cacheAccessor.GetEntries());
        Assert.Equal(document1.FilePath, filePath);
        Assert.Equal(2, entries.Length);

        // Should still be tracking document 1 with no changes
        (actualDocument, actualVersion) = entries[0];
        Assert.Same(document1, actualDocument);
        Assert.Equal(1337, actualVersion);

        (actualDocument, actualVersion) = entries[1];
        Assert.Same(document2, actualDocument);
        Assert.Equal(1337, actualVersion);
    }

    private TestProjectSnapshotManager CreateProjectSnapshotManager()
    {
        var result = TestProjectSnapshotManager.Create(ErrorReporter, Dispatcher);
        result.AllowNotifyListeners = true;

        return result;
    }
}
