// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

public class DefaultLSPRequestInvokerTest : TestBase
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
        var broker = new TestLanguageServiceBroker((method) =>
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
        var broker = new TestLanguageServiceBroker((method) =>
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
        var broker = new TestLanguageServiceBroker((method) =>
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
        var broker = new TestLanguageServiceBroker((method) =>
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
        var broker = new TestLanguageServiceBroker((method) =>
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
        var broker = new TestLanguageServiceBroker((method) =>
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
}
