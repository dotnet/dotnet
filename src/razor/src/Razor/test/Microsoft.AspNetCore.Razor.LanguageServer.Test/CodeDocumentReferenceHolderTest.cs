// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;
using System.Threading;
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

public class CodeDocumentReferenceHolderTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    private static readonly HostProject s_hostProject = new(
        filePath: "C:/path/to/project.csproj",
        intermediateOutputPath: "C:/path/to/obj",
        configuration: RazorConfiguration.Default,
        rootNamespace: "TestNamespace");

    private static readonly HostDocument s_hostDocument = new("C:/path/to/file.razor", "file.razor");

#nullable disable
    private TestProjectSnapshotManager _projectManager;
    private CodeDocumentReferenceHolder _referenceHolder;
#nullable enable

    protected override Task InitializeAsync()
    {
        _projectManager = CreateProjectSnapshotManager();
        _referenceHolder = new CodeDocumentReferenceHolder(_projectManager);

        return Task.CompletedTask;
    }

    [Fact]
    public async Task DocumentProcessed_ReferencesGeneratedCodeDocument()
    {
        // Arrange
        var documentSnapshot = await CreateDocumentSnapshotAsync();
        var codeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(documentSnapshot, DisposalToken);

        // Act
        PerformFullGC();

        // Assert
        Assert.True(codeDocumentReference.TryGetTarget(out _));
    }

    [Fact]
    public async Task UpdateUnrelatedDocumentText_ReferencesGeneratedCodeDocument()
    {
        // Arrange
        var documentSnapshot = await CreateDocumentSnapshotAsync();
        var unrelatedHostDocument = new HostDocument("C:/path/to/otherfile.razor", "otherfile.razor");

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddDocument(s_hostProject.Key, unrelatedHostDocument, TestMocks.CreateTextLoader("<p>Unrelated</p>"));
        });

            var unrelatedDocumentSnapshot = _projectManager.GetRequiredDocument(s_hostProject.Key, unrelatedHostDocument.FilePath);

        var mainCodeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(documentSnapshot, DisposalToken);
        var unrelatedCodeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(unrelatedDocumentSnapshot, DisposalToken);

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.UpdateDocumentText(s_hostProject.Key, unrelatedHostDocument.FilePath, SourceText.From(string.Empty));
        });

        PerformFullGC();

        // Assert
        Assert.True(mainCodeDocumentReference.TryGetTarget(out _));
        Assert.False(unrelatedCodeDocumentReference.TryGetTarget(out _));
    }

    [Fact]
    public async Task UpdateDocumentText_DereferencesGeneratedCodeDocument()
    {
        // Arrange
        var documentSnapshot = await CreateDocumentSnapshotAsync();
        var codeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(documentSnapshot, DisposalToken);

        // Act

        await _projectManager.UpdateAsync(updater =>
        {
            updater.UpdateDocumentText(s_hostProject.Key, s_hostDocument.FilePath, SourceText.From(string.Empty));
        });

        PerformFullGC();

        // Assert
        Assert.False(codeDocumentReference.TryGetTarget(out _));
    }

    [Fact]
    public async Task RemoveDocument_DereferencesGeneratedCodeDocument()
    {
        // Arrange
        var documentSnapshot = await CreateDocumentSnapshotAsync();
        var codeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(documentSnapshot, DisposalToken);

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.RemoveDocument(s_hostProject.Key, s_hostDocument.FilePath);
        });

        PerformFullGC();

        // Assert
        Assert.False(codeDocumentReference.TryGetTarget(out _));
    }

    [Fact]
    public async Task UpdateProjectConfiguration_DereferencesGeneratedCodeDocument()
    {
        // Arrange
        var documentSnapshot = await CreateDocumentSnapshotAsync();
        var codeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(documentSnapshot, DisposalToken);

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.UpdateProjectConfiguration(s_hostProject with { Configuration = RazorConfiguration.Default, RootNamespace = "NewRootNamespace" });
        });

        PerformFullGC();

        // Assert
        Assert.False(codeDocumentReference.TryGetTarget(out _));
    }

    [Fact]
    public async Task RemoveProject_DereferencesGeneratedCodeDocument()
    {
        // Arrange
        var documentSnapshot = await CreateDocumentSnapshotAsync();
        var codeDocumentReference = await ProcessDocumentAndRetrieveOutputAsync(documentSnapshot, DisposalToken);

        // Act
        await _projectManager.UpdateAsync(updater =>
        {
            updater.RemoveProject(s_hostProject.Key);
        });

        PerformFullGC();

        // Assert
        Assert.False(codeDocumentReference.TryGetTarget(out _));
    }

    private Task<IDocumentSnapshot> CreateDocumentSnapshotAsync()
    {
        return _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
            updater.AddDocument(s_hostProject.Key, s_hostDocument, TestMocks.CreateTextLoader("<p>Hello World</p>"));

            return updater.GetRequiredDocument(s_hostProject.Key, s_hostDocument.FilePath);
        });
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private async Task<WeakReference<RazorCodeDocument>> ProcessDocumentAndRetrieveOutputAsync(IDocumentSnapshot documentSnapshot, CancellationToken cancellationToken)
    {
        var codeDocument = await documentSnapshot.GetGeneratedOutputAsync(cancellationToken);

        _referenceHolder.DocumentProcessed(codeDocument, documentSnapshot);

        return new(codeDocument);
    }

    private static void PerformFullGC()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }
}
