// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Test;
using Microsoft.VisualStudio.Text;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

public class DefaultLSPDocumentMappingProviderTest : TestBase
{
    private static readonly Uri s_razorFile = new("file:///some/folder/to/file.razor");
    private static readonly Uri s_razorVirtualCSharpFile = new("file:///some/folder/to/file.razor.ide.g.cs");
    private static readonly Uri s_anotherRazorFile = new("file:///some/folder/to/anotherfile.razor");
    private static readonly Uri s_anotherRazorVirtualCSharpFile = new("file:///some/folder/to/anotherfile.razor.ide.g.cs");
    private static readonly Uri s_csharpFile = new("file:///some/folder/to/csharpfile.cs");

    private readonly RazorLSPConventions _razorLSPConventions;
    private readonly Lazy<LSPDocumentManager> _documentManager;

    public DefaultLSPDocumentMappingProviderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var csharpVirtualDocumentSnapshot = new CSharpVirtualDocumentSnapshot(s_razorVirtualCSharpFile, new StringTextSnapshot(string.Empty), hostDocumentSyncVersion: 0);
        var documentSnapshot1 = new TestLSPDocumentSnapshot(s_razorFile, version: 1, "first doc", csharpVirtualDocumentSnapshot);
        var documentSnapshot2 = new TestLSPDocumentSnapshot(s_anotherRazorFile, version: 5, "second doc", csharpVirtualDocumentSnapshot);
        var documentManager = new TestDocumentManager();
        documentManager.AddDocument(s_razorFile, documentSnapshot1);
        documentManager.AddDocument(s_anotherRazorFile, documentSnapshot2);
        _documentManager = new Lazy<LSPDocumentManager>(() => documentManager);
        _razorLSPConventions = new RazorLSPConventions(TestLanguageServerFeatureOptions.Instance);
    }

    [Fact]
    public async Task RazorMapToDocumentRangeAsync_InvokesLanguageServer()
    {
        // Arrange
        var uri = new Uri("file:///some/folder/to/file.razor");

        var response = new RazorMapToDocumentRangesResponse()
        {
            Ranges = new[] {
                new Range()
                {
                    Start = new Position(1, 1),
                    End = new Position(3, 3),
                }
            },
            HostDocumentVersion = 1
        };
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<RazorMapToDocumentRangesParams, RazorMapToDocumentRangesResponse>(
                It.IsAny<ITextBuffer>(),
                LanguageServerConstants.RazorMapToDocumentRangesEndpoint,
                RazorLSPConstants.RazorLanguageServerName,
                It.IsAny<Func<JToken, bool>>(),
                It.IsAny<RazorMapToDocumentRangesParams>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReinvocationResponse<RazorMapToDocumentRangesResponse>("TestLanguageClient", response));

        var mappingProvider = new DefaultLSPDocumentMappingProvider(requestInvoker.Object, _documentManager, _razorLSPConventions);
        var projectedRange = new Range()
        {
            Start = new Position(10, 10),
            End = new Position(15, 15)
        };

        // Act
        var result = await mappingProvider.MapToDocumentRangesAsync(RazorLanguageKind.CSharp, uri, new[] { projectedRange }, DisposalToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.HostDocumentVersion);
        var actualRange = result.Ranges[0];
        Assert.Equal(new Position(1, 1), actualRange.Start);
        Assert.Equal(new Position(3, 3), actualRange.End);
    }

    [Fact]
    public async Task RemapLocationsAsync_ReturnsModifiedInstance()
    {
        // Arrange
        var uri = new Uri("file:///some/folder/to/file.razor.ide.g.cs");

        var response = new RazorMapToDocumentRangesResponse()
        {
            Ranges = new[] {
                new Range()
                {
                    Start = new Position(1, 1),
                    End = new Position(3, 3),
                }
            },
            HostDocumentVersion = 1
        };
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<RazorMapToDocumentRangesParams, RazorMapToDocumentRangesResponse>(
                It.IsAny<ITextBuffer>(),
                LanguageServerConstants.RazorMapToDocumentRangesEndpoint,
                RazorLSPConstants.RazorLanguageServerName,
                It.IsAny<Func<JToken, bool>>(),
                It.IsAny<RazorMapToDocumentRangesParams>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReinvocationResponse<RazorMapToDocumentRangesResponse>("TestLanguageClient", response));

        var mappingProvider = new DefaultLSPDocumentMappingProvider(requestInvoker.Object, _documentManager, _razorLSPConventions);
        var projected= new VSInternalLocation()
        {
            Uri = uri,
            Range = new Range()
            {
                Start = new Position(10, 10),
                End = new Position(15, 15)
            }
        };

        // Act
        var result = await mappingProvider.RemapLocationsAsync(new[] { projected }, DisposalToken);

        // Assert
        Assert.NotNull(result);
        var actualRange = result[0].Range;
        Assert.Equal(new Position(1, 1), actualRange.Start);
        Assert.Equal(new Position(3, 3), actualRange.End);

        // Ensuring the same type is returned ensures we don't lose any extra information
        Assert.IsType<VSInternalLocation>(result[0]);
    }

    [Fact]
    public async Task RemapWorkspaceEditAsync_RemapsEditsAsExpected()
    {
        // Arrange
        var expectedRange = new TestRange(1, 1, 1, 5);
        var expectedVersion = 1;

        var requestInvoker = GetRequestInvoker(new[]
        {
            ((RazorLanguageKind.CSharp, s_razorFile, new[] { new TestTextEdit("newText", new TestRange(10, 10, 10, 15)) }), (new[] { new TestTextEdit("newText", expectedRange) }, expectedVersion))
        });
        var mappingProvider = new DefaultLSPDocumentMappingProvider(requestInvoker, _documentManager, _razorLSPConventions);

        var workspaceEdit = new TestWorkspaceEdit(versionedEdits: true);
        workspaceEdit.AddEdits(s_razorVirtualCSharpFile, 10, new TestTextEdit("newText", new TestRange(10, 10, 10, 15)));

        // Act
        var result = await mappingProvider.RemapWorkspaceEditAsync(workspaceEdit, DisposalToken);

        // Assert
        var documentEdit = Assert.Single(result.DocumentChanges?.Value as TextDocumentEdit[]);
        Assert.Equal(s_razorFile, documentEdit.TextDocument.Uri);
        Assert.Equal(expectedVersion, documentEdit.TextDocument.Version);

        var actualEdit = Assert.Single(documentEdit.Edits);
        Assert.Equal("newText", actualEdit.NewText);
        Assert.Equal(expectedRange, actualEdit.Range);
    }

    [Fact]
    public async Task RemapWorkspaceEditAsync_DocumentChangesNull_RemapsEditsAsExpected()
    {
        // Arrange
        var expectedRange = new TestRange(1, 1, 1, 5);
        var expectedVersion = 1;

        var requestInvoker = GetRequestInvoker(new[]
        {
            ((RazorLanguageKind.CSharp, s_razorFile, new[] { new TestTextEdit("newText", new TestRange(10, 10, 10, 15)) }), (new[] { new TestTextEdit("newText", expectedRange) }, expectedVersion))
        });
        var mappingProvider = new DefaultLSPDocumentMappingProvider(requestInvoker, _documentManager, _razorLSPConventions);

        var workspaceEdit = new TestWorkspaceEdit(versionedEdits: false);
        workspaceEdit.AddEdits(s_razorVirtualCSharpFile, 10, new TestTextEdit("newText", new TestRange(10, 10, 10, 15)));

        // Act
        var result = await mappingProvider.RemapWorkspaceEditAsync(workspaceEdit, DisposalToken);

        // Assert
        Assert.Null(result.DocumentChanges);
        var change = Assert.Single(result.Changes);
        Assert.Equal(s_razorFile.AbsoluteUri, change.Key);

        var actualEdit = Assert.Single(change.Value);
        Assert.Equal("newText", actualEdit.NewText);
        Assert.Equal(expectedRange, actualEdit.Range);
    }

    [Fact]
    public async Task RemapWorkspaceEditAsync_DoesNotRemapsNonRazorFiles()
    {
        // Arrange
        var expectedRange = new TestRange(10, 10, 10, 15);
        var expectedVersion = 10;

        var requestInvoker = GetRequestInvoker(mappingPairs: null); // will throw if RequestInvoker is called.
        var mappingProvider = new DefaultLSPDocumentMappingProvider(requestInvoker, _documentManager, _razorLSPConventions);

        var workspaceEdit = new TestWorkspaceEdit(versionedEdits: true);
        workspaceEdit.AddEdits(s_csharpFile, expectedVersion, new TestTextEdit("newText", expectedRange));

        // Act
        var result = await mappingProvider.RemapWorkspaceEditAsync(workspaceEdit, DisposalToken);

        // Assert
        var documentEdit = Assert.Single(result.DocumentChanges?.Value as TextDocumentEdit[]);
        Assert.Equal(s_csharpFile, documentEdit.TextDocument.Uri);
        Assert.Equal(expectedVersion, documentEdit.TextDocument.Version);

        var actualEdit = Assert.Single(documentEdit.Edits);
        Assert.Equal("newText", actualEdit.NewText);
        Assert.Equal(expectedRange, actualEdit.Range);
    }

    [Fact]
    public async Task RemapWorkspaceEditAsync_RemapsMultipleRazorFiles()
    {
        // Arrange
        var expectedRange1 = new TestRange(1, 1, 1, 5);
        var expectedRange2 = new TestRange(5, 5, 5, 10);
        var expectedVersion1 = 1;
        var expectedVersion2 = 5;

        var requestInvoker = GetRequestInvoker(new[]
        {
            ((RazorLanguageKind.CSharp, s_razorFile, new[] { new TestTextEdit("newText", new TestRange(10, 10, 10, 15)) }), (new[] { new TestTextEdit("newText", expectedRange1) }, expectedVersion1)),
            ((RazorLanguageKind.CSharp, s_anotherRazorFile, new[] { new TestTextEdit("newText", new TestRange(20, 20, 20, 25)) }), (new[] { new TestTextEdit("newText", expectedRange2) }, expectedVersion2))
        });
        var mappingProvider = new DefaultLSPDocumentMappingProvider(requestInvoker, _documentManager, _razorLSPConventions);

        var workspaceEdit = new TestWorkspaceEdit(versionedEdits: true);
        workspaceEdit.AddEdits(s_razorVirtualCSharpFile, 10, new TestTextEdit("newText", new TestRange(10, 10, 10, 15)));
        workspaceEdit.AddEdits(s_anotherRazorVirtualCSharpFile, 20, new TestTextEdit("newText", new TestRange(20, 20, 20, 25)));

        // Act
        var result = await mappingProvider.RemapWorkspaceEditAsync(workspaceEdit, DisposalToken);

        // Assert
        Assert.Collection(result.DocumentChanges?.Value as TextDocumentEdit[],
            d =>
            {
                Assert.Equal(s_razorFile, d.TextDocument.Uri);
                Assert.Equal(expectedVersion1, d.TextDocument.Version);

                var actualEdit = Assert.Single(d.Edits);
                Assert.Equal("newText", actualEdit.NewText);
                Assert.Equal(expectedRange1, actualEdit.Range);
            },
            d =>
            {
                Assert.Equal(s_anotherRazorFile, d.TextDocument.Uri);
                Assert.Equal(expectedVersion2, d.TextDocument.Version);

                var actualEdit = Assert.Single(d.Edits);
                Assert.Equal("newText", actualEdit.NewText);
                Assert.Equal(expectedRange2, actualEdit.Range);
            });
    }

    private static LSPRequestInvoker GetRequestInvoker(((RazorLanguageKind, Uri, TestTextEdit[]), (TestTextEdit[], int))[] mappingPairs)
    {
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        if (mappingPairs is null)
        {
            return requestInvoker.Object;
        }

        // mappingPairs will contain the request/response pair for each of MapToDocumentRange LSP request we want to mock.
        foreach (var ((kind, uri, projectedEdits), (mappedEdits, version)) in mappingPairs)
        {
            var requestParams = new RazorMapToDocumentEditsParams()
            {
                Kind = kind,
                RazorDocumentUri = uri,
                ProjectedTextEdits = projectedEdits,
                FormattingOptions = null
            };
            var response = new RazorMapToDocumentEditsResponse()
            {
                TextEdits = mappedEdits,
                HostDocumentVersion = version
            };

            requestInvoker
                .Setup(r => r.ReinvokeRequestOnServerAsync<RazorMapToDocumentEditsParams, RazorMapToDocumentEditsResponse>(
                    It.IsAny<ITextBuffer>(),
                    LanguageServerConstants.RazorMapToDocumentEditsEndpoint,
                    RazorLSPConstants.RazorLanguageServerName,
                    It.IsAny<Func<JToken, bool>>(), requestParams,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ReinvocationResponse<RazorMapToDocumentEditsResponse>("TestLanguageClient", response));
        }

        return requestInvoker.Object;
    }

    private class TestWorkspaceEdit : WorkspaceEdit
    {
        public TestWorkspaceEdit(bool versionedEdits = false)
        {
            if (versionedEdits)
            {
                DocumentChanges = Array.Empty<TextDocumentEdit>();
            }

            Changes = new Dictionary<string, TextEdit[]>();
        }

        public void AddEdits(Uri uri, int version, params TextEdit[] edits)
        {
            Changes[uri.AbsoluteUri] = edits;

            DocumentChanges = (DocumentChanges?.Value as TextDocumentEdit[])?.Append(new TextDocumentEdit()
            {
                Edits = edits,
                TextDocument = new OptionalVersionedTextDocumentIdentifier()
                {
                    Uri = uri,
                    Version = version
                }
            }).ToArray();
        }
    }

    private class TestTextEdit : TextEdit
    {
        public TestTextEdit(string newText, Range range)
        {
            NewText = newText;
            Range = range;
        }
    }

    private class TestRange : Range
    {
        public TestRange(int startLine, int startCharacter, int endLine, int endCharacter)
        {
            Start = new Position(startLine, startCharacter);
            End = new Position(endLine, endCharacter);
        }
    }
}
