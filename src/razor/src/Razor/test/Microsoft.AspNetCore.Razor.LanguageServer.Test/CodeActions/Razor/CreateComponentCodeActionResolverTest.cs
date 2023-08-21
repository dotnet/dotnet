// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class CreateComponentCodeActionResolverTest : LanguageServerTestBase
{
    private readonly DocumentContextFactory _emptyDocumentContextFactory;

    public CreateComponentCodeActionResolverTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _emptyDocumentContextFactory = new TestDocumentContextFactory();
    }

    [Fact]
    public async Task Handle_MissingFile()
    {
        // Arrange
        var resolver = new CreateComponentCodeActionResolver(_emptyDocumentContextFactory, TestLanguageServerFeatureOptions.Instance);
        var data = JObject.FromObject(new CreateComponentCodeActionParams()
        {
            Uri = new Uri("c:/Test.razor"),
            Path = "c:/Another.razor",
        });

        // Act
        var workspaceEdit = await resolver.ResolveAsync(data, default);

        // Assert
        Assert.Null(workspaceEdit);
    }

    [Fact]
    public async Task Handle_Unsupported()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = $"@page \"/test\"";
        var codeDocument = CreateCodeDocument(contents);
        codeDocument.SetUnsupported();

        var resolver = new CreateComponentCodeActionResolver(CreateDocumentContextFactory(documentPath, codeDocument), TestLanguageServerFeatureOptions.Instance);
        var data = JObject.FromObject(new CreateComponentCodeActionParams()
        {
            Uri = documentPath,
            Path = "c:/Another.razor",
        });

        // Act
        var workspaceEdit = await resolver.ResolveAsync(data, default);

        // Assert
        Assert.Null(workspaceEdit);
    }

    [Fact]
    public async Task Handle_InvalidFileKind()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = $"@page \"/test\"";
        var codeDocument = CreateCodeDocument(contents);
        codeDocument.SetFileKind(FileKinds.Legacy);

        var resolver = new CreateComponentCodeActionResolver(CreateDocumentContextFactory(documentPath, codeDocument), TestLanguageServerFeatureOptions.Instance);
        var data = JObject.FromObject(new CreateComponentCodeActionParams()
        {
            Uri = documentPath,
            Path = "c:/Another.razor",
        });

        // Act
        var workspaceEdit = await resolver.ResolveAsync(data, default);

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

        var resolver = new CreateComponentCodeActionResolver(CreateDocumentContextFactory(documentPath, codeDocument), TestLanguageServerFeatureOptions.Instance);
        var actionParams = new CreateComponentCodeActionParams
        {
            Uri = documentPath,
            Path = "c:/Another.razor",
        };
        var data = JObject.FromObject(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(data, default);

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
        var contents = $"@page \"/test\"{Environment.NewLine}@namespace Another.Namespace";
        var codeDocument = CreateCodeDocument(contents);

        var resolver = new CreateComponentCodeActionResolver(CreateDocumentContextFactory(documentPath, codeDocument), TestLanguageServerFeatureOptions.Instance);
        var actionParams = new CreateComponentCodeActionParams
        {
            Uri = documentPath,
            Path = "c:/Another.razor",
        };
        var data = JObject.FromObject(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(data, default);

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

    private static RazorCodeDocument CreateCodeDocument(string text)
    {
        var projectItem = new TestRazorProjectItem("c:/Test.razor", "c:/Test.razor", "Test.razor") { Content = text };
        var projectEngine = RazorProjectEngine.Create(RazorConfiguration.Default, TestRazorProjectFileSystem.Empty, (builder) => builder.SetRootNamespace("test.Pages"));

        var codeDocument = projectEngine.Process(projectItem);
        codeDocument.SetFileKind(FileKinds.Component);

        return codeDocument;
    }
}
