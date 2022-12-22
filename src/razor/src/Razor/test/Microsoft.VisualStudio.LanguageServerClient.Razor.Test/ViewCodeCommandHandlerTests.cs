// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.CodeDom.Compiler;
using System.IO;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Editor.Commanding.Commands;
using Microsoft.VisualStudio.Threading;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

public class ViewCodeCommandHandlerTests : TestBase
{
    public ViewCodeCommandHandlerTests(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void RazorFile_Available()
    {
        using var _ = CreateTestFiles("test.razor", out var razorFilePath);

        var handler = CreateHandler(razorFilePath, out var args);

        var result = handler.GetCommandState(args);

        Assert.True(result.IsAvailable);
    }

    [Fact]
    public void CsHtmlFile_Available()
    {
        using var _ = CreateTestFiles("test.cshtml", out var razorFilePath);

        var handler = CreateHandler(razorFilePath, out var args);

        var result = handler.GetCommandState(args);

        Assert.True(result.IsAvailable);
    }

    [Fact]
    public void RazorFile_Cached_Available()
    {
        using var files = CreateTestFiles("test.razor", out var razorFilePath);

        var handler = CreateHandler(razorFilePath, out var args);

        var result = handler.GetCommandState(args);

        Assert.True(result.IsAvailable);

        files.Delete();

        // Even though the file doesn't exist now, we should still be available because the result is cached
        Assert.True(result.IsAvailable);
        Assert.False(File.Exists(razorFilePath + ".cs"), "The premise of this test is bad and it should feel bad");
    }

    [Fact]
    public void NonRazorFile_NotAvailable()
    {
        using var _ = CreateTestFiles("test.daveswebframework", out var razorFilePath);

        var handler = CreateHandler(razorFilePath, out var args);

        var result = handler.GetCommandState(args);

        Assert.False(result.IsAvailable);
    }

    [Fact]
    public void RazorFile_NoCSharpFile_NotAvailable()
    {
        var razorFilePath = "nonexistent.razor";

        var handler = CreateHandler(razorFilePath, out var args);

        var result = handler.GetCommandState(args);

        Assert.False(result.IsAvailable);
    }

    private static ViewCodeCommandHandler CreateHandler(string razorFilePath, out ViewCodeCommandArgs args)
    {
        var textBuffer = Mock.Of<ITextBuffer>(MockBehavior.Strict);
        var textDocument = Mock.Of<ITextDocument>(doc => doc.FilePath == razorFilePath, MockBehavior.Strict);
        var textDocumentFactory = Mock.Of<ITextDocumentFactoryService>(factory => factory.TryGetTextDocument(textBuffer, out textDocument) == true, MockBehavior.Strict);
        var joinableTaskContext = new JoinableTaskContext();
        var documentInteractionManager = Mock.Of<DocumentInteractionManager>(MockBehavior.Strict);
        var handler = new ViewCodeCommandHandler(documentInteractionManager, textDocumentFactory, joinableTaskContext);

        var textView = Mock.Of<ITextView>(MockBehavior.Strict);
        args = new ViewCodeCommandArgs(textView, textBuffer);

        return handler;
    }

    private static TempFileCollection CreateTestFiles(string razorFileName, out string razorFilePath)
    {
        var files = new TempFileCollection();
        razorFilePath = Path.Combine(files.TempDir, razorFileName);
        var csharpFilePath = razorFilePath + ".cs";

        // Create our temp file
        File.WriteAllText(csharpFilePath, "");

        // Add it to the list so it gets cleaned up
        files.AddFile(csharpFilePath, false);

        return files;
    }
}
