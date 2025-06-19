// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.CodeActions;
using Microsoft.CodeAnalysis.Razor.CodeActions.Models;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

public class ExtractToCodeBehindCodeActionResolverTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    private readonly IClientConnection _clientConnection = TestClientConnection.Create(builder =>
    {
        builder.Add(CustomMessageNames.RazorFormatNewFileEndpointName, (string?)null);
    });

    [Fact]
    public async Task Handle_InvalidFileKind()
    {
        // Arrange
        var documentPath = new Uri("c:\\Test.razor");
        var contents = """
            @page "/test"
            @code { private int x = 1; }
            """;
        var codeDocument = CreateCodeDocument(contents, fileKind: RazorFileKind.Legacy);

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var roslynCodeActionHelpers = new RoslynCodeActionHelpers(_clientConnection);
        var resolver = new ExtractToCodeBehindCodeActionResolver(TestLanguageServerFeatureOptions.Instance, roslynCodeActionHelpers);
        var data = JsonSerializer.SerializeToElement(CreateExtractToCodeBehindCodeActionParams(contents, "@code", "Test"));

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.Null(workspaceEdit);
    }

    [Fact]
    public async Task Handle_ExtractCodeBlock()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = """
            @page "/test"

            @code {
                private int x = 1;
            }
            """;
        var codeDocument = CreateCodeDocument(contents);
        Assert.True(codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace));

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var roslynCodeActionHelpers = new RoslynCodeActionHelpers(_clientConnection);
        var resolver = new ExtractToCodeBehindCodeActionResolver(TestLanguageServerFeatureOptions.Instance, roslynCodeActionHelpers);
        var actionParams = CreateExtractToCodeBehindCodeActionParams(contents, "@code", @namespace);
        var data = JsonSerializer.SerializeToElement(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit.DocumentChanges);
        Assert.Equal(3, workspaceEdit.DocumentChanges.Value.Count());

        var documentChanges = workspaceEdit.DocumentChanges.Value.ToArray();
        var createFileChange = documentChanges[0];
        Assert.True(createFileChange.TryGetSecond(out var _));

        var editCodeDocumentChange = documentChanges[1];
        Assert.True(editCodeDocumentChange.TryGetFirst(out var textDocumentEdit1));
        var editCodeDocumentEdit = textDocumentEdit1.Edits.First();
        var sourceText = codeDocument.Source.Text;
        Assert.True(sourceText.TryGetAbsoluteIndex(((TextEdit)editCodeDocumentEdit).Range.Start, out var removeStart));
        Assert.Equal(actionParams.RemoveStart, removeStart);
        Assert.True(sourceText.TryGetAbsoluteIndex(((TextEdit)editCodeDocumentEdit).Range.End, out var removeEnd));
        Assert.Equal(actionParams.RemoveEnd, removeEnd);

        var editCodeBehindChange = documentChanges[2];
        Assert.True(editCodeBehindChange.TryGetFirst(out var textDocumentEdit2));
        var editCodeBehindEdit = textDocumentEdit2.Edits.First();

        AssertEx.EqualOrDiff("""
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Components;

            namespace test.Pages
            {
                public partial class Test
                {
                    private int x = 1;
                }
            }
            """,
            ((TextEdit)editCodeBehindEdit).NewText);
    }

    [Fact]
    public async Task Handle_ExtractCodeBlock2()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = """
            @page "/test"

            @code
            {
                private int x = 1;
            }
            """;
        var codeDocument = CreateCodeDocument(contents);
        Assert.True(codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace));

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var roslynCodeActionHelpers = new RoslynCodeActionHelpers(_clientConnection);
        var resolver = new ExtractToCodeBehindCodeActionResolver(TestLanguageServerFeatureOptions.Instance, roslynCodeActionHelpers);
        var actionParams = CreateExtractToCodeBehindCodeActionParams(contents, "@code", @namespace);
        var data = JsonSerializer.SerializeToElement(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit.DocumentChanges);
        Assert.Equal(3, workspaceEdit.DocumentChanges.Value.Count());

        var documentChanges = workspaceEdit.DocumentChanges.Value.ToArray();
        var createFileChange = documentChanges[0];
        Assert.True(createFileChange.TryGetSecond(out var _));

        var editCodeDocumentChange = documentChanges[1];
        Assert.True(editCodeDocumentChange.TryGetFirst(out var textDocumentEdit1));
        var editCodeDocumentEdit = textDocumentEdit1.Edits.First();
        var sourceText = codeDocument.Source.Text;
        Assert.True(sourceText.TryGetAbsoluteIndex(((TextEdit)editCodeDocumentEdit).Range.Start, out var removeStart));
        Assert.Equal(actionParams.RemoveStart, removeStart);
        Assert.True(sourceText.TryGetAbsoluteIndex(((TextEdit)editCodeDocumentEdit).Range.End, out var removeEnd));
        Assert.Equal(actionParams.RemoveEnd, removeEnd);

        var editCodeBehindChange = documentChanges[2];
        Assert.True(editCodeBehindChange.TryGetFirst(out var textDocumentEdit2));
        var editCodeBehindEdit = textDocumentEdit2.Edits.First();

        AssertEx.EqualOrDiff("""
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Components;

            namespace test.Pages
            {
                public partial class Test
                {
                    private int x = 1;
                }
            }
            """,
            ((TextEdit)editCodeBehindEdit).NewText);
    }

    [Fact]
    public async Task Handle_ExtractCodeBlock_MultipleMembers()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = """
            @page "/test"

            @code {
                private int x = 1;
                private int z = 2;

                private string y = "hello";

                // Here is a comment
                private void M()
                {
                    // okay
                }
            }
            """;
        var codeDocument = CreateCodeDocument(contents);
        Assert.True(codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace));

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var roslynCodeActionHelpers = new RoslynCodeActionHelpers(_clientConnection);
        var resolver = new ExtractToCodeBehindCodeActionResolver(TestLanguageServerFeatureOptions.Instance, roslynCodeActionHelpers);
        var actionParams = CreateExtractToCodeBehindCodeActionParams(contents, "@code", @namespace);
        var data = JsonSerializer.SerializeToElement(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit.DocumentChanges);
        Assert.Equal(3, workspaceEdit.DocumentChanges.Value.Count());

        var documentChanges = workspaceEdit.DocumentChanges.Value.ToArray();
        var createFileChange = documentChanges[0];
        Assert.True(createFileChange.TryGetSecond(out var _));

        var editCodeDocumentChange = documentChanges[1];
        Assert.True(editCodeDocumentChange.TryGetFirst(out var textDocumentEdit1));
        var editCodeDocumentEdit = textDocumentEdit1.Edits.First();
        var sourceText = codeDocument.Source.Text;
        Assert.True(sourceText.TryGetAbsoluteIndex(((TextEdit)editCodeDocumentEdit).Range.Start, out var removeStart));
        Assert.Equal(actionParams.RemoveStart, removeStart);
        Assert.True(sourceText.TryGetAbsoluteIndex(((TextEdit)editCodeDocumentEdit).Range.End, out var removeEnd));
        Assert.Equal(actionParams.RemoveEnd, removeEnd);

        var editCodeBehindChange = documentChanges[2];
        Assert.True(editCodeBehindChange.TryGetFirst(out var textDocumentEdit2));
        var editCodeBehindEdit = textDocumentEdit2.Edits.First();

        AssertEx.EqualOrDiff("""
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Components;

            namespace test.Pages
            {
                public partial class Test
                {
                    private int x = 1;
                    private int z = 2;

                    private string y = "hello";

                    // Here is a comment
                    private void M()
                    {
                        // okay
                    }
                }
            }
            """,
            ((TextEdit)editCodeBehindEdit).NewText);
    }

    [Fact]
    public async Task Handle_ExtractCodeBlock_MultipleMembers2()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = """
            @page "/test"

            @code
            {
                private int x = 1;
                private int z = 2;

                private string y = "hello";

                // Here is a comment
                private void M()
                {
                    // okay
                }
            }
            """;
        var codeDocument = CreateCodeDocument(contents);
        Assert.True(codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace));

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var roslynCodeActionHelpers = new RoslynCodeActionHelpers(_clientConnection);
        var resolver = new ExtractToCodeBehindCodeActionResolver(TestLanguageServerFeatureOptions.Instance, roslynCodeActionHelpers);
        var actionParams = CreateExtractToCodeBehindCodeActionParams(contents, "@code", @namespace);
        var data = JsonSerializer.SerializeToElement(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit.DocumentChanges);
        Assert.Equal(3, workspaceEdit.DocumentChanges.Value.Count());

        var documentChanges = workspaceEdit.DocumentChanges.Value.ToArray();
        var createFileChange = documentChanges[0];
        Assert.True(createFileChange.TryGetSecond(out var _));

        var editCodeDocumentChange = documentChanges[1];
        Assert.True(editCodeDocumentChange.TryGetFirst(out var textDocumentEdit1));
        var editCodeDocumentEdit = textDocumentEdit1.Edits.First();
        var sourceText = codeDocument.Source.Text;
        Assert.True(sourceText.TryGetAbsoluteIndex(((TextEdit)editCodeDocumentEdit).Range.Start, out var removeStart));
        Assert.Equal(actionParams.RemoveStart, removeStart);
        Assert.True(sourceText.TryGetAbsoluteIndex(((TextEdit)editCodeDocumentEdit).Range.End, out var removeEnd));
        Assert.Equal(actionParams.RemoveEnd, removeEnd);

        var editCodeBehindChange = documentChanges[2];
        Assert.True(editCodeBehindChange.TryGetFirst(out var textDocumentEdit2));
        var editCodeBehindEdit = textDocumentEdit2.Edits.First();

        AssertEx.EqualOrDiff("""
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Components;

            namespace test.Pages
            {
                public partial class Test
                {
                    private int x = 1;
                    private int z = 2;

                    private string y = "hello";

                    // Here is a comment
                    private void M()
                    {
                        // okay
                    }
                }
            }
            """,
            ((TextEdit)editCodeBehindEdit).NewText);
    }

    [Fact]
    public async Task Handle_ExtractCodeBlock_MultipleMembers3()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = """
            @page "/test"

            <div>
                @code
                {
                    private int x = 1;
                    private int z = 2;

                    private string y = "hello";

                    // Here is a comment
                    private void M()
                    {
                        // okay
                    }
                }
            </div>
            """;
        var codeDocument = CreateCodeDocument(contents);
        Assert.True(codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace));

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var roslynCodeActionHelpers = new RoslynCodeActionHelpers(_clientConnection);
        var resolver = new ExtractToCodeBehindCodeActionResolver(TestLanguageServerFeatureOptions.Instance, roslynCodeActionHelpers);
        var actionParams = CreateExtractToCodeBehindCodeActionParams(contents, "@code", @namespace);
        var data = JsonSerializer.SerializeToElement(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit.DocumentChanges);
        Assert.Equal(3, workspaceEdit.DocumentChanges.Value.Count());

        var documentChanges = workspaceEdit.DocumentChanges.Value.ToArray();
        var createFileChange = documentChanges[0];
        Assert.True(createFileChange.TryGetSecond(out var _));

        var editCodeDocumentChange = documentChanges[1];
        Assert.True(editCodeDocumentChange.TryGetFirst(out var textDocumentEdit1));
        var editCodeDocumentEdit = textDocumentEdit1.Edits.First();
        var sourceText = codeDocument.Source.Text;
        Assert.True(sourceText.TryGetAbsoluteIndex(((TextEdit)editCodeDocumentEdit).Range.Start, out var removeStart));
        Assert.Equal(actionParams.RemoveStart, removeStart);
        Assert.True(sourceText.TryGetAbsoluteIndex(((TextEdit)editCodeDocumentEdit).Range.End, out var removeEnd));
        Assert.Equal(actionParams.RemoveEnd, removeEnd);

        var editCodeBehindChange = documentChanges[2];
        Assert.True(editCodeBehindChange.TryGetFirst(out var textDocumentEdit2));
        var editCodeBehindEdit = textDocumentEdit2.Edits.First();

        AssertEx.EqualOrDiff("""
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Components;

            namespace test.Pages
            {
                public partial class Test
                {
                    private int x = 1;
                    private int z = 2;

                    private string y = "hello";

                    // Here is a comment
                    private void M()
                    {
                        // okay
                    }
                }
            }
            """,
            ((TextEdit)editCodeBehindEdit).NewText);
    }

    [Fact]
    public async Task Handle_ExtractFunctionsBlock()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = """
            @page "/test"

            @functions {
                private int x = 1;
            }
            """;
        var codeDocument = CreateCodeDocument(contents);
        Assert.True(codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace));

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var roslynCodeActionHelpers = new RoslynCodeActionHelpers(_clientConnection);
        var resolver = new ExtractToCodeBehindCodeActionResolver(TestLanguageServerFeatureOptions.Instance, roslynCodeActionHelpers);
        var actionParams = CreateExtractToCodeBehindCodeActionParams(contents, "@functions", @namespace);
        var data = JsonSerializer.SerializeToElement(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit.DocumentChanges);
        Assert.Equal(3, workspaceEdit.DocumentChanges.Value.Count());

        var documentChanges = workspaceEdit.DocumentChanges.Value.ToArray();
        var createFileChange = documentChanges[0];
        Assert.True(createFileChange.TryGetSecond(out var _));

        var editCodeDocumentChange = documentChanges[1];
        Assert.True(editCodeDocumentChange.TryGetFirst(out var editCodeDocument));
        var editCodeDocumentEdit = editCodeDocument.Edits.First();
        var sourceText = codeDocument.Source.Text;
        Assert.True(sourceText.TryGetAbsoluteIndex(((TextEdit)editCodeDocumentEdit).Range.Start, out var removeStart));
        Assert.Equal(actionParams.RemoveStart, removeStart);
        Assert.True(sourceText.TryGetAbsoluteIndex(((TextEdit)editCodeDocumentEdit).Range.End, out var removeEnd));
        Assert.Equal(actionParams.RemoveEnd, removeEnd);

        var editCodeBehindChange = documentChanges[2];
        Assert.True(editCodeBehindChange.TryGetFirst(out var editCodeBehind));
        var editCodeBehindEdit = editCodeBehind.Edits.First();

        AssertEx.EqualOrDiff("""
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Components;

            namespace test.Pages
            {
                public partial class Test
                {
                    private int x = 1;
                }
            }
            """,
            ((TextEdit)editCodeBehindEdit).NewText);
    }

    [Fact]
    public async Task Handle_ExtractCodeBlockWithUsing()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = """
            @page "/test"
            @using System.Diagnostics

            @code {
                private int x = 1;
            }
            """;
        var codeDocument = CreateCodeDocument(contents);
        Assert.True(codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace));

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var roslynCodeActionHelpers = new RoslynCodeActionHelpers(_clientConnection);
        var resolver = new ExtractToCodeBehindCodeActionResolver(TestLanguageServerFeatureOptions.Instance, roslynCodeActionHelpers);
        var actionParams = CreateExtractToCodeBehindCodeActionParams(contents, "@code", @namespace);
        var data = JsonSerializer.SerializeToElement(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit.DocumentChanges);
        Assert.Equal(3, workspaceEdit.DocumentChanges.Value.Count());

        var documentChanges = workspaceEdit.DocumentChanges.Value.ToArray();
        var createFileChange = documentChanges[0];
        Assert.True(createFileChange.TryGetSecond(out var _));

        var editCodeDocumentChange = documentChanges[1];
        Assert.True(editCodeDocumentChange.TryGetFirst(out var editCodeDocument));
        var editCodeDocumentEdit = editCodeDocument.Edits.First();
        var sourceText = codeDocument.Source.Text;
        Assert.True(sourceText.TryGetAbsoluteIndex(((TextEdit)editCodeDocumentEdit).Range.Start, out var removeStart));
        Assert.Equal(actionParams.RemoveStart, removeStart);
        Assert.True(sourceText.TryGetAbsoluteIndex(((TextEdit)editCodeDocumentEdit).Range.End, out var removeEnd));
        Assert.Equal(actionParams.RemoveEnd, removeEnd);

        var editCodeBehindChange = documentChanges[2];
        Assert.True(editCodeBehindChange.TryGetFirst(out var editCodeBehind));
        var editCodeBehindEdit = editCodeBehind.Edits.First();

        AssertEx.EqualOrDiff("""
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Components;
            using System.Diagnostics;

            namespace test.Pages
            {
                public partial class Test
                {
                    private int x = 1;
                }
            }
            """,
            ((TextEdit)editCodeBehindEdit).NewText);
    }

    [Fact]
    public async Task Handle_ExtractCodeBlockWithDirectives()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = """
            @page "/test"

            @code {
            #region TestRegion
                    private int x = 1;
            #endregion
            }
            """;
        var codeDocument = CreateCodeDocument(contents);
        Assert.True(codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace));

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var roslynCodeActionHelpers = new RoslynCodeActionHelpers(_clientConnection);
        var resolver = new ExtractToCodeBehindCodeActionResolver(TestLanguageServerFeatureOptions.Instance, roslynCodeActionHelpers);
        var actionParams = CreateExtractToCodeBehindCodeActionParams(contents, "@code", @namespace);
        var data = JsonSerializer.SerializeToElement(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit.DocumentChanges);
        Assert.Equal(3, workspaceEdit.DocumentChanges.Value.Count());

        var documentChanges = workspaceEdit.DocumentChanges.Value.ToArray();
        var createFileChange = documentChanges[0];
        Assert.True(createFileChange.TryGetSecond(out var _));

        var editCodeDocumentChange = documentChanges[1];
        Assert.True(editCodeDocumentChange.TryGetFirst(out var textDocumentEdit1));
        var editCodeDocumentEdit = textDocumentEdit1.Edits.First();
        var sourceText = codeDocument.Source.Text;
        Assert.True(sourceText.TryGetAbsoluteIndex(((TextEdit)editCodeDocumentEdit).Range.Start, out var removeStart));
        Assert.Equal(actionParams.RemoveStart, removeStart);
        Assert.True(sourceText.TryGetAbsoluteIndex(((TextEdit)editCodeDocumentEdit).Range.End, out var removeEnd));
        Assert.Equal(actionParams.RemoveEnd, removeEnd);

        var editCodeBehindChange = documentChanges[2];
        Assert.True(editCodeBehindChange.TryGetFirst(out var textDocumentEdit2));
        var editCodeBehindEdit = textDocumentEdit2.Edits.First();

        AssertEx.EqualOrDiff("""
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Components;

            namespace test.Pages
            {
                public partial class Test
                {
                    #region TestRegion
                    private int x = 1;
                    #endregion
                }
            }
            """,
            ((TextEdit)editCodeBehindEdit).NewText);
    }

    [Fact]
    public async Task Handle_ExtractCodeBlock_CallsRoslyn()
    {
        // Arrange
        var documentPath = new Uri("c:/Test.razor");
        var contents = """
            @page "/test"

            @code {
                private int x = 1;
            }
            """;
        var codeDocument = CreateCodeDocument(contents);
        Assert.True(codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace));

        var clientConnection = TestClientConnection.Create(builder =>
        {
            builder.Add(CustomMessageNames.RazorFormatNewFileEndpointName, "Hi there! I'm from Roslyn");
        });

        var documentContext = CreateDocumentContext(documentPath, codeDocument);
        var roslynCodeActionHelpers = new RoslynCodeActionHelpers(clientConnection);
        var resolver = new ExtractToCodeBehindCodeActionResolver(TestLanguageServerFeatureOptions.Instance, roslynCodeActionHelpers);
        var actionParams = CreateExtractToCodeBehindCodeActionParams(contents, "@code", @namespace);
        var data = JsonSerializer.SerializeToElement(actionParams);

        // Act
        var workspaceEdit = await resolver.ResolveAsync(documentContext, data, new RazorFormattingOptions(), DisposalToken);

        // Assert
        Assert.NotNull(workspaceEdit);
        Assert.NotNull(workspaceEdit.DocumentChanges);
        Assert.Equal(3, workspaceEdit.DocumentChanges.Value.Count());

        var documentChanges = workspaceEdit.DocumentChanges.Value.ToArray();
        var createFileChange = documentChanges[0];
        Assert.True(createFileChange.TryGetSecond(out var _));

        var editCodeDocumentChange = documentChanges[1];
        Assert.True(editCodeDocumentChange.TryGetFirst(out var textDocumentEdit1));
        var editCodeDocumentEdit = textDocumentEdit1.Edits.First();
        var sourceText = codeDocument.Source.Text;
        Assert.True(sourceText.TryGetAbsoluteIndex(((TextEdit)editCodeDocumentEdit).Range.Start, out var removeStart));
        Assert.Equal(actionParams.RemoveStart, removeStart);
        Assert.True(sourceText.TryGetAbsoluteIndex(((TextEdit)editCodeDocumentEdit).Range.End, out var removeEnd));
        Assert.Equal(actionParams.RemoveEnd, removeEnd);

        var editCodeBehindChange = documentChanges[2];
        Assert.True(editCodeBehindChange.TryGetFirst(out var textDocumentEdit2));
        var editCodeBehindEdit = textDocumentEdit2.Edits.First();

        AssertEx.EqualOrDiff("""
            Hi there! I'm from Roslyn
            """,
            ((TextEdit)editCodeBehindEdit).NewText);
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

    private static ExtractToCodeBehindCodeActionParams CreateExtractToCodeBehindCodeActionParams(string contents, string removeStart, string @namespace)
    {
        // + 1 to ensure we do not cut off the '}'.
        var endIndex = contents.LastIndexOf("}", StringComparison.Ordinal) + 1;
        return new ExtractToCodeBehindCodeActionParams
        {
            RemoveStart = contents.IndexOf(removeStart, StringComparison.Ordinal),
            ExtractStart = contents.IndexOf("{", StringComparison.Ordinal),
            ExtractEnd = endIndex,
            RemoveEnd = endIndex,
            Namespace = @namespace
        };
    }
}
