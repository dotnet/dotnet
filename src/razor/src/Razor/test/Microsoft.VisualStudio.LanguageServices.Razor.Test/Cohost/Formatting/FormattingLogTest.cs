// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.Razor.LanguageClient.Cohost;
using Microsoft.VisualStudio.Razor.LanguageClient.Cohost.Formatting;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Test.Cohost.Formatting;

/// <summary>
/// Not tests of the formatting log, but tests that use formatting logs sent in
/// by users reporting issues.
/// </summary>
[Collection(HtmlFormattingCollection.Name)]
public class FormattingLogTest(FormattingTestContext context, HtmlFormattingFixture fixture, ITestOutputHelper testOutput)
    : FormattingTestBase(context, fixture.Service, testOutput), IClassFixture<FormattingTestContext>
{
    [Fact]
    [WorkItem("https://github.com/dotnet/vscode-csharp/issues/7264")]
    public async Task UnexpectedFalseInIndentBlockOperation()
    {
        var contents = GetResource("InitialDocument.txt");
        var document = CreateProjectAndRazorDocument(contents);

        var optionsFile = GetResource("Options.json");
        var options = (RazorFormattingOptions)JsonSerializer.Deserialize(optionsFile, typeof(RazorFormattingOptions), JsonHelpers.JsonSerializerOptions).AssumeNotNull();

        var formattingService = (RazorFormattingService)OOPExportProvider.GetExportedValue<IRazorFormattingService>();
        formattingService.GetTestAccessor().SetFormattingLoggerFactory(new TestFormattingLoggerFactory(TestOutputHelper));

        var htmlChangesFile = GetResource("HtmlChanges.json");
        var htmlChanges = JsonSerializer.Deserialize<RazorTextChange[]>(htmlChangesFile, JsonHelpers.JsonSerializerOptions);
        var sourceText = await document.GetTextAsync();
        var htmlEdits = htmlChanges.Select(c => sourceText.GetTextEdit(c.ToTextChange())).ToArray();

        await GetFormattingEditsAsync(document, htmlEdits, span: default, options.CodeBlockBraceOnNextLine, options.AttributeIndentStyle, options.InsertSpaces, options.TabSize, options.CSharpSyntaxFormattingOptions.AssumeNotNull());
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/12416")]
    public async Task MixedIndentation()
    {
        var contents = GetResource("InitialDocument.txt");
        var htmlChangesFile = GetResource("HtmlChanges.json");

        Assert.NotNull(await GetFormattingEditsAsync(contents, htmlChangesFile));
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor/issues/12416")]
    public async Task RealWorldMixedIndentation()
    {
        var contents = GetResource("InitialDocument.txt");
        var htmlChangesFile = GetResource("HtmlChanges.json");

        Assert.NotNull(await GetFormattingEditsAsync(contents, htmlChangesFile));
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/vscode-csharp/issues/8333")]
    public async Task CSharpStringLiteral()
    {
        var contents = GetResource("InitialDocument.txt");
        var htmlChangesFile = GetResource("HtmlChanges.json");

        // All edits should have been filtered out
        Assert.Null(await GetFormattingEditsAsync(contents, htmlChangesFile));
    }

    [Fact]
    [WorkItem("https://github.com/microsoft/vscode-dotnettools/issues/2766")]
    public async Task RanOutOfOriginalLines()
    {
        var contents = GetResource("InitialDocument.txt");
        var htmlChangesFile = GetResource("HtmlChanges.json");

        await GetFormattingEditsAsync(contents, htmlChangesFile);
    }

    [Fact]
    [WorkItem("https://developercommunity.visualstudio.com/t/Razor-Formatting-Feature-internal-error/11041869#T-ND11043454")]
    public async Task MultiLineLambda()
    {
        var contents = GetResource("InitialDocument.txt");

        var document = CreateProjectAndRazorDocument(contents);

        var options = new RazorFormattingOptions();

        var formattingService = (RazorFormattingService)OOPExportProvider.GetExportedValue<IRazorFormattingService>();
        formattingService.GetTestAccessor().SetFormattingLoggerFactory(new TestFormattingLoggerFactory(TestOutputHelper));

        await GetFormattingEditsAsync(document, [], span: default, options.CodeBlockBraceOnNextLine, options.AttributeIndentStyle, options.InsertSpaces, options.TabSize, RazorCSharpSyntaxFormattingOptions.Default);
    }

    private async Task<TextEdit[]?> GetFormattingEditsAsync(string contents, string htmlChangesFile)
    {
        var document = CreateProjectAndRazorDocument(contents);

        var options = new RazorFormattingOptions();

        var formattingService = (RazorFormattingService)OOPExportProvider.GetExportedValue<IRazorFormattingService>();
        formattingService.GetTestAccessor().SetFormattingLoggerFactory(new TestFormattingLoggerFactory(TestOutputHelper));

        var htmlChanges = JsonSerializer.Deserialize<RazorTextChange[]>(htmlChangesFile, JsonHelpers.JsonSerializerOptions);
        var sourceText = await document.GetTextAsync();
        var htmlEdits = htmlChanges.Select(c => sourceText.GetTextEdit(c.ToTextChange())).ToArray();

        return await GetFormattingEditsAsync(document, htmlEdits, span: default, options.CodeBlockBraceOnNextLine, options.AttributeIndentStyle, options.InsertSpaces, options.TabSize, RazorCSharpSyntaxFormattingOptions.Default);
    }

    private string GetResource(string name, [CallerMemberName] string? testName = null)
    {
        var baselineFileName = $@"TestFiles\FormattingLog\{testName}\{name}";

        var testFile = TestFile.Create(baselineFileName, GetType().Assembly);
        Assert.True(testFile.Exists());

        return testFile.ReadAllText();
    }
}
