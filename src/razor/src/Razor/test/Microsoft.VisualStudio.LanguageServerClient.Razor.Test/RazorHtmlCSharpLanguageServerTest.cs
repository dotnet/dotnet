// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

public class RazorHtmlCSharpLanguageServerTest : TestBase
{
    public RazorHtmlCSharpLanguageServerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task ExecuteRequestAsync_InvokesCustomHandler()
    {
        // Arrange
        var handler = new Mock<IRequestHandler<string, int>>(MockBehavior.Strict);
        handler
            .Setup(h => h.HandleRequestAsync("hello world", It.IsAny<ClientCapabilities>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(123)
            .Verifiable();
        var metadata = Mock.Of<IRequestHandlerMetadata>(rhm => rhm.MethodName == "test", MockBehavior.Strict);
        using var languageServer = new RazorHtmlCSharpLanguageServer(new[] { new Lazy<IRequestHandler, IRequestHandlerMetadata>(() => handler.Object, metadata) });

        // Act
        var result = await languageServer.ExecuteRequestAsync<string, int>(
            "test", "hello world", clientCapabilities: null, DisposalToken);

        // Assert
        Assert.Equal(123, result);
        handler.VerifyAll();
    }

    [Fact]
    public async Task InitializeAsync_InvokesHandlerWithParamsAndCapabilities()
    {
        // Arrange
        var originalInitParams = new InitializeParams()
        {
            Capabilities = new ClientCapabilities()
            {
                Experimental = true
            },
            RootUri = new Uri("C:/path/to/workspace"),
        };
        var initializeResult = new InitializeResult();
        var handler = new Mock<IRequestHandler<InitializeParams, InitializeResult>>(MockBehavior.Strict);
        handler.Setup(h => h.HandleRequestAsync(It.IsAny<InitializeParams>(), It.IsAny<ClientCapabilities>(), It.IsAny<CancellationToken>()))
            .Callback<InitializeParams, ClientCapabilities, CancellationToken>((initParams, clientCapabilities, token) =>
            {
                Assert.True((bool)initParams.Capabilities.Experimental);
                Assert.Equal(originalInitParams.RootUri.AbsoluteUri, initParams.RootUri.AbsoluteUri);
            })
            .ReturnsAsync(initializeResult)
            .Verifiable();
        var metadata = Mock.Of<IRequestHandlerMetadata>(rhm => rhm.MethodName == Methods.InitializeName, MockBehavior.Strict);
        using var languageServer = new RazorHtmlCSharpLanguageServer(new[] { new Lazy<IRequestHandler, IRequestHandlerMetadata>(() => handler.Object, metadata) });
        var serializedInitParams = JToken.FromObject(originalInitParams);

        // Act
        var result = await languageServer.InitializeAsync(serializedInitParams, DisposalToken);

        // Assert
        Assert.Same(initializeResult, result);
        handler.VerifyAll();
    }
}
