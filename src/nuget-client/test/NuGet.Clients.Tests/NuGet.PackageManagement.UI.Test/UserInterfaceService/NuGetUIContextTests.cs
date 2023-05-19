// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using Moq;
using NuGet.Configuration;
using NuGet.PackageManagement.UI.Utility;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.UserInterfaceService
{
    public class NuGetUIContextTests : IDisposable
    {
        private static readonly PackageIdentity PackageIdentity = new PackageIdentity(id: "x", NuGetVersion.Parse("1.0.0"));

        private readonly Mock<IVsSolutionManager> _solutionManager = new Mock<IVsSolutionManager>();
        private readonly TestDirectory _testDirectory = TestDirectory.Create();

        [Fact]
        public void ProjectActionsExecuted_WhenSolutionManagerActionsExecutedEventRaisedWithNullEventArgs_Throws()
        {
            NuGetUIContext context = CreateNuGetUIContext();
            var wasEventRaised = false;

            context.ProjectActionsExecuted += (object sender, IReadOnlyCollection<string> actualProjectIds) =>
            {
                wasEventRaised = true;
            };

            Assert.ThrowsAny<Exception>(() => _solutionManager.Raise(s => s.ActionsExecuted += null, (EventArgs)null));

            Assert.False(wasEventRaised);
        }

        [Fact]
        public void ProjectActionsExecuted_WhenSolutionManagerActionsExecutedEventRaisedWithNoActions_IsNotRaised()
        {
            NuGetUIContext context = CreateNuGetUIContext();
            var resolvedActions = Enumerable.Empty<ResolvedAction>();
            var wasEventRaised = false;

            context.ProjectActionsExecuted += (object sender, IReadOnlyCollection<string> actualProjectIds) =>
            {
                wasEventRaised = true;
            };

            _solutionManager.Raise(s => s.ActionsExecuted += null, new ActionsExecutedEventArgs(resolvedActions));

            Assert.False(wasEventRaised);
        }

        [Fact]
        public void ProjectActionsExecuted_WhenSolutionManagerActionsExecutedEventRaised_DistinctIdsReturned()
        {
            NuGetUIContext context = CreateNuGetUIContext();
            var projectA = new TestNuGetProject();
            var projectB = new TestNuGetProject();

            var projectActionA1 = NuGetProjectAction.CreateInstallProjectAction(
                PackageIdentity,
                sourceRepository: null,
                projectA);
            var projectActionA2 = NuGetProjectAction.CreateInstallProjectAction(
                PackageIdentity,
                sourceRepository: null,
                projectA);
            var projectActionB = NuGetProjectAction.CreateInstallProjectAction(
                PackageIdentity,
                sourceRepository: null,
                projectB);
            var resolvedActions = new ResolvedAction[]
            {
                new ResolvedAction(projectA, projectActionA1),
                new ResolvedAction(projectA, projectActionA2),
                new ResolvedAction(projectB, projectActionB),
            };
            var wasEventRaised = false;
            string[] expectedProjectIds = resolvedActions
                .Select(resolvedAction => resolvedAction.Project.GetMetadata<string>(NuGetProjectMetadataKeys.ProjectId))
                .Distinct()
                .ToArray();

            context.ProjectActionsExecuted += (object sender, IReadOnlyCollection<string> actualProjectIds) =>
            {
                wasEventRaised = true;

                Assert.Equal(expectedProjectIds, actualProjectIds);
            };

            _solutionManager.Raise(s => s.ActionsExecuted += null, new ActionsExecutedEventArgs(resolvedActions));

            Assert.True(wasEventRaised);
        }

        [Fact]
        public void ProjectActionsExecuted_WhenSolutionManagerActionsExecutedEventRaised_IsRaised()
        {
            NuGetUIContext context = CreateNuGetUIContext();
            var project = new TestNuGetProject();

            var projectAction = NuGetProjectAction.CreateInstallProjectAction(
                PackageIdentity,
                sourceRepository: null,
                project);
            var resolvedActions = new ResolvedAction[] { new ResolvedAction(project, projectAction) };
            var wasEventRaised = false;
            string[] expectedProjectIds = resolvedActions
                .Select(resolvedAction => resolvedAction.Project.GetMetadata<string>(NuGetProjectMetadataKeys.ProjectId))
                .ToArray();

            context.ProjectActionsExecuted += (object sender, IReadOnlyCollection<string> actualProjectIds) =>
            {
                wasEventRaised = true;

                Assert.Equal(expectedProjectIds, actualProjectIds);
            };

            _solutionManager.Raise(s => s.ActionsExecuted += null, new ActionsExecutedEventArgs(resolvedActions));

            Assert.True(wasEventRaised);
        }

        [Fact]
        public void PackageSourceMapping_OnSettingsChanged_ReferencesNewObject()
        {
            // Arrange
            var settings = new Mock<ISettings>();
            NuGetUIContext nuGetUIContext = CreateNuGetUIContext(settings.Object);
            PackageSourceMapping targetBefore = nuGetUIContext.PackageSourceMapping;

            // Act
            settings.Raise(s => s.SettingsChanged += null, (EventArgs)null);

            PackageSourceMapping targetAfter = nuGetUIContext.PackageSourceMapping;

            // Assert
            Assert.NotEqual(targetBefore, targetAfter);
        }

        public void Dispose()
        {
            _testDirectory.Dispose();
        }

        private NuGetUIContext CreateNuGetUIContext(ISettings settings = null)
        {
            settings = settings ?? Mock.Of<ISettings>();
            var sourceRepositoryProvider = Mock.Of<ISourceRepositoryProvider>();
            var packageManager = new NuGetPackageManager(
                sourceRepositoryProvider,
                Mock.Of<ISettings>(),
                _testDirectory.Path);

            return new NuGetUIContext(
                Mock.Of<IServiceBroker>(),
                Mock.Of<IReconnectingNuGetSearchService>(),
                _solutionManager.Object,
                new NuGetSolutionManagerServiceWrapper(),
                packageManager,
                new UIActionEngine(
                    sourceRepositoryProvider,
                    packageManager,
                    Mock.Of<INuGetLockService>()),
                Mock.Of<IPackageRestoreManager>(),
                Mock.Of<IOptionsPageActivator>(),
                Mock.Of<IUserSettingsManager>(),
                new NuGetSourcesServiceWrapper(),
                settings);
        }

        private sealed class TestNuGetProject : NuGetProject
        {
            internal TestNuGetProject()
            {
                InternalMetadata[NuGetProjectMetadataKeys.ProjectId] = Guid.NewGuid().ToString();
            }

            public override Task<IEnumerable<PackageReference>> GetInstalledPackagesAsync(
                CancellationToken token)
            {
                throw new NotImplementedException();
            }

            public override Task<bool> InstallPackageAsync(
                PackageIdentity packageIdentity,
                DownloadResourceResult downloadResourceResult,
                INuGetProjectContext nuGetProjectContext,
                CancellationToken token)
            {
                throw new NotImplementedException();
            }

            public override Task<bool> UninstallPackageAsync(
                PackageIdentity packageIdentity,
                INuGetProjectContext nuGetProjectContext,
                CancellationToken token)
            {
                throw new NotImplementedException();
            }
        }
    }
}
