// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test;

public class DocumentContextFactoryTest : LanguageServerTestBase
{
    private static readonly string s_baseDirectory = TestPathUtilities.CreateRootedPath("path", "to");

    private readonly TestProjectSnapshotManager _projectManager;

    public DocumentContextFactoryTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _projectManager = CreateProjectSnapshotManager();
    }

    [Fact]
    public void TryCreateAsync_CanNotResolveDocument_ReturnsNull()
    {
        // Arrange
        var filePath = FilePathNormalizer.Normalize(Path.Combine(s_baseDirectory, "file.cshtml"));
        var uri = new Uri(filePath);

        var factory = new DocumentContextFactory(_projectManager, LoggerFactory);

        // Act
        Assert.False(factory.TryCreate(uri, out _));
    }

    [Fact]
    public void TryCreateForOpenDocumentAsync_CanNotResolveDocument_ReturnsNull()
    {
        // Arrange
        var filePath = FilePathNormalizer.Normalize(Path.Combine(s_baseDirectory, "file.cshtml"));
        var uri = new Uri(filePath);

        var factory = new DocumentContextFactory(_projectManager, LoggerFactory);

        // Act
        Assert.False(factory.TryCreate(uri, out _));
    }

    [Fact]
    public async Task TryCreateAsync_ResolvesContent()
    {
        // Arrange
        var filePath = FilePathNormalizer.Normalize(Path.Combine(s_baseDirectory, "file.cshtml"));
        var uri = new Uri(filePath);

        var hostDocument = new HostDocument(filePath, "file.cshtml");

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddDocument(MiscFilesProject.Key, hostDocument, EmptyTextLoader.Instance);
        });

        var documentSnapshot = _projectManager
            .GetMiscellaneousProject()
            .GetRequiredDocument(filePath);

        var factory = new DocumentContextFactory(_projectManager, LoggerFactory);

        // Act
        Assert.True(factory.TryCreate(uri, out var documentContext));

        // Assert
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

        var factory = new DocumentContextFactory(_projectManager, LoggerFactory);

        var hostProject = new HostProject(projectFilePath, intermediateOutputPath, RazorConfiguration.Default, rootNamespace: null);
        var hostDocument = new HostDocument(filePath, "file.cshtml");

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject);
            updater.AddDocument(hostProject.Key, hostDocument, EmptyTextLoader.Instance);
        });

        // Act
        Assert.True(factory.TryCreate(uri, new VisualStudio.LanguageServer.Protocol.VSProjectContext { Id = hostProject.Key.Id }, out var documentContext));

        // Assert
        Assert.Equal(uri, documentContext.Uri);
    }

    [Fact]
    public async Task TryCreateForOpenDocumentAsync_ResolvesContent()
    {
        // Arrange
        var filePath = FilePathNormalizer.Normalize(Path.Combine(s_baseDirectory, "file.cshtml"));
        var uri = new Uri(filePath);

        var hostDocument = new HostDocument(filePath, "file.cshtml");

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddDocument(MiscFilesProject.Key, hostDocument, EmptyTextLoader.Instance);
        });

        var documentSnapshot = _projectManager
            .GetMiscellaneousProject()
            .GetRequiredDocument(filePath);

        var factory = new DocumentContextFactory(_projectManager, LoggerFactory);

        // Act
        Assert.True(factory.TryCreate(uri, out var documentContext));

        // Assert
        Assert.Equal(1, documentContext.Snapshot.Version);
        Assert.Equal(uri, documentContext.Uri);
        Assert.Same(documentSnapshot, documentContext.Snapshot);
    }
}
