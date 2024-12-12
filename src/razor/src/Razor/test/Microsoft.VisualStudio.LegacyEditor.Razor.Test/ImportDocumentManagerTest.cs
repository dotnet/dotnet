// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Razor.Documents;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

public class ImportDocumentManagerTest : VisualStudioTestBase
{
    private readonly string _projectPath;
    private readonly string _directoryPath;
    private readonly RazorProjectEngine _projectEngine;

    public ImportDocumentManagerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _projectPath = TestProjectData.SomeProject.FilePath;
        _directoryPath = Path.GetDirectoryName(_projectPath);

        var fileSystem = RazorProjectFileSystem.Create(Path.GetDirectoryName(_projectPath));

        // These tests rely on MVC's import behavior.
        _projectEngine = RazorProjectEngine.Create(
            FallbackRazorConfiguration.MVC_2_1,
            fileSystem,
            AspNetCore.Mvc.Razor.Extensions.RazorExtensions.Register);
    }

    [UIFact]
    public void OnSubscribed_StartsFileChangeTrackers()
    {
        // Arrange
        var tracker = StrictMock.Of<IVisualStudioDocumentTracker>(t =>
            t.FilePath == Path.Combine(_directoryPath, "Views", "Home", "file.cshtml") &&
            t.ProjectPath == _projectPath &&
            t.ProjectSnapshot == StrictMock.Of<IProjectSnapshot>(p =>
                p.GetProjectEngine() == _projectEngine &&
                p.TryGetDocument(It.IsAny<string>(), out It.Ref<IDocumentSnapshot?>.IsAny) == false));

        var fileChangeTrackerFactoryMock = new StrictMock<IFileChangeTrackerFactory>();
        var fileChangeTracker1Mock = new StrictMock<IFileChangeTracker>();
        fileChangeTracker1Mock
            .Setup(f => f.StartListening())
            .Verifiable();
        fileChangeTrackerFactoryMock
            .Setup(f => f.Create(Path.Combine(_directoryPath, "Views", "Home", "_ViewImports.cshtml")))
            .Returns(fileChangeTracker1Mock.Object)
            .Verifiable();
        var fileChangeTracker2Mock = new StrictMock<IFileChangeTracker>();
        fileChangeTracker2Mock
            .Setup(f => f.StartListening())
            .Verifiable();
        fileChangeTrackerFactoryMock
            .Setup(f => f.Create(Path.Combine(_directoryPath, "Views", "_ViewImports.cshtml")))
            .Returns(fileChangeTracker2Mock.Object)
            .Verifiable();
        var fileChangeTracker3Mock = new StrictMock<IFileChangeTracker>();
        fileChangeTracker3Mock.Setup(f => f.StartListening()).Verifiable();
        fileChangeTrackerFactoryMock
            .Setup(f => f.Create(Path.Combine(_directoryPath, "_ViewImports.cshtml")))
            .Returns(fileChangeTracker3Mock.Object)
            .Verifiable();

        var manager = new ImportDocumentManager(fileChangeTrackerFactoryMock.Object);

        // Act
        manager.OnSubscribed(tracker);

        // Assert
        fileChangeTrackerFactoryMock.Verify();
        fileChangeTracker1Mock.Verify();
        fileChangeTracker2Mock.Verify();
        fileChangeTracker3Mock.Verify();
    }

    [UIFact]
    public void OnSubscribed_AlreadySubscribed_DoesNothing()
    {
        // Arrange
        var tracker = StrictMock.Of<IVisualStudioDocumentTracker>(t =>
            t.FilePath == Path.Combine(_directoryPath, "file.cshtml") &&
            t.ProjectPath == _projectPath &&
            t.ProjectSnapshot == StrictMock.Of<IProjectSnapshot>(p =>
                p.GetProjectEngine() == _projectEngine &&
                p.TryGetDocument(It.IsAny<string>(), out It.Ref<IDocumentSnapshot?>.IsAny) == false));

        var anotherTracker = StrictMock.Of<IVisualStudioDocumentTracker>(t =>
            t.FilePath == Path.Combine(_directoryPath, "anotherFile.cshtml") &&
            t.ProjectPath == _projectPath &&
            t.ProjectSnapshot == StrictMock.Of<IProjectSnapshot>(p =>
                p.GetProjectEngine() == _projectEngine &&
                p.TryGetDocument(It.IsAny<string>(), out It.Ref<IDocumentSnapshot?>.IsAny) == false));

        var callCount = 0;
        var fileChangeTrackerFactoryMock = new StrictMock<IFileChangeTrackerFactory>();
        var fileChangeTrackerMock = new StrictMock<IFileChangeTracker>();
        fileChangeTrackerMock.Setup(t => t.StartListening()).Verifiable();
        fileChangeTrackerFactoryMock
            .Setup(f => f.Create(It.IsAny<string>()))
            .Returns(fileChangeTrackerMock.Object)
            .Callback(() => callCount++);

        var manager = new ImportDocumentManager(fileChangeTrackerFactoryMock.Object);

        manager.OnSubscribed(tracker); // Start tracking the import.

        // Act
        manager.OnSubscribed(anotherTracker);

        // Assert
        Assert.Equal(1, callCount);
    }

    [UIFact]
    public void OnUnsubscribed_StopsFileChangeTracker()
    {
        // Arrange
        var tracker = StrictMock.Of<IVisualStudioDocumentTracker>(t =>
            t.FilePath == Path.Combine(_directoryPath, "file.cshtml") &&
            t.ProjectPath == _projectPath &&
            t.ProjectSnapshot == StrictMock.Of<IProjectSnapshot>(p =>
                p.GetProjectEngine() == _projectEngine &&
                p.TryGetDocument(It.IsAny<string>(), out It.Ref<IDocumentSnapshot?>.IsAny) == false));

        var fileChangeTrackerFactoryMock = new StrictMock<IFileChangeTrackerFactory>();
        var fileChangeTrackerMock = new StrictMock<IFileChangeTracker>();
        fileChangeTrackerMock.Setup(f => f.StartListening()).Verifiable();
        fileChangeTrackerMock
            .Setup(f => f.StopListening())
            .Verifiable();
        fileChangeTrackerFactoryMock
            .Setup(f => f.Create(Path.Combine(_directoryPath, "_ViewImports.cshtml")))
            .Returns(fileChangeTrackerMock.Object)
            .Verifiable();

        var manager = new ImportDocumentManager(fileChangeTrackerFactoryMock.Object);

        manager.OnSubscribed(tracker); // Start tracking the import.

        // Act
        manager.OnUnsubscribed(tracker);

        // Assert
        fileChangeTrackerFactoryMock.Verify();
        fileChangeTrackerMock.Verify();
    }

    [UIFact]
    public void OnUnsubscribed_AnotherDocumentTrackingImport_DoesNotStopFileChangeTracker()
    {
        // Arrange
        var tracker = StrictMock.Of<IVisualStudioDocumentTracker>(t =>
            t.FilePath == Path.Combine(_directoryPath, "file.cshtml") &&
            t.ProjectPath == _projectPath &&
            t.ProjectSnapshot == StrictMock.Of<IProjectSnapshot>(p =>
                p.GetProjectEngine() == _projectEngine &&
                p.TryGetDocument(It.IsAny<string>(), out It.Ref<IDocumentSnapshot?>.IsAny) == false));

        var anotherTracker = StrictMock.Of<IVisualStudioDocumentTracker>(t =>
            t.FilePath == Path.Combine(_directoryPath, "anotherFile.cshtml") &&
            t.ProjectPath == _projectPath &&
            t.ProjectSnapshot == StrictMock.Of<IProjectSnapshot>(p =>
                p.GetProjectEngine() == _projectEngine &&
                p.TryGetDocument(It.IsAny<string>(), out It.Ref<IDocumentSnapshot?>.IsAny) == false));

        var fileChangeTrackerFactoryMock = new StrictMock<IFileChangeTrackerFactory>();
        var fileChangeTrackerMock = new StrictMock<IFileChangeTracker>();
        fileChangeTrackerMock.Setup(f => f.StartListening()).Verifiable();
        fileChangeTrackerMock
            .Setup(f => f.StopListening())
            .Throws(new InvalidOperationException());
        fileChangeTrackerFactoryMock
            .Setup(f => f.Create(It.IsAny<string>()))
            .Returns(fileChangeTrackerMock.Object);

        var manager = new ImportDocumentManager(fileChangeTrackerFactoryMock.Object);

        manager.OnSubscribed(tracker); // Starts tracking import for the first document.

        manager.OnSubscribed(anotherTracker); // Starts tracking import for the second document.

        // Act & Assert (Does not throw)
        manager.OnUnsubscribed(tracker);
        manager.OnUnsubscribed(tracker);
    }
}
