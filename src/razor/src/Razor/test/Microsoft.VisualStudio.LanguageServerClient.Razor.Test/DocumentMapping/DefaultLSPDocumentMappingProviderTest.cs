// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
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

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.DocumentMapping;

public class DefaultLSPDocumentMappingProviderTest : TestBase
{
    private static readonly Uri s_razorFile = new("file:///some/folder/to/file.razor");
    private static readonly Uri s_razorVirtualCSharpFile = new("file:///some/folder/to/file.razor.ide.g.cs");
    private static readonly Uri s_anotherRazorFile = new("file:///some/folder/to/anotherfile.razor");

    private readonly Lazy<LSPDocumentManager> _documentManager;

    public DefaultLSPDocumentMappingProviderTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var csharpVirtualDocumentSnapshot = new CSharpVirtualDocumentSnapshot(projectKey: default, s_razorVirtualCSharpFile, new StringTextSnapshot(string.Empty), hostDocumentSyncVersion: 0);
        var documentSnapshot1 = new TestLSPDocumentSnapshot(s_razorFile, version: 1, "first doc", csharpVirtualDocumentSnapshot);
        var documentSnapshot2 = new TestLSPDocumentSnapshot(s_anotherRazorFile, version: 5, "second doc", csharpVirtualDocumentSnapshot);
        var documentManager = new TestDocumentManager();
        documentManager.AddDocument(s_razorFile, documentSnapshot1);
        documentManager.AddDocument(s_anotherRazorFile, documentSnapshot2);
        _documentManager = new Lazy<LSPDocumentManager>(() => documentManager);
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

        var mappingProvider = new DefaultLSPDocumentMappingProvider(requestInvoker.Object, _documentManager);
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
}
