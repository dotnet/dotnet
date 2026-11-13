// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation.Runspaces;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Moq;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common.Telemetry.PowerShell;
using NuGetConsole.Host.PowerShell.Implementation;
using Xunit;

namespace NuGetConsole.Host.PowerShell.Test
{
    [Collection(MockedVS.Collection)]
    public class PowerShellHostTests : IAsyncServiceProvider
    {
        private readonly GlobalServiceProvider _globalServiceProvider;

        public PowerShellHostTests(GlobalServiceProvider globalServiceProvider)
        {
            _globalServiceProvider = globalServiceProvider;
            _globalServiceProvider.Reset();
            ServiceLocator.InitializePackageServiceProvider(this);
        }

        [Theory]
        [InlineData(true, false, 1, false)]
        [InlineData(true, true, 0, false)]
        [InlineData(false, false, 0, false)]
        [InlineData(true, false, 2, true)]
        public async Task Initialize_RaisesEventOnlyForUnvisitedInitScriptAsync(
            bool hasInitScript, bool alreadyVisited, int expectedCount, bool changeSolutionBetweenScripts)
        {
            using var pathContext = new SimpleTestPathContext();
            var package = new SimpleTestPackageContext("TestPackage", "1.0.0");
            if (hasInitScript)
            {
                package.AddFile("tools/init.ps1", "$global:InitScriptExecuted = $true");
            }

            var packages = new List<SimpleTestPackageContext> { package };
            if (changeSolutionBetweenScripts)
            {
                var nextPackage = new SimpleTestPackageContext("NextPackage", "1.0.0");
                nextPackage.AddFile("tools/init.ps1", "$global:NextInitScriptExecuted = $true");
                packages.Add(nextPackage);
            }

            await SimpleTestPackageUtility.CreateFolderFeedPackagesConfigAsync(pathContext.PackagesV2, packages.ToArray());
            var identity = new PackageIdentity(package.Id, NuGetVersion.Parse(package.Version));
            var installPath = Path.Combine(pathContext.PackagesV2, "TestPackage.1.0.0");
            ISettings settings = Settings.LoadSpecificSettings(
                Path.GetDirectoryName(pathContext.NuGetConfig), Path.GetFileName(pathContext.NuGetConfig));
            settings.AddOrUpdate("config", new AddItem("repositoryPath", pathContext.PackagesV2));

            var project = new Mock<NuGetProject>(new Dictionary<string, object>
            {
                { NuGetProjectMetadataKeys.Name, "TestProject" }
            });
            var installedPackages = new List<PackageReference> { new PackageReference(identity, NuGetFramework.AnyFramework) };
            if (changeSolutionBetweenScripts)
            {
                installedPackages.Add(new PackageReference(
                    new PackageIdentity("NextPackage", NuGetVersion.Parse("1.0.0")), NuGetFramework.AnyFramework));
            }

            project.Setup(p => p.GetInstalledPackagesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(installedPackages);
            var solutionManager = new Mock<IVsSolutionManager>();
            solutionManager.Setup(s => s.IsSolutionOpenAsync()).ReturnsAsync(true);
            solutionManager.Setup(s => s.GetSolutionDirectoryAsync()).ReturnsAsync(pathContext.SolutionRoot);
            solutionManager.SetupGet(s => s.SolutionDirectory).Returns(pathContext.SolutionRoot);
            solutionManager.Setup(s => s.GetNuGetProjectsAsync()).ReturnsAsync(new[] { project.Object });
            solutionManager.Setup(s => s.GetDefaultNuGetProjectAsync()).ReturnsAsync((NuGetProject)null!);
            var scriptExecutor = new Mock<IScriptExecutor>();
            var visitedCount = 0;
            scriptExecutor.Setup(s => s.TryMarkVisited(
                It.IsAny<PackageIdentity>(),
                PackageInitPS1State.FoundAndExecuted))
                .Callback(() =>
                {
                    if (changeSolutionBetweenScripts && Interlocked.Increment(ref visitedCount) == 2)
                    {
                        NuGetPowerShellUsage.RaiseSolutionCloseEvent();
                        NuGetPowerShellUsage.RaiseSolutionOpenEvent();
                    }
                })
                .Returns(!alreadyVisited);
            var componentModel = new Mock<IComponentModel>();
            componentModel.Setup(c => c.GetService<ISettings>()).Returns(settings);
            componentModel.Setup(c => c.GetService<IVsSolutionManager>()).Returns(solutionManager.Object);
            componentModel.Setup(c => c.GetService<IScriptExecutor>()).Returns(scriptExecutor.Object);
            componentModel.Setup(c => c.GetService<ISourceRepositoryProvider>())
                .Returns(new SourceRepositoryProvider(
                    new PackageSourceProvider(settings), Repository.Provider.GetCoreV3()));
            componentModel.Setup(c => c.GetService<IDeleteOnRestartManager>())
                .Returns(Mock.Of<IDeleteOnRestartManager>());
            componentModel.Setup(c => c.GetService<IRestoreProgressReporter>())
                .Returns(Mock.Of<IRestoreProgressReporter>());
            _globalServiceProvider.AddService(typeof(SComponentModel), componentModel.Object);

            // Include the tools directory so initialization does not change the process PATH.
            var environment = new Mock<IEnvironmentVariableReader>();
            environment.Setup(e => e.GetEnvironmentVariable("path")).Returns(
                Path.Combine(installPath, "tools") + Path.PathSeparator +
                Path.Combine(pathContext.PackagesV2, "NextPackage.1.0.0", "tools"));
            using var runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
            var runspaceManager = new Mock<IRunspaceManager>();
            runspaceManager.Setup(r => r.GetRunspace(It.IsAny<IConsole>(), It.IsAny<string>()))
                .Returns(Tuple.Create(new RunspaceDispatcher(runspace), new NuGetPSHost("TestHost")));
            var hostMock = new Mock<PowerShellHost>(
                "TestHost", Mock.Of<IRestoreEvents>(), runspaceManager.Object, environment.Object)
            {
                CallBase = true
            };
            using var host = hostMock.Object;
            var console = Mock.Of<IConsole>();

            var executionSessions = new List<long>();
            void OnInitScriptExecuted(long solutionSessionId) => executionSessions.Add(solutionSessionId);
            NuGetPowerShellUsage.InitPs1ExecuteEvent += OnInitScriptExecuted;
            NuGetPowerShellUsage.RaiseSolutionOpenEvent();
            var originalSessionId = NuGetPowerShellUsage.SolutionSessionId;
            try
            {
                await Task.Run(() => host.Initialize(console));

                Assert.True(host.IsInitializedSuccessfully);
                Assert.Equal(expectedCount, executionSessions.Count);
                Assert.All(executionSessions, id => Assert.Equal(originalSessionId, id));
                Assert.Equal(expectedCount > 0, runspace.SessionStateProxy.GetVariable("InitScriptExecuted") is true);
                if (changeSolutionBetweenScripts)
                {
                    Assert.NotEqual(originalSessionId, NuGetPowerShellUsage.SolutionSessionId);
                    Assert.True(runspace.SessionStateProxy.GetVariable("NextInitScriptExecuted") is true);
                }

                scriptExecutor.Verify(s => s.TryMarkVisited(
                    It.Is<PackageIdentity>(p => PackageIdentityComparer.Default.Equals(p, identity)),
                    PackageInitPS1State.FoundAndExecuted),
                    Times.Exactly(hasInitScript ? 1 : 0));
            }
            finally
            {
                NuGetPowerShellUsage.InitPs1ExecuteEvent -= OnInitScriptExecuted;
                NuGetPowerShellUsage.RaiseSolutionCloseEvent();
            }
        }

        public Task<object?> GetServiceAsync(Type serviceType) => Task.FromResult<object?>(null);
    }
}
