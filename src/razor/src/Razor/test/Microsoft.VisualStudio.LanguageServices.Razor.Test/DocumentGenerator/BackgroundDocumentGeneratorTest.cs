// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Razor.DynamicFiles;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.ProjectSystem;

// These tests are really integration tests. There isn't a good way to unit test this functionality since
// the only thing in here is threading.
public class BackgroundDocumentGeneratorTest(ITestOutputHelper testOutput) : VisualStudioWorkspaceTestBase(testOutput)
{
    private static readonly HostDocument[] s_documents = [TestProjectData.SomeProjectFile1, TestProjectData.AnotherProjectFile1];

    private static readonly HostProject s_hostProject1 = TestProjectData.SomeProject with { Configuration = FallbackRazorConfiguration.MVC_1_0 };
    private static readonly HostProject s_hostProject2 = TestProjectData.AnotherProject with { Configuration = FallbackRazorConfiguration.MVC_1_0 };

    private static IFallbackProjectManager s_fallbackProjectManager = StrictMock.Of<IFallbackProjectManager>(x =>
        x.IsFallbackProject(It.IsAny<ProjectKey>()) == false);

    private readonly TestDynamicFileInfoProvider _dynamicFileInfoProvider = new();

    protected override void ConfigureProjectEngine(RazorProjectEngineBuilder builder)
    {
        builder.SetImportFeature(new TestImportProjectFeature(HierarchicalImports.Legacy));
    }

