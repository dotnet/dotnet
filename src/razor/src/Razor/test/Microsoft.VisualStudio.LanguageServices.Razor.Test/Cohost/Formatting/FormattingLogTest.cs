// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Razor.LanguageClient.Cohost.Formatting;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Test.Cohost.Formatting;

/// <summary>
/// Not tests of the formatting log, but tests that use formatting logs sent in
/// by users reporting issues.
/// </summary>
public class FormattingLogTest(ITestOutputHelper testOutput) : DocumentFormattingTestBase(testOutput)
{
    [Fact]
    [WorkItem("https://github.com/dotnet/vscode-csharp/issues/7264")]
    public async Task UnexpectedFalseInIndentBlockOperation()
        => Assert.NotNull(await GetFormattingEditsAsync());

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/12416")]
    public async Task MixedIndentation()
        => Assert.NotNull(await GetFormattingEditsAsync());

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/12416")]
    public async Task RealWorldMixedIndentation()
        => Assert.NotNull(await GetFormattingEditsAsync());

    [Fact]
    [WorkItem("https://github.com/dotnet/vscode-csharp/issues/8333")]
    public async Task CSharpStringLiteral()
        => Assert.Null(await GetFormattingEditsAsync()); // All edits should have been filtered out

    [Fact]
    [WorkItem("https://github.com/microsoft/vscode-dotnettools/issues/2766")]
    public async Task RanOutOfOriginalLines()
        => Assert.NotNull(await GetFormattingEditsAsync());

    [Fact]
    [WorkItem("https://developercommunity.visualstudio.com/t/Whilst-using-format-document-on-a-razo/11041051#T-N11042031-N11049221")]
    public async Task CSSWrappedToMultipleLines()
        => Assert.NotNull(await GetFormattingEditsAsync());

    [Fact]
    [WorkItem("https://developercommunity.visualstudio.com/t/Razor-Formatting-Feature-internal-error/11041869#T-ND11043454")]
    public async Task MultiLineLambda()
        => Assert.NotNull(await GetFormattingEditsAsync());

    [Fact]
    [WorkItem("https://developercommunity.visualstudio.com/t/Razor-Formatting-Feature---Internal-Erro/11068847")]
    public async Task GameTracAdmin()
        => Assert.NotNull(await GetFormattingEditsAsync());

    private async Task<TextEdit[]?> GetFormattingEditsAsync([CallerMemberName] string? testName = null)
    {
        var contents = GetResource(testName.AssumeNotNull(), "InitialDocument.txt").AssumeNotNull();
        var document = CreateProjectAndRazorDocument(contents);
        var sourceText = await document.GetTextAsync();

        var options = new RazorFormattingOptions() with
        {
            CSharpSyntaxFormattingOptions = CodeAnalysis.ExternalAccess.Razor.Features.RazorCSharpSyntaxFormattingOptions.Default
        };
        if (GetResource(testName, "Options.json") is { } optionsFile)
        {
            options = (RazorFormattingOptions)JsonSerializer.Deserialize(optionsFile, typeof(RazorFormattingOptions), JsonHelpers.JsonSerializerOptions).AssumeNotNull();
        }

        TextEdit[] htmlEdits = [];
        if (GetResource(testName, "HtmlChanges.json") is { } htmlChangesFile)
        {
            var htmlChanges = JsonSerializer.Deserialize<RazorTextChange[]>(htmlChangesFile, JsonHelpers.JsonSerializerOptions);
            htmlEdits = htmlChanges.Select(c => sourceText.GetTextEdit(c.ToTextChange())).ToArray();
        }

        TextSpan span = default;
        if (GetResource(testName, "Range.json") is { } rangeFile && rangeFile != "null")
        {
            var linePositionSpan = (LinePositionSpan)JsonSerializer.Deserialize(rangeFile, typeof(LinePositionSpan), JsonHelpers.JsonSerializerOptions).AssumeNotNull();
            span = sourceText.GetTextSpan(linePositionSpan);
        }

        var formattingService = (RazorFormattingService)OOPExportProvider.GetExportedValue<IRazorFormattingService>();
        formattingService.GetTestAccessor().SetFormattingLoggerFactory(new TestFormattingLoggerFactory(TestOutputHelper));

        return await GetFormattingEditsAsync(document, htmlEdits, span, options.CodeBlockBraceOnNextLine, options.AttributeIndentStyle, options.InsertSpaces, options.TabSize, options.CSharpSyntaxFormattingOptions.AssumeNotNull());
    }

    private string? GetResource(string testName, string name)
    {
        var baselineFileName = $@"TestFiles\FormattingLog\{testName}\{name}";

        var testFile = TestFile.Create(baselineFileName, GetType().Assembly);
        if (!testFile.Exists())
        {
            return null;
        }

        // Formatting logs capture absolute spans against the original file contents, so we must not normalize line endings.
        return testFile.ReadAllText(normalizeLineEndings: false);
    }
}
