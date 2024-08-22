// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class TimeoutUtilityTests
    {
        [Fact]
        public async Task TimeoutUtility_SucceedsWithResult()
        {
            // Arrange
            var expected = 23;
            CancellationToken timeoutToken = CancellationToken.None;
            Func<CancellationToken, Task<int>> actionAsync = token =>
            {
                timeoutToken = token;
                return Task.FromResult(expected);
            };

            // Act
            var actual = await TimeoutUtility.StartWithTimeout(
                actionAsync,
                TimeSpan.FromSeconds(1),
                "message",
                CancellationToken.None);

            // Assert
            Assert.Equal(expected, actual);
            Assert.False(timeoutToken.IsCancellationRequested);
        }

        [Fact]
        public async Task TimeoutUtility_FailsWithResult()
        {
            // Arrange
            var expected = new Exception();
            CancellationToken timeoutToken = CancellationToken.None;
            Func<CancellationToken, Task<int>> actionAsync = token =>
            {
                timeoutToken = token;
                throw expected;
            };

            // Act & Assert
            var actual = await Assert.ThrowsAsync<Exception>(() => TimeoutUtility.StartWithTimeout(
                actionAsync,
                TimeSpan.FromSeconds(1),
                "message",
                CancellationToken.None));
            Assert.Same(expected, actual);
            Assert.False(timeoutToken.IsCancellationRequested);
        }

        [Fact]
        public async Task TimeoutUtility_TimesOutWithResult()
        {
            // Arrange
            var expected = "timeout message";
            CancellationToken timeoutToken = CancellationToken.None;
            Func<CancellationToken, Task<int>> actionAsync = async token =>
            {
                timeoutToken = token;
                await Task.Delay(TimeSpan.FromMilliseconds(250), token);
                return 23;
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<TimeoutException>(() => TimeoutUtility.StartWithTimeout(
                actionAsync,
                TimeSpan.FromTicks(1),
                expected,
                CancellationToken.None));
            Assert.Equal(expected, exception.Message);
            Assert.True(timeoutToken.IsCancellationRequested);
        }

        [Fact]
        public async Task TimeoutUtility_SucceedsWithoutResult()
        {
            // Arrange
            CancellationToken timeoutToken = CancellationToken.None;
            Func<CancellationToken, Task> actionAsync = token =>
            {
                timeoutToken = token;
                return Task.CompletedTask;
            };

            // Act
            await TimeoutUtility.StartWithTimeout(
                actionAsync,
                TimeSpan.FromSeconds(1),
                "message",
                CancellationToken.None);

            // Assert
            Assert.False(timeoutToken.IsCancellationRequested);
        }

        [Fact]
        public async Task TimeoutUtility_FailsWithoutResult()
        {
            // Arrange
            var expected = new Exception();
            CancellationToken timeoutToken = CancellationToken.None;
            Func<CancellationToken, Task> actionAsync = token =>
            {
                timeoutToken = token;
                throw expected;
            };

            // Act & Assert
            var actual = await Assert.ThrowsAsync<Exception>(() => TimeoutUtility.StartWithTimeout(
                actionAsync,
                TimeSpan.FromSeconds(1),
                "message",
                CancellationToken.None));
            Assert.Same(expected, actual);
            Assert.False(timeoutToken.IsCancellationRequested);
        }

        [Fact]
        public async Task TimeoutUtility_TimesOutWithoutResult()
        {
            // Arrange
            var expected = "timeout message";
            CancellationToken timeoutToken = CancellationToken.None;
            Func<CancellationToken, Task> actionAsync = async token =>
            {
                timeoutToken = token;
                await Task.Delay(TimeSpan.FromMilliseconds(250), token);
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<TimeoutException>(() => TimeoutUtility.StartWithTimeout(
                actionAsync,
                TimeSpan.FromTicks(1),
                expected,
                CancellationToken.None));
            Assert.Equal(expected, exception.Message);
            Assert.True(timeoutToken.IsCancellationRequested);
        }
    }
}
