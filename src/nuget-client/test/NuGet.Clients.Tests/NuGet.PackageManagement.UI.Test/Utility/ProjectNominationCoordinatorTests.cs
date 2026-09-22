// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.PackageManagement.UI.Utility;
using NuGet.PackageManagement.VisualStudio;
using NuGet.SolutionRestoreManager;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Utility
{
    public class ProjectNominationCoordinatorTests
    {
        [Fact]
        public void Constructor_WithNullSolutionManager_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new ProjectNominationCoordinator(solutionManager: null!));
        }

        [Fact]
        public async Task WaitForNominationsToSettleAsync_WhenNoProjects_CompletesWithoutWaiting()
        {
            var solutionManager = CreateSolutionManager();
            var coordinator = new ProjectNominationCoordinator(solutionManager);

            TimeSpan? nominationWaitDuration = await coordinator.WaitForNominationsToSettleAsync(projectFullPath: null, CancellationToken.None);

            Assert.Null(nominationWaitDuration);
        }

        [Fact]
        public async Task WaitForNominationsToSettleAsync_WhenNoPendingNominations_CompletesWithoutWaiting()
        {
            var source = new FakeRestoreInfoSource("a") { HasPendingNominationFunc = () => false };
            var solutionManager = CreateSolutionManager(source);
            var coordinator = new ProjectNominationCoordinator(solutionManager);

            TimeSpan? nominationWaitDuration = await coordinator.WaitForNominationsToSettleAsync(projectFullPath: null, CancellationToken.None);

            Assert.Equal(0, source.WhenNominatedCallCount);
            Assert.Null(nominationWaitDuration);
        }

        [Fact]
        public async Task WaitForNominationsToSettleAsync_WhenPendingThenNominated_WaitsThenCompletes()
        {
            int reads = 0;
            var nomination = new TaskCompletionSource<bool>();
            var source = new FakeRestoreInfoSource("a")
            {
                HasPendingNominationFunc = () => reads++ == 0,
                NominationTask = nomination.Task,
            };
            var solutionManager = CreateSolutionManager(source);
            var coordinator = new ProjectNominationCoordinator(solutionManager);

            Task<TimeSpan?> wait = coordinator.WaitForNominationsToSettleAsync(projectFullPath: null, CancellationToken.None);
            Assert.False(wait.IsCompleted);

            nomination.SetResult(true);
            TimeSpan? nominationWaitDuration = await wait;

            Assert.Equal(1, source.WhenNominatedCallCount);
            Assert.NotNull(nominationWaitDuration);
        }

        [Fact]
        public async Task WaitForNominationsToSettleAsync_WhenPendingFlagLagsCompletedNomination_StopsWithoutBusySpinning()
        {
            var source = new FakeRestoreInfoSource("a")
            {
                HasPendingNominationFunc = () => true,
                NominationTask = Task.CompletedTask,
            };
            var solutionManager = CreateSolutionManager(source);
            var coordinator = new ProjectNominationCoordinator(solutionManager);

            TimeSpan? nominationWaitDuration = await coordinator.WaitForNominationsToSettleAsync(projectFullPath: null, CancellationToken.None);

            Assert.Equal(1, source.WhenNominatedCallCount);
            Assert.Null(nominationWaitDuration);
        }

        [Fact]
        public async Task WaitForNominationsToSettleAsync_WhenNominationNeverCompletes_ReturnsAfterTimeoutBudget()
        {
            var neverCompletes = new TaskCompletionSource<bool>();
            var source = new FakeRestoreInfoSource("a")
            {
                HasPendingNominationFunc = () => true,
                NominationTask = neverCompletes.Task,
            };
            var solutionManager = CreateSolutionManager(source);
            var coordinator = new ProjectNominationCoordinator(solutionManager, nominationSettleTimeout: TimeSpan.FromMilliseconds(50));

            TimeSpan? nominationWaitDuration = await coordinator.WaitForNominationsToSettleAsync(projectFullPath: null, CancellationToken.None);

            Assert.Equal(1, source.WhenNominatedCallCount);
            Assert.NotNull(nominationWaitDuration);
            Assert.True(nominationWaitDuration > TimeSpan.Zero);
        }

        [Fact]
        public async Task WaitForNominationsToSettleAsync_WithMultiplePendingProjects_WaitsSequentially()
        {
            var nominationA = new TaskCompletionSource<bool>();
            var nominationB = new TaskCompletionSource<bool>();
            int readsA = 0, readsB = 0;
            var sourceA = new FakeRestoreInfoSource("a") { HasPendingNominationFunc = () => readsA++ == 0, NominationTask = nominationA.Task };
            var sourceB = new FakeRestoreInfoSource("b") { HasPendingNominationFunc = () => readsB++ == 0, NominationTask = nominationB.Task };
            var solutionManager = CreateSolutionManager(sourceA, sourceB);
            var coordinator = new ProjectNominationCoordinator(solutionManager);

            Task wait = coordinator.WaitForNominationsToSettleAsync(projectFullPath: null, CancellationToken.None);

            Assert.Equal(1, sourceA.WhenNominatedCallCount);
            Assert.Equal(0, sourceB.WhenNominatedCallCount);

            nominationA.SetResult(true);

            await WaitForAsync(() => sourceB.WhenNominatedCallCount == 1);

            nominationB.SetResult(true);
            await wait;

            Assert.Equal(1, sourceA.WhenNominatedCallCount);
            Assert.Equal(1, sourceB.WhenNominatedCallCount);
        }

        [Fact]
        public async Task WaitForNominationsToSettleAsync_WhenNominationStartsDuringWait_RescansAndWaitsAgain()
        {
            var firstNomination = new TaskCompletionSource<bool>();
            var secondNomination = new TaskCompletionSource<bool>();
            int calls = 0;
            int reads = 0;
            var source = new FakeRestoreInfoSource("a")
            {
                HasPendingNominationFunc = () => reads++ < 2,
                NominationTaskFactory = () => ++calls == 1 ? firstNomination.Task : secondNomination.Task,
            };
            var solutionManager = CreateSolutionManager(source);
            var coordinator = new ProjectNominationCoordinator(solutionManager);

            Task wait = coordinator.WaitForNominationsToSettleAsync(projectFullPath: null, CancellationToken.None);

            firstNomination.SetResult(true);
            Assert.False(wait.IsCompleted);

            secondNomination.SetResult(true);
            await wait;

            Assert.Equal(2, source.WhenNominatedCallCount);
        }

        [Fact]
        public async Task WaitForNominationsToSettleAsync_WhenSourceWithdrawsNomination_TreatsAsSettled()
        {
            int reads = 0;
            var source = new FakeRestoreInfoSource("a")
            {
                HasPendingNominationFunc = () => reads++ == 0,
                NominationTask = Task.FromCanceled(new CancellationToken(canceled: true)),
            };
            var solutionManager = CreateSolutionManager(source);
            var coordinator = new ProjectNominationCoordinator(solutionManager);

            await coordinator.WaitForNominationsToSettleAsync(projectFullPath: null, CancellationToken.None);
        }

        [Fact]
        public async Task WaitForNominationsToSettleAsync_WhenNominationFaults_ObservesFaultAndSettles()
        {
            int reads = 0;
            var source = new FakeRestoreInfoSource("a")
            {
                HasPendingNominationFunc = () => reads++ == 0,
                NominationTask = Task.FromException(new IOException("design-time build failed")),
            };
            var solutionManager = CreateSolutionManager(source);
            var coordinator = new ProjectNominationCoordinator(solutionManager);

            await coordinator.WaitForNominationsToSettleAsync(projectFullPath: null, CancellationToken.None);
        }

        [Fact]
        public async Task WaitForNominationsToSettleAsync_WhenCancelledBeforeStart_Throws()
        {
            var solutionManager = CreateSolutionManager();
            var coordinator = new ProjectNominationCoordinator(solutionManager);
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(
                () => coordinator.WaitForNominationsToSettleAsync(projectFullPath: null, cts.Token));
        }

        [Fact]
        public async Task WaitForNominationsToSettleAsync_WhenCancelledWhileWaiting_Throws()
        {
            var neverCompletes = new TaskCompletionSource<bool>();
            var source = new FakeRestoreInfoSource("a")
            {
                HasPendingNominationFunc = () => true,
                NominationTask = neverCompletes.Task,
            };
            var solutionManager = CreateSolutionManager(source);
            var coordinator = new ProjectNominationCoordinator(solutionManager);
            using var cts = new CancellationTokenSource();

            Task wait = coordinator.WaitForNominationsToSettleAsync(projectFullPath: null, cts.Token);
            Assert.False(wait.IsCompleted);

            cts.Cancel();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => wait);
        }

        [Fact]
        public async Task WaitForNominationsToSettleAsync_WhenScopedToProject_IgnoresOtherProjectsPendingNominations()
        {
            var neverCompletes = new TaskCompletionSource<bool>();
            var sourceA = new FakeRestoreInfoSource("a") { HasPendingNominationFunc = () => false };
            var sourceB = new FakeRestoreInfoSource("b") { HasPendingNominationFunc = () => true, NominationTask = neverCompletes.Task };
            var solutionManager = CreateSolutionManager(sourceA, sourceB);
            var coordinator = new ProjectNominationCoordinator(solutionManager);

            await coordinator.WaitForNominationsToSettleAsync(projectFullPath: "a", CancellationToken.None);

            Assert.Equal(0, sourceB.WhenNominatedCallCount);
        }

        [Fact]
        public async Task WaitForNominationsToSettleAsync_WhenScopedToProject_WaitsOnlyForThatProject()
        {
            var nominationA = new TaskCompletionSource<bool>();
            int readsA = 0;
            var sourceA = new FakeRestoreInfoSource("a") { HasPendingNominationFunc = () => readsA++ == 0, NominationTask = nominationA.Task };
            var sourceB = new FakeRestoreInfoSource("b") { HasPendingNominationFunc = () => true, NominationTask = new TaskCompletionSource<bool>().Task };
            var solutionManager = CreateSolutionManager(sourceA, sourceB);
            var coordinator = new ProjectNominationCoordinator(solutionManager);

            Task wait = coordinator.WaitForNominationsToSettleAsync(projectFullPath: "a", CancellationToken.None);
            Assert.False(wait.IsCompleted);

            nominationA.SetResult(true);
            await wait;

            Assert.Equal(1, sourceA.WhenNominatedCallCount);
            Assert.Equal(0, sourceB.WhenNominatedCallCount);
        }

        [Fact]
        public async Task WaitForNominationsToSettleAsync_WhenScopedToProject_MatchesNameCaseInsensitively()
        {
            int reads = 0;
            var source = new FakeRestoreInfoSource(@"C:\Repo\Foo\Foo.csproj")
            {
                HasPendingNominationFunc = () => reads++ == 0,
                NominationTask = Task.CompletedTask,
            };
            var solutionManager = CreateSolutionManager(source);
            var coordinator = new ProjectNominationCoordinator(solutionManager);

            await coordinator.WaitForNominationsToSettleAsync(projectFullPath: @"c:\repo\foo\foo.csproj", CancellationToken.None);

            Assert.Equal(1, source.WhenNominatedCallCount);
        }

        private static async Task WaitForAsync(Func<bool> condition)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            while (!condition())
            {
                await Task.Delay(10, cts.Token);
            }
        }

        private static IVsSolutionManager CreateSolutionManager(params object[] sources)
        {
            var solutionManager = new Mock<IVsSolutionManager>();
            solutionManager.Setup(x => x.GetAllProjectRestoreInfoSources()).Returns(sources);
            return solutionManager.Object;
        }

        private sealed class FakeRestoreInfoSource : IVsProjectRestoreInfoSource
        {
            public FakeRestoreInfoSource(string name)
            {
                Name = name;
            }

            public string Name { get; }

            public Func<bool> HasPendingNominationFunc { get; set; } = () => false;

            public Task NominationTask { get; set; } = Task.CompletedTask;

            public Func<Task>? NominationTaskFactory { get; set; }

            public int WhenNominatedCallCount { get; private set; }

            public bool HasPendingNomination => HasPendingNominationFunc();

            public Task WhenNominated(CancellationToken cancellationToken)
            {
                WhenNominatedCallCount++;

                Task source = NominationTaskFactory != null ? NominationTaskFactory() : NominationTask;
                if (source.IsCompleted)
                {
                    return source;
                }

                var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
                CancellationTokenRegistration registration = cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken));
                _ = source.ContinueWith(
                    t =>
                    {
                        registration.Dispose();
                        if (t.IsCanceled)
                        {
                            tcs.TrySetCanceled();
                        }
                        else if (t.IsFaulted)
                        {
                            tcs.TrySetException(t.Exception!.InnerExceptions);
                        }
                        else
                        {
                            tcs.TrySetResult(true);
                        }
                    },
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default);
                return tcs.Task;
            }
        }
    }
}
