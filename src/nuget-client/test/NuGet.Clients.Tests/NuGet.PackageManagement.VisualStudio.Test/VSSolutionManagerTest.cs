// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Threading;
using Moq;
using NuGet.Configuration;
using NuGet.VisualStudio;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    [Collection(MockedVS.Collection)]
    public class VSSolutionManagerTest : MockedVSCollectionTests
    {
        private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(10);

        public VSSolutionManagerTest(GlobalServiceProvider globalServiceProvider)
            : base(globalServiceProvider)
        {
        }

        [Fact]
        public async Task CreateCredentialService_WhenBackgroundInitializationNeedsMainThread_DoesNotDeadlock()
        {
            JoinableTaskFactory joinableTaskFactory = NuGetUIThreadHelper.JoinableTaskFactory;
            var factoryStarted = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var continueFactory = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var credentialService = Mock.Of<ICredentialService>();
            var factoryCallCount = 0;

            using var cancellationTokenSource = new CancellationTokenSource(TestTimeout);
            using CancellationTokenRegistration cancellationRegistration = cancellationTokenSource.Token.Register(
                () => continueFactory.TrySetCanceled(cancellationTokenSource.Token));

            Lazy<ICredentialService> lazyCredentialService = VSSolutionManager.CreateCredentialService(
                async () =>
                {
                    Interlocked.Increment(ref factoryCallCount);
                    factoryStarted.TrySetResult(true);
                    await continueFactory.Task;
                    await joinableTaskFactory.SwitchToMainThreadAsync(cancellationTokenSource.Token);
                    return credentialService;
                },
                joinableTaskFactory);

            Task<ICredentialService> backgroundCaller = Task.Run(() => lazyCredentialService.Value);

            Task factoryStart = await Task.WhenAny(factoryStarted.Task, Task.Delay(TestTimeout));
            Assert.Same(factoryStarted.Task, factoryStart);

            JoinableTask<ICredentialService> mainThreadCaller = joinableTaskFactory.RunAsync(async () =>
            {
                await joinableTaskFactory.SwitchToMainThreadAsync(cancellationTokenSource.Token);
                continueFactory.TrySetResult(true);
                return lazyCredentialService.Value;
            });

            Task<ICredentialService[]> callers = Task.WhenAll(backgroundCaller, mainThreadCaller.Task);
            Task completedTask = await Task.WhenAny(callers, Task.Delay(TestTimeout + TestTimeout));
            Assert.Same(callers, completedTask);

            try
            {
                ICredentialService[] results = await callers;

                Assert.False(cancellationTokenSource.IsCancellationRequested, "Credential service initialization timed out.");
                Assert.Same(credentialService, results[0]);
                Assert.Same(credentialService, results[1]);
                Assert.Equal(1, factoryCallCount);
            }
            catch (OperationCanceledException) when (cancellationTokenSource.IsCancellationRequested)
            {
                Assert.Fail("The main thread deadlocked waiting for credential service initialization.");
            }
        }
    }
}
