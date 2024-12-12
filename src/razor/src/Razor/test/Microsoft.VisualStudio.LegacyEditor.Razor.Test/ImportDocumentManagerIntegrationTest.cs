// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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

public class ImportDocumentManagerIntegrationTest : VisualStudioTestBase
{
    private readonly string _directoryPath;
    private readonly string _projectPath;
    private readonly RazorProjectEngine _projectEngine;

    public ImportDocumentManagerIntegrationTest(ITestOutputHelper testOutput)
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
    public void Changed_TrackerChanged_ResultsInChangedHavingCorrectArgs()
    {
        // Arrange
        var testImportsPath = Path.Combine(_directoryPath, "_ViewImports.cshtml");

        var tracker = StrictMock.Of<IVisualStudioDocumentTracker>(t =>
            t.FilePath == Path.Combine(_directoryPath, "Views", "Home", "_ViewImports.cshtml") &&
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
        var fileChangeTracker1Mock = new StrictMock<IFileChangeTracker>();
        fileChangeTracker1Mock
            .Setup(f => f.FilePath)
            .Returns(testImportsPath);
        fileChangeTracker1Mock.Setup(f => f.StartListening()).Verifiable();
        fileChangeTrackerFactoryMock
            .Setup(f => f.Create(testImportsPath))
            .Returns(fileChangeTracker1Mock.Object);

        var fileChangeTracker2Mock = new StrictMock<IFileChangeTracker>();
        fileChangeTracker2Mock.Setup(f => f.StartListening()).Verifiable();

        fileChangeTrackerFactoryMock
            .Setup(f => f.Create(Path.Combine(_directoryPath, "Views", "_ViewImports.cshtml")))
            .Returns(fileChangeTracker2Mock.Object);
        fileChangeTrackerFactoryMock
            .Setup(f => f.Create(Path.Combine(_directoryPath, "Views", "Home", "_ViewImports.cshtml")))
            .Returns(StrictMock.Of<IFileChangeTracker>());

        var called = false;
        var manager = new ImportDocumentManager(fileChangeTrackerFactoryMock.Object);

        manager.OnSubscribed(tracker);

        manager.OnSubscribed(anotherTracker);

        manager.Changed += (sender, args) =>
        {
            called = true;
            Assert.Same(sender, manager);
            Assert.Equal(testImportsPath, args.FilePath);
            Assert.Equal(FileChangeKind.Changed, args.Kind);
            Assert.Collection(
                args.AssociatedDocuments,
                f => Assert.Equal(Path.Combine(_directoryPath, "Views", "Home", "_ViewImports.cshtml"), f),
                f => Assert.Equal(Path.Combine(_directoryPath, "anotherFile.cshtml"), f));
        };

        // Act
        fileChangeTracker1Mock.Raise(t => t.Changed += null, new FileChangeEventArgs(testImportsPath, FileChangeKind.Changed));

        // Assert
        Assert.True(called);
    }
}
