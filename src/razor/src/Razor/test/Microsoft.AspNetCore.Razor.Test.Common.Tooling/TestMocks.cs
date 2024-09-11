// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Moq;
using Moq.Language.Flow;

namespace Microsoft.AspNetCore.Razor.Test.Common;

internal static class TestMocks
{
    public static TextLoader CreateTextLoader(string filePath, string text)
    {
        return CreateTextLoader(filePath, SourceText.From(text));
    }

    public static TextLoader CreateTextLoader(string filePath, SourceText text)
    {
        var mock = new StrictMock<TextLoader>();

        var textAndVersion = TextAndVersion.Create(text, VersionStamp.Create(), filePath);

        mock.Setup(x => x.LoadTextAndVersionAsync(It.IsAny<LoadTextOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(textAndVersion);

        return mock.Object;
    }

    public interface IClientConnectionBuilder
    {
        void SetupSendRequest<TParams, TResponse>(string method, TResponse response, bool verifiable = false);
        void SetupSendRequest<TParams, TResponse>(string method, TParams @params, TResponse response, bool verifiable = false);
    }

    private sealed class ClientConnectionBuilder : IClientConnectionBuilder
    {
        public StrictMock<IClientConnection> Mock { get; } = new();

        public void SetupSendRequest<TParams, TResponse>(string method, TResponse response, bool verifiable = false)
        {
            var returnsResult = Mock
                .Setup(x => x.SendRequestAsync<TParams, TResponse>(method, It.IsAny<TParams>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            if (verifiable)
            {
                returnsResult.Verifiable();
            }
        }

        public void SetupSendRequest<TParams, TResponse>(string method, TParams @params, TResponse response, bool verifiable = false)
        {
            var returnsResult = Mock
                .Setup(x => x.SendRequestAsync<TParams, TResponse>(method, @params, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            if (verifiable)
            {
                returnsResult.Verifiable();
            }
        }
    }

    public static IClientConnection CreateClientConnection(Action<IClientConnectionBuilder> configure)
    {
        var builder = new ClientConnectionBuilder();
        configure?.Invoke(builder);
        return builder.Mock.Object;
    }

    public static void VerifySendRequest<TParams, TResponse>(this Mock<IClientConnection> mock, string method, Times times)
        => mock.Verify(x => x.SendRequestAsync<TParams, TResponse>(method, It.IsAny<TParams>(), It.IsAny<CancellationToken>()), times);

    public static void VerifySendRequest<TParams, TResponse>(this Mock<IClientConnection> mock, string method, Func<Times> times)
        => mock.Verify(x => x.SendRequestAsync<TParams, TResponse>(method, It.IsAny<TParams>(), It.IsAny<CancellationToken>()), times);

    public static void VerifySendRequest<TParams, TResponse>(this Mock<IClientConnection> mock, string method, TParams @params, Times times)
        => mock.Verify(x => x.SendRequestAsync<TParams, TResponse>(method, @params, It.IsAny<CancellationToken>()), times);

    public static void VerifySendRequest<TParams, TResponse>(this Mock<IClientConnection> mock, string method, TParams @params, Func<Times> times)
        => mock.Verify(x => x.SendRequestAsync<TParams, TResponse>(method, @params, It.IsAny<CancellationToken>()), times);
}
