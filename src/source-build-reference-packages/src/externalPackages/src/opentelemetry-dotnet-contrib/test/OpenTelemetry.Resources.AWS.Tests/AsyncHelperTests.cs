// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Xunit;

namespace OpenTelemetry.Resources.AWS.Tests;

public class AsyncHelperTests
{
    [Fact]
    public void RunSyncTaskCompletesSuccessfully()
    {
        // Arrange
        static async Task Task()
        {
            await System.Threading.Tasks.Task.Delay(100);
        }

        // Act
        var exception = Record.Exception(() => AsyncHelper.RunSync(Task));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void RunSyncTaskThrowsException()
    {
        // Arrange
        static async Task Task()
        {
            await System.Threading.Tasks.Task.Delay(100);
            throw new InvalidOperationException("Test exception");
        }

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => AsyncHelper.RunSync(Task));
        Assert.Equal("Test exception", exception.Message);
    }

    [Fact]
    public void RunSyncTaskCancellationThrowsTaskCanceledException()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.Cancel();

        async Task Task()
        {
            await System.Threading.Tasks.Task.Delay(100, cts.Token);
        }

        // Act & Assert
        var exception = Assert.Throws<TaskCanceledException>(() => AsyncHelper.RunSync(Task));
        Assert.Equal(cts.Token, exception.CancellationToken);
    }

    [Fact]
    public void RunSyncTaskWithResultCompletesSuccessfully()
    {
        // Arrange
        static async Task<string> Task()
        {
            await System.Threading.Tasks.Task.Delay(100);
            return "Completed";
        }

        // Act
        var result = AsyncHelper.RunSync(Task);

        // Assert
        Assert.Equal("Completed", result);
    }

    [Fact]
    public void RunSyncTaskWithResultThrowsException()
    {
        // Arrange
        static async Task<string> Task()
        {
            await System.Threading.Tasks.Task.Delay(100);
            throw new InvalidOperationException("Test exception");
        }

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => AsyncHelper.RunSync(Task));
        Assert.Equal("Test exception", exception.Message);
    }

    [Fact]
    public void RunSyncTaskWithResultCancellationThrowsTaskCanceledException()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.Cancel();

        async Task<string> Task()
        {
            await System.Threading.Tasks.Task.Delay(100, cts.Token);
            return "Completed";
        }

        // Act & Assert
        var exception = Assert.Throws<TaskCanceledException>(() => AsyncHelper.RunSync(Task));
        Assert.Equal(cts.Token, exception.CancellationToken);
    }
}
