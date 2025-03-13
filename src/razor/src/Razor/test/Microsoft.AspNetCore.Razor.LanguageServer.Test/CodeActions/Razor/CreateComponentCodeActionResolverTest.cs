// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class CreateComponentCodeActionResolverTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task Handle_Unsupported()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = $"@page \"/test\"";
        var codeDocument = CreateCodeDocument(contents);
        codeDocument.SetUnsupported();

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var resolver = new CreateComponentCodeActionResolver(TestLanguageServerFeatureOptions.Instance);
        var data = JsonSerializer.SerializeToElement(new CreateComponentCodeActionParams()
        {
            Path = "c:/Another.razor",
        });

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.Null(workspaceEdit);
    }

    [Fact]
    public async Task Handle_InvalidFileKind()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = $"@page \"/test\"";
        var codeDocument = CreateCodeDocument(contents, fileKind: FileKinds.Legacy);

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var resolver = new CreateComponentCodeActionResolver(TestLanguageServerFeatureOptions.Instance);
        var data = JsonSerializer.SerializeToElement(new CreateComponentCodeActionParams()
        {
            Path = "c:/Another.razor",
        });

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.Null(workspaceEdit);
    }

    [Fact]
    public async Task Handle_CreateComponent()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = $"@page \"/test\"";
        var codeDocument = CreateCodeDocument(contents);

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var resolver = new CreateComponentCodeActionResolver(TestLanguageServerFeatureOptions.Instance);
        var actionParams = new CreateComponentCodeActionParams
        {
            Path = "c:/Another.razor",
        };
        var data = JsonSerializer.SerializeToElement(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit.DocumentChanges);
        Assert.Equal(1, workspaceEdit.DocumentChanges.Value.Count());

        var createFileChange = workspaceEdit.DocumentChanges.Value.First();
        Assert.True(createFileChange.TryGetSecond(out var _));
    }

    [Fact]
    public async Task Handle_CreateComponentWithNamespace()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = $"""
            @page "/test"
            @namespace Another.Namespace
            """;
        var codeDocument = CreateCodeDocument(contents);

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var resolver = new CreateComponentCodeActionResolver(TestLanguageServerFeatureOptions.Instance);
        var actionParams = new CreateComponentCodeActionParams
        {
            Path = "c:/Another.razor",
        };
        var data = JsonSerializer.SerializeToElement(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit.DocumentChanges);
        Assert.Equal(2, workspaceEdit.DocumentChanges.Value.Count());

        var createFileChange = workspaceEdit.DocumentChanges.Value.First();
        Assert.True(createFileChange.TryGetSecond(out var _));

        var editNewComponentChange = workspaceEdit.DocumentChanges.Value.Last();
        var editNewComponentEdit = editNewComponentChange.First.Edits.First();
        Assert.Contains("@namespace Another.Namespace", editNewComponentEdit.NewText, StringComparison.Ordinal);
    }

    private static RazorCodeDocument CreateCodeDocument(string text, string? fileKind = null)
    {
        fileKind ??= FileKinds.Component;

        var projectItem = new TestRazorProjectItem(
            filePath: "c:/Test.razor",
            physicalPath: "c:/Test.razor",
            relativePhysicalPath: "Test.razor",
            fileKind: fileKind)
        {
            Content = text
        };

        var projectEngine = RazorProjectEngine.Create(RazorConfiguration.Default, TestRazorProjectFileSystem.Empty, builder =>
        {
            builder.SetRootNamespace("test.Pages");
        });

        return projectEngine.Process(projectItem);
    }
}
