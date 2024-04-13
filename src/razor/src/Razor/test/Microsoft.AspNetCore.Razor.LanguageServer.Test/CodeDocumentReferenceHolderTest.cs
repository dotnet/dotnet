// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class CodeDocumentReferenceHolderTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    private static readonly HostProject s_hostProject = new(
        projectFilePath: "C:/path/to/project.csproj",
        intermediateOutputPath: "C:/path/to/obj",
        razorConfiguration: RazorConfiguration.Default,
        rootNamespace: "TestNamespace");

    private static readonly HostDocument s_hostDocument = new("C:/path/to/file.razor", "file.razor");

#nullable disable
    private TestProjectSnapshotManager _projectManager;
    private CodeDocumentReferenceHolder _referenceHolder;
#nullable enable

    protected override Task InitializeAsync()
    {
        _projectManager = CreateProjectSnapshotManager();
        _referenceHolder = new CodeDocumentReferenceHolder();
        _referenceHolder.Initialize(_projectManager);

        return Task.CompletedTask;
    }

    [Fact]
    public async Task DocumentProcessed_ReferencesGeneratedCodeDocument()
    {
        // Arrange
        var documentSnapshot = await CreateDocumentSnapshotAsync();
        var codeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(documentSnapshot);

        // Act
        PerformFullGC();

        // Assert
        Assert.True(codeDocumentReference.TryGetTarget(out _));
    }

    [Fact]
    public async Task UnrelatedDocumentChanged_ReferencesGeneratedCodeDocument()
    {
        // Arrange
        var documentSnapshot = await CreateDocumentSnapshotAsync();
        var unrelatedHostDocument = new HostDocument("C:/path/to/otherfile.razor", "otherfile.razor");
        var unrelatedDocumentSnapshot = await _projectManager.UpdateAsync(updater =>
        {
            var unrelatedTextLoader = new SourceTextLoader("<p>Unrelated</p>", unrelatedHostDocument.FilePath);
            updater.DocumentAdded(s_hostProject.Key, unrelatedHostDocument, unrelatedTextLoader);
            var project = updater.GetLoadedProject(s_hostProject.Key);

            return project.GetDocument(unrelatedHostDocument.FilePath);
        });

        Assert.NotNull(unrelatedDocumentSnapshot);

        var mainCodeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(documentSnapshot);
        var unrelatedCodeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(unrelatedDocumentSnapshot);

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.DocumentChanged(s_hostProject.Key, unrelatedHostDocument.FilePath, SourceText.From(string.Empty));
        });

        PerformFullGC();

        // Assert
        Assert.True(mainCodeDocumentReference.TryGetTarget(out _));
        Assert.False(unrelatedCodeDocumentReference.TryGetTarget(out _));
    }

    [Fact]
    public async Task DocumentChanged_DereferencesGeneratedCodeDocument()
    {
        // Arrange
        var documentSnapshot = await CreateDocumentSnapshotAsync();
        var codeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(documentSnapshot);

        // Act

        await _projectManager.UpdateAsync(updater =>
        {
            updater.DocumentChanged(s_hostProject.Key, s_hostDocument.FilePath, SourceText.From(string.Empty));
        });

        PerformFullGC();

        // Assert
        Assert.False(codeDocumentReference.TryGetTarget(out _));
    }

    [Fact]
    public async Task DocumentRemoved_DereferencesGeneratedCodeDocument()
    {
        // Arrange
        var documentSnapshot = await CreateDocumentSnapshotAsync();
        var codeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(documentSnapshot);

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.DocumentRemoved(s_hostProject.Key, s_hostDocument);
        });

        PerformFullGC();

        // Assert
        Assert.False(codeDocumentReference.TryGetTarget(out _));
    }

    [Fact]
    public async Task ProjectChanged_DereferencesGeneratedCodeDocument()
    {
        // Arrange
        var documentSnapshot = await CreateDocumentSnapshotAsync();
        var codeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(documentSnapshot);

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectConfigurationChanged(new HostProject(s_hostProject.FilePath, s_hostProject.IntermediateOutputPath, RazorConfiguration.Default, rootNamespace: "NewRootNamespace"));
        });

        PerformFullGC();

        // Assert
        Assert.False(codeDocumentReference.TryGetTarget(out _));
    }

    [Fact]
    public async Task ProjectRemoved_DereferencesGeneratedCodeDocument()
    {
        // Arrange
        var documentSnapshot = await CreateDocumentSnapshotAsync();
        var codeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(documentSnapshot);

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectRemoved(s_hostProject.Key);
        });

        PerformFullGC();

        // Assert
        Assert.False(codeDocumentReference.TryGetTarget(out _));
    }

    private Task<IDocumentSnapshot> CreateDocumentSnapshotAsync()
    {
        return _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(s_hostProject);
            var textLoader = new SourceTextLoader("<p>Hello World</p>", s_hostDocument.FilePath);
            updater.DocumentAdded(s_hostProject.Key, s_hostDocument, textLoader);
            var project = updater.GetLoadedProject(s_hostProject.Key);
            return project.GetDocument(s_hostDocument.FilePath).AssumeNotNull();
        });
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private async Task<WeakReference<RazorCodeDocument>> ProcessDocumentAndRetrieveOutputAsync(IDocumentSnapshot documentSnapshot)
    {
        var codeDocument = await documentSnapshot.GetGeneratedOutputAsync();

        await RunOnDispatcherAsync(() =>
        {
            _referenceHolder.DocumentProcessed(codeDocument, documentSnapshot);
        });

        return new(codeDocument);
    }

    private static void PerformFullGC()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }

    private sealed class SourceTextLoader(string content, string filePath) : TextLoader
    {
        public override Task<TextAndVersion> LoadTextAndVersionAsync(
            LoadTextOptions options, CancellationToken cancellationToken)
            => Task.FromResult(
                TextAndVersion.Create(
                    SourceText.From(content),
                    VersionStamp.Default,
                    filePath));
    }
}
