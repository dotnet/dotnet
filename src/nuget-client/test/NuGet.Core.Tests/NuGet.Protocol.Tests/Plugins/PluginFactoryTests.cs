// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class PluginFactoryTests
    {
        [Fact]
        public void Constructor_ThrowsForTimeSpanBelowMinimum()
        {
            var timeout = TimeSpan.FromMilliseconds(Timeout.InfiniteTimeSpan.TotalMilliseconds - 1);

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new PluginFactory(timeout));

            Assert.Equal("pluginIdleTimeout", exception.ParamName);
            Assert.Equal(timeout, exception.ActualValue);
        }

        [Fact]
        public void Constructor_AcceptsInfiniteTimeSpan()
        {
            new PluginFactory(Timeout.InfiniteTimeSpan);
        }

        [Fact]
        public void Dispose_IsIdempotent()
        {
            using (var factory = new PluginFactory(Timeout.InfiniteTimeSpan))
            {
                factory.Dispose();
                factory.Dispose();
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetOrCreateAsync_ThrowsForNullOrEmptyFilePath(string filePath)
        {
            var factory = new PluginFactory(Timeout.InfiniteTimeSpan);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => factory.GetOrCreateAsync(
                    filePath,
                    PluginConstants.PluginArguments,
                    new RequestHandlers(),
                    ConnectionOptions.CreateDefault(),
                    CancellationToken.None));

            Assert.Equal("filePath", exception.ParamName);
        }

        [Fact]
        public async Task GetOrCreateAsync_ThrowsForNullArguments()
        {
            var factory = new PluginFactory(Timeout.InfiniteTimeSpan);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => factory.GetOrCreateAsync(
                    filePath: "a",
                    arguments: null,
                    requestHandlers: new RequestHandlers(),
                    options: ConnectionOptions.CreateDefault(),
                    sessionCancellationToken: CancellationToken.None));

            Assert.Equal("arguments", exception.ParamName);
        }

        [Fact]
        public async Task GetOrCreateAsync_ThrowsForNullRequestHandlers()
        {
            var factory = new PluginFactory(Timeout.InfiniteTimeSpan);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => factory.GetOrCreateAsync(
                    filePath: "a",
                    arguments: PluginConstants.PluginArguments,
                    requestHandlers: null,
                    options: ConnectionOptions.CreateDefault(),
                    sessionCancellationToken: CancellationToken.None));

            Assert.Equal("requestHandlers", exception.ParamName);
        }

        [Fact]
        public async Task GetOrCreateAsync_ThrowsForNullConnectionOptions()
        {
            var factory = new PluginFactory(Timeout.InfiniteTimeSpan);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => factory.GetOrCreateAsync(
                    filePath: "a",
                    arguments: PluginConstants.PluginArguments,
                    requestHandlers: new RequestHandlers(),
                    options: null,
                    sessionCancellationToken: CancellationToken.None));

            Assert.Equal("options", exception.ParamName);
        }

        [Fact]
        public async Task GetOrCreateAsync_ThrowsIfCancelled()
        {
            var factory = new PluginFactory(Timeout.InfiniteTimeSpan);

            await Assert.ThrowsAsync<OperationCanceledException>(
                () => factory.GetOrCreateAsync(
                    filePath: "a",
                    arguments: PluginConstants.PluginArguments,
                    requestHandlers: new RequestHandlers(),
                    options: ConnectionOptions.CreateDefault(),
                    sessionCancellationToken: new CancellationToken(canceled: true)));
        }

        [Fact]
        public async Task GetOrCreateAsync_ThrowsIfDisposed()
        {
            var factory = new PluginFactory(Timeout.InfiniteTimeSpan);

            factory.Dispose();

            var exception = await Assert.ThrowsAsync<ObjectDisposedException>(
                () => factory.GetOrCreateAsync(
                    filePath: "a",
                    arguments: PluginConstants.PluginArguments,
                    requestHandlers: new RequestHandlers(),
                    options: ConnectionOptions.CreateDefault(),
                    sessionCancellationToken: CancellationToken.None));

            Assert.Equal(nameof(PluginFactory), exception.ObjectName);
        }

        [Fact]
        public async Task CreateFromCurrentProcessAsync_ThrowsForNullRequestHandlers()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => PluginFactory.CreateFromCurrentProcessAsync(
                    requestHandlers: null,
                    options: ConnectionOptions.CreateDefault(),
                    sessionCancellationToken: CancellationToken.None));

            Assert.Equal("requestHandlers", exception.ParamName);
        }

        [Fact]
        public async Task CreateFromCurrentProcessAsync_ThrowsForNullConnectionOptions()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => PluginFactory.CreateFromCurrentProcessAsync(
                    requestHandlers: new RequestHandlers(),
                    options: null,
                    sessionCancellationToken: CancellationToken.None));

            Assert.Equal("options", exception.ParamName);
        }

        [Fact]
        public async Task CreateFromCurrentProcessAsync_ThrowsIfCancelled()
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => PluginFactory.CreateFromCurrentProcessAsync(
                    new RequestHandlers(),
                    ConnectionOptions.CreateDefault(),
                    sessionCancellationToken: new CancellationToken(canceled: true)));
        }
    }
}
