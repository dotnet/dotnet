// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Settings;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.Razor.Settings;
using Microsoft.VisualStudio.Razor.Snippets;
using Roslyn.Test.Utilities;
using Roslyn.Text.Adornments;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public class CohostDocumentCompletionEndpointTest(ITestOutputHelper testOutputHelper) : CohostEndpointTestBase(testOutputHelper)
{
    [Fact]
    public async Task CSharpInEmptyExplicitStatement()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                @{
                    $$
                }

                The end.
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Explicit,
                TriggerKind = CompletionTriggerKind.Invoked
            },
            expectedItemLabels: ["var", "char", "DateTime", "Exception"]);
    }

    [Fact]
    public async Task CSharpClassesAtTransition()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                <div>@$$</div>

                The end.
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Typing,
                TriggerCharacter = "@",
                TriggerKind = CompletionTriggerKind.TriggerCharacter
            },
            expectedItemLabels: ["char", "DateTime", "Exception"],
            itemToResolve: "DateTime",
            expectedResolvedItemDescription: "readonly struct System.DateTime");
    }

    [Fact]
    public async Task CSharpClassMembersAtProvisionalCompletion()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                <div>@DateTime.$$</div>

                The end.
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Typing,
                TriggerCharacter = ".",
                TriggerKind = CompletionTriggerKind.TriggerCharacter
            },
            expectedItemLabels: ["DaysInMonth", "IsLeapYear", "Now"]);
    }

    [Fact]
    public async Task CSharpClassesInCodeBlock()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                <div></div>

                @code{ $$ }

                The end.
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Explicit,
                TriggerCharacter = null,
                TriggerKind = CompletionTriggerKind.Invoked
            },
            expectedItemLabels: ["char", "DateTime", "Exception"]);
    }

    [Fact]
    public async Task CSharpClassMembersInCodeBlock()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                <div></div>

                @code{
                    void foo()
                    {
                        DateTime.$$
                    }
                }

                The end.
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Typing,
                TriggerCharacter = ".",
                TriggerKind = CompletionTriggerKind.TriggerCharacter
            },
            expectedItemLabels: ["DaysInMonth", "IsLeapYear", "Now"]);
    }

    [Fact]
    public async Task CSharpOverrideMethods()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                <div></div>

                @code {
                    public override $$
                }

                The end.
                """,
            expected: """
                This is a Razor document.
            
                <div></div>
            
                @code {
                    public override Task SetParametersAsync(ParameterView parameters)
                    {
                        return base.SetParametersAsync(parameters);
                    }
                }
            
                The end.
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Explicit,
                TriggerCharacter = null,
                TriggerKind = CompletionTriggerKind.Invoked
            },
            expectedItemLabels: ["Equals(object? obj)", "GetHashCode()", "SetParametersAsync(ParameterView parameters)", "ToString()"],
            itemToResolve: "SetParametersAsync(ParameterView parameters)",
            expectedResolvedItemDescription: "(awaitable) Task ComponentBase.SetParametersAsync(ParameterView parameters)");
    }

    [Fact]
    public async Task CSharpOverrideMethods_VSCode()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                <div></div>

                @code {
                    public override $$
                }

                The end.
                """,
            expected: """
                This is a Razor document.
            
                <div></div>
            
                @code {
                    public override Task SetParametersAsync(ParameterView parameters)
                    {
                        return base.SetParametersAsync(parameters);
                    }
                }
            
                The end.
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Explicit,
                TriggerCharacter = null,
                TriggerKind = CompletionTriggerKind.Invoked
            },
            expectedItemLabels: ["Equals(object? obj)", "GetHashCode()", "SetParametersAsync(ParameterView parameters)", "ToString()"],
            itemToResolve: "SetParametersAsync(ParameterView parameters)",
            expectedResolvedItemDescription: "(awaitable) Task ComponentBase.SetParametersAsync(ParameterView parameters)",
            supportsVisualStudioExtensions: false);
    }

    // Tests MarkupTransitionCompletionItemProvider
    [Fact]
    public async Task CSharpMarkupTransitionAndTagHelpersInCodeBlock()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                <div></div>

                @code{
                    void foo()
                    {
                        <$$
                    }
                }

                The end.
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Typing,
                TriggerCharacter = "<",
                TriggerKind = CompletionTriggerKind.TriggerCharacter
            },
            expectedItemLabels: ["text", "EditForm", "InputDate", "div"],
            htmlItemLabels: ["div"]);
    }

    [Fact]
    public async Task RazorDirectives()
    {
        var expectedDirectiveLabels = new string[]
            {
                "attribute", "implements", "inherits", "inject", "layout", "namespace", "page",
                "preservewhitespace", "typeparam", "using"
            };
        var expectedDirectiveSnippetLabels = expectedDirectiveLabels.Select(label => $"{label} directive ...");
        var expectedCSharpLabels = new string[] { "char", "DateTime", "Exception" };
        var expectedLabels = expectedDirectiveLabels
            .Concat(expectedDirectiveSnippetLabels)
            .Concat(expectedCSharpLabels)
            .ToArray();

        await VerifyCompletionListAsync(
            input: """
                @$$
                This is a Razor document.

                <div></div>

                @code{
                    void foo()
                    {
                        
                    }
                }

                The end.
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Typing,
                TriggerCharacter = "@",
                TriggerKind = CompletionTriggerKind.TriggerCharacter
            },
            expectedItemLabels: expectedLabels,
            itemToResolve: "page",
            expectedResolvedItemDescription: "Mark the page as a routable component.");
    }

    [Fact]
    public async Task ElementNameTagHelpersCompletion()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                <$$

                The end.
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Typing,
                TriggerCharacter = "<",
                TriggerKind = CompletionTriggerKind.TriggerCharacter
            },
            expectedItemLabels: ["LayoutView", "EditForm", "ValidationMessage", "div"],
            htmlItemLabels: ["div"],
            itemToResolve: "EditForm",
            expectedResolvedItemDescription: "Microsoft.AspNetCore.Components.Forms.EditForm");
    }

    [Fact]
    public async Task HtmlElementNamesAndTagHelpersCompletion()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                <$$

                The end.
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Typing,
                TriggerCharacter = "<",
                TriggerKind = CompletionTriggerKind.TriggerCharacter
            },
            expectedItemLabels: ["div", "h1", "LayoutView", "EditForm", "ValidationMessage"],
            htmlItemLabels: ["div", "h1"]);
    }

    [Fact]
    public async Task HtmlElementNamesAndTagHelpersCompletion_EndOfDocument()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                <$$
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Typing,
                TriggerCharacter = "<",
                TriggerKind = CompletionTriggerKind.TriggerCharacter
            },
            expectedItemLabels: ["div", "h1", "LayoutView", "EditForm", "ValidationMessage"],
            htmlItemLabels: ["div", "h1"],
            unexpectedItemLabels: ["snippet1", "snippet2"]);
    }

    [Fact]
    public async Task HtmlElementDoNotCommitWithSpace()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                <$$

                The end.
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Typing,
                TriggerCharacter = "<",
                TriggerKind = CompletionTriggerKind.TriggerCharacter
            },
            expectedItemLabels: ["div", "h1", "LayoutView", "EditForm", "ValidationMessage"],
            htmlItemLabels: ["div", "h1"],
            htmlItemCommitCharacters: [" ", ">"],
            commitElementsWithSpace: false);
    }

    [Fact]
    public async Task HtmlSnippetsCompletion()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                $$

                The end.
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Explicit,
                TriggerCharacter = null,
                TriggerKind = CompletionTriggerKind.Invoked
            },
            expectedItemLabels: ["snippet1", "snippet2"],
            htmlItemLabels: [],
            snippetLabels: ["snippet1", "snippet2"]);
    }

    [Fact]
    public async Task HtmlSnippetsCompletion_EmptyDocument()
    {
        await VerifyCompletionListAsync(
            input: """
                $$
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Explicit,
                TriggerCharacter = null,
                TriggerKind = CompletionTriggerKind.Invoked
            },
            expectedItemLabels: ["snippet1", "snippet2"],
            htmlItemLabels: [],
            snippetLabels: ["snippet1", "snippet2"]);
    }

    [Fact]
    public async Task HtmlSnippetsCompletion_WhitespaceOnlyDocument1()
    {
        await VerifyCompletionListAsync(
            input: """

                $$
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Explicit,
                TriggerCharacter = null,
                TriggerKind = CompletionTriggerKind.Invoked
            },
            expectedItemLabels: ["snippet1", "snippet2"],
            htmlItemLabels: [],
            snippetLabels: ["snippet1", "snippet2"]);
    }

    [Fact]
    public async Task HtmlSnippetsCompletion_WhitespaceOnlyDocument2()
    {
        await VerifyCompletionListAsync(
            input: """
                $$

                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Explicit,
                TriggerCharacter = null,
                TriggerKind = CompletionTriggerKind.Invoked
            },
            expectedItemLabels: ["snippet1", "snippet2"],
            htmlItemLabels: [],
            snippetLabels: ["snippet1", "snippet2"]);
    }

    [Fact]
    public async Task HtmlSnippetsCompletion_NotInStartTag()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                <div $$></div>

                The end.
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Typing,
                TriggerCharacter = " ",
                TriggerKind = CompletionTriggerKind.TriggerCharacter
            },
            expectedItemLabels: ["style", "dir"],
            unexpectedItemLabels: ["snippet1", "snippet2"],
            htmlItemLabels: ["style", "dir"],
            snippetLabels: ["snippet1", "snippet2"]);
    }

    // Tests HTML attributes and DirectiveAttributeTransitionCompletionItemProvider
    [Fact]
    public async Task HtmlAndDirectiveAttributeTransitionNamesCompletion()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                <div $$></div>

                The end.
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Typing,
                TriggerCharacter = " ",
                TriggerKind = CompletionTriggerKind.TriggerCharacter
            },
            expectedItemLabels: ["style", "dir", "@..."],
            htmlItemLabels: ["style", "dir"]);
    }

    // Tests HTML attributes and DirectiveAttributeCompletionItemProvider
    [Fact]
    public async Task HtmlAndDirectiveAttributeNamesCompletion()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                <div @$$></div>

                The end.
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Typing,
                TriggerCharacter = "@",
                TriggerKind = CompletionTriggerKind.TriggerCharacter
            },
            expectedItemLabels: ["style", "dir", "@rendermode", "@bind-..."],
            htmlItemLabels: ["style", "dir"],
            itemToResolve: "@rendermode",
            expectedResolvedItemDescription: """
                IComponentRenderMode Microsoft.AspNetCore.Components.RenderMode.RenderMode
                Specifies the render mode for a component.
                """);
    }

    // Tests HTML attributes and DirectiveAttributeParameterCompletionItemProvider
    [Fact]
    public async Task HtmlAndDirectiveAttributeParameterNamesCompletion()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                <input @bind:f$$></div>

                The end.
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Typing,
                TriggerCharacter = null,
                TriggerKind = CompletionTriggerKind.Invoked
            },
            expectedItemLabels: ["style", "dir", "culture", "event", "format", "get", "set", "after"],
            htmlItemLabels: ["style", "dir"]);
    }

    [Fact]
    public async Task HtmlAttributeNamesAndTagHelpersCompletion()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                <EditForm $$></EditForm>

                The end.
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Typing,
                TriggerCharacter = " ",
                TriggerKind = CompletionTriggerKind.TriggerCharacter
            },
            expectedItemLabels: ["style", "dir", "FormName", "OnValidSubmit", "@..."],
            htmlItemLabels: ["style", "dir"],
            itemToResolve: "FormName",
            expectedResolvedItemDescription: "string Microsoft.AspNetCore.Components.Forms.EditForm.FormName");
    }

    [Fact]
    public async Task HtmlAttributeNamesAndTagHelpersCompletion_EndOfDocument()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                <EditForm $$
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Explicit,
                TriggerCharacter = null,
                TriggerKind = CompletionTriggerKind.Invoked
            },
            expectedItemLabels: ["style", "dir", "FormName", "OnValidSubmit", "@..."],
            htmlItemLabels: ["style", "dir"]);
    }

    [Fact]
    public async Task TagHelperAttributes_NoAutoInsertQuotes_Completion()
    {
        await VerifyCompletionListAsync(
            input: """
                This is a Razor document.

                <EditForm $$></EditForm>

                The end.
                """,
            completionContext: new VSInternalCompletionContext()
            {
                InvokeKind = VSInternalCompletionInvokeKind.Typing,
                TriggerCharacter = " ",
                TriggerKind = CompletionTriggerKind.TriggerCharacter
            },
            expectedItemLabels: ["FormName", "OnValidSubmit", "@...", "style"],
            htmlItemLabels: ["style"],
            autoInsertAttributeQuotes: false);
    }

    private async Task VerifyCompletionListAsync(
        TestCode input,
        VSInternalCompletionContext completionContext,
        string[] expectedItemLabels,
        string[]? unexpectedItemLabels = null,
        string[]? htmlItemLabels = null,
        string[]? htmlItemCommitCharacters = null,
        string[]? snippetLabels = null,
        string? itemToResolve = null,
        string? expected = null,
        string? expectedResolvedItemDescription = null,
        bool autoInsertAttributeQuotes = true,
        bool commitElementsWithSpace = true,
        bool supportsVisualStudioExtensions = true)
    {
        UpdateClientLSPInitializationOptions(c =>
        {
            c.ClientCapabilities.SupportsVisualStudioExtensions = supportsVisualStudioExtensions;
            return c;
        });

        var document = CreateProjectAndRazorDocument(input.Text);
        var sourceText = await document.GetTextAsync(DisposalToken);

        var clientSettingsManager = new ClientSettingsManager([], null, null);
        clientSettingsManager.Update(ClientAdvancedSettings.Default with { AutoInsertAttributeQuotes = autoInsertAttributeQuotes, CommitElementsWithSpace = commitElementsWithSpace });

        const string InvalidLabel = "_INVALID_";

        // If delegatedItemLabels wasn't supplied, supply our own to ensure delegation isn't happening and causing a false positive result
        htmlItemLabels ??= [InvalidLabel];
        var response = new RazorVSInternalCompletionList()
        {
            Items = [.. htmlItemLabels.Select((label) => new VSInternalCompletionItem()
            {
                Label = label,
                CommitCharacters = htmlItemCommitCharacters,
                // If test specifies not to commit with space, set kind to element since we remove space
                // commit from elements only. Otherwise test doesn't care, so set to None
                Kind = !commitElementsWithSpace ? CompletionItemKind.Element : CompletionItemKind.None,
            })],
            IsIncomplete = true
        };

        var requestInvoker = new TestHtmlRequestInvoker([(Methods.TextDocumentCompletionName, response)]);

        var snippetCompletionItemProvider = new SnippetCompletionItemProvider(new SnippetCache());
        // If snippetLabels wasn't supplied, supply our own to ensure snippets aren't being requested and causing a false positive result
        snippetLabels ??= [InvalidLabel];
        var snippetInfos = snippetLabels.Select(label => new SnippetInfo(label, label, label, string.Empty, SnippetLanguage.Html)).ToImmutableArray();
        snippetCompletionItemProvider.SnippetCache.Update(SnippetLanguage.Html, snippetInfos);

        var completionListCache = new CompletionListCache();
        var endpoint = new CohostDocumentCompletionEndpoint(
            RemoteServiceInvoker,
            clientSettingsManager,
            ClientCapabilitiesService,
            snippetCompletionItemProvider,
            TestLanguageServerFeatureOptions.Instance,
            requestInvoker,
            completionListCache,
            NoOpTelemetryReporter.Instance,
            LoggerFactory);

        var request = new CompletionParams()
        {
            TextDocument = new TextDocumentIdentifier()
            {
                Uri = document.CreateUri()
            },
            Position = sourceText.GetPosition(input.Position),
            Context = completionContext
        };

        var result = await endpoint.GetTestAccessor().HandleRequestAsync(request, document, DisposalToken);

        Assert.NotNull(result);

        using var _ = HashSetPool<string>.GetPooledObject(out var labelSet);
        labelSet.AddRange(result.Items.Select((item) => item.Label));

        Assert.DoesNotContain(InvalidLabel, labelSet);

        foreach (var expectedItemLabel in expectedItemLabels)
        {
            Assert.Contains(expectedItemLabel, labelSet);
        }

        if (unexpectedItemLabels is not null)
        {
            foreach (var unexpectedItemLabel in unexpectedItemLabels)
            {
                Assert.DoesNotContain(unexpectedItemLabel, labelSet);
            }
        }

        if (!commitElementsWithSpace)
        {
            Assert.False(result.Items.Any(item => item.CommitCharacters?.First().Contains(" ") ?? false));
        }

        if (!autoInsertAttributeQuotes)
        {
            // Tag helper attributes create InsertText that looks something like
            // "OnValidSubmit=\"$0\"" (for OnValidSubmit attribute). Make sure the value
            // placeholder $0 is not surrounded with quotes if we set AutoInsertAttributeQuotes
            // to false
            Assert.False(result.Items.Any(item => item.InsertText?.Contains("\"$0\"") ?? false));
        }

        if (itemToResolve is null)
        {
            return;
        }

        // In the real world the client will send us back the data for the item to resolve, but in tests its easier if we just set it here.
        // We clone the item first though, to ensure us setting the data doesn't hide a bug in our caching logic, around wrapping" the data.
        var item = Assert.Single(result.Items.Where(i => i.Label == itemToResolve));
        item = JsonSerializer.Deserialize<VSInternalCompletionItem>(JsonSerializer.SerializeToElement(item, JsonHelpers.JsonSerializerOptions), JsonHelpers.JsonSerializerOptions)!;
        item.Data ??= result.Data ?? result.ItemDefaults?.Data;

        Assert.NotNull(item);
        Assert.NotNull(expectedResolvedItemDescription);

        await VerifyCompletionResolveAsync(document, completionListCache, item, expected, expectedResolvedItemDescription);
    }

    private async Task VerifyCompletionResolveAsync(CodeAnalysis.TextDocument document, CompletionListCache completionListCache, VSInternalCompletionItem item, string? expected, string expectedResolvedItemDescription)
    {
        // We expect data to be a JsonElement, so for tests we have to _not_ strongly type
        item.Data = JsonSerializer.SerializeToElement(item.Data, JsonHelpers.JsonSerializerOptions);

        var clientSettingsManager = new ClientSettingsManager(changeTriggers: []);
        var endpoint = new CohostDocumentCompletionResolveEndpoint(
            completionListCache,
            RemoteServiceInvoker,
            clientSettingsManager,
            new TestHtmlRequestInvoker(),
            LoggerFactory);

        var tdi = endpoint.GetTestAccessor().GetRazorTextDocumentIdentifier(item);
        Assert.NotNull(tdi);
        Assert.Equal(document.CreateUri(), tdi.Value.Uri);

        var result = await endpoint.GetTestAccessor().HandleRequestAsync(item, document, DisposalToken);

        Assert.NotNull(result);

        if (result.TextEdit is { Value: TextEdit edit })
        {
            Assert.NotNull(expected);

            var text = await document.GetTextAsync(DisposalToken).ConfigureAwait(false);
            var changedText = text.WithChanges(text.GetTextChange(edit));

            AssertEx.EqualOrDiff(expected, changedText.ToString());
        }
        else if (result.Command is { Arguments: [_, TextEdit textEdit, ..] })
        {
            Assert.NotNull(expected);

            var text = await document.GetTextAsync(DisposalToken).ConfigureAwait(false);
            var changedText = text.WithChanges(text.GetTextChange(textEdit));

            AssertEx.EqualOrDiff(expected, changedText.ToString());
        }
        else if (expected is not null)
        {
            Assert.Fail("Expected a TextEdit or Command with TextEdit, but got none. Presumably resolve failed. Result: " + JsonSerializer.SerializeToElement(result).ToString());
        }

        if (result.Description is not null)
        {
            AssertEx.EqualOrDiff(expectedResolvedItemDescription, FlattenDescription(result.Description));
        }
        else if (result.Documentation is { Value: string description })
        {
            AssertEx.EqualOrDiff(expectedResolvedItemDescription, description);
        }
        else if (result.Documentation is { Value: MarkupContent { Kind.Value: "plaintext" } content })
        {
            AssertEx.EqualOrDiff(expectedResolvedItemDescription, content.Value);
        }
        else
        {
            Assert.Fail("Unhandled description type: " + JsonSerializer.SerializeToElement(result).ToString());
        }
    }

    private string? FlattenDescription(ClassifiedTextElement? description)
    {
        if (description is null)
        {
            return null;
        }

        var sb = new StringBuilder();
        foreach (var run in description.Runs)
        {
            sb.Append(run.Text);
        }

        return sb.ToString();
    }
}
