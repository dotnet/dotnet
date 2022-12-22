// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Common.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class ExtractToCodeBehindCodeActionResolverTest : LanguageServerTestBase
{
    private readonly DocumentContextFactory _emptyDocumentContextFactory;

    public ExtractToCodeBehindCodeActionResolverTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _emptyDocumentContextFactory = new Mock<DocumentContextFactory>(MockBehavior.Strict).Object;

        Mock.Get(_emptyDocumentContextFactory)
            .Setup(r => r.TryCreateAsync(
                It.IsAny<Uri>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);
    }

    [Fact]
    public async Task Handle_MissingFile()
    {
        // Arrange
        var resolver = new ExtractToCodeBehindCodeActionResolver(_emptyDocumentContextFactory);
        var data = JObject.FromObject(new ExtractToCodeBehindCodeActionParams()
        {
            Uri = new Uri("c:/Test.razor"),
            RemoveStart = 14,
            ExtractStart = 19,
            ExtractEnd = 41,
            RemoveEnd = 41,
            Namespace = "Test"
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
        var documentPath = new Uri("c:\\Test.razor");
        var contents = $"@page \"/test\"{Environment.NewLine}@code {{ private var x = 1; }}";
        var codeDocument = CreateCodeDocument(contents);
        codeDocument.SetUnsupported();

        var resolver = new ExtractToCodeBehindCodeActionResolver(CreateDocumentContextFactory(documentPath, codeDocument));
        var data = JObject.FromObject(new ExtractToCodeBehindCodeActionParams()
        {
            Uri = new Uri("c:/Test.razor"),
            RemoveStart = 14,
            ExtractStart = 20,
            ExtractEnd = 41,
            RemoveEnd = 41,
            Namespace = "Test"
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
        var documentPath = new Uri("c:\\Test.razor");
        var contents = $"@page \"/test\"{Environment.NewLine}@code {{ private var x = 1; }}";
        var codeDocument = CreateCodeDocument(contents);
        codeDocument.SetFileKind(FileKinds.Legacy);

        var resolver = new ExtractToCodeBehindCodeActionResolver(CreateDocumentContextFactory(documentPath, codeDocument));
        var data = JObject.FromObject(new ExtractToCodeBehindCodeActionParams()
        {
            Uri = new Uri("c:/Test.razor"),
            RemoveStart = 14,
            ExtractStart = 20,
            ExtractEnd = 41,
            RemoveEnd = 41,
            Namespace = "Test"
        });

        // Act
        var workspaceEdit = await resolver.ResolveAsync(data, default);

        // Assert
        Assert.Null(workspaceEdit);
    }

    [Fact]
    public async Task Handle_ExtractCodeBlock()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = $"@page \"/test\"{Environment.NewLine}@code {{ private var x = 1; }}";
        var codeDocument = CreateCodeDocument(contents);
        Assert.True(codeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out var @namespace));

        var resolver = new ExtractToCodeBehindCodeActionResolver(CreateDocumentContextFactory(documentPath, codeDocument));
        var actionParams = new ExtractToCodeBehindCodeActionParams
        {
            Uri = documentPath,
            RemoveStart = contents.IndexOf("@code", StringComparison.Ordinal),
            ExtractStart = contents.IndexOf("{", StringComparison.Ordinal),
            ExtractEnd = contents.IndexOf("}", StringComparison.Ordinal),
            RemoveEnd = contents.IndexOf("}", StringComparison.Ordinal),
            Namespace = @namespace,
        };
        var data = JObject.FromObject(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(data, default);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit!.DocumentChanges);
        Assert.Equal(3, workspaceEdit.DocumentChanges!.Value.Count());

        var documentChanges = workspaceEdit.DocumentChanges!.Value.ToArray();
        var createFileChange = documentChanges[0];
        Assert.True(createFileChange.TryGetSecond(out var _));

        var editCodeDocumentChange = documentChanges[1];
        Assert.True(editCodeDocumentChange.TryGetFirst(out var textDocumentEdit1));
        var editCodeDocumentEdit = textDocumentEdit1!.Edits.First();
        Assert.True(editCodeDocumentEdit.Range.Start.TryGetAbsoluteIndex(codeDocument.GetSourceText(), Logger, out var removeStart));
        Assert.Equal(actionParams.RemoveStart, removeStart);
        Assert.True(editCodeDocumentEdit.Range.End.TryGetAbsoluteIndex(codeDocument.GetSourceText(), Logger, out var removeEnd));
        Assert.Equal(actionParams.RemoveEnd, removeEnd);

        var editCodeBehindChange = documentChanges[2];
        Assert.True(editCodeBehindChange.TryGetFirst(out var textDocumentEdit2));
        var editCodeBehindEdit = textDocumentEdit2!.Edits.First();
        Assert.Contains("public partial class Test", editCodeBehindEdit.NewText, StringComparison.Ordinal);
        Assert.Contains("private var x = 1", editCodeBehindEdit.NewText, StringComparison.Ordinal);
        Assert.Contains("namespace test.Pages", editCodeBehindEdit.NewText, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Handle_ExtractFunctionsBlock()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = $"@page \"/test\"{Environment.NewLine}@functions {{ private var x = 1; }}";
        var codeDocument = CreateCodeDocument(contents);
        Assert.True(codeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out var @namespace));

        var resolver = new ExtractToCodeBehindCodeActionResolver(CreateDocumentContextFactory(documentPath, codeDocument));
        var actionParams = new ExtractToCodeBehindCodeActionParams
        {
            Uri = documentPath,
            RemoveStart = contents.IndexOf("@functions", StringComparison.Ordinal),
            ExtractStart = contents.IndexOf("{", StringComparison.Ordinal),
            ExtractEnd = contents.IndexOf("}", StringComparison.Ordinal),
            RemoveEnd = contents.IndexOf("}", StringComparison.Ordinal),
            Namespace = @namespace,
        };
        var data = JObject.FromObject(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(data, default);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit!.DocumentChanges);
        Assert.Equal(3, workspaceEdit.DocumentChanges!.Value.Count());

        var documentChanges = workspaceEdit.DocumentChanges!.Value.ToArray();
        var createFileChange = documentChanges[0];
        Assert.True(createFileChange.TryGetSecond(out var _));

        var editCodeDocumentChange = documentChanges[1];
        Assert.True(editCodeDocumentChange.TryGetFirst(out var editCodeDocument));
        var editCodeDocumentEdit = editCodeDocument!.Edits.First();
        Assert.True(editCodeDocumentEdit.Range.Start.TryGetAbsoluteIndex(codeDocument.GetSourceText(), Logger, out var removeStart));
        Assert.Equal(actionParams.RemoveStart, removeStart);
        Assert.True(editCodeDocumentEdit.Range.End.TryGetAbsoluteIndex(codeDocument.GetSourceText(), Logger, out var removeEnd));
        Assert.Equal(actionParams.RemoveEnd, removeEnd);

        var editCodeBehindChange = documentChanges[2];
        Assert.True(editCodeBehindChange.TryGetFirst(out var editCodeBehind));
        var editCodeBehindEdit = editCodeBehind!.Edits.First();
        Assert.Contains("public partial class Test", editCodeBehindEdit.NewText, StringComparison.Ordinal);
        Assert.Contains("private var x = 1", editCodeBehindEdit.NewText, StringComparison.Ordinal);
        Assert.Contains("namespace test.Pages", editCodeBehindEdit.NewText, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Handle_ExtractCodeBlockWithUsing()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = $"@page \"/test\"\n@using System.Diagnostics{Environment.NewLine}@code {{ private var x = 1; }}";
        var codeDocument = CreateCodeDocument(contents);
        Assert.True(codeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out var @namespace));

        var resolver = new ExtractToCodeBehindCodeActionResolver(CreateDocumentContextFactory(documentPath, codeDocument));
        var actionParams = new ExtractToCodeBehindCodeActionParams
        {
            Uri = documentPath,
            RemoveStart = contents.IndexOf("@code", StringComparison.Ordinal),
            ExtractStart = contents.IndexOf("{", StringComparison.Ordinal),
            ExtractEnd = contents.IndexOf("}", StringComparison.Ordinal),
            RemoveEnd = contents.IndexOf("}", StringComparison.Ordinal),
            Namespace = @namespace,
        };
        var data = JObject.FromObject(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(data, default);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit!.DocumentChanges);
        Assert.Equal(3, workspaceEdit.DocumentChanges!.Value.Count());

        var documentChanges = workspaceEdit.DocumentChanges.Value.ToArray();
        var createFileChange = documentChanges[0];
        Assert.True(createFileChange.TryGetSecond(out var _));

        var editCodeDocumentChange = documentChanges[1];
        Assert.True(editCodeDocumentChange.TryGetFirst(out var editCodeDocument));
        var editCodeDocumentEdit = editCodeDocument!.Edits.First();
        Assert.True(editCodeDocumentEdit.Range.Start.TryGetAbsoluteIndex(codeDocument.GetSourceText(), Logger, out var removeStart));
        Assert.Equal(actionParams.RemoveStart, removeStart);
        Assert.True(editCodeDocumentEdit.Range.End.TryGetAbsoluteIndex(codeDocument.GetSourceText(), Logger, out var removeEnd));
        Assert.Equal(actionParams.RemoveEnd, removeEnd);

        var editCodeBehindChange = documentChanges[2];
        Assert.True(editCodeBehindChange.TryGetFirst(out var editCodeBehind));
        var editCodeBehindEdit = editCodeBehind!.Edits.First();
        Assert.Contains("using System.Diagnostics", editCodeBehindEdit.NewText, StringComparison.Ordinal);
        Assert.Contains("public partial class Test", editCodeBehindEdit.NewText, StringComparison.Ordinal);
        Assert.Contains("private var x = 1", editCodeBehindEdit.NewText, StringComparison.Ordinal);
        Assert.Contains("namespace test.Pages", editCodeBehindEdit.NewText, StringComparison.Ordinal);
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
