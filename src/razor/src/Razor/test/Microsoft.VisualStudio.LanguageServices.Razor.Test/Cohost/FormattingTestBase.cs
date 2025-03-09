// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.Settings;
using Microsoft.VisualStudio.Threading;
using Roslyn.Test.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public abstract class FormattingTestBase : CohostEndpointTestBase
{
    private readonly FormattingTestContext _context;
    private readonly HtmlFormattingService _htmlFormattingService;

    private protected FormattingTestBase(FormattingTestContext context, HtmlFormattingService htmlFormattingService, ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
        ITestOnlyLoggerExtensions.TestOnlyLoggingEnabled = true;

        _context = context;
        _htmlFormattingService = htmlFormattingService;
    }

    private protected async Task RunFormattingTestAsync(
        TestCode input,
        string expected,
        string? fileKind = null,
        bool inGlobalNamespace = false,
        bool codeBlockBraceOnNextLine = false,
        bool insertSpaces = true,
        int tabSize = 4,
        bool allowDiagnostics = false)
    {
        (input, expected) = ProcessFormattingContext(input, expected);

        var document = CreateProjectAndRazorDocument(input.Text, fileKind, inGlobalNamespace: inGlobalNamespace);
        if (!allowDiagnostics)
        {
            //TODO: Tests in LanguageServer have extra components that are not present in this project, like Counter, etc.
            //      so we can't validate for diagnostics here until we make them the same. Since the test inputs are all
            //      shared this doesn't really matter while the language server tests are present.
            //var snapshotManager = OOPExportProvider.GetExportedValue<RemoteSnapshotManager>();
            //var snapshot = snapshotManager.GetSnapshot(document);
            //var codeDocument = await snapshot.GetGeneratedOutputAsync(DisposalToken);
            //var csharpDocument = codeDocument.GetCSharpDocument();
            //Assert.False(csharpDocument.Diagnostics.Any(), "Error creating document:" + Environment.NewLine + string.Join(Environment.NewLine, csharpDocument.Diagnostics));
        }

        var htmlDocumentPublisher = new HtmlDocumentPublisher(RemoteServiceInvoker, StrictMock.Of<TrackingLSPDocumentManager>(), JoinableTaskContext, LoggerFactory);
        var generatedHtml = await htmlDocumentPublisher.GetHtmlSourceFromOOPAsync(document, DisposalToken);
        Assert.NotNull(generatedHtml);

        var uri = new Uri(document.CreateUri(), $"{document.FilePath}{FeatureOptions.HtmlVirtualDocumentSuffix}");
        var htmlEdits = await _htmlFormattingService.GetDocumentFormattingEditsAsync(LoggerFactory, uri, generatedHtml, insertSpaces, tabSize);

        var requestInvoker = new TestLSPRequestInvoker([(Methods.TextDocumentFormattingName, htmlEdits)]);

        var clientSettingsManager = new ClientSettingsManager(changeTriggers: []);
        clientSettingsManager.Update(clientSettingsManager.GetClientSettings().AdvancedSettings with { CodeBlockBraceOnNextLine = codeBlockBraceOnNextLine });

        var span = input.TryGetNamedSpans(string.Empty, out var spans)
            ? spans.First()
            : default;
        var edits = await GetFormattingEditsAsync(span, insertSpaces, tabSize, document, requestInvoker, clientSettingsManager);

        if (edits is null)
        {
            AssertEx.EqualOrDiff(expected, input.Text);
            return;
        }

        var inputText = await document.GetTextAsync(DisposalToken);
        var changes = edits.Select(inputText.GetTextChange);
        var finalText = inputText.WithChanges(changes);

        AssertEx.EqualOrDiff(expected, finalText.ToString());
    }

    private protected async Task RunOnTypeFormattingTestAsync(
        TestCode input,
        string expected,
        char triggerCharacter,
        bool inGlobalNamespace = false,
        bool insertSpaces = true,
        int tabSize = 4,
        string? fileKind = null,
        int? expectedChangedLines = null)
    {
        (input, expected) = ProcessFormattingContext(input, expected);

        var document = CreateProjectAndRazorDocument(input.Text, fileKind: fileKind, inGlobalNamespace: inGlobalNamespace);
        var inputText = await document.GetTextAsync(DisposalToken);
        var position = inputText.GetPosition(input.Position);

        var htmlDocumentPublisher = new HtmlDocumentPublisher(RemoteServiceInvoker, StrictMock.Of<TrackingLSPDocumentManager>(), StrictMock.Of<JoinableTaskContext>(), LoggerFactory);
        var generatedHtml = await htmlDocumentPublisher.GetHtmlSourceFromOOPAsync(document, DisposalToken);
        Assert.NotNull(generatedHtml);

        var uri = new Uri(document.CreateUri(), $"{document.FilePath}{FeatureOptions.HtmlVirtualDocumentSuffix}");
        var htmlEdits = await _htmlFormattingService.GetOnTypeFormattingEditsAsync(LoggerFactory, uri, generatedHtml, position, insertSpaces: true, tabSize: 4);

        var requestInvoker = new TestLSPRequestInvoker([(Methods.TextDocumentOnTypeFormattingName, htmlEdits)]);

        var clientSettingsManager = new ClientSettingsManager(changeTriggers: []);

        var endpoint = new CohostOnTypeFormattingEndpoint(RemoteServiceInvoker, TestHtmlDocumentSynchronizer.Instance, requestInvoker, clientSettingsManager, LoggerFactory);

        var request = new DocumentOnTypeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = document.CreateUri() },
            Options = new FormattingOptions()
            {
                TabSize = tabSize,
                InsertSpaces = insertSpaces
            },
            Character = triggerCharacter.ToString(),
            Position = position
        };

        var edits = await endpoint.GetTestAccessor().HandleRequestAsync(request, document, DisposalToken);

        if (edits is null)
        {
            Assert.Equal(expected, input.Text);
            return;
        }

        var changes = edits.Select(inputText.GetTextChange);
        var finalText = inputText.WithChanges(changes);

        AssertEx.EqualOrDiff(expected, finalText.ToString());

        if (expectedChangedLines is { } changedLines)
        {
            var firstLine = changes.Min(e => inputText.GetLinePositionSpan(e.Span).Start.Line);
            var lastLine = changes.Max(e => inputText.GetLinePositionSpan(e.Span).End.Line);
            var delta = lastLine - firstLine + changes.Count(e => e.NewText.AssumeNotNull().Contains(Environment.NewLine));
            Assert.Equal(changedLines, delta + 1);
        }
    }

    private (TestCode, string) ProcessFormattingContext(TestCode input, string expected)
    {
        Assert.True(_context.CreatedByFormattingDiscoverer, "Test class is using FormattingTestContext, but not using [FormattingTestFact] or [FormattingTestTheory]");

        if (_context.ShouldFlipLineEndings)
        {
            // flip the line endings of the stings (LF to CRLF and vice versa) and run again
            input = new TestCode(_context.FlipLineEndings(input.OriginalInput));
            expected = _context.FlipLineEndings(expected);
        }

        return (input, expected);
    }

    private async Task<TextEdit[]?> GetFormattingEditsAsync(TextSpan span, bool insertSpaces, int tabSize, TextDocument document, LSPRequestInvoker requestInvoker, IClientSettingsManager clientSettingsManager)
    {
        if (span.IsEmpty)
        {
            var endpoint = new CohostDocumentFormattingEndpoint(RemoteServiceInvoker, TestHtmlDocumentSynchronizer.Instance, requestInvoker, clientSettingsManager, LoggerFactory);
            var request = new DocumentFormattingParams()
            {
                TextDocument = new TextDocumentIdentifier() { Uri = document.CreateUri() },
                Options = new FormattingOptions()
                {
                    TabSize = tabSize,
                    InsertSpaces = insertSpaces
                }
            };

            return await endpoint.GetTestAccessor().HandleRequestAsync(request, document, DisposalToken);
        }

        var inputText = await document.GetTextAsync(DisposalToken);
        var rangeEndpoint = new CohostRangeFormattingEndpoint(RemoteServiceInvoker, TestHtmlDocumentSynchronizer.Instance, requestInvoker, clientSettingsManager, LoggerFactory);
        var rangeRequest = new DocumentRangeFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier() { Uri = document.CreateUri() },
            Options = new FormattingOptions()
            {
                TabSize = 4,
                InsertSpaces = true
            },
            Range = inputText.GetRange(span)
        };

        return await rangeEndpoint.GetTestAccessor().HandleRequestAsync(rangeRequest, document, DisposalToken);
    }
}
