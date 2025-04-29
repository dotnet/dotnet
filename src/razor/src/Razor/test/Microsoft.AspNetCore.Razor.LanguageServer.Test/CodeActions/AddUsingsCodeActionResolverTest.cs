// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class AddUsingsCodeActionResolverTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public void GetNamespaceFromFQN_Invalid_ReturnsEmpty()
    {
        // Arrange
        var fqn = "Abc";

        // Act
        var namespaceName = AddUsingsCodeActionResolver.GetNamespaceFromFQN(fqn);

        // Assert
        Assert.Empty(namespaceName);
    }

    [Fact]
    public void GetNamespaceFromFQN_Valid_ReturnsNamespace()
    {
        // Arrange
        var fqn = "Abc.Xyz";

        // Act
        var namespaceName = AddUsingsCodeActionResolver.GetNamespaceFromFQN(fqn);

        // Assert
        Assert.Equal("Abc", namespaceName);
    }

    [Fact]
    public void TryCreateAddUsingResolutionParams_CreatesResolutionParams()
    {
        // Arrange
        var fqn = "Abc.Xyz";
        var docUri = new VSTextDocumentIdentifier { Uri = new Uri("c:/path") };

        // Act
        var result = AddUsingsCodeActionResolver.TryCreateAddUsingResolutionParams(fqn, docUri, additionalEdit: null, delegatedDocumentUri: null, out var @namespace, out var resolutionParams);

        // Assert
        Assert.True(result);
        Assert.Equal("Abc", @namespace);
        Assert.NotNull(resolutionParams);
    }

    [Fact]
    public async Task Handle_Unsupported()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = "@page \"/test\"";
        var codeDocument = CreateCodeDocument(contents);
        codeDocument.SetUnsupported();

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var resolver = new AddUsingsCodeActionResolver();
        var data = JsonSerializer.SerializeToElement(new AddUsingsCodeActionParams()
        {
            Namespace = "System",
        });

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.Null(workspaceEdit);
    }

    [Fact]
    public async Task Handle_AddOneUsingToEmpty()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = string.Empty;
        var codeDocument = CreateCodeDocument(contents);

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var resolver = new AddUsingsCodeActionResolver();
        var actionParams = new AddUsingsCodeActionParams
        {
            Namespace = "System",
        };
        var data = JsonSerializer.SerializeToElement(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit.DocumentChanges);
        Assert.Equal(1, workspaceEdit.DocumentChanges.Value.Count());

        var addUsingsChange = workspaceEdit.DocumentChanges.Value.First();
        Assert.True(addUsingsChange.TryGetFirst(out var textDocumentEdit));
        Assert.Single(textDocumentEdit.Edits);
        var firstEdit = textDocumentEdit.Edits.First();
        Assert.Equal(0, firstEdit.Range.Start.Line);
        Assert.Equal("""
            @using System

            """, firstEdit.NewText);
    }

    [Fact]
    public async Task Handle_AddOneUsingToComponentPageDirective()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = """
            @page "/"

            """;
        var codeDocument = CreateCodeDocument(contents);

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var resolver = new AddUsingsCodeActionResolver();
        var actionParams = new AddUsingsCodeActionParams
        {
            Namespace = "System",
        };
        var data = JsonSerializer.SerializeToElement(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit.DocumentChanges);
        Assert.Equal(1, workspaceEdit.DocumentChanges.Value.Count());

        var addUsingsChange = workspaceEdit.DocumentChanges.Value.First();
        Assert.True(addUsingsChange.TryGetFirst(out var textDocumentEdit));
        var firstEdit = Assert.Single(textDocumentEdit.Edits);
        Assert.Equal(1, firstEdit.Range.Start.Line);
        Assert.Equal("""
            @using System

            """, firstEdit.NewText);
    }

    [Fact]
    public async Task Handle_AddOneUsingToPageDirective()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.cshtml");
        var contents = """
            @page
            @model IndexModel
            """;

        var projectItem = new TestRazorProjectItem(
            filePath: "c:/Test.cshtml",
            physicalPath: "c:/Test.cshtml",
            relativePhysicalPath: "Test.cshtml",
            fileKind: FileKinds.Legacy)
        {
            Content = contents
        };

        var projectEngine = RazorProjectEngine.Create(RazorConfiguration.Default, TestRazorProjectFileSystem.Empty, builder =>
        {
            PageDirective.Register(builder);
            ModelDirective.Register(builder);
        });

        var codeDocument = projectEngine.Process(projectItem);

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var resolver = new AddUsingsCodeActionResolver();
        var actionParams = new AddUsingsCodeActionParams
        {
            Namespace = "System",
        };
        var data = JsonSerializer.SerializeToElement(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit.DocumentChanges);
        Assert.Equal(1, workspaceEdit.DocumentChanges.Value.Count());

        var addUsingsChange = workspaceEdit.DocumentChanges.Value.First();
        Assert.True(addUsingsChange.TryGetFirst(out var textDocumentEdit));
        var firstEdit = Assert.Single(textDocumentEdit.Edits);
        Assert.Equal(1, firstEdit.Range.Start.Line);
        Assert.Equal("""
            @using System

            """, firstEdit.NewText);
    }

    [Fact]
    public async Task Handle_AddOneUsingToHTML()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = """
            <table>
            <tr>
            </tr>
            </table>
            """;
        var codeDocument = CreateCodeDocument(contents);

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var resolver = new AddUsingsCodeActionResolver();
        var actionParams = new AddUsingsCodeActionParams
        {
            Namespace = "System",
        };
        var data = JsonSerializer.SerializeToElement(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit.DocumentChanges);
        Assert.Equal(1, workspaceEdit.DocumentChanges.Value.Count());

        var addUsingsChange = workspaceEdit.DocumentChanges.Value.First();
        Assert.True(addUsingsChange.TryGetFirst(out var textDocumentEdit));
        var firstEdit = Assert.Single(textDocumentEdit.Edits);
        Assert.Equal(0, firstEdit.Range.Start.Line);
        Assert.Equal("""
            @using System

            """, firstEdit.NewText);
    }

    [Fact]
    public async Task Handle_AddOneUsingToNamespace()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = """
            @namespace Testing

            """;
        var codeDocument = CreateCodeDocument(contents);

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var resolver = new AddUsingsCodeActionResolver();
        var actionParams = new AddUsingsCodeActionParams
        {
            Namespace = "System",
        };
        var data = JsonSerializer.SerializeToElement(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit.DocumentChanges);
        Assert.Equal(1, workspaceEdit.DocumentChanges.Value.Count());

        var addUsingsChange = workspaceEdit.DocumentChanges.Value.First();
        Assert.True(addUsingsChange.TryGetFirst(out var textDocumentEdit));
        var firstEdit = Assert.Single(textDocumentEdit.Edits);
        Assert.Equal(1, firstEdit.Range.Start.Line);
        Assert.Equal("""
            @using System

            """, firstEdit.NewText);
    }

    [Fact]
    public async Task Handle_AddOneUsingToPageAndNamespace()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = """
            @page "/"
            @namespace Testing

            """;
        var codeDocument = CreateCodeDocument(contents);

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var resolver = new AddUsingsCodeActionResolver();
        var actionParams = new AddUsingsCodeActionParams
        {
            Namespace = "System",
        };
        var data = JsonSerializer.SerializeToElement(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit.DocumentChanges);
        Assert.Equal(1, workspaceEdit.DocumentChanges.Value.Count());

        var addUsingsChange = workspaceEdit.DocumentChanges.Value.First();
        Assert.True(addUsingsChange.TryGetFirst(out var textDocumentEdit));
        var firstEdit = Assert.Single(textDocumentEdit.Edits);
        Assert.Equal(2, firstEdit.Range.Start.Line);
        Assert.Equal("""
            @using System

            """, firstEdit.NewText);
    }

    [Fact]
    public async Task Handle_AddOneUsingToUsings()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = "@using System";
        var codeDocument = CreateCodeDocument(contents);

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var resolver = new AddUsingsCodeActionResolver();
        var actionParams = new AddUsingsCodeActionParams
        {
            Namespace = "System.Linq",
        };
        var data = JsonSerializer.SerializeToElement(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit.DocumentChanges);
        Assert.Equal(1, workspaceEdit.DocumentChanges.Value.Count());

        var addUsingsChange = workspaceEdit.DocumentChanges.Value.First();
        Assert.True(addUsingsChange.TryGetFirst(out var textDocumentEdit));
        var firstEdit = Assert.Single(textDocumentEdit.Edits);
        Assert.Equal(1, firstEdit.Range.Start.Line);
        Assert.Equal("""
            @using System.Linq

            """, firstEdit.NewText);
    }

    [Fact]
    public async Task Handle_AddOneNonSystemUsingToSystemUsings()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = """
            @using System
            @using System.Linq

            """;
        var codeDocument = CreateCodeDocument(contents);

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var resolver = new AddUsingsCodeActionResolver();
        var actionParams = new AddUsingsCodeActionParams
        {
            Namespace = "Microsoft.AspNetCore.Razor.Language",
        };
        var data = JsonSerializer.SerializeToElement(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit.DocumentChanges);
        Assert.Equal(1, workspaceEdit.DocumentChanges.Value.Count());

        var addUsingsChange = workspaceEdit.DocumentChanges.Value.First();
        Assert.True(addUsingsChange.TryGetFirst(out var textDocumentEdit));
        var firstEdit = Assert.Single(textDocumentEdit.Edits);
        Assert.Equal(2, firstEdit.Range.Start.Line);
        Assert.Equal("""
            @using Microsoft.AspNetCore.Razor.Language

            """, firstEdit.NewText);
    }

    private static RazorCodeDocument CreateCodeDocument(string text)
    {
        var fileName = "Test.razor";
        var filePath = $"c:/{fileName}";
        var projectItem = new TestRazorProjectItem(
            filePath,
            physicalPath: filePath,
            relativePhysicalPath: fileName,
            fileKind: FileKinds.Component)
        {
            Content = text
        };

        var projectEngine = RazorProjectEngine.Create(RazorConfiguration.Default, TestRazorProjectFileSystem.Empty, builder =>
        {
            PageDirective.Register(builder);
        });

        return projectEngine.Process(projectItem);
    }
}
