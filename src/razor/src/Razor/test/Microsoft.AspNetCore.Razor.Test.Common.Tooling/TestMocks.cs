// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Moq;

namespace Microsoft.AspNetCore.Razor.Test.Common;

internal static class TestMocks
{
    public static TextLoader CreateTextLoader(string text)
        => CreateTextLoader(text, VersionStamp.Create());

    public static TextLoader CreateTextLoader(string text, VersionStamp version)
        => CreateTextLoader(SourceText.From(text), version);

    public static TextLoader CreateTextLoader(SourceText text)
        => CreateTextLoader(text, VersionStamp.Create());

    public static TextLoader CreateTextLoader(SourceText text, VersionStamp version)
    {
        var mock = new StrictMock<TextLoader>();

        var textAndVersion = TextAndVersion.Create(text, version);

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

    public static IProjectSnapshot CreateProjectSnapshot(HostProject hostProject, ProjectWorkspaceState? projectWorkspaceState = null)
    {
        var mock = new StrictMock<IProjectSnapshot>();

        mock.SetupGet(x => x.Key)
            .Returns(hostProject.Key);
        mock.SetupGet(x => x.FilePath)
            .Returns(hostProject.FilePath);
        mock.SetupGet(x => x.IntermediateOutputPath)
            .Returns(hostProject.IntermediateOutputPath);
        mock.SetupGet(x => x.RootNamespace)
            .Returns(hostProject.RootNamespace);
        mock.SetupGet(x => x.DisplayName)
            .Returns(hostProject.DisplayName);

        if (projectWorkspaceState is not null)
        {
            mock.Setup(x => x.GetTagHelpersAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(projectWorkspaceState.TagHelpers);
        }

        return mock.Object;
    }
}
