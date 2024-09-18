// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodBeAnalysis.Remote.Razor.AutoInsert;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.AutoInsert;
using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServices.Razor.LanguageClient.Cohost;
using Microsoft.VisualStudio.Razor.Settings;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class CohostOnAutoInsertEndpointTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    [Theory]
    [InlineData("PageTitle")]
    [InlineData("div")]
    [InlineData("text")]
    public async Task EndTag(string startTag)
    {
        await VerifyOnAutoInsertAsync(
            input: $"""
                This is a Razor document.

                <{startTag}>$$

                The end.
                """,
            output: $"""
                This is a Razor document.

                <{startTag}>$0</{startTag}>

                The end.
                """,
            triggerCharacter: ">");
    }

    [Theory]
    [InlineData("PageTitle")]
    [InlineData("div")]
    [InlineData("text")]
    public async Task DoNotAutoInsertEndTag_DisabledAutoClosingTags(string startTag)
    {
        await VerifyOnAutoInsertAsync(
            input: $"""
                This is a Razor document.

                <{startTag}>$$

                The end.
                """,
            output: null,
            triggerCharacter: ">",
            autoClosingTags: false);
    }

    [Fact]
    public async Task AttributeQuotes()
    {
        await VerifyOnAutoInsertAsync(
            input: $"""
                This is a Razor document.

                <PageTitle style=$$></PageTitle>

                The end.
                """,
            output: $"""
                This is a Razor document.

                <PageTitle style="$0"></PageTitle>

                The end.
                """,
            triggerCharacter: "=",
            delegatedResponseText: "\"$0\"");
    }

    [Fact]
    public async Task CSharp_OnForwardSlash()
    {
        await VerifyOnAutoInsertAsync(
            input: """
                @code {
                    ///$$
                    void TestMethod() {}
                }
                """,
            output: """
                @code {
                    /// <summary>
                    /// $0
                    /// </summary>
                    void TestMethod() {}
                }
                """,
            triggerCharacter: "/");
    }

    [Fact]
    public async Task DoNotAutoInsertCSharp_OnForwardSlashWithFormatOnTypeDisabled()
    {
        await VerifyOnAutoInsertAsync(
            input: """
                @code {
                    ///$$
                    void TestMethod() {}
                }
                """,
            output: null,
            triggerCharacter: "/",
            formatOnType: false);
    }

    [Fact]
    public async Task CSharp_OnEnter()
    {
        await VerifyOnAutoInsertAsync(
            input: """
                @code {
                    void TestMethod() {
                $$}
                }
                """,
            output: """
                @code {
                    void TestMethod()
                    {
                        $0
                    }
                }
                """,
            triggerCharacter: "\n");
    }

    [Fact]
    public async Task CSharp_OnEnter_TwoSpaceIndent()
    {
        await VerifyOnAutoInsertAsync(
            input: """
                @code {
                    void TestMethod() {
                $$}
                }
                """,
            output: """
                @code {
                  void TestMethod()
                  {
                    $0
                  }
                }
                """,
            triggerCharacter: "\n",
            tabSize: 2);
    }

    [Fact]
    public async Task CSharp_OnEnter_UseTabs()
    {
        const char tab = '\t';
        await VerifyOnAutoInsertAsync(
            input: """
                @code {
                    void TestMethod() {
                $$}
                }
                """,
            output: $$"""
                @code {
                {{tab}}void TestMethod()
                {{tab}}{
                {{tab}}{{tab}}$0
                {{tab}}}
                }
                """,
            triggerCharacter: "\n",
            insertSpaces: false);
    }

    private async Task VerifyOnAutoInsertAsync(
        TestCode input,
        string? output,
        string triggerCharacter,
        string? delegatedResponseText = null,
        bool insertSpaces = true,
        int tabSize = 4,
        bool formatOnType = true,
        bool autoClosingTags = true)
    {     
        var document = await CreateProjectAndRazorDocumentAsync(input.Text);
        var sourceText = await document.GetTextAsync(DisposalToken);

        var clientSettingsManager = new ClientSettingsManager([], null, null);
        clientSettingsManager.Update(ClientAdvancedSettings.Default with { FormatOnType = formatOnType, AutoClosingTags = autoClosingTags });

        IOnAutoInsertTriggerCharacterProvider[] onAutoInsertTriggerCharacterProviders = [
            new RemoteAutoClosingTagOnAutoInsertProvider(),
            new RemoteCloseTextTagOnAutoInsertProvider()];

        VSInternalDocumentOnAutoInsertResponseItem? response = null;
        if (delegatedResponseText is not null)
        {
            var start = sourceText.GetPosition(input.Position);
            var end = start;
            response = new VSInternalDocumentOnAutoInsertResponseItem()
            {
                TextEdit = new TextEdit() { NewText = delegatedResponseText, Range = new() { Start = start, End = end } },
                TextEditFormat = InsertTextFormat.Snippet
            };
        }

        var requestInvoker = new TestLSPRequestInvoker([(VSInternalMethods.OnAutoInsertName, response)]);

        var endpoint = new CohostOnAutoInsertEndpoint(
            RemoteServiceInvoker,
            clientSettingsManager,
            onAutoInsertTriggerCharacterProviders,
            TestHtmlDocumentSynchronizer.Instance,
            requestInvoker,
            LoggerFactory);

        var formattingOptions = new FormattingOptions()
        {
            InsertSpaces = insertSpaces,
            TabSize = tabSize
        };

        var request = new VSInternalDocumentOnAutoInsertParams()
        {
            TextDocument = new TextDocumentIdentifier()
            {
                Uri = document.CreateUri()
            },
            Position = sourceText.GetPosition(input.Position),
            Character = triggerCharacter,
            Options = formattingOptions
        };

        var result = await endpoint.GetTestAccessor().HandleRequestAsync(request, document, DisposalToken);

        if (output is not null)
        {
            Assert.NotNull(result);
        }
        else
        {
            Assert.Null(result);
            return;
        }

        if (result is not null)
        {
            var change = sourceText.GetTextChange(result.TextEdit);
            sourceText = sourceText.WithChanges(change);
        }

        AssertEx.EqualOrDiff(output, sourceText.ToString());
    }
}
