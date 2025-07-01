// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class CreateComponentCodeActionResolverTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task Handle_InvalidFileKind()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = $"@page \"/test\"";
        var codeDocument = CreateCodeDocument(contents, fileKind: RazorFileKind.Legacy);

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
        Assert.Contains("@namespace Another.Namespace", ((TextEdit)editNewComponentEdit).NewText, StringComparison.Ordinal);
    }

    private static RazorCodeDocument CreateCodeDocument(string text, RazorFileKind? fileKind = null)
    {
        var projectItem = new TestRazorProjectItem(
            filePath: "c:/Test.razor",
            physicalPath: "c:/Test.razor",
            relativePhysicalPath: "Test.razor",
            fileKind: fileKind ?? RazorFileKind.Component)
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
