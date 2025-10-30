// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
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
        var options = (TempRazorFormattingOptions)JsonSerializer.Deserialize(optionsFile, typeof(TempRazorFormattingOptions), JsonHelpers.JsonSerializerOptions).AssumeNotNull();

        var formattingService = (RazorFormattingService)OOPExportProvider.GetExportedValue<IRazorFormattingService>();
        formattingService.GetTestAccessor().SetFormattingLoggerFactory(new TestFormattingLoggerFactory(TestOutputHelper));

        var htmlChangesFile = GetResource("HtmlChanges.json");
        var htmlChanges = JsonSerializer.Deserialize<RazorTextChange[]>(htmlChangesFile, JsonHelpers.JsonSerializerOptions);
        var sourceText = await document.GetTextAsync();
        var htmlEdits = htmlChanges.Select(c => sourceText.GetTextEdit(c.ToTextChange())).ToArray();

        await GetFormattingEditsAsync(document, htmlEdits, span: default, options.CodeBlockBraceOnNextLine, options.InsertSpaces, options.TabSize, options.ToRazorFormattingOptions().CSharpSyntaxFormattingOptions.AssumeNotNull());
    }

    private string GetResource(string name, [CallerMemberName] string? testName = null)
    {
        var baselineFileName = $@"TestFiles\FormattingLog\{testName}\{name}";

        var testFile = TestFile.Create(baselineFileName, GetType().Assembly);
        Assert.True(testFile.Exists());

        return testFile.ReadAllText();
    }

    // HACK: Temporary types for deserializing because RazorCSharpSyntaxFormattingOptions doesn't have a parameterless constructor.
    internal class TempRazorFormattingOptions()
    {
        [DataMember(Order = 0)]
        public bool InsertSpaces { get; init; } = true;
        [DataMember(Order = 1)]
        public int TabSize { get; init; } = 4;
        [DataMember(Order = 2)]
        public bool CodeBlockBraceOnNextLine { get; init; } = false;
        [DataMember(Order = 3)]
        public TempRazorCSharpSyntaxFormattingOptions? CSharpSyntaxFormattingOptions { get; init; }

        public RazorFormattingOptions ToRazorFormattingOptions()
            => new()
            {
                InsertSpaces = InsertSpaces,
                TabSize = TabSize,
                CodeBlockBraceOnNextLine = CodeBlockBraceOnNextLine,
                CSharpSyntaxFormattingOptions = CSharpSyntaxFormattingOptions is not null
                    ? new RazorCSharpSyntaxFormattingOptions(
                        CSharpSyntaxFormattingOptions.Spacing,
                        CSharpSyntaxFormattingOptions.SpacingAroundBinaryOperator,
                        CSharpSyntaxFormattingOptions.NewLines,
                        CSharpSyntaxFormattingOptions.LabelPositioning,
                        CSharpSyntaxFormattingOptions.Indentation,
                        CSharpSyntaxFormattingOptions.WrappingKeepStatementsOnSingleLine,
                        CSharpSyntaxFormattingOptions.WrappingPreserveSingleLine,
                        CSharpSyntaxFormattingOptions.NamespaceDeclarations,
                        CSharpSyntaxFormattingOptions.PreferTopLevelStatements,
                        CSharpSyntaxFormattingOptions.CollectionExpressionWrappingLength)
                    : RazorCSharpSyntaxFormattingOptions.Default
            };
    }

    [DataContract]
    internal sealed record class TempRazorCSharpSyntaxFormattingOptions(
        [property: DataMember] RazorSpacePlacement Spacing,
        [property: DataMember] RazorBinaryOperatorSpacingOptions SpacingAroundBinaryOperator,
        [property: DataMember] RazorNewLinePlacement NewLines,
        [property: DataMember] RazorLabelPositionOptions LabelPositioning,
        [property: DataMember] RazorIndentationPlacement Indentation,
        [property: DataMember] bool WrappingKeepStatementsOnSingleLine,
        [property: DataMember] bool WrappingPreserveSingleLine,
        [property: DataMember] RazorNamespaceDeclarationPreference NamespaceDeclarations,
        [property: DataMember] bool PreferTopLevelStatements,
        [property: DataMember] int CollectionExpressionWrappingLength)
    {
        public TempRazorCSharpSyntaxFormattingOptions()
            : this(
                  default,
                  default,
                  default,
                  default,
                  default,
                  default,
                  default,
                  default,
                  true,
                  default)
        {
        }
    }
}
