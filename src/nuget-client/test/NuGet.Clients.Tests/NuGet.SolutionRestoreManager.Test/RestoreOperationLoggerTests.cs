// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Xunit;

namespace NuGet.SolutionRestoreManager.Test
{
    [Collection(MockedVS.Collection)]
    public class RestoreOperationLoggerTests
    {
        [Fact]
        public async Task StatusBarProgress_StartAsync_CancellationTokenThrowsAsync()
        {
            // Prepare
            var cts = new CancellationTokenSource();

            var task = RestoreOperationLogger.StatusBarProgress.StartAsync(
                AsyncServiceProvider.GlobalProvider,
                ThreadHelper.JoinableTaskFactory,
                cts.Token);
            cts.Cancel();

            // Act and Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        }
    }
}