    [UIFact]
    public async Task ProcessDocument_LongDocumentParse_DoesNotUpdateAfterSuppress()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject1);
        });

        // We utilize a task completion source here so we can "fake" a document parse taking a significant amount of time
        var tcs = new TaskCompletionSource<TextAndVersion>();
        var textLoader = new StrictMock<TextLoader>();
        textLoader
            .Setup(loader => loader.LoadTextAndVersionAsync(It.IsAny<LoadTextOptions>(), It.IsAny<CancellationToken>()))
            .Returns(tcs.Task);
        var hostDocument = s_documents[0];

        var project = projectManager.GetRequiredProject(s_hostProject1.Key);
        using var generator = new TestBackgroundDocumentGenerator(projectManager, s_fallbackProjectManager, _dynamicFileInfoProvider, LoggerFactory)
        {
            NotifyBackgroundWorkStarting = new ManualResetEventSlim(initialState: false)
        };

        // We trigger enqueued notifications via adding/opening to the project manager

        // Act & Assert
        await projectManager.UpdateAsync(updater =>
        {
            updater.AddDocument(s_hostProject1.Key, hostDocument, textLoader.Object);
        });

        generator.NotifyBackgroundWorkStarting.Wait();

        await projectManager.UpdateAsync(updater =>
        {
            updater.OpenDocument(s_hostProject1.Key, hostDocument.FilePath, SourceText.From(string.Empty));
        });

        // Verify document was suppressed because it was opened
        Assert.Null(_dynamicFileInfoProvider.DynamicDocuments[hostDocument.FilePath]);

        // Unblock document processing
        tcs.SetResult(TextAndVersion.Create(SourceText.From(string.Empty), VersionStamp.Default));

        await generator.WaitUntilCurrentBatchCompletesAsync();

        // Validate that even though document parsing took a significant amount of time that the dynamic document wasn't "unsuppressed"
        Assert.Null(_dynamicFileInfoProvider.DynamicDocuments[hostDocument.FilePath]);
    }

    [UIFact]
    public async Task ProcessDocument_SwallowsIOExceptions()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject1);
        });

        var loggerMock = new StrictMock<ILogger>();
        loggerMock
            .Setup(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<string>(), It.IsAny<IOException>()))
            .Throws<InvalidOperationException>(); // If this is thrown, the test fails

        var loggerFactoryMock = new StrictMock<ILoggerFactory>();
        loggerFactoryMock
            .Setup(x => x.GetOrCreateLogger(It.IsAny<string>()))
            .Returns(loggerMock.Object);

        var textLoader = new StrictMock<TextLoader>();
        textLoader
            .Setup(loader => loader.LoadTextAndVersionAsync(It.IsAny<LoadTextOptions>(), It.IsAny<CancellationToken>()))
            .Throws<FileNotFoundException>();

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddDocument(s_hostProject1.Key, s_documents[0], textLoader.Object);
        });

        var documentKey1 = new DocumentKey(s_hostProject1.Key, s_documents[0].FilePath);

        using var generator = new TestBackgroundDocumentGenerator(projectManager, s_fallbackProjectManager, _dynamicFileInfoProvider, loggerFactoryMock.Object);

        // Act & Assert
        generator.EnqueueIfNecessary(documentKey1);

        await generator.WaitUntilCurrentBatchCompletesAsync();
    }

    [UIFact]
    public async Task ProcessDocument_SwallowsUnauthorizedAccessExceptions()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject1);
        });

        var loggerMock = new StrictMock<ILogger>();
        loggerMock
            .Setup(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<string>(), It.IsAny<UnauthorizedAccessException>()))
            .Throws<InvalidOperationException>(); // If this is thrown, the test fails

        var loggerFactoryMock = new StrictMock<ILoggerFactory>();
        loggerFactoryMock
            .Setup(x => x.GetOrCreateLogger(It.IsAny<string>()))
            .Returns(loggerMock.Object);

        var textLoaderMock = new StrictMock<TextLoader>();
        textLoaderMock
            .Setup(loader => loader.LoadTextAndVersionAsync(It.IsAny<LoadTextOptions>(), It.IsAny<CancellationToken>()))
            .Throws<UnauthorizedAccessException>();

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddDocument(s_hostProject1.Key, s_documents[0], textLoaderMock.Object);
        });

        var documentKey1 = new DocumentKey(s_hostProject1.Key, s_documents[0].FilePath);

        using var generator = new TestBackgroundDocumentGenerator(projectManager, s_fallbackProjectManager, _dynamicFileInfoProvider, loggerFactoryMock.Object);

        // Act & Assert
        generator.EnqueueIfNecessary(documentKey1);

        await generator.WaitUntilCurrentBatchCompletesAsync();
    }

    [UIFact]
    public async Task ProcessWorkAndGoBackToSleep()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject1);
            updater.AddProject(s_hostProject2);
            updater.AddDocument(s_hostProject1.Key, s_documents[0], EmptyTextLoader.Instance);
            updater.AddDocument(s_hostProject1.Key, s_documents[1], EmptyTextLoader.Instance);
        });

        var documentKey1 = new DocumentKey(s_hostProject1.Key, s_documents[0].FilePath);

        using var generator = new TestBackgroundDocumentGenerator(projectManager, s_fallbackProjectManager, _dynamicFileInfoProvider, LoggerFactory);

        // Act & Assert

        // Enqueue some work.
        generator.EnqueueIfNecessary(documentKey1);

        // Wait for the work to complete.
        await generator.WaitUntilCurrentBatchCompletesAsync();
        Assert.False(generator.HasPendingWork);
        Assert.Single(generator.CompletedWork, documentKey1);

        await generator.WaitUntilCurrentBatchCompletesAsync();
        Assert.False(generator.HasPendingWork);
        Assert.Single(generator.CompletedWork, documentKey1);
    }

    [UIFact]
    public async Task ProcessWorkAndRestart()
    {
        var hostProject1 = TestProjectData.SomeProject;
        var hostProject2 = TestProjectData.AnotherProject;
        var hostDocument1 = TestProjectData.SomeProjectFile1;
        var hostDocument2 = TestProjectData.SomeProjectFile2;

        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject1);
            updater.AddProject(hostProject2);
            updater.AddDocument(hostProject1.Key, hostDocument1, EmptyTextLoader.Instance);
            updater.AddDocument(hostProject1.Key, hostDocument2, EmptyTextLoader.Instance);
        });

        var documentKey1 = new DocumentKey(hostProject1.Key, hostDocument1.FilePath);
        var documentKey2 = new DocumentKey(hostProject1.Key, hostDocument2.FilePath);

        using var generator = new TestBackgroundDocumentGenerator(projectManager, s_fallbackProjectManager, _dynamicFileInfoProvider, LoggerFactory);

        // Act & Assert

        // First, enqueue some work.
        generator.EnqueueIfNecessary(documentKey1);

        // Wait for the work to complete.
        await generator.WaitUntilCurrentBatchCompletesAsync();

        Assert.False(generator.HasPendingWork);
        Assert.Single(generator.CompletedWork, documentKey1);

        // Enqueue more work.
        generator.EnqueueIfNecessary(documentKey2);

        // Wait for the work to complete.
        await generator.WaitUntilCurrentBatchCompletesAsync();

        Assert.Collection(generator.CompletedWork.OrderBy(key => key.FilePath),
            key => Assert.Equal(documentKey1, key),
            key => Assert.Equal(documentKey2, key));
    }

    [UIFact]
    public async Task UpdateDocumentText_ReparsesRelatedFiles()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        var documents = new[]
        {
            TestProjectData.SomeProjectImportFile,
            TestProjectData.SomeProjectComponentFile1,
        };

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject1);
            for (var i = 0; i < documents.Length; i++)
            {
                updater.AddDocument(s_hostProject1.Key, documents[i], EmptyTextLoader.Instance);
            }
        });

        using var generator = new TestBackgroundDocumentGenerator(projectManager, s_fallbackProjectManager, _dynamicFileInfoProvider, LoggerFactory)
        {
            BlockBatchProcessing = true
        };

        var changedSourceText = SourceText.From("@inject DateTime Time");

        // Act & Assert
        await projectManager.UpdateAsync(updater =>
        {
            updater.UpdateDocumentText(s_hostProject1.Key, TestProjectData.SomeProjectImportFile.FilePath, changedSourceText);
        });

        Assert.True(generator.HasPendingWork);

        Assert.Collection(generator.PendingWork.OrderBy(key => key.FilePath),
            key => Assert.Equal(new(s_hostProject1.Key, documents[0].FilePath), key),
            key => Assert.Equal(new(s_hostProject1.Key, documents[1].FilePath), key));

        // Allow the background work to start.
        generator.UnblockBatchProcessing();

        await generator.WaitUntilCurrentBatchCompletesAsync();

        Assert.False(generator.HasPendingWork);

        Assert.Collection(generator.CompletedWork.OrderBy(key => key.FilePath),
            key => Assert.Equal(new(s_hostProject1.Key, documents[0].FilePath), key),
            key => Assert.Equal(new(s_hostProject1.Key, documents[1].FilePath), key));
    }

    [UIFact]
    public async Task RemoveDocument_ReparsesRelatedFiles()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject1);
            updater.AddDocument(s_hostProject1.Key, TestProjectData.SomeProjectComponentFile1, EmptyTextLoader.Instance);
            updater.AddDocument(s_hostProject1.Key, TestProjectData.SomeProjectImportFile, EmptyTextLoader.Instance);
        });

        using var generator = new TestBackgroundDocumentGenerator(projectManager, s_fallbackProjectManager, _dynamicFileInfoProvider, LoggerFactory)
        {
            BlockBatchProcessing = true
        };

        // Act & Assert
        await projectManager.UpdateAsync(updater =>
        {
            updater.RemoveDocument(s_hostProject1.Key, TestProjectData.SomeProjectImportFile.FilePath);
        });

        Assert.True(generator.HasPendingWork, "Queue should have a notification created during Enqueue");

        var expectedKey = new DocumentKey(s_hostProject1.Key, TestProjectData.SomeProjectComponentFile1.FilePath);
        Assert.Single(generator.PendingWork, expectedKey);

        // Allow the background work to start.
        generator.UnblockBatchProcessing();

        await generator.WaitUntilCurrentBatchCompletesAsync();

        Assert.Single(generator.CompletedWork, expectedKey);
    }

    private class TestBackgroundDocumentGenerator(
        ProjectSnapshotManager projectManager,
        IFallbackProjectManager fallbackProjectManager,
        IRazorDynamicFileInfoProviderInternal dynamicFileInfoProvider,
        ILoggerFactory loggerFactory)
        : BackgroundDocumentGenerator(projectManager, fallbackProjectManager, dynamicFileInfoProvider, loggerFactory, delay: TimeSpan.FromMilliseconds(1))
    {
        public readonly List<DocumentKey> PendingWork = [];
        public readonly List<DocumentKey> CompletedWork = [];

        public ManualResetEventSlim? NotifyBackgroundWorkStarting { get; set; }

        private ManualResetEventSlim? _blockBatchProcessingSource;

        public bool HasPendingWork => PendingWork.Count > 0;

        [MemberNotNullWhen(true, nameof(_blockBatchProcessingSource))]
        public bool BlockBatchProcessing
        {
            get => _blockBatchProcessingSource is not null;

            init
            {
                _blockBatchProcessingSource = new ManualResetEventSlim(initialState: false);
            }
        }

        public new Task WaitUntilCurrentBatchCompletesAsync()
            => base.WaitUntilCurrentBatchCompletesAsync();

        public void UnblockBatchProcessing()
        {
            Assert.True(BlockBatchProcessing);
            _blockBatchProcessingSource.Set();
        }

        protected override async ValueTask ProcessBatchAsync(ImmutableArray<DocumentKey> items, CancellationToken token)
        {
            if (_blockBatchProcessingSource is { } blockEvent)
            {
                blockEvent.Wait();
                blockEvent.Reset();
            }

            if (NotifyBackgroundWorkStarting is { } resetEvent)
            {
                resetEvent.Set();
            }

            await base.ProcessBatchAsync(items, token);
        }

        public override void EnqueueIfNecessary(DocumentKey documentKey)
        {
            PendingWork.Add(documentKey);

            base.EnqueueIfNecessary(documentKey);
        }

        protected override Task ProcessDocumentAsync(DocumentSnapshot document, CancellationToken cancellationToken)
        {
            var key = document.Key;
            PendingWork.Remove(key);

            var task = base.ProcessDocumentAsync(document, cancellationToken);

            CompletedWork.Add(key);

            return task;
        }
    }

    private class TestDynamicFileInfoProvider : IRazorDynamicFileInfoProviderInternal
    {
        private readonly Dictionary<string, IDynamicDocumentContainer?> _dynamicDocuments;

        public TestDynamicFileInfoProvider()
        {
            _dynamicDocuments = [];
        }

        public IReadOnlyDictionary<string, IDynamicDocumentContainer?> DynamicDocuments => _dynamicDocuments;

        public void SuppressDocument(DocumentKey documentKey)
        {
            _dynamicDocuments[documentKey.FilePath] = null;
        }

        public void UpdateFileInfo(ProjectKey projectKey, IDynamicDocumentContainer documentContainer)
        {
            _dynamicDocuments[documentContainer.FilePath] = documentContainer;
        }

        public void UpdateLSPFileInfo(Uri documentUri, IDynamicDocumentContainer documentContainer)
        {
            _dynamicDocuments[documentContainer.FilePath] = documentContainer;
        }
    }
}
