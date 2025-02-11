// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Test.Utility;
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
                    new PluginFile(filePath: filePath, state: new Lazy<PluginFileState>(() => PluginFileState.Valid)),
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
                    new PluginFile(filePath: "a", state: new Lazy<PluginFileState>(() => PluginFileState.Valid)),
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
                    new PluginFile(filePath: "a", state: new Lazy<PluginFileState>(() => PluginFileState.Valid)),
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
                    new PluginFile(filePath: "a", state: new Lazy<PluginFileState>(() => PluginFileState.Valid)),
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
                    new PluginFile(filePath: "a", state: new Lazy<PluginFileState>(() => PluginFileState.Valid)),
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
                    new PluginFile(filePath: "a", state: new Lazy<PluginFileState>(() => PluginFileState.Valid)),
                    arguments: PluginConstants.PluginArguments,
                    requestHandlers: new RequestHandlers(),
                    options: ConnectionOptions.CreateDefault(),
                    sessionCancellationToken: CancellationToken.None));

            Assert.Equal(nameof(PluginFactory), exception.ObjectName);
        }

        [PlatformFact(Platform.Windows)]
        public async Task GetOrCreateNetPluginAsync_UsingBatchFile_CreatesPluginAndExecutes()
        {
            using TestDirectory testDirectory = TestDirectory.Create();
            string pluginPath = Path.Combine(testDirectory.Path, "nuget-plugin-batFile.bat");
            string outputPath = Path.Combine(testDirectory.Path, "plugin-output.txt");

            string batFileContent = $@"
        @echo off
        echo File executed > ""{outputPath}""
    ";

            File.WriteAllText(pluginPath, batFileContent);

            var args = PluginConstants.PluginArguments;
            var reqHandler = new RequestHandlers();
            var options = ConnectionOptions.CreateDefault();

            using var pluginFactory = new PluginFactory(Timeout.InfiniteTimeSpan);

            // Act
            var plugin = await Assert.ThrowsAnyAsync<Exception>(() => pluginFactory.GetOrCreateAsync(new PluginFile(filePath: pluginPath, state: new Lazy<PluginFileState>(() => PluginFileState.Valid), requiresDotnetHost: false), args, reqHandler, options, CancellationToken.None));

            // Assert
            string outputContent = File.ReadAllText(outputPath);
            Assert.Contains("File executed", outputContent);
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
