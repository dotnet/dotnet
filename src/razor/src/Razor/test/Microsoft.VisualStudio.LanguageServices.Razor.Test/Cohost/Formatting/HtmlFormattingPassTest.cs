// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Razor.LanguageClient.Cohost;
using Microsoft.VisualStudio.Razor.LanguageClient.Cohost.Formatting;
using Xunit;
using Xunit.Abstractions;
using AssertEx = Roslyn.Test.Utilities.AssertEx;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Test.Cohost.Formatting;

[Collection(HtmlFormattingCollection.Name)]
public class HtmlFormattingPassTest(FormattingTestContext context, HtmlFormattingFixture fixture, ITestOutputHelper testOutput)
    : FormattingTestBase(context, fixture.Service, testOutput), IClassFixture<FormattingTestContext>
{
    [Theory]
    [WorkItem("https://github.com/dotnet/razor/issues/11846")]
    [InlineData("", "")]
    [InlineData("$", "")]
    [InlineData("", "u8")]
    [InlineData("$", "u8")]
    [InlineData("@", "")]
    [InlineData("@$", "")]
    [InlineData(@"""""""", @"""""""")]
    [InlineData(@"$""""""", @"""""""")]
    [InlineData(@"""""""\r\n", @"\r\n""""""")]
    [InlineData(@"$""""""\r\n", @"\r\n""""""")]
    [InlineData(@"""""""", @"""""""u8")]
    [InlineData(@"$""""""", @"""""""u8")]
    [InlineData(@"""""""\r\n", @"\r\n""""""u8")]
    [InlineData(@"$""""""\r\n", @"\r\n""""""u8")]
    public async Task RemoveEditThatSplitsStringLiteral(string prefix, string suffix)
    {
        var document = CreateProjectAndRazorDocument($"""
            @({prefix}"this is a line that is 46 characters long"{suffix})
            """);
        var change = new TextChange(new TextSpan(24, 0), "\r\n");
        var edits = await GetHtmlFormattingEditsAsync(document, change);
        Assert.Empty(edits);
    }

    [Fact]
    public async Task FilterOutHtmlEdits()
    {
        TestCode input = """
            <div>
            </div>
            <div>
                <span>
                    Test
                </span>
            </div>
            <script>
            $$   script1
            </script>
            <div>
                <script>
            $$        script2
                </script>
            </div>
            <style>
            $$     style1
            </style>
            <div>
                <style>
            $$        style2
                </style>
            </div>
            <script>hello</script>
            <div><script>hello</script></div>
            <script>
            $$hello</script>
            <div><script>
            $$hello</script></div>
            <script>
            </script>
            """;

        var document = CreateProjectAndRazorDocument(input.Text);
        var sourceText = SourceText.From(input.Text);
        var changes = ImmutableArray.CreateBuilder<TextChange>();

        // Create an edit to "indent" every line. Using $$ makes test assertions easier.
        foreach (var line in sourceText.Lines)
        {
            changes.Add(new TextChange(new TextSpan(line.Start, 0), "$$"));
        }

        var edits = await GetHtmlFormattingEditsAsync(document, changes.ToImmutable());

        var newDoc = sourceText.WithChanges(edits);
        AssertEx.EqualOrDiff(input.OriginalInput, newDoc.ToString());
    }

    private async Task<ImmutableArray<TextChange>> GetHtmlFormattingEditsAsync(CodeAnalysis.TextDocument document, params ImmutableArray<TextChange> changes)
    {
        var documentMappingService = OOPExportProvider.GetExportedValue<IDocumentMappingService>();
        var pass = new HtmlFormattingPass(documentMappingService);

        var snapshotManager = OOPExportProvider.GetExportedValue<RemoteSnapshotManager>();
        var snapshot = snapshotManager.GetSnapshot(document);

        var loggerFactory = new TestFormattingLoggerFactory(TestOutputHelper);
        var logger = loggerFactory.CreateLogger(document.FilePath.AssumeNotNull(), "Html");
        var codeDocument = await snapshot.GetGeneratedOutputAsync(DisposalToken);
        var context = FormattingContext.Create(snapshot,
            codeDocument,
            new RazorFormattingOptions(),
            logger);

        var edits = await pass.GetTestAccessor().FilterIncomingChangesAsync(context, changes, DisposalToken);
        return edits;
    }
}
