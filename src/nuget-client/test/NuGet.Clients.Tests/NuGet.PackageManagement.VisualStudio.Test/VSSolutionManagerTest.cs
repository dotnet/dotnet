// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;
using Moq;
using NuGet.Configuration;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Services;
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
        public async Task GetNuGetProjectsAsync_WaitsForFeatureFlagInitialization_AndLogsUnexpectedFailure()
        {
            var initialization = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var featureFlagInitializer = new Mock<INuGetFeatureFlagInitializer>(MockBehavior.Strict);
            featureFlagInitializer.Setup(x => x.InitializeAsync()).Returns(initialization.Task);
            var projectSystemCache = new Mock<IProjectSystemCache>(MockBehavior.Strict);
            var logger = new Mock<Common.ILogger>();
            var projectFactory = new NuGetProjectFactory(
                Array.Empty<Lazy<INuGetProjectProvider, IOrderable>>(),
                Mock.Of<IVsProjectThreadingService>(),
                logger.Object);
            using var solutionManager = new VSSolutionManager(
                this,
                projectSystemCache.Object,
                projectFactory,
                Mock.Of<ICredentialServiceProvider>(),
                Mock.Of<IVsProjectAdapterProvider>(),
                logger.Object,
                new Lazy<ISettings>(() => Mock.Of<ISettings>()),
                featureFlagInitializer.Object,
                NuGetUIThreadHelper.JoinableTaskFactory.Context,
                new Lazy<IProjectJsonToPackageReferenceMigratorExt>(
                    () => Mock.Of<IProjectJsonToPackageReferenceMigratorExt>()),
                new Lazy<INuGetUILogger>(() => Mock.Of<INuGetUILogger>()));

            var projects = solutionManager.GetNuGetProjectsAsync();

            Assert.False(projects.IsCompleted);
            projectSystemCache.VerifyNoOtherCalls();
            projectSystemCache.Setup(x => x.GetNuGetProjects()).Returns(Array.Empty<NuGetProject>());

            var expected = new InvalidOperationException("Feature flag initialization failed.");
            // The existing catch calls Debug.Fail; suppress listeners for this intentional failure.
            TraceListener[] listeners = Trace.Listeners.Cast<TraceListener>().ToArray();
            try
            {
                Trace.Listeners.Clear();
                initialization.SetException(expected);
                Assert.Empty(await projects);
            }
            finally
            {
                Trace.Listeners.AddRange(listeners);
            }
            logger.Verify(x => x.LogError(expected.ToString()), Times.Once);
            projectSystemCache.Verify(x => x.GetNuGetProjects(), Times.Once);
            projectSystemCache.VerifyNoOtherCalls();
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
