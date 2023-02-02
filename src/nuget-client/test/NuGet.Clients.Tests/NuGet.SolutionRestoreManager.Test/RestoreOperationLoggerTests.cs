// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Xunit;

namespace NuGet.SolutionRestoreManager.Test
{
    [Collection(MockedVS.Collection)]
    public class RestoreOperationLoggerTests
    {
        public RestoreOperationLoggerTests(GlobalServiceProvider sp)
        {
            sp.Reset();
        }

        [Fact]
        public async Task StatusBarProgress_StartAsync_CancellationTokenThrowsAsync()
        {
            // Prepare
            var token = new CancellationToken(canceled: true);

            var task = RestoreOperationLogger.StatusBarProgress.StartAsync(
                AsyncServiceProvider.GlobalProvider,
                ThreadHelper.JoinableTaskFactory,
                token);

            // Act and Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        }
    }
}
