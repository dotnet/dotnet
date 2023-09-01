// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class CodeDocumentReferenceHolderTest : LanguageServerTestBase
{
    private readonly TestProjectSnapshotManager _projectManager;
    private readonly CodeDocumentReferenceHolder _referenceHolder;
    private readonly HostProject _hostProject;
    private readonly HostDocument _hostDocument;

    public CodeDocumentReferenceHolderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _projectManager = TestProjectSnapshotManager.Create(ErrorReporter, Dispatcher);
        _projectManager.AllowNotifyListeners = true;
        _referenceHolder = new CodeDocumentReferenceHolder();
        _referenceHolder.Initialize(_projectManager);

        _hostProject = new HostProject("C:/path/to/project.csproj", "C:/path/to/obj", RazorConfiguration.Default, rootNamespace: "TestNamespace");
        _hostDocument = new HostDocument("C:/path/to/file.razor", "file.razor");
    }

    [Fact]
    public async Task DocumentProcessed_ReferencesGeneratedCodeDocument()
    {
        // Arrange
        var documentSnapshot = await CreateDocumentSnapshotAsync(DisposalToken);
        var codeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(documentSnapshot, DisposalToken);

        // Act
        GC.Collect();

        // Assert
        Assert.True(codeDocumentReference.TryGetTarget(out _));
    }

    [Fact]
    public async Task UnrelatedDocumentChanged_ReferencesGeneratedCodeDocument()
    {
        // Arrange
        var documentSnapshot = await CreateDocumentSnapshotAsync(DisposalToken);
        var unrelatedHostDocument = new HostDocument("C:/path/to/otherfile.razor", "otherfile.razor");
        var unrelatedDocumentSnapshot = await Dispatcher.RunOnDispatcherThreadAsync(() =>
        {
            var unrelatedTextLoader = new SourceTextLoader("<p>Unrelated</p>", unrelatedHostDocument.FilePath);
            _projectManager.DocumentAdded(_hostProject.Key, unrelatedHostDocument, unrelatedTextLoader);
            var project = _projectManager.GetLoadedProject(_hostProject.Key);
            var document = project?.GetDocument(unrelatedHostDocument.FilePath);
            return document;
        }, DisposalToken);

        Assert.NotNull(unrelatedDocumentSnapshot);

        var mainCodeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(documentSnapshot, DisposalToken);
        var unrelatedCodeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(unrelatedDocumentSnapshot, DisposalToken);

        // Act
        await Dispatcher.RunOnDispatcherThreadAsync(() =>
        {
            _projectManager.DocumentChanged(_hostProject.Key, unrelatedHostDocument.FilePath, SourceText.From(string.Empty));
        }, DisposalToken);

        GC.Collect();

        // Assert
        Assert.True(mainCodeDocumentReference.TryGetTarget(out _));
        Assert.False(unrelatedCodeDocumentReference.TryGetTarget(out _));
    }

    [Fact]
    public async Task DocumentChanged_DereferencesGeneratedCodeDocument()
    {
        // Arrange
        var documentSnapshot = await CreateDocumentSnapshotAsync(DisposalToken);
        var codeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(documentSnapshot, DisposalToken);

        // Act

        await Dispatcher.RunOnDispatcherThreadAsync(() =>
        {
            _projectManager.DocumentChanged(_hostProject.Key, _hostDocument.FilePath, SourceText.From(string.Empty));
        }, DisposalToken);

        GC.Collect();

        // Assert
        Assert.False(codeDocumentReference.TryGetTarget(out _));
    }

    [Fact]
    public async Task DocumentRemoved_DereferencesGeneratedCodeDocument()
    {
        // Arrange
        var documentSnapshot = await CreateDocumentSnapshotAsync(DisposalToken);
        var codeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(documentSnapshot, DisposalToken);

        // Act
        await Dispatcher.RunOnDispatcherThreadAsync(() =>
        {
            _projectManager.DocumentRemoved(_hostProject.Key, _hostDocument);
        }, DisposalToken);

        GC.Collect();

        // Assert
        Assert.False(codeDocumentReference.TryGetTarget(out _));
    }

    [Fact]
    public async Task ProjectChanged_DereferencesGeneratedCodeDocument()
    {
        // Arrange
        var documentSnapshot = await CreateDocumentSnapshotAsync(DisposalToken);
        var codeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(documentSnapshot, DisposalToken);

        // Act
        await Dispatcher.RunOnDispatcherThreadAsync(() =>
        {
            _projectManager.ProjectConfigurationChanged(new HostProject(_hostProject.FilePath, _hostProject.IntermediateOutputPath, RazorConfiguration.Default, rootNamespace: "NewRootNamespace"));
        }, DisposalToken);

        GC.Collect();

        // Assert
        Assert.False(codeDocumentReference.TryGetTarget(out _));
    }

    [Fact]
    public async Task ProjectRemoved_DereferencesGeneratedCodeDocument()
    {
        // Arrange
        var documentSnapshot = await CreateDocumentSnapshotAsync(DisposalToken);
        var codeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(documentSnapshot, DisposalToken);

        // Act
        await Dispatcher.RunOnDispatcherThreadAsync(() =>
        {
            _projectManager.ProjectRemoved(_hostProject.Key);
        }, DisposalToken);

        GC.Collect();

        // Assert
        Assert.False(codeDocumentReference.TryGetTarget(out _));
    }

    private Task<IDocumentSnapshot> CreateDocumentSnapshotAsync(CancellationToken cancellationToken)
    {
        return Dispatcher.RunOnDispatcherThreadAsync(() =>
        {
            _projectManager.ProjectAdded(_hostProject);
            var textLoader = new SourceTextLoader("<p>Hello World</p>", _hostDocument.FilePath);
            _projectManager.DocumentAdded(_hostProject.Key, _hostDocument, textLoader);
            var project = _projectManager.GetLoadedProject(_hostProject.Key).AssumeNotNull();
            return project.GetDocument(_hostDocument.FilePath).AssumeNotNull();
        }, cancellationToken);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private async Task<WeakReference<RazorCodeDocument>> ProcessDocumentAndRetrieveOutputAsync(IDocumentSnapshot documentSnapshot, CancellationToken cancellationToken)
    {
        var codeDocument = await documentSnapshot.GetGeneratedOutputAsync();
        await Dispatcher.RunOnDispatcherThreadAsync(() =>
        {
            _referenceHolder.DocumentProcessed(codeDocument, documentSnapshot);
        }, cancellationToken);
        var codeDocumentReference = new WeakReference<RazorCodeDocument>(codeDocument);

        return codeDocumentReference;
    }

    private sealed class SourceTextLoader : TextLoader
    {
        private readonly SourceText _sourceText;
        private readonly string _filePath;

        public SourceTextLoader(string content, string filePath)
        {
            _sourceText = SourceText.From(content);
            _filePath = filePath;
        }

        public override Task<TextAndVersion> LoadTextAndVersionAsync(
            LoadTextOptions options, CancellationToken cancellationToken)
        {
            return Task.FromResult(TextAndVersion.Create(_sourceText, VersionStamp.Default, _filePath));
        }
    }
}
