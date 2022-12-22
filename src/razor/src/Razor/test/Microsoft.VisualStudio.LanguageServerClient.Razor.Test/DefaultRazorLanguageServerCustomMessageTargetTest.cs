// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic.Models;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Test;
using Microsoft.VisualStudio.Test;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

    public class DefaultRazorLanguageServerCustomMessageTargetTest : TestBase
    {
        private readonly ITextBuffer _textBuffer;
        private readonly EditorSettingsManager _editorSettingsManager;

        public DefaultRazorLanguageServerCustomMessageTargetTest(ITestOutputHelper testOutput)
            : base(testOutput)
        {
            _textBuffer = new TestTextBuffer(new StringTextSnapshot(string.Empty));
            _editorSettingsManager = new DefaultEditorSettingsManager(Array.Empty<EditorSettingsChangedTrigger>());
        }

        [Fact]
        public void UpdateCSharpBuffer_CannotLookupDocument_NoopsGracefully()
        {
            // Arrange
            LSPDocumentSnapshot document;
            var documentManager = new Mock<TrackingLSPDocumentManager>(MockBehavior.Strict);
            documentManager
                .Setup(manager => manager.TryGetDocument(It.IsAny<Uri>(), out document))
                .Returns(false);
            var documentSynchronizer = new Mock<LSPDocumentSynchronizer>(MockBehavior.Strict);
            var target = new DefaultRazorLanguageServerCustomMessageTarget(documentManager.Object, documentSynchronizer.Object);
            var request = new UpdateBufferRequest()
            {
                HostDocumentFilePath = "C:/path/to/file.razor",
                Changes = null
            };

            // Act & Assert
            target.UpdateCSharpBuffer(request);
        }

        [Fact]
        public void UpdateCSharpBuffer_UpdatesDocument()
        {
            // Arrange
            var documentManager = new Mock<TrackingLSPDocumentManager>(MockBehavior.Strict);
            documentManager
                .Setup(manager => manager.UpdateVirtualDocument<CSharpVirtualDocument>(
                    It.IsAny<Uri>(),
                    It.IsAny<IReadOnlyList<ITextChange>>(),
                    1337,
                    It.IsAny<object>()))
                .Verifiable();
            var documentSynchronizer = new Mock<LSPDocumentSynchronizer>(MockBehavior.Strict);

            var target = new DefaultRazorLanguageServerCustomMessageTarget(documentManager.Object, documentSynchronizer.Object);
            var request = new UpdateBufferRequest()
            {
                HostDocumentFilePath = "C:/path/to/file.razor",
                HostDocumentVersion = 1337,
                Changes = Array.Empty<TextChange>(),
            };

            // Act
            target.UpdateCSharpBuffer(request);

            // Assert
            documentManager.VerifyAll();
        }

        [Fact]
        public async Task RazorRangeFormattingAsync_LanguageKindRazor_ReturnsEmpty()
        {
            // Arrange
            var documentManager = Mock.Of<TrackingLSPDocumentManager>(MockBehavior.Strict);
            var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);

            var documentSynchronizer = new Mock<LSPDocumentSynchronizer>(MockBehavior.Strict);

            var target = new DefaultRazorLanguageServerCustomMessageTarget(
                documentManager, JoinableTaskContext, requestInvoker.Object,
                TestFormattingOptionsProvider.Default, _editorSettingsManager, documentSynchronizer.Object);

            var request = new RazorDocumentRangeFormattingParams()
            {
                HostDocumentFilePath = "c:/Some/path/to/file.razor",
                Kind = RazorLanguageKind.Razor,
                ProjectedRange = new Range(),
                Options = new FormattingOptions()
                {
                    TabSize = 4,
                    InsertSpaces = true
                }
            };

            // Act
            var result = await target.RazorRangeFormattingAsync(request, DisposalToken);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Edits);
        }

        [Fact]
        public async Task RazorRangeFormattingAsync_DocumentNotFound_ReturnsEmpty()
        {
            // Arrange
            var documentManager = new Mock<TrackingLSPDocumentManager>(MockBehavior.Strict).Object;
            Mock.Get(documentManager)
                .Setup(m => m.TryGetDocument(
                    new Uri("c:/Some/path/to/file.razor"),
                    out It.Ref<LSPDocumentSnapshot>.IsAny))
                .Returns(false);
            var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);

            var documentSynchronizer = GetDocumentSynchronizer();

            var target = new DefaultRazorLanguageServerCustomMessageTarget(
                documentManager, JoinableTaskContext, requestInvoker.Object,
                TestFormattingOptionsProvider.Default, _editorSettingsManager, documentSynchronizer);

            var request = new RazorDocumentRangeFormattingParams()
            {
                HostDocumentFilePath = "c:/Some/path/to/file.razor",
                Kind = RazorLanguageKind.CSharp,
                ProjectedRange = new Range(),
                Options = new FormattingOptions()
                {
                    TabSize = 4,
                    InsertSpaces = true
                }
            };

            // Act
            var result = await target.RazorRangeFormattingAsync(request, DisposalToken);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Edits);
        }

        [Fact]
        public async Task RazorRangeFormattingAsync_ValidRequest_InvokesLanguageServer()
        {
            // Arrange
            var filePath = "c:/Some/path/to/file.razor";
            var uri = new Uri(filePath);
            var virtualDocument = new CSharpVirtualDocumentSnapshot(new Uri($"{filePath}.g.cs"), _textBuffer.CurrentSnapshot, 1);
            LSPDocumentSnapshot document = new TestLSPDocumentSnapshot(uri, 1, new[] { virtualDocument });
            var documentManager = new Mock<TrackingLSPDocumentManager>(MockBehavior.Strict);
            documentManager
                .Setup(manager => manager.TryGetDocument(It.IsAny<Uri>(), out document))
                .Returns(true);

            var expectedEdit = new TextEdit()
            {
                NewText = "SomeEdit",
                Range = new Range() { Start = new Position(), End = new Position() }
            };

            var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
            requestInvoker
                .Setup(r => r.ReinvokeRequestOnServerAsync<DocumentRangeFormattingParams, TextEdit[]>(
                    _textBuffer,
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DocumentRangeFormattingParams>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ReinvocationResponse<TextEdit[]>("languageClient", new[] { expectedEdit }));

            var documentSynchronizer = GetDocumentSynchronizer(GetCSharpSnapshot(uri, hostDocumentSyncVersion: 1));

            var target = new DefaultRazorLanguageServerCustomMessageTarget(
                documentManager.Object, JoinableTaskContext, requestInvoker.Object,
                TestFormattingOptionsProvider.Default, _editorSettingsManager, documentSynchronizer);

            var request = new RazorDocumentRangeFormattingParams()
            {
                HostDocumentFilePath = filePath,
                Kind = RazorLanguageKind.CSharp,
                ProjectedRange = new Range()
                {
                    Start = new Position(),
                    End = new Position()
                },
                Options = new FormattingOptions()
                {
                    TabSize = 4,
                    InsertSpaces = true
                }
            };

            // Act
            var result = await target.RazorRangeFormattingAsync(request, DisposalToken);

            // Assert
            Assert.NotNull(result);
            var edit = Assert.Single(result.Edits);
            Assert.Equal("SomeEdit", edit.NewText);
        }

        [Fact]
        public async Task ProvideCodeActionsAsync_CannotLookupDocument_ReturnsNullAsync()
        {
            // Arrange
            LSPDocumentSnapshot document;
            var documentManager = new Mock<TrackingLSPDocumentManager>(MockBehavior.Strict);
            documentManager
                .Setup(manager => manager.TryGetDocument(It.IsAny<Uri>(), out document))
                .Returns(false);
            var documentSynchronizer = GetDocumentSynchronizer();
            var target = new DefaultRazorLanguageServerCustomMessageTarget(documentManager.Object, documentSynchronizer);
            var request = new DelegatedCodeActionParams()
            {
                HostDocumentVersion = 1,
                LanguageKind = RazorLanguageKind.CSharp,
                CodeActionParams = new CodeActionParams()
                {
                    TextDocument = new TextDocumentIdentifier()
                    {
                        Uri = new Uri("C:/path/to/file.razor")
                    }
                }
            };

            // Act
            var result = await target.ProvideCodeActionsAsync(request, DisposalToken);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ProvideCodeActionsAsync_ReturnsCodeActionsAsync()
        {
            // Arrange
            var testDocUri = new Uri("C:/path/to/file.razor");
            var testVirtualDocUri = new Uri("C:/path/to/file2.razor.g");
            var testCSharpDocUri = new Uri("C:/path/to/file.razor.g.cs");

            var testVirtualDocument = new TestVirtualDocumentSnapshot(testVirtualDocUri, 0);
            var csharpVirtualDocument = new CSharpVirtualDocumentSnapshot(testCSharpDocUri, _textBuffer.CurrentSnapshot, 0);
            LSPDocumentSnapshot testDocument = new TestLSPDocumentSnapshot(testDocUri, 0, testVirtualDocument, csharpVirtualDocument);

            var documentManager = new Mock<TrackingLSPDocumentManager>(MockBehavior.Strict);
            documentManager
                .Setup(manager => manager.TryGetDocument(It.IsAny<Uri>(), out testDocument))
                .Returns(true);

            var languageServer1Response = new[] { new VSInternalCodeAction() { Title = "Response 1" } };
            var languageServer2Response = new[] { new VSInternalCodeAction() { Title = "Response 2" } };

            async IAsyncEnumerable<ReinvocationResponse<IReadOnlyList<VSInternalCodeAction>>> GetExpectedResultsAsync()
            {
                yield return new ReinvocationResponse<IReadOnlyList<VSInternalCodeAction>>("languageClient", languageServer1Response);
                yield return new ReinvocationResponse<IReadOnlyList<VSInternalCodeAction>>("languageClient", languageServer2Response);

                await Task.CompletedTask;
            }

            var expectedResults = GetExpectedResultsAsync();
            var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
            requestInvoker
                .Setup(invoker => invoker.ReinvokeRequestOnMultipleServersAsync<CodeActionParams, IReadOnlyList<VSInternalCodeAction>>(
                    _textBuffer,
                    Methods.TextDocumentCodeActionName,
                    It.IsAny<Func<JToken, bool>>(),
                    It.IsAny<CodeActionParams>(),
                    It.IsAny<CancellationToken>()))
                .Returns(expectedResults);

            var documentSynchronizer = GetDocumentSynchronizer(GetCSharpSnapshot());

            var target = new DefaultRazorLanguageServerCustomMessageTarget(
                documentManager.Object, JoinableTaskContext, requestInvoker.Object,
                TestFormattingOptionsProvider.Default, _editorSettingsManager, documentSynchronizer);

            var request = new DelegatedCodeActionParams()
            {
                HostDocumentVersion = 1,
                LanguageKind = RazorLanguageKind.CSharp,
                CodeActionParams = new CodeActionParams()
                {
                    TextDocument = new TextDocumentIdentifier()
                    {
                        Uri = testDocUri
                    }
                }
            };

            // Act
            var result = await target.ProvideCodeActionsAsync(request, DisposalToken);

            // Assert
            Assert.Collection(result,
                r => Assert.Equal(languageServer1Response[0].Title, r.Title),
                r => Assert.Equal(languageServer2Response[0].Title, r.Title));
        }

        [Fact]
        public async Task ResolveCodeActionsAsync_ReturnsSingleCodeAction()
        {
            // Arrange
            var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
            var csharpVirtualDocument = new CSharpVirtualDocumentSnapshot(new Uri("C:/path/to/file.razor.g.cs"), _textBuffer.CurrentSnapshot, hostDocumentSyncVersion: 0);
            var documentManager = new TestDocumentManager();
            var razorUri = new Uri("C:/path/to/file.razor");
            documentManager.AddDocument(razorUri, new TestLSPDocumentSnapshot(razorUri, version: 0, "Some Content", csharpVirtualDocument));
            var expectedCodeAction = new VSInternalCodeAction()
            {
                Title = "Something",
                Data = new object()
            };
            var unexpectedCodeAction = new VSInternalCodeAction()
            {
                Title = "Something Else",
                Data = new object()
            };

            async IAsyncEnumerable<ReinvocationResponse<VSInternalCodeAction>> GetExpectedResultsAsync()
            {
                yield return new ReinvocationResponse<VSInternalCodeAction>("languageClient", expectedCodeAction);
                yield return new ReinvocationResponse<VSInternalCodeAction>("languageClient", unexpectedCodeAction);

                await Task.CompletedTask;
            }

            var expectedResponses = GetExpectedResultsAsync();
            requestInvoker
                .Setup(invoker => invoker.ReinvokeRequestOnMultipleServersAsync<CodeAction, VSInternalCodeAction>(
                    It.IsAny<ITextBuffer>(),
                    Methods.CodeActionResolveName,
                    It.IsAny<Func<JToken, bool>>(),
                    It.IsAny<VSInternalCodeAction>(),
                    It.IsAny<CancellationToken>()))
                .Returns(expectedResponses);

            var documentSynchronizer = new Mock<LSPDocumentSynchronizer>(MockBehavior.Strict);
            documentSynchronizer
                .Setup(r => r.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                    1,
                    It.IsAny<Uri>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DefaultLSPDocumentSynchronizer.SynchronizedResult<CSharpVirtualDocumentSnapshot>(true, csharpVirtualDocument));

            var target = new DefaultRazorLanguageServerCustomMessageTarget(
                documentManager, JoinableTaskContext, requestInvoker.Object,
                TestFormattingOptionsProvider.Default, _editorSettingsManager, documentSynchronizer.Object);

            var codeAction = new VSInternalCodeAction()
            {
                Title = "Something",
            };
            var request = new RazorResolveCodeActionParams(razorUri, HostDocumentVersion: 1, RazorLanguageKind.CSharp, codeAction);

            // Act
            var result = await target.ResolveCodeActionsAsync(request, DisposalToken);

            // Assert
            Assert.Equal(expectedCodeAction.Title, result.Title);
        }

        [Fact]
        public async Task ProvideSemanticTokensAsync_CannotLookupDocument_ReturnsNullAsync()
        {
            // Arrange
            LSPDocumentSnapshot document;
            var documentManager = new Mock<TrackingLSPDocumentManager>(MockBehavior.Strict);
            documentManager
                .Setup(manager => manager.TryGetDocument(It.IsAny<Uri>(), out document))
                .Returns(false);
            var documentSynchronizer = GetDocumentSynchronizer();
            var target = new DefaultRazorLanguageServerCustomMessageTarget(documentManager.Object, documentSynchronizer);
            var request = new ProvideSemanticTokensRangeParams(
                textDocument: new TextDocumentIdentifier()
                {
                    Uri = new Uri("C:/path/to/file.razor")
                },
                requiredHostDocumentVersion: 1,
                range: new Range());

            // Act
            var result = await target.ProvideSemanticTokensRangeAsync(request, DisposalToken);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ProvideSemanticTokensAsync_CannotLookupVirtualDocument_ReturnsNullAsync()
        {
            // Arrange
            var testDocUri = new Uri("C:/path/to/file.razor");
            LSPDocumentSnapshot testDocument = new TestLSPDocumentSnapshot(testDocUri, 0);

            var documentManager = new Mock<TrackingLSPDocumentManager>(MockBehavior.Strict);
            documentManager
                .Setup(manager => manager.TryGetDocument(It.IsAny<Uri>(), out testDocument))
                .Returns(true);
            var documentSynchronizer = GetDocumentSynchronizer();
            var target = new DefaultRazorLanguageServerCustomMessageTarget(documentManager.Object, documentSynchronizer);
            var request = new ProvideSemanticTokensRangeParams(
                textDocument: new TextDocumentIdentifier()
                {
                    Uri = new Uri("C:/path/to/file.razor")
                },
                requiredHostDocumentVersion: 0,
                range: new Range());

            // Act
            var result = await target.ProvideSemanticTokensRangeAsync(request, DisposalToken);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ProvideSemanticTokensAsync_ReturnsSemanticTokensAsync()
        {
            // Arrange
            var testDocUri = new Uri("C:/path/to%20-%20project/file.razor");
            var testVirtualDocUri = new Uri("C:/path/to - project/file2.razor.g");
            var testCSharpDocUri = new Uri("C:/path/to - project/file.razor.g.cs");

            var documentVersion = 0;
            var testVirtualDocument = new TestVirtualDocumentSnapshot(testVirtualDocUri, 0);
            var csharpVirtualDocument = new CSharpVirtualDocumentSnapshot(testCSharpDocUri, _textBuffer.CurrentSnapshot, 0);
            LSPDocumentSnapshot testDocument = new TestLSPDocumentSnapshot(testDocUri, documentVersion, testVirtualDocument, csharpVirtualDocument);

            var documentManager = new Mock<TrackingLSPDocumentManager>(MockBehavior.Strict);
            documentManager
                .Setup(manager => manager.TryGetDocument(testDocUri, out testDocument))
                .Returns(true);

            var expectedcSharpResults = new VSSemanticTokensResponse();
            var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
            requestInvoker
                .Setup(invoker => invoker.ReinvokeRequestOnServerAsync<SemanticTokensRangeParams, VSSemanticTokensResponse>(
                    _textBuffer,
                    Methods.TextDocumentSemanticTokensRangeName,
                    LanguageServerKind.CSharp.ToLanguageServerName(),
                    It.IsAny<SemanticTokensRangeParams>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ReinvocationResponse<VSSemanticTokensResponse>("languageClient", expectedcSharpResults));

            var documentSynchronizer = new Mock<LSPDocumentSynchronizer>(MockBehavior.Strict);
            documentSynchronizer
                .Setup(r => r.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                    0,
                    It.IsAny<Uri>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DefaultLSPDocumentSynchronizer.SynchronizedResult<CSharpVirtualDocumentSnapshot>(true, csharpVirtualDocument));

            var target = new DefaultRazorLanguageServerCustomMessageTarget(
                documentManager.Object, JoinableTaskContext, requestInvoker.Object,
                TestFormattingOptionsProvider.Default, _editorSettingsManager, documentSynchronizer.Object);
            var request = new ProvideSemanticTokensRangeParams(
                textDocument: new TextDocumentIdentifier()
                {
                    Uri = new Uri("C:/path/to%20-%20project/file.razor")
                },
                requiredHostDocumentVersion: 0,
                range: new Range());
            var expectedResults = new ProvideSemanticTokensResponse(expectedcSharpResults.Data, documentVersion);

            // Act
            var result = await target.ProvideSemanticTokensRangeAsync(request, DisposalToken);

            // Assert
            Assert.Equal(expectedResults, result);
        }

        private LSPDocumentSynchronizer GetDocumentSynchronizer(CSharpVirtualDocumentSnapshot csharpDoc = null, HtmlVirtualDocumentSnapshot htmlDoc = null)
        {
            var synchronizer = new Mock<LSPDocumentSynchronizer>(MockBehavior.Strict);
            synchronizer.Setup(s => s.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(It.IsAny<int>(), It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DefaultLSPDocumentSynchronizer.SynchronizedResult<CSharpVirtualDocumentSnapshot>(csharpDoc is not null, csharpDoc));

            synchronizer.Setup(s => s.TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(It.IsAny<int>(), It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DefaultLSPDocumentSynchronizer.SynchronizedResult<HtmlVirtualDocumentSnapshot>(htmlDoc is not null, htmlDoc));

            return synchronizer.Object;
        }

        private CSharpVirtualDocumentSnapshot GetCSharpSnapshot(Uri uri = null, int hostDocumentSyncVersion = 1)
        {
            if (uri is null)
            {
                uri = new Uri("C:/thing.razor");
            }

            var textBuffer = new Mock<ITextBuffer>(MockBehavior.Strict);
            var snapshot = new Mock<ITextSnapshot>(MockBehavior.Strict);
            snapshot.Setup(s => s.TextBuffer)
                .Returns(_textBuffer);

            var csharpDoc = new CSharpVirtualDocumentSnapshot(uri, snapshot.Object, hostDocumentSyncVersion);

            return csharpDoc;
        }
    }
