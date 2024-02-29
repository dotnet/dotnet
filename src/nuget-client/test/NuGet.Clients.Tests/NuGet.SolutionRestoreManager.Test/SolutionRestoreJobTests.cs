// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;
using Test.Utility;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace NuGet.SolutionRestoreManager.Test
{
    [Collection(MockedVS.Collection)]
    public class SolutionRestoreJobTests
    {
        private GlobalServiceProvider _globalProvider;

        public SolutionRestoreJobTests(GlobalServiceProvider sp)
        {
            sp.Reset();
            _globalProvider = sp;
        }

        [Fact]
        public async Task Simple_ReportsNoOp_Async()
        {
            var restoreMan = Mock.Of<IPackageRestoreManager>();
            _globalProvider.AddService(typeof(IPackageRestoreManager), restoreMan);
            var slnMan = Mock.Of<IVsSolutionManager>();
            _globalProvider.AddService(typeof(IVsSolutionManager), slnMan);
            ISourceRepositoryProvider sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            _globalProvider.AddService(typeof(ISourceRepositoryProvider), sourceRepositoryProvider);

            var infoBar = Mock.Of<Lazy<IVulnerabilitiesNotificationService>>();
            var restoreChecker = Mock.Of<ISolutionRestoreChecker>();
            var eventsPublisher = Mock.Of<IRestoreEventsPublisher>();
            var settings = Mock.Of<ISettings>();
            var nuGetProgressReporter = Mock.Of<IVsNuGetProgressReporter>();
            var auditCheckResultCachingService = Mock.Of<IAuditCheckResultCachingService>();

            Mock.Get(settings)
                .Setup(x => x.GetSection("packageRestore"))
                .Returns(() => new VirtualSettingSection("packageRestore",
                    new AddItem("automatic", bool.TrueString)));

            var consoleProvider = Mock.Of<IOutputConsoleProvider>();
            var logger = new RestoreOperationLogger(new Lazy<IOutputConsoleProvider>(() => consoleProvider));

            var job = new SolutionRestoreJob(
                asyncServiceProvider: AsyncServiceProvider.GlobalProvider,
                packageRestoreManager: restoreMan,
                solutionManager: slnMan,
                sourceRepositoryProvider: sourceRepositoryProvider,
                restoreEventsPublisher: eventsPublisher,
                settings: settings,
                solutionRestoreChecker: restoreChecker,
                nuGetProgressReporter: nuGetProgressReporter,
                auditCheckResultCachingService);

            var restoreRequest = new SolutionRestoreRequest(
                forceRestore: true,
                RestoreOperationSource.OnBuild);
            var restoreJobContext = new SolutionRestoreJobContext();

            await job.ExecuteAsync(
                request: restoreRequest,
                jobContext: restoreJobContext,
                logger: logger,
                trackingData: new Dictionary<string, object>(),
                vulnerabilitiesFoundService: infoBar,
                token: CancellationToken.None);

            Assert.Equal(NuGetOperationStatus.NoOp, job.Status);
        }

        [Fact]
        public async Task Simple_WhenCancelled_Reports_Cancelled_Async()
        {
            var restoreMan = Mock.Of<IPackageRestoreManager>();
            _globalProvider.AddService(typeof(IPackageRestoreManager), restoreMan);
            var slnMan = Mock.Of<IVsSolutionManager>();
            _globalProvider.AddService(typeof(IVsSolutionManager), slnMan);
            ISourceRepositoryProvider sourceRepositoryProvider = TestSourceRepositoryUtility.CreateV3OnlySourceRepositoryProvider();
            _globalProvider.AddService(typeof(ISourceRepositoryProvider), sourceRepositoryProvider);

            var infoBar = Mock.Of<Lazy<IVulnerabilitiesNotificationService>>();
            var restoreChecker = Mock.Of<ISolutionRestoreChecker>();
            var eventsPublisher = Mock.Of<IRestoreEventsPublisher>();
            var settings = Mock.Of<ISettings>();
            var nuGetProgressReporter = Mock.Of<IVsNuGetProgressReporter>();
            var auditCheckResultCachingService = Mock.Of<IAuditCheckResultCachingService>();

            Mock.Get(settings)
                .Setup(x => x.GetSection("packageRestore"))
                .Returns(() => new VirtualSettingSection("packageRestore",
                    new AddItem("automatic", bool.TrueString)));

            var consoleProvider = Mock.Of<IOutputConsoleProvider>();
            var logger = new RestoreOperationLogger(new Lazy<IOutputConsoleProvider>(() => consoleProvider));

            var job = new SolutionRestoreJob(
                asyncServiceProvider: AsyncServiceProvider.GlobalProvider,
                packageRestoreManager: restoreMan,
                solutionManager: slnMan,
                sourceRepositoryProvider: sourceRepositoryProvider,
                restoreEventsPublisher: eventsPublisher,
                settings: settings,
                solutionRestoreChecker: restoreChecker,
                nuGetProgressReporter: nuGetProgressReporter,
                auditCheckResultCachingService);

            var restoreRequest = new SolutionRestoreRequest(
                forceRestore: true,
                RestoreOperationSource.OnBuild);
            var restoreJobContext = new SolutionRestoreJobContext();

            var cts = new CancellationTokenSource();
            cts.Cancel();

            await job.ExecuteAsync(
                request: restoreRequest,
                jobContext: restoreJobContext,
                logger: logger,
                trackingData: new Dictionary<string, object>(),
                vulnerabilitiesFoundService: infoBar,
                token: cts.Token);

            Assert.Equal(NuGetOperationStatus.Cancelled, job.Status);
        }
    }
}
