// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

public class DefaultLSPRequestInvokerTest : ToolingTestBase
{
    private readonly FallbackCapabilitiesFilterResolver _capabilitiesResolver;

    public DefaultLSPRequestInvokerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _capabilitiesResolver = new DefaultFallbackCapabilitiesFilterResolver();
    }

    [Fact]
    public async Task ReinvokeRequestOnServerAsync_InvokesRazorLanguageClient()
    {
        // Arrange
        var called = false;
        var expectedMethod = "razor/test";
        var broker = CreateLanguageServiceBroker((method) =>
        {
            called = true;
            Assert.Equal(expectedMethod, method);
        });
        var requestInvoker = new DefaultLSPRequestInvoker(broker, _capabilitiesResolver);

        // Act
        await requestInvoker.ReinvokeRequestOnServerAsync<object, object>(
            expectedMethod, RazorLSPConstants.RazorLanguageServerName, new object(), DisposalToken);

        // Assert
        Assert.True(called);
    }

    [Fact]
    public async Task ReinvokeRequestOnServerAsync_InvokesHtmlLanguageClient()
    {
        // Arrange
        var called = false;
        var expectedMethod = "textDocument/test";
        var broker = CreateLanguageServiceBroker((method) =>
        {
            called = true;
            Assert.Equal(expectedMethod, method);
        });
        var requestInvoker = new DefaultLSPRequestInvoker(broker, _capabilitiesResolver);

        // Act
        await requestInvoker.ReinvokeRequestOnServerAsync<object, object>(
            expectedMethod, RazorLSPConstants.HtmlLanguageServerName, new object(), DisposalToken);

        // Assert
        Assert.True(called);
    }

    [Fact]
    public async Task ReinvokeRequestOnServerAsync_InvokesCSharpLanguageClient()
    {
        // Arrange
        var called = false;
        var expectedMethod = "textDocument/test";
        var broker = CreateLanguageServiceBroker((method) =>
        {
            called = true;
            Assert.Equal(expectedMethod, method);
        });
        var requestInvoker = new DefaultLSPRequestInvoker(broker, _capabilitiesResolver);

        // Act
        await requestInvoker.ReinvokeRequestOnServerAsync<object, object>(
            expectedMethod, RazorLSPConstants.RazorCSharpLanguageServerName, new object(), DisposalToken);

        // Assert
        Assert.True(called);
    }

    [Fact]
    public async Task CustomRequestServerAsync_InvokesRazorLanguageClient()
    {
        // Arrange
        var called = false;
        var expectedMethod = "razor/test";
        var broker = CreateLanguageServiceBroker((method) =>
        {
            called = true;
            Assert.Equal(expectedMethod, method);
        });
        var requestInvoker = new DefaultLSPRequestInvoker(broker, _capabilitiesResolver);

        // Act
        await requestInvoker.ReinvokeRequestOnServerAsync<object, object>(
            expectedMethod, RazorLSPConstants.RazorLanguageServerName, new object(), DisposalToken);

        // Assert
        Assert.True(called);
    }

    [Fact]
    public async Task CustomRequestServerAsync_InvokesHtmlLanguageClient()
    {
        // Arrange
        var called = false;
        var expectedMethod = "textDocument/test";
        var broker = CreateLanguageServiceBroker((method) =>
        {
            called = true;
            Assert.Equal(expectedMethod, method);
        });
        var requestInvoker = new DefaultLSPRequestInvoker(broker, _capabilitiesResolver);

        // Act
        await requestInvoker.ReinvokeRequestOnServerAsync<object, object>(
            expectedMethod, RazorLSPConstants.HtmlLanguageServerName, new object(), DisposalToken);

        // Assert
        Assert.True(called);
    }

    [Fact]
    public async Task CustomRequestServerAsync_InvokesCSharpLanguageClient()
    {
        // Arrange
        var called = false;
        var expectedMethod = "textDocument/test";
        var broker = CreateLanguageServiceBroker((method) =>
        {
            called = true;
            Assert.Equal(expectedMethod, method);
        });
        var requestInvoker = new DefaultLSPRequestInvoker(broker, _capabilitiesResolver);

        // Act
        await requestInvoker.ReinvokeRequestOnServerAsync<object, object>(
            expectedMethod, RazorLSPConstants.RazorCSharpLanguageServerName, new object(), DisposalToken);

        // Assert
        Assert.True(called);
    }

    private static ILanguageServiceBroker2 CreateLanguageServiceBroker(Action<string> callback)
    {
        var broker = new StrictMock<ILanguageServiceBroker2>();
#pragma warning disable CS0618 // Type or member is obsolete
        broker.Setup(b => b.RequestAsync(It.IsAny<string[]>(), It.IsAny<Func<JToken, bool>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<JToken>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((null, null))
            .Callback((string[] _, Func<JToken, bool> _, string _, string method, JToken _, CancellationToken _) => callback(method));
#pragma warning restore CS0618 // Type or member is obsolete

        return broker.Object;
    }
}
