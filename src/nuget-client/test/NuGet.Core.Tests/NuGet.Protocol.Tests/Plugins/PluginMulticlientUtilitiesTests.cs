// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class PluginMulticlientUtilitiesTests
    {
        private readonly PluginMulticlientUtilities _utilities;

        public PluginMulticlientUtilitiesTests()
        {
            _utilities = new PluginMulticlientUtilities();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task DoOncePerPluginLifetimeAsync_ThrowsForNullOrEmptyKey(string key)
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _utilities.DoOncePerPluginLifetimeAsync(
                    key,
                    () => Task.CompletedTask,
                    CancellationToken.None));

            Assert.Equal("key", exception.ParamName);
        }

        [Fact]
        public async Task DoOncePerPluginLifetimeAsync_ThrowsForNullTaskFunc()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _utilities.DoOncePerPluginLifetimeAsync(
                    key: "a",
                    taskFunc: null,
                    cancellationToken: CancellationToken.None));

            Assert.Equal("taskFunc", exception.ParamName);
        }

        [Fact]
        public async Task DoOncePerPluginLifetimeAsync_ThrowsIfCancelled()
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => _utilities.DoOncePerPluginLifetimeAsync(
                    key: "a",
                    taskFunc: () => Task.CompletedTask,
                    cancellationToken: new CancellationToken(canceled: true)));
        }

        [Fact]
        public async Task DoOncePerPluginLifetimeAsync_ExecutesTaskOnlyOncePerKey()
        {
            var wasExecuted = false;

            await _utilities.DoOncePerPluginLifetimeAsync(
                    key: "a",
                    taskFunc: () =>
                    {
                        wasExecuted = true;
                        return Task.CompletedTask;
                    },
                    cancellationToken: CancellationToken.None);

            Assert.True(wasExecuted);

            wasExecuted = false;

            await _utilities.DoOncePerPluginLifetimeAsync(
                key: "a",
                taskFunc: () =>
                {
                    wasExecuted = true;
                    return Task.CompletedTask;
                },
                cancellationToken: CancellationToken.None);

            Assert.False(wasExecuted);
        }
    }
}
