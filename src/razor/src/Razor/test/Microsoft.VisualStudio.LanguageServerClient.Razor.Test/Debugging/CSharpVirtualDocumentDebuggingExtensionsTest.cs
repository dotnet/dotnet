// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Test;
using Microsoft.VisualStudio.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Debugging;

public class CSharpVirtualDocumentDebuggingExtensionsTest : TestBase
{
    public CSharpVirtualDocumentDebuggingExtensionsTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task GetCSharpSyntaxTreeAsync_InWorkspace_ReusesSyntaxTree()
    {
        // Arrange
        var filePath = "C:\\path\\to\\file.razor.cs";
        var uri = new Uri(filePath);
        using var workspace = TestWorkspace.Create(adhocWorkspace =>
        {
            var project = adhocWorkspace.AddProject("TestProject", LanguageNames.CSharp);
            var csharpSourceText = SourceText.From("class Foo{}");
            var documentId = DocumentId.CreateNewId(project.Id);
            var textAndVersion = TextAndVersion.Create(csharpSourceText, VersionStamp.Default);
            var textLoader = TextLoader.From(textAndVersion);
            var documentInfo = DocumentInfo.Create(documentId, "TestDocument", loader: textLoader, filePath: filePath);
            adhocWorkspace.AddDocument(documentInfo);
        });
        var textBuffer = new TestTextBuffer(new StringTextSnapshot("INVALID"));
        var virtualDocument = new CSharpVirtualDocument(uri, textBuffer);
        var virtualDocumentSnapshot = (CSharpVirtualDocumentSnapshot)virtualDocument.CurrentSnapshot;

        // Act
        var syntaxTree = await virtualDocumentSnapshot.GetCSharpSyntaxTreeAsync(workspace, DisposalToken);

        // Assert
        var diagnostics = syntaxTree.GetDiagnostics();
        Assert.Empty(diagnostics);
        Assert.True(syntaxTree.Length > 0);
    }

    [Fact]
    public async Task GetCSharpSyntaxTreeAsync_NotInWorkspace_CreatesSyntaxTree()
    {
        // Arrange
        var filePath = "C:\\path\\to\\file.razor.cs";
        var uri = new Uri(filePath);
        using var workspace = TestWorkspace.Create();
        var textBuffer = new TestTextBuffer(new StringTextSnapshot("class Foo{}"));
        var virtualDocument = new CSharpVirtualDocument(uri, textBuffer);
        var virtualDocumentSnapshot = (CSharpVirtualDocumentSnapshot)virtualDocument.CurrentSnapshot;

        // Act
        var syntaxTree = await virtualDocumentSnapshot.GetCSharpSyntaxTreeAsync(workspace, DisposalToken);

        // Assert
        var diagnostics = syntaxTree.GetDiagnostics();
        Assert.Empty(diagnostics);
        Assert.True(syntaxTree.Length > 0);
    }

    [Fact]
    public async Task GetCSharpSyntaxTreeAsync_NoWorkspace_CreatesSyntaxTree()
    {
        // Arrange
        var filePath = "C:\\path\\to\\file.razor.cs";
        var uri = new Uri(filePath);
        var textBuffer = new TestTextBuffer(new StringTextSnapshot("class Foo{}"));
        var virtualDocument = new CSharpVirtualDocument(uri, textBuffer);
        var virtualDocumentSnapshot = (CSharpVirtualDocumentSnapshot)virtualDocument.CurrentSnapshot;

        // Act
        var syntaxTree = await virtualDocumentSnapshot.GetCSharpSyntaxTreeAsync(workspace: null, DisposalToken);

        // Assert
        var diagnostics = syntaxTree.GetDiagnostics();
        Assert.Empty(diagnostics);
        Assert.True(syntaxTree.Length > 0);
    }
}
